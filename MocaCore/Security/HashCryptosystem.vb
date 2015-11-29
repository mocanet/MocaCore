Imports System.Text
Imports System.Security.Cryptography

Namespace Security

	''' <summary>
	''' ハッシュアルゴリズム を使用してデータの暗号化を行う
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Class HashCryptosystem
		Implements IDisposable

		''' <summary>暗号化するメソッドのデリゲート</summary>
		Protected Delegate Function ComputeHashDelegate(ByVal buffer() As Byte) As Byte()

		''' <summary>暗号化する種別</summary>
		Public Enum ComputeHashType
			''' <summary>MD5 ハッシュ アルゴリズム</summary>
			MD5 = 1
			''' <summary>SH1 ハッシュ アルゴリズム</summary>
			SH1
			''' <summary>SH256 ハッシュ アルゴリズム</summary>
			SH256
			''' <summary>SH384 ハッシュ アルゴリズム</summary>
			SH384
			''' <summary>SH512 ハッシュ アルゴリズム</summary>
			SH512
		End Enum

		''' <summary>暗号化するアルゴリズムインスタンス</summary>
		Private _hashAlgorithm As HashAlgorithm

		''' <summary>暗号化するメソッド</summary>
		Private _computeHash As ComputeHashDelegate

#Region " Constructor/DeConstructor "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <param name="hashType">暗号化する種別</param>
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
		''' リソースの解放
		''' </summary>
		''' <remarks>
		''' </remarks>
		Public Sub Dispose() Implements System.IDisposable.Dispose
			_hashAlgorithm.Clear()
		End Sub

#End Region

		''' <summary>
		''' 暗号化
		''' </summary>
		''' <param name="plainText">対象の文字列</param>
		''' <returns>結果文字列</returns>
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
		''' 暗号化
		''' </summary>
		''' <param name="plainText">対象のバイト配列</param>
		''' <returns>結果バイト配列</returns>
		''' <remarks>
		''' </remarks>
		Public Function Encrypt(ByVal plainText As Byte()) As Byte()
			Return _computeHash(plainText)
		End Function

	End Class

End Namespace
