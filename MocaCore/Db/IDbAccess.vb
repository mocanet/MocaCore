Imports System.Configuration
Imports System.Data.Common

Namespace Db

	''' <summary>
	''' DBへアクセスする為の基本的な機能を提供するインタフェース
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Interface IDbAccess
		Inherits IDao, IDisposable

#Region " Property "

		''' <summary>
		''' トランザクションオブジェクト
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		ReadOnly Property Transaction() As IDbTransaction

		''' <summary>
		''' トランザクションスコープオブジェクト
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		ReadOnly Property TransactionScope() As Transactions.TransactionScope

#End Region
#Region " Transaction "

		''' <summary>
		''' トランザクションスコープを作成する
		''' </summary>
		''' <returns>トランザクションスコープ</returns>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Function NewTransactionScope() As Transactions.TransactionScope

		''' <summary>
		''' トランザクションスコープを完了する
		''' </summary>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Sub TransactionComplete()

		''' <summary>
		''' トランザクションを開始する
		''' </summary>
		''' <remarks>
		''' トランザクションを使用する場合は事前にDBへの接続が必要な為、自動でDBとの接続を行います。<br/>
		''' 通常は、<see cref="TransactionScope"/>を使用してください。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Sub TransactionStart()

		''' <summary>
		''' 他のDBAccessクラスとトランザクションを同じにする
		''' </summary>
		''' <param name="dba">同期するDbAccessインスタンス</param>
		''' 通常は、<see cref="TransactionScope"/>を使用してください。
		''' <remarks>
		''' コネクションオブジェクトとトランザクションオブジェクトを指定されたDbAccessのオブジェクトで上書きします。
		''' </remarks>
		Sub TransactionBinding(ByVal dba As IDbAccess)

		''' <summary>
		''' トランザクションを終了する（コミット）
		''' </summary>
		''' <remarks>
		''' DBとの接続を切断します。
		''' 通常は、<see cref="TransactionScope"/>を使用してください。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Sub TransactionEnd()

		''' <summary>
		''' トランザクションをロールバックする
		''' </summary>
		''' <remarks>
		''' DBとの接続を切断します。
		''' 通常は、<see cref="TransactionScope"/>を使用してください。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Sub TransactionRollback()

#End Region
#Region " Execute "

		''' <summary>
		''' INSERT文の実行
		''' </summary>
		''' <param name="commandWrapper">INSERT文を実行する為のDBCommandのラッパーインスタンス</param>
		''' <returns>更新件数</returns>
		''' <remarks>
		''' 当メソッドを使用する場合は、トランザクションの開始<see cref="DBAccess.TransactionStart"></see>、終了<see cref="DBAccess.TransactionEnd"></see>を行ってください。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandInsert) As Integer

		''' <summary>
		''' UPDATE文の実行
		''' </summary>
		''' <param name="commandWrapper">UPDATE文を実行する為のDBCommandのラッパーインスタンス</param>
		''' <returns>更新件数</returns>
		''' <remarks>
		''' 当メソッドを使用する場合は、トランザクションの開始<see cref="DBAccess.TransactionStart"></see>、終了<see cref="DBAccess.TransactionEnd"></see>を行ってください。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandUpdate) As Integer

		''' <summary>
		''' DELETE文の実行
		''' </summary>
		''' <param name="commandWrapper">DELETE文を実行する為のDBCommandのラッパーインスタンス</param>
		''' <returns>更新件数</returns>
		''' <remarks>
		''' 当メソッドを使用する場合は、トランザクションの開始<see cref="DBAccess.TransactionStart"></see>、終了<see cref="DBAccess.TransactionEnd"></see>を行ってください。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandDelete) As Integer

		''' <summary>
		''' ストアドの実行
		''' </summary>
		''' <param name="commandWrapper">ストアドを実行する為のDBCommandのラッパーインスタンス</param>
		''' <returns>更新件数</returns>
		''' <remarks>
		''' 当メソッドを使用する場合は、トランザクションの開始<see cref="DBAccess.TransactionStart"></see>、終了<see cref="DBAccess.TransactionEnd"></see>を行ってください。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandStoredProcedure) As Integer

		''' <summary>
		''' DDLの実行
		''' </summary>
		''' <param name="commandWrapper">DDLを実行する為のDBCommandのラッパーインスタンス</param>
		''' <returns>更新件数</returns>
		''' <remarks>
		''' 当メソッドを使用する場合は、トランザクションの開始<see cref="DBAccess.TransactionStart"></see>、終了<see cref="DBAccess.TransactionEnd"></see>を行ってください。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandDDL) As Integer

		''' <summary>
		''' データを更新
		''' </summary>
		''' <param name="commandWrapper">更新を実行する為のDBCommandのラッパーインスタンス</param>
		''' <returns></returns>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandSelect4Update) As Integer

#End Region

	End Interface

End Namespace
