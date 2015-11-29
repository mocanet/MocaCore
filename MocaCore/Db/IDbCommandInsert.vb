
Namespace Db

	''' <summary>
	''' INSERT文を実行する為のDBCommandをラッピングするインタフェース
	''' </summary>
	''' <remarks>
	''' <example>
	''' <code lang="vb">
	''' Using dba As IDbAccess = New DbAccess("Connection String")
	''' 	Using cmd As IDbCommandInsert = dba.CreateCommandInsert("INSERT INTO HOGE([IDA],[ID],[Name],[Note])VALUES(@IDA,@ID,@Name,@Note)")
	''' 		cmd.AddInParameter("IDA", 1)
	''' 		cmd.AddInParameter("ID", 1)
	''' 		cmd.AddInParameter("Name", "Name11")
	''' 		cmd.AddInParameter("Note", "Note11")
	''' 
	''' 		Dim rc As Integer
	''' 		rc = cmd.Execute()
	''' 		Debug.Print(rc)
	''' 	End Using
	''' End Using
	''' </code>
	''' </example>
	''' </remarks>
	Public Interface IDbCommandInsert
		Inherits IDbCommandSql

	End Interface

End Namespace
