
Imports System.Reflection

Namespace Aop

	''' <summary>
	''' �v���p�e�B�����p���ꂽ���⃁�\�b�h���Ăяo���ꂽ���ȂǁAAdvice�i�U�镑���j�����荞�܂��邱�Ƃ��\�ȂƂ��̂��Ƃł��B
	''' �Ȃ�AOP�̎����ɂ���ẮA�u�v���p�e�B�̗��p��Joinpoint�ɂȂ�Ȃ��v�u���\�b�h���Ăяo���ꂽ�Ƃ�������Joinpoint�ɂȂ�v�Ȃǂ̂悤�ɈقȂ�܂��B
	''' </summary>
	''' <remarks></remarks>
	Public Interface IJoinpoint

		''' <summary>
		''' ���\�b�h��`
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Method() As MethodBase

		''' <summary>
		''' ���s�Ώۂ̃C���X�^���X
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property This() As Object

		''' <summary>
		''' ���s
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function Proceed() As Object

	End Interface

End Namespace
