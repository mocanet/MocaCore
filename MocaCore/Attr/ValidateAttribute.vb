
Imports System.Reflection
Imports Moca.Util

Namespace Attr

	''' <summary>
	''' 検証属性
	''' </summary>
	''' <remarks></remarks>
	<AttributeUsage(AttributeTargets.Property)> _
	Public Class ValidateAttribute
		Inherits Attribute

		''' <summary>チェック項目</summary>
		Private _validateType As ValidateTypes

		''' <summary>最小値</summary>
		Private _min As Object

		''' <summary>最大値</summary>
		Private _max As Object

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="validateType">チェック項目</param>
		''' <param name="min">最小値</param>
		''' <param name="max">最大値</param>
		''' <remarks></remarks>
		Public Sub New(ByVal validateType As ValidateTypes, Optional ByVal min As Object = Nothing, Optional ByVal max As Object = Nothing)
			_validateType = validateType
			_min = min
			_max = max
		End Sub

#End Region
#Region " プロパティ "

		''' <summary>
		''' チェック項目プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ValidateType As ValidateTypes
			Get
				Return _validateType
			End Get
		End Property

		''' <summary>
		''' 最小値プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Min As Object
			Get
				Return _min
			End Get
		End Property

		''' <summary>
		''' 最大値プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Max As Object
			Get
				Return _max
			End Get
		End Property

#End Region

	End Class

End Namespace
