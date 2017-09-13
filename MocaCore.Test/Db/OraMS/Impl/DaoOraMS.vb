
Imports Moca.Db

Namespace Db.Impl

	''' <summary>
	''' DaoOraMS データアクセス
	''' </summary>
	''' <remarks></remarks>
	Public Class DaoOraMS
		Inherits AbstractDao
		Implements IDaoOraMS

		Public Function Find(row As UserRowOra) As IList(Of UserRowOra) Implements IDaoOraMS.Find
			Const sql As String = "SELECT * FROM MST_USER WHERE ID = :ID"
			Return [Select](Of UserRowOra)(sql,
										New With {
										.ID = row.Id
										})
		End Function

	End Class

End Namespace
