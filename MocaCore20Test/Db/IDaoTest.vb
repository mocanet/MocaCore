
Imports Moca.Db.Attr

Namespace Db

    ''' <summary>
	''' DaoTest データアクセスインタフェース
    ''' </summary>
    ''' <remarks></remarks>
    <Dao("MocaCore20Test.My.MySettings.Db", GetType(Impl.DaoTest))>
    Public Interface IDaoTest

        Function GetTableInfo(ByVal name As String) As Moca.Db.DbInfoTable

        Function FindAll() As IList(Of HogeRow)

    End Interface

End Namespace
