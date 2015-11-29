Imports System.Reflection
Imports Moca.Aop
Imports Moca.Di
Imports Moca.Util

Namespace Attr

	''' <summary>
	''' ������͂̃C���^�t�F�[�X
	''' </summary>
	''' <remarks></remarks>
	Public Interface IAttributeAnalyzer

		''' <summary>
		''' �N���X���
		''' </summary>
		''' <param name="target">�ΏۂƂȂ�I�u�W�F�N�g</param>
		''' <returns>�쐬�����R���|�[�l���g</returns>
		''' <remarks></remarks>
		Function Analyze(ByVal target As Type) As MocaComponent

		''' <summary>
		''' �t�B�[���h���
		''' </summary>
		''' <param name="target">�ΏۂƂȂ�I�u�W�F�N�g</param>
		''' <param name="field">�t�B�[���h</param>
		''' <returns>�쐬�����R���|�[�l���g</returns>
		''' <remarks></remarks>
		Function Analyze(ByVal target As Object, ByVal field As FieldInfo) As MocaComponent

		''' <summary>
		''' �v���p�e�B���
		''' </summary>
		''' <param name="targetType">�ΏۂƂȂ�^�C�v</param>
		''' <param name="prop">�v���p�e�B</param>
		''' <returns>�A�X�y�N�g�z��</returns>
		''' <remarks></remarks>
		Function Analyze(ByVal targetType As Type, ByVal prop As PropertyInfo) As IAspect()

		''' <summary>
		''' ���\�b�h���
		''' </summary>
		''' <param name="targetType">�ΏۂƂȂ�^�C�v</param>
		''' <param name="method">���\�b�h</param>
		''' <returns>�A�X�y�N�g�z��</returns>
		''' <remarks></remarks>
		Function Analyze(ByVal targetType As Type, ByVal method As MethodInfo) As IAspect()

		''' <summary>
		''' �C�x���g���
		''' </summary>
		''' <param name="targetType">�ΏۂƂȂ�^�C�v</param>
		''' <param name="method">�C�x���g</param>
		''' <returns>�A�X�y�N�g�z��</returns>
		''' <remarks></remarks>
		Function Analyze(ByVal targetType As Type, ByVal method As EventInfo) As IAspect()

	End Interface

End Namespace
