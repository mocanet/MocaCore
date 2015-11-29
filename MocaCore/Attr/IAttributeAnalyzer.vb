Imports System.Reflection
Imports Moca.Aop
Imports Moca.Di
Imports Moca.Util

Namespace Attr

	''' <summary>
	''' 属性解析のインタフェース
	''' </summary>
	''' <remarks></remarks>
	Public Interface IAttributeAnalyzer

		''' <summary>
		''' クラス解析
		''' </summary>
		''' <param name="target">対象となるオブジェクト</param>
		''' <returns>作成したコンポーネント</returns>
		''' <remarks></remarks>
		Function Analyze(ByVal target As Type) As MocaComponent

		''' <summary>
		''' フィールド解析
		''' </summary>
		''' <param name="target">対象となるオブジェクト</param>
		''' <param name="field">フィールド</param>
		''' <returns>作成したコンポーネント</returns>
		''' <remarks></remarks>
		Function Analyze(ByVal target As Object, ByVal field As FieldInfo) As MocaComponent

		''' <summary>
		''' プロパティ解析
		''' </summary>
		''' <param name="targetType">対象となるタイプ</param>
		''' <param name="prop">プロパティ</param>
		''' <returns>アスペクト配列</returns>
		''' <remarks></remarks>
		Function Analyze(ByVal targetType As Type, ByVal prop As PropertyInfo) As IAspect()

		''' <summary>
		''' メソッド解析
		''' </summary>
		''' <param name="targetType">対象となるタイプ</param>
		''' <param name="method">メソッド</param>
		''' <returns>アスペクト配列</returns>
		''' <remarks></remarks>
		Function Analyze(ByVal targetType As Type, ByVal method As MethodInfo) As IAspect()

		''' <summary>
		''' イベント解析
		''' </summary>
		''' <param name="targetType">対象となるタイプ</param>
		''' <param name="method">イベント</param>
		''' <returns>アスペクト配列</returns>
		''' <remarks></remarks>
		Function Analyze(ByVal targetType As Type, ByVal method As EventInfo) As IAspect()

	End Interface

End Namespace
