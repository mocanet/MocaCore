
Namespace Db.Attr

	''' <summary>
	''' 未使用属性
	''' </summary>
	''' <remarks>
	''' 列として使用しないときに指定する。<br/>
	''' 指定されていないときは、「使用する」となる。
	''' </remarks>
	<AttributeUsage(AttributeTargets.Property)> _
	Public Class ColumnIgnoreAttribute
		Inherits Attribute

		''' <summary>使用有無</summary>
		Private _ignore As Boolean

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="ignore">使用するときは True、使用しないときは False</param>
		''' <remarks></remarks>
		Public Sub New(ByVal ignore As Boolean)
			_ignore = ignore
		End Sub

#End Region

#Region " プロパティ "

		''' <summary>
		''' 使用有無プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Ignore() As Boolean
			Get
				Return _ignore
			End Get
		End Property

#End Region

	End Class

End Namespace
