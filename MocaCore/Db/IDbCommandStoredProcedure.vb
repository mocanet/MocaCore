
Namespace Db

	''' <summary>
	''' StoredProcedure�����s����ׂ�DBCommand�����b�s���O����C���^�t�F�[�X
	''' </summary>
	''' <remarks>
	''' StoredProcedure�����s����Ƃ��Ɏg�p���܂��B<br/>
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
		''' ���s��̖߂�l��Ԃ�
		''' </summary>
		''' <value></value>
		''' <returns>�߂�l</returns>
		''' <remarks></remarks>
		ReadOnly Property ReturnValue() As Object

#End Region

#Region " Methods "

		''' <summary>
		''' ���̓p�����[�^�l��ݒ肷��
		''' </summary>
		''' <param name="index">�p�����[�^�ʒu</param>
		''' <param name="value">�l</param>
		''' <remarks>
		''' �X�g�A�h�̃p�����[�^��ݒ肷��Ƃ��̂ݎg�p�\�ł��B
		''' �p�����[�^�ʒu�̂O�Ԗڂ�@RETURN_VALUE�ɂȂ�ׁA�w�肳�ꂽ�ʒu�Ɂ{�P����B
		''' </remarks>
		Sub SetParameterValue(ByVal index As Integer, ByVal value As Object)

		''' <summary>
		''' ���̓p�����[�^�l��ݒ肷��
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <param name="value">�l</param>
		''' <remarks>
		''' �X�g�A�h�̃p�����[�^��ݒ肷��Ƃ��̂ݎg�p�\
		''' </remarks>
		Sub SetParameterValue(ByVal parameterName As String, ByVal value As Object)

		''' <summary>
		''' ���̓p�����[�^�l�̐ݒ��ǉ�����
		''' </summary>
		''' <param name="value"></param>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' �w�肳�ꂽ�p�����[�^���������܂��B
		''' </exception>
		Sub AddParameterValue(ByVal value As Object)

		''' <summary>
		''' �X�V�n�̃X�g�A�h�����s�I
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function ExecuteNonQuery() As Integer

#End Region

	End Interface

End Namespace
