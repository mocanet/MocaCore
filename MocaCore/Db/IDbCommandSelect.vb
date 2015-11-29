
Namespace Db

	''' <summary>
	''' SELECT�������s����ׂ�DBCommand�����b�s���O����C���^�t�F�[�X
	''' </summary>
	''' <remarks>
	''' �f�[�^���o�n�̃X�g�A�h���s�ł��g�p�o���܂����A�X�g�A�h�̎���<see cref="IDbCommandStoredProcedure"/>���g�p���Ă��������B<br/>
	''' <example>
	''' <code lang="vb">
	''' Using dba As IDbAccess = New DbAccess("Connection String")
	''' 	Using cmd As IDbCommandSelect = dba.CreateCommandSelect("SELECT * FROM TableName")
	''' 		If cmd.Execute() &lt;= 0 Then
	''' 			Return Nothing
	''' 		End If
	''' 		Return cmd.ResultDataSet
	''' 	End Using
	''' End Using
	''' </code>
	''' </example>
	''' </remarks>
	Public Interface IDbCommandSelect
		Inherits IDbCommandSql

#Region " Property "

		''' <summary>
		''' ExecuteReader �ɓn�� CommandBehavior
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks>
		''' <see cref="Execute"></see>�ȊO�ł͖�������܂��B
		''' <see cref="System.Data.SqlClient.SqlDataReader"></see>���g�p���Ă���ꍇ�̂ݗL���B
		''' </remarks>
		Property Behavior As CommandBehavior

		''' <summary>
		''' Select�������s�������ʂ�ݒ�^�Q��
		''' </summary>
		''' <value>Select�������s��������</value>
		''' <remarks></remarks>
		Property ResultDataSet() As DataSet

		''' <summary>
		''' DataSet���̐擪�e�[�u����Ԃ�
		''' </summary>
		''' <value></value>
		''' <returns>�擪�e�[�u��</returns>
		''' <remarks></remarks>
		ReadOnly Property Result1stTable() As DataTable

		''' <summary>
		''' DataSet���̐擪�e�[�u���ɑ��݂���s�f�[�^��Enumerator��Ԃ�
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Result1stTableRowEnumerator() As IEnumerator(Of DataRow)

#End Region

#Region " Methods "

		''' <summary>
		''' DataSet���̐擪�e�[�u����Ԃ�
		''' </summary>
		''' <typeparam name="T"></typeparam>
		''' <returns>�擪�e�[�u���̃f�[�^���w�肳�ꂽEntity���g�p�����z��ɕϊ����ĕԂ�</returns>
		''' <remarks>
		''' Execute ��ɓ����\�b�h�ŃG���e�B�e�B���擾������<see cref="Execute(OF T)"></see>���g�������������ŃX�e�b�v�����点�܂��B
		''' </remarks>
		<Obsolete("Execute(Of T)() ���g���悤�ɂ��Ă��������B")> _
		Function Result1stTableEntitis(Of T)() As T()

		''' <summary>
		''' DataSet���̐擪�e�[�u���̎w�肳�ꂽ�s��Ԃ�
		''' </summary>
		''' <typeparam name="T"></typeparam>
		''' <param name="index"></param>
		''' <returns>�擪�e�[�u���̃f�[�^���w�肳�ꂽEntity���g�p�����z��ɕϊ����ĕԂ�</returns>
		''' <remarks></remarks>
		Function Result1stTableEntity(Of T)(ByVal index As Integer) As T

		''' <summary>
		''' DataSet���̐擪�e�[�u����ConstantDataSet�^�ŕԂ�
		''' </summary>
		''' <param name="textColumnName">�e�L�X�g�Ƃ��Ĉ�����̖���</param>
		''' <param name="valueColumnName">�l�Ƃ��Ĉ�����̖���</param>
		''' <param name="blankRow">�󔒍s�̗L��</param>
		''' <param name="blankValue">�󔒍s�L��̂Ƃ��̋󔒍s�̒l</param>
		''' <param name="delm">�e�L�X�g�ƒl�̋�؂蕶��</param>
		''' <returns>ConstantDataSet</returns>
		''' <remarks>
		''' ��ɁA�R���{�{�b�N�X���Ŏg�p����ꍇ�Ɏg���܂��B
		''' </remarks>
		Overloads Function ResultConstantDataSet(ByVal textColumnName As String, ByVal valueColumnName As String, Optional ByVal blankRow As Boolean = False, Optional ByVal blankValue As Object = -1, Optional ByVal delm As String = " : ") As ConstantDataSet

		''' <summary>
		''' DataSet���̐擪�e�[�u����ConstantDataSet�^�ŕԂ�
		''' </summary>
		''' <param name="textColumnIndex">�e�L�X�g�Ƃ��Ĉ�����̈ʒu</param>
		''' <param name="valueColumnIndex">�l�Ƃ��Ĉ�����̈ʒu</param>
		''' <param name="blankRow">�󔒍s�̗L��</param>
		''' <param name="blankValue">�󔒍s�L��̂Ƃ��̋󔒍s�̒l</param>
		''' <param name="delm">�e�L�X�g�ƒl�̋�؂蕶��</param>
		''' <returns>ConstantDataSet</returns>
		''' <remarks>
		''' ��ɁA�R���{�{�b�N�X���Ŏg�p����ꍇ�Ɏg���܂��B
		''' </remarks>
		Overloads Function ResultConstantDataSet(ByVal textColumnIndex As Integer, ByVal valueColumnIndex As Integer, Optional ByVal blankRow As Boolean = False, Optional ByVal blankValue As Object = -1, Optional ByVal delm As String = " : ") As ConstantDataSet

		''' <summary>
		''' �N�G�������s���iExecuteScalar�j�A���̃N�G�����Ԃ����ʃZ�b�g�̍ŏ��̍s�ɂ���ŏ��̗��Ԃ��܂��B�]���ȗ�܂��͍s�͖�������܂��B
		''' </summary>
		''' <returns>���ʃZ�b�g�̍ŏ��̍s�ɂ���ŏ��̗�B</returns>
		''' <remarks>
		''' �����\�b�h�͗\�߃f�[�^�x�[�X���I�[�v�����Ă����K�v������܂����A
		''' �I�[�v������Ă��Ȃ��Ƃ��́A�����ŃI�[�v�����ďI�����ɃN���[�Y���܂��B<br/>
		''' �ڍׂ́A<seealso cref="IDbCommand.ExecuteScalar"/> ���Q�Ƃ��Ă��������B
		''' </remarks>
		Function ExecuteScalar() As Object

		''' <summary>
		''' �N�G�������s���iExecuteReader�j�A�w�肳�ꂽ�G���e�B�e�B�ɕϊ����ĕԂ��܂��B
		''' </summary>
		''' <typeparam name="T">�G���e�B�e�B</typeparam>
		''' <returns>�G���e�B�e�B�̃��X�g</returns>
		''' <remarks>
		''' �����\�b�h�͗\�߃f�[�^�x�[�X���I�[�v�����Ă����K�v������܂����A
		''' �I�[�v������Ă��Ȃ��Ƃ��́A�����ŃI�[�v�����ďI�����ɃN���[�Y���܂��B<br/>
		''' �ڍׂ́A<seealso cref="IDbCommand.ExecuteReader"/> ���Q�Ƃ��Ă��������B<br/>
		''' <br/>
		''' �Ȃ��A�����\�b�h���g�p�����ꍇ�͌��ʂ��G���e�B�e�B�Ƃ��Ĉ������Ƃ�O��Ƃ��Ă��邽�߁A<see cref="DataSet"></see>��<see cref="DataTable"></see>�Ƃ��Ă͈����܂���B<br/>
		''' �����<see cref="ResultDataSet"></see>, <see cref="Result1stTable"></see>�Ȃǂ̃��\�b�h�͎g�p�ł��܂���B<br/>
		''' �o�b�`SQL�X�e�[�g�����g����<see cref="NextResult"></see>�ɂĎ��̌��ʂ��擾���Ă��������B
		''' </remarks>
		Overloads Function Execute(Of T)() As IList(Of T)

		''' <summary>
		''' ���̌��ʂ�Ԃ�
		''' </summary>
		''' <typeparam name="T">�G���e�B�e�B</typeparam>
		''' <returns>���݂��Ȃ��Ƃ��� Nothing ��������</returns>
		''' <remarks></remarks>
		Function NextResult(Of T)() As IList(Of T)

#End Region

	End Interface

End Namespace
