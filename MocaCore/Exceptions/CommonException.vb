
Imports System.Reflection

Namespace Exceptions

	''' <summary>
	''' カスタム共通例外クラス
	''' </summary>
	''' <remarks></remarks>
	Public Class CommonException
		Inherits ApplicationException

		''' <summary>元になる例外</summary>
		Private _baseException As Exception
		''' <summary>エラーメッセージ</summary>
		Protected myMessage As String

		''' <summary>不要かどうか検証中</summary>
		Protected myStatckTrace As StackTrace

#Region " Constructor/DeConstructor "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="Message">エラーメッセージ</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal Message As String)
			MyBase.New(Message)
			Me.NewMessage = Message
			myStatckTrace = New StackTrace(True)
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="ex">例外インスタンス</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal ex As Exception)
			MyBase.New("", ex)
			_baseException = ex
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="ex">例外インスタンス</param>
		''' <param name="Message">エラーメッセージ</param>
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
		''' メッセージ
		''' </summary>
		''' <value>当システム用のメッセージに元の例外のメッセージを付加したメッセージ</value>
		''' <remarks>
		''' フォーマットは下記のとおり
		''' 当システム用メッセージ（元の例外メッセージ）
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

				Return myMessage & "（" & buf & "）"
			End Get
		End Property

		''' <summary>
		''' スタックトレース
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

		''' <summary>元になる例外</summary>
		Public Property BaseException() As Exception
			Get
				Return _baseException
			End Get
			Set(ByVal value As Exception)
				_baseException = value
			End Set
		End Property
		''' <summary>
		''' エラーメッセージメッセージ
		''' </summary>
		''' <value>当システム用に設定したメッセージ</value>
		''' <remarks>
		''' </remarks>
		Public WriteOnly Property NewMessage() As String
			Set(ByVal Value As String)
				myMessage = Value
			End Set
		End Property

		''' <summary>
		''' オリジナル例外のメッセージ
		''' </summary>
		''' <value>オリジナル例外のメッセージ内容</value>
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
		''' StackTraceを保存する
		''' </summary>
		''' <param name="ex">発生した例外</param>
		''' <remarks>
		''' <see cref="Exception"/>の private field である _remoteStackTraceString に InnerException の StackTrace を保存する。
		''' </remarks>
		Public Shared Sub SaveStackTraceToRemoteStackTraceString(ByVal ex As Exception)
			Dim remoteStackTraceString As FieldInfo
			remoteStackTraceString = GetType(Exception).GetField("_remoteStackTraceString", BindingFlags.Instance Or BindingFlags.NonPublic)
			remoteStackTraceString.SetValue(ex, ex.StackTrace + Environment.NewLine)
		End Sub

	End Class

End Namespace
