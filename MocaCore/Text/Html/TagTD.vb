Imports System.Text

Namespace Html

	''' <summary>
	''' ＴＤタグ出力クラス
	''' </summary>
	''' <remarks></remarks>
	Public Class TagTD
		Inherits AbstractTag

		''' <summary>タグ名</summary>
		Protected Const Tag As String = "td"

		''' <summary>値</summary>
		Protected value As String
		''' <summary>属性</summary>
		Protected attr As String

#Region " コンストラクタ "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			value = String.Empty
			attr = String.Empty
			sNewLine = False
		End Sub

#End Region

#Region " Overrides "

		''' <summary>
		''' ＴＤタグの出力
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overrides Function ToString() As String
			Dim sb As StringBuilder

			sb = New StringBuilder()

			sb.Append(sTag(Tag, attr))
			sb.Append(IIf(value.Length = 0, "<br>", value))
			sb.Append(eTag(Tag))

			Return sb.ToString
		End Function

#End Region

		''' <summary>
		''' 属性の追加
		''' </summary>
		''' <param name="value">値</param>
		''' <remarks></remarks>
		Public Sub AddAttribute(ByVal value As String)
			attr = attr & " " & value
		End Sub

		''' <summary>
		''' 値の追加
		''' </summary>
		''' <param name="value">値</param>
		''' <remarks></remarks>
		Public Sub AddValue(ByVal value As String)
			AddValue(value, String.Empty)
		End Sub

		''' <summary>
		''' 値の追加
		''' </summary>
		''' <param name="value">値</param>
		''' <param name="attr">属性</param>
		''' <remarks></remarks>
		Public Sub AddValue(ByVal value As String, ByVal attr As String)
			value = value
			attr = attr
		End Sub

	End Class

End Namespace
