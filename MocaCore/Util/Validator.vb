
Imports System.Text.RegularExpressions

Imports Moca.Util

Namespace Util

#Region " ValidateTypes "

	''' <summary>
	''' 検証種別及び検証結果
	''' </summary>
	''' <remarks></remarks>
	<Flags()> _
	Public Enum ValidateTypes As Integer
		''' <summary>「無し」又は「正常」</summary>
		None = 0
		''' <summary>必須</summary>
		Required = 1
		''' <summary>数字のみ</summary>
		Numeric = Required * 2
		''' <summary>数値のみ</summary>
		[Decimal] = Numeric * 2
		''' <summary>符号付数値のみ</summary>
		WithNumericSigned = [Decimal] * 2
		''' <summary>〒</summary>
		Zip = WithNumericSigned * 2
		''' <summary>電話番号</summary>
		Tel = Zip * 2
		''' <summary>文字列に全角文字が含まれているか</summary>
		InJis = Tel * 2
		''' <summary>最小桁数</summary>
		LenghtMin = InJis * 2
		''' <summary>最大桁数</summary>
		LenghtMax = LenghtMin * 2
		''' <summary>最小桁数（半角 1 バイト、全角 2 バイトとして）</summary>
		LenghtMinB = LenghtMax * 2
		''' <summary>最大桁数（半角 1 バイト、全角 2 バイトとして）</summary>
		LenghtMaxB = LenghtMinB * 2
		''' <summary>日付</summary>
		DateFormat = LenghtMaxB * 2
		''' <summary>最小数値</summary>
		Minimum = DateFormat * 2
		''' <summary>最大数値</summary>
		Maximum = Minimum * 2
		''' <summary>半角英数</summary>
		HalfWidthAlphanumeric = Maximum * 2
		''' <summary>Mail</summary>
		Mail = HalfWidthAlphanumeric * 2
		''' <summary>URL</summary>
		URL = Mail * 2
	End Enum

#End Region

	''' <summary>
	''' 値の検証クラス
	''' </summary>
	''' <remarks></remarks>
	Public Class Validator

#Region " Methods "

		''' <summary>
		''' 検証実行
		''' </summary>
		''' <param name="value">検証対象の値</param>
		''' <param name="validates">検証内容（OR にて複数同時指定可能）</param>
		''' <param name="min">検証内容にて最小値が必要なとき指定する</param>
		''' <param name="max">検証内容にて最大値が必要なとき指定する</param>
		''' <returns>検証結果（複数エラーが発生したときは複数指定される）</returns>
		''' <remarks>
		''' 必須チェックが指定されていてエラーとなったときは、他のチェックが指定されていたとしてもチェックしません。
		''' （必須エラーだけを返します。）
		''' </remarks>
		Public Function Verify(ByVal value As String, ByVal validates As ValidateTypes, Optional ByVal min As Object = Nothing, Optional ByVal max As Object = Nothing) As ValidateTypes
			Dim val As String
			Dim rc As ValidateTypes

			If validates = ValidateTypes.None Then
				Return ValidateTypes.None
			End If

			If value Is Nothing Then
				val = String.Empty
			Else
				val = value.ToString.Trim
			End If
			rc = ValidateTypes.None

			' 必須
			If _IsValidateType(validates, ValidateTypes.Required) Then
				If val.Length = 0 Then
					rc = rc Or ValidateTypes.Required
				End If
			End If
			' 必須チェックでエラーのときは抜ける
			If rc <> ValidateTypes.None Then
				Return rc
			End If
			' 未入力時はここまで
			If val.Length = 0 Then
				Return rc
			End If

			' 数字のみ, 符号付数値のみ
			If _IsValidateType(validates, ValidateTypes.Numeric) Then
				If _IsValidateType(validates, ValidateTypes.WithNumericSigned) Then
					If Not Regex.IsMatch(val, "^(?![-+]0+$)(?:|-?[0-9]+)*(?:|[0-9])?$") Then
						rc = rc Or ValidateTypes.WithNumericSigned
					End If
				Else
					If Not Regex.IsMatch(val, "^[0-9]*$") Then
						rc = rc Or ValidateTypes.Numeric
					End If
				End If
			End If

			' 数値のみ, 符号付数値のみ
			If _IsValidateType(validates, ValidateTypes.Decimal) Then
				If _IsValidateType(validates, ValidateTypes.WithNumericSigned) Then
					If Not Regex.IsMatch(val, "^(?![-+]0+$)(?:|-?[0-9]+)*(?:|[0-9]*\.[0-9]+)$") Then	' (?![-+]0+$)[-+]?([1-9][0-9]*)?[0-9](\.[0-9]*[1-9])?
						rc = rc Or ValidateTypes.Decimal
					End If
				Else
					If Not Regex.IsMatch(val, "^(?:|[0-9]+)*(?:|[0-9]*\.[0-9]+)$") Then
						rc = rc Or ValidateTypes.Decimal
					End If
				End If
			End If

			' 〒
			If _IsValidateType(validates, ValidateTypes.Zip) Then
				If Not Regex.IsMatch(val, "^([0-9]{3}\-?[0-9]{4})?$") Then
					rc = rc Or ValidateTypes.Zip
				End If
			End If

			' 電話番号
			If _IsValidateType(validates, ValidateTypes.Tel) Then
				If Not Regex.IsMatch(val, "^([0-9]{1,5}[\-]?[0-9]{1,4}[\-]?[0-9]{3,4})?$") Then
					rc = rc Or ValidateTypes.Tel
				End If
			End If

			' 文字列に全角文字が含まれているか調べる
			If _IsValidateType(validates, ValidateTypes.InJis) Then
				' 全角文字が含まれている場合はTrue、そうでない場合False
				If Len(value) <> System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(value) Then
					rc = rc Or ValidateTypes.InJis
				End If
			End If

			' 半角英数
			If _IsValidateType(validates, ValidateTypes.HalfWidthAlphanumeric) Then
				If Not Regex.IsMatch(val, "^[a-zA-Z0-9]+$") Then
					rc = rc Or ValidateTypes.HalfWidthAlphanumeric
				End If
			End If

			' Mail
			If _IsValidateType(validates, ValidateTypes.Mail) Then
                If Not Regex.IsMatch(val, "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])", RegexOptions.IgnoreCase) Then
                    rc = rc Or ValidateTypes.Mail
                End If
            End If

			' URL
			If _IsValidateType(validates, ValidateTypes.URL) Then
				If Not Regex.IsMatch(val, "^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$") Then
					rc = rc Or ValidateTypes.URL
				End If
			End If

			' 最小桁数
			If _IsValidateType(validates, ValidateTypes.LenghtMin) And min IsNot Nothing Then
				If val.Length < CInt(min) Then
					rc = rc Or ValidateTypes.LenghtMin
				End If
			End If

			' 最大桁数
			If _IsValidateType(validates, ValidateTypes.LenghtMax) And max IsNot Nothing Then
				If val.Length > CInt(max) Then
					rc = rc Or ValidateTypes.LenghtMax
				End If
			End If

			' 最小桁数（半角 1 バイト、全角 2 バイトとして）
			If _IsValidateType(validates, ValidateTypes.LenghtMinB) And min IsNot Nothing Then
				If VBUtil.LenB(val) < CInt(min) Then
					rc = rc Or ValidateTypes.LenghtMinB
				End If
			End If

			' 最大桁数（半角 1 バイト、全角 2 バイトとして）
			If _IsValidateType(validates, ValidateTypes.LenghtMaxB) And max IsNot Nothing Then
				If VBUtil.LenB(val) > CInt(max) Then
					rc = rc Or ValidateTypes.LenghtMaxB
				End If
			End If

			' 日付
			Dim dateVal As Date
			If _IsValidateType(validates, ValidateTypes.DateFormat) Then
				If Not Date.TryParse(value, dateVal) Then
					rc = rc Or ValidateTypes.DateFormat
				End If
			End If

			' 数値、日付以外はここまで
			If Not _IsValidateType(validates, ValidateTypes.Numeric) _
			And Not _IsValidateType(validates, ValidateTypes.WithNumericSigned) _
			And Not _IsValidateType(validates, ValidateTypes.Decimal) _
			And Not _IsValidateType(validates, ValidateTypes.DateFormat) Then
				Return rc
			End If

			' エラーになってる時もここまで
			If _IsValidateType(rc, ValidateTypes.Numeric) _
			OrElse _IsValidateType(rc, ValidateTypes.WithNumericSigned) _
			OrElse _IsValidateType(rc, ValidateTypes.Decimal) _
			OrElse _IsValidateType(rc, ValidateTypes.DateFormat) Then
				Return rc
			End If

			' 最小数値
			If _IsValidateType(validates, ValidateTypes.Minimum) And min IsNot Nothing Then
				If _IsValidateType(validates, ValidateTypes.DateFormat) Then
					If CDate(val) < CDate(min) Then
						rc = rc Or ValidateTypes.Minimum
					End If
				Else
					If CDec(val) < CDec(min) Then
						rc = rc Or ValidateTypes.Minimum
					End If
				End If
			End If

			' 最大数値
			If _IsValidateType(validates, ValidateTypes.Maximum) And max IsNot Nothing Then
				If _IsValidateType(validates, ValidateTypes.DateFormat) Then
					If CDate(val) > CDate(max) Then
						rc = rc Or ValidateTypes.Maximum
					End If
				Else
					If CDec(val) > CDec(max) Then
						rc = rc Or ValidateTypes.Maximum
					End If
				End If
			End If

			Return rc
		End Function

		''' <summary>
		''' 検証種別のチェック
		''' </summary>
		''' <param name="validates">チェック対象</param>
		''' <param name="targetType">含まれているかチェックする検証種別</param>
		''' <returns>True: 含まれている、False: 含まれていない</returns>
		''' <remarks></remarks>
		Private Function _IsValidateType(ByVal validates As ValidateTypes, ByVal targetType As ValidateTypes) As Boolean
			Return ((validates And targetType) = targetType)
		End Function

		''' <summary>
		''' 検証種別のチェック
		''' </summary>
		''' <param name="validates">チェック対象</param>
		''' <param name="targetType">含まれているかチェックする検証種別</param>
		''' <returns>True: 含まれている、False: 含まれていない</returns>
		''' <remarks></remarks>
		<Obsolete("個別のIsValidXxxxを使用してください。")> _
		Public Function IsValidateType(ByVal validates As ValidateTypes, ByVal targetType As ValidateTypes) As Boolean
			Return ((validates And targetType) = targetType)
		End Function

		''' <summary>
		''' 必須エラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidRequired(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Required
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 数字のみエラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidNumeric(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Numeric
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 数値のみエラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidDecimal(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Decimal
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 符号付数値のみエラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidWithNumericSigned(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.WithNumericSigned
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 〒エラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidZip(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Zip
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 電話番号エラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidTel(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Tel
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 文字列に全角文字が含まれているかエラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidInJis(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.InJis
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 最小桁数エラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidLenghtMin(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.LenghtMin
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 最大桁数エラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidLenghtMax(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.LenghtMax
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 最小桁数（半角 1 バイト、全角 2 バイトとして）エラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidLenghtMinB(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.LenghtMinB
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 最大桁数（半角 1 バイト、全角 2 バイトとして）エラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidLenghtMaxB(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.LenghtMaxB
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 日付エラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidDateFormat(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.DateFormat
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 最小数値エラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidMinimum(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Minimum
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 最大数値エラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidMaximum(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Maximum
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' 半角英数エラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidHalfWidthAlphanumeric(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.HalfWidthAlphanumeric
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' Mailエラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidMail(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Mail
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' URLエラー有無
		''' </summary>
		''' <param name="validates">チェック結果</param>
		''' <returns>True:正常、False:エラー</returns>
		''' <remarks></remarks>
		Public Function IsValidURL(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.URL
			Return Not ((validates And chk) = chk)
		End Function

#End Region

	End Class

End Namespace
