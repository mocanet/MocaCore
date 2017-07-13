
Imports Moca.Db.Attr

Namespace Db

    ''' <summary>
	''' DaoMSSql データアクセスインタフェース
    ''' </summary>
    ''' <remarks></remarks>
    <Dao("MocaCore.Test.My.MySettings.MSSQL", GetType(Impl.DaoMSSql))>
    Public Interface IDaoMSSql

        <Transaction()>
        Function Insert(ByVal row As UserRow) As Integer

        Function Find() As IList(Of UserRow)

    End Interface

End Namespace
