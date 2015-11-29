Imports Moca.Db.CommandWrapper

Namespace Db

	''' <summary>
	''' Data Access Object �̃C���^�t�F�[�X
	''' </summary>
	''' <remarks>
	''' �f�[�^�x�[�X�A�N�Z�X����ۂɍŒ���K�v�Ǝv����@�\��񋟂��܂��B<br/>
	''' </remarks>
	Public Interface IDao
		Inherits IDisposable

#Region " Property "

		''' <summary>
		''' DBMS
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		ReadOnly Property Dbms() As Dbms

		''' <summary>
		''' �R�l�N�V�����I�u�W�F�N�g
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		ReadOnly Property Connection() As IDbConnection

		''' <summary>
		''' �A�_�v�^�I�u�W�F�N�g
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		ReadOnly Property Adapter() As IDbDataAdapter

		''' <summary>
		''' �w���p�[�N���X��Ԃ�
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Helper() As IDbAccessHelper

		''' <summary>
		''' ���݂̃R�}���h���b�p�[�N���X��Ԃ�
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property CommandWrapper() As IDbCommandSql

		''' <summary>
		''' �R�}���h���s����L���w��
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Property ExecuteHistory() As Boolean

		''' <summary>
		''' �X�V�R�}���h���s����L���w��
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Property ExecuteUpdateHistory() As Boolean

		''' <summary>
		''' �R�}���h���s����
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property ExecuteHistories() As IList(Of String)

		''' <summary>
		''' ���[���o�b�N�L��
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Property RollbackStatus() As Boolean

#End Region
#Region " Check "

		''' <summary>
		''' �ڑ��m�F�ׂ̈Ɉ�x�ڑ����Ă݂�
		''' </summary>
		''' <remarks>
		''' �ڑ��o�����Ƃ��͐ؒf���܂��B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Sub CheckConnect()

#End Region
#Region " Create "

		''' <summary>
		''' SELECT�������s����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">SELECT��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandSelect(ByVal commandText As String) As IDbCommandSelect

		''' <summary>
		''' SELECT�������s���ADataSet���g����UPDATE����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">SELECT��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandSelect4Update(ByVal commandText As String) As IDbCommandSelect4Update

		''' <summary>
		''' INSERT�������s����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">INSERT��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandInsert(ByVal commandText As String) As IDbCommandInsert

		''' <summary>
		''' UPDATE�������s����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">UPDATE��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandUpdate(ByVal commandText As String) As IDbCommandUpdate

		''' <summary>
		''' DELETE�������s����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">DELETE��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandDelete(ByVal commandText As String) As IDbCommandDelete

		''' <summary>
		''' StoredProcedure�����s����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">�X�g�A�h��</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandStoredProcedure(ByVal commandText As String) As IDbCommandStoredProcedure

		''' <summary>
		''' DDL�����s����ׂ�DBCommand�̃��b�p�[�N���X�𐶐�����B
		''' </summary>
		''' <param name="commandText">DDL��������</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CreateCommandDDL(ByVal commandText As String) As IDbCommandDDL

#End Region
#Region " Execute "

		''' <summary>
		''' �N�G�������s���A���̃N�G�����Ԃ����ʃZ�b�g�̍ŏ��̍s�ɂ���ŏ��̗��Ԃ��܂��B�]���ȗ�܂��͍s�͖�������܂��B
		''' </summary>
		''' <param name="commandWrapper"></param>
		''' <returns>���ʃZ�b�g�̍ŏ��̍s�ɂ���ŏ��̗�B</returns>
		''' <remarks>
		''' �����\�b�h�͗\�߃f�[�^�x�[�X���I�[�v�����Ă����K�v������܂����A
		''' �I�[�v������Ă��Ȃ��Ƃ��́A�����ŃI�[�v�����ďI�����ɃN���[�Y���܂��B<br/>
		''' �ڍׂ́A<seealso cref="IDbCommand.ExecuteScalar"/> ���Q�Ƃ��Ă��������B
		''' </remarks>
		Function ExecuteScalar(ByVal commandWrapper As IDbCommandSelect) As Object

		''' <summary>
		''' SELECT���̎��s
		''' </summary>
		''' <param name="commandWrapper">SELECT�������s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns>�f�[�^����</returns>
		''' <remarks>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Function Execute(ByVal commandWrapper As IDbCommandSelect) As Integer

		''' <summary>
		''' SELECT���̎��s
		''' </summary>
		''' <param name="commandWrapper">SELECT�������s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns>����</returns>
		''' <remarks>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Function Execute(Of T)(ByVal commandWrapper As IDbCommandSelect) As ISQLStatementResult

		''' <summary>
		''' SELECT���̎��s(���Adapter�𗘗p�����X�V���s���ꍇ)
		''' </summary>
		''' <param name="commandWrapper">SELECT�������s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns>�f�[�^����</returns>
		''' <remarks>
		''' SELECT���s��̃f�[�^�X�V��DataSet���g���čX�V����ꍇ�́A��������g�p���Ă��������B<br/>
		''' �\��Adapter��Command���֘A�t���܂��B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Function Execute(ByVal commandWrapper As IDbCommandSelect4Update) As Integer

		''' <summary>
		''' INSERT,UPDATE,DELETE���̎��s
		''' </summary>
		''' <param name="commandWrapper">���s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns>�X�V����</returns>
		''' <remarks>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandSql) As Integer

		''' <summary>
		''' �A�_�v�^�[�ɂ��X�V
		''' </summary>
		''' <param name="ds">�X�V����f�[�^</param>
		''' <param name="adp">�A�_�v�^�[</param>
		''' <returns>�X�V����</returns>
		''' <remarks></remarks>
		Function UpdateAdapter(ByVal ds As DataSet, ByVal adp As IDbDataAdapter) As Integer

#End Region

	End Interface

End Namespace
