
Namespace Db

	''' <summary>
	''' テーブル情報のモデル
	''' </summary>
	''' <remarks></remarks>
	Public Class DbInfoTable
		Inherits DbInfo

		''' <summary>列情報</summary>
		Private _columns As DbInfoColumnCollection

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
		''' 列情報
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Columns() As DbInfoColumnCollection
			Get
				Return _columns
			End Get
			Set(ByVal value As DbInfoColumnCollection)
				_columns = value
			End Set
		End Property

		''' <summary>
		''' テーブル名のみ返す
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ToTableName() As String
			Get
				Return Me.Name
			End Get
		End Property

#End Region

	End Class

End Namespace
