
Namespace Db

	''' <summary>
	''' DBCommandをラッピングするインタフェース
	''' </summary>
	''' <remarks></remarks>
	Public Interface IDbCommandSql
		Inherits IDisposable

#Region " Property "

		''' <summary>
		''' 実行するDBCommandインスタンスを参照
		''' </summary>
		''' <value>実行するDBCommandインスタンス</value>
		''' <remarks></remarks>
		ReadOnly Property Command() As IDbCommand

		''' <summary>
		''' コンパイル済みのSQLを使うかどうかを指定
		''' </summary>
		''' <value>
		''' True:使用する
		''' False:使用しない
		''' </value>
		''' <remarks></remarks>
		Property PreparedStatement() As Boolean

		''' <summary>
		''' SQL文
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Property CommandText() As String

		''' <summary>
		''' 実行後の出力パラメータを返す
		''' </summary>
		''' <value></value>
		''' <returns>出力パラメータ</returns>
		''' <remarks></remarks>
		ReadOnly Property ResultOutParameter() As Hashtable

#End Region

#Region " Parameter "

		''' <summary>
		''' 入力パラメータを設定する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="value">値</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks></remarks>
		Function SetParameter(ByVal parameterName As String, ByVal value As Object) As IDbDataParameter

		''' <summary>
		''' 入力パラメータを設定する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="values">値配列</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks>
		''' 当メソッドでは IN 句を作成します。
		''' IN 句はパラメータとしては扱えないので、SQL文内に存在するパラメータ名部分を文字列変換します。
		''' </remarks>
		Function SetParameter(ByVal parameterName As String, ByVal values As Array) As String

		''' <summary>
		''' 入力パラメータを追加する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="dbTypeValue">パラメータの型</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks></remarks>
		Function AddInParameter(ByVal parameterName As String, ByVal dbTypeValue As DbType) As IDbDataParameter

		''' <summary>
		''' 入力パラメータを追加する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="dbTypeValue">パラメータの型</param>
		''' <param name="size">パラメータのサイズ</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks></remarks>
		Function AddInParameter(ByVal parameterName As String, ByVal dbTypeValue As DbType, ByVal size As Integer) As IDbDataParameter

		''' <summary>
		''' 出力パラメータを追加する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks></remarks>
		Function AddOutParameter(ByVal parameterName As String) As IDbDataParameter

		''' <summary>
		''' 出力パラメータを追加する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="dbTypeValue">パラメータの型</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks></remarks>
		Function AddOutParameter(ByVal parameterName As String, ByVal dbTypeValue As DbType) As IDbDataParameter

		''' <summary>
		''' パラメータ内に戻り値があるか返す
		''' </summary>
		''' <returns>True は戻り値有り、False は戻り値無し</returns>
		''' <remarks></remarks>
		Function HaveOutParameter() As Boolean

		''' <summary>
		''' 出力パラメータを参照する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <returns>出力パラメータ値</returns>
		''' <remarks></remarks>
		Function GetParameterValue(ByVal parameterName As String) As Object

#End Region

		''' <summary>
		''' コンパイル済みのSQLにする
		''' </summary>
		''' <remarks>
		''' 当メソッド実行前に予め <see cref="AddInParameter"/> を使用してパラメータを設定しておいてください。<br/>
		''' </remarks>
		Sub Prepare()

		''' <summary>
		''' SQL実行！
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function Execute() As Integer

	End Interface

End Namespace
