
Namespace Db.CommandWrapper

	''' <summary>
	''' StoredProcedure�����s����ׂ�DBCommand�̃��b�p�[�N���X
	''' </summary>
	''' <remarks></remarks>
	Public Class StoredProcedureCommandWrapper
		Inherits SelectCommandWrapper
		Implements IDbCommandStoredProcedure

		''' <summary>�p�����[�^�J�E���^�[</summary>
		Private _addParameterValueCount As Integer

#Region " Constructor/DeConstructor "

		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="dba">�e�ƂȂ�DBAccess�C���X�^���X</param>
		''' <param name="cmd">���s����DBCommand�C���X�^���X</param>
		''' <remarks>
		''' </remarks>
		Protected Friend Sub New(ByVal dba As IDao, ByVal cmd As IDbCommand)
			MyBase.New(dba, cmd)
			getParameters()
			_addParameterValueCount = 0
		End Sub

#End Region

#Region " Property "

		''' <summary>
		''' ���s��̖߂�l��Ԃ�
		''' </summary>
		''' <value></value>
		''' <returns>�߂�l</returns>
		''' <remarks></remarks>
		Public ReadOnly Property ReturnValue() As Object Implements IDbCommandStoredProcedure.ReturnValue
			Get
				If Me.ResultOutputParam.ContainsKey("RETURN_VALUE") Then
					Return Me.ResultOutputParam("RETURN_VALUE")
				End If
				Return Nothing
			End Get
		End Property

#End Region

		''' <summary>
		''' ���̓p�����[�^�l��ݒ肷��
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <param name="value">�l</param>
		''' <remarks>
		''' �X�g�A�h�̃p�����[�^��ݒ肷��Ƃ��̂ݎg�p�\
		''' </remarks>
		Public Sub SetParameterValue(ByVal parameterName As String, ByVal value As Object) Implements IDbCommandStoredProcedure.SetParameterValue
			Dim param As IDbDataParameter

			parameterName = dba.Helper.CDbParameterName(parameterName)
			param = DirectCast(Me.cmd.Parameters.Item(parameterName), IDbDataParameter)
			param.Value = DbUtil.CNull(value)
		End Sub

		''' <summary>
		''' ���̓p�����[�^�l��ݒ肷��
		''' </summary>
		''' <param name="index">�p�����[�^�ʒu</param>
		''' <param name="value">�l</param>
		''' <remarks>
		''' �X�g�A�h�̃p�����[�^��ݒ肷��Ƃ��̂ݎg�p�\�ł��B
		''' �p�����[�^�ʒu�̂O�Ԗڂ�@RETURN_VALUE�ɂȂ�ׁA�w�肳�ꂽ�ʒu�Ɂ{�P����B
		''' </remarks>
		Public Sub SetParameterValue(ByVal index As Integer, ByVal value As Object) Implements IDbCommandStoredProcedure.SetParameterValue
			Dim param As IDbDataParameter

			param = DirectCast(Me.cmd.Parameters.Item(index + 1), IDbDataParameter)
			param.Value = DbUtil.CNull(value)
		End Sub

		''' <summary>
		''' ���̓p�����[�^�l�̐ݒ��ǉ�����
		''' </summary>
		''' <param name="value"></param>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' �w�肳�ꂽ�p�����[�^���������܂��B
		''' </exception>
		Public Sub AddParameterValue(ByVal value As Object) Implements IDbCommandStoredProcedure.AddParameterValue
			Dim idx As Integer

			idx = _addParameterValueCount
			idx += 1
			If Me.cmd.Parameters.Count < idx Then
				Throw New DbAccessException(Me.dba, "�w�肳�ꂽ�p�����[�^���������܂��B")
			End If

			_addParameterValueCount = idx
			SetParameterValue(_addParameterValueCount - 1, value)
		End Sub

		''' <summary>
		''' �X�g�A�h �v���V�[�W������p�����[�^�����擾���A�w�肵�� SqlCommand �I�u�W�F�N�g�� Parameters �R���N�V�����Ƀp�����[�^���i�[���܂��B
		''' </summary>
		''' <remarks></remarks>
		Protected Sub getParameters()
			Me.dba.Helper.RefreshProcedureParameters(Me.cmd)
		End Sub

		''' <summary>
		''' �X�V�n�̃X�g�A�h�����s�I
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ExecuteNonQuery() As Integer Implements IDbCommandStoredProcedure.ExecuteNonQuery
			_addParameterValueCount = 0
			Return dba.ExecuteNonQuery(Me)
		End Function

		''' <summary>
		''' SQL���s�I
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overrides Function Execute() As Integer
			_addParameterValueCount = 0
			Return MyBase.Execute()
		End Function

		''' <summary>
		''' �N�G�������s���A�w�肳�ꂽ�G���e�B�e�B�ɕϊ����ĕԂ��܂��B
		''' </summary>
		''' <typeparam name="T">�G���e�B�e�B</typeparam>
		''' <returns>�G���e�B�e�B�̃��X�g</returns>
		''' <remarks>
		''' �����\�b�h�͗\�߃f�[�^�x�[�X���I�[�v�����Ă����K�v������܂����A
		''' �I�[�v������Ă��Ȃ��Ƃ��́A�����ŃI�[�v�����ďI�����ɃN���[�Y���܂��B<br/>
		''' </remarks>
		Public Overrides Function Execute(Of T)() As System.Collections.Generic.IList(Of T)
			_addParameterValueCount = 0
			Return MyBase.Execute(Of T)()
		End Function

		''' <summary>
		''' �N�G�������s���A���̃N�G�����Ԃ����ʃZ�b�g�̍ŏ��̍s�ɂ���ŏ��̗��Ԃ��܂��B�]���ȗ�܂��͍s�͖�������܂��B
		''' </summary>
		''' <returns>���ʃZ�b�g�̍ŏ��̍s�ɂ���ŏ��̗�B</returns>
		''' <remarks>
		''' �����\�b�h�͗\�߃f�[�^�x�[�X���I�[�v�����Ă����K�v������܂����A
		''' �I�[�v������Ă��Ȃ��Ƃ��́A�����ŃI�[�v�����ďI�����ɃN���[�Y���܂��B<br/>
		''' �ڍׂ́A<seealso cref="IDbCommand.ExecuteScalar"/> ���Q�Ƃ��Ă��������B
		''' </remarks>
		Public Overrides Function ExecuteScalar() As Object
			_addParameterValueCount = 0
			Return MyBase.ExecuteScalar()
		End Function

	End Class

End Namespace
