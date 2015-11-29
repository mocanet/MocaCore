Imports System.Security.Cryptography

Namespace Security

	''' <summary>
	''' ���J���Í����@�ňÍ�������
	''' </summary>
	''' <remarks>
	''' ������
	''' </remarks>
	Public Class PublicKeyCryptosystem

		''' <summary>�Í���������</summary>
		Public Enum AlgorithmType
			''' <summary>RSA �A���S���Y��</summary>
			RSA = 1
			''' <summary>DSA �A���S���Y��</summary>
			DSA
		End Enum

		''' <summary>�Í�������A���S���Y���C���X�^���X</summary>
		Private _asymmetricAlgorithm As AsymmetricAlgorithm

#Region " Constructor/DeConstructor "

		''' <summary>
		''' �f�t�H���g�R���X�g���N�^
		''' </summary>
		''' <param name="hashType">�Í���������</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal hashType As AlgorithmType)
			Select Case hashType
				Case AlgorithmType.RSA
					_asymmetricAlgorithm = New RSACryptoServiceProvider
				Case AlgorithmType.DSA
					_asymmetricAlgorithm = New DSACryptoServiceProvider
			End Select
		End Sub

#End Region

	End Class

End Namespace
