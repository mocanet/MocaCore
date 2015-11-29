
Namespace Db

	''' <summary>
	''' UPDATE�������s����ׂ�DBCommand�����b�s���O����C���^�t�F�[�X
	''' </summary>
	''' <remarks>
	''' UPDATE�������s����Ƃ��Ɏg�p���܂��B<br/>
	''' <example>
	''' <code lang="vb">
	''' Using dba As DbAccess = New DbAccess("Connection String")
	''' 	Using cmd As IDbCommandUpdate = dba.CreateCommandUpdate("UPDATE HOGE SET Name=@Name, Note=@Note WHERE IDA=@IDA AND ID=@ID")
	''' 		cmd.AddInParameter("@IDA", 1)
	''' 		cmd.AddInParameter("@ID", 1)
	''' 		cmd.AddInParameter("@Name", "hoge11")
	''' 		cmd.AddInParameter("@Note", "hogehoge11")
	'''
	''' 		Dim rc As Integer
	''' 		rc = cmd.Execute()
	''' 		Debug.Print(rc)
	''' 	End Using
	''' End Using
	''' </code>
	''' </example>
	''' </remarks>
	Public Interface IDbCommandUpdate
		Inherits IDbCommandSql

	End Interface

End Namespace
