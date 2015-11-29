
Namespace Db.Attr

	''' <summary>
	''' 列名属性
	''' </summary>
	''' <remarks>
	''' 列名がプロパティ名とは異なるときに指定する。
	''' </remarks>
	<AttributeUsage(AttributeTargets.Property)> _
	Public Class ColumnAttribute
		Inherits Attribute

		''' <summary>カラム名</summary>
		Private _columnName As String

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="columnName">列名</param>
		''' <remarks></remarks>
		Public Sub New(ByVal columnName As String)
			_columnName = columnName
		End Sub

#End Region

#Region " プロパティ "

		''' <summary>
		''' 列名プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ColumnName() As String
			Get
				Return _columnName
			End Get
		End Property

#End Region

	End Class

End Namespace
