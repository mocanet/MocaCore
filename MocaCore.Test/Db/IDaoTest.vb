
Imports Moca.Db.Attr

Namespace Db

	''' <summary>
	''' DaoTest データアクセスインタフェース
	''' </summary>
	''' <remarks></remarks>
	<Dao("MocaCore.Test.My.MySettings.MSSQL", GetType(Impl.DaoTest))>
	Public Interface IDaoTest

		Function FindDataTable() As DataTable
		Function FindEntity() As IList(Of UserRow)
		Function Find() As DataTable
		Function FindEntityNoProp() As IList(Of UserRow)

	End Interface

End Namespace
