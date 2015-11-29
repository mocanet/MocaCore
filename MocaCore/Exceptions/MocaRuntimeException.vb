
Namespace Exceptions

	''' <summary>
	''' 当ライブラリの実行時例外の基本クラス
	''' </summary>
	''' <remarks></remarks>
	<Serializable()> _
	Public Class MocaRuntimeException
		Inherits CommonException

#Region " Constructor/DeConstructor "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="Message">エラーメッセージ</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal Message As String)
			MyBase.New(Message)
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="ex">例外インスタンス</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal ex As Exception)
			MyBase.New(ex)
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="ex">例外インスタンス</param>
		''' <param name="Message">エラーメッセージ</param>
		''' <remarks>
		''' </remarks>
		Public Sub New(ByVal ex As Exception, ByVal Message As String)
			MyBase.New(ex, Message)
		End Sub

#End Region

	End Class

End Namespace
