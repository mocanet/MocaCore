
Namespace Di

	''' <summary>
	''' �R���|�[�l���g�����̃R���e�i�C���^�t�F�[�X
	''' </summary>
	''' <remarks></remarks>
	Public Interface IContainer

		''' <summary>
		''' ����������
		''' </summary>
		''' <remarks></remarks>
		Sub Init()

		''' <summary>
		''' �R���|�[�l���g�̏���
		''' </summary>
		''' <remarks></remarks>
		Sub Destroy()

		''' <summary>
		''' �i�[���Ă���R���|�[�l���g��Ԃ��B
		''' </summary>
		''' <param name="componentType">�擾����^</param>
		''' <returns>�Y������R���|�[�l���g�B�Y�����Ȃ��Ƃ��� Nothing ��Ԃ��B</returns>
		''' <remarks></remarks>
		Function GetComponent(ByVal componentType As Type) As MocaComponent

		''' <summary>
		''' �i�[���Ă���R���|�[�l���g��Ԃ��B
		''' </summary>
		''' <param name="componentKey">�擾����L�[</param>
		''' <returns>�Y������R���|�[�l���g�B�Y�����Ȃ��Ƃ��� Nothing ��Ԃ��B</returns>
		''' <remarks></remarks>
		Function GetComponent(ByVal componentKey As String) As MocaComponent

		''' <summary>
		''' �R���|�[�l���g���i�[����B
		''' </summary>
		''' <param name="component">�Ώۂ̃R���|�[�l���g</param>
		''' <remarks></remarks>
		Sub SetComponent(ByVal component As MocaComponent)

		''' <summary>
		''' <see cref="MocaComponent"/> �𔽕���������񋓎q��Ԃ��܂��B
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetEnumerator() As IEnumerator(Of MocaComponent)

	End Interface

End Namespace
