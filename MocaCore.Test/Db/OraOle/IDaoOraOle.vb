
Imports Moca.Db.Attr

Namespace Db

	''' <summary>
	''' DaoOraOle データアクセスインタフェース
	''' </summary>
	''' <remarks></remarks>
	<Dao("MocaCore.Test.My.MySettings.OraOle", GetType(Impl.DaoOraOle))>
	Public Interface IDaoOraOle

		Function Find(ByVal row As UserRowOra) As IList(Of UserRowOra)

	End Interface

End Namespace
