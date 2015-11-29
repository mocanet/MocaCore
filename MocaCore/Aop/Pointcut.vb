
Namespace Aop

	''' <summary>
	''' Joinpointのうち、Adviceを適用したいJoinpointを正規表現などを用いた条件を使用して絞り込むためのフィルタです。
	''' 例えば、Adviceを適用したいのは「add」ではじまるメソッドが実行された時だけだとすると、
	''' 条件を「add*」として絞り込まれたaddXxxメソッドが実行された時だけにAdviceが実行されるようにもできます。
	''' ここでは指定されたメソッド文字列が一致するときだけAdviceが実行されるようにします。
	''' </summary>
	''' <remarks></remarks>
	Public Class Pointcut
		Implements IPointcut

		''' <summary>メソッド名の文字列リスト</summary>
		Private _patterns As IList(Of String)

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="names">メソッド名の文字列配列</param>
		''' <remarks></remarks>
		Public Sub New(ByVal names() As String)
			_patterns = New List(Of String)(names)
		End Sub

#End Region

#Region " プロパティ "

		''' <summary>
		''' メソッド名の文字列リスト
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Names() As IList(Of String)
			Get
				Return _patterns
			End Get
		End Property

#End Region

		''' <summary>
		''' 引数で渡されたメソッド名にAdviceを挿入するか確認します。
		''' </summary>
		''' <param name="pattern">メソッド名</param>
		''' <returns>TrueならAdviceを挿入する、FalseならAdviceは挿入されない</returns>
		''' <remarks></remarks>
		Public Function IsExecution(ByVal pattern As String) As Boolean Implements IPointcut.IsExecution
			Return _patterns.Contains(pattern)
		End Function
	End Class

End Namespace
