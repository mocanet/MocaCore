
Imports Moca.Db

Namespace Db.Impl

    ''' <summary>
	''' DaoTest データアクセス
	''' </summary>
    ''' <remarks></remarks>
    Public Class DaoTest
        Inherits AbstractDao
        Implements IDaoTest

        Public Function FindAll() As IList(Of HogeRow) Implements IDaoTest.FindAll
            Const C_SQL As String =
                "SELECT [id]" & vbCrLf &
                "      ,[midasi_id]" & vbCrLf &
                "      ,[level]" & vbCrLf &
                "      ,[name]" & vbCrLf &
                "  FROM [tbMidasi]"

            Using cmd As IDbCommandSelect = CreateCommandSelect(C_SQL)
                Return cmd.Execute(Of HogeRow)()
            End Using

        End Function

        Public Function GetTableInfo(name As String) As DbInfoTable Implements IDaoTest.GetTableInfo
            Return Helper.GetSchemaTable(name)
        End Function

    End Class

End Namespace
