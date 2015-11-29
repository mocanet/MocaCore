Imports System.Text
Imports System.Security.Cryptography

Namespace Security

	''' <summary>
	''' �n�b�V���A���S���Y�� ���g�p���ăf�[�^�̈Í������s��
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class HashCryptosystem
		Implements IDisposable

		''' <summary>�Í������郁�\�b�h�̃f���Q�[�g</summary>
		Protected Delegate Function ComputeHashDelegate(ByVal buffer() As Byte) As Byte()

		''' <summary>�Í���������</summary>
		Public Enum ComputeHashType
			''' <summary>MD5 �n�b�V�� �A���S���Y��</summary>
			MD5 = 1
			''' <summary>SH1 �n�b�V�� �A���S���Y��</summary>
			SH1
			''' <summary>SH256 �n�b�V�� �A���S���Y��</summary>
			SH256
			''' <summary>SH384 �n�b�V�� �A���S���Y��</summary>
			SH384
			''' <summary>SH512 �n�b�V�� �A���S���Y��</summary>
			SH512
		End Enum

		''' <summary>�Í�������A���S���Y���C���X�^���X</summary>
		Private _hashAlgorithm As HashAlgorithm

		''' <summary>�Í������郁�\�b�h</summary>
		Private _computeHash As ComputeHashDelegate

#Region " Constructor/DeConstructor "

		''' <summary>
		''' �f�t�H���g�R���X�g���N�^
		''' </summary>
		''' <param name="hashType">�Í���������</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal hashType As ComputeHashType)
			Select Case hashType
				Case ComputeHashType.MD5
					_hashAlgorithm = New MD5CryptoServiceProvider
				Case ComputeHashType.SH1
					_hashAlgorithm = New SHA1CryptoServiceProvider
				Case ComputeHashType.SH256
					_hashAlgorithm = New SHA256Managed
				Case ComputeHashType.SH384
					_hashAlgorithm = New SHA384Managed
				Case ComputeHashType.SH512
					_hashAlgorithm = New SHA512Managed
			End Select
			_computeHash = New ComputeHashDelegate(AddressOf _hashAlgorithm.ComputeHash)
		End Sub

#End Region

#Region " Implements IDisposable "

		''' <summary>
		''' ���\�[�X�̉��
		''' </summary>
		''' <remarks>
		''' </remarks>
		Public Sub Dispose() Implements System.IDisposable.Dispose
			_hashAlgorithm.Clear()
		End Sub

#End Region

		''' <summary>
		''' �Í���
		''' </summary>
		''' <param name="plainText">�Ώۂ̕�����</param>
		''' <returns>���ʕ�����</returns>
		''' <remarks>
		''' </remarks>
		Public Function Encrypt(ByVal plainText As String) As String
			If plainText Is Nothing Then
				plainText = String.Empty
			End If

			Dim data As Byte()
			Dim result As Byte()

			data = Encoding.UTF8.GetBytes(plainText)
			result = Encrypt(data)

			Return Convert.ToBase64String(result)
		End Function

		''' <summary>
		''' �Í���
		''' </summary>
		''' <param name="plainText">�Ώۂ̃o�C�g�z��</param>
		''' <returns>���ʃo�C�g�z��</returns>
		''' <remarks>
		''' </remarks>
		Public Function Encrypt(ByVal plainText As Byte()) As Byte()
			Return _computeHash(plainText)
		End Function

	End Class

End Namespace
