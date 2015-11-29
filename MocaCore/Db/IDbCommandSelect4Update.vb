
Namespace Db

	''' <summary>
	''' SELECT�������s���ADataSet���g����UPDATE����ׂ�DBCommand�̃��b�s���O����C���^�t�F�[�X
	''' </summary>
	''' <remarks>
	''' <example>
	''' <code lang="vb">
	''' Using dba As IDbAccess = New DbAccess("Connection String")
	''' 	Using cmd As IDbCommandSelect4Update = dba.CreateCommandSelect4Update("SELECT * FROM TableName")
	''' 		If cmd.Execute() &lt;= 0 Then
	''' 			Return Nothing
	''' 		End If
	''' 
	''' 		... cmd.ResultDataSet �ɑ΂��čX�V���� ...
	''' 
	''' 		Dim rc As Integer
	''' 		rc = cmd.Update()
	''' 		Debug.Print(rc)
	''' 	End Using
	''' End Using
	''' </code>
	''' </example>
	''' </remarks>
	Public Interface IDbCommandSelect4Update
		Inherits IDbCommandSelect

#Region " Property "

		''' <summary>
		''' �A�_�v�^�C���X�^���X
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Adapter() As IDbDataAdapter

#End Region

		''' <summary>
		''' Adapter Update ���s�I
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function Update() As Integer

	End Interface

End Namespace
