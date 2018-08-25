
Imports Moca.Db

Namespace Db.Impl

    ''' <summary>
	''' DaoPostgreSQL データアクセス
	''' </summary>
    ''' <remarks></remarks>
    Public Class DaoPostgreSQL
        Inherits AbstractDao
        Implements IDaoPostgreSQL

        Public Function Find(row As UserRowPosatgreSql) As IList(Of UserRowPosatgreSql) Implements IDaoPostgreSQL.Find
            Const sql As String = "SELECT * FROM ""mstUser"" WHERE ""Id"" = @ID"
            Return [Select](Of UserRowPosatgreSql)(sql,
                                        New With {
                                        .Id = row.Id
                                        })
        End Function

        Public Function Find2() As IList(Of UserRowPosatgreSql) Implements IDaoPostgreSQL.Find2
            Return QueryProcedure(Of UserRowPosatgreSql)("""funcTest""",
                                        New With {
                                        .param1 = 123,
                                        .param2 = 456
                                        })
        End Function

        Public Sub Ins() Implements IDaoPostgreSQL.Ins
            Insert(New UserRowPosatgreSql() With {
                   .Id = "moca",
                   .Name = "hoge"
                   })
        End Sub

    End Class

End Namespace
