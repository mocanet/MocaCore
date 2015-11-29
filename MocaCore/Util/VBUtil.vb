
Imports System.IO
Imports System.Text

Namespace Util

	''' <summary>
	''' VB.NET �֗����\�b�h�W
	''' </summary>
	''' <remarks></remarks>
	Public Class VBUtil

		''' <summary>���K�\���̃��^�����W</summary>
		Private Const C_REGEX_META As String = ".,^,$,[,],*,+,?,|,(,)"
		''' <summary>���K�\���̃��^�����W�z��</summary>
		Private Shared _regexMeata() As String = C_REGEX_META.Split(CChar(","))

		''' <summary>
		''' ���ݓW�J����Ă���f�B���N�g���̃J�����g�p�X��Ԃ�
		''' </summary>
		''' <returns>�J�����g�p�X</returns>
		''' <remarks>
		''' VB6 �ȑO�ł������� App.Path �Ɠ����ł��B
		''' </remarks>
		Public Shared Function AppPath() As String
			Return My.Application.Info.DirectoryPath
		End Function

		''' <summary>
		''' ���ݓW�J����Ă���f�B���N�g���̃J�����g�p�X��Ԃ�
		''' </summary>
		''' <param name="value">�J�����g�p�X�ɕt������p�X</param>
		''' <returns>�J�����g�p�X</returns>
		''' <remarks>
		''' VB6 �ȑO�ł������� App.Path �Ɠ����ł����A�w�肳�ꂽ�p�X��t�������`�Ŗ߂��܂��B
		''' </remarks>
		Public Shared Function AppPath(ByVal value As String) As String
			Return Path.Combine(My.Application.Info.DirectoryPath, value)
		End Function

		''' <summary>
		''' �w�肳�ꂽ�t�H���_�����݂��邩�𔻒肷��
		''' </summary>
		''' <param name="path">�ΏۂƂȂ�t�H���_</param>
		''' <returns>True:���݂���AFalse:���݂��Ȃ�</returns>
		''' <remarks>
		''' </remarks>
		Public Shared Function ExistsDir(ByVal path As String) As Boolean
			Return ExistsDir(path, False)
		End Function

		''' <summary>
		''' �w�肳�ꂽ�t�H���_�����݂��邩�𔻒肵�A���݂��Ȃ����͍쐬���邩�ǂ����w��ł���
		''' </summary>
		''' <param name="path">�ΏۂƂȂ�t�H���_</param>
		''' <param name="autoMake">���݂��Ȃ��Ƃ��̓���<br/>True:�쐬����AFalse:�쐬���Ȃ�</param>
		''' <returns>True:���݂���AFalse:���݂��Ȃ�</returns>
		''' <remarks>
		''' </remarks>
		Public Shared Function ExistsDir(ByVal path As String, ByVal autoMake As Boolean) As Boolean
			If Not Directory.Exists(path) Then
				If autoMake Then
					Directory.CreateDirectory(path)
				End If
			End If
			Return Directory.Exists(path)
		End Function

		''' <summary>
		''' ���K�\���̃��^�������G�X�P�[�v����
		''' </summary>
		''' <param name="value">���K�\��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function RegexMeataEscape(ByVal value As String) As String
			For Each meta As String In _regexMeata
				value = value.Replace(meta, "\" & meta)
			Next
			Return value
		End Function

		''' <summary>
		''' �C���[�W���o�C�g�^�̔z��Ɏ擾
		''' </summary>
		''' <param name="filePath"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetImageByte(ByVal filePath As String) As Byte()
			Return File.ReadAllBytes(filePath)
		End Function

		''' <summary>
		''' �萔�l�f�[�^���쐬����ׂ�ConstantDataSet���쐬����
		''' </summary>
		''' <param name="tableName">�萔�l�e�[�u������</param>
		''' <param name="blankRow">�u�����N�s��ǉ����邩�ǂ����i�f�t�H���g�F�쐬�j</param>
		''' <param name="blankValue">�u�����N�s�̒l�i�f�t�H���g�F-1�j</param>
		''' <returns></returns>
		''' <remarks>
		''' <c>blankRow</c> ��True��ݒ肷��ΐ擪�֋�f�[�^����s�����Œǉ����܂��B<br/>
		''' ����͉��L�̒ʂ�ł��B<br/>
		''' <br/>
		''' <list type="table">
		'''  <item>
		'''   <term>Display</term>
		'''   <description>�u�l�F���́v�iText &amp; ":" &amp; Value�j�ɂ����������ێ����܂��B</description>
		'''  </item>
		'''  <item>
		'''   <term>Text</term>
		'''   <description>���̂�ێ����܂��B</description>
		'''  </item>
		'''  <item>
		'''   <term>Value</term>
		'''   <description>�l��ێ����܂��B</description>
		'''  </item>
		'''  <item>
		'''   <term>ValueText</term>
		'''   <description>�l�̕������ێ����܂��B</description>
		'''  </item>
		''' </list>
		''' </remarks>
		Public Shared Function CreateConstantDataSet(ByVal tableName As String, Optional ByVal blankRow As Boolean = True, Optional ByVal blankValue As Object = -1) As ConstantDataSet
			Return New ConstantDataSet(tableName, blankRow, blankValue)
		End Function

		''' <summary>
		''' �񋓌^�̖��̂�Ԃ�
		''' </summary>
		''' <typeparam name="T">�񋓌^</typeparam>
		''' <param name="value">�ΏۂƂȂ�l</param>
		''' <returns>����</returns>
		''' <remarks></remarks>
		Public Shared Function GetEnumName(Of T)(ByVal value As Object) As String
			Return [Enum].GetName(GetType(T), value)
		End Function

#Region "�@LeftB ���\�b�h�@"

		''' <summary>
		''' ������̍��[����w�肵���o�C�g�����̕������Ԃ��܂��B
		''' </summary>
		''' <param name="stTarget">���o�����ɂȂ镶����B</param>
		''' <param name="iByteSize">���o���o�C�g���B</param>
		''' <returns>���[����w�肳�ꂽ�o�C�g�����̕�����B</returns>
		Public Shared Function LeftB(ByVal stTarget As String, ByVal iByteSize As Integer) As String
			Return MidB(stTarget, 1, iByteSize)
		End Function

#End Region

#Region "�@MidB ���\�b�h (+1)�@"

		' ''' <summary>
		' ''' ������̎w�肳�ꂽ�o�C�g�ʒu�ȍ~�̂��ׂĂ̕������Ԃ��܂��B
		' ''' </summary>
		' ''' <param name="stTarget">���o�����ɂȂ镶����B</param>
		' ''' <param name="iStart">���o�����J�n����ʒu�B</param>
		' ''' <returns>�w�肳�ꂽ�o�C�g�ʒu�ȍ~�̂��ׂĂ̕�����B</returns>
		'Public Shared Function MidB(ByVal stTarget As String, ByVal iStart As Integer) As String
		'	Dim hEncoding As Encoding = Encoding.GetEncoding("Shift_JIS")
		'	Dim bBytes As Byte() = hEncoding.GetBytes(stTarget)

		'	Return hEncoding.GetString(bBytes, iStart - 1, bBytes.Length - iStart + 1)
		'End Function

		' ''' <summary>
		' ''' ������̎w�肳�ꂽ�o�C�g�ʒu����A�w�肳�ꂽ�o�C�g�����̕������Ԃ��܂��B
		' ''' </summary>
		' ''' <param name="stTarget">���o�����ɂȂ镶����B</param>
		' ''' <param name="iStart">���o�����J�n����ʒu�B</param>
		' ''' <param name="iByteSize">���o���o�C�g���B</param>
		' ''' <returns>�w�肳�ꂽ�o�C�g�ʒu����w�肳�ꂽ�o�C�g�����̕�����B</returns>
		'Public Shared Function MidB _
		'(ByVal stTarget As String, ByVal iStart As Integer, ByVal iByteSize As Integer) As String
		'	Dim hEncoding As Encoding = Encoding.GetEncoding("Shift_JIS")
		'	Dim bBytes As Byte() = hEncoding.GetBytes(stTarget)

		'	Return hEncoding.GetString(bBytes, iStart - 1, iByteSize)
		'End Function

		''' <summary>
		''' ������̎w�肳�ꂽ�o�C�g�ʒu�ȍ~�̂��ׂĂ̕������Ԃ��܂��B
		''' </summary>
		''' <param name="value">���o�����ɂȂ镶����B</param>
		''' <param name="startPos">���o�����J�n����ʒu�B</param>
		''' <returns>�w�肳�ꂽ�o�C�g�ʒu�ȍ~�̂��ׂĂ̕�����B</returns>
		Public Shared Function MidB(ByVal value As String, ByVal startPos As Integer) As String
			Dim enc As Encoding = Encoding.GetEncoding("Shift_JIS")
			Dim getLength As Integer = enc.GetByteCount(value) - startPos + 1
			Return MidB(value, startPos, getLength)
		End Function

		''' <summary>
		''' ������̎w�肳�ꂽ�o�C�g�ʒu����A�w�肳�ꂽ�o�C�g�����̕������Ԃ��܂��B
		''' </summary>
		''' <param name="value">���o�����ɂȂ镶����B</param>
		''' <param name="startPos">���o�����J�n����ʒu�B</param>
		''' <param name="getLength">���o���o�C�g���B</param>
		''' <returns>�w�肳�ꂽ�o�C�g�ʒu����w�肳�ꂽ�o�C�g�����̕�����B</returns>
		Public Shared Function MidB(ByVal value As String, ByVal startPos As Integer, ByVal getLength As Integer) As String
			If value Is Nothing OrElse value.Length = 0 Then
				Return String.Empty
			End If

			Dim rc As String
			Dim enc As Encoding = Encoding.GetEncoding("Shift_JIS")
			Dim bytes As Byte() = enc.GetBytes(value)
			Dim len As Integer = enc.GetByteCount(value) - startPos + 1
			If getLength > len Then
				getLength = len
			End If

			'���؂蔲�������ʁA�Ō�̂P�o�C�g���S�p�����̔����������ꍇ�A���̔����͐؂�̂Ă�B

			rc = enc.GetString(bytes, startPos - 1, getLength)
			Dim rcLength As Integer = enc.GetByteCount(rc)

			If getLength = rcLength - 1 Then
				Return rc.Substring(0, rc.Length - 1)
			End If

			Return rc
		End Function

#End Region

#Region "�@RightB ���\�b�h�@"

		''' <summary>
		''' ������̉E�[����w�肳�ꂽ�o�C�g�����̕������Ԃ��܂��B
		''' </summary>
		''' <param name="stTarget">���o�����ɂȂ镶����B</param>
		''' <param name="iByteSize">���o���o�C�g���B</param>
		''' <returns>�E�[����w�肳�ꂽ�o�C�g�����̕�����B</returns>
		Public Shared Function RightB(ByVal stTarget As String, ByVal iByteSize As Integer) As String
			Dim hEncoding As Encoding = Encoding.GetEncoding("Shift_JIS")
			Dim bBytes As Byte() = hEncoding.GetBytes(stTarget)

			Return hEncoding.GetString(bBytes, bBytes.Length - iByteSize, iByteSize)
		End Function

#End Region

#Region "�@LenB ���\�b�h�@"

		''' <summary>
		''' ���p 1 �o�C�g�A�S�p 2 �o�C�g�Ƃ��āA�w�肳�ꂽ������̃o�C�g����Ԃ��܂��B
		''' </summary>
		''' <param name="stTarget">�o�C�g���擾�̑ΏۂƂȂ镶����B</param>
		''' <returns>���p 1 �o�C�g�A�S�p 2 �o�C�g�ŃJ�E���g���ꂽ�o�C�g���B</returns>
		Public Shared Function LenB(ByVal stTarget As String) As Integer
			Return Encoding.GetEncoding("Shift_JIS").GetByteCount(stTarget)
		End Function

#End Region

		''' <summary>
		''' ������𕶎��R�[�h��\�������l�ɕϊ�����
		''' </summary>
		''' <param name="targetValue">�ϊ��Ώۂ̕�����</param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Public Shared Function CAsc(ByVal targetValue As String) As String
			Dim beforeData As String
			Dim afterData As String
			Dim chrValue As String
			Dim ii As Integer

			beforeData = targetValue
			afterData = ""
			If Len(beforeData) <> 0& Then
				For ii = 1 To Len(beforeData)
					chrValue = String.Empty
					'chrValue = Hex(Asc(Mid$(beforeData, ii, 1)))
					chrValue = CStr(Asc(Mid$(beforeData, ii, 1)))
					afterData = afterData & chrValue
				Next
			Else
				afterData = "0"
			End If

			Return afterData
		End Function

		Public Shared Function ByteCutString(ByVal str As String, ByVal cutByteLength As Integer) As String
			Dim srcB() As Byte
			Dim encB() As Byte
			Dim enc As Encoding
			Dim rc As String

			' ��������o�C�g�z��ɕϊ�
			srcB = Encoding.Unicode.GetBytes(str)
			' Unicode����V�t�gJIS�ɕϊ�
			enc = Encoding.GetEncoding(932)
			encB = Encoding.Convert(Encoding.Unicode, enc, srcB)

			Dim rcB(encB.Length - cutByteLength) As Byte
			Array.Copy(encB, rcB, rcB.Length)

			rc = Encoding.Unicode.GetString(Encoding.Convert(enc, Encoding.Unicode, rcB))

			Return rc.Substring(0, rc.Length - 1)
		End Function

		''' <summary>
		''' ShiftJIS�ɕϊ��ł��Ȃ��������܂܂�Ă��邩�Ԃ�
		''' </summary>
		''' <param name="val">�Ώۂ̕�����</param>
		''' <returns>True:�܂܂�Ă��Ȃ��AFalse:�܂܂�Ă���</returns>
		''' <remarks></remarks>
		Public Shared Function IsShiftJISOnlyText(ByVal val As String) As Boolean
			Dim encoderFallback As New EncoderExceptionFallback
			Dim decoderFallback As New DecoderExceptionFallback
			Dim sjis As Encoding = Encoding.GetEncoding("Shift_JIS", encoderFallback, decoderFallback)

			Try
				Dim bytes As Byte() = sjis.GetBytes(val)
			Catch ex As EncoderFallbackException
				Return False
			End Try
			Return True

		End Function

		''' <summary>
		''' �C���[�W���o�C�g�֕ϊ�
		''' </summary>
		''' <param name="source"></param>
		''' <param name="format"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function CBytes(ByVal source As System.Drawing.Image, ByVal format As System.Drawing.Imaging.ImageFormat) As Byte()
			Dim blob() As Byte

			' �t�@�C���X�g���[���ŃC���[�W�f�[�^��ǂݍ���
			Using tempStream As System.IO.MemoryStream = New System.IO.MemoryStream()
				' �������X�g���[��(Byte[])��Image�f�[�^��ۑ�
				source.Save(tempStream, format)
				' �������X�g���[������Byte[]�f�[�^���擾
				blob = tempStream.ToArray()
			End Using

			Return blob
		End Function

	End Class

End Namespace
