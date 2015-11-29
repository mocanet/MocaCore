
Imports System.Transactions
Imports System.Threading

Namespace Db.Tx.Local

	''' <summary>
	''' トランザクション属性がついたメソッド一つ分のスコープ
	''' </summary>
	''' <remarks></remarks>
	Public Class LocalTransactionScope
		Implements ITransactionContext

#Region " Declare "

		''' <summary>トランザクションマネージャー</summary>
		<ThreadStatic()> _
		Private Shared _mgr As LocalTransactionManager

		''' <summary>Dao</summary>
		Private _dao As AbstractDao

		''' <summary>ローカルトランザクションコンテキスト</summary>
		Private _localTxContext As LocalTransactionContext

		''' <summary>ローカルトランザクション</summary>
		Private _txContext As ITransactionContext

#Region " Logging For Log4net "
		''' <summary>Logging For Log4net</summary>
		Private Shared ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)
#End Region
#End Region

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="scopeOption">スコープオプション</param>
		''' <remarks></remarks>
		Public Sub New(ByVal scopeOption As Object, ByVal isolationLevel As Object, ByVal dao As IDao)
			If _mgr Is Nothing Then
				_mgr = New LocalTransactionManager()
			End If

			_dao = DirectCast(dao, AbstractDao)
			_localTxContext = _mgr.GetLocalTransactionContext(scopeOption, isolationLevel)
			If _localTxContext Is Nothing Then
				' Suppress のときはここまで
				Return
			End If

			_txContext = _localTxContext.Add(Me)
			_dao.TransactionContext = _txContext
		End Sub

#End Region
#Region " Property "

		''' <summary>
		''' Dao
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Dao() As IDao
			Get
				Return _dao
			End Get
		End Property

		''' <summary>
		''' DBMS
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Dbms() As Dbms
			Get
				Return _dao.Dbms
			End Get
		End Property

		''' <summary>
		''' ローカルトランザクションコンテキスト
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property TransactionContext() As LocalTransactionContext
			Get
				Return _localTxContext
			End Get
		End Property

		''' <summary>
		''' ローカルトランザクションコンテキストハッシュコード
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property TransactionContextHashCode() As Integer
			Get
				If _localTxContext Is Nothing Then
					Return Nothing
				End If
				Return _localTxContext.GetHashCode
			End Get
		End Property

		Public ReadOnly Property IsolationLevel() As IsolationLevel
			Get
				If _localTxContext Is Nothing Then
					Return Nothing
				End If
				Return _localTxContext.IsolationLevel
			End Get
		End Property

#End Region
#Region " Implements "

#Region "IDisposable Support"
		Private disposedValue As Boolean ' 重複する呼び出しを検出するには

		' IDisposable
		Protected Overridable Sub Dispose(disposing As Boolean)
			If Not Me.disposedValue Then
				If disposing Then
					If _localTxContext Is Nothing Then
						_dao.TransactionContext = _txContext
						' Suppress のときはここまで
						Return
					End If

					_localTxContext.Remove(Me)
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
#Region " ITransactionContext"

#Region " Property "

		Public ReadOnly Property Transaction As System.Data.IDbTransaction Implements ITransactionContext.Transaction
			Get
				If _localTxContext Is Nothing Then
					Return Nothing
				End If
				Return _txContext.Transaction()
			End Get
		End Property

		Public ReadOnly Property IsAlreadyTransaction As Boolean Implements ITransactionContext.IsAlreadyTransaction
			Get
				If _localTxContext Is Nothing Then
					Return False
				End If
				Return _txContext.IsAlreadyTransaction()
			End Get
		End Property

		Public Property RollbackStatus As Boolean Implements ITransactionContext.RollbackStatus
			Get
				If _localTxContext Is Nothing Then
					Return False
				End If
				Return _txContext.RollbackStatus
			End Get
			Set(value As Boolean)
				_localTxContext.RollbackStatus = value
			End Set
		End Property

#End Region
#Region " Method "

		Public Sub Start() Implements ITransactionContext.Start
			If _localTxContext Is Nothing Then
				_txContext = _dao.TransactionContext
				_dao.TransactionContext = Nothing
				' Suppress のときはここまで
				Return
			End If

			If _txContext.IsAlreadyTransaction Then
				Return
			End If
			_txContext.Start()
		End Sub

		Public Sub Complete() Implements ITransactionContext.Complete
			If _localTxContext Is Nothing Then
				' Suppress のときはここまで
				Return
			End If

			_localTxContext.Remove(Me)
			_localTxContext.Complete()
		End Sub

		Public Sub Rollback() Implements ITransactionContext.Rollback
			If _localTxContext Is Nothing Then
				' Suppress のときはここまで
				Return
			End If

			_localTxContext.Remove(Me)
			_localTxContext.Rollback()
		End Sub

#End Region

#End Region

#End Region

	End Class

End Namespace
