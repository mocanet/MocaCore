
Imports System.IO
Imports System.Net

Namespace Net

	''' <summary>
	''' FTPクライアント
	''' </summary>
	''' <remarks>
	''' <see cref="FtpWebRequest"/>を使ってFTP操作します。
	''' </remarks>
	Public Class FtpClient

#Region " Property "

		''' <summary>FTPサーバー名</summary>
		Public Property Server() As String
		''' <summary>接続ユーザー名</summary>
		Public Property UserName() As String
		''' <summary>接続ユーザーのパスワード</summary>
		Public Property Password() As String

		''' <summary>接続維持の有無</summary>
		''' <remarks>デフォルトは維持しない</remarks>
		Public Property KeepAlive() As Boolean
		''' <summary>転送モード</summary>
		''' <remarks>デフォルトはASCIIモード</remarks>
		Public Property UseBinary() As Boolean
		''' <summary>PASVモード</summary>
		''' <remarks>デフォルトはPASVモードではない</remarks>
		Public Property UsePassive() As Boolean
		''' <summary>タイムアウト時間</summary>
		''' <remarks>デフォルトは１０</remarks>
		Public Property RequestTimeout() As Integer

#End Region

#Region " コンストラクタ "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			Me.KeepAlive = False
			Me.UseBinary = False
			Me.UsePassive = False
			Me.RequestTimeout = 10
		End Sub

#End Region
#Region " Method "

		''' <summary>
		''' アップロード
		''' </summary>
		''' <param name="localFileName">ローカルファイルフルパス</param>
		''' <param name="serverDir">サーバー上のディレクトリ</param>
		''' <param name="filename">サーバー上のファイル名</param>
		''' <returns><see cref="FtpWebResponse"></see></returns>
		''' <remarks></remarks>
		Public Function UploadFile(ByVal localFileName As String, ByVal serverDir As String, ByVal filename As String) As FtpWebResponse
			Dim req As FtpWebRequest = CType(WebRequest.Create("ftp://" & Me.Server & serverDir & filename), FtpWebRequest)
			req.Credentials = New NetworkCredential(Me.UserName, Me.Password)
			req.Method = WebRequestMethods.Ftp.UploadFile
			' 要求の完了後に接続を閉じる
			req.KeepAlive = Me.KeepAlive
			' ASCIIモードで転送する
			req.UseBinary = Me.UseBinary
			' PASVモードを無効にする
			req.UsePassive = Me.UsePassive
			' タイムアウト時間
			req.Timeout = Me.RequestTimeout

			' Upload
			Using st As Stream = req.GetRequestStream()
				Using fs As New FileStream(localFileName, FileMode.Open)
					Dim buf(1024) As Byte
					Dim count As Integer = 0

					Do
						count = fs.Read(buf, 0, buf.Length)
						st.Write(buf, 0, count)
					Loop While count <> 0
				End Using
			End Using

			' FTPサーバーから送信されたステータス
			Using res As FtpWebResponse = CType(req.GetResponse(), FtpWebResponse)
				Return res
			End Using
		End Function

#End Region

	End Class

End Namespace
