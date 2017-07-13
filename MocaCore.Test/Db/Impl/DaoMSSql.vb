
Imports Moca.Db

Namespace Db.Impl

    ''' <summary>
	''' DaoMSSql データアクセス
	''' </summary>
    ''' <remarks></remarks>
    Public Class DaoMSSql
        Inherits AbstractDao
        Implements IDaoMSSql

        Public Function Find() As IList(Of UserRow) Implements IDaoMSSql.Find
            Const sql As String = "SELECT * FROM mstUser"

            Using cmd As IDbCommandSelect = CreateCommandSelect(sql)
                'cmd.SetParameter("param name", Nothing)

                Return cmd.Execute(Of UserRow)()
            End Using
        End Function

        Public Function Insert(row As UserRow) As Integer Implements IDaoMSSql.Insert
            Const sql As String = "INSERT INTO mstUser (Id,Name,InsertDate, UpdateDate) VALUES (@Id, @Name, GETDATE(), GETDATE())"

            Using cmd As IDbCommandInsert = CreateCommandInsert(sql)
                cmd.SetParameter("Name", row.Name)
                cmd.SetParameter("Id", row.Id)

                Dim rc As Integer
                rc = cmd.Execute()
                Return rc
            End Using
        End Function

    End Class

End Namespace
