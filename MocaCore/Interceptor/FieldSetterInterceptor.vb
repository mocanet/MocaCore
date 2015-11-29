
Imports System.Reflection

Imports Moca.Aop

Namespace Interceptor

	''' <summary>
	''' フィールドの Setter メソッドインターセプター
	''' </summary>
	''' <remarks>
	''' AOPにてインスタンスが定義を違うインスタンスになってしまったとき、
	''' フィールドが存在しない状態となり、フィールドへインスタンスを設定するときにエラーとなる。
	''' これを回避するためにこいつを使う。
	''' </remarks>
	Public Class FieldSetterInterceptor
		Implements IMethodInterceptor

		''' <summary>log4net logger</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

		Public Function Invoke(ByVal invocation As Aop.IMethodInvocation) As Object Implements Aop.IMethodInterceptor.Invoke
			Dim rc As Object = Nothing
			Dim methodName As String = invocation.This.GetType.FullName & "." & invocation.Method.ToString

			_mylog.DebugFormat("(Aspect:{0}) Field Setter {1}.{2}", methodName, invocation.Args(0), invocation.Args(1))

			' 実処理実行
			rc = invocation.This.GetType().InvokeMember(invocation.Args(1).ToString, BindingFlags.SetField Or BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public, Nothing, invocation.This, New Object() {invocation.Args(2)})

			Return rc
		End Function

	End Class

End Namespace
