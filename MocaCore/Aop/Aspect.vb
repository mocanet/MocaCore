
Imports Moca.Util

Namespace Aop

	''' <summary>
	''' 横断的な関心事が持つ振る舞い（処理のこと）と、いつ振る舞いを適用するかを関連付けします。
	''' Advice(Interceptor)とPointcutをまとめたものをAspect(アスペクト)といいます。
	''' </summary>
	''' <remarks></remarks>
	Public Class Aspect
		Implements IAspect

		''' <summary>Advice(Interceptor)</summary>
		Private _advice As IMethodInterceptor
		''' <summary>Advice(Interceptor)の型</summary>
		Private _adviceType As Type

		''' <summary>Pointcut</summary>
		Private _pointcut As IPointcut

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="advice">Advice(Interceptor)</param>
		''' <param name="pointcut">Pointcut</param>
		''' <remarks></remarks>
		Public Sub New(ByVal advice As IMethodInterceptor, ByVal pointcut As IPointcut)
			_advice = advice
			_pointcut = pointcut
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="adviceType">Advice(Interceptor)の型</param>
		''' <param name="pointcut">Pointcut</param>
		''' <remarks></remarks>
		Public Sub New(ByVal adviceType As Type, ByVal pointcut As IPointcut)
			If Not ClassUtil.IsInterfaceImpl(adviceType, GetType(IMethodInterceptor)) Then
				Throw New ArgumentException("指定されたAdvice(Interceptor)の型が IMethodInterceptor を実装したものではありません。")
			End If
			_advice = Nothing
			_adviceType = adviceType
			_pointcut = pointcut
		End Sub

#End Region

#Region " プロパティ "

		''' <summary>
		''' Advice(Interceptor) プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Advice() As IMethodInterceptor Implements IAspect.Advice
			Get
				If _advice IsNot Nothing Then
					Return _advice
				End If
				Return DirectCast(ClassUtil.NewInstance(_adviceType), IMethodInterceptor)
			End Get
		End Property

		''' <summary>
		''' Pointcut プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Pointcut() As IPointcut Implements IAspect.Pointcut
			Get
				Return _pointcut
			End Get
		End Property

#End Region

	End Class

End Namespace
