
Namespace Aop

	''' <summary>
	''' メソッドに対するInterceptorのインターフェイス
	''' </summary>
	''' <remarks></remarks>
	Public Interface IMethodInterceptor
		Inherits IInterceptor

		''' <summary>
		''' メソッド実行
		''' </summary>
		''' <param name="invocation">Interceptorからインターセプトされているメソッドの情報</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function Invoke(ByVal invocation As IMethodInvocation) As Object

	End Interface

End Namespace
