
Namespace Db

	''' <summary>
	''' StoredProcedureを実行する為のDBCommandをラッピングするインタフェース
	''' </summary>
	''' <remarks>
	''' StoredProcedureを実行するときに使用します。<br/>
	''' <example>
	''' <code lang="vb">
	''' Using dba As DbAccess = New DbAccess("Connection String")
	''' 	Using cmd As IDbCommandStoredProcedure = dba.CreateCommandStoredProcedure("HOGE_S01")
	''' 		cmd.SetParameterValue("@IDA", 1)
	''' 		cmd.SetParameterValue("@ID", 1)
	'''
	''' 		Dim rc As Integer
	''' 		rc = cmd.Execute()
	''' 		Debug.Print(rc)
	''' 	End Using
	''' End Using
	''' </code>
	''' </example>
	''' </remarks>
	Public Interface IDbCommandStoredProcedure
		Inherits IDbCommandSelect

#Region " Property "

		''' <summary>
		''' 実行後の戻り値を返す
		''' </summary>
		''' <value></value>
		''' <returns>戻り値</returns>
		''' <remarks></remarks>
		ReadOnly Property ReturnValue() As Object

#End Region

#Region " Methods "

		''' <summary>
		''' 入力パラメータ値を設定する
		''' </summary>
		''' <param name="index">パラメータ位置</param>
		''' <param name="value">値</param>
		''' <remarks>
		''' ストアドのパラメータを設定するときのみ使用可能です。
		''' パラメータ位置の０番目は@RETURN_VALUEになる為、指定された位置に＋１する。
		''' </remarks>
		Sub SetParameterValue(ByVal index As Integer, ByVal value As Object)

		''' <summary>
		''' 入力パラメータ値を設定する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="value">値</param>
		''' <remarks>
		''' ストアドのパラメータを設定するときのみ使用可能
		''' </remarks>
		Sub SetParameterValue(ByVal parameterName As String, ByVal value As Object)

		''' <summary>
		''' 入力パラメータ値の設定を追加する
		''' </summary>
		''' <param name="value"></param>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' 指定されたパラメータが多すぎます。
		''' </exception>
		Sub AddParameterValue(ByVal value As Object)

		''' <summary>
		''' 更新系のストアドを実行！
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function ExecuteNonQuery() As Integer

#End Region

	End Interface

End Namespace
