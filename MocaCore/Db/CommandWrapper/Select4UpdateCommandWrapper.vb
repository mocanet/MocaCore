
Namespace Db.CommandWrapper

	''' <summary>
	''' SELECT文を実行し、DataSetを使ってUPDATEする為のDBCommandのラッパークラス
	''' </summary>
	''' <remarks></remarks>
	Public Class Select4UpdateCommandWrapper
		Inherits SelectCommandWrapper
		Implements IDbCommandSelect4Update

		''' <summary>アダプタオブジェクト</summary>
		Private _adp As IDbDataAdapter

#Region " Constructor/DeConstructor "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="dba">親となるDBAccessインスタンス</param>
		''' <param name="cmd">実行するDBCommandインスタンス</param>
		''' <remarks>
		''' </remarks>
		Friend Sub New(ByVal dba As IDao, ByVal cmd As IDbCommand)
			MyBase.New(dba, cmd)
			Me._adp = dba.Dbms.ProviderFactory.CreateDataAdapter()
		End Sub

#End Region

#Region " Property "

		''' <summary>
		''' アダプタインスタンス
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Adapter() As IDbDataAdapter Implements IDbCommandSelect4Update.Adapter
			Get
				Return _adp
			End Get
		End Property

#End Region

		''' <summary>
		''' Select SQL実行！
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overrides Function Execute() As Integer
			Return dba.Execute(Me)
		End Function

		''' <summary>
		''' Adapter Update 実行！
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Update() As Integer Implements IDbCommandSelect4Update.Update
			Return dba.UpdateAdapter(Me.ResultDataSet, Me.Adapter)
		End Function

	End Class

End Namespace
