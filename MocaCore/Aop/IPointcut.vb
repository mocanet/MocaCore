
Namespace Aop

	''' <summary>
	''' Joinpointのうち、Adviceを適用したいJoinpointを正規表現などを用いた条件を使用して絞り込むためのフィルタです。
	''' 例えば、Adviceを適用したいのは「add」ではじまるメソッドが実行された時だけだとすると、
	''' 条件を「add*」として絞り込まれたaddXxxメソッドが実行された時だけにAdviceが実行されるようにもできますし、
	''' 指定されたメソッド名が一致するときだけAdviceが実行されるようにするなどのフィルタを作成します。
	''' </summary>
	''' <remarks></remarks>
	Public Interface IPointcut

		''' <summary>
		''' 引数で渡されたメソッド名にAdviceを挿入するか確認します。
		''' </summary>
		''' <param name="pattern">メソッド名</param>
		''' <returns>TrueならAdviceを挿入する、FalseならAdviceは挿入されない</returns>
		''' <remarks></remarks>
		Function IsExecution(ByVal pattern As String) As Boolean

	End Interface

End Namespace
