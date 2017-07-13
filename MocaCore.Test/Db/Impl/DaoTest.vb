
Imports Moca.Db

Namespace Db.Impl

    ''' <summary>
	''' DaoTest データアクセス
	''' </summary>
    ''' <remarks></remarks>
    Public Class DaoTest
        Inherits AbstractDao
        Implements IDaoTest

        Public Function FindTakeTime() As DataTable Implements IDaoTest.FindTakeTime
            Const sql As String = "select a.* from trnTable a , trnTable b , trnTable c , trnTable d , trnTable e , trnTable f , trnTable g"

            Using cmd As IDbCommandSelect = CreateCommandSelect(sql)
                cmd.Execute()
                Return cmd.Result1stTable
            End Using
        End Function

        Public Function FindTakeTime2() As IList(Of TestRow) Implements IDaoTest.FindTakeTime2
            Const sql As String = "select a.* from trnTable a , trnTable b , trnTable c , trnTable d , trnTable e , trnTable f , trnTable g"

            Using cmd As IDbCommandSelect = CreateCommandSelect(sql)
                Return cmd.Execute(Of TestRow)()
            End Using
        End Function

    End Class

End Namespace
