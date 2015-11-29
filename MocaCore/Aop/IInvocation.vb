
Namespace Aop

	''' <summary>
	''' 
	''' </summary>
	''' <remarks></remarks>
	Public Interface IInvocation
		Inherits IJoinpoint

		''' <summary>
		''' メソッドの引数 プロパティ
		''' </summary>
		''' <value>オブジェクト配列</value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Args() As Object()

	End Interface

End Namespace
