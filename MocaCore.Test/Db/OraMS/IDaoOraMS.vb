
Imports Moca.Db.Attr

Namespace Db

	''' <summary>
	''' DaoOraMS データアクセスインタフェース
	''' </summary>
	''' <remarks></remarks>
	<Dao("MocaCore.Test.My.MySettings.OraMS", GetType(Impl.DaoOraMS))>
	Public Interface IDaoOraMS

		Function Find(ByVal row As UserRowOra) As IList(Of UserRowOra)

	End Interface

End Namespace
