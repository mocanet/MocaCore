
Namespace Db

	''' <summary>
	''' DDL�������s����ׂ�DBCommand�����b�s���O����C���^�t�F�[�X
	''' </summary>
	''' <remarks>
	''' UPDATE�������s����Ƃ��Ɏg�p���܂��B<br/>
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
