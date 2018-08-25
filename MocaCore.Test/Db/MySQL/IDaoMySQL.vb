
Imports Moca.Db.Attr

Namespace Db

    ''' <summary>
	''' DaoMySQL データアクセスインタフェース
    ''' </summary>
    ''' <remarks></remarks>
    <Dao("MocaCore.Test.My.MySettings.MySQL", GetType(Impl.DaoMySQL))>
    Public Interface IDaoMySQL

        Function Find(ByVal row As UserRowMysql) As IList(Of UserRowMysql)

        Function Find2() As IList(Of UserRowMysql)

        <Transaction()>
        Sub Ins()

    End Interface

End Namespace
