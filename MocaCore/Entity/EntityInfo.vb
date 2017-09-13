
Imports System.Reflection
Imports Moca.Attr
Imports Moca.Db
Imports Moca.Db.Attr
Imports Moca.Util

Namespace Entity

	''' <summary>
	''' 
	''' </summary>
	Public Class EntityInfo

#Region " Declare "

		Private _type As Type

		''' <summary></summary>
		Private _columnNames As New Dictionary(Of String, String)()
		''' <summary></summary>
		Private _columnPropertyInfo As New Dictionary(Of String, PropertyInfo)
#If net20 Then
#Else
		Private _columnPropertyAccessor As New Dictionary(Of String, IAccessor)()
#End If

		'Private _columnDefs As New Hashtable()

		Private _tableName1st As String
		Private _tblDefs As New Hashtable
		Private _tblDefColumns As New Hashtable

#End Region
#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="typ"></param>
		Public Sub New(ByVal typ As Type)
			_type = typ

			Dim pinfo() As PropertyInfo

			pinfo = ClassUtil.GetProperties(typ)
			For Each prop As PropertyInfo In pinfo
				Dim name As String
				name = DbUtil.GetColumnName(prop)

				If String.IsNullOrEmpty(name) Then
					Continue For
				End If
				_columnNames.Add(name, prop.Name)
				_columnPropertyInfo.Add(name, prop)
#If net20 Then
#Else
				_columnPropertyAccessor.Add(name, prop.ToAccessor())
#End If
			Next

			_setColumnInfo(typ)

			Dim fields() As FieldInfo = _type.GetFields(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Static)
			For Each field As FieldInfo In fields
				Dim attr As Moca.Db.Attr.TableAttribute = ClassUtil.GetCustomAttribute(Of Moca.Db.Attr.TableAttribute)(field.FieldType)
				If attr Is Nothing Then
					Continue For
				End If
				If String.IsNullOrEmpty(_tableName1st) Then
					_tableName1st = attr.TableName
				End If
				_tblDefs.Add(attr.TableName, field.GetValue(_type))
				_tblDefColumns.Add(attr.TableName, New EntityInfo(field.FieldType))
			Next

		End Sub

#End Region
#Region " Property "

		Public ReadOnly Property TableName1st As String
			Get
				Return _tableName1st
			End Get
		End Property

		Public ReadOnly Property ColumnType As Type
			Get
				Return _type
			End Get
		End Property

		Public ReadOnly Property NameMap As IDictionary(Of String, String)
			Get
				Return _columnNames
			End Get
		End Property

		Public ReadOnly Property PropertyInfoMap As IDictionary(Of String, PropertyInfo)
			Get
				Return _columnPropertyInfo
			End Get
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ColumnNames As ICollection
			Get
				Return _columnNames.Keys
			End Get
		End Property

		Public ReadOnly Property PropertyName(ByVal columnName As String) As String
			Get
				Return _columnNames(columnName)
			End Get
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <returns></returns>
		Public ReadOnly Property ProprtyInfo(ByVal columnName As String) As PropertyInfo
			Get
				Return _columnPropertyInfo(columnName)
			End Get
		End Property

#If net20 Then
#Else
		Public ReadOnly Property ProprtyAccessor(ByVal columnName As String) As IAccessor
			Get
				Return _columnPropertyAccessor(columnName)
			End Get
		End Property
#End If

#End Region
#Region " Method "

		'''' <summary>
		'''' 列名取得
		'''' </summary>
		'Private Function _getColumnNames(ByVal defFieldName As String, ByVal defFieldInfo As FieldInfo) As IDictionary(Of String, String)
		'	Dim columnNames As IDictionary(Of String, String) = New Dictionary(Of String, String)

		'	Dim props() As PropertyInfo = defFieldInfo.FieldType.GetProperties()
		'	For Each prop As PropertyInfo In props
		'		Dim attrColumn As Moca.Db.Attr.ColumnAttribute = ClassUtil.GetCustomAttribute(Of Moca.Db.Attr.ColumnAttribute)(prop)
		'		Dim columnName As String = prop.Name
		'		If attrColumn IsNot Nothing Then
		'			columnName = attrColumn.ColumnName
		'		End If
		'		columnNames.Add(columnName, prop.Name)
		'	Next

		'	_columnNames.Add(defFieldName, columnNames)

		'	Return columnNames
		'End Function

#End Region

		''' <summary>
		''' 引数のオブジェクト内にTable属性のフィールドが存在する場合は列情報を設定する
		''' </summary>
		''' <param name="obj">対象のインスタンス</param>
		''' <remarks></remarks>
		Private Sub _setColumnInfo(ByVal obj As Type)
			_analyzer.Add(AttributeAnalyzerTargets.Field, _tblAttrAnalyer)
			_analyzer.Analyze(obj)
		End Sub
		Private _analyzer As New AttributeAnalyzer
		Private _tblAttrAnalyer As New TableAttributeAnalyzer

	End Class

End Namespace
