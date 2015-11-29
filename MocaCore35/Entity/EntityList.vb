
''' <summary>
''' エンティティのコレクション
''' </summary>
''' <typeparam name="T"></typeparam>
''' <remarks></remarks>
Public Class EntityList(Of T)
	Inherits List(Of T)
	Implements IEntityList(Of T)

#Region " コンストラクタ "

	''' <summary>
	''' デフォルトコンストラクタ
	''' </summary>
	''' <remarks></remarks>
	Public Sub New()
		MyBase.New()
	End Sub

	''' <summary>
	''' コンストラクタ
	''' </summary>
	''' <param name="capacity">新しいリストに格納できる要素の数</param>
	''' <remarks></remarks>
	Public Sub New(ByVal capacity As Integer)
		MyBase.New(capacity)
	End Sub

	''' <summary>
	''' コンストラクタ
	''' </summary>
	''' <param name="values">新しいリストにコピーする要素元</param>
	''' <remarks></remarks>
	Public Sub New(ByVal values As IList(Of T))
		MyBase.New(values)
	End Sub

#End Region

#Region " Implements "

	''' <summary>
	''' 選択されている行を取得する
	''' </summary>
	''' <param name="func">選択されているかどうかの判定</param>
	''' <returns>IEnumerable(Of SelectedRow(Of TSource))</returns>
	''' <remarks></remarks>
	Public Function GetSelected(func As Func(Of SelectedEntity(Of T), Boolean)) As IEnumerable(Of SelectedEntity(Of T)) Implements IEntityList(Of T).GetSelected
		'Return Me.Select(Function(row, ii) New SelectedEntity(Of T) With {.Entity = row, .Index = ii}).Where(func)
		Return Me.Select(Function(row, ii) New SelectedEntity(Of T)(ii, row)).Where(func)
	End Function

#End Region

End Class

Public Class EntityList(Of T, TTag)
	Inherits EntityList(Of T)
	Implements IEntityList(Of T, TTag)

#Region " コンストラクタ "

	''' <summary>
	''' デフォルトコンストラクタ
	''' </summary>
	''' <remarks></remarks>
	Public Sub New()
		MyBase.New()
	End Sub

	''' <summary>
	''' コンストラクタ
	''' </summary>
	''' <param name="capacity">新しいリストに格納できる要素の数</param>
	''' <remarks></remarks>
	Public Sub New(ByVal capacity As Integer)
		MyBase.New(capacity)
	End Sub

	''' <summary>
	''' コンストラクタ
	''' </summary>
	''' <param name="values">新しいリストにコピーする要素元</param>
	''' <remarks></remarks>
	Public Sub New(ByVal values As IList(Of T))
		MyBase.New(values)
	End Sub

#End Region

#Region " Implements "

	Public Shadows Function GetSelected(func As Func(Of SelectedEntity(Of T, TTag), Boolean)) As IEnumerable(Of SelectedEntity(Of T, TTag)) Implements IEntityList(Of T, TTag).GetSelected
		Return Me.Select(Function(row, ii) New SelectedEntity(Of T, TTag)(ii, row)).Where(func)
	End Function

#End Region

End Class
