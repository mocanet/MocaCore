
Namespace Db

	''' <summary>
	''' DELETE文を実行する為のDBCommandをラッピングするインタフェース
	''' </summary>
	''' <remarks>
	''' DELETE文を実行するときに使用します。<br/>
	''' <example>
	''' <code lang="vb">
	''' Using dba As DbAccess = New DbAccess("Connection String")
	''' 	Using cmd As IDbCommandDelete = dba.CreateCommandDelete("DELETE FROM HOGE WHERE IDA=@IDA AND ID=@ID")
	''' 		cmd.AddInParameter("@IDA", 1)
	''' 		cmd.AddInParameter("@ID", 1)
	'''
	''' 		Dim rc As Integer
	''' 		rc = cmd.Execute()
	''' 		Debug.Print(rc)
	''' 	End Using
	''' End Using
	''' </code>
	''' </example>
	''' </remarks>
	Public Interface IDbCommandDelete
		Inherits IDbCommandSql

	End Interface

End Namespace
