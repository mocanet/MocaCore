
Imports Moca.Db

Namespace Db.Impl

    ''' <summary>
	''' DaoTest データアクセス
	''' </summary>
    ''' <remarks></remarks>
    Public Class DaoTest
        Inherits AbstractDao
        Implements IDaoTest

		Public Function FindDataTable() As DataTable Implements IDaoTest.FindDataTable
			Const sql As String = "select a.* from mstUser a , mstUser b , mstUser c , mstUser d , mstUser e , mstUser f"
			'Const sql As String = "select a.* from mstUser a , mstUser b , mstUser c , mstUser d , mstUser e"

			Using cmd As IDbCommandSelect = CreateCommandSelect(sql)
				cmd.Execute()
				Return cmd.Result1stTable
			End Using
		End Function

		Public Function FindEntity() As IList(Of UserRow) Implements IDaoTest.FindEntity
			Const sql As String = "select a.* from mstUser a , mstUser b , mstUser c , mstUser d , mstUser e , mstUser f"
			'Const sql As String = "select a.* from mstUser a , mstUser b , mstUser c , mstUser d , mstUser e"

			Using cmd As IDbCommandSelect = CreateCommandSelect(sql)
				Return cmd.Execute(Of UserRow)()
			End Using
		End Function

		Public Function Find() As DataTable Implements IDaoTest.Find
			Const sql As String = "select a.*, NULL AS NullValue from mstUser a"

			Using cmd As IDbCommandSelect = CreateCommandSelect(sql)
				cmd.Execute()
				Return cmd.Result1stTable
			End Using
		End Function

		Public Function FindEntityNoProp() As IList(Of UserRow) Implements IDaoTest.FindEntityNoProp
			Const sql As String = "select a.*, Null AS NoProp from mstUser a"

			Using cmd As IDbCommandSelect = CreateCommandSelect(sql)
				Return cmd.Execute(Of UserRow)()
			End Using
		End Function

	End Class

End Namespace
