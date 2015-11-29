
Imports System.Transactions

Namespace Db.Tx.Local

	''' <summary>
	''' ローカルトランザクションマネージャー
	''' </summary>
	''' <remarks></remarks>
	Public Class LocalTransactionManager

#Region " Declare "

		''' <summary>ローカルトランザクションコンテキストたち</summary>
		Private _localTxContexts As IList(Of LocalTransactionContext)

		''' <summary>カレントのローカルトランザクションコンテキスト</summary>
		Private _currentLocalTxContext As LocalTransactionContext

#Region " Logging For Log4net "

		''' <summary>Logging For Log4net</summary>
		Private Shared ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

#End Region
#End Region

#Region " コンストラクタ "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			_localTxContexts = New List(Of LocalTransactionContext)
			_currentLocalTxContext = Nothing
		End Sub

#End Region

		''' <summary>
		''' コンテキスト返す
		''' </summary>
		''' <param name="scopeOption"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetLocalTransactionContext(ByVal scopeOption As Object, ByVal isolationLevel As Object) As LocalTransactionContext
			Dim context As LocalTransactionContext
			Dim wkScopeOption As TransactionScopeOption
			Dim wkIsolationLevel As IsolationLevel

			' スコープの決定
			If scopeOption Is Nothing Then
				wkScopeOption = TransactionScopeOption.Required
			Else
				wkScopeOption = DirectCast(scopeOption, TransactionScopeOption)
			End If

			' 分離レベルの決定
			If isolationLevel Is Nothing Then
				If _currentLocalTxContext IsNot Nothing Then
					wkIsolationLevel = _currentLocalTxContext.IsolationLevel
				Else
					wkIsolationLevel = Transactions.IsolationLevel.ReadCommitted
				End If
			Else
				wkIsolationLevel = DirectCast(isolationLevel, IsolationLevel)
			End If

			' ローカルトランザクションコンテキストの作成
			Select Case wkScopeOption
				Case TransactionScopeOption.RequiresNew
					' 新規
					context = New LocalTransactionContext(Me, wkIsolationLevel, _currentLocalTxContext)
					_currentLocalTxContext = context
					_localTxContexts.Add(context)

				Case TransactionScopeOption.Suppress
					' トランザクション外
					context = Nothing

				Case Else
					' カレント
					If _currentLocalTxContext Is Nothing Then
						_currentLocalTxContext = New LocalTransactionContext(Me, wkIsolationLevel)
					End If
					If wkIsolationLevel <> _currentLocalTxContext.IsolationLevel Then
						Throw New ArgumentException("同一トランザクション内では同じ分離レベルを指定してください。")
					End If
					context = _currentLocalTxContext
			End Select

			Return context
		End Function

		''' <summary>
		''' コンテキスト削除する
		''' </summary>
		''' <param name="val"></param>
		''' <remarks></remarks>
		Public Sub RemoveScope(ByVal val As LocalTransactionContext)
			_localTxContexts.Remove(val)
			_currentLocalTxContext = val.ParentScope
		End Sub

	End Class

End Namespace
