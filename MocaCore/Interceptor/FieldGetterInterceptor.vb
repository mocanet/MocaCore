
Imports System.Reflection

Imports Moca.Aop

Namespace Interceptor

	''' <summary>
	''' フィールドの Getter メソッドインターセプター
	''' </summary>
	''' <remarks>
	''' AOPにてインスタンスが定義を違うインスタンスになってしまったとき、
	''' フィールドが存在しない状態となり、フィールドへインスタンスを取得するときにエラーとなる。
	''' これを回避するためにこいつを使う。
	''' </remarks>
	Public Class FieldGetterInterceptor
		Implements IMethodInterceptor

		''' <summary>log4net logger</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

		Public Function Invoke(ByVal invocation As Aop.IMethodInvocation) As Object Implements Aop.IMethodInterceptor.Invoke
			Dim rc As Object = Nothing
			Dim methodName As String = invocation.This.GetType.FullName & "." & invocation.Method.ToString

			_mylog.DebugFormat("(Aspect:{0}) Field Getter {1}.{2}", methodName, invocation.Args(0), invocation.Args(1))

			' 実処理実行
			rc = invocation.This.GetType().InvokeMember(invocation.Args(1).ToString, BindingFlags.GetField Or BindingFlags.Instance Or BindingFlags.NonPublic Or BindingFlags.Public, Nothing, invocation.This, New Object() {})

			' FieldGetter は引数の３番目に値が返るのでここで設定しておく
			invocation.Args(2) = rc

			Return rc
		End Function

	End Class

End Namespace
