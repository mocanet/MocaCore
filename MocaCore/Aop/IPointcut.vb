
Namespace Aop

	''' <summary>
	''' Joinpoint�̂����AAdvice��K�p������Joinpoint�𐳋K�\���Ȃǂ�p�����������g�p���či�荞�ނ��߂̃t�B���^�ł��B
	''' �Ⴆ�΁AAdvice��K�p�������̂́uadd�v�ł͂��܂郁�\�b�h�����s���ꂽ���������Ƃ���ƁA
	''' �������uadd*�v�Ƃ��či�荞�܂ꂽaddXxx���\�b�h�����s���ꂽ��������Advice�����s�����悤�ɂ��ł��܂����A
	''' �w�肳�ꂽ���\�b�h������v����Ƃ�����Advice�����s�����悤�ɂ���Ȃǂ̃t�B���^���쐬���܂��B
	''' </summary>
	''' <remarks></remarks>
	Public Interface IPointcut

		''' <summary>
		''' �����œn���ꂽ���\�b�h����Advice��}�����邩�m�F���܂��B
		''' </summary>
		''' <param name="pattern">���\�b�h��</param>
		''' <returns>True�Ȃ�Advice��}������AFalse�Ȃ�Advice�͑}������Ȃ�</returns>
		''' <remarks></remarks>
		Function IsExecution(ByVal pattern As String) As Boolean

	End Interface

End Namespace
