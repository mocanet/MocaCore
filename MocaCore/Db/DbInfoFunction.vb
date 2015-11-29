
Namespace Db

	''' <summary>
	''' 関数情報のモデル
	''' </summary>
	''' <remarks></remarks>
	Public Class DbInfoFunction
		Inherits DbInfo

		''' <summary>ソース</summary>
		Private _src As String

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="catalog">カタログ名</param>
		''' <param name="schema">スキーマ名</param>
		''' <param name="name">名称</param>
		''' <param name="typ">型</param>
		''' <remarks></remarks>
		Public Sub New(ByVal catalog As String, ByVal schema As String, ByVal name As String, ByVal typ As String)
			MyBase.New(catalog, schema, name, typ)
		End Sub

#End Region

#Region " プロパティ "

		''' <summary>
		''' ソースプロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Src() As String
			Get
				Return _src
			End Get
			Set(ByVal value As String)
				_src = value
			End Set
		End Property

#End Region

	End Class

End Namespace
