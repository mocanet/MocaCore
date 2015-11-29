Imports System.Reflection

Imports Moca.Aop
Imports Moca.Attr
Imports Moca.Db.Attr
Imports Moca.Di
Imports Moca.Util

Namespace Db

	''' <summary>
	''' データベースから取得したデータの格納先となる Entity を作成する
	''' </summary>
	''' <remarks>
	''' 一度解析したEntity情報は内部で保存します。<br/>
	''' 2回目以降は解析せず保存した情報を元にします。
	''' </remarks>
	Public Class EntityBuilder

		''' <summary>一度解析したEntityを格納しておく</summary>
		Private _entityMap As New Hashtable

		''' <summary>
		''' DataTableのカラム構成を構築する
		''' </summary>
		''' <typeparam name="T"></typeparam>
		''' <typeparam name="Order"></typeparam>
		''' <param name="captions"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function CreateTable(Of T, Order)(Optional ByVal captions() As String = Nothing) As DataTable
			Dim ds As New DataSet
			Dim dt As New DataTable
			Dim typ As Type
			Dim keys As Hashtable

			typ = GetType(T)

			ds.Tables.Add(dt)

			keys = _getEntityInfo(typ)
			For Each key As String In [Enum].GetNames(GetType(Order))
				Dim prop As PropertyInfo
				prop = ClassUtil.GetProperties(typ, DirectCast(keys(key), String))

				Dim col As DataColumn
				col = New DataColumn(key, prop.PropertyType)
				dt.Columns.Add(col)
				If captions Is Nothing Then
					Continue For
				End If

				col.Caption = captions(CInt([Enum].Parse(GetType(Order), key)))
			Next

			Return dt
		End Function

		''' <summary>
		''' 引数の DataTable から指定されたタイプのデータ配列へ変換して返す。
		''' </summary>
		''' <typeparam name="T">変換先のタイプ</typeparam>
		''' <param name="tbl">変換元テーブルデータ</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Create(Of T)(ByVal tbl As DataTable) As T()
			Dim ary As ArrayList
			Dim typ As Type
			Dim entity As T
			Dim keys As Hashtable
			Dim view As DataView

			typ = GetType(T)
			keys = _getEntityInfo(typ)
			ary = New ArrayList

			view = tbl.DefaultView

			For ii As Integer = 0 To view.Count - 1
				entity = _create(Of T)(keys, view.Item(ii).Row)
				ary.Add(entity)
			Next

			Return DirectCast(ary.ToArray(typ), T())
		End Function

		''' <summary>
		''' 引数の DataRow から指定されたタイプのデータ配列へ変換して返す。
		''' </summary>
		''' <typeparam name="T">変換先のタイプ</typeparam>
		''' <param name="row">変換元行データ</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Create(Of T)(ByVal row As DataRow) As T
			Dim typ As Type
			Dim entity As T
			Dim keys As Hashtable

			typ = GetType(T)
			keys = _getEntityInfo(typ)

			entity = _create(Of T)(keys, row, Nothing)

			Return entity
		End Function

		''' <summary>
		''' 
		''' </summary>
		''' <typeparam name="T"></typeparam>
		''' <param name="row"></param>
		''' <param name="version">変換するデータのバージョン</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Create(Of T)(ByVal row As DataRow, ByVal version As DataRowVersion) As T
			Dim typ As Type
			Dim entity As T
			Dim keys As Hashtable

			typ = GetType(T)
			keys = _getEntityInfo(typ)

			entity = _create(Of T)(keys, row, version)

			Return entity
		End Function

		''' <summary>
		''' 
		''' </summary>
		''' <typeparam name="T"></typeparam>
		''' <param name="reader"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Create(Of T)(ByVal reader As IDataReader) As IList(Of T)
			Dim lst As IList(Of T)
			Dim typ As Type
			Dim entity As T
			Dim keys As Hashtable

			typ = GetType(T)
			keys = _getEntityInfo(typ)
			lst = New List(Of T)()

			Do While reader.Read()
				entity = _create(Of T)(keys, reader)
				lst.Add(entity)
			Loop

			Return lst
		End Function

		''' <summary>
		''' 引数のエンティティから DataRow へ変換
		''' </summary>
		''' <param name="entity">変換元</param>
		''' <param name="row">変換先</param>
		''' <remarks></remarks>
		Public Sub Convert(ByVal entity As Object, ByVal row As DataRow)
			Dim typ As Type
			Dim keys As Hashtable

			typ = entity.GetType
			keys = _getEntityInfo(typ)

			For Each col As DataColumn In row.Table.Columns
				Dim oo As Object = DBNull.Value
				Dim prop As PropertyInfo

				prop = ClassUtil.GetProperties(typ, DirectCast(keys(col.ColumnName), String))
				oo = typ.InvokeMember(DirectCast(keys(col.ColumnName), String), BindingFlags.GetProperty Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance, Nothing, entity, New Object() {})

				row.Item(col.ColumnName) = oo
			Next
		End Sub

		''' <summary>
		''' 引数のオブジェクト内にTable属性のフィールドが存在する場合は列情報を設定する
		''' </summary>
		''' <param name="obj">対象のインスタンス</param>
		''' <remarks></remarks>
		Public Sub SetColumnInfo(ByVal obj As Object)
			Dim analyzer As AttributeAnalyzer
			analyzer = New AttributeAnalyzer
			analyzer.Add(AttributeAnalyzerTargets.Field, New TableAttributeAnalyzer)
			analyzer.Analyze(obj)
		End Sub

		''' <summary>
		''' Entity をインスタンス化し、行データを Entity へ設定する
		''' </summary>
		''' <typeparam name="T"></typeparam>
		''' <param name="keys"></param>
		''' <param name="row"></param>
		''' <param name="version"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function _create(Of T)(ByVal keys As Hashtable, ByVal row As DataRow, Optional ByVal version As Object = Nothing) As T
			Dim entity As T
			Dim typ As Type
			Dim ver As DataRowVersion

			typ = GetType(T)
			entity = DirectCast(ClassUtil.NewInstance(typ), T)
			If version IsNot Nothing Then
				ver = DirectCast(version, DataRowVersion)
			End If
			For Each col As DataColumn In row.Table.Columns
				Dim val As Object
				Dim prop As PropertyInfo

				If version Is Nothing Then
					val = row(col.ColumnName)
				Else
					Select Case row.RowState
						Case DataRowState.Added
							If ver = DataRowVersion.Original Then
								ver = DataRowVersion.Default
							End If
					End Select

					val = row(col.ColumnName, ver)
				End If

				prop = ClassUtil.GetProperties(typ, DirectCast(keys(col.ColumnName), String))
				If prop.PropertyType.Equals(GetType(Object)) Then
					typ.InvokeMember(DirectCast(keys(col.ColumnName), String), BindingFlags.SetProperty Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance, Nothing, entity, New Object() {val})
				Else
					typ.InvokeMember(DirectCast(keys(col.ColumnName), String), BindingFlags.SetProperty Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance, Nothing, entity, New Object() {DbUtil.CNull(val)})
				End If
			Next
			Return entity
		End Function

		''' <summary>
		''' 
		''' </summary>
		''' <typeparam name="T"></typeparam>
		''' <param name="keys"></param>
		''' <param name="reader"></param>
		''' <param name="version"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function _create(Of T)(ByVal keys As Hashtable, ByVal reader As IDataReader, Optional ByVal version As Object = Nothing) As T
			Dim entity As T
			Dim typ As Type
			Dim ver As DataRowVersion

			typ = GetType(T)
			entity = DirectCast(ClassUtil.NewInstance(typ), T)
			If version IsNot Nothing Then
				ver = DirectCast(version, DataRowVersion)
			End If
			For ii As Integer = 0 To reader.FieldCount - 1
				Dim key As String
				Dim colName As String
				Dim val As Object
				Dim prop As PropertyInfo

				colName = reader.GetName(ii)
				key = DirectCast(keys(colName), String)
				val = reader.Item(ii)

				prop = ClassUtil.GetProperties(typ, key)
				If Not prop.PropertyType.Equals(GetType(Object)) Then
					val = DbUtil.CNull(val)
				End If

				typ.InvokeMember(key, BindingFlags.SetProperty Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance, Nothing, entity, New Object() {val})
			Next

			Return entity
		End Function

		''' <summary>
		''' Entity をインスタンス化し、行データを Entity へ設定する
		''' </summary>
		''' <typeparam name="T"></typeparam>
		''' <param name="keys"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function _create(Of T)(ByVal keys As Hashtable) As T
			Dim entity As T
			Dim typ As Type

			typ = GetType(T)
			entity = DirectCast(ClassUtil.NewInstance(typ), T)
			Return entity
		End Function

		''' <summary>
		''' Entity 情報を取得する
		''' </summary>
		''' <param name="typ"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function _getEntityInfo(ByVal typ As Type) As Hashtable
			Dim keys As Hashtable
			If _entityMap.ContainsKey(typ) Then
				keys = DirectCast(_entityMap.Item(typ), Hashtable)
			Else
				keys = DbUtil.GetColumnNames(typ)
				_entityMap.Add(typ, keys)
			End If
			Return keys
		End Function

	End Class

#Region " 未使用ではあるが、サンプルとしてとっておきたいのでコメントにして残してます "

	'''' <summary>
	'''' 引数の DataRow から指定されたタイプのデータ配列へ変換して返す。
	'''' </summary>
	'''' <typeparam name="T">変換先のタイプ</typeparam>
	'''' <returns></returns>
	'''' <remarks></remarks>
	'Public Function Create(Of T)() As T
	'	Dim typ As Type
	'	Dim entity As T
	'	Dim keys As Hashtable

	'	typ = GetType(T)

	'	keys = _getEntityInfo(typ)

	'	Dim analyzer As AttributeAnalyzer
	'	analyzer = New AttributeAnalyzer
	'	analyzer.Add(AttributeAnalyzerTargets.Class, New TableInfoAttributeAnalyzer)
	'	analyzer.Add(AttributeAnalyzerTargets.Property, New TableInfoAttributeAnalyzer)
	'	entity = DirectCast(analyzer.Create(typ), T)

	'	Return entity
	'End Function

	'	Public Class TableAttributeAnalyzer
	'		Implements IAttributeAnalyzer

	'		''' <summary>log4net logger</summary>
	'		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

	'		Public Function Analyze(ByVal target As System.Type) As Di.kmComponent Implements IAttributeAnalyzer.Analyze
	'			Dim attr As TableAttribute

	'			attr = ClassUtil.GetCustomAttribute(Of TableAttribute)(target)
	'			If attr Is Nothing Then
	'				Return Nothing
	'			End If
	'			Return attr.CreateComponent2(target)
	'		End Function

	'		Public Function Analyze(ByVal target As Object, ByVal field As System.Reflection.FieldInfo) As Di.kmComponent Implements IAttributeAnalyzer.Analyze
	'			Return Nothing
	'		End Function

	'		Public Function Analyze(ByVal targetType As System.Type, ByVal method As System.Reflection.MethodInfo) As Aop.IAspect() Implements IAttributeAnalyzer.Analyze
	'			Return Nothing
	'		End Function

	'		Public Function Analyze(ByVal targetType As System.Type, ByVal prop As System.Reflection.PropertyInfo) As Aop.IAspect() Implements IAttributeAnalyzer.Analyze
	'			Dim attr As TableAttribute
	'			Dim aspects As ArrayList
	'			Dim aspect As IAspect

	'			aspects = New ArrayList()

	'			attr = ClassUtil.GetCustomAttribute(Of TableAttribute)(targetType)
	'			If attr Is Nothing Then
	'				Return Nothing
	'			End If
	'			aspect = attr.CreateAspect2(prop)
	'			If aspect Is Nothing Then
	'				Return Nothing
	'			End If
	'			aspects.Add(aspect)

	'			_mylog.DebugFormat("Table Attribute Analyzer : {0} {1}", targetType.ToString, prop.Name)

	'			Return DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())
	'		End Function
	'	End Class

	'	Public Class ColumnInfoInterceptor
	'		Implements IMethodInterceptor

	'		''' <summary></summary>
	'		Private _length As Integer

	'		''' <summary>log4net logger</summary>
	'		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

	'#Region " コンストラクタ "

	'		''' <summary>
	'		''' コンストラクタ
	'		''' </summary>
	'		''' <remarks></remarks>
	'		Public Sub New(ByVal length As Integer)
	'			_length = length
	'		End Sub

	'#End Region

	'		Public Function Invoke(ByVal invocation As Aop.IMethodInvocation) As Object Implements Aop.IMethodInterceptor.Invoke
	'			Dim valid As Validator = New Validator

	'			If valid.Verify(invocation.Args(0), ValidateTypes.LenghtMax, max:=_length) <> ValidateTypes.None Then
	'				Throw New ArgumentException("入力桁数が多すぎます。")
	'			End If
	'			Return invocation.Proceed()
	'		End Function

	'	End Class

#End Region

End Namespace
