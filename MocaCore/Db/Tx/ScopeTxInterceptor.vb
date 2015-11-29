
Imports System.Transactions
Imports Moca.Aop

Namespace Db.Tx

	''' <summary>
	''' スコープトランザクション処理のインターセプター
	''' </summary>
	''' <remarks></remarks>
	Public Class ScopeTxInterceptor
		Implements IMethodInterceptor

#Region " Declare "

		''' <summary>トランザクションオプション</summary>
		Private _scopeOption As Object

		''' <summary>分離レベル</summary>
		Private _isolationLevel As Object

		''' <summary>Aspectメソッド名</summary>
		Private _methodName As String

#Region " Logging For Log4net "
		''' <summary>Logging For Log4net</summary>
		Private Shared ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)
#End Region
#End Region

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="scopeOption"></param>
		''' <remarks></remarks>
		Public Sub New(ByVal scopeOption As Object, ByVal isolationLevel As Object)
			_scopeOption = scopeOption
			_isolationLevel = isolationLevel
		End Sub

#End Region
#Region " Implements "

		''' <summary>
		''' メソッド実行
		''' </summary>
		''' <param name="invocation">Interceptorからインターセプトされているメソッドの情報</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Invoke(ByVal invocation As Aop.IMethodInvocation) As Object Implements Aop.IMethodInterceptor.Invoke
			Dim rc As Object = Nothing
			Dim txLocalIdentifier As String
			Dim dao As AbstractDao

			If TryCast(invocation.This, IDao) Is Nothing Then
				Throw New Exceptions.MocaRuntimeException("DataAccessObjectではありません。")
			End If

			_methodName = invocation.This.GetType.FullName & ":" & invocation.Method.ToString

			' DTC 昇格が発生した場合のイベントハンドラの追加
			AddHandler TransactionManager.DistributedTransactionStarted, AddressOf TransactionManager_DistributedTransactionStarted

			dao = DirectCast(invocation.This, AbstractDao)

			Try
				' トランザクションスコープ開始
				Using txs As TransactionScope = _newTransactionScope()
					txLocalIdentifier = "Suppress"
					If Transaction.Current IsNot Nothing Then
						txLocalIdentifier = Transaction.Current.TransactionInformation.LocalIdentifier
						AddHandler Transaction.Current.TransactionCompleted, AddressOf TransactionCompleted_OnCommitted
					End If
					_mylog.Debug(String.Format("Transaction Start[{1}, {2}, {3}] !! (Aspect:{0})", _methodName, txLocalIdentifier, _scopeOption, _isolationLevel))

					Try
						' データベースアクセスクラスで保持している接続をオープン
						If dao IsNot Nothing Then
							If dao.Connection.State = ConnectionState.Closed Then
								dao.Connection.Open()
							End If
						End If

						' 実処理実行
						rc = invocation.Proceed()

						' ロールバック有りのときはここで終了
						If dao.RollbackStatus Then
							Return rc
						End If

						' トランザクション完了
						txs.Complete()
					Finally
						' データベースアクセスクラスで保持している接続をクローズ
						If dao IsNot Nothing Then
							dao.Connection.Close()
						End If
					End Try
				End Using
			Catch ex As Exception
				_mylog.Debug(String.Format("Transaction   Aborted Exception[{1}] !! (Aspect:{0})", _methodName, ex.Message))
				Throw ex
			End Try

			Return rc
		End Function

#End Region
#Region " Method "

		Private Function _newTransactionScope() As TransactionScope
			Dim txScope As TransactionScope

			If _scopeOption Is Nothing Then
				txScope = New TransactionScope()
			Else
				Dim scopeOption As TransactionScopeOption
				scopeOption = DirectCast(_scopeOption, TransactionScopeOption)
				If _isolationLevel Is Nothing Then
					txScope = New TransactionScope(scopeOption)
				Else
					Dim txOpt As TransactionOptions
					txOpt.IsolationLevel = DirectCast(_isolationLevel, IsolationLevel)
					txOpt.Timeout = TransactionManager.DefaultTimeout
					txScope = New TransactionScope(scopeOption, txOpt)
				End If
			End If

			'If Transaction.Current IsNot Nothing AndAlso _scopeOption = TransactionScopeOption.Required Then
			'	opt.IsolationLevel = Transaction.Current.IsolationLevel
			'End If

			Return txScope
		End Function

		''' <summary>
		''' DTC 昇格が発生した場合のイベント
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="d"></param>
		''' <remarks></remarks>
		Protected Sub TransactionManager_DistributedTransactionStarted(ByVal sender As Object, ByVal d As TransactionEventArgs)
			_mylog.Debug(String.Format("MS-DTC トランザクションへの昇格が発生しました。 (Aspect:{0})", _methodName))
		End Sub

		''' <summary>
		''' トランザクション完了イベント
		''' </summary>
		''' <param name="sender"></param>
		''' <param name="e"></param>
		''' <remarks></remarks>
		Private Sub TransactionCompleted_OnCommitted(sender As Object, e As System.Transactions.TransactionEventArgs)
			_mylog.Debug(String.Format("Transaction   {2} [{1}] !! (Aspect:{0})", _methodName, e.Transaction.TransactionInformation.LocalIdentifier, e.Transaction.TransactionInformation.Status.ToString))
		End Sub

#End Region

	End Class

End Namespace
