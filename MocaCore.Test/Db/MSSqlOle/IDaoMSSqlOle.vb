
Imports Moca.Db.Attr

Namespace Db

	''' <summary>
	''' DaoMSSqlOle データアクセスインタフェース
	''' </summary>
	''' <remarks></remarks>
	<Dao("MocaCore.Test.My.MySettings.MSSQLOle", GetType(Impl.DaoMSSqlOle))>
	Public Interface IDaoMSSqlOle

		Function Find(ByVal row As UserRow) As IList(Of UserRow)

	End Interface

End Namespace
