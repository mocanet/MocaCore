Imports System.Text

Namespace Html

	''' <summary>
	''' ＴＲタグ出力クラス
	''' </summary>
	''' <remarks></remarks>
	Public Class TagTR
		Inherits AbstractTag

		''' <summary>タグ名</summary>
		Protected Const Tag As String = "tr"

		''' <summary>属性</summary>
		Protected attr As String
		''' <summary>行</summary>
		Protected rows As ArrayList
		''' <summary>列</summary>
		Protected cols As ArrayList

#Region " コンストラクタ "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			rows = New ArrayList
			cols = New ArrayList
			attr = String.Empty
		End Sub

#End Region

#Region " Overrides "

		''' <summary>
		''' タグ出力
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overrides Function ToString() As String
			Dim ii As Integer
			Dim sb As StringBuilder

			sb = New StringBuilder()

			sb.Append(sTag(Tag, attr))
			For ii = 0 To cols.Count - 1
				If ii = 0 Then
					If rows.Count > 0 Then
						CType(cols(ii), TagTD).AddAttribute(chkRowSpan())
					End If
					sb.Append(cols(ii).ToString)
					Continue For
				End If
				sb.Append(cols(ii).ToString)
			Next
			sb.Append(eTag(Tag))

			For ii = 0 To rows.Count - 1
				sb.Append(rows(ii).ToString)
			Next

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
		''' 行の追加
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function AddRow() As TagTR
			Return AddRow(String.Empty)
		End Function

		''' <summary>
		''' 行の追加
		''' </summary>
		''' <param name="attr">属性</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function AddRow(ByVal attr As String) As TagTR
			Dim tr As TagTR
			tr = New TagTR
			tr.AddAttribute(attr)
			rows.Add(tr)
			Return tr
		End Function

		''' <summary>
		''' 列の追加
		''' </summary>
		''' <param name="value">値</param>
		''' <remarks></remarks>
		Public Sub AddTD(ByVal value As String)
			AddTD(value, String.Empty)
		End Sub

		''' <summary>
		''' 列の追加
		''' </summary>
		''' <param name="value">値</param>
		''' <param name="attr">属性</param>
		''' <remarks></remarks>
		Public Sub AddTD(ByVal value As String, ByVal attr As String)
			Dim td As TagTD

			td = New TagTD()
			td.AddValue(value, attr)

			cols.Add(td)
		End Sub

		''' <summary>
		''' 行追加があれば、rowspan属性を付ける
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function chkRowSpan() As String
			If rows.Count = 0 Then
				Return String.Empty
			End If

			Return String.Format(" rowspan=""{0}""", rows.Count + 1)
		End Function

	End Class

End Namespace
