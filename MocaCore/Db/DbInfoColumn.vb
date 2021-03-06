﻿
Namespace Db

	''' <summary>
	''' 列情報のモデル
	''' </summary>
	''' <remarks></remarks>
	Public Class DbInfoColumn
		Inherits DbInfo

        ''' <summary>最大桁数</summary>
        Private _maxLength As Long

        Private _precision As Integer

		Private _scale As Integer

		Private _uniCode As Boolean

		Private _columnType As Object

		Private _dbColumn As DataColumn

		Private _primaryKey As Boolean

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
        Public Property MaxLength() As Long
            Get
                Return _maxLength
            End Get
            Friend Set(ByVal value As Long)
                _maxLength = value
            End Set
        End Property

        ''' <summary>ユニコード文字列かどうか</summary>
        Public Property UniCode() As Boolean
			Get
				Return _uniCode
			End Get
			Friend Set(ByVal value As Boolean)
				_uniCode = value
			End Set
		End Property

        ''' <summary>最大桁数（半角 1 バイト、全角 2 バイトとして）</summary>
        Public ReadOnly Property MaxLengthB() As Long
            Get
                Return CLng(IIf(_uniCode, _maxLength, _maxLength * 2))
            End Get
        End Property

        ''' <summary>小数点の右側および左側にある保存できる最大文字</summary>
        Public Property Precision() As System.Int32
			Get
				Return Me._precision
			End Get
			Friend Set(ByVal value As System.Int32)
				Me._precision = value
			End Set
		End Property

		''' <summary>小数点の右側にある保存できる最大文字</summary>
		Public Property Scale() As System.Int32
			Get
				Return Me._scale
			End Get
			Friend Set(ByVal value As System.Int32)
				Me._scale = value
			End Set
		End Property

		''' <summary>列の型オブジェクト</summary>
		Public Property ColumnType() As System.Object
			Get
				Return Me._columnType
			End Get
			Friend Set(ByVal value As System.Object)
				Me._columnType = value
			End Set
		End Property

		''' <summary>プライマリキーかどうか</summary>
		Public Property PrimaryKey As Boolean
			Get
				Return _primaryKey
			End Get
			Friend Set(value As Boolean)
				_primaryKey = value
			End Set
		End Property

		''' <summary>DataColumnオブジェクト</summary>
		Friend Property DbColumn As DataColumn
			Get
				Return _dbColumn
			End Get
			Set(value As DataColumn)
				_dbColumn = value
			End Set
		End Property

#End Region

	End Class

End Namespace
