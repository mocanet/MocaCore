
Imports System.Configuration
Imports System.ComponentModel
Imports System.Data.Common

Imports Moca.Security

Namespace Db

	''' <summary>
	''' 構成ファイルの接続文字列セクション又はDB接続文字列を管理します。
	''' </summary>
	''' <remarks>
	''' 特定のコンピュータ、アプリケーション、またはリソースに適用できる構成ファイルへDB接続文字列を保存したり、読込んだりします。
	''' </remarks>
	Public Class DbSetting

		''' <summary>特定のコンピュータ、アプリケーション、またはリソースに適用できる構成ファイル</summary>
		Private _config As System.Configuration.Configuration

		''' <summary>接続文字列の名称</summary>
		Private _name As String
		''' <summary>プロパイダクラス名</summary>
		Private _providerName As String
		''' <summary>サーバー名</summary>
		Private _server As String
		''' <summary>データベース名</summary>
		Private _database As String
		''' <summary>接続ユーザー名</summary>
		Private _user As String
		''' <summary>接続ユーザーのパスワード</summary>
		Private _password As String

		''' <summary>OleDbプロパイダクラス名</summary>
		Private _oleDbProviderName As String

		''' <summary>接続文字列の名称（カレント保存用）</summary>
		Private _currentName As String

#Region " コンストラクタ "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			_name = String.Empty
			_providerName = String.Empty
			_server = String.Empty
			_database = String.Empty
			_user = String.Empty
			_password = String.Empty
			_currentName = String.Empty
			_oleDbProviderName = String.Empty
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="setting">構成ファイルの接続文字列セクション内の名前付きで単一の接続文字列を表すクラス</param>
		''' <remarks></remarks>
		Public Sub New(ByVal setting As ConnectionStringSettings)
			Me.New()
			moveValues(setting)
		End Sub

#End Region

#Region " プロパティ "

		''' <summary>
		''' 構成ファイルプロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Browsable(False)> _
		Public Property Config() As System.Configuration.Configuration
			Get
				Return _config
			End Get
			Set(ByVal value As System.Configuration.Configuration)
				_config = value
			End Set
		End Property

		''' <summary>
		''' 接続文字列の名称プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Browsable(False)> _
		Public Property Name() As String
			Get
				Return _name
			End Get
			Set(ByVal value As String)
				_name = value
			End Set
		End Property

		''' <summary>
		''' プロパイダクラス名プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Browsable(False) _
		, Category("Database") _
		, Description("Provider to connect it with data base.")> _
		Public Property ProviderName() As String
			Get
				Return _providerName
			End Get
			Set(ByVal value As String)
				_providerName = value
			End Set
		End Property

		''' <summary>
		''' サーバー名プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Category("Database") _
		, Description("Server name at connection destination.")> _
		Public Property Server() As String
			Get
				Return _server
			End Get
			Set(ByVal value As String)
				_server = value
			End Set
		End Property

		''' <summary>
		''' データベース名プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Category("Database") _
		, Description("Database name at connection destination.")> _
		Public Property Database() As String
			Get
				Return _database
			End Get
			Set(ByVal value As String)
				_database = value
			End Set
		End Property

		''' <summary>
		''' 接続ユーザー名プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Category("Database") _
		, Description("Connected user name.")> _
		Public Property User() As String
			Get
				Return _user
			End Get
			Set(ByVal value As String)
				_user = value
			End Set
		End Property

		''' <summary>
		''' 接続ユーザーのパスワードプロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Category("Database") _
		, Description("Password of user who connects it.") _
		, PasswordPropertyText(True)> _
		Public Property Password() As String
			Get
				Return _password
			End Get
			Set(ByVal value As String)
				_password = value
			End Set
		End Property

		''' <summary>
		''' OleDb接続時のプロバイダー
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<Category("Database") _
		, Description("OleDb Provider Name.")> _
		Public Property OleDbProviderName() As String
			Get
				Return _oleDbProviderName
			End Get
			Set(ByVal value As String)
				_oleDbProviderName = value
			End Set
		End Property

#End Region

		''' <summary>
		''' 構成ファイルの接続文字列セクションを返します。
		''' </summary>
		''' <param name="name">ConnectionStringSettings</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetSection(ByVal name As String) As ConnectionStringSettings
			Dim csSection As ConnectionStringsSection = Config.ConnectionStrings
			Return csSection.ConnectionStrings.Item(name)
		End Function

		''' <summary>
		''' 構成ファイルへ接続文字列セクションを追加します。
		''' </summary>
		''' <remarks></remarks>
		Public Sub AddSection()
			GetSection().ConnectionStrings.Add(cnvSetting())
		End Sub

		''' <summary>
		''' 構成ファイルの接続文字列セクションを変更します。
		''' </summary>
		''' <remarks></remarks>
		Public Sub ModSection()
			DelSection(_currentName)
			GetSection().ConnectionStrings.Add(cnvSetting())
		End Sub

		''' <summary>
		''' 構成ファイルの接続文字列セクションを削除します。
		''' </summary>
		''' <param name="name">セクション名称</param>
		''' <remarks></remarks>
		Public Sub DelSection(ByVal name As String)
			GetSection().ConnectionStrings.Remove(name)
		End Sub

		''' <summary>
		''' 接続文字列セクションを
		''' </summary>
		''' <param name="name">セクション名称</param>
		''' <remarks></remarks>
		Public Sub Read(ByVal name As String)
			moveValues(GetSection(name))
		End Sub

		''' <summary>
		''' 構成ファイルを保存します。
		''' </summary>
		''' <remarks></remarks>
		Public Sub Save()
			Config.Save(ConfigurationSaveMode.Modified)
		End Sub

		''' <summary>
		''' 構成ファイルを暗号化して保存します。
		''' </summary>
		''' <remarks></remarks>
		Public Sub SaveDPAPI()
			Dim dpapi As DPAPIConfiguration

			If GetSection.SectionInformation.IsProtected Then
				Save()
				Exit Sub
			End If

			' 暗号化
			dpapi = New DPAPIConfiguration(Config)
			dpapi.ProtectConnectionStrings()
		End Sub

		''' <summary>
		''' 設定内容を退避します。
		''' </summary>
		''' <param name="setting"></param>
		''' <remarks></remarks>
		Protected Sub moveValues(ByVal setting As ConnectionStringSettings)
			Dim builder As DbConnectionStringBuilder
			Dim buf As Object = Nothing

			_name = setting.Name
			_currentName = _name

			_providerName = setting.ProviderName

			builder = New DbConnectionStringBuilder
			builder.ConnectionString = setting.ConnectionString
			If builder.TryGetValue("Data Source", buf) Then
				_server = CStr(buf)
			End If
			If builder.TryGetValue("Initial Catalog", buf) Then
				_database = CStr(buf)
			End If
			If builder.TryGetValue("User ID", buf) Then
				_user = CStr(buf)
			End If
			If builder.TryGetValue("Password", buf) Then
				_password = CStr(buf)
			End If
			If builder.TryGetValue("Provider", buf) Then
				_oleDbProviderName = CStr(buf)
			End If
		End Sub

		''' <summary>
		''' 接続文字列セクションを返します。
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function getSection() As ConnectionStringsSection
			Return Config.ConnectionStrings
		End Function

		''' <summary>
		''' 内部で保持しているデータをConnectionStringSettingsへ変換する。
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function cnvSetting() As ConnectionStringSettings
			Dim builder As DbConnectionStringBuilder

			builder = New DbConnectionStringBuilder()
			builder.Add("Data Source", _server)
			builder.Add("Initial Catalog", _database)
			builder.Add("User ID", _user)
			builder.Add("Password", _password)
			builder.Add("Persist Security Info", True)
			If Trim(_oleDbProviderName).Length <= 0 Then
				builder.Add("Provider", _oleDbProviderName)
			End If
			If Trim(_name).Length <= 0 Then
				_name = _server & "." & _database
			End If

			Dim setting As ConnectionStringSettings

			setting = New ConnectionStringSettings()
			setting.Name = _name
			setting.ProviderName = _providerName
			setting.ConnectionString = builder.ConnectionString

			_currentName = setting.Name
			Return setting
		End Function

	End Class

End Namespace
