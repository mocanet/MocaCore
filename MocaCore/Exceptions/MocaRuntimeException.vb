
Namespace Exceptions

	''' <summary>
	''' �����C�u�����̎��s����O�̊�{�N���X
	''' </summary>
	''' <remarks></remarks>
	<Serializable()> _
	Public Class MocaRuntimeException
		Inherits CommonException

#Region " Constructor/DeConstructor "

		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="Message">�G���[���b�Z�[�W</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal Message As String)
			MyBase.New(Message)
		End Sub

		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="ex">��O�C���X�^���X</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal ex As Exception)
			MyBase.New(ex)
		End Sub

		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="ex">��O�C���X�^���X</param>
		''' <param name="Message">�G���[���b�Z�[�W</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal ex As Exception, ByVal Message As String)
			MyBase.New(ex, Message)
		End Sub

#End Region

	End Class

End Namespace
