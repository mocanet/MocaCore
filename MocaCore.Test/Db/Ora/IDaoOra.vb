
Imports Moca.Db.Attr

Namespace Db

	''' <summary>
	''' DaoOra データアクセスインタフェース
	''' </summary>
	''' <remarks></remarks>
	<Dao("MocaCore.Test.My.MySettings.Ora", GetType(Impl.DaoOra))>
	Public Interface IDaoOra

		Function Find(ByVal row As UserRowOra) As IList(Of UserRowOra)

	End Interface

End Namespace
