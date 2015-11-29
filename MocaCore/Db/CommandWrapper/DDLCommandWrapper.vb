
Namespace Db.CommandWrapper

	''' <summary>
	''' DDL文を実行する為のDBCommandのラッパークラス
	''' </summary>
	''' <remarks></remarks>
	Public Class DDLCommandWrapper
		Inherits SqlCommandWrapper
		Implements IDbCommandDDL

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
		End Sub

#End Region

		''' <summary>
		''' SQL実行！
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overrides Function Execute() As Integer
			Return dba.ExecuteNonQuery(Me)
		End Function

	End Class

End Namespace
