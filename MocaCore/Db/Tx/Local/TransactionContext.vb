
Imports System.Transactions
Imports System.Threading

Namespace Db.Tx.Local

	''' <summary>
	''' トランザクション情報
	''' </summary>
	''' <remarks></remarks>
	Public Class TransactionContext
		Implements ITransactionContext

#Region " Declare "

		''' <summary>DBMS</summary>
		Private _dbms As Dbms
		''' <summary>接続</summary>
		Private _conn As IDbConnection
		''' <summary>トランザクション</summary>
		Private _tx As IDbTransaction
		''' <summary>ロールバック有無</summary>
		Private _rollbackStatus As Boolean
		''' <summary>分離レベル</summary>
		Private _isolationLevel As IsolationLevel

#Region " Logging For Log4net "
		''' <summary>Logging For Log4net</summary>
		Private Shared ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)
#End Region
#End Region

#Region "IDisposable Support"
		Private disposedValue As Boolean ' 重複する呼び出しを検出するには

		' IDisposable
		Protected Overridable Sub Dispose(disposing As Boolean)
			If Not Me.disposedValue Then
				If disposing Then
					' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
				End If

				' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
				' TODO: 大きなフィールドを null に設定します。
			End If
			Me.disposedValue = True
		End Sub

		' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
		'Protected Overrides Sub Finalize()
		'    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
		'    Dispose(False)
		'    MyBase.Finalize()
		'End Sub

		' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
		Public Sub Dispose() Implements IDisposable.Dispose
			' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
#End Region

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="dbms"></param>
		''' <remarks></remarks>
		Public Sub New(ByVal dbms As Dbms, ByVal isolationLevel As IsolationLevel)
			_dbms = dbms
			_isolationLevel = isolationLevel
		End Sub

#End Region
#Region " Implements "

#Region " Property "

		Public ReadOnly Property Transaction As System.Data.IDbTransaction Implements ITransactionContext.Transaction
			Get
				Return _tx
			End Get
		End Property

		Public ReadOnly Property IsAlreadyTransaction As Boolean Implements ITransactionContext.IsAlreadyTransaction
			Get
				Return _tx IsNot Nothing
			End Get
		End Property

		Public Property RollbackStatus() As Boolean Implements ITransactionContext.RollbackStatus
			Get
				Return _rollbackStatus
			End Get
			Set(ByVal value As Boolean)
				_rollbackStatus = value
			End Set
		End Property

#End Region
#Region " Method "

		Public Sub Start() Implements ITransactionContext.Start
			Try
				If _tx IsNot Nothing Then
					Return
				End If

				If _conn Is Nothing Then
					_conn = _dbms.CreateConnection()
				End If
				If _conn.State = ConnectionState.Closed Then
					_conn.Open()
				End If

				Dim level As System.Data.IsolationLevel

				Select Case _isolationLevel
					Case Transactions.IsolationLevel.Chaos
						level = Data.IsolationLevel.Chaos
					Case Transactions.IsolationLevel.ReadCommitted
						level = Data.IsolationLevel.ReadCommitted
					Case Transactions.IsolationLevel.ReadUncommitted
						level = Data.IsolationLevel.ReadUncommitted
					Case Transactions.IsolationLevel.RepeatableRead
						level = Data.IsolationLevel.RepeatableRead
					Case Transactions.IsolationLevel.Serializable
						level = Data.IsolationLevel.Serializable
					Case Transactions.IsolationLevel.Snapshot
						level = Data.IsolationLevel.Snapshot
					Case Transactions.IsolationLevel.Unspecified
						level = Data.IsolationLevel.Unspecified
				End Select

				_tx = _conn.BeginTransaction(level)
				_mylog.Debug(String.Format("Begin Transaction [{2}:{3}]: {0}, {1}", _dbms.Setting.Database, level, Thread.CurrentThread.ManagedThreadId, Me.GetHashCode))
			Catch ex As Exception
				Throw New Exceptions.MocaRuntimeException(ex)
			End Try
		End Sub

		Public Sub Complete() Implements ITransactionContext.Complete
			Try
				If _tx Is Nothing Then
					Return
				End If
				If _conn.State = ConnectionState.Closed Then
					Return
				End If

				Try
					_tx.Commit()
					_conn.Close()
					_mylog.Debug(String.Format("Complete Transaction [{1}:{2}]: {0}", _dbms.Setting.Database, Thread.CurrentThread.ManagedThreadId, Me.GetHashCode))
				Catch ex As Exception
					Throw New Exceptions.MocaRuntimeException(ex)
				Finally
					_tx.Dispose()
					_conn.Dispose()
				End Try
			Finally
				_tx = Nothing
				_conn = Nothing
			End Try
		End Sub

		Public Sub Rollback() Implements ITransactionContext.Rollback
			Try
				If _tx Is Nothing Then
					Exit Sub
				End If
				If _conn.State = ConnectionState.Closed Then
					Exit Sub
				End If

				Try
					_tx.Rollback()
					_conn.Close()
					_mylog.Debug(String.Format("Rollback Transaction [{1}:{2}]: {0}", _dbms.Setting.Database, Thread.CurrentThread.ManagedThreadId, Me.GetHashCode))
				Catch ex As Exception
					Throw New Exceptions.MocaRuntimeException(ex)
				Finally
					_tx.Dispose()
					_conn.Dispose()
				End Try
			Finally
				_tx = Nothing
				_conn = Nothing
			End Try
		End Sub

#End Region

#End Region

	End Class

End Namespace
