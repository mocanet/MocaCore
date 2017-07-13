
''' <summary>
''' 選択されたエンティティ
''' </summary>
''' <typeparam name="T">エンティティの型</typeparam>
''' <remarks></remarks>
Public Class SelectedEntity(Of T)

#Region " Declare "

	''' <summary>行番号</summary>
	Private _index As Integer
	''' <summary>エンティティ</summary>
	Private _entity As T

#End Region

#Region " コンストラクタ "

	''' <summary>
	''' コンストラクタ
	''' </summary>
	''' <param name="index"></param>
	''' <param name="entity"></param>
	''' <remarks></remarks>
	Public Sub New(ByVal index As Integer, ByVal entity As T)
		_index = index
		_entity = entity
	End Sub

#End Region

#Region " Property "

	''' <summary>
	''' 行番号
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property Index As Integer
		Get
			Return _index
		End Get
	End Property

	''' <summary>
	''' エンティティ
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public ReadOnly Property Entity As T
		Get
			Return _entity
		End Get
	End Property

	''' <summary>
	''' 選択されたエンティティに関連するデータを格納する
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Property Tag As Object

#End Region

End Class


''' <summary>
''' 選択されたエンティティ
''' </summary>
''' <typeparam name="T">エンティティの型</typeparam>
''' <typeparam name="TTag">Tag の型</typeparam>
''' <remarks></remarks>
Public Class SelectedEntity(Of T, TTag)
	Inherits SelectedEntity(Of T)

#Region " コンストラクタ "

	''' <summary>
	''' コンストラクタ
	''' </summary>
	''' <param name="index"></param>
	''' <param name="entity"></param>
	''' <remarks></remarks>
	Public Sub New(ByVal index As Integer, ByVal entity As T)
		MyBase.New(index, entity)
	End Sub

#End Region

#Region " Property "

	''' <summary>
	''' 選択されたエンティティに関連するデータを格納する
	''' </summary>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shadows Property Tag As TTag

#End Region

End Class
