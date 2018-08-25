
Imports Moca.Db.Attr

Namespace Db

    ''' <summary>
	''' DaoPostgreSQL データアクセスインタフェース
    ''' </summary>
    ''' <remarks></remarks>
    <Dao("MocaCore.Test.My.MySettings.PostgreSQL", GetType(Impl.DaoPostgreSQL))>
    Public Interface IDaoPostgreSQL

        Function Find(ByVal row As UserRowPosatgreSql) As IList(Of UserRowPosatgreSql)

        Function Find2() As IList(Of UserRowPosatgreSql)

        <Transaction()>
        Sub Ins()

    End Interface

End Namespace
