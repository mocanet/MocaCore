
Partial Public Class ConstantDataSet

    ''' <summary>ブランク行の有無</summary>
    Private _blankRow As Boolean
    ''' <summary>ブランク行の値</summary>
    Private _blankValue As Object
    ''' <summary>値と内容の区切り文字</summary>
    Private _delm As String

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="tableName">定数値テーブル名称</param>
    ''' <param name="blankRow">ブランク行を追加するかどうか（デフォルト：作成）</param>
    ''' <param name="blankValue">ブランク行の値（デフォルト：-1）</param>
    ''' <param name="delm">値と内容の区切り文字</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal tableName As String, Optional ByVal blankRow As Boolean = True, Optional ByVal blankValue As Object = -1, Optional ByVal delm As String = ":")
        Me.New()

        _blankRow = blankRow
        _delm = delm

        Constant.TableName = tableName
        _blankValue = blankValue

        ' ブランク行
        If _blankRow Then
            Constant.Rows.Add(String.Empty, _blankValue)
        End If
    End Sub

    ''' <summary>
    ''' ブランク行の値プロパティ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BlankValue() As Object
        Get
            Return _blankValue
        End Get
        Set(ByVal value As Object)
            _blankValue = value
            If Not _blankRow Then
                Exit Property
            End If
            Constant.Rows(0).Item(1) = _blankValue
        End Set
    End Property

    ''' <summary>
    ''' 値と内容の区切り文字
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Delm() As System.String
        Get
            Return Me._delm
        End Get
        Set(ByVal value As System.String)
            Me._delm = value
        End Set
    End Property

    Partial Class ConstantDataTable

        Private ReadOnly Property Delim() As String
            Get
                Return DirectCast(Me.DataSet, ConstantDataSet).Delm
            End Get
        End Property

        ''' <summary>
        ''' 定数値データのDataTableへ行を追加します。
        ''' </summary>
        ''' <param name="text">名称</param>
        ''' <param name="value">値</param>
        ''' <remarks></remarks>
        Public Sub AddRow(ByVal text As String, ByVal value As Integer)
            Rows.Add(text, value, CStr(value), value & Delim & text)
        End Sub

        ''' <summary>
        ''' 定数値データのDataTableへ行を追加します。
        ''' </summary>
        ''' <param name="text">名称</param>
        ''' <param name="value">値</param>
        ''' <remarks></remarks>
        Public Sub AddRow(ByVal text As String, ByVal value As String)
            Rows.Add(text, value, value, value & Delim & text)
        End Sub

        ''' <summary>
        ''' 定数値データのDataTableへ行を追加します。
        ''' </summary>
        ''' <param name="text">名称</param>
        ''' <param name="value">値</param>
        ''' <remarks></remarks>
        Public Sub AddRow(ByVal text As String, ByVal value As Double)
            Rows.Add(text, value, value, value & Delim & text)
        End Sub

        ''' <summary>
        ''' <see cref="Rows"/> メソッドの戻り値を <see cref="ConstantRow"/> にしたメソッド。
        ''' </summary>
        ''' <param name="index">行位置</param>
        ''' <returns>ConstantRow</returns>
        ''' <remarks></remarks>
        Public Function RowsConstant(ByVal index As Integer) As ConstantRow
            Return DirectCast(Rows(index), ConstantRow)
        End Function

    End Class

End Class
