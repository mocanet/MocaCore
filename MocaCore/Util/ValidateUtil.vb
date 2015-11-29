Imports System.IO

Namespace Util

	''' <summary>
	''' ���؏W
	''' </summary>
	''' <remarks></remarks>
	Public Class ValidateUtil

		''' <summary>
		''' ������ɑS�p�������܂܂�Ă��邩���ׂ�B
		''' </summary>
		''' <param name="Value">���ׂ�Ώۂ̕�����B</param>
		''' <returns>�S�p�������܂܂�Ă���ꍇ��True�A�����łȂ��ꍇFalse�B</returns>
		''' <remarks></remarks>
		Public Shared Function IsInJis(ByVal value As String) As Boolean
			Dim length As Integer

			length = System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(value)

			Return Len(value) <> length
		End Function

		''' <summary>
		''' Single�̔�r
		''' </summary>
		''' <param name="valueA">��r��</param>
		''' <param name="valueB">��r��</param>
		''' <param name="delta">���e�\�ȍ�</param>
		''' <returns></returns>
		''' <remarks>
		''' System ���O��Ԃɂ��� Math �N���X�� Abs ���\�b�h���g�p���āA2 �̐��l�̐�ΓI�ȍ����v�Z���܂��B
		''' ���e�ł���ő�̍������߂܂��B2 �̐��l�̍��������菬�����ꍇ�A2 �������ł���ƌ��Ȃ��Ă����p��̖�肪�Ȃ��悤�Ȓl�ɂ��Ă��������B
		''' ���̐�Βl�Ƌ��e�\�ȍ����r���܂��B
		''' </remarks>
		Public Shared Function IsSngEqules(ByVal valueA As Single, ByVal valueB As Single, Optional ByVal delta As Single = 0.0000001) As Boolean
			Dim absoluteDifference As Double = Math.Abs(valueA - valueB)
			Dim practicallyEqual As Boolean = (absoluteDifference < delta)

			Return practicallyEqual
		End Function

		''' <summary>
		''' �t�@�C�����Ƃ��Ďg�p�o���Ȃ��֎~�������u�Q�v�i�A���_�[�o�[�j�Œu�����܂��B
		''' </summary>
		''' <param name="filename">�t�@�C����</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function ValidFileName(ByVal filename As String) As String
			Return ValidFileName(filename, "_")
		End Function

		''' <summary>
		''' �t�@�C�����Ƃ��Ďg�p�o���Ȃ��֎~�������w�肳�ꂽ������Œu�����܂��B
		''' </summary>
		''' <param name="filename">�t�@�C����</param>
		''' <param name="replaceString">�u�����镶����</param>
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
		''' �V�[�g���Ƃ��Ďg�p�o���Ȃ��֎~�������u�Q�v�i�A���_�[�o�[�j�Œu����31�����ȓ��ɂ��܂��B
		''' </summary>
		''' <param name="sheetName">�V�[�g��</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function ValidExcelSheetName(ByVal sheetName As String) As String
			Return ValidExcelSheetName(sheetName, "_")
		End Function

		''' <summary>
		''' �V�[�g���Ƃ��Ďg�p�o���Ȃ��֎~�������w�肳�ꂽ������Œu����31�����ȓ��ɂ��܂��B
		''' </summary>
		''' <param name="sheetName">�V�[�g��</param>
		''' <param name="replaceString">�u�����镶����</param>
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
