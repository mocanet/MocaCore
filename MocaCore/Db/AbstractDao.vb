
Imports System.Data.Common
Imports Moca.Db.CommandWrapper

Namespace Db

	''' <summary>
	''' Data Access Object �̃C���^�t�F�[�X�̎������ۃN���X
	''' </summary>
	''' <remarks>
	''' �f�[�^�x�[�X�A�N�Z�X����ۂɍŒ���K�v�Ǝv����@�\��񋟂��܂��B<br/>
	''' �e�V�X�e���� Data Access Object�iDAO�j���g�p����Ƃ��́A���N���X���p�����܂��B<br/>
	''' </remarks>
	Public MustInherit Class AbstractDao
		Inherits MarshalByRefObject
		Implements IDao, IDisposable

#Region " Declare "

		''' <summary>DBMS</summary>
		Private _dbms As Dbms
		''' <summary>�R�l�N�V�����I�u�W�F�N�g</summary>
		Private _conn As IDbConnection
		''' <summary>�A�_�v�^�I�u�W�F�N�g</summary>
		Private _adp As IDbDataAdapter
		''' <summary>�w���p�[�I�u�W�F�N�g</summary>
		Private _dbaHelper As IDbAccessHelper
		''' <summary>�R�}���h���b�p�[�I�u�W�F�N�g</summary>
		Private _commandWrapper As IDbCommandSql
		''' <summary>�R�}���h����L��</summary>
		Private _executeHistory As Boolean
		''' <summary>�X�V�R�}���h����L��</summary>
		Private _executeUpdateHistory As Boolean
		''' <summary>�R�}���h����</summary>
		Private _executeHistoryList As IList(Of String)
		''' <summary>���[���o�b�N�L��</summary>
		Private _rollbackStatus As Boolean

		''' <summary>�g�����U�N�V����������</summary>
		Private _txContext As Tx.ITransactionContext

#Region " log4net "
		''' <summary>log4net logger</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)
#End Region
#End Region

#Region " Constructor/DeConstructor "

		''' <summary>
		''' �f�t�H���g�R���X�g���N�^
		''' </summary>
		''' <remarks>
		''' �O������͗��p�s��
		''' </remarks>
		Protected Sub New()
			_executeHistory = False
			_executeUpdateHistory = False
		End Sub

		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="myDbms">�ڑ����DBMS</param>
		''' <remarks></remarks>
		Public Sub New(ByVal myDbms As Dbms)
			MyClass.New()
			Me._dbms = myDbms
			_conn = myDbms.CreateConnection()
			_adp = myDbms.ProviderFactory.CreateDataAdapter()
		End Sub

		''' <summary>
		''' �f�X�g���N�^
		''' </summary>
		''' <remarks></remarks>
		Protected Overrides Sub Finalize()
			MyBase.Finalize()
		End Sub

#End Region
#Region " IDisposable Support "

		Private disposedValue As Boolean = False		' �d������Ăяo�������o����ɂ�

		' IDisposable
		Protected Overridable Sub Dispose(ByVal disposing As Boolean)
			If Not Me.disposedValue Then
				If disposing Then
					' TODO: �����I�ɌĂяo���ꂽ�Ƃ��Ƀ}�l�[�W ���\�[�X��������܂�
				End If

				' TODO: ���L�̃A���}�l�[�W ���\�[�X��������܂�
				Me.Disposing()

				If _dbaHelper IsNot Nothing Then
					_dbaHelper.Dispose()
				End If
				If _conn IsNot Nothing Then
					If _conn.State <> ConnectionState.Closed Then
						_conn.Close()
					End If
					_conn.Dispose()
				End If

			End If
			Me.disposedValue = True
		End Sub

		' ���̃R�[�h�́A�j���\�ȃp�^�[���𐳂��������ł���悤�� Visual Basic �ɂ���Ēǉ�����܂����B
		Public Sub Dispose() Implements IDisposable.Dispose
			' ���̃R�[�h��ύX���Ȃ��ł��������B�N���[���A�b�v �R�[�h����� Dispose(ByVal disposing As Boolean) �ɋL�q���܂��B
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub

		Protected Overridable Sub Disposing()

		End Sub

#End Region
#Region " Property "

		''' <summary>
		''' �R�l�N�V�����I�u�W�F�N�g
		''' </summary>
		''' <value></value>
		''' <remarks>
		''' </remarks>
		Public Overridable ReadOnly Property Connection() As IDbConnection Implements IDao.Connection
			Get
				Return _conn
			End Get
		End Property

		''' <summary>
		''' �A�_�v�^�I�u�W�F�N�g
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Adapter() As System.Data.IDbDataAdapter Implements IDao.Adapter
			Get
				Return _adp
			End Get
		End Property

		''' <summary>
		''' DBMS
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Dbms() As Dbms Implements IDao.Dbms
			Get
				Return _dbms
			End Get
		End Property

		''' <summary>
		''' �w���p�[�N���X��Ԃ�
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Helper() As IDbAccessHelper Implements IDao.Helper
			Get
				If _dbaHelper Is Nothing Then
					_dbaHelper = _dbms.GetHelper(Me)
				End If
				Return _dbaHelper
			End Get
		End Property

		''' <summary>
		''' DBMS
		''' </summary>
		''' <value></value>
		''' <remarks>
		''' </remarks>
		Friend Overridable WriteOnly Property TargetDbms() As Dbms
			Set(ByVal value As Dbms)
				_dbms = value
				_conn = _dbms.ProviderFactory.CreateConnection()
				_conn.ConnectionString = _dbms.ConnectionStringSettings.ConnectionString
				_adp = _dbms.ProviderFactory.CreateDataAdapter()
			End Set
		End Property

		Public ReadOnly Property CommandWrapper() As IDbCommandSql Implements IDao.CommandWrapper
			Get
				Return _commandWrapper
			End Get
		End Property

		Public Property ExecuteHistory() As Boolean Implements IDao.ExecuteHistory
			Get
				Return _executeHistory
			End Get
			Set(ByVal value As Boolean)
				_executeHistory = value
				_executeUpdateHistory = value
				If _executeHistory Then
					_executeHistoryList = New List(Of String)
				Else
					_executeHistoryList = Nothing
				End If
			End Set
		End Property

		Public Property ExecuteUpdateHistory() As Boolean Implements IDao.ExecuteUpdateHistory
			Get
				Return _executeUpdateHistory
			End Get
			Set(ByVal value As Boolean)
				_executeUpdateHistory = value
				If _executeUpdateHistory Then
					_executeHistoryList = New List(Of String)
				Else
					_executeHistoryList = Nothing
				End If
			End Set
		End Property

		Public ReadOnly Property ExecuteHistories() As IList(Of String) Implements IDao.ExecuteHistories
			Get
				Return _executeHistoryList
			End Get
		End Property

		''' <summary>
		''' ���[���o�b�N�L��
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property RollbackStatus() As Boolean Implements IDao.RollbackStatus
			Get
				If _txContext IsNot Nothing Then
					Return _txContext.RollbackStatus
				End If
				Return _rollbackStatus
			End Get
			Set(ByVal value As Boolean)
				_rollbackStatus = value
				If _txContext Is Nothing Then
					Return
				End If
				_txContext.RollbackStatus = value
			End Set
		End Property

#End Region
#Region " Check "

		''' <summary>
		''' �ڑ��m�F�ׂ̈Ɉ�x�ڑ����Ă݂�
		''' </summary>
		''' <remarks>
		''' �ڑ��o�����Ƃ��͐ؒf���܂��B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Public Sub CheckConnect() Implements IDao.CheckConnect
			Try
				_conn.Open()
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			Finally
				If _conn.State <> ConnectionState.Closed Then
					_conn.Close()
				End If
			End Try
		End Sub

#End Region
#Region " Create "

		''' <summary>
		''' �w�肳�ꂽ�^�C�v��DbCommand�C���X�^���X�𐶐�����
		''' </summary>
		''' <param name="sqlCommandType">�R�}���h���</param>
		''' <param name="commandText">���s����SQL�����́A�X�g�A�h��</param>
		''' <param name="useConn">�g�p����R�l�N�V����</param>
		''' <returns>�w�肳�ꂽ�^�C�v�̃C���X�^���X</returns>
		''' <remarks>
		''' �R�}���h��ʂɊY������ISqlCommand�̃C���X�^���X�𐶐����܂��B<br/>
		''' <list>
		''' <item><term>SelectText</term><description>ISelectCommand</description></item>
		''' <item><term>Select4Update</term><description>ISelect4UpdateCommand</description></item>
		''' <item><term>UpdateText</term><description>IUpdateCommand</description></item>
		''' <item><term>InsertText</term><description>IInsertCommand</description></item>
		''' <item><term>DeleteText</term><description>IDeleteCommand</description></item>
		''' <item><term>StoredProcedure</term><description>IStoredProcedureCommand</description></item>
		''' <item><term>DDL</term><description>IDDLCommand</description></item>
		''' </list>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Protected Friend Overridable Function createCommandWrapper(ByVal sqlCommandType As SQLCommandTypes, ByVal commandText As String, ByVal useConn As IDbConnection) As IDbCommandSql
			Dim cmd As IDbCommand
			Dim cmdWrapper As IDbCommandSql

			Try
				' �N���X�̃C���X�^���X�̐���
				cmd = _dbms.ProviderFactory.CreateCommand()

				cmd.Connection = _conn
				If useConn IsNot Nothing Then
					cmd.Connection = useConn
				End If

				cmd.Transaction = _getTransaction()

				cmd.CommandText = commandText

				Select Case sqlCommandType
					Case SQLCommandTypes.SelectText
						cmd.CommandType = CommandType.Text
						cmdWrapper = New SelectCommandWrapper(Me, cmd)
					Case SQLCommandTypes.Select4Update
						cmd.CommandType = CommandType.Text
						cmdWrapper = New Select4UpdateCommandWrapper(Me, cmd)
					Case SQLCommandTypes.UpdateText
						cmd.CommandType = CommandType.Text
						cmdWrapper = New UpdateCommandWrapper(Me, cmd)
					Case SQLCommandTypes.InsertText
						cmd.CommandType = CommandType.Text
						cmdWrapper = New InsertCommandWrapper(Me, cmd)
					Case SQLCommandTypes.DeleteText
						cmd.CommandType = CommandType.Text
						cmdWrapper = New DeleteCommandWrapper(Me, cmd)
					Case SQLCommandTypes.StoredProcedure
						cmd.CommandType = CommandType.StoredProcedure
						cmdWrapper = New StoredProcedureCommandWrapper(Me, cmd)
					Case SQLCommandTypes.DDL
						cmd.CommandType = CommandType.Text
						cmdWrapper = New DDLCommandWrapper(Me, cmd)
					Case Else
						cmdWrapper = Nothing
				End Select

				' ���g�p�ɂ��Ă܂��B
				'' �p�����[�^�L���b�V�����擾
				'_sqlParameterCacheMgr.GetParameterSet(cmd)

#If DEBUG Then
				Dim st As StackTrace
				st = New StackTrace()
				_mylog.Debug(st.GetFrame(1).GetMethod().ToString & vbTab & commandText)
#End If
				Return cmdWrapper
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			End Try
		End Function

		''' <summary>
		''' SELECT�������s����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">SELECT��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function CreateCommandSelect(ByVal commandText As String) As IDbCommandSelect Implements IDao.CreateCommandSelect
			Return DirectCast(createCommandWrapper(SQLCommandTypes.SelectText, commandText, _getTxConnection), IDbCommandSelect)
		End Function

		''' <summary>
		''' SELECT�������s���ADataSet���g����UPDATE����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">SELECT��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function CreateCommandSelect4Update(ByVal commandText As String) As IDbCommandSelect4Update Implements IDao.CreateCommandSelect4Update
			Return DirectCast(createCommandWrapper(SQLCommandTypes.Select4Update, commandText, _getTxConnection), IDbCommandSelect4Update)
		End Function

		''' <summary>
		''' INSERT�������s����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">INSERT��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function CreateCommandInsert(ByVal commandText As String) As IDbCommandInsert Implements IDao.CreateCommandInsert
			Return DirectCast(createCommandWrapper(SQLCommandTypes.InsertText, commandText, _getTxConnection), IDbCommandInsert)
		End Function

		''' <summary>
		''' UPDATE�������s����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">UPDATE��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function CreateCommandUpdate(ByVal commandText As String) As IDbCommandUpdate Implements IDao.CreateCommandUpdate
			Return DirectCast(createCommandWrapper(SQLCommandTypes.UpdateText, commandText, _getTxConnection), IDbCommandUpdate)
		End Function

		''' <summary>
		''' DELETE�������s����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">DELETE��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function CreateCommandDelete(ByVal commandText As String) As IDbCommandDelete Implements IDao.CreateCommandDelete
			Return DirectCast(createCommandWrapper(SQLCommandTypes.DeleteText, commandText, _getTxConnection), IDbCommandDelete)
		End Function

		''' <summary>
		''' StoredProcedure�����s����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">�X�g�A�h��</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function CreateCommandStoredProcedure(ByVal commandText As String) As IDbCommandStoredProcedure Implements IDao.CreateCommandStoredProcedure
			Return DirectCast(createCommandWrapper(SQLCommandTypes.StoredProcedure, commandText, _getTxConnection), IDbCommandStoredProcedure)
		End Function

		''' <summary>
		''' DDL�����s����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">DDL��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function CreateCommandDDL(ByVal commandText As String) As IDbCommandDDL Implements IDao.CreateCommandDDL
			Return DirectCast(createCommandWrapper(SQLCommandTypes.DDL, commandText, _getTxConnection), IDbCommandDDL)
		End Function

#End Region
#Region " Execute "

		''' <summary>
		''' �N�G�������s���A���̃N�G�����Ԃ����ʃZ�b�g�̍ŏ��̍s�ɂ���ŏ��̗��Ԃ��܂��B�]���ȗ�܂��͍s�͖�������܂��B
		''' </summary>
		''' <param name="commandWrapper"></param>
		''' <returns>���ʃZ�b�g�̍ŏ��̍s�ɂ���ŏ��̗�B</returns>
		''' <remarks>
		''' �����\�b�h�͗\�߃f�[�^�x�[�X���I�[�v�����Ă����K�v������܂����A
		''' �I�[�v������Ă��Ȃ��Ƃ��́A�����ŃI�[�v�����ďI�����ɃN���[�Y���܂��B<br/>
		''' �ڍׂ́A<seealso cref="IDbCommand.ExecuteScalar"/> ���Q�Ƃ��Ă��������B
		''' </remarks>
		Public Overridable Function ExecuteScalar(ByVal commandWrapper As IDbCommandSelect) As Object Implements IDao.ExecuteScalar
			Dim cmd As IDbCommand = Nothing
			Dim result As Object
			Dim myOpen As Boolean

			Try
				_commandWrapper = commandWrapper

				cmd = commandWrapper.Command
#If DEBUG Then
				Dim st As StackTrace
				st = New StackTrace()
				_mylog.Debug("ExecuteScalar SQL : " & vbTab & cmd.CommandText)
#End If

				If cmd.Connection.State <> ConnectionState.Open Then
					cmd.Connection.Open()
					myOpen = True
				End If

				result = cmd.ExecuteScalar()

				' ���g�p�ɂ��Ă܂��B
				'If Not commandWrapper.PreparedStatement Then
				'    If Not _sqlParameterCacheMgr.Contains(cmd) Then
				'        _sqlParameterCacheMgr.PutParameterSet(cmd)
				'    End If
				'End If

				Return result
			Catch ex As DbAccessException
				Throw ex
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			Finally
				_addExecuteHistory(cmd, _executeHistory)
				If myOpen Then
					cmd.Connection.Close()
				End If
				Call cmd.Dispose()
				'_commandWrapper = Nothing
			End Try
		End Function

		''' <summary>
		''' SELECT���̎��s
		''' </summary>
		''' <param name="commandWrapper">SELECT�������s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns>�f�[�^����</returns>
		''' <remarks>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Public Overridable Function Execute(ByVal commandWrapper As IDbCommandSelect) As Integer Implements IDao.Execute
			Dim cmd As IDbCommand = Nothing
			Dim ds As DataSet = Nothing
			Dim result As Integer

			commandWrapper.ResultOutParameter.Clear()

			Try
				_commandWrapper = commandWrapper

				cmd = commandWrapper.Command
#If DEBUG Then
				Dim st As StackTrace
				st = New StackTrace()
				_mylog.Debug("Execute SQL : " & vbTab & cmd.CommandText)
#End If

				Me.Adapter.SelectCommand = cmd
				result = fill(ds)
				commandWrapper.ResultDataSet = ds

				' ���g�p�ɂ��Ă܂��B
				'If Not commandWrapper.PreparedStatement Then
				'    If Not _sqlParameterCacheMgr.Contains(cmd) Then
				'        _sqlParameterCacheMgr.PutParameterSet(cmd)
				'    End If
				'End If

				Return result
			Catch ex As DbAccessException
				Throw ex
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			Finally
				_addExecuteHistory(cmd, _executeHistory)
				cmd.Dispose()
				'_commandWrapper = Nothing
			End Try
		End Function

		''' <summary>
		''' SELECT���̎��s(ExecuteReader)
		''' </summary>
		''' <typeparam name="T">�G���e�B�e�B</typeparam>
		''' <param name="commandWrapper">�R�}���h���b�p�[</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Execute(Of T)(commandWrapper As IDbCommandSelect) As ISQLStatementResult Implements IDao.Execute
			Dim cmd As IDbCommand = Nothing
			Dim ds As DataSet = Nothing
			Dim result As ISQLStatementResult
			Dim myOpen As Boolean

			Try
				_commandWrapper = commandWrapper

				cmd = commandWrapper.Command
#If DEBUG Then
				Dim st As StackTrace
				st = New StackTrace()
				_mylog.Debug("Execute SQL : " & vbTab & cmd.CommandText)
#End If

				If cmd.Connection.State <> ConnectionState.Open Then
					cmd.Connection.Open()
					myOpen = True
				End If

				Dim reader As IDataReader
				reader = cmd.ExecuteReader(commandWrapper.Behavior)
				result = New ExecuteReaderResult(cmd, myOpen, reader)

				' ���g�p�ɂ��Ă܂��B
				'If Not commandWrapper.PreparedStatement Then
				'    If Not _sqlParameterCacheMgr.Contains(cmd) Then
				'        _sqlParameterCacheMgr.PutParameterSet(cmd)
				'    End If
				'End If

				Return result
			Catch ex As DbAccessException
				Throw ex
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			Finally
				_addExecuteHistory(cmd, _executeHistory)
				'_commandWrapper = Nothing
			End Try
		End Function

		''' <summary>
		''' SELECT���̎��s(���Adapter�𗘗p�����X�V���s���ꍇ)
		''' </summary>
		''' <param name="commandWrapper">SELECT�������s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns>�f�[�^����</returns>
		''' <remarks>
		''' SELECT���s��̃f�[�^�X�V��DataSet���g���čX�V����ꍇ�́A��������g�p���Ă��������B<br/>
		''' �\��Adapter��Command���֘A�t���܂��B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Public Overridable Function Execute(ByVal commandWrapper As IDbCommandSelect4Update) As Integer Implements IDao.Execute
			Dim cmd As IDbCommand = Nothing
			Dim cmdBuilder As DbCommandBuilder = Nothing
			Dim ds As DataSet = Nothing
			Dim result As Integer

			cmdBuilder = Me.Dbms.ProviderFactory.CreateCommandBuilder()
			cmdBuilder.DataAdapter = DirectCast(commandWrapper.Adapter, DbDataAdapter)

			Try
				_commandWrapper = commandWrapper

				cmd = commandWrapper.Command

				cmdBuilder.DataAdapter.SelectCommand = DirectCast(cmd, DbCommand)

				cmdBuilder.DataAdapter.InsertCommand = cmdBuilder.GetInsertCommand
				cmdBuilder.DataAdapter.UpdateCommand = cmdBuilder.GetUpdateCommand
				cmdBuilder.DataAdapter.DeleteCommand = cmdBuilder.GetDeleteCommand

				result = fill(ds, commandWrapper.Adapter)
				commandWrapper.ResultDataSet = ds

				' ���g�p�ɂ��Ă܂��B
				'If Not commandWrapper.PreparedStatement Then
				'    If Not _sqlParameterCacheMgr.Contains(cmd) Then
				'        _sqlParameterCacheMgr.PutParameterSet(cmd)
				'    End If
				'End If

				Return result
			Catch ex As DbAccessException
				Throw ex
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			Finally
				_addExecuteHistory(cmd, _executeUpdateHistory)
				cmd.Dispose()
				'_commandWrapper = Nothing
			End Try
		End Function

		''' <summary>
		''' INSERT,UPDATE,DELETE���̎��s
		''' </summary>
		''' <param name="commandWrapper">���s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns>�X�V����</returns>
		''' <remarks>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Public Overridable Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandSql) As Integer Implements IDao.ExecuteNonQuery
			Dim result As Integer
			Dim cmd As IDbCommand

			cmd = Nothing
			commandWrapper.ResultOutParameter.Clear()

			Try
				_commandWrapper = commandWrapper

				cmd = commandWrapper.Command

				result = cmd.ExecuteNonQuery()

				' ���g�p�ɂ��Ă܂��B
				'If Not commandWrapper.PreparedStatement Then
				'    If Not _sqlParameterCacheMgr.Contains(cmd) Then
				'        _sqlParameterCacheMgr.PutParameterSet(cmd)
				'    End If
				'End If

				' �߂�l���擾����
				getResultOutParameter(commandWrapper)

				Return result
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			Finally
				_addExecuteHistory(cmd, _executeUpdateHistory)
				If cmd IsNot Nothing Then
					cmd.Dispose()
				End If
				'_commandWrapper = Nothing
			End Try
		End Function

		''' <summary>
		''' �f�[�^���X�V
		''' </summary>
		''' <param name="ds">�f�[�^�Z�b�g�I�u�W�F�N�g</param>
		''' <param name="adp">�ΏۂƂȂ�A�_�v�^�[</param>
		''' <returns>DataSet �Ő���ɒǉ��܂��͍X�V���ꂽ�s��</returns>
		''' <remarks>
		''' DataAdapter���g����DataSet���̕ύX���ꂽ���ɂ��X�V���s���܂��B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Public Function UpdateAdapter(ByVal ds As DataSet, ByVal adp As IDbDataAdapter) As Integer Implements IDao.UpdateAdapter
			Dim result As Integer

			' �X�V�I
			Try
				result = adp.Update(ds)

				Return result
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			End Try
		End Function

#End Region

		''' <summary>
		''' �����񂪋�̎���Nothing�ɕϊ�
		''' </summary>
		''' <param name="value"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function CNothing(ByVal value As String) As String
			If String.IsNullOrEmpty(value) Then
				Return Nothing
			End If
			Return value
		End Function

		''' <summary>
		''' ���l��0�i�f�t�H���g�j�̎���Nothing�ɕϊ�
		''' </summary>
		''' <param name="value"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function CNothing(ByVal value As Integer, Optional ByVal isValue As Integer = 0) As Integer
			If value.Equals(isValue) Then
				Return Nothing
			End If
			Return value
		End Function

		''' <summary>
		''' �f�[�^���擾
		''' </summary>
		''' <param name="ds">�擾�����f�[�^�̃f�[�^�Z�b�g</param>
		''' <returns>�f�[�^����</returns>
		''' <remarks>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Protected Friend Function fill(ByRef ds As DataSet) As Integer
			Return fill(ds, "Results Data", Nothing)
		End Function

		''' <summary>
		''' �f�[�^���擾
		''' </summary>
		''' <param name="ds">�擾�����f�[�^�̃f�[�^�Z�b�g</param>
		''' <param name="adp">�A�_�v�^</param>
		''' <returns>�f�[�^����</returns>
		''' <remarks>
		''' �A�_�v�^���g�p���čX�V����Ƃ��Ɏg�p����B
		''' </remarks>
		Protected Friend Function fill(ByRef ds As DataSet, ByVal adp As IDbDataAdapter) As Integer
			Return fill(ds, "Results Data", adp)
		End Function

		''' <summary>
		''' �f�[�^���擾
		''' </summary>
		''' <param name="ds">�擾�����f�[�^�̃f�[�^�Z�b�g</param>
		''' <param name="DataSetName">�f�[�^�Z�b�g�̖���</param>
		''' <param name="adapter">�A�_�v�^</param>
		''' <returns>�f�[�^����</returns>
		''' <remarks>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Protected Friend Function fill(ByRef ds As DataSet, ByVal dataSetName As String, ByVal adapter As IDbDataAdapter) As Integer
			' �f�[�^�擾
			Try
				Dim result As Integer

				' �f�[�^�Z�b�g���C���X�^���X������Ă��Ȃ����̓C���X�^���X��
				If ds Is Nothing Then
					ds = New DataSet
					ds.DataSetName = dataSetName
				End If

				If adapter Is Nothing Then
					result = Me.Adapter.Fill(ds)
				Else
					result = adapter.Fill(ds)
				End If

				' �߂�l���擾����
				getResultOutParameter(CommandWrapper)

				Return result
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			Finally
				If adapter Is Nothing Then
					Me.Adapter.SelectCommand.Dispose()
				Else
					adapter.SelectCommand.Dispose()
				End If
			End Try
		End Function

		''' <summary>
		''' �߂�l���擾����
		''' </summary>
		''' <param name="commandWrapper"></param>
		''' <remarks></remarks>
		Protected Friend Sub getResultOutParameter(ByVal commandWrapper As IDbCommandSql)
			Dim cmd As IDbCommand = Nothing

			cmd = commandWrapper.Command

			' �߂�l���������͏I��
			If Not commandWrapper.HaveOutParameter() Then
				Exit Sub
			End If

			' �߂�l���擾����
			Dim ee As IEnumerator = cmd.Parameters.GetEnumerator
			While ee.MoveNext
				Dim param As IDbDataParameter
				param = DirectCast(ee.Current, IDbDataParameter)
				If param.Direction = ParameterDirection.InputOutput Or param.Direction = ParameterDirection.Output Or param.Direction = ParameterDirection.ReturnValue Then
					commandWrapper.ResultOutParameter.Add(param.ParameterName, param.Value)
					Dim key As String
					key = param.ParameterName.Replace(Helper.PlaceholderMark, "")
					If Not commandWrapper.ResultOutParameter.ContainsKey(key) Then
						commandWrapper.ResultOutParameter.Add(key, param.Value)
					End If
				End If
			End While
		End Sub

#Region " Transaction "

		''' <summary>
		''' �g�����U�N�V����������
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property TransactionContext() As Tx.ITransactionContext
			Get
				Return _txContext
			End Get
			Set(ByVal value As Tx.ITransactionContext)
				_txContext = value
			End Set
		End Property

		''' <summary>
		''' DB�ڑ�
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function _getTxConnection() As IDbConnection
			Dim conn As IDbConnection
			conn = Nothing
			If _txContext IsNot Nothing AndAlso _txContext.Transaction IsNot Nothing Then
				conn = _txContext.Transaction.Connection
			End If
			Return conn
		End Function

		''' <summary>
		''' �g�����U�N�V����
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function _getTransaction() As IDbTransaction
			Dim tx As IDbTransaction
			tx = Nothing
			If _txContext IsNot Nothing AndAlso _txContext.Transaction IsNot Nothing Then
				tx = _txContext.Transaction
			End If
			Return tx
		End Function

		'TODO: �C������K�v�L��
		Protected Friend WriteOnly Property ConnectionJoin() As IDbConnection
			Set(ByVal value As IDbConnection)
				_conn = value
			End Set
		End Property

#End Region

		''' <summary>
		''' �R�}���h���s�����̒ǉ�
		''' </summary>
		''' <param name="cmd"></param>
		''' <remarks></remarks>
		Private Sub _addExecuteHistory(ByVal cmd As IDbCommand, ByVal history As Boolean)
			If Not history Then
				Exit Sub
			End If
			If Not _executeHistory And Not _executeUpdateHistory Then
				Return
			End If

			_executeHistoryList.Add(cmd.CommandText & DbUtil.ToStringParameter(cmd.Parameters))
		End Sub

	End Class

End Namespace
