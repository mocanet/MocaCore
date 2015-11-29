
Imports System.Configuration
Imports System.Data.Common
Imports System.Threading
Imports Moca.Util

Namespace Db

	''' <summary>
	''' DBMS
	''' </summary>
	''' <remarks>
	''' DataBase Management Systemの略。
	''' データベースを構築・運用するために用いられる管理ソフトウェアのことで、
	''' このクラスで１接続先を表します。
	''' </remarks>
	Public Class Dbms

		''' <summary>構成ファイルの接続文字列セクション又はDB接続文字列を管理</summary>
		Private _dbSetting As DbSetting
		''' <summary>構成ファイルの接続文字列セクション内の名前付きで単一の接続文字列を表します。</summary>
		Private _connectionStringSettings As ConnectionStringSettings
		''' <summary>プロバイダのデータ ソース クラスの実装のインスタンスを作成するためのメソッドのセットを表します。</summary>
		Private _providerFactory As DbProviderFactory

		Private _helperFactory As Helper.DbAccessHelperFactory

		''' <summary>log4net logger</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="appKey">app.config の接続文字列キー名</param>
		''' <remarks>
		''' app.config から接続文字列、プロパイダを取得しコネクションを作成する
		''' </remarks>
		''' <exception cref="ArgumentException">
		''' DB接続する為の接続文字列が設定されていないときに発生する。
		''' </exception>
		Public Sub New(ByVal appKey As String)
			Try
				' 接続文字列をapp.configファイルから取得
				_connectionStringSettings = ConfigurationManager.ConnectionStrings(appKey)
				If _connectionStringSettings Is Nothing Then
					Throw New ArgumentException(appKey & " が app.configに未登録です。")
				End If
				_dbSetting = New DbSetting(_connectionStringSettings)

				' DBアクセス用のプロパイダをインスタンス化
				_providerFactory = DbProviderFactories.GetFactory(_connectionStringSettings.ProviderName)

				_helperFactory = New Helper.DbAccessHelperFactory(_dbSetting)

			Catch ex As ArgumentException
				Throw ex
			Finally
				If _connectionStringSettings IsNot Nothing Then
					_mylog.Debug("ProviderName=" & _connectionStringSettings.ProviderName)
					_mylog.Debug("ConnectionString=" & _connectionStringSettings.ConnectionString)
				End If
			End Try
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="providerName">プロパイダ名</param>
		''' <param name="serverName">接続先サーバー名</param>
		''' <param name="databaseName">接続するデータベース名</param>
		''' <param name="userName">接続ユーザー名</param>
		''' <param name="password">接続ユーザーパスワード</param>
		''' <remarks>
		''' 指定された情報を元に接続文字列、プロパイダを取得しコネクションを作成する
		''' </remarks>
		<Obsolete("")> _
		Public Sub New(ByVal providerName As String, ByVal serverName As String, ByVal databaseName As String, ByVal userName As String, ByVal password As String)
			Dim connectionStringBuilder As DbConnectionStringBuilder

			Try
				' DBアクセス用のプロパイダをインスタンス化
				_providerFactory = DbProviderFactories.GetFactory(providerName)

				connectionStringBuilder = _providerFactory.CreateConnectionStringBuilder()
				connectionStringBuilder.Add("Data Source", serverName)
				connectionStringBuilder.Add("Initial Catalog", databaseName)
				connectionStringBuilder.Add("User ID", userName)
				connectionStringBuilder.Add("Password", password)
				connectionStringBuilder.Add("Persist Security Info", True)

				_connectionStringSettings = New ConnectionStringSettings("Main", connectionStringBuilder.ConnectionString, providerName)
				_dbSetting = New DbSetting(_connectionStringSettings)

				_helperFactory = New Helper.DbAccessHelperFactory(_dbSetting)

			Catch ex As Exception
				Throw ex
			Finally
				If _connectionStringSettings IsNot Nothing Then
					_mylog.Debug("ProviderName=" & _connectionStringSettings.ProviderName)
					_mylog.Debug("ConnectionString=" & _connectionStringSettings.ConnectionString)
				End If
			End Try
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="name">接続先名称</param>
		''' <param name="providerName">プロパイダ名</param>
		''' <param name="connectionString">接続文字列</param>
		''' <remarks>
		''' 指定された情報を元に接続文字列、プロパイダを取得しコネクションを作成する
		''' </remarks>
		Public Sub New(ByVal name As String, ByVal providerName As String, ByVal connectionString As String)
			Try
				' DBアクセス用のプロパイダをインスタンス化
				_providerFactory = DbProviderFactories.GetFactory(providerName)

				_connectionStringSettings = New ConnectionStringSettings(name, connectionString, providerName)
				_dbSetting = New DbSetting(_connectionStringSettings)

				_helperFactory = New Helper.DbAccessHelperFactory(_dbSetting)

			Catch ex As Exception
				Throw ex
			Finally
				If _connectionStringSettings IsNot Nothing Then
					_mylog.Debug("ProviderName=" & _connectionStringSettings.ProviderName)
					_mylog.Debug("ConnectionString=" & _connectionStringSettings.ConnectionString)
				End If
			End Try
		End Sub

#End Region

#Region " プロパティ "

		''' <summary>構成ファイルの接続文字列セクション又はDB接続文字列を管理</summary>
		Public ReadOnly Property Setting() As DbSetting
			Get
				Return _dbSetting
			End Get
		End Property

		''' <summary>構成ファイルの接続文字列セクション内の名前付きで単一の接続文字列を表します。</summary>
		Public ReadOnly Property ConnectionStringSettings() As ConnectionStringSettings
			Get
				Return _connectionStringSettings
			End Get
		End Property

		''' <summary>プロバイダのデータ ソース クラスの実装のインスタンスを作成するためのメソッドのセットを表します。</summary>
		Public ReadOnly Property ProviderFactory() As DbProviderFactory
			Get
				Return _providerFactory
			End Get
		End Property

#End Region

		''' <summary>
		''' ヘルパークラスのインスタンス化
		''' </summary>
		''' <param name="dba">DBへアクセスするインスタンス</param>
		''' <remarks></remarks>
		Public Function GetHelper(ByVal dba As IDao) As IDbAccessHelper
			Return _helperFactory.Create(dba)
		End Function

		''' <summary>
		''' 新しい接続を作成する
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function CreateConnection() As IDbConnection
			Dim conn As IDbConnection

			conn = Me.ProviderFactory.CreateConnection()
			conn.ConnectionString = Me.ConnectionStringSettings.ConnectionString

			Return conn
		End Function

		''' <summary>
		''' 新しい DBAccess インスタンスを作成する。
		''' </summary>
		''' <returns>新しい DBAccess インスタンス</returns>
		''' <remarks></remarks>
		Public Function CreateDbAccess() As IDbAccess
			Dim dba As IDbAccess

			dba = New DbAccess(Me)

			Return dba
		End Function

		''' <summary>
		''' 新しい DBAccess インスタンスを作成する。
		''' </summary>
		''' <returns>新しい DBAccess インスタンス</returns>
		''' <remarks></remarks>
		Public Function CreateDbAccess(Of T)() As T
			Dim dba As Object

			' 型チェック
			Dim ok As Boolean
			For Each item As Type In GetType(T).GetInterfaces
				If item.Equals(GetType(IDbAccess)) Then
					ok = True
				End If
			Next
			If Not ok Then
				Throw New ArgumentException(GetType(T).FullName & " は、" & GetType(IDbAccess).FullName & " を実装したクラスではありません。")
			End If

			dba = ClassUtil.NewInstance(GetType(T), New Object() {Me})

			Return DirectCast(dba, T)
		End Function

	End Class

End Namespace
