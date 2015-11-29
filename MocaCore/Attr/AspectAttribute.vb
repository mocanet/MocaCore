
Imports System.Reflection
Imports Moca.Aop
Imports Moca.Util

Namespace Attr

	''' <summary>
	''' アスペクト属性
	''' </summary>
	''' <remarks>
	''' アスペクトしたいときに指定する。
	''' メソッドのみ指定可能です。
	''' </remarks>
	<AttributeUsage(AttributeTargets.Method, allowmultiple:=True)> _
	Public Class AspectAttribute
		Inherits Attribute

		''' <summary>指定された実体化するクラスタイプ</summary>
		Private _type As Type

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="typ">クラスタイプ</param>
		''' <remarks></remarks>
		Public Sub New(ByVal typ As Type)
			_type = typ
		End Sub

#End Region
#Region " プロパティ "

		''' <summary>
		''' クラスタイププロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ImplType() As Type
			Get
				Return _type
			End Get
		End Property

#End Region

		''' <summary>
		''' アスペクトを作成する
		''' </summary>
		''' <param name="method">メソッド</param>
		''' <returns>アスペクト</returns>
		''' <remarks></remarks>
		Public Function CreateAspect(ByVal method As MethodBase) As IAspect
			Dim pointcut As IPointcut
			Dim val As IAspect

			pointcut = New Pointcut(New String() {method.ToString})
			val = New Aspect(DirectCast(ClassUtil.NewInstance(_type), IMethodInterceptor), pointcut)
			Return val
		End Function

		''' <summary>
		''' アスペクトを作成する
		''' </summary>
		''' <param name="method">メソッド</param>
		''' <returns>アスペクト</returns>
		''' <remarks></remarks>
		Public Function CreateAspect(ByVal method As EventInfo) As IAspect
			Dim pointcut As IPointcut
			Dim val As IAspect

			pointcut = New Pointcut(New String() {method.ToString})
			val = New Aspect(DirectCast(ClassUtil.NewInstance(_type), IMethodInterceptor), pointcut)
			Return val
		End Function

	End Class

End Namespace
