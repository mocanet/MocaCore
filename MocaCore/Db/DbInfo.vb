
Namespace Db

	''' <summary>
	''' データベース情報の共通モデル
	''' </summary>
	''' <remarks></remarks>
	Public Class DbInfo

		''' <summary>カタログ名称</summary>
		Private _catalog As String
		''' <summary>スキーマ名</summary>
		Private _schema As String
		''' <summary>名称</summary>
		Private _name As String
		''' <summary>型</summary>
		Private _typ As String

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
			Me._catalog = catalog
			Me._schema = schema
			Me._name = name
			Me._typ = typ
		End Sub

#End Region

#Region " プロパティ "

		''' <summary>
		''' カタログ名称プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Catalog() As String
			Get
				Return _catalog
			End Get
			Set(ByVal value As String)
				_catalog = value
			End Set
		End Property

		''' <summary>
		''' スキーマ名プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Schema() As String
			Get
				Return _schema
			End Get
			Set(ByVal value As String)
				_schema = value
			End Set
		End Property

		''' <summary>
		''' 名称プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Name() As String
			Get
				Return _name
			End Get
			Set(ByVal value As String)
				_name = value
			End Set
		End Property

		''' <summary>
		''' 型プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Typ() As String
			Get
				Return _typ
			End Get
			Set(ByVal value As String)
				_typ = value
			End Set
		End Property

#End Region

#Region " Overrides "

		''' <summary>
		''' ToString の上書き処理
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overrides Function ToString() As String
			Return Catalog & "." & Schema & "." & Typ & "." & Name
		End Function

#End Region

	End Class

End Namespace
