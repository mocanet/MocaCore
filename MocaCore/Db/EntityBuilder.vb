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

        '''' <summary>一度解析したEntityを格納しておく</summary>
        'Private _entityMapName As New Hashtable
        'Private _entityMapPropertyInfo As New Hashtable

        Private Shared _objType As Type = GetType(Object)

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
            Dim keys As IDictionary(Of String, String)

            typ = GetType(T)

            ds.Tables.Add(dt)

            keys = Moca.Entity.EntityInfoCache.Store(typ).NameMap
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
            Dim keys As IDictionary(Of String, String)
            Dim props As IDictionary(Of String, PropertyInfo)
            Dim view As DataView

            typ = GetType(T)
            keys = Moca.Entity.EntityInfoCache.Store(typ).NameMap
            props = Moca.Entity.EntityInfoCache.Store(typ).PropertyInfoMap
            ary = New ArrayList(256)

            view = tbl.DefaultView

            For ii As Integer = 0 To view.Count - 1
                entity = _create(Of T)(keys, props, view.Item(ii).Row)
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
            Dim keys As IDictionary(Of String, String)
            Dim props As IDictionary(Of String, PropertyInfo)

            typ = GetType(T)
            keys = Moca.Entity.EntityInfoCache.Store(typ).NameMap
            props = Moca.Entity.EntityInfoCache.Store(typ).PropertyInfoMap

            entity = _create(Of T)(keys, props, row, Nothing)

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
            Dim keys As IDictionary(Of String, String)
            Dim props As IDictionary(Of String, PropertyInfo)

            typ = GetType(T)
            keys = Moca.Entity.EntityInfoCache.Store(typ).NameMap
            props = Moca.Entity.EntityInfoCache.Store(typ).PropertyInfoMap

            entity = _create(Of T)(keys, props, row, version)

            Return entity
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="reader"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Create(Of T)(ByVal reader As IDataReader) As IList
            Dim lst As IList
            Dim typ As Type
            Dim entity As T
            Dim keys As IDictionary(Of String, String)
            Dim props As IDictionary(Of String, PropertyInfo)

            typ = chkType(Of T)(reader)
            keys = Moca.Entity.EntityInfoCache.Store(typ).NameMap
            props = Moca.Entity.EntityInfoCache.Store(typ).PropertyInfoMap

            If GetType(T).Equals(typ) Then
                lst = New List(Of T)(256)
            Else
                Dim store As ITypeStore = ClassUtil.NewInstance(typ)
                lst = store.NewList()
            End If

            Do While reader.Read()
                entity = _create(Of T)(typ, Moca.Entity.EntityInfoCache.Store(typ), reader)
                lst.Add(entity)
            Loop
            If Not lst.Count.Equals(0) Then
                SetColumnInfo(lst(0))
            End If

            Return lst
        End Function

        Protected Function chkType(Of T)(ByVal reader As IDataReader) As Type
            Dim typ As Type = GetType(T)

            Dim attr As DynamicPropertyAttribute
            attr = ClassUtil.GetCustomAttribute(Of DynamicPropertyAttribute)(typ)
            If attr IsNot Nothing Then
                If TypeStore.Store.ContainsKey(typ) Then
                    typ = TypeStore.Store(typ)
                Else
                    Dim lst As New List(Of KeyValuePair(Of String, Type))
                    For ii As Integer = 0 To reader.FieldCount - 1
                        Dim colName As String
                        Dim colType As Type

                        colName = reader.GetName(ii)
                        ' 同じプロパティは作らない
                        If typ.GetProperty(colName) IsNot Nothing Then
                            Continue For
                        End If

                        ' プリミティブ型と日付は Nullable に強制
                        colType = reader.GetFieldType(ii)
                        If colType.IsPrimitive OrElse colType.Equals(GetType(DateTime)) Then
                            colType = GetType(Nullable(Of)).MakeGenericType(colType)
                        End If
                        lst.Add(New KeyValuePair(Of String, Type)(colName, colType))
                    Next
                    typ = TypeStore.Store.Create(typ, lst.ToArray())
                End If
            End If

            Return typ
        End Function


        ''' <summary>
        ''' 引数のエンティティから DataRow へ変換
        ''' </summary>
        ''' <param name="entity">変換元</param>
        ''' <param name="row">変換先</param>
        ''' <remarks></remarks>
        Public Sub Convert(ByVal entity As Object, ByVal row As DataRow)
            Dim typ As Type
            Dim keys As IDictionary(Of String, String)
            Dim key As String
            Dim props As IDictionary(Of String, PropertyInfo)

            typ = entity.GetType
            keys = Moca.Entity.EntityInfoCache.Store(typ).NameMap
            props = Moca.Entity.EntityInfoCache.Store(typ).PropertyInfoMap

            For Each col As DataColumn In row.Table.Columns
                Dim oo As Object
                Dim prop As PropertyInfo

                key = keys(col.ColumnName)
                If String.IsNullOrEmpty(key) Then
                    ' エンティティに列が指定されていなかった
                    Throw New MissingMemberException(typ.FullName, col.ColumnName)
                End If
                prop = props(col.ColumnName)
                oo = prop.GetValue(entity, Nothing)
                oo = DbUtil.CNull(oo)

                row.Item(col.ColumnName) = oo
            Next
        End Sub

        ''' <summary>
        ''' 引数のオブジェクト内にTable属性のフィールドが存在する場合は列情報を設定する
        ''' </summary>
        ''' <param name="obj">対象のインスタンス</param>
        ''' <remarks></remarks>
        Public Sub SetColumnInfo(ByVal obj As Object)
            _analyzer.Add(AttributeAnalyzerTargets.Field, _tblAttrAnalyer)
            _analyzer.Analyze(obj)
        End Sub
        Private _analyzer As New AttributeAnalyzer
        Private _tblAttrAnalyer As New TableAttributeAnalyzer

        ''' <summary>
        ''' Entity をインスタンス化し、行データを Entity へ設定する
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="keys"></param>
        ''' <param name="row"></param>
        ''' <param name="version"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function _create(Of T)(ByVal keys As IDictionary(Of String, String), ByVal props As IDictionary(Of String, PropertyInfo), ByVal row As DataRow, Optional ByVal version As Object = Nothing) As T
            Dim entity As T
            Dim typ As Type
            Dim ver As DataRowVersion

            typ = GetType(T)
            entity = ClassUtil.NewInstance(typ)
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

                prop = props(col.ColumnName)

                If Not prop.PropertyType.Equals(_objType) Then
                    val = DbUtil.CNull(val)
                End If
                prop.SetValue(entity, val, Nothing)
            Next
            Return entity
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function _create(Of T)(ByVal typ As Type, ByVal entityInfo As Entity.EntityInfo, ByVal reader As IDataRecord) As T
            Dim entity As T
            Dim keys As Dictionary(Of String, String) = entityInfo.NameMap
            Dim props As IDictionary(Of String, PropertyInfo) = entityInfo.PropertyInfoMap

#If net20 Then
			entity = ClassUtil.NewInstance(typ)
#Else
            If GetType(T).Equals(typ) Then
                entity = ClassUtil.NewInstance(Of T)()
            Else
                entity = ClassUtil.NewInstance(typ)
            End If
#End If

            For ii As Integer = 0 To reader.FieldCount - 1
                Dim colName As String
                Dim val As Object
                Dim prop As PropertyInfo

                colName = reader.GetName(ii)

                Try
                    prop = props(colName)
                Catch ex As Exception
                    ' エンティティに列が指定されていなかった
                    Throw New MissingMemberException(typ.FullName, colName)
                End Try

                val = reader.Item(ii)
                If Not prop.PropertyType.Equals(_objType) Then
                    val = DbUtil.CNull(val)
                End If

#If net20 Then
				prop.SetValue(entity, val, Nothing)
#Else
                entityInfo.ProprtyAccessor(colName).SetValue(entity, val)
#End If
            Next

            Return entity
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
