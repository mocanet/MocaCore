
Namespace Html

	''' <summary>
	''' タグ出力抽象クラス
	''' </summary>
	''' <remarks></remarks>
	Public MustInherit Class AbstractTag

		''' <summary>改行有無</summary>
		Protected sNewLine As Boolean

#Region " コンストラクタ "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			sNewLine = True
		End Sub

#End Region

		''' <summary>
		''' 開始タグ
		''' </summary>
		''' <param name="value">タグ名</param>
		''' <param name="attr">属性文字列</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function sTag(ByVal value As String, ByVal attr As String) As String
			attr = attr.Trim()
			attr = CStr(IIf(attr.Length = 0, attr, " " & attr))

			If sNewLine Then
				Return String.Format("<{0}{1}>{2}", value, attr, Environment.NewLine)
			End If
			Return String.Format("<{0}{1}>", value, attr)
		End Function

		''' <summary>
		''' 閉じタグ
		''' </summary>
		''' <param name="value">タグ名</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function eTag(ByVal value As String) As String
			Return String.Format("</{0}>{1}", value, Environment.NewLine)
		End Function

	End Class

End Namespace
