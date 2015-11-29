
Imports System.Reflection

Namespace Attr

	''' <summary>
	''' フォーマット属性
	''' </summary>
	''' <remarks></remarks>
	<AttributeUsage(AttributeTargets.Property)> _
	Public Class FormatAttribute
		Inherits Attribute

		''' <summary>フォーマット</summary>
		Private _format As String

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="format">フォーマット</param>
		''' <remarks></remarks>
		Public Sub New(ByVal format As String)
			_format = format
		End Sub

#End Region

#Region " プロパティ "

		''' <summary>
		''' フォーマットプロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Format() As String
			Get
				Return _format
			End Get
		End Property

#End Region

	End Class

End Namespace
