
Imports System.Transactions

Namespace Db.Tx

	''' <summary>
	''' トランザクション制御情報インタフェース
	''' </summary>
	''' <remarks></remarks>
	Public Interface ITransactionContext
		Inherits IDisposable

#Region " Property "

		''' <summary>
		''' トランザクションオブジェクト
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		ReadOnly Property Transaction() As IDbTransaction

		''' <summary>
		''' 既にトランザクションを開始しているかどうか
		''' </summary>
		''' <returns>True:開始している、False:開始していない</returns>
		''' <remarks></remarks>
		ReadOnly Property IsAlreadyTransaction() As Boolean

		''' <summary>
		''' ロールバックステータス
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>True:ロールバックする、False:ロールバックしない</remarks>
		Property RollbackStatus() As Boolean

#End Region
#Region " Method "

		''' <summary>
		''' トランザクションを開始する
		''' </summary>
		''' <remarks>
		''' トランザクションを使用する場合は事前にDBへの接続が必要な為、自動でDBとの接続を行います。<br/>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Sub Start()

		''' <summary>
		''' トランザクションを完了する
		''' </summary>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Sub Complete()

		''' <summary>
		''' トランザクションをロールバックする
		''' </summary>
		''' <remarks>
		''' DBとの接続を切断します。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Sub Rollback()

#End Region

	End Interface

End Namespace
