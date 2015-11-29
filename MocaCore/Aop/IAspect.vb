
Namespace Aop

	''' <summary>
	''' 横断的な関心事が持つ振る舞い（処理のこと）と、いつ振る舞いを適用するかを関連付けします。
	''' Advice(Interceptor)とPointcutをまとめたものをAspect(アスペクト)といいます。
	''' </summary>
	''' <remarks></remarks>
	Public Interface IAspect

		''' <summary>
		''' Advice(Interceptor) プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Advice() As IMethodInterceptor

		''' <summary>
		''' Pointcut プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Pointcut() As IPointcut

	End Interface

End Namespace
