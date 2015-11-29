Imports System.Security.Cryptography

Namespace Security

	''' <summary>
	''' 公開鍵暗号方法で暗号化する
	''' </summary>
	''' <remarks>
	''' 未実装
	''' </remarks>
	Public Class PublicKeyCryptosystem

		''' <summary>暗号化する種別</summary>
		Public Enum AlgorithmType
			''' <summary>RSA アルゴリズム</summary>
			RSA = 1
			''' <summary>DSA アルゴリズム</summary>
			DSA
		End Enum

		''' <summary>暗号化するアルゴリズムインスタンス</summary>
		Private _asymmetricAlgorithm As AsymmetricAlgorithm

#Region " Constructor/DeConstructor "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <param name="hashType">暗号化する種別</param>
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
