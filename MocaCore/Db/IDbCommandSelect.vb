
Namespace Db

	''' <summary>
	''' SELECT文を実行する為のDBCommandをラッピングするインタフェース
	''' </summary>
	''' <remarks>
	''' データ抽出系のストアド実行でも使用出来ますが、ストアドの時は<see cref="IDbCommandStoredProcedure"/>を使用してください。<br/>
	''' <example>
	''' <code lang="vb">
	''' Using dba As IDbAccess = New DbAccess("Connection String")
	''' 	Using cmd As IDbCommandSelect = dba.CreateCommandSelect("SELECT * FROM TableName")
	''' 		If cmd.Execute() &lt;= 0 Then
	''' 			Return Nothing
	''' 		End If
	''' 		Return cmd.ResultDataSet
	''' 	End Using
	''' End Using
	''' </code>
	''' </example>
	''' </remarks>
	Public Interface IDbCommandSelect
		Inherits IDbCommandSql

#Region " Property "

		''' <summary>
		''' ExecuteReader に渡す CommandBehavior
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>
		''' <see cref="Execute"></see>以外では無視されます。
		''' <see cref="System.Data.SqlClient.SqlDataReader"></see>を使用している場合のみ有効。
		''' </remarks>
		Property Behavior As CommandBehavior

		''' <summary>
		''' Select文を実行した結果を設定／参照
		''' </summary>
		''' <value>Select文を実行した結果</value>
		''' <remarks></remarks>
		Property ResultDataSet() As DataSet

		''' <summary>
		''' DataSet内の先頭テーブルを返す
		''' </summary>
		''' <value></value>
		''' <returns>先頭テーブル</returns>
		''' <remarks></remarks>
		ReadOnly Property Result1stTable() As DataTable

		''' <summary>
		''' DataSet内の先頭テーブルに存在する行データのEnumeratorを返す
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Result1stTableRowEnumerator() As IEnumerator(Of DataRow)

#End Region

#Region " Methods "

		''' <summary>
		''' DataSet内の先頭テーブルを返す
		''' </summary>
		''' <typeparam name="T"></typeparam>
		''' <returns>先頭テーブルのデータを指定されたEntityを使用した配列に変換して返す</returns>
		''' <remarks>
		''' Execute 後に当メソッドでエンティティを取得するより<see cref="Execute(OF T)"></see>を使った方が高速でステップを減らせます。
		''' </remarks>
		<Obsolete("Execute(Of T)() を使うようにしてください。")> _
		Function Result1stTableEntitis(Of T)() As T()

		''' <summary>
		''' DataSet内の先頭テーブルの指定された行を返す
		''' </summary>
		''' <typeparam name="T"></typeparam>
		''' <param name="index"></param>
		''' <returns>先頭テーブルのデータを指定されたEntityを使用した配列に変換して返す</returns>
		''' <remarks></remarks>
		Function Result1stTableEntity(Of T)(ByVal index As Integer) As T

		''' <summary>
		''' DataSet内の先頭テーブルをConstantDataSet型で返す
		''' </summary>
		''' <param name="textColumnName">テキストとして扱う列の名称</param>
		''' <param name="valueColumnName">値として扱う列の名称</param>
		''' <param name="blankRow">空白行の有無</param>
		''' <param name="blankValue">空白行有りのときの空白行の値</param>
		''' <param name="delm">テキストと値の区切り文字</param>
		''' <returns>ConstantDataSet</returns>
		''' <remarks>
		''' 主に、コンボボックス等で使用する場合に使えます。
		''' </remarks>
		Overloads Function ResultConstantDataSet(ByVal textColumnName As String, ByVal valueColumnName As String, Optional ByVal blankRow As Boolean = False, Optional ByVal blankValue As Object = -1, Optional ByVal delm As String = " : ") As ConstantDataSet

		''' <summary>
		''' DataSet内の先頭テーブルをConstantDataSet型で返す
		''' </summary>
		''' <param name="textColumnIndex">テキストとして扱う列の位置</param>
		''' <param name="valueColumnIndex">値として扱う列の位置</param>
		''' <param name="blankRow">空白行の有無</param>
		''' <param name="blankValue">空白行有りのときの空白行の値</param>
		''' <param name="delm">テキストと値の区切り文字</param>
		''' <returns>ConstantDataSet</returns>
		''' <remarks>
		''' 主に、コンボボックス等で使用する場合に使えます。
		''' </remarks>
		Overloads Function ResultConstantDataSet(ByVal textColumnIndex As Integer, ByVal valueColumnIndex As Integer, Optional ByVal blankRow As Boolean = False, Optional ByVal blankValue As Object = -1, Optional ByVal delm As String = " : ") As ConstantDataSet

		''' <summary>
		''' クエリを実行し（ExecuteScalar）、そのクエリが返す結果セットの最初の行にある最初の列を返します。余分な列または行は無視されます。
		''' </summary>
		''' <returns>結果セットの最初の行にある最初の列。</returns>
		''' <remarks>
		''' 当メソッドは予めデータベースをオープンしておく必要がありますが、
		''' オープンされていないときは、自動でオープンして終了時にクローズします。<br/>
		''' 詳細は、<seealso cref="IDbCommand.ExecuteScalar"/> を参照してください。
		''' </remarks>
		Function ExecuteScalar() As Object

		''' <summary>
		''' クエリを実行し（ExecuteReader）、指定されたエンティティに変換して返します。
		''' </summary>
		''' <typeparam name="T">エンティティ</typeparam>
		''' <returns>エンティティのリスト</returns>
		''' <remarks>
		''' 当メソッドは予めデータベースをオープンしておく必要がありますが、
		''' オープンされていないときは、自動でオープンして終了時にクローズします。<br/>
		''' 詳細は、<seealso cref="IDbCommand.ExecuteReader"/> を参照してください。<br/>
		''' <br/>
		''' なお、当メソッドを使用した場合は結果をエンティティとして扱うことを前提としているため、<see cref="DataSet"></see>や<see cref="DataTable"></see>としては扱えません。<br/>
		''' よって<see cref="ResultDataSet"></see>, <see cref="Result1stTable"></see>などのメソッドは使用できません。<br/>
		''' バッチSQLステートメント時は<see cref="NextResult"></see>にて次の結果を取得してください。
		''' </remarks>
		Overloads Function Execute(Of T)() As IList(Of T)

		''' <summary>
		''' 次の結果を返す
		''' </summary>
		''' <typeparam name="T">エンティティ</typeparam>
		''' <returns>存在しないときは Nothing をかえす</returns>
		''' <remarks></remarks>
		Function NextResult(Of T)() As IList(Of T)

#End Region

	End Interface

End Namespace
