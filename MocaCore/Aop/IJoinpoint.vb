
Imports System.Reflection

Namespace Aop

	''' <summary>
	''' プロパティが利用された時やメソッドが呼び出された時など、Advice（振る舞い）を割り込ませることが可能なときのことです。
	''' なおAOPの実装によっては、「プロパティの利用はJoinpointにならない」「メソッドが呼び出されたときだけがJoinpointになる」などのように異なります。
	''' </summary>
	''' <remarks></remarks>
	Public Interface IJoinpoint

		''' <summary>
		''' メソッド定義
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Method() As MethodBase

		''' <summary>
		''' 実行対象のインスタンス
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property This() As Object

		''' <summary>
		''' 実行
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function Proceed() As Object

	End Interface

End Namespace
