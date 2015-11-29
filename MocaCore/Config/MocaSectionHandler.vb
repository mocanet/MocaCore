
Imports System.Configuration

Namespace Config

	''' <summary>
	''' moca セクション解析
	''' </summary>
	''' <remarks></remarks>
	Public Class MocaSectionHandler
		Inherits ConfigurationSection

		Const C_TAG_TRANSACTION As String = "transaction"

		''' <summary>
		''' transactionType 要素
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<ConfigurationProperty(C_TAG_TRANSACTION, isRequired:=False)> _
		Public Property Transaction() As TransactionElement
			Get
				Return DirectCast(Me(C_TAG_TRANSACTION), TransactionElement)
			End Get
			Set(ByVal value As TransactionElement)
				Me(C_TAG_TRANSACTION) = value
			End Set
		End Property

	End Class

End Namespace
