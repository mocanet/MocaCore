
Imports System.Reflection
Imports Moca.Exceptions

Namespace Aop

	''' <summary>
	''' Interceptorからインターセプトされているメソッドの情報
	''' </summary>
	''' <remarks></remarks>
	Public Class MethodInvocation
		Implements IMethodInvocation

		''' <summary>実行対象のインスタンス</summary>
		Private _this As Object

		''' <summary>メソッド定義</summary>
		Private _method As MethodBase

		''' <summary>メソッドの引数</summary>
		Private _args() As Object

		''' <summary>実行するAdvice(Interceptor)</summary>
		Private _advice As IMethodInterceptor

		''' <summary>次に実行するInterceptorからインターセプトされているメソッドの情報</summary>
		Private _nextInvocation As MethodInvocation

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="this">実行対象となるインスタンス</param>
		''' <param name="method">実行対象となるメソッド定義</param>
		''' <param name="args">実行するメソッドの引数配列</param>
		''' <remarks></remarks>
		Public Sub New(ByVal this As Object, ByVal method As MethodBase, ByVal args() As Object)
			_this = this
			_method = method
			_args = args
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="interceptor">実行するAdvice</param>
		''' <param name="forwardInvocation">一つ前のInterceptorからインターセプトされているメソッドの情報</param>
		''' <remarks></remarks>
		Public Sub New(ByVal interceptor As IMethodInterceptor, ByVal forwardInvocation As MethodInvocation)
			_this = forwardInvocation.This
			_method = forwardInvocation.Method
			_args = forwardInvocation.Arguments
			_advice = interceptor
			forwardInvocation.NextInvocation = Me
		End Sub

#End Region

#Region " Implements IMethodInvocation "

#Region " プロパティ "

		Public ReadOnly Property Arguments() As Object() Implements IMethodInvocation.Args
			Get
				Return _args
			End Get
		End Property

		Public ReadOnly Property Method() As MethodBase Implements IMethodInvocation.Method
			Get
				Return _method
			End Get
		End Property

		Public ReadOnly Property This() As Object Implements IMethodInvocation.This
			Get
				Return _this
			End Get
		End Property

#End Region

		Public Function Proceed() As Object Implements IMethodInvocation.Proceed
			' 次の Interceptor を実行する
			If _nextInvocation IsNot Nothing Then
				Return _nextInvocation.Advice.Invoke(_nextInvocation)
			End If

			'TODO: これでいいのかなぁ・・・
			'' ターゲットが Object 型は原型なしの仮想オブジェクトなので以下は処理なし
			'If TypeOf _this Is Object Then
			'	Return Nothing
			'End If
			Try
				Return _method.Invoke(_this, _args)
			Catch ex As TargetInvocationException
				CommonException.SaveStackTraceToRemoteStackTraceString(ex.InnerException)
				Throw ex.InnerException
			End Try
		End Function

#End Region

#Region " プロパティ "

		''' <summary>
		''' 次に実行するInterceptorからインターセプトされているメソッドの情報 プロパティ
		''' </summary>
		''' <value></value>
		''' <remarks></remarks>
		Public WriteOnly Property NextInvocation() As MethodInvocation
			Set(ByVal value As MethodInvocation)
				_nextInvocation = value
			End Set
		End Property

		''' <summary>
		''' 実行するAdvice(Interceptor) プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Friend ReadOnly Property Advice() As IMethodInterceptor
			Get
				Return _advice
			End Get
		End Property

#End Region

	End Class

End Namespace
