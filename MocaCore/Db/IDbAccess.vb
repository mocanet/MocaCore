Imports System.Configuration
Imports System.Data.Common

Namespace Db

	''' <summary>
	''' DB�փA�N�Z�X����ׂ̊�{�I�ȋ@�\��񋟂���C���^�t�F�[�X
	''' </summary>
	''' <remarks>
	''' </remarks>
	Public Interface IDbAccess
		Inherits IDao, IDisposable

#Region " Property "

		''' <summary>
		''' �g�����U�N�V�����I�u�W�F�N�g
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		ReadOnly Property Transaction() As IDbTransaction

		''' <summary>
		''' �g�����U�N�V�����X�R�[�v�I�u�W�F�N�g
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		ReadOnly Property TransactionScope() As Transactions.TransactionScope

#End Region
#Region " Transaction "

		''' <summary>
		''' �g�����U�N�V�����X�R�[�v���쐬����
		''' </summary>
		''' <returns>�g�����U�N�V�����X�R�[�v</returns>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Function NewTransactionScope() As Transactions.TransactionScope

		''' <summary>
		''' �g�����U�N�V�����X�R�[�v����������
		''' </summary>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Sub TransactionComplete()

		''' <summary>
		''' �g�����U�N�V�������J�n����
		''' </summary>
		''' <remarks>
		''' �g�����U�N�V�������g�p����ꍇ�͎��O��DB�ւ̐ڑ����K�v�ȈׁA������DB�Ƃ̐ڑ����s���܂��B<br/>
		''' �ʏ�́A<see cref="TransactionScope"/>���g�p���Ă��������B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Sub TransactionStart()

		''' <summary>
		''' ����DBAccess�N���X�ƃg�����U�N�V�����𓯂��ɂ���
		''' </summary>
		''' <param name="dba">��������DbAccess�C���X�^���X</param>
		''' �ʏ�́A<see cref="TransactionScope"/>���g�p���Ă��������B
		''' <remarks>
		''' �R�l�N�V�����I�u�W�F�N�g�ƃg�����U�N�V�����I�u�W�F�N�g���w�肳�ꂽDbAccess�̃I�u�W�F�N�g�ŏ㏑�����܂��B
		''' </remarks>
		Sub TransactionBinding(ByVal dba As IDbAccess)

		''' <summary>
		''' �g�����U�N�V�������I������i�R�~�b�g�j
		''' </summary>
		''' <remarks>
		''' DB�Ƃ̐ڑ���ؒf���܂��B
		''' �ʏ�́A<see cref="TransactionScope"/>���g�p���Ă��������B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Sub TransactionEnd()

		''' <summary>
		''' �g�����U�N�V���������[���o�b�N����
		''' </summary>
		''' <remarks>
		''' DB�Ƃ̐ڑ���ؒf���܂��B
		''' �ʏ�́A<see cref="TransactionScope"/>���g�p���Ă��������B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Sub TransactionRollback()

#End Region
#Region " Execute "

		''' <summary>
		''' INSERT���̎��s
		''' </summary>
		''' <param name="commandWrapper">INSERT�������s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns>�X�V����</returns>
		''' <remarks>
		''' �����\�b�h���g�p����ꍇ�́A�g�����U�N�V�����̊J�n<see cref="DBAccess.TransactionStart"></see>�A�I��<see cref="DBAccess.TransactionEnd"></see>���s���Ă��������B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandInsert) As Integer

		''' <summary>
		''' UPDATE���̎��s
		''' </summary>
		''' <param name="commandWrapper">UPDATE�������s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns>�X�V����</returns>
		''' <remarks>
		''' �����\�b�h���g�p����ꍇ�́A�g�����U�N�V�����̊J�n<see cref="DBAccess.TransactionStart"></see>�A�I��<see cref="DBAccess.TransactionEnd"></see>���s���Ă��������B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandUpdate) As Integer

		''' <summary>
		''' DELETE���̎��s
		''' </summary>
		''' <param name="commandWrapper">DELETE�������s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns>�X�V����</returns>
		''' <remarks>
		''' �����\�b�h���g�p����ꍇ�́A�g�����U�N�V�����̊J�n<see cref="DBAccess.TransactionStart"></see>�A�I��<see cref="DBAccess.TransactionEnd"></see>���s���Ă��������B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandDelete) As Integer

		''' <summary>
		''' �X�g�A�h�̎��s
		''' </summary>
		''' <param name="commandWrapper">�X�g�A�h�����s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns>�X�V����</returns>
		''' <remarks>
		''' �����\�b�h���g�p����ꍇ�́A�g�����U�N�V�����̊J�n<see cref="DBAccess.TransactionStart"></see>�A�I��<see cref="DBAccess.TransactionEnd"></see>���s���Ă��������B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandStoredProcedure) As Integer

		''' <summary>
		''' DDL�̎��s
		''' </summary>
		''' <param name="commandWrapper">DDL�����s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns>�X�V����</returns>
		''' <remarks>
		''' �����\�b�h���g�p����ꍇ�́A�g�����U�N�V�����̊J�n<see cref="DBAccess.TransactionStart"></see>�A�I��<see cref="DBAccess.TransactionEnd"></see>���s���Ă��������B
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandDDL) As Integer

		''' <summary>
		''' �f�[�^���X�V
		''' </summary>
		''' <param name="commandWrapper">�X�V�����s����ׂ�DBCommand�̃��b�p�[�C���X�^���X</param>
		''' <returns></returns>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DB�A�N�Z�X�ŃG���[����������
		''' </exception>
		Overloads Function ExecuteNonQuery(ByVal commandWrapper As IDbCommandSelect4Update) As Integer

#End Region

	End Interface

End Namespace
