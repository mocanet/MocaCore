
Namespace Db

	''' <summary>
	''' DBCommand�����b�s���O����C���^�t�F�[�X
	''' </summary>
	''' <remarks></remarks>
	Public Interface IDbCommandSql
		Inherits IDisposable

#Region " Property "

		''' <summary>
		''' ���s����DBCommand�C���X�^���X���Q��
		''' </summary>
		''' <value>���s����DBCommand�C���X�^���X</value>
		''' <remarks></remarks>
		ReadOnly Property Command() As IDbCommand

		''' <summary>
		''' �R���p�C���ς݂�SQL���g�����ǂ������w��
		''' </summary>
		''' <value>
		''' True:�g�p����
		''' False:�g�p���Ȃ�
		''' </value>
		''' <remarks></remarks>
		Property PreparedStatement() As Boolean

		''' <summary>
		''' SQL��
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Property CommandText() As String

		''' <summary>
		''' ���s��̏o�̓p�����[�^��Ԃ�
		''' </summary>
		''' <value></value>
		''' <returns>�o�̓p�����[�^</returns>
		''' <remarks></remarks>
		ReadOnly Property ResultOutParameter() As Hashtable

#End Region

#Region " Parameter "

		''' <summary>
		''' ���̓p�����[�^��ݒ肷��
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <param name="value">�l</param>
		''' <returns>�p�����[�^�C���X�^���X</returns>
		''' <remarks></remarks>
		Function SetParameter(ByVal parameterName As String, ByVal value As Object) As IDbDataParameter

		''' <summary>
		''' ���̓p�����[�^��ݒ肷��
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <param name="values">�l�z��</param>
		''' <returns>�p�����[�^�C���X�^���X</returns>
		''' <remarks>
		''' �����\�b�h�ł� IN ����쐬���܂��B
		''' IN ��̓p�����[�^�Ƃ��Ă͈����Ȃ��̂ŁASQL�����ɑ��݂���p�����[�^�������𕶎���ϊ����܂��B
		''' </remarks>
		Function SetParameter(ByVal parameterName As String, ByVal values As Array) As String

		''' <summary>
		''' ���̓p�����[�^��ǉ�����
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <param name="dbTypeValue">�p�����[�^�̌^</param>
		''' <returns>�p�����[�^�C���X�^���X</returns>
		''' <remarks></remarks>
		Function AddInParameter(ByVal parameterName As String, ByVal dbTypeValue As DbType) As IDbDataParameter

		''' <summary>
		''' ���̓p�����[�^��ǉ�����
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <param name="dbTypeValue">�p�����[�^�̌^</param>
		''' <param name="size">�p�����[�^�̃T�C�Y</param>
		''' <returns>�p�����[�^�C���X�^���X</returns>
		''' <remarks></remarks>
		Function AddInParameter(ByVal parameterName As String, ByVal dbTypeValue As DbType, ByVal size As Integer) As IDbDataParameter

		''' <summary>
		''' �o�̓p�����[�^��ǉ�����
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <returns>�p�����[�^�C���X�^���X</returns>
		''' <remarks></remarks>
		Function AddOutParameter(ByVal parameterName As String) As IDbDataParameter

		''' <summary>
		''' �o�̓p�����[�^��ǉ�����
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <param name="dbTypeValue">�p�����[�^�̌^</param>
		''' <returns>�p�����[�^�C���X�^���X</returns>
		''' <remarks></remarks>
		Function AddOutParameter(ByVal parameterName As String, ByVal dbTypeValue As DbType) As IDbDataParameter

		''' <summary>
		''' �p�����[�^���ɖ߂�l�����邩�Ԃ�
		''' </summary>
		''' <returns>True �͖߂�l�L��AFalse �͖߂�l����</returns>
		''' <remarks></remarks>
		Function HaveOutParameter() As Boolean

		''' <summary>
		''' �o�̓p�����[�^���Q�Ƃ���
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <returns>�o�̓p�����[�^�l</returns>
		''' <remarks></remarks>
		Function GetParameterValue(ByVal parameterName As String) As Object

#End Region

		''' <summary>
		''' �R���p�C���ς݂�SQL�ɂ���
		''' </summary>
		''' <remarks>
		''' �����\�b�h���s�O�ɗ\�� <see cref="AddInParameter"/> ���g�p���ăp�����[�^��ݒ肵�Ă����Ă��������B<br/>
		''' </remarks>
		Sub Prepare()

		''' <summary>
		''' SQL���s�I
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function Execute() As Integer

	End Interface

End Namespace
