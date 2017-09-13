
Imports Moca.Db

Namespace Db.Impl

	''' <summary>
	''' DaoOraOle データアクセス
	''' </summary>
	''' <remarks></remarks>
	Public Class DaoOraOle
		Inherits AbstractDao
		Implements IDaoOraOle

		Public Function Find(row As UserRowOra) As IList(Of UserRowOra) Implements IDaoOraOle.Find
			Const sql As String = "SELECT * FROM MST_USER WHERE ID = ?"
			Return [Select](Of UserRowOra)(sql,
										New With {
										.ID = row.Id
										})
		End Function

	End Class

End Namespace
