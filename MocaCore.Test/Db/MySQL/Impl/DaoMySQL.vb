
Imports Moca.Db

Namespace Db.Impl

    ''' <summary>
	''' DaoMySQL データアクセス
	''' </summary>
    ''' <remarks></remarks>
    Public Class DaoMySQL
        Inherits AbstractDao
        Implements IDaoMySQL

        Public Function Find(row As UserRowMysql) As IList(Of UserRowMysql) Implements IDaoMySQL.Find
            Const sql As String = "SELECT * FROM mstUser WHERE ID = @ID"
            Return [Select](Of UserRowMysql)(sql,
                                        New With {
                                        .ID = row.Id
                                        })
        End Function

        Public Function Find2() As IList(Of UserRowMysql) Implements IDaoMySQL.Find2
            Return QueryProcedure(Of UserRowMysql)("DaoUser_Find",
                                        New With {
                                        .param1 = 123,
                                        .param2 = 456
                                        })

        End Function

        Public Sub Ins() Implements IDaoMySQL.Ins
            Insert(New UserRowMysql() With {
                   .Id = "moca",
                   .Name = "hoge"
                   })
        End Sub

    End Class

End Namespace
