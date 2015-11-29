
Imports System.Runtime.Remoting.Proxies
Imports System.Runtime.Remoting.Messaging
Imports System.Reflection
Imports Moca.Exceptions

Namespace Aop

	''' <summary>
	''' 透過的プロクシ
	''' </summary>
	''' <remarks></remarks>
	Public Class AopProxy
		Inherits RealProxy

		''' <summary>透過的プロクシを作成する型</summary>
		Private _type As Type

		''' <summary>適用する Aspect 配列</summary>
		Private _aspects As IList(Of IAspect)

		''' <summary>透過的プロクシを作成するインスタンス</summary>
		Private _target As Object

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="target">透過的プロクシを作成するインスタンス</param>
		''' <remarks></remarks>
		Public Sub New(ByVal target As Object)
			MyBase.New(target.GetType)
			_aspects = New List(Of IAspect)
			_type = target.GetType
			_target = target
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="type">透過的プロクシを作成する型</param>
		''' <remarks></remarks>
		Public Sub New(ByVal type As Type)
			MyBase.New(type)
			_aspects = New List(Of IAspect)
			_type = type
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="type">透過的プロクシを作成する型</param>
		''' <param name="aspects">適用する Aspect 配列</param>
		''' <remarks></remarks>
		Public Sub New(ByVal type As Type, ByVal aspects() As IAspect)
			Me.New(type, aspects, Nothing)
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="type">透過的プロクシを作成する型</param>
		''' <param name="aspects">適用する Aspect 配列</param>
		''' <param name="target">透過的プロクシを作成するインスタンス</param>
		''' <remarks></remarks>
		Public Sub New(ByVal type As Type, ByVal aspects() As IAspect, ByVal target As Object)
			Me.New(type)
			_aspects = aspects
			_target = target
		End Sub

#End Region

#Region " RealProxy Overrides "

		''' <summary>
		''' IMessage で指定されたメソッドを、現在のインスタンスが表すリモート オブジェクトで呼び出します。
		''' </summary>
		''' <param name="msg">メソッドの呼び出しに関する情報</param>
		''' <returns>呼び出されたメソッドが返すメッセージで、out パラメータまたは ref パラメータのどちらかと戻り値を格納しているメッセージ。</returns>
		''' <remarks></remarks>
		Public Overrides Function Invoke(ByVal msg As IMessage) As IMessage
			Dim mm As IMethodMessage
			Dim method As MethodInfo
			Dim args() As Object
			Dim ret As Object

			' 初期化
			ret = Nothing
			mm = DirectCast(msg, IMethodMessage)
			method = DirectCast(mm.MethodBase, MethodInfo)
			args = mm.Args

			' インタフェースの以外で実態が無い時はインスタンス化する
			If Not _type.IsInterface Then
				If _target Is Nothing Then
					_target = Activator.CreateInstance(_type)
				End If
			End If
			' 透過的プロクシを作成するインスタンスが存在しない時は Object 型で仮インスタンスを作成
			If _target Is Nothing Then
				_target = New Object()
			End If

			' メソッド実行！
			If _aspects.Count = 0 Then
				' 振る舞いを適用しないとき
				Try
					ret = method.Invoke(_target, args)
				Catch ex As TargetInvocationException
					CommonException.SaveStackTraceToRemoteStackTraceString(ex.InnerException)
					Throw ex.InnerException
				End Try
			Else
				' 振る舞いを適用するとき
				Dim topInvocation As IMethodInvocation
				Dim invocation As IMethodInvocation

				topInvocation = New MethodInvocation(_target, method, args)
				invocation = topInvocation

				For Each aspect As IAspect In _aspects
					If Not aspect.Pointcut.IsExecution(method.ToString) Then
						Continue For
					End If
					invocation = New MethodInvocation(aspect.Advice, DirectCast(invocation, MethodInvocation))
				Next
				ret = topInvocation.Proceed()
			End If

			' 戻り値作成
			Dim mrm As IMethodReturnMessage
			mrm = New ReturnMessage(ret, args, args.Length, mm.LogicalCallContext, DirectCast(msg, IMethodCallMessage))
			Return mrm
		End Function

#End Region

		''' <summary>
		''' 適用する Aspect を追加する
		''' </summary>
		''' <param name="aspect">Aspect インスタンス</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function AddAspect(ByVal aspect As IAspect) As IAspect
			_aspects.Add(aspect)
			Return aspect
		End Function

		''' <summary>
		''' 適用する Aspect を追加する
		''' </summary>
		''' <param name="advice">Advice(Interceptor)</param>
		''' <param name="pointcut">Pointcut</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function AddAspect(ByVal advice As IMethodInterceptor, ByVal pointcut As IPointcut) As IAspect
			Dim item As IAspect

			item = New Aspect(advice, pointcut)
			_aspects.Add(item)

			Return item
		End Function

		''' <summary>
		''' 透過的プロクシを返す
		''' </summary>
		''' <returns>透過的プロクシのインスタンス</returns>
		''' <remarks></remarks>
		Public Function Create() As Object
			Return GetTransparentProxy()
		End Function

		''' <summary>
		''' 透過的プロクシを返す
		''' </summary>
		''' <typeparam name="T">透過的プロクシのインスタンスの型</typeparam>
		''' <returns>透過的プロクシのインスタンス</returns>
		''' <remarks></remarks>
		Public Function Create(Of T)() As T
			Return DirectCast(GetTransparentProxy(), T)
		End Function

	End Class

End Namespace
