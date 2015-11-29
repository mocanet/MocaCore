
Imports System.Reflection

Imports Moca.Aop
Imports Moca.Db.Interceptor
Imports Moca.Di
Imports Moca.Util

Namespace Db.Attr

	''' <summary>
	''' DBテーブル属性
	''' </summary>
	''' <remarks>
	''' DBテーブルの定義を表すクラスとする場合にこの属性を使う。<br/>
	''' DBから情報を取得する為にアプリケーション構成ファイルからコネクションストリングを取得します。
	''' </remarks>
	<AttributeUsage(AttributeTargets.Class Or AttributeTargets.Interface, Inherited:=True)> _
	Public Class TableAttribute
		Inherits Attribute

		''' <summary>コネクションストリングのキー値</summary>
		Private _appkey As String

		''' <summary>テーブル名</summary>
		Private _tableName As String

#Region " Logging For Log4net "
		''' <summary>Logging For Log4net</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)
#End Region

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="appkey">アプリケーション構成のキー</param>
		''' <param name="tableName">テーブル名</param>
		''' <remarks></remarks>
		Public Sub New(ByVal appkey As String, ByVal tableName As String)
			_appkey = appkey
			_tableName = tableName
		End Sub

#End Region
#Region " Property "

		''' <summary>キー値</summary>
		Public Property Appkey() As String
			Get
				Return _appkey
			End Get
			Set(ByVal value As String)
				_appkey = value
			End Set
		End Property

		''' <summary>
		''' テーブル名プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property TableName() As String
			Get
				Return _tableName
			End Get
		End Property

#End Region
#Region " Method "

		''' <summary>
		''' コンポーネント作成
		''' </summary>
		''' <returns>コンポーネント</returns>
		''' <remarks></remarks>
		Public Function CreateComponent(ByVal target As Object, ByVal field As FieldInfo) As MocaComponent
			Dim aspects As ArrayList

			aspects = New ArrayList()

			' DBMS 特定
			Dim targetDbms As Dbms
			targetDbms = DbmsManager.GetDbms(Appkey)
			' 列情報の取得
			Dim hlp As IDbAccessHelper
			Dim tblInfo As DbInfoTable
			Dim colInfos As DbInfoColumnCollection
			Dim colInfo As DbInfoColumn
			hlp = targetDbms.CreateDbAccess().Helper
			tblInfo = hlp.GetSchemaTable(_tableName)
			colInfos = tblInfo.Columns

			' フィールドのインタフェースを解析
			' プロパティ
			Dim props() As PropertyInfo
			props = ClassUtil.GetProperties(field.FieldType)
			For Each prop As PropertyInfo In props
				Dim name As String
				Dim attr As ColumnAttribute
				Dim pointcut As IPointcut

				' テーブル定義のとき
				If prop.PropertyType.Equals(GetType(DbInfoTable)) Then
					' Getter メソッドのアスペクト作成
					pointcut = New Pointcut(New String() {prop.GetGetMethod().ToString})
					aspects.Add(New Aspect(New TableInfoInterceptor(tblInfo), pointcut))
					Continue For
				End If

				' Column属性の確認
				name = prop.Name
				attr = ClassUtil.GetCustomAttribute(Of ColumnAttribute)(prop)
				If attr IsNot Nothing Then
					name = attr.ColumnName
				End If

				' 列情報特定
				colInfo = colInfos(name)

				' Getter メソッドのアスペクト作成
				Pointcut = New Pointcut(New String() {prop.GetGetMethod().ToString})
				aspects.Add(New Aspect(New ColumnInfoInterceptor(colInfo), pointcut))
			Next

			' メソッド
			Dim methods() As MethodInfo
			methods = ClassUtil.GetMethods(field.FieldType)
			For Each method As MethodInfo In methods
				If method.ReturnType.Equals(GetType(DbInfoTable)) Then
					' Getter メソッドのアスペクト作成
					Dim pointcut As IPointcut
					pointcut = New Pointcut(New String() {method.Name().ToString})
					aspects.Add(New Aspect(New TableInfoInterceptor(tblInfo), pointcut))
				End If
			Next

			' コンポーネント作成
			Dim component As MocaComponent
			component = New MocaComponent(field.FieldType, field.FieldType)
			component.Aspects = DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())
			Return component
		End Function

#Region " 未使用ではあるが、サンプルとしてとっておきたいのでコメントにして残してます "

		'''' <summary>
		'''' コンポーネント作成
		'''' </summary>
		'''' <returns>DBMSコンポーネント</returns>
		'''' <remarks></remarks>
		'Public Function CreateComponent2(ByVal typ As Type) As kmComponent
		'	Dim component As kmComponent
		'	component = New kmComponent(typ, typ)
		'	Return component
		'End Function

		'''' <summary>
		'''' アスペクトを作成する
		'''' </summary>
		'''' <param name="prop">プロパティ</param>
		'''' <returns>アスペクト</returns>
		'''' <remarks></remarks>
		'Public Shadows Function CreateAspect2(ByVal prop As PropertyInfo) As IAspect
		'	Dim pointcut As IPointcut
		'	Dim val As IAspect

		'	Dim colName As String
		'	colName = DbUtil.GetColumnName(prop)

		'	' DBMS 特定
		'	Dim targetDbms As Dbms
		'	targetDbms = DbmsManager.GetDbms(Appkey)

		'	Dim hlp As IDbAccessHelper
		'	Dim tblInfo As DbInfoTable
		'	Dim colInfo As DbInfoColumn
		'	hlp = targetDbms.CreateDbAccess().Helper
		'	tblInfo = hlp.GetSchemaTables.Table(_tableName)
		'	colInfo = hlp.GetSchemaColumns(tblInfo)(colName)

		'	' TODO: SQL Server 特有の為、Oracle等の場合は対処が必要
		'	If colInfo.Typ <> "nvarchar" AndAlso colInfo.Typ <> "varchar" AndAlso colInfo.Typ <> "nchar" AndAlso colInfo.Typ <> "char" Then
		'		Return Nothing
		'	End If

		'	pointcut = New Pointcut(New String() {prop.GetSetMethod.Name})
		'	val = New Aspect(New ColumnInfoInterceptor(colInfo.MaxLength), pointcut)

		'	Return val
		'End Function

#End Region

#End Region

	End Class

End Namespace
