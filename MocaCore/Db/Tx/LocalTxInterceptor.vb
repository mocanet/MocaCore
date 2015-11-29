
Imports System.Transactions
Imports System.Threading
Imports Moca.Aop
Imports Moca.Db.Tx.Local

Namespace Db.Tx

	''' <summary>
	''' ローカルトランザクション処理のインターセプター
	''' </summary>
	''' <remarks></remarks>
	Public Class LocalTxInterceptor
		Implements IMethodInterceptor

#Region " Declare "

		''' <summary>スコープオプション</summary>
		Private _scopeOption As Object

		''' <summary>分離レベル</summary>
		Private _isolationLevel As Object

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
		Public Function Invoke(invocation As Aop.IMethodInvocation) As Object Implements Aop.IMethodInterceptor.Invoke
			Dim rc As Object = Nothing
			Dim dao As AbstractDao
			Dim methodName As String = invocation.This.GetType.FullName & ":" & invocation.Method.ToString
			Dim txs As LocalTransactionScope

			If TryCast(invocation.This, IDao) Is Nothing Then
				Throw New Exceptions.MocaRuntimeException("DataAccessObjectではありません。")
			End If

			dao = DirectCast(invocation.This, AbstractDao)
			txs = Nothing

			Try
				txs = New LocalTransactionScope(_scopeOption, _isolationLevel, dao)
				_mylog.Debug(String.Format("TransactionScope Start[{1}:{2}, {3}, {4}] !! (Aspect:{0})", methodName, Thread.CurrentThread.ManagedThreadId, txs.TransactionContextHashCode, _scopeOption, txs.IsolationLevel))

				' トランザクション開始
				txs.Start()

				' 実処理実行
				rc = invocation.Proceed()

				' ロールバック有りのときはここで終了
				If txs.RollbackStatus Then
					txs.Rollback()
					_mylog.Debug(String.Format("TransactionScope Rollback[{1}:{2}] !! (Aspect:{0})", methodName, Thread.CurrentThread.ManagedThreadId, txs.TransactionContextHashCode))
					Return rc
				End If

				' コミット
				txs.Complete()

				_mylog.Debug(String.Format("TransactionScope End[{1}:{2}] !! (Aspect:{0})", methodName, Thread.CurrentThread.ManagedThreadId, txs.TransactionContextHashCode))
				Return rc
			Catch ex As Exception
				If txs IsNot Nothing Then
					' ロールバック
					txs.Rollback()
					_mylog.Debug(String.Format("TransactionScope Aborted[{1}:{2}] Exception[{3}]!! (Aspect:{0})", methodName, Thread.CurrentThread.ManagedThreadId, txs.TransactionContextHashCode, ex.Message))
				Else
					_mylog.Debug(String.Format("TransactionScope Aborted[{1}:{2}] Exception[{3}]!! (Aspect:{0})", methodName, Thread.CurrentThread.ManagedThreadId, String.Empty, ex.Message))
				End If
				Throw ex
			Finally
				If txs IsNot Nothing Then
					txs.Dispose()
				End If
			End Try
		End Function

#End Region

	End Class

End Namespace
