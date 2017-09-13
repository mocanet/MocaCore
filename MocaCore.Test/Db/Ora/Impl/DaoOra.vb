
Imports Moca.Db

Namespace Db.Impl

	''' <summary>
	''' DaoOra データアクセス
	''' </summary>
	''' <remarks></remarks>
	Public Class DaoOra
		Inherits AbstractDao
		Implements IDaoOra

		Public Function Find(row As UserRowOra) As IList(Of UserRowOra) Implements IDaoOra.Find
			Const sql As String = "SELECT * FROM MST_USER WHERE ID = :ID"
			Return [Select](Of UserRowOra)(sql,
										New With {
										.ID = row.Id
										})
		End Function

	End Class

End Namespace
