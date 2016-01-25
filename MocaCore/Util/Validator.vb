
Imports System.Text.RegularExpressions

Imports Moca.Util

Namespace Util

#Region " ValidateTypes "

	''' <summary>
	''' ���؎�ʋy�ь��،���
	''' </summary>
	''' <remarks></remarks>
	<Flags()> _
	Public Enum ValidateTypes As Integer
		''' <summary>�u�����v���́u����v</summary>
		None = 0
		''' <summary>�K�{</summary>
		Required = 1
		''' <summary>�����̂�</summary>
		Numeric = Required * 2
		''' <summary>���l�̂�</summary>
		[Decimal] = Numeric * 2
		''' <summary>�����t���l�̂�</summary>
		WithNumericSigned = [Decimal] * 2
		''' <summary>��</summary>
		Zip = WithNumericSigned * 2
		''' <summary>�d�b�ԍ�</summary>
		Tel = Zip * 2
		''' <summary>������ɑS�p�������܂܂�Ă��邩</summary>
		InJis = Tel * 2
		''' <summary>�ŏ�����</summary>
		LenghtMin = InJis * 2
		''' <summary>�ő包��</summary>
		LenghtMax = LenghtMin * 2
		''' <summary>�ŏ������i���p 1 �o�C�g�A�S�p 2 �o�C�g�Ƃ��āj</summary>
		LenghtMinB = LenghtMax * 2
		''' <summary>�ő包���i���p 1 �o�C�g�A�S�p 2 �o�C�g�Ƃ��āj</summary>
		LenghtMaxB = LenghtMinB * 2
		''' <summary>���t</summary>
		DateFormat = LenghtMaxB * 2
		''' <summary>�ŏ����l</summary>
		Minimum = DateFormat * 2
		''' <summary>�ő吔�l</summary>
		Maximum = Minimum * 2
		''' <summary>���p�p��</summary>
		HalfWidthAlphanumeric = Maximum * 2
		''' <summary>Mail</summary>
		Mail = HalfWidthAlphanumeric * 2
		''' <summary>URL</summary>
		URL = Mail * 2
	End Enum

#End Region

	''' <summary>
	''' �l�̌��؃N���X
	''' </summary>
	''' <remarks></remarks>
	Public Class Validator

#Region " Methods "

		''' <summary>
		''' ���؎��s
		''' </summary>
		''' <param name="value">���ؑΏۂ̒l</param>
		''' <param name="validates">���ؓ��e�iOR �ɂĕ��������w��\�j</param>
		''' <param name="min">���ؓ��e�ɂčŏ��l���K�v�ȂƂ��w�肷��</param>
		''' <param name="max">���ؓ��e�ɂčő�l���K�v�ȂƂ��w�肷��</param>
		''' <returns>���،��ʁi�����G���[�����������Ƃ��͕����w�肳���j</returns>
		''' <remarks>
		''' �K�{�`�F�b�N���w�肳��Ă��ăG���[�ƂȂ����Ƃ��́A���̃`�F�b�N���w�肳��Ă����Ƃ��Ă��`�F�b�N���܂���B
		''' �i�K�{�G���[������Ԃ��܂��B�j
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

			' �K�{
			If _IsValidateType(validates, ValidateTypes.Required) Then
				If val.Length = 0 Then
					rc = rc Or ValidateTypes.Required
				End If
			End If
			' �K�{�`�F�b�N�ŃG���[�̂Ƃ��͔�����
			If rc <> ValidateTypes.None Then
				Return rc
			End If
			' �����͎��͂����܂�
			If val.Length = 0 Then
				Return rc
			End If

			' �����̂�, �����t���l�̂�
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

			' ���l�̂�, �����t���l�̂�
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

			' ��
			If _IsValidateType(validates, ValidateTypes.Zip) Then
				If Not Regex.IsMatch(val, "^([0-9]{3}\-?[0-9]{4})?$") Then
					rc = rc Or ValidateTypes.Zip
				End If
			End If

			' �d�b�ԍ�
			If _IsValidateType(validates, ValidateTypes.Tel) Then
				If Not Regex.IsMatch(val, "^([0-9]{1,5}[\-]?[0-9]{1,4}[\-]?[0-9]{3,4})?$") Then
					rc = rc Or ValidateTypes.Tel
				End If
			End If

			' ������ɑS�p�������܂܂�Ă��邩���ׂ�
			If _IsValidateType(validates, ValidateTypes.InJis) Then
				' �S�p�������܂܂�Ă���ꍇ��True�A�����łȂ��ꍇFalse
				If Len(value) <> System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(value) Then
					rc = rc Or ValidateTypes.InJis
				End If
			End If

			' ���p�p��
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

			' �ŏ�����
			If _IsValidateType(validates, ValidateTypes.LenghtMin) And min IsNot Nothing Then
				If val.Length < CInt(min) Then
					rc = rc Or ValidateTypes.LenghtMin
				End If
			End If

			' �ő包��
			If _IsValidateType(validates, ValidateTypes.LenghtMax) And max IsNot Nothing Then
				If val.Length > CInt(max) Then
					rc = rc Or ValidateTypes.LenghtMax
				End If
			End If

			' �ŏ������i���p 1 �o�C�g�A�S�p 2 �o�C�g�Ƃ��āj
			If _IsValidateType(validates, ValidateTypes.LenghtMinB) And min IsNot Nothing Then
				If VBUtil.LenB(val) < CInt(min) Then
					rc = rc Or ValidateTypes.LenghtMinB
				End If
			End If

			' �ő包���i���p 1 �o�C�g�A�S�p 2 �o�C�g�Ƃ��āj
			If _IsValidateType(validates, ValidateTypes.LenghtMaxB) And max IsNot Nothing Then
				If VBUtil.LenB(val) > CInt(max) Then
					rc = rc Or ValidateTypes.LenghtMaxB
				End If
			End If

			' ���t
			Dim dateVal As Date
			If _IsValidateType(validates, ValidateTypes.DateFormat) Then
				If Not Date.TryParse(value, dateVal) Then
					rc = rc Or ValidateTypes.DateFormat
				End If
			End If

			' ���l�A���t�ȊO�͂����܂�
			If Not _IsValidateType(validates, ValidateTypes.Numeric) _
			And Not _IsValidateType(validates, ValidateTypes.WithNumericSigned) _
			And Not _IsValidateType(validates, ValidateTypes.Decimal) _
			And Not _IsValidateType(validates, ValidateTypes.DateFormat) Then
				Return rc
			End If

			' �G���[�ɂȂ��Ă鎞�������܂�
			If _IsValidateType(rc, ValidateTypes.Numeric) _
			OrElse _IsValidateType(rc, ValidateTypes.WithNumericSigned) _
			OrElse _IsValidateType(rc, ValidateTypes.Decimal) _
			OrElse _IsValidateType(rc, ValidateTypes.DateFormat) Then
				Return rc
			End If

			' �ŏ����l
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

			' �ő吔�l
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
		''' ���؎�ʂ̃`�F�b�N
		''' </summary>
		''' <param name="validates">�`�F�b�N�Ώ�</param>
		''' <param name="targetType">�܂܂�Ă��邩�`�F�b�N���錟�؎��</param>
		''' <returns>True: �܂܂�Ă���AFalse: �܂܂�Ă��Ȃ�</returns>
		''' <remarks></remarks>
		Private Function _IsValidateType(ByVal validates As ValidateTypes, ByVal targetType As ValidateTypes) As Boolean
			Return ((validates And targetType) = targetType)
		End Function

		''' <summary>
		''' ���؎�ʂ̃`�F�b�N
		''' </summary>
		''' <param name="validates">�`�F�b�N�Ώ�</param>
		''' <param name="targetType">�܂܂�Ă��邩�`�F�b�N���錟�؎��</param>
		''' <returns>True: �܂܂�Ă���AFalse: �܂܂�Ă��Ȃ�</returns>
		''' <remarks></remarks>
		<Obsolete("�ʂ�IsValidXxxx���g�p���Ă��������B")> _
		Public Function IsValidateType(ByVal validates As ValidateTypes, ByVal targetType As ValidateTypes) As Boolean
			Return ((validates And targetType) = targetType)
		End Function

		''' <summary>
		''' �K�{�G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidRequired(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Required
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' �����̂݃G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidNumeric(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Numeric
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' ���l�̂݃G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidDecimal(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Decimal
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' �����t���l�̂݃G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidWithNumericSigned(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.WithNumericSigned
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' ���G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidZip(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Zip
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' �d�b�ԍ��G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidTel(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Tel
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' ������ɑS�p�������܂܂�Ă��邩�G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidInJis(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.InJis
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' �ŏ������G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidLenghtMin(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.LenghtMin
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' �ő包���G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidLenghtMax(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.LenghtMax
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' �ŏ������i���p 1 �o�C�g�A�S�p 2 �o�C�g�Ƃ��āj�G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidLenghtMinB(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.LenghtMinB
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' �ő包���i���p 1 �o�C�g�A�S�p 2 �o�C�g�Ƃ��āj�G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidLenghtMaxB(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.LenghtMaxB
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' ���t�G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidDateFormat(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.DateFormat
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' �ŏ����l�G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidMinimum(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Minimum
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' �ő吔�l�G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidMaximum(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Maximum
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' ���p�p���G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidHalfWidthAlphanumeric(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.HalfWidthAlphanumeric
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' Mail�G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidMail(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.Mail
			Return Not ((validates And chk) = chk)
		End Function

		''' <summary>
		''' URL�G���[�L��
		''' </summary>
		''' <param name="validates">�`�F�b�N����</param>
		''' <returns>True:����AFalse:�G���[</returns>
		''' <remarks></remarks>
		Public Function IsValidURL(ByVal validates As ValidateTypes) As Boolean
			Dim chk As ValidateTypes = ValidateTypes.URL
			Return Not ((validates And chk) = chk)
		End Function

#End Region

	End Class

End Namespace
