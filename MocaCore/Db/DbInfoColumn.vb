
Namespace Db

	''' <summary>
	''' 列情報のモデル
	''' </summary>
	''' <remarks></remarks>
	Public Class DbInfoColumn
		Inherits DbInfo

		''' <summary>最大桁数</summary>
		Private _maxLength As Integer

		Private _precision As Integer

		Private _scale As Integer

		Private _uniCode As Boolean

		Private _columnType As Object

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

		''' <summary>最大桁数</summary>
		Public Property MaxLength() As Integer
			Get
				Return _maxLength
			End Get
			Set(ByVal value As Integer)
				_maxLength = value
			End Set
		End Property

		''' <summary>ユニコード文字列かどうか</summary>
		Public Property UniCode() As Boolean
			Get
				Return _uniCode
			End Get
			Set(ByVal value As Boolean)
				_uniCode = value
			End Set
		End Property

		''' <summary>最大桁数（半角 1 バイト、全角 2 バイトとして）</summary>
		Public ReadOnly Property MaxLengthB() As Integer
			Get
				Return CInt(IIf(_uniCode, _maxLength, _maxLength * 2))
			End Get
		End Property

		''' <summary>小数点の右側および左側にある保存できる最大文字</summary>
		Public Property Precision() As System.Int32
			Get
				Return Me._precision
			End Get
			Set(ByVal value As System.Int32)
				Me._precision = value
			End Set
		End Property

		''' <summary>小数点の右側にある保存できる最大文字</summary>
		Public Property Scale() As System.Int32
			Get
				Return Me._scale
			End Get
			Set(ByVal value As System.Int32)
				Me._scale = value
			End Set
		End Property

		''' <summary>列の型オブジェクト</summary>
		Public Property ColumnType() As System.Object
			Get
				Return Me._columnType
			End Get
			Set(ByVal value As System.Object)
				Me._columnType = value
			End Set
		End Property

#End Region

	End Class

End Namespace
