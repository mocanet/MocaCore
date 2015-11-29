
Imports Moca.Exceptions

Namespace Db

	''' <summary>
	''' �f�[�^�x�[�X�A�N�Z�X�֌W�̗�O
	''' </summary>
	''' <remarks></remarks>
	Public Class DbAccessException
		Inherits MocaRuntimeException

		''' <summary>�g�p���Ă���DBAccess�C���X�^���X</summary>
		Protected useDBAccess As IDao

#Region " Constructor/DeConstructor "

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="useDBAccess">�g�p���Ă���DBAccess�C���X�^���X</param>
		''' <param name="Message">�G���[���b�Z�[�W</param>
		''' <remarks>
		''' </remarks>
		''' -----------------------------------------------------------------------------
		Public Sub New(ByVal useDBAccess As IDao, ByVal Message As String)
			MyBase.New(Message)
			Me.useDBAccess = useDBAccess
		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="useDBAccess">�g�p���Ă���DBAccess�C���X�^���X</param>
		''' <param name="ex">��O�C���X�^���X</param>
		''' <remarks>
		''' </remarks>
		''' -----------------------------------------------------------------------------
		Public Sub New(ByVal useDBAccess As IDao, ByVal ex As Exception)
			MyBase.New(ex)
			Me.useDBAccess = useDBAccess
		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="useDBAccess">�g�p���Ă���DBAccess�C���X�^���X</param>
		''' <param name="ex">��O�C���X�^���X</param>
		''' <param name="Message">�G���[���b�Z�[�W</param>
		''' <remarks>
		''' </remarks>
		''' -----------------------------------------------------------------------------
		Public Sub New(ByVal useDBAccess As IDbAccess, ByVal ex As Exception, ByVal Message As String)
			MyBase.New(ex, Message)
			Me.useDBAccess = useDBAccess
		End Sub

#End Region

		''' <summary>
		''' �G���[�ԍ�����
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetErrors() As String()
			If BaseException Is Nothing Then
				Return Nothing
			End If

			Return useDBAccess.Helper.ErrorNumbers(BaseException)
		End Function

		''' <summary>
		''' �d���G���[������������O�ɑ��݂��邩�Ԃ�
		''' </summary>
		''' <returns>True:���݂���AFalse:���݂��Ȃ�</returns>
		''' <remarks>
		''' </remarks>
		Public Function HasSqlNativeErrorDuplicationPKey() As Boolean
			If BaseException Is Nothing Then
				Return False
			End If

			Return useDBAccess.Helper.HasSqlNativeErrorDuplicationPKey(BaseException)
		End Function

		''' <summary>
		''' �^�C���A�E�g�G���[������������O�ɑ��݂��邩�Ԃ�
		''' </summary>
		''' <returns>True:���݂���AFalse:���݂��Ȃ�</returns>
		''' <remarks>
		''' </remarks>
		Public Function HasSqlNativeErrorTimtout() As Boolean
			If BaseException Is Nothing Then
				Return False
			End If

			Return useDBAccess.Helper.HasSqlNativeErrorTimtout(BaseException)
		End Function

		''' <summary>
		''' �w�肳�ꂽ�G���[�ԍ�������������O�ɑ��݂��邩�Ԃ�
		''' </summary>
		''' <param name="errorNumber">�G���[�ԍ�</param>
		''' <returns>True:���݂���AFalse:���݂��Ȃ�</returns>
		''' <remarks>
		''' </remarks>
		Public Function HasSqlNativeError(ByVal errorNumber As Long) As Boolean
			If BaseException Is Nothing Then
				Return False
			End If

			Return HasSqlNativeError(BaseException, errorNumber)
		End Function

		''' <summary>
		''' �w�肳�ꂽ�G���[�ԍ�������������O�ɑ��݂��邩�Ԃ�
		''' </summary>
		''' <param name="ex">��O</param>
		''' <param name="errorNumber">�G���[�ԍ�</param>
		''' <returns>True:���݂���AFalse:���݂��Ȃ�</returns>
		''' <remarks>
		''' </remarks>
		Protected Function hasSqlNativeError(ByVal ex As Exception, ByVal errorNumber As Long) As Boolean
			Return useDBAccess.Helper.HasSqlNativeError(ex, errorNumber)
		End Function

	End Class

End Namespace
