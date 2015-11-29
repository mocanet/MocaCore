
Namespace Db

	''' <summary>
	''' DB�A�N�Z�X�̊e�v���p�C�_�[�ɑΉ������w���p�[�̃C���^�t�F�[�X
	''' </summary>
	''' <remarks>
	''' �eDB�x���_�[���ɈقȂ镔�����z������ׂ̃C���^�t�F�[�X�ł��B<br/>
	''' </remarks>
	Public Interface IDbAccessHelper
		Inherits IDisposable

		''' <summary>
		''' �G���[�̌�����Ԃ�
		''' </summary>
		''' <param name="ex">�G���[�������擾��������O</param>
		''' <returns>�G���[����</returns>
		''' <remarks>
		''' </remarks>
		Function ErrorCount(ByVal ex As Exception) As Integer

		''' <summary>
		''' �G���[�ԍ���Ԃ�
		''' </summary>
		''' <param name="ex">�G���[�ԍ����擾��������O</param>
		''' <returns>�G���[�ԍ��z��</returns>
		''' <remarks>
		''' </remarks>
		Function ErrorNumbers(ByVal ex As Exception) As String()

		''' <summary>
		''' �w�肳�ꂽ�G���[�ԍ�������������O�ɑ��݂��邩�Ԃ�
		''' </summary>
		''' <param name="ex">�ΏۂƂȂ��O</param>
		''' <param name="errorNumber">�G���[�ԍ�</param>
		''' <returns>True:���݂���AFalse:���݂��Ȃ�</returns>
		''' <remarks>
		''' </remarks>
		Function HasSqlNativeError(ByVal ex As Exception, ByVal errorNumber As Long) As Boolean

		''' <summary>
		''' �d���G���[������������O�ɑ��݂��邩�Ԃ�
		''' </summary>
		''' <param name="ex">�ΏۂƂȂ��O</param>
		''' <returns>True:���݂���AFalse:���݂��Ȃ�</returns>
		''' <remarks>
		''' </remarks>
		Function HasSqlNativeErrorDuplicationPKey(ByVal ex As Exception) As Boolean

		''' <summary>
		''' �^�C���A�E�g�G���[������������O�ɑ��݂��邩�Ԃ�
		''' </summary>
		''' <param name="ex">�ΏۂƂȂ��O</param>
		''' <returns>True:���݂���AFalse:���݂��Ȃ�</returns>
		''' <remarks>
		''' </remarks>
		Function HasSqlNativeErrorTimtout(ByVal ex As Exception) As Boolean

		''' <summary>
		''' �X�L�[�}�ɑ��݂���e�[�u�������擾����
		''' </summary>
		''' <param name="tablename">�擾�������e�[�u����</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSchemaTable(ByVal tablename As String) As DbInfoTable

		''' <summary>
		''' �X�L�[�}�ɑ��݂���e�[�u�������擾����
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSchemaTables() As DbInfoTableCollection

		''' <summary>
		''' �X�L�[�}�ɑ��݂���e�[�u�������擾����
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSchemaColumns(ByVal table As DbInfoTable) As DbInfoColumnCollection

		''' <summary>
		''' �X�L�[�}�ɑ��݂���v���V�[�W�������擾����
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSchemaProcedures() As DbInfoProcedureCollection

		''' <summary>
		''' �X�L�[�}�ɑ��݂���֐������擾����
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSchemaFunctions() As DbInfoFunctionCollection

		''' <summary>
		''' �X�g�A�h�̃p�����[�^���擾����
		''' </summary>
		''' <param name="cmd">���s�Ώۂ�DB�R�}���h</param>
		''' <remarks></remarks>
		Sub RefreshProcedureParameters(ByVal cmd As IDbCommand)

		''' <summary>
		''' SQL�X�e�[�^�X�̃p�����[�^����ϊ�����B
		''' </summary>
		''' <param name="name">�p�����[�^��</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CDbParameterName(ByVal name As String) As String

		''' <summary>
		''' SQL�v���[�X�t�H���_�̃}�[�N��Ԃ��B
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property PlaceholderMark() As String

	End Interface

End Namespace
