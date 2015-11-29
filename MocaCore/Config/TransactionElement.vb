
Imports System.Configuration

Namespace Config

	''' <summary>
	''' transactionType 要素
	''' </summary>
	''' <remarks></remarks>
	Public Class TransactionElement
		Inherits ConfigurationElement

		Const C_PROP_TYPE As String = "type"

		''' <summary>
		''' type 属性
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<ConfigurationProperty(C_PROP_TYPE, isRequired:=False)> _
		Public Property Type() As String
			Get
				Return DirectCast(Me(C_PROP_TYPE), String).Trim
			End Get
			Set(ByVal value As String)
				Me(C_PROP_TYPE) = value
			End Set
		End Property

	End Class

End Namespace
