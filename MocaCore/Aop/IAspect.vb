
Namespace Aop

	''' <summary>
	''' ���f�I�Ȋ֐S�������U�镑���i�����̂��Ɓj�ƁA���U�镑����K�p���邩���֘A�t�����܂��B
	''' Advice(Interceptor)��Pointcut���܂Ƃ߂����̂�Aspect(�A�X�y�N�g)�Ƃ����܂��B
	''' </summary>
	''' <remarks></remarks>
	Public Interface IAspect

		''' <summary>
		''' Advice(Interceptor) �v���p�e�B
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Advice() As IMethodInterceptor

		''' <summary>
		''' Pointcut �v���p�e�B
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Pointcut() As IPointcut

	End Interface

End Namespace
