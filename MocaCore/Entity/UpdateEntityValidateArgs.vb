
Imports Moca.Util

#Region " Delegate "

''' <summary>
''' エンティティに対してコントロールの値を設定する際に検証した結果を処理するデリゲート
''' </summary>
''' <param name="sender"></param>
''' <param name="e"></param>
''' <remarks></remarks>
Public Delegate Sub UpdateEntityValidate(ByVal sender As Object, ByVal e As UpdateEntityValidateArgs)

#End Region

#Region " Delegate Args "

''' <summary>
''' エンティティに対してコントロールの値を設定する際に検証した結果を処理するデリゲートの引数
''' </summary>
''' <remarks></remarks>
Public Class UpdateEntityValidateArgs
	Inherits System.EventArgs

	Private _errorColumns As IList(Of String)

#Region " コンストラクタ "

	''' <summary>
	''' コンストラクタ
	''' </summary>
	''' <param name="errorColumns"></param>
	''' <remarks></remarks>
	Public Sub New(ByVal errorColumns As IList(Of String))
		_errorColumns = errorColumns
	End Sub

#End Region

#Region " Property "

	''' <summary>エンティティのプロパティ名</summary>
	Public Property EntityPropertyName As String
	''' <summary>エンティティのプロパティキャプション</summary>
	Public Property Caption As String

	''' <summary>検証種別</summary>
	Public Property ValidateType As ValidateTypes
	''' <summary>検証結果</summary>
	Public Property ValidateResultType As ValidateTypes

	''' <summary>最小値</summary>
	Public Property Min As Object

	''' <summary>最大値</summary>
	Public Property Max As Object

	''' <summary>検証の中止有無</summary>
	Public Property ValidateStop As Boolean

	''' <summary>値</summary>
	Public Property Value As Object

	''' <summary>検証結果</summary>
	Public Property IsValid As Boolean

	''' <summary>
	''' エラー発生コントロールか返す
	''' </summary>
	''' <param name="caption"></param>
	''' <value></value>
	''' <returns></returns>
	''' <remarks></remarks>
	ReadOnly Property IsError(ByVal caption As String) As Boolean
		Get
			Return _errorColumns.Contains(caption)
		End Get
	End Property

#End Region

End Class

#End Region
