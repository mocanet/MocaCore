
Imports System.Transactions
Imports System.Threading

Namespace Db.Tx.Local

	''' <summary>
	''' ローカルトランザクションコンテキスト
	''' </summary>
	''' <remarks></remarks>
	Public Class LocalTransactionContext

#Region " Declare "

		''' <summary>ローカルトランザクションマネージャー</summary>
		Private _mgr As LocalTransactionManager

		''' <summary>親のコンテキスト</summary>
		Private _parentLocalTxContext As LocalTransactionContext

		''' <summary>当コンテキストで扱うスコープたち</summary>
		Private _scopes As IList(Of LocalTransactionScope)

		''' <summary>当コンテキストで扱うトランザクション情報たち</summary>
		Private _localTxContext As IDictionary(Of Dbms, ITransactionContext)

		''' <summary>分離レベル</summary>
		Private _isolationLevel As IsolationLevel

#Region " Logging For Log4net "

		''' <summary>Logging For Log4net</summary>
		Private Shared ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

#End Region
#End Region

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New(ByVal mgr As LocalTransactionManager, ByVal isolationLevel As IsolationLevel)
			Me.New(mgr, isolationLevel, Nothing)
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New(ByVal mgr As LocalTransactionManager, ByVal isolationLevel As IsolationLevel, ByVal parentContext As LocalTransactionContext)
			_mgr = mgr
			_parentLocalTxContext = parentContext
			_scopes = New List(Of LocalTransactionScope)
			_localTxContext = New Dictionary(Of Dbms, ITransactionContext)
			_isolationLevel = isolationLevel
		End Sub

#End Region
#Region " Property "

		''' <summary>
		''' 親のコンテキスト
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ParentScope As LocalTransactionContext
			Get
				Return _parentLocalTxContext
			End Get
		End Property

		''' <summary>
		''' ロールバックステータス
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		Public WriteOnly Property RollbackStatus As Boolean
			Set(value As Boolean)
				For Each key As Dbms In _localTxContext.Keys
					Dim tx As ITransactionContext

					tx = _localTxContext.Item(key)
					tx.RollbackStatus = value
				Next
			End Set
		End Property

		''' <summary>
		''' 分離レベル
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property IsolationLevel As IsolationLevel
			Get
				Return _isolationLevel
			End Get
		End Property

		''' <summary>
		''' トランザクションコンテキスト
		''' </summary>
		''' <param name="val"></param>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property TransactionContext(ByVal val As LocalTransactionScope) As ITransactionContext
			Get
				Return _localTxContext(val.Dbms)
			End Get
		End Property

#End Region
#Region " Method "

		''' <summary>
		''' トランザクションメソッドコンテキスト追加
		''' </summary>
		''' <param name="context"></param>
		''' <remarks></remarks>
		Public Function Add(ByVal context As LocalTransactionScope) As ITransactionContext
			_scopes.Add(context)

			Dim rc As ITransactionContext = Nothing

			If Not _localTxContext.TryGetValue(context.Dbms, rc) Then
				rc = New TransactionContext(context.Dbms, _isolationLevel)
				_localTxContext.Add(context.Dbms, rc)
				_mylog.DebugFormat("TransactionContext Add[{1}:{2}] {0}", context.Dbms.Setting.Database, Thread.CurrentThread.ManagedThreadId, Me.GetHashCode)
			End If

			Return rc
		End Function

		''' <summary>
		''' トランザクションメソッドコンテキスト削除
		''' </summary>
		''' <param name="context"></param>
		''' <remarks></remarks>
		Public Sub Remove(ByVal context As LocalTransactionScope)
			_scopes.Remove(context)
		End Sub

		''' <summary>
		''' トランザクション終了
		''' </summary>
		''' <remarks></remarks>
		Public Sub Complete()
			If Not _isLastTransaction() Then
				Return
			End If

			For Each key As Dbms In _localTxContext.Keys
				Dim tx As ITransactionContext

				tx = _localTxContext.Item(key)
				tx.Complete()
			Next
			_localTxContext.Clear()
			_mgr.RemoveScope(Me)
		End Sub

		''' <summary>
		''' トランザクションロールバック
		''' </summary>
		''' <remarks></remarks>
		Public Sub Rollback()
			If Not _isLastTransaction() Then
				Return
			End If

			For Each key As Dbms In _localTxContext.Keys
				Dim tx As ITransactionContext

				tx = _localTxContext.Item(key)
				tx.Rollback()
			Next
			_localTxContext.Clear()
			_mgr.RemoveScope(Me)
		End Sub

		''' <summary>
		''' 最終のトランザクションかどうか
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function _isLastTransaction() As Boolean
			Return _scopes.Count = 0
		End Function

#End Region

	End Class

End Namespace
