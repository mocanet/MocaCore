Imports System.IO

Namespace Util

	''' <summary>
	''' 検証集
	''' </summary>
	''' <remarks></remarks>
	Public Class ValidateUtil

		''' <summary>
		''' 文字列に全角文字が含まれているか調べる。
		''' </summary>
		''' <param name="Value">調べる対象の文字列。</param>
		''' <returns>全角文字が含まれている場合はTrue、そうでない場合False。</returns>
		''' <remarks></remarks>
		Public Shared Function IsInJis(ByVal value As String) As Boolean
			Dim length As Integer

			length = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(value)

			Return Len(value) <> length
		End Function

		''' <summary>
		''' Singleの比較
		''' </summary>
		''' <param name="valueA">比較元</param>
		''' <param name="valueB">比較先</param>
		''' <param name="delta">許容可能な差</param>
		''' <returns></returns>
		''' <remarks>
		''' System 名前空間にある Math クラスの Abs メソッドを使用して、2 つの数値の絶対的な差を計算します。
		''' 許容できる最大の差を決めます。2 つの数値の差がこれより小さい場合、2 つが等価であると見なしても実用上の問題がないような値にしてください。
		''' 差の絶対値と許容可能な差を比較します。
		''' </remarks>
		Public Shared Function IsSngEqules(ByVal valueA As Single, ByVal valueB As Single, Optional ByVal delta As Single = 0.0000001) As Boolean
			Dim absoluteDifference As Double = Math.Abs(valueA - valueB)
			Dim practicallyEqual As Boolean = (absoluteDifference < delta)

			Return practicallyEqual
		End Function

		''' <summary>
		''' ファイル名として使用出来ない禁止文字を「＿」（アンダーバー）で置換します。
		''' </summary>
		''' <param name="filename">ファイル名</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function ValidFileName(ByVal filename As String) As String
			Return ValidFileName(filename, "_")
		End Function

		''' <summary>
		''' ファイル名として使用出来ない禁止文字を指定された文字列で置換します。
		''' </summary>
		''' <param name="filename">ファイル名</param>
		''' <param name="replaceString">置換する文字列</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function ValidFileName(ByVal filename As String, ByVal replaceString As String) As String
			Dim valid As String = filename
			Dim invalidch As Char() = Path.GetInvalidFileNameChars()

			For Each c As Char In invalidch
				valid = valid.Replace(c, replaceString)
			Next
			Return valid
		End Function

		''' <summary>
		''' シート名として使用出来ない禁止文字を「＿」（アンダーバー）で置換し31文字以内にします。
		''' </summary>
		''' <param name="sheetName">シート名</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function ValidExcelSheetName(ByVal sheetName As String) As String
			Return ValidExcelSheetName(sheetName, "_")
		End Function

		''' <summary>
		''' シート名として使用出来ない禁止文字を指定された文字列で置換し31文字以内にします。
		''' </summary>
		''' <param name="sheetName">シート名</param>
		''' <param name="replaceString">置換する文字列</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function ValidExcelSheetName(ByVal sheetName As String, ByVal replaceString As String) As String
			Dim valid As String = sheetName
			Dim invalidch As Char() = {"/"c, ":"c, "\"c, "?"c, "*"c, "["c, "]"c}

			For Each c As Char In invalidch
				valid = valid.Replace(c, replaceString)
			Next
			If valid.Length >= 31 Then
				valid = valid.Substring(0, 30)
			End If
			Return valid
		End Function

	End Class

End Namespace
