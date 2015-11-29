
Imports Microsoft.Win32
Imports System.ComponentModel

Namespace Db

	''' <summary>
	''' ODBC�ݒ�̊Ǘ�
	''' </summary>
	''' <remarks>
	''' ODBC�f�[�^�\�[�X�ݒ�̊Ǘ������܂��B
	''' </remarks>
	Public Class OdbcSetting

		''' <summary>�f�[�^�\�[�X�̃V�X�e���L�[�l</summary>
		Private Const C_DSN_SYS As String = "CONFIGSYSDSN"
		''' <summary>�f�[�^�\�[�X�̃��[�U�[�L�[�l</summary>
		Private Const C_DSN_USR As String = "CONFIGDSN"
		''' <summary>�f�[�^�\�[�X�̃��W�X�g���L�[�l</summary>
		Private Const C_DSN_REG_KEY As String = "SOFTWARE\ODBC\ODBC.INI\"

		''' <summary>
		''' �o�^�̎��
		''' </summary>
		''' <remarks></remarks>
		Public Enum RegistrationType As Integer
			System = 0
			User
		End Enum

		''' <summary>
		''' ���p�o����v���o�C�_�̎��
		''' </summary>
		''' <remarks></remarks>
		Public Enum ProviderType As Integer
			SQLServer
		End Enum

		''' <summary>�o�^�̎��</summary>
		Private _dsnRegist As RegistrationType
		''' <summary>�v���o�C�_�̎��</summary>
		Private _dbProvider As ProviderType
		''' <summary>�v���o�C�_�[����</summary>
		Private _dsnName As String
		''' <summary>����</summary>
		Private _dsnDescription As String
		''' <summary>�T�[�o�[��</summary>
		Private _dbServer As String
		''' <summary>�f�[�^�x�[�X��</summary>
		Private _dbName As String
		''' <summary>�f�[�^�x�[�X�ڑ����[�U�[</summary>
		Private _dbUser As String
		''' <summary>�f�[�^�x�[�X�ڑ����[�U�[�p�X���[�h</summary>
		Private _dbUserPassword As String

#Region " �v���p�e�B "

		''' <summary>
		''' �o�^�̎��
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
		''' �o�^�̎��
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
		''' �v���o�C�_�[
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
		''' �v���o�C�_�[����
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
		''' ����
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
		''' �T�[�o�[��
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
		''' �f�[�^�x�[�X��
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
		''' �f�[�^�x�[�X�ڑ����[�U�[
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
		''' �f�[�^�x�[�X�ڑ����[�U�[�p�X���[�h
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
		''' �ݒ肪���݂��邩�Ԃ�
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
		''' �ǉ�����
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
		''' �C������
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
		''' ODBC�ݒ�R�}���h���s
		''' </summary>
		''' <remarks></remarks>
		Protected Sub execODBCconf()
			Dim psi As New System.Diagnostics.ProcessStartInfo

			'ComSpec�̃p�X���擾����
			psi.FileName = System.Environment.GetEnvironmentVariable("ComSpec")

			'�o�͂�ǂݎ���悤�ɂ���
			psi.RedirectStandardInput = False
			psi.RedirectStandardOutput = True
			psi.UseShellExecute = False

			'�E�B���h�E��\�����Ȃ��悤�ɂ���
			psi.CreateNoWindow = True

			'�R�}���h���C�����w��i"/c"�͎��s����邽�߂ɕK�v�j
			psi.Arguments = "/c ODBCconf /A {" & makeCommandParam() & "}"

			'�N��
			Dim p As Process = Process.Start(psi)

			'�o�͌��ʂ��i�[
			Dim results As String = p.StandardOutput.ReadToEnd

			'WaitForExit��ReadToEnd�̌�ł���K�v������(�e�v���Z�X�A�q�v���Z�X�Ńu���b�N�h�~�̂���)
			p.WaitForExit()

			p.Close() '�v���Z�X�J��
		End Sub

		''' <summary>
		''' ODBC�ݒ�R�}���h�̃p�����[�^������쐬
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
