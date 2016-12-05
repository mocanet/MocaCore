
Imports Microsoft.Win32
Imports System.ComponentModel

Namespace Db

	''' <summary>
	''' ODBC設定の管理
	''' </summary>
	''' <remarks>
	''' ODBCデータソース設定の管理をします。
	''' </remarks>
	Public Class OdbcSetting

		''' <summary>データソースのシステムキー値</summary>
		Private Const C_DSN_SYS As String = "CONFIGSYSDSN"
		''' <summary>データソースのユーザーキー値</summary>
		Private Const C_DSN_USR As String = "CONFIGDSN"
		''' <summary>データソースのレジストリキー値</summary>
		Private Const C_DSN_REG_KEY As String = "SOFTWARE\ODBC\ODBC.INI\"

		''' <summary>
		''' 登録の種類
		''' </summary>
		''' <remarks></remarks>
		Public Enum RegistrationType As Integer
			System = 0
			User
		End Enum

		''' <summary>
		''' 利用出来るプロバイダの種類
		''' </summary>
		''' <remarks></remarks>
		Public Enum ProviderType As Integer
			SQLServer
		End Enum

		''' <summary>登録の種類</summary>
		Private _dsnRegist As RegistrationType
		''' <summary>プロバイダの種類</summary>
		Private _dbProvider As ProviderType
		''' <summary>プロバイダー名称</summary>
		Private _dsnName As String
		''' <summary>説明</summary>
		Private _dsnDescription As String
		''' <summary>サーバー名</summary>
		Private _dbServer As String
		''' <summary>データベース名</summary>
		Private _dbName As String
		''' <summary>データベース接続ユーザー</summary>
		Private _dbUser As String
		''' <summary>データベース接続ユーザーパスワード</summary>
		Private _dbUserPassword As String

#Region " プロパティ "

		''' <summary>
		''' 登録の種類
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Browsable(True) _
		, Category("ODBC") _
		, Description("DSN Registration Type.")> _
		Public Property Registration() As RegistrationType
			Get
				Return _dsnRegist
			End Get
			Set(ByVal value As RegistrationType)
				_dsnRegist = value
			End Set
		End Property

		''' <summary>
		''' 登録の種類
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Browsable(True) _
		, Category("ODBC") _
		, Description("DSN Registration Type.")> _
		Public ReadOnly Property RegistrationName() As String
			Get
				Select Case _dsnRegist
					Case RegistrationType.System
						Return C_DSN_SYS
					Case RegistrationType.User
						Return C_DSN_USR
				End Select

				Return String.Empty
			End Get
		End Property

		''' <summary>
		''' プロバイダー
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Browsable(True) _
		, Category("ODBC") _
		, Description("Provider to connect it with data base.")> _
		Public Property Provider() As ProviderType
			Get
				Return _dbProvider
			End Get
			Set(ByVal value As ProviderType)
				_dbProvider = value
			End Set
		End Property

		''' <summary>
		''' プロバイダー名称
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Browsable(False) _
		, Category("ODBC") _
		, Description("Provider to connect it with data base.")> _
		Public ReadOnly Property ProviderName() As String
			Get
				Select Case _dbProvider
					Case ProviderType.SQLServer
						Return "SQL Server"
				End Select

				Return String.Empty
			End Get
		End Property

		''' <summary>
		''' DSN
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Browsable(True) _
		, Category("ODBC") _
		, Description("Data Source Name")> _
		Public Property DSN() As String
			Get
				Return _dsnName
			End Get
			Set(ByVal value As String)
				_dsnName = value
			End Set
		End Property

		''' <summary>
		''' 説明
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Browsable(True) _
		, Category("ODBC") _
		, Description("Data Source Description")> _
		Public Property DsnDescription() As String
			Get
				Return _dsnDescription
			End Get
			Set(ByVal value As String)
				_dsnDescription = value
			End Set
		End Property

		''' <summary>
		''' サーバー名
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Category("ODBC") _
		  , Description("Server name at connection destination.")> _
		Public Property ServerName() As String
			Get
				Return _dbServer
			End Get
			Set(ByVal value As String)
				_dbServer = value
			End Set
		End Property

		''' <summary>
		''' データベース名
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Category("ODBC") _
		, Description("Database name at connection destination.")> _
		Public Property DatabaseName() As String
			Get
				Return _dbName
			End Get
			Set(ByVal value As String)
				_dbName = value
			End Set
		End Property

		''' <summary>
		''' データベース接続ユーザー
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Category("ODBC") _
		, Description("Connected user name.")> _
		Public Property UserName() As String
			Get
				Return _dbUser
			End Get
			Set(ByVal value As String)
				_dbUser = value
			End Set
		End Property

		''' <summary>
		''' データベース接続ユーザーパスワード
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Category("ODBC") _
		, Description("Password of user who connects it.")> _
		Public Property Password() As String
			Get
				Return _dbUserPassword
			End Get
			Set(ByVal value As String)
				_dbUserPassword = value
			End Set
		End Property

#End Region

		''' <summary>
		''' 設定が存在するか返す
		''' </summary>
		''' <param name="dsn"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function IsExisting(ByVal dsn As String) As Boolean
			Dim regkey As RegistryKey

			regkey = Registry.LocalMachine.OpenSubKey(C_DSN_REG_KEY & dsn)

			Return (Not regkey Is Nothing)
		End Function

		''' <summary>
		''' 追加する
		''' </summary>
		''' <remarks></remarks>
		Public Sub Add()
			Dim regkey As RegistryKey

			execODBCconf()

			regkey = Registry.LocalMachine.CreateSubKey(C_DSN_REG_KEY & _dsnName)
			regkey.SetValue("LastUser", _dbUser)
			regkey.SetValue("Password", _dbUserPassword)
		End Sub

		''' <summary>
		''' 修正する
		''' </summary>
		''' <remarks></remarks>
		Public Sub Modify()
			Dim regkey As RegistryKey

			regkey = Registry.LocalMachine.OpenSubKey(C_DSN_REG_KEY & _dsnName, True)
			If regkey Is Nothing Then
				Exit Sub
			End If

			regkey.SetValue("SERVER", _dbServer)
			regkey.SetValue("Database", _dbName)
			regkey.SetValue("LastUser", _dbUser)
			regkey.SetValue("Password", _dbUserPassword)
		End Sub

		''' <summary>
		''' ODBC設定コマンド実行
		''' </summary>
		''' <remarks></remarks>
		Protected Sub execODBCconf()
			Dim psi As New System.Diagnostics.ProcessStartInfo

			'ComSpecのパスを取得する
			psi.FileName = System.Environment.GetEnvironmentVariable("ComSpec")

			'出力を読み取れるようにする
			psi.RedirectStandardInput = False
			psi.RedirectStandardOutput = True
			psi.UseShellExecute = False

			'ウィンドウを表示しないようにする
			psi.CreateNoWindow = True

			'コマンドラインを指定（"/c"は実行後閉じるために必要）
			psi.Arguments = "/c ODBCconf /A {" & makeCommandParam() & "}"

			'起動
			Dim p As Process = Process.Start(psi)

			'出力結果を格納
			Dim results As String = p.StandardOutput.ReadToEnd

			'WaitForExitはReadToEndの後である必要がある(親プロセス、子プロセスでブロック防止のため)
			p.WaitForExit()

			p.Close() 'プロセス開放
		End Sub

		''' <summary>
		''' ODBC設定コマンドのパラメータ文字列作成
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function makeCommandParam() As String
			Return String.Format("{0} ""{1}"" ""DSN={2}|Description={3}|Database={4}|SERVER={5}|Trusted_Connection=no""" _
			  , RegistrationName _
			, ProviderName _
			, _dsnName _
			, _dsnDescription _
			, _dbName _
			, _dbServer)
		End Function

	End Class

End Namespace
