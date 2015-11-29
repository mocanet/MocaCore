
Imports System.Reflection

Namespace Exceptions

	''' <summary>
	''' �J�X�^�����ʗ�O�N���X
	''' </summary>
	''' <remarks></remarks>
	Public Class CommonException
		Inherits ApplicationException

		''' <summary>���ɂȂ��O</summary>
		Private _baseException As Exception
		''' <summary>�G���[���b�Z�[�W</summary>
		Protected myMessage As String

		''' <summary>�s�v���ǂ������ؒ�</summary>
		Protected myStatckTrace As StackTrace

#Region " Constructor/DeConstructor "

		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="Message">�G���[���b�Z�[�W</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal Message As String)
			MyBase.New(Message)
			Me.NewMessage = Message
			myStatckTrace = New StackTrace(True)
		End Sub

		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="ex">��O�C���X�^���X</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal ex As Exception)
			MyBase.New("", ex)
			_baseException = ex
		End Sub

		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="ex">��O�C���X�^���X</param>
		''' <param name="Message">�G���[���b�Z�[�W</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal ex As Exception, ByVal Message As String)
			MyBase.New(Message, ex)
			_baseException = ex
			Me.NewMessage = Message
		End Sub

#End Region

#Region " Overrides "

		''' <summary>
		''' ���b�Z�[�W
		''' </summary>
		''' <value>���V�X�e���p�̃��b�Z�[�W�Ɍ��̗�O�̃��b�Z�[�W��t���������b�Z�[�W</value>
		''' <remarks>
		''' �t�H�[�}�b�g�͉��L�̂Ƃ���
		''' ���V�X�e���p���b�Z�[�W�i���̗�O���b�Z�[�W�j
		''' </remarks>
		Public Overrides ReadOnly Property Message() As String
			Get
				Dim buf As String

				If _baseException Is Nothing Then
					buf = String.Empty
				Else
					buf = _baseException.Message
				End If

				If myMessage Is Nothing Then
					Return buf
				End If

				If buf.Length = 0 Then
					Return myMessage
				End If

				Return myMessage & "�i" & buf & "�j"
			End Get
		End Property

		''' <summary>
		''' �X�^�b�N�g���[�X
		''' </summary>
		''' <value></value>
		''' <remarks>
		''' </remarks>
		Public Overrides ReadOnly Property StackTrace() As String
			Get
				If _baseException Is Nothing Then
					Return myStatckTrace.ToString
				End If

				Return _baseException.StackTrace
			End Get
		End Property

#End Region

#Region " Properties "

		''' <summary>���ɂȂ��O</summary>
		Public Property BaseException() As Exception
			Get
				Return _baseException
			End Get
			Set(ByVal value As Exception)
				_baseException = value
			End Set
		End Property
		''' <summary>
		''' �G���[���b�Z�[�W���b�Z�[�W
		''' </summary>
		''' <value>���V�X�e���p�ɐݒ肵�����b�Z�[�W</value>
		''' <remarks>
		''' </remarks>
		Public WriteOnly Property NewMessage() As String
			Set(ByVal Value As String)
				myMessage = Value
			End Set
		End Property

		''' <summary>
		''' �I���W�i����O�̃��b�Z�[�W
		''' </summary>
		''' <value>�I���W�i����O�̃��b�Z�[�W���e</value>
		''' <remarks>
		''' </remarks>
		Public ReadOnly Property OrignalMessage() As String
			Get
				If _baseException Is Nothing Then
					Return String.Empty
				Else
					Return _baseException.Message
				End If
			End Get
		End Property

#End Region

		''' <summary>
		''' StackTrace��ۑ�����
		''' </summary>
		''' <param name="ex">����������O</param>
		''' <remarks>
		''' <see cref="Exception"/>�� private field �ł��� _remoteStackTraceString �� InnerException �� StackTrace ��ۑ�����B
		''' </remarks>
		Public Shared Sub SaveStackTraceToRemoteStackTraceString(ByVal ex As Exception)
			Dim remoteStackTraceString As FieldInfo
			remoteStackTraceString = GetType(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance Or BindingFlags.NonPublic)
			remoteStackTraceString.SetValue(ex, ex.StackTrace + Environment.NewLine)
		End Sub

	End Class

End Namespace
