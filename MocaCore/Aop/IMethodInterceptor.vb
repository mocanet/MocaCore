
Namespace Aop

	''' <summary>
	''' ���\�b�h�ɑ΂���Interceptor�̃C���^�[�t�F�C�X
	''' </summary>
	''' <remarks></remarks>
	Public Interface IMethodInterceptor
		Inherits IInterceptor

		''' <summary>
		''' ���\�b�h���s
		''' </summary>
		''' <param name="invocation">Interceptor����C���^�[�Z�v�g����Ă��郁�\�b�h�̏��</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function Invoke(ByVal invocation As IMethodInvocation) As Object

	End Interface

End Namespace
