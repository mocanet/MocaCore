
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
        Private _columnPropertyInfo As New Dictionary(Of String, PropertyInfo)()
        'Private _dbColumnPropertyInfo As New Dictionary(Of String, PropertyInfo)()
#If net20 Then
#Else
        Private _columnPropertyAccessor As New Dictionary(Of String, IAccessor)()
        'Private _dbColumnPropertyAccessor As New Dictionary(Of String, IAccessor)()
#End If

        Private _tableName1st As String
        Private _defProperty1st As String
        Private _tblDefs As New Dictionary(Of String, Object)()
        Private _tblDefColumns As New Dictionary(Of String, EntityInfo)()

        Private _sqlInsert As New Dictionary(Of String, String)()
        Private _sqlDelete As New Dictionary(Of String, String)()
        Private _sqlUpdate As New Dictionary(Of String, String)()

        Private _columnPropertyDef As New Dictionary(Of String, String)()
        Private _columnFunctions As New Dictionary(Of String, DbFunctionAttribute)()
        Private _columnCrudConditions As New Dictionary(Of String, CrudAttribute())()

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
                '_dbColumnPropertyInfo.Add(name, prop)
#If net20 Then
#Else
                _columnPropertyAccessor.Add(name, prop.ToAccessor())
                '_dbColumnPropertyAccessor.Add(name, prop.ToAccessor())
#End If

                Dim attrDbFunc As Moca.Db.Attr.DbFunctionAttribute = ClassUtil.GetCustomAttribute(Of Moca.Db.Attr.DbFunctionAttribute)(prop)
                If attrDbFunc IsNot Nothing Then
                    _columnFunctions.Add(name, attrDbFunc)
                End If
                Dim attrCrud() As Moca.Db.Attr.CrudAttribute = ClassUtil.GetCustomAttributes(Of Moca.Db.Attr.CrudAttribute)(prop)
                _columnCrudConditions.Add(name, attrCrud)
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
                    _defProperty1st = field.Name
                End If
                _columnPropertyDef.Add(attr.TableName, field.Name)
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

        Public ReadOnly Property TableDef(ByVal name As String) As Object
            Get
                If Not _tblDefs.ContainsKey(name) Then
                    Return Nothing
                End If
                Return _tblDefs(name)
            End Get
        End Property

        Public ReadOnly Property TableEntityInfo(ByVal name As String) As EntityInfo
            Get
                If Not _tblDefColumns.ContainsKey(name) Then
                    Return Nothing
                End If
                Return _tblDefColumns(name)
            End Get
        End Property

        Public Property SqlInsert(ByVal name As String) As String
            Get
                If Not _sqlInsert.ContainsKey(name) Then
                    Return Nothing
                End If
                Return _sqlInsert(name)
            End Get
            Set(value As String)
                _sqlInsert(name) = value
            End Set
        End Property

        Public Property SqlDelete(ByVal name As String) As String
            Get
                If Not _sqlDelete.ContainsKey(name) Then
                    Return Nothing
                End If
                Return _sqlDelete(name)
            End Get
            Set(value As String)
                _sqlDelete(name) = value
            End Set
        End Property

        Public Property SqlUpdate(ByVal name As String) As String
            Get
                If Not _sqlUpdate.ContainsKey(name) Then
                    Return Nothing
                End If
                Return _sqlUpdate(name)
            End Get
            Set(value As String)
                _sqlUpdate(name) = value
            End Set
        End Property

        Public ReadOnly Property ColumnFunctions(ByVal name As String) As DbFunctionAttribute
            Get
                If Not _columnFunctions.ContainsKey(name) Then
                    Return Nothing
                End If
                Return _columnFunctions(name)
            End Get
        End Property

        Public ReadOnly Property ColumnCrudConditions(ByVal name As String) As CrudAttribute()
            Get
                If Not _columnCrudConditions.ContainsKey(name) Then
                    Return Nothing
                End If
                Return _columnCrudConditions(name)
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
                If Not _columnNames.ContainsKey(columnName) Then
                    Return Nothing
                End If
                Return _columnNames(columnName)
            End Get
		End Property

		''' <summary>
		''' 
		''' </summary>
		''' <returns></returns>
		Public ReadOnly Property ProprtyInfo(ByVal columnName As String) As PropertyInfo
			Get
                If Not _columnPropertyInfo.ContainsKey(columnName) Then
                    Return Nothing
                End If
                Return _columnPropertyInfo(columnName)
            End Get
		End Property

#If net20 Then
#Else
		Public ReadOnly Property ProprtyAccessor(ByVal columnName As String) As IAccessor
			Get
                If Not _columnPropertyAccessor.ContainsKey(columnName) Then
                    Return Nothing
                End If
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
