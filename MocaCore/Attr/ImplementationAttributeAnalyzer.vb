
Imports Moca.Util

Namespace Attr

	''' <summary>
	''' é¿ë‘ÇéwíËÇ∑ÇÈëÆê´ÇÃâêÕ
	''' </summary>
	''' <remarks></remarks>
	Public Class ImplementationAttributeAnalyzer
		Implements IAttributeAnalyzer

		''' <summary>log4net logger</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

		Public Function Analyze(ByVal target As System.Type) As Di.MocaComponent Implements IAttributeAnalyzer.Analyze
			Return Nothing
		End Function

		Public Function Analyze(ByVal target As Object, ByVal field As System.Reflection.FieldInfo) As Di.MocaComponent Implements IAttributeAnalyzer.Analyze
			Dim attr As ImplementationAttribute

			attr = ClassUtil.GetCustomAttribute(Of ImplementationAttribute)(field)
			If attr Is Nothing Then
				attr = ClassUtil.GetCustomAttribute(Of ImplementationAttribute)(field.FieldType)
				If attr Is Nothing Then
					Return Nothing
				End If
			End If

			_mylog.DebugFormat("Implementation Attribute Analyzer : {0} {1}", target.ToString, field.Name)

			Return attr.CreateComponent(field)
		End Function

		Public Function Analyze(ByVal targetType As System.Type, ByVal method As System.Reflection.MethodInfo) As Aop.IAspect() Implements IAttributeAnalyzer.Analyze
			Return Nothing
		End Function

		Public Function Analyze(ByVal targetType As System.Type, ByVal prop As System.Reflection.PropertyInfo) As Aop.IAspect() Implements IAttributeAnalyzer.Analyze
			Return Nothing
		End Function

		Public Function Analyze(ByVal targetType As System.Type, ByVal method As System.Reflection.EventInfo) As Aop.IAspect() Implements IAttributeAnalyzer.Analyze
			Return Nothing
		End Function

	End Class

End Namespace
