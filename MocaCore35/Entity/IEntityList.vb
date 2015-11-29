
''' <summary>
''' エンティティのコレクションを表します。
''' </summary>
''' <typeparam name="T">エンティティの型</typeparam>
''' <remarks></remarks>
Public Interface IEntityList(Of T)
	Inherits IList(Of T)

	''' <summary>
	''' 選択されている行を取得する
	''' </summary>
	''' <param name="func">選択されているかどうかの判定</param>
	''' <returns>IEnumerable(Of SelectedRow(Of TSource))</returns>
	''' <remarks></remarks>
	Function GetSelected(ByVal func As Func(Of SelectedEntity(Of T), Boolean)) As IEnumerable(Of SelectedEntity(Of T))

End Interface

''' <summary>
''' エンティティのコレクションを表します。
''' </summary>
''' <typeparam name="T">エンティティの型</typeparam>
''' <typeparam name="TTag"><see cref="SelectedEntity"></see>.Tag の型</typeparam>
''' <remarks></remarks>
Public Interface IEntityList(Of T, TTag)
	Inherits IEntityList(Of T)

	''' <summary>
	''' 選択されている行を取得する
	''' </summary>
	''' <param name="func">選択されているかどうかの判定</param>
	''' <returns>IEnumerable(Of SelectedRow(Of TSource))</returns>
	''' <remarks></remarks>
	Shadows Function GetSelected(ByVal func As Func(Of SelectedEntity(Of T, TTag), Boolean)) As IEnumerable(Of SelectedEntity(Of T, TTag))

End Interface
