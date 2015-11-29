
Namespace Db

	''' <summary>
	''' DDL文を実行する為のDBCommandをラッピングするインタフェース
	''' </summary>
	''' <remarks>
	''' UPDATE文を実行するときに使用します。<br/>
	''' <example>
	''' <code lang="vb">
	''' Using dba As DbAccess = New DbAccess("Connection String")
	''' 	Using cmd As IDbCommandDDL = dba.CreateCommandDDL("DROP TABLE [HOGE]")
	''' 		Dim rc As Integer
	''' 		rc = cmd.Execute()
	''' 		Debug.Print(rc)
	''' 	End Using
	''' End Using
	''' </code>
	''' </example>
	''' </remarks>
	Public Interface IDbCommandDDL
		Inherits IDbCommandSql

	End Interface

End Namespace
