
Imports System.Reflection

Namespace Attr

	''' <summary>
	''' バインドコントロール属性
	''' </summary>
	''' <remarks></remarks>
	<AttributeUsage(AttributeTargets.Property)> _
	Public Class BindControlAttribute
		Inherits Attribute

		''' <summary>コントロール名</summary>
		Private _name As IList(Of String)

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="name">コントロール名</param>
		''' <remarks></remarks>
		Public Sub New(ByVal ParamArray name() As String)
			_name = name
		End Sub

#End Region
#Region " プロパティ "

		''' <summary>
		''' コントロール名プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ControlName() As IList(Of String)
			Get
				Return _name
			End Get
		End Property

#End Region

	End Class

End Namespace
