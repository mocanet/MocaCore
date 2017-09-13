
Imports Moca.Db

Namespace Db.Impl

	''' <summary>
	''' DaoMSSqlOle データアクセス
	''' </summary>
	''' <remarks></remarks>
	Public Class DaoMSSqlOle
		Inherits AbstractDao
		Implements IDaoMSSqlOle

		Public Function Find(row As UserRow) As IList(Of UserRow) Implements IDaoMSSqlOle.Find
			Const sql As String = "SELECT * FROM mstUser WHERE Id = ?"
			Return [Select](Of UserRow)(sql,
										New With {
										.Param1 = row.Id
										})
		End Function

	End Class

End Namespace
