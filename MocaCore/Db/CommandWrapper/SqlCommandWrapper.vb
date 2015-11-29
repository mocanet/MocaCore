
Imports System.Text.RegularExpressions

Namespace Db.CommandWrapper

	''' <summary>
	''' DBCommand�̃��b�s���O���ۃN���X
	''' </summary>
	''' <remarks></remarks>
	Public MustInherit Class SqlCommandWrapper
		Implements IDbCommandSql

		''' <summary>�e�ƂȂ�DBAccess�C���X�^���X</summary>
		Protected dba As IDao
		''' <summary>���s����DBCommand�C���X�^���X</summary>
		Protected cmd As IDbCommand
		''' <summary>�R���p�C���ς݂�SQL���g�����ǂ���</summary>
		Private _preparedStatement As Boolean
		''' <summary>�v���[�X�t�H���_�z��</summary>
		Private _placeholders() As String
		''' <summary>SQL���̃I���W�i��</summary>
		Private _originalCommandText As String
		''' <summary>���s��̖߂�l</summary>
		Private _outputParams As Hashtable

		''' <summary>�f�[�^�x�[�X����擾�����f�[�^�̊i�[��ƂȂ� Entity ���쐬����</summary>
		Protected entityBuilder As New EntityBuilder

#Region " Constructor/DeConstructor "

		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="dba">�e�ƂȂ�DBAccess�C���X�^���X</param>
		''' <param name="cmd">���s����DBCommand�C���X�^���X</param>
		''' <remarks>
		''' </remarks>
		Friend Sub New(ByVal dba As IDao, ByVal cmd As IDbCommand)
			Me.dba = dba
			Me.cmd = cmd
			_preparedStatement = False
			_originalCommandText = cmd.CommandText
			_outputParams = New Hashtable
		End Sub

#End Region

#Region " IDisposable Support "

		Private _disposedValue As Boolean = False		' �d������Ăяo�������o����ɂ�

		''' <summary>
		''' IDisposable
		''' </summary>
		''' <param name="disposing"></param>
		''' <remarks></remarks>
		Protected Overridable Sub Dispose(ByVal disposing As Boolean)
			If Not Me._disposedValue Then
				If disposing Then
					' TODO: �����I�ɌĂяo���ꂽ�Ƃ��Ƀ}�l�[�W ���\�[�X��������܂�
				End If

				' TODO: ���L�̃A���}�l�[�W ���\�[�X��������܂�
			End If
			Me._disposedValue = True
		End Sub

		' ���̃R�[�h�́A�j���\�ȃp�^�[���𐳂��������ł���悤�� Visual Basic �ɂ���Ēǉ�����܂����B
		Public Sub Dispose() Implements IDisposable.Dispose
			' ���̃R�[�h��ύX���Ȃ��ł��������B�N���[���A�b�v �R�[�h����� Dispose(ByVal disposing As Boolean) �ɋL�q���܂��B
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
#End Region

#Region " Implements IDbCommandSql "

#Region " Properties "

		''' <summary>
		''' ���s����DBCommand�C���X�^���X���Q��
		''' </summary>
		''' <value>���s����DBCommand�C���X�^���X</value>
		''' <remarks>
		''' </remarks>
        Public ReadOnly Property Command() As IDbCommand Implements IDbCommandSql.Command
            Get
                Return cmd
            End Get
        End Property

		''' <summary>
		''' �R���p�C���ς݂�SQL���g�����ǂ������w��
		''' </summary>
		''' <value>
		''' True:�g�p����
		''' False:�g�p���Ȃ�
		''' </value>
		''' <remarks>
		''' </remarks>
        Public Property PreparedStatement() As Boolean Implements IDbCommandSql.PreparedStatement
            Get
                Return _preparedStatement
            End Get
            Set(ByVal Value As Boolean)
                _preparedStatement = Value
            End Set
        End Property

		''' <summary>
		''' SQL��
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CommandText() As String Implements IDbCommandSql.CommandText
			Get
				Return _originalCommandText
			End Get
			Set(ByVal value As String)
				_originalCommandText = value
				Me.cmd.CommandText = value
			End Set
		End Property

		''' <summary>
		''' ���s��̖߂�l��Ԃ�
		''' </summary>
		''' <value></value>
		''' <returns>�߂�l</returns>
		''' <remarks></remarks>
		Public ReadOnly Property ResultOutputParam() As System.Collections.Hashtable Implements IDbCommandSql.ResultOutParameter
			Get
				Return _outputParams
			End Get
		End Property

#End Region

#Region " DbDataParameter "

		''' <summary>
		''' ���̓p�����[�^��ݒ肷��
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <param name="value">�l</param>
		''' <returns>�p�����[�^�C���X�^���X</returns>
		''' <remarks>
		''' </remarks>
		Public Function SetParameter(ByVal parameterName As String, ByVal value As Object) As IDbDataParameter Implements IDbCommandSql.SetParameter
			Dim param As IDbDataParameter

			param = addParameter(parameterName, value)
			param.Direction = ParameterDirection.Input

			Return param
		End Function

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
		Public Function SetParameter(ByVal parameterName As String, ByVal values As Array) As String Implements IDbCommandSql.SetParameter
			Dim sql As String
			Dim whereIn As String

			whereIn = String.Empty
			sql = cmd.CommandText
			parameterName = dba.Helper.CDbParameterName(parameterName)

			If sql.IndexOf(parameterName) < 0 Then
				sql = _originalCommandText
			End If

			Dim lst As ArrayList

			lst = New ArrayList
			For Each value As Object In values
				lst.Add("'" & CStr(value).Replace("'", "''") & "'")
			Next

			whereIn = "(" & String.Join(",", DirectCast(lst.ToArray(GetType(String)), String())) & ")"
			sql = sql.Replace(parameterName, whereIn)
			cmd.CommandText = sql

			Return whereIn
		End Function

		''' <summary>
		''' ���̓p�����[�^��ǉ�����
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <param name="dbTypeValue">�p�����[�^�̌^</param>
		''' <returns>�p�����[�^�C���X�^���X</returns>
		''' <remarks>
		''' </remarks>
		Public Function AddInParameter(ByVal parameterName As String, ByVal dbTypeValue As DbType) As IDbDataParameter Implements IDbCommandSql.AddInParameter
			Dim param As IDbDataParameter

			param = addParameter(parameterName, Nothing)
			param.Direction = ParameterDirection.Input
			param.DbType = dbTypeValue

			Return param
		End Function

		''' <summary>
		''' ���̓p�����[�^��ǉ�����
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <param name="dbTypeValue">�p�����[�^�̌^</param>
		''' <param name="size">�p�����[�^�̃T�C�Y</param>
		''' <returns>�p�����[�^�C���X�^���X</returns>
		''' <remarks>
		''' </remarks>
		Public Function AddInParameter(ByVal parameterName As String, ByVal dbTypeValue As DbType, ByVal size As Integer) As IDbDataParameter Implements IDbCommandSql.AddInParameter
			Dim param As IDbDataParameter

			param = AddInParameter(parameterName, dbTypeValue)
			param.Size = size

			Return param
		End Function

		''' <summary>
		''' �o�̓p�����[�^��ǉ�����
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <returns>�p�����[�^�C���X�^���X</returns>
		''' <remarks>
		''' </remarks>
		Public Function AddOutParameter(ByVal parameterName As String) As IDbDataParameter Implements IDbCommandSql.AddOutParameter
			Dim param As IDbDataParameter

			param = addParameter(parameterName, Nothing)
			param.Direction = ParameterDirection.Output

			Return param
		End Function

		''' <summary>
		''' �o�̓p�����[�^��ǉ�����
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <param name="dbTypeValue">�p�����[�^�̌^</param>
		''' <returns>�p�����[�^�C���X�^���X</returns>
		''' <remarks>
		''' </remarks>
		Public Function AddOutParameter(ByVal parameterName As String, ByVal dbTypeValue As DbType) As IDbDataParameter Implements IDbCommandSql.AddOutParameter
			Dim param As IDbDataParameter

			param = AddOutParameter(parameterName)
			param.Direction = ParameterDirection.Output
			param.DbType = dbTypeValue

			Return param
		End Function

		''' <summary>
		''' �p�����[�^���ɖ߂�l�����邩�Ԃ�
		''' </summary>
		''' <returns>True �͖߂�l�L��AFalse �͖߂�l����</returns>
		''' <remarks></remarks>
		Public Function HaveOutParameter() As Boolean Implements IDbCommandSql.HaveOutParameter
			Dim ee As IEnumerator = cmd.Parameters.GetEnumerator
			While ee.MoveNext
				Dim param As IDbDataParameter
				param = DirectCast(ee.Current, IDbDataParameter)
				If param.Direction = ParameterDirection.InputOutput Or param.Direction = ParameterDirection.Output Then
					Return True
				End If
			End While
			Return False
		End Function

		''' <summary>
		''' �o�̓p�����[�^���Q�Ƃ���
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <returns>�o�̓p�����[�^�l</returns>
		''' <remarks>
		''' </remarks>
		Public Function GetParameterValue(ByVal parameterName As String) As Object Implements IDbCommandSql.GetParameterValue
			parameterName = dba.Helper.CDbParameterName(parameterName)
			Return cmd.Parameters.Item(parameterName)
		End Function

#End Region

		''' <summary>
		''' SQL���s
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public MustOverride Function Execute() As Integer Implements IDbCommandSql.Execute

		''' <summary>
		''' �R���p�C���ς݂�SQL�ɂ���
		''' </summary>
		''' <remarks>
		''' �����\�b�h���s�O�ɗ\�� <see cref="AddInParameter"/> ���g�p���ăp�����[�^��ݒ肵�Ă����Ă��������B<br/>
		''' </remarks>
		Public Sub Prepare() Implements IDbCommandSql.Prepare
			cmd.Prepare()
			PreparedStatement = True
		End Sub

#End Region

#Region " Methods "

		''' <summary>
		''' �p�����[�^��ǉ����͎擾����
		''' </summary>
		''' <param name="parameterName">�p�����[�^��</param>
		''' <param name="value">�p�����[�^�̒l</param>
		''' <returns>�p�����[�^�C���X�^���X</returns>
		''' <remarks>
		''' </remarks>
		Protected Function addParameter(ByVal parameterName As String, ByVal value As Object) As IDbDataParameter
			Dim param As IDbDataParameter

			parameterName = dba.Helper.CDbParameterName(parameterName)

			If cmd.Parameters.Contains(parameterName) Then
				param = DirectCast(cmd.Parameters.Item(parameterName), IDbDataParameter)
				param.Value = DbUtil.CNull(value)
			Else
				param = dba.Dbms.ProviderFactory.CreateParameter()
				param.ParameterName = parameterName
				param.Value = DbUtil.CNull(value)
				cmd.Parameters.Add(param)
			End If

			Return param
		End Function

		''' <summary>
		''' �v���[�X�t�H���_���擾
		''' </summary>
		''' <remarks>
		''' ����̊g���̂��߂̎���<br/>
		''' �����A�����[�X���邩�͕s��
		''' </remarks>
		Protected Sub cnvPlaceholder()
			_placeholders = _getPlaceholder()
		End Sub

		''' <summary>
		''' SQL�R�}���h�̃v���[�X�t�H���_��Ԃ��B
		''' </summary>
		''' <returns>�v���[�X�t�H���_���̔z��</returns>
		''' <remarks>
		''' �v���[�X�t�H���_�́u/*name*/�v�Ƃ��Ă��������B<br/>
		''' </remarks>
		Private Function _getPlaceholder() As String()
			Dim params As ArrayList
			Dim r As New Regex("/\*(.*?)\*/", RegexOptions.IgnoreCase Or RegexOptions.Singleline)

			' ���K�\���ƈ�v����Ώۂ����ׂČ��� 
			Dim mc As MatchCollection = r.Matches(Me.CommandText)

			params = New ArrayList

			' ���K�\���Ɉ�v�����O���[�v�̕������\�� 
			For Each m As Match In mc
				If (Not m.Groups(1).Value.StartsWith(" ")) Then
					params.Add(m.Groups(1).Value)
				End If
			Next
			Return DirectCast(params.ToArray(GetType(String)), String())
		End Function

#End Region

	End Class

End Namespace
