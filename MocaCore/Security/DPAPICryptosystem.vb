Imports System.Text
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports Moca.Exceptions

Namespace Security

	''' <summary>
	''' DPAPI を使用してデータの暗号化と解読を行う
	''' </summary>
	''' <remarks>
	''' コンピュータまたはユーザーベースのキーストアを使用してデータを暗号化および解読するために、
	''' DPAPI (データ保護 API) への呼び出しをカプセル化するマネージクラスライブラリです。
	''' 
	''' ユーザー ストア では、DPAPI 関数を呼び出すコードに関連付けられたユーザー アカウントのパスワードを利用して、暗号化キーを生成します。
	''' したがって、オペレーティング システムがキーを管理するため、アプリケーションではキーの管理が不要となります。
	''' 
	''' コンピュータ ストア では、ユーザーは関係なくコンピュータで暗号化キーを管理します。
	''' エントロピ パラメータを使用しないと、すべてのユーザーがデータの暗号化を解除できるため、セキュリティが弱くなります。
	''' エントロピとは、機密情報の解読をより困難にするランダム値です。このパラメータはアプリケーションにて管理する必要があります。
	''' </remarks>
	Public Class DPAPICryptosystem

#Region " DPAPI Declarations from PInvoke Site"

		<Flags()> _
		Public Enum CryptProtectPromptFlags
			'prompt on unprotect
			CRYPTPROTECT_PROMPT_ON_UNPROTECT = &H1
			'prompt on protect
			CRYPTPROTECT_PROMPT_ON_PROTECT = &H2
		End Enum

		Private Const CRYPTPROTECT_UI_FORBIDDEN As Integer = 1
		Private Const CRYPTPROTECT_LOCAL_MACHINE As Integer = 4

		<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
		Structure DATA_BLOB
			Public cbData As Integer
			Public pbData As IntPtr
		End Structure

		<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
		Structure CRYPTPROTECT_PROMPTSTRUCT
			Public cbSize As Integer
			Public dwPromptFlags As CryptProtectPromptFlags
			Public hwndApp As IntPtr
			Public szPrompt As String
		End Structure

		''' <summary>
		''' 暗号化
		''' </summary>
		''' <param name="pDataIn"></param>
		''' <param name="szDataDescr"></param>
		''' <param name="pOptionalEntropy"></param>
		''' <param name="pvReseved"></param>
		''' <param name="pPromptStruct"></param>
		''' <param name="dwFlags"></param>
		''' <param name="pDataOut"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		<DllImport("Crypt32.dll", SetLastError:=True, CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
		Private Shared Function CryptProtectData( _
   ByRef pDataIn As DATA_BLOB, _
   ByVal szDataDescr As String, _
   ByRef pOptionalEntropy As DATA_BLOB, _
   ByVal pvReseved As IntPtr, _
   ByRef pPromptStruct As CRYPTPROTECT_PROMPTSTRUCT, _
   ByVal dwFlags As Integer, _
   ByRef pDataOut As DATA_BLOB _
   ) As Boolean
		End Function

		''' <summary>
		''' 復号化
		''' </summary>
		''' <param name="pDataIn"></param>
		''' <param name="szDataDescr"></param>
		''' <param name="pOptionalEntropy"></param>
		''' <param name="pvReserved"></param>
		''' <param name="pPromptStruct"></param>
		''' <param name="dwFlags"></param>
		''' <param name="pDataOut"></param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		<DllImport("Crypt32.dll", SetLastError:=True, CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
		Private Shared Function CryptUnprotectData( _
	ByRef pDataIn As DATA_BLOB, _
	ByVal szDataDescr As String, _
	ByRef pOptionalEntropy As DATA_BLOB, _
	ByVal pvReserved As IntPtr, _
	ByRef pPromptStruct As CRYPTPROTECT_PROMPTSTRUCT, _
	ByVal dwFlags As Integer, _
	ByRef pDataOut As DATA_BLOB _
   ) As Boolean
		End Function

#End Region

		''' <summary>暗号化方法</summary>
		Public Enum StoreType
			''' <summary>コンピュータ単位</summary>
			USE_MACHINE_STORE = 1
			''' <summary>ユーザー単位</summary>
			USE_USER_STORE
		End Enum

		''' <summary>暗号化方法</summary>
		Private _store As StoreType

#Region " Constructor/DeConstructor "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="storeValue">コンピュータストアかユーザーストアを指定</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal storeValue As StoreType)
			_store = storeValue
		End Sub

#End Region

#Region " 暗号化 "

		''' <summary>
		''' 暗号化
		''' </summary>
		''' <param name="plainText">対象となる文字列</param>
		''' <returns>暗号化した結果文字列</returns>
		''' <remarks>
		''' </remarks>
		Public Function Encrypt(ByVal plainText As String) As String
			If plainText Is Nothing Then
				plainText = String.Empty
			End If
			Return Encrypt(plainText, String.Empty)
		End Function

		''' <summary>
		''' 暗号化
		''' </summary>
		''' <param name="plainText">対象となる文字列</param>
		''' <param name="optionalEntropy">情報の解読をより困難にするキー（ランダム値）</param>
		''' <returns>暗号化した結果文字列</returns>
		''' <remarks>
		''' optionalEntropyはアプリケーションにて管理する必要があります。
		''' </remarks>
		Public Function Encrypt(ByVal plainText As String, ByVal optionalEntropy As String) As String
			If plainText Is Nothing Then
				plainText = String.Empty
			End If
			If optionalEntropy Is Nothing Then
				optionalEntropy = String.Empty
			End If
			Return Convert.ToBase64String(Encrypt(Encoding.UTF8.GetBytes(plainText), Encoding.UTF8.GetBytes(optionalEntropy)))
		End Function

		''' <summary>
		''' 暗号化
		''' </summary>
		''' <param name="plainText">対象となるバイト配列</param>
		''' <param name="optionalEntropy">情報の解読をより困難にするキー（ランダム値）</param>
		''' <returns>暗号化した結果バイト配列</returns>
		''' <remarks>
		''' optionalEntropyはアプリケーションにて管理する必要があります。
		''' </remarks>
		Public Function Encrypt(ByVal plainText As Byte(), ByVal optionalEntropy As Byte()) As Byte()
			Dim plainTextBlob As DATA_BLOB = New DATA_BLOB
			Dim cipherTextBlob As DATA_BLOB = New DATA_BLOB
			Dim entropyBlob As DATA_BLOB = New DATA_BLOB

			Dim prompt As CRYPTPROTECT_PROMPTSTRUCT = New CRYPTPROTECT_PROMPTSTRUCT
			_initPromptstruct(prompt)

			Try
				Try
					_allocBlob(plainText, plainTextBlob)
				Catch ex As Exception
					Throw New MocaRuntimeException(ex, "プレーン テキスト バッファを割り当てることができません。")
				End Try

				Dim dwFlags As Integer

				If StoreType.USE_MACHINE_STORE = _store Then
					' エントロピを提供する場合はコンピュータ ストアを使用する
					dwFlags = CRYPTPROTECT_LOCAL_MACHINE Or CRYPTPROTECT_UI_FORBIDDEN
					Try
						_allocBlob(optionalEntropy, entropyBlob)
					Catch ex As Exception
						Throw New MocaRuntimeException(ex, "エントロピ データ バッファを割り当てることができません")
					End Try
				Else
					' ユーザー ストアの使用
					dwFlags = CRYPTPROTECT_UI_FORBIDDEN
				End If

				Dim result As Boolean = False
				result = CryptProtectData(plainTextBlob, String.Empty, entropyBlob, IntPtr.Zero, prompt, dwFlags, cipherTextBlob)
				If Not result Then
					Throw New MocaRuntimeException(New Win32Exception(Marshal.GetLastWin32Error()), "暗号化に失敗しました。 ")
				End If

				Dim cipherText(cipherTextBlob.cbData) As Byte

				Marshal.Copy(cipherTextBlob.pbData, cipherText, 0, cipherTextBlob.cbData)

				Return cipherText
			Catch ex As Exception
				Throw New MocaRuntimeException(ex, "暗号化中に例外が発生しました。 ")
			Finally
				If Not (plainTextBlob.pbData.Equals(IntPtr.Zero)) Then
					Marshal.FreeHGlobal(plainTextBlob.pbData)
				End If
				If Not (cipherTextBlob.pbData.Equals(IntPtr.Zero)) Then
					Marshal.FreeHGlobal(cipherTextBlob.pbData)
				End If
				If Not (entropyBlob.pbData.Equals(IntPtr.Zero)) Then
					Marshal.FreeHGlobal(entropyBlob.pbData)
				End If
			End Try
		End Function

#End Region

#Region " 復号化 "

		''' <summary>
		''' 復号化
		''' </summary>
		''' <param name="cipherText">暗号化された文字列</param>
		''' <returns>結果文字列</returns>
		''' <remarks>
		''' </remarks>
		Public Function Decrypt(ByVal cipherText As String) As String
			Return Decrypt(cipherText, String.Empty)
		End Function

		''' <summary>
		''' 復号化
		''' </summary>
		''' <param name="cipherText">暗号化された文字列</param>
		''' <param name="optionalEntropy">情報の解読をより困難にするキー（ランダム値）</param>
		''' <returns>結果文字列</returns>
		''' <remarks>
		''' optionalEntropyはアプリケーションにて管理する必要があります。
		''' </remarks>
		Public Function Decrypt(ByVal cipherText As String, ByVal optionalEntropy As String) As String
			If cipherText Is Nothing Then
				cipherText = String.Empty
			End If
			If optionalEntropy Is Nothing Then
				optionalEntropy = String.Empty
			End If
			Return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(cipherText), Encoding.UTF8.GetBytes(optionalEntropy))).Replace(Chr(0), String.Empty)
		End Function

		''' <summary>
		''' 復号化
		''' </summary>
		''' <param name="cipherText">暗号化されたバイト配列</param>
		''' <param name="optionalEntropy">情報の解読をより困難にするキー（ランダム値）</param>
		''' <returns>結果バイト配列</returns>
		''' <remarks>
		''' optionalEntropyはアプリケーションにて管理する必要があります。
		''' </remarks>
		Public Function Decrypt(ByVal cipherText As Byte(), ByVal optionalEntropy As Byte()) As Byte()
			Dim plainTextBlob As DATA_BLOB = New DATA_BLOB
			Dim cipherBlob As DATA_BLOB = New DATA_BLOB
			Dim entropyBlob As DATA_BLOB = New DATA_BLOB

			Dim prompt As CRYPTPROTECT_PROMPTSTRUCT = New CRYPTPROTECT_PROMPTSTRUCT
			_initPromptstruct(prompt)

			Try
				Try
					_allocBlob(cipherText, cipherBlob)
				Catch ex As Exception
					Throw New MocaRuntimeException(ex, "暗号テキスト バッファを割り当てることができません。")
				End Try

				Dim dwFlags As Integer

				If StoreType.USE_MACHINE_STORE = _store Then
					' エントロピを提供する場合はコンピュータ ストアを使用する
					dwFlags = CRYPTPROTECT_LOCAL_MACHINE Or CRYPTPROTECT_UI_FORBIDDEN
					Try
						_allocBlob(optionalEntropy, entropyBlob)
					Catch ex As Exception
						Throw New MocaRuntimeException(ex, "エントロピ データ バッファを割り当てることができません")
					End Try
				Else
					' ユーザー ストアの使用
					dwFlags = CRYPTPROTECT_UI_FORBIDDEN
				End If

				Dim result As Boolean = False
				result = CryptUnprotectData(cipherBlob, String.Empty, entropyBlob, IntPtr.Zero, prompt, dwFlags, plainTextBlob)
				If Not result Then
					Throw New MocaRuntimeException(New Win32Exception(Marshal.GetLastWin32Error()), "解読に失敗しました。 ")
				End If

				Dim plainText(plainTextBlob.cbData) As Byte

				Marshal.Copy(plainTextBlob.pbData, plainText, 0, plainTextBlob.cbData)

				Return plainText

			Catch ex As Exception
				Throw New MocaRuntimeException(ex, "解読中に例外が発生しました。")
			Finally
				' blob とエントロピを解放する
				If Not cipherBlob.pbData.Equals(IntPtr.Zero) Then
					Marshal.FreeHGlobal(cipherBlob.pbData)
				End If
				If Not entropyBlob.pbData.Equals(IntPtr.Zero) Then
					Marshal.FreeHGlobal(entropyBlob.pbData)
				End If
			End Try
		End Function

#End Region

		''' <summary>
		''' 初期化
		''' </summary>
		''' <param name="ps"></param>
		''' <remarks>
		''' </remarks>
		Private Sub _initPromptstruct(ByRef ps As CRYPTPROTECT_PROMPTSTRUCT)
			ps.cbSize = Marshal.SizeOf(GetType(CRYPTPROTECT_PROMPTSTRUCT))
			ps.dwPromptFlags = 0
			ps.hwndApp = IntPtr.Zero
			ps.szPrompt = Nothing
		End Sub

		''' <summary>
		''' 暗号化データの領域割り当て
		''' </summary>
		''' <param name="data">対象データバイト配列</param>
		''' <param name="blob">割当てた領域</param>
		''' <remarks>
		''' </remarks>
		Private Sub _allocBlob(ByVal data As Byte(), ByRef blob As DATA_BLOB)
			' dataが NULL の時は、何かを割り当てる
			If data Is Nothing Then
				data = New Byte(0) {}
			End If

			blob.pbData = Marshal.AllocHGlobal(data.Length)

			If blob.pbData.Equals(IntPtr.Zero) Then
				Throw New MocaRuntimeException("バッファを割り当てることができません。")
			End If

			blob.cbData = data.Length
			Marshal.Copy(data, 0, blob.pbData, data.Length)
		End Sub

	End Class

End Namespace
