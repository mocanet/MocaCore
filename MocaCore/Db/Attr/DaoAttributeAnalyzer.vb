
Imports Moca.Attr
Imports Moca.Util

Namespace Db.Attr

	''' <summary>
	''' DAO属性解析
	''' </summary>
	''' <remarks></remarks>
	Public Class DaoAttributeAnalyzer
		Implements IAttributeAnalyzer

		''' <summary>log4net logger</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

		Public Function Analyze(ByVal target As System.Type) As Di.MocaComponent Implements IAttributeAnalyzer.Analyze
			Return Nothing
		End Function

		Public Function Analyze(ByVal target As Object, ByVal field As System.Reflection.FieldInfo) As Di.MocaComponent Implements IAttributeAnalyzer.Analyze
			Dim attr As DaoAttribute

			attr = ClassUtil.GetCustomAttribute(Of DaoAttribute)(field.FieldType)
			If attr Is Nothing Then
				Return Nothing
			End If

			_mylog.DebugFormat("Dao Attribute Analyzer : {0} {1}", target.ToString, field.Name)

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
