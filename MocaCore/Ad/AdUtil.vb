
Imports System.DirectoryServices

Namespace Ad

    ''' <summary>
    ''' Active Directoryサービスへアクセスするためのメソッド集
    ''' </summary>
    ''' <remarks></remarks>
	Public Class AdUtil

		Private _Secure As Boolean
		Private _ReadonlyServer As Boolean

		Private _domainAndUsername As String

		''' <summary>Directory Path</summary>
		Private _path As String

		''' <summary>属性</summary>
		Private _filterAttribute As String

		''' <summary>Logging For Log4net</summary>
		Private Shared ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

		Public ReadOnly Property DomainAndUsername() As System.String
			Get
				Return Me._domainAndUsername
			End Get
		End Property

		Public ReadOnly Property Path() As System.String
			Get
				Return Me._path
			End Get
		End Property

		Public ReadOnly Property FilterAttribute() As System.String
			Get
				Return Me._filterAttribute
			End Get
		End Property

		''' <summary>
		''' ログインしているユーザーがActive Directoryサービスに登録されているか確認します。
		''' </summary>
		''' <param name="domain">ドメイン名</param>
		''' <param name="username">ユーザー名</param>
		''' <param name="pwd">パスワード</param>
		''' <returns>
		''' True: 登録されている。<br/>
		''' False: 登録されていない。<br/>
		''' </returns>
		''' <remarks></remarks>
		Public Function IsAuthenticated(ByVal domain As String, ByVal username As String, ByVal pwd As String, Optional ByVal ldap As String = "") As Boolean
			Dim auth As AuthenticationTypes = New AuthenticationTypes

			If ldap.Length > 0 Then
				_path = ldap
			End If

			If Me.Secure Then
				auth = AuthenticationTypes.Secure
			End If
			If Me.ReadonlyServer Then
				auth = auth Or AuthenticationTypes.ReadonlyServer
			End If

			_domainAndUsername = domain & "\" & username
			If domain.Length = 0 Then
				_domainAndUsername = username
			End If
			_mylog.Debug(_domainAndUsername)

			Dim entry As DirectoryEntry

			entry = New DirectoryEntry(_path, _domainAndUsername, pwd, auth)

			Dim dSearcher As DirectorySearcher
			Dim obj As Object = entry.NativeObject

			dSearcher = New DirectorySearcher(entry)
			dSearcher.Filter = "(SAMAccountName=" & username & ")"
			dSearcher.PropertiesToLoad.Add("cn")

			Dim result As SearchResult

			result = dSearcher.FindOne()

			If result Is Nothing Then
				Return False
			End If

			_path = result.Path
			_filterAttribute = DirectCast(result.Properties("cn")(0), String)

			Return True
		End Function

		Public Property Secure() As System.Boolean
			Get
				Return Me._Secure
			End Get
			Set(ByVal value As System.Boolean)
				Me._Secure = value
			End Set
		End Property

		Public Property ReadonlyServer() As System.Boolean
			Get
				Return Me._ReadonlyServer
			End Get
			Set(ByVal value As System.Boolean)
				Me._ReadonlyServer = value
			End Set
		End Property

	End Class

End Namespace
