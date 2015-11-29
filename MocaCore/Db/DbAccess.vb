Imports System.Configuration
Imports System.Data.Common
Imports System.Reflection
Imports Moca.Db.CommandWrapper
Imports Moca.Util

Namespace Db

	''' <summary>
	''' DBへアクセスする為の基本的な機能を提供する
	''' </summary>
	''' <remarks>
	''' 
	''' </remarks>
	Public Class DbAccess
		Inherits AbstractDao
		Implements IDbAccess

		''' <summary>トランザクションオブジェクト</summary>
		Private _tx As IDbTransaction
		''' <summary>トランザクションスコープオブジェクト</summary>
		Private _txs As Transactions.TransactionScope

		''' <summary>log4net logger</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

#Region " IDisposable Support "

		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			MyBase.Dispose(disposing)
			If _tx IsNot Nothing Then
				_tx.Dispose()
				_tx = Nothing
			End If
			If _txs IsNot Nothing Then
				_txs.Dispose()
				_txs = Nothing
			End If
		End Sub

#End Region
#Region " Constructor/DeConstructor "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks>
		''' 外部からは利用不可
		''' </remarks>
		Protected Sub New()
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="myDbms">接続先のDBMS</param>
		''' <remarks></remarks>
		Public Sub New(ByVal myDbms As Dbms)
			MyBase.New(myDbms)
		End Sub

#End Region
#Region " Property "

		''' <summary>
		''' トランザクションスコープオブジェクト
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		Public Overridable ReadOnly Property TransactionScope() As System.Transactions.TransactionScope Implements IDbAccess.TransactionScope
			Get
				Return _txs
			End Get
		End Property

		''' <summary>
		''' トランザクションオブジェクト
		''' </summary>
		''' <value></value>
		''' <remarks>
		''' </remarks>
		Public Overridable ReadOnly Property Transaction() As IDbTransaction Implements IDbAccess.Transaction
			Get
				Return _tx
			End Get
		End Property

#End Region
#Region " Transaction "

		''' <summary>
		''' トランザクションスコープを作成する
		''' </summary>
		''' <returns>トランザクションスコープ</returns>
		''' <remarks></remarks>
		Public Overridable Function NewTransactionScope() As Transactions.TransactionScope Implements IDbAccess.NewTransactionScope
			Try
				If _tx IsNot Nothing Then
					Throw New DbAccessException(Me, "既に TransactionStart メソッドにてトランザクションが開始されています。")
				End If
				If _txs IsNot Nothing Then
					Return _txs
				End If
				_txs = New Transactions.TransactionScope()
				Me.Connection.Open()
				Return _txs
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			End Try
		End Function

		''' <summary>
		''' トランザクションスコープを完了する
		''' </summary>
		''' <remarks></remarks>
		Public Overridable Sub TransactionComplete() Implements IDbAccess.TransactionComplete
			If _tx IsNot Nothing Then
				Throw New DbAccessException(Me, "既に TransactionStart メソッドにてトランザクションが開始されています。")
			End If
			If _txs Is Nothing Then
				Exit Sub
			End If
			If Me.Connection.State = ConnectionState.Closed Then
				Exit Sub
			End If

			Try
				Me.Connection.Close()
				_txs.Complete()
				_txs = Nothing
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			End Try
		End Sub

		''' <summary>
		''' トランザクションを開始する
		''' </summary>
		''' <remarks>
		''' トランザクションを使用する場合は事前にDBへの接続が必要な為、自動でDBとの接続を行います。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Public Overridable Sub TransactionStart() Implements IDbAccess.TransactionStart
			Try
				If _txs IsNot Nothing Then
					Throw New DbAccessException(Me, "既に TransactionScope メソッドにてトランザクションが開始されています。")
				End If
				If _tx IsNot Nothing Then
					Exit Sub
				End If
				Me.Connection.Open()
				_tx = Me.Connection.BeginTransaction
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			End Try
		End Sub

		''' <summary>
		''' 他のDBAccessクラスとトランザクションを同じにする
		''' </summary>
		''' <param name="dba">同期するDbAccessインスタンス</param>
		''' <remarks>
		''' コネクションオブジェクトとトランザクションオブジェクトを指定されたDbAccessのオブジェクトで上書きします。
		''' </remarks>
		Public Overridable Sub TransactionBinding(ByVal dba As IDbAccess) Implements IDbAccess.TransactionBinding
			Me.ConnectionJoin = dba.Connection
			_tx = dba.Transaction
		End Sub

		''' <summary>
		''' トランザクションを終了する（コミット）
		''' </summary>
		''' <remarks>
		''' DBとの接続を切断します。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Public Overridable Sub TransactionEnd() Implements IDbAccess.TransactionEnd
			If _tx Is Nothing Then
				Exit Sub
			End If
			If Me.Connection.State = ConnectionState.Closed Then
				Exit Sub
			End If

			Try
				_tx.Commit()
				Me.Connection.Close()
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			Finally
				_tx.Dispose()
			End Try
		End Sub

		''' <summary>
		''' トランザクションをロールバックする
		''' </summary>
		''' <remarks>
		''' DBとの接続を切断します。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Public Overridable Sub TransactionRollback() Implements IDbAccess.TransactionRollback
			If _tx Is Nothing Then
				Exit Sub
			End If
			If Me.Connection.State = ConnectionState.Closed Then
				Exit Sub
			End If

			Try
				_tx.Rollback()
				Me.Connection.Close()
			Catch ex As Exception
				Throw New DbAccessException(Me, ex)
			Finally
				_tx.Dispose()
			End Try
		End Sub

#End Region
#Region " Create "

		''' <summary>
		''' 指定されたタイプのDbCommandインスタンスを生成する
		''' </summary>
		''' <param name="sqlCommandType">コマンド種別</param>
		''' <param name="commandText">実行するSQL文又は、ストアド名</param>
		''' <param name="useConn">使用するコネクション</param>
		''' <returns>指定されたタイプのインスタンス</returns>
		''' <remarks>
		''' コマンド種別に該当するISqlCommandのインスタンスを生成します。<br/>
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
		''' DBアクセスでエラーが発生した
		''' </exception>
		Protected Friend Overrides Function createCommandWrapper(ByVal sqlCommandType As SQLCommandTypes, ByVal commandText As String, ByVal useConn As System.Data.IDbConnection) As IDbCommandSql
			Dim cmdWrapper As IDbCommandSql
			cmdWrapper = MyBase.createCommandWrapper(sqlCommandType, commandText, useConn)
			If Not _tx Is Nothing Then
				cmdWrapper.Command.Transaction = _tx
			End If
			Return cmdWrapper
		End Function

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
		Public Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandInsert) As Integer Implements IDbAccess.ExecuteNonQuery
			Return ExecuteNonQuery(commandWrapper)
		End Function

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
		Public Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandUpdate) As Integer Implements IDbAccess.ExecuteNonQuery
			Return ExecuteNonQuery(commandWrapper)
		End Function

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
		Public Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandDelete) As Integer Implements IDbAccess.ExecuteNonQuery
			Return ExecuteNonQuery(commandWrapper)
		End Function

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
		Public Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandStoredProcedure) As Integer Implements IDbAccess.ExecuteNonQuery
			Return ExecuteNonQuery(commandWrapper)
		End Function

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
		Public Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandDDL) As Integer Implements IDbAccess.ExecuteNonQuery
			Return ExecuteNonQuery(commandWrapper)
		End Function

		''' <summary>
		''' データを更新
		''' </summary>
		''' <param name="commandWrapper">更新を実行する為のDBCommandのラッパーインスタンス</param>
		''' <returns></returns>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Public Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandSelect4Update) As Integer Implements IDbAccess.ExecuteNonQuery
			Return UpdateAdapter(commandWrapper.ResultDataSet, commandWrapper.Adapter)
		End Function

#End Region

	End Class

End Namespace
