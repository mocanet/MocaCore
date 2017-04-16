
Imports System.ComponentModel

''' <summary>
''' ソート用のバインディングリスト
''' </summary>
''' <typeparam name="T"></typeparam>
Public Class SortableBindingList(Of T)
    Inherits BindingList(Of T)

    Private _isSortedValue As Boolean
    Private _sortDirectionValue As ListSortDirection
    Private _sortPropertyValue As PropertyDescriptor

#Region " コンストラクタ "

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="list"></param>
    Public Sub New(list As IList(Of T))
        MyBase.New(list)
        'For Each o As Object In list
        '    Me.Add(DirectCast(o, T))
        'Next
    End Sub

#End Region

#Region " Overrides "

    ''' <summary>
    ''' ソート処理
    ''' </summary>
    ''' <param name="prop"></param>
    ''' <param name="direction"></param>
    Protected Overrides Sub ApplySortCore(prop As PropertyDescriptor, direction As ListSortDirection)
        ' 並べ替えるリストを取得します。
        Dim items As List(Of T) = TryCast(Me.Items, List(Of T))

        ' 並べ替えるアイテムがある場合は、sort を適用して設定します。
        If Not (items Is Nothing) Then
            Dim pc As New PropertyComparer(Of T)(prop, direction)
            items.Sort(pc)

            _isSortedValue = True
        Else
            _isSortedValue = False
        End If

        ' バインドされたコントロールに表示を更新するように指示します。
        Me.OnListChanged(New ListChangedEventArgs(ListChangedType.Reset, -1))
    End Sub

    ''' <summary>
    ''' ソート対象
    ''' </summary>
    ''' <returns></returns>
    Protected Overrides ReadOnly Property SortPropertyCore() As PropertyDescriptor
        Get
            Return _sortPropertyValue
        End Get
    End Property

    ''' <summary>
    ''' ソート方向
    ''' </summary>
    ''' <returns></returns>
    Protected Overrides ReadOnly Property SortDirectionCore() As ListSortDirection
        Get
            Return _sortDirectionValue
        End Get
    End Property

    ''' <summary>
    ''' ソートのサポート
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Protected Overrides ReadOnly Property SupportsSortingCore() As Boolean
        Get
            Return True
        End Get
    End Property

    ''' <summary>
    ''' ソート済かどうか
    ''' </summary>
    ''' <returns></returns>
    Protected Overrides ReadOnly Property IsSortedCore() As Boolean
        Get
            Return _isSortedValue
        End Get
    End Property

#End Region

End Class


'汎用コンペアラークラス
Public Class PropertyComparer(Of T)
    Implements IComparer(Of T)

    Public Sub New(prop As PropertyDescriptor, direction As ListSortDirection)
        Me.prop = prop
        sortDirection = If((direction = ListSortDirection.Ascending), 1, -1)
    End Sub

    Private prop As PropertyDescriptor
    Private sortDirection As Integer


    Private Function IComparer_Compare(x As T, y As T) As Integer Implements IComparer(Of T).Compare
        Dim left As IComparable = TryCast(prop.GetValue(x), IComparable)
        Dim right As IComparable = TryCast(prop.GetValue(y), IComparable)

        Dim result As Integer

        If Not (left Is Nothing) Then
            result = left.CompareTo(right)
        ElseIf right Is Nothing Then
            result = 0
        Else
            result = -1
        End If

        Return result * sortDirection
    End Function

End Class
