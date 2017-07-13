
Imports Moca.Db.Attr

Namespace Db

    ''' <summary>
	''' DaoTest データアクセスインタフェース
    ''' </summary>
    ''' <remarks></remarks>
    <Dao("MocaCore.Test.My.MySettings.Test", GetType(Impl.DaoTest))>
    Public Interface IDaoTest

        Function FindTakeTime() As DataTable
        Function FindTakeTime2() As IList(Of TestRow)

    End Interface

End Namespace
