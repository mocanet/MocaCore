
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
    End Sub

#End Region

#Region " Overrides "

    ''' <summary>
    ''' ソート処理
    ''' </summary>
    ''' <param name="prop"></param>
    ''' <param name="direction"></param>
    Protected Overrides Sub ApplySortCore(prop As PropertyDescriptor, direction As ListSortDirection)
        Dim interfaceType As Type = prop.PropertyType.GetInterface("IComparable")

        If interfaceType Is Nothing AndAlso prop.PropertyType.IsValueType Then
            Dim underlyingType As Type = Nullable.GetUnderlyingType(prop.PropertyType)

            If Not (underlyingType Is Nothing) Then
                interfaceType = underlyingType.GetInterface("IComparable")
            End If
        End If

        If Not (interfaceType Is Nothing) Then
            _sortPropertyValue = prop
            _sortDirectionValue = direction

            Dim query As IEnumerable(Of T) = MyBase.Items

            If direction = ListSortDirection.Ascending Then
                query = query.OrderBy(Function(i) prop.GetValue(i))
            Else
                query = query.OrderByDescending(Function(i) prop.GetValue(i))
            End If

            Dim newIndex As Integer = 0
            For Each item As Object In query
                Me.Items(newIndex) = DirectCast(item, T)
                System.Math.Max(System.Threading.Interlocked.Increment(newIndex), newIndex - 1)
            Next

            _isSortedValue = True
            Me.OnListChanged(New ListChangedEventArgs(ListChangedType.Reset, -1))
        Else
            Throw New NotSupportedException("Cannot sort by " + prop.Name + ". This" + prop.PropertyType.ToString() + " does not implement IComparable")
        End If
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
