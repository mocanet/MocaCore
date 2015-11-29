
Imports System.Reflection

Namespace Attr

	''' <summary>
	''' キャプション属性
	''' </summary>
	''' <remarks></remarks>
	<AttributeUsage(AttributeTargets.Property)> _
	Public Class CaptionAttribute
		Inherits Attribute

		''' <summary>キャプション</summary>
		Private _caption As String

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="caption">キャプション</param>
		''' <remarks></remarks>
		Public Sub New(ByVal caption As String)
			_caption = caption
		End Sub

#End Region

#Region " プロパティ "

		''' <summary>
		''' キャプションプロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Caption() As String
			Get
				Return _caption
			End Get
		End Property

#End Region

	End Class

End Namespace
