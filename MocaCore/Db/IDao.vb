Imports Moca.Db.CommandWrapper

Namespace Db

	''' <summary>
	''' Data Access Object のインタフェース
	''' </summary>
	''' <remarks>
	''' データベースアクセスする際に最低限必要と思われる機能を提供します。<br/>
	''' </remarks>
	Public Interface IDao
		Inherits IDisposable

#Region " Property "

		''' <summary>
		''' DBMS
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		ReadOnly Property Dbms() As Dbms

		''' <summary>
		''' コネクションオブジェクト
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		ReadOnly Property Connection() As IDbConnection

		''' <summary>
		''' アダプタオブジェクト
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		ReadOnly Property Adapter() As IDbDataAdapter

		''' <summary>
		''' ヘルパークラスを返す
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Helper() As IDbAccessHelper

		''' <summary>
		''' 現在のコマンドラッパークラスを返す
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property CommandWrapper() As IDbCommandSql

		''' <summary>
		''' コマンド実行履歴有無指定
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Property ExecuteHistory() As Boolean

		''' <summary>
		''' 更新コマンド実行履歴有無指定
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Property ExecuteUpdateHistory() As Boolean

		''' <summary>
		''' コマンド実行履歴
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property ExecuteHistories() As IList(Of String)

		''' <summary>
		''' ロールバック有無
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Property RollbackStatus() As Boolean

#End Region
#Region " Check "

		''' <summary>
		''' 接続確認の為に一度接続してみる
		''' </summary>
		''' <remarks>
		''' 接続出来たときは切断します。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Sub CheckConnect()

#End Region
#Region " Create "

		''' <summary>
		''' SELECT文を実行する為のDBCommandのラッパークラスを生成する。
		''' </summary>
		''' <param name="commandText">SELECT文文字列</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandSelect(ByVal commandText As String) As IDbCommandSelect

		''' <summary>
		''' SELECT文を実行し、DataSetを使ってUPDATEする為のDBCommandのラッパークラスを生成する。
		''' </summary>
		''' <param name="commandText">SELECT文文字列</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandSelect4Update(ByVal commandText As String) As IDbCommandSelect4Update

		''' <summary>
		''' INSERT文を実行する為のDBCommandのラッパークラスを生成する。
		''' </summary>
		''' <param name="commandText">INSERT文文字列</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandInsert(ByVal commandText As String) As IDbCommandInsert

		''' <summary>
		''' UPDATE文を実行する為のDBCommandのラッパークラスを生成する。
		''' </summary>
		''' <param name="commandText">UPDATE文文字列</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandUpdate(ByVal commandText As String) As IDbCommandUpdate

		''' <summary>
		''' DELETE文を実行する為のDBCommandのラッパークラスを生成する。
		''' </summary>
		''' <param name="commandText">DELETE文文字列</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandDelete(ByVal commandText As String) As IDbCommandDelete

		''' <summary>
		''' StoredProcedureを実行する為のDBCommandのラッパークラスを生成する。
		''' </summary>
		''' <param name="commandText">ストアド名</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandStoredProcedure(ByVal commandText As String) As IDbCommandStoredProcedure

		''' <summary>
		''' DDLを実行する為のDBCommandのラッパークラスを生成する。
		''' </summary>
		''' <param name="commandText">DDL文文字列</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandDDL(ByVal commandText As String) As IDbCommandDDL

#End Region
#Region " Execute "

		''' <summary>
		''' クエリを実行し、そのクエリが返す結果セットの最初の行にある最初の列を返します。余分な列または行は無視されます。
		''' </summary>
		''' <param name="commandWrapper"></param>
		''' <returns>結果セットの最初の行にある最初の列。</returns>
		''' <remarks>
		''' 当メソッドは予めデータベースをオープンしておく必要がありますが、
		''' オープンされていないときは、自動でオープンして終了時にクローズします。<br/>
		''' 詳細は、<seealso cref="IDbCommand.ExecuteScalar"/> を参照してください。
		''' </remarks>
		Function ExecuteScalar(ByVal commandWrapper As IDbCommandSelect) As Object

		''' <summary>
		''' SELECT文の実行
		''' </summary>
		''' <param name="commandWrapper">SELECT文を実行する為のDBCommandのラッパーインスタンス</param>
		''' <returns>データ件数</returns>
		''' <remarks>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Function Execute(ByVal commandWrapper As IDbCommandSelect) As Integer

		''' <summary>
		''' SELECT文の実行
		''' </summary>
		''' <param name="commandWrapper">SELECT文を実行する為のDBCommandのラッパーインスタンス</param>
		''' <returns>結果</returns>
		''' <remarks>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Function Execute(Of T)(ByVal commandWrapper As IDbCommandSelect) As ISQLStatementResult

		''' <summary>
		''' SELECT文の実行(後にAdapterを利用した更新を行う場合)
		''' </summary>
		''' <param name="commandWrapper">SELECT文を実行する為のDBCommandのラッパーインスタンス</param>
		''' <returns>データ件数</returns>
		''' <remarks>
		''' SELECT実行後のデータ更新をDataSetを使って更新する場合は、こちらを使用してください。<br/>
		''' 予めAdapterとCommandを関連付けます。
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Function Execute(ByVal commandWrapper As IDbCommandSelect4Update) As Integer

		''' <summary>
		''' INSERT,UPDATE,DELETE文の実行
		''' </summary>
		''' <param name="commandWrapper">実行する為のDBCommandのラッパーインスタンス</param>
		''' <returns>更新件数</returns>
		''' <remarks>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandSql) As Integer

		''' <summary>
		''' アダプターによる更新
		''' </summary>
		''' <param name="ds">更新するデータ</param>
		''' <param name="adp">アダプター</param>
		''' <returns>更新件数</returns>
		''' <remarks></remarks>
		Function UpdateAdapter(ByVal ds As DataSet, ByVal adp As IDbDataAdapter) As Integer

#End Region

	End Interface

End Namespace
