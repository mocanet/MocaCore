Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class DaoMySQLTest
    Inherits MyTestBase

    Private _dao As Db.IDaoMySQL

#Region "追加のテスト属性"
    '
    ' テストを作成する際には、次の追加属性を使用できます:
    '

    ''' <summary>
    ''' クラス内で最初のテストを実行する前に、ClassInitialize を使用してコードを実行してください
    ''' </summary>
    ''' <param name="testContext"></param>
    <ClassInitialize()>
    Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
        MocaInitialize(testContext)
    End Sub

    ''' <summary>
    ''' クラス内のテストをすべて実行したら、ClassCleanup を使用してコードを実行してください
    ''' </summary>
    <ClassCleanup()>
    Public Shared Sub MyClassCleanup()
        MocaCleanup()
    End Sub

    ''' <summary>
    ''' 各テストを実行する前に、TestInitialize を使用してコードを実行してください
    ''' </summary>
    <TestInitialize()>
    Public Sub MyTestInitialize()
        injector.Inject(Me)
    End Sub

    ''' <summary>
    ''' 各テストを実行した後に、TestCleanup を使用してコードを実行してください
    ''' </summary>
    <TestCleanup()>
    Public Sub MyTestCleanup()
    End Sub

#End Region

    '<TestMethod()>
    'Public Sub SelectTest()
    '	Dim lst As IList(Of UserRow)
    '	lst = _dao.Find()
    'End Sub

    <TestMethod()>
    Public Sub SelectTest()
        Dim row As New UserRowMysql
        Dim lst As New List(Of UserRowMysql)

        row.Id = "moca"

        lst = _dao.Find(row)

        Assert.AreEqual(lst.Count, 1)
        Assert.AreEqual(lst.First.Id, "moca")
        Assert.AreEqual(lst.First.Name, "Moca.NET")
        Assert.IsTrue(lst.First.Admin)
    End Sub

    <TestMethod()>
    Public Sub Select2Test()
        Dim lst As New List(Of UserRowMysql)

        lst = _dao.Find2()

        Assert.AreEqual(lst.Count, 1)
        Assert.AreEqual(lst.First.Id, "123")
        Assert.AreEqual(lst.First.Name, "456")
        Assert.IsFalse(lst.First.Admin)
    End Sub

    <TestMethod()>
    Public Sub ErrorTest()
        Dim er As Moca.Db.DbAccessException = Nothing
        Try
            _dao.Ins()
            _dao.Ins()
        Catch ex As Moca.Db.DbAccessException
            er = ex
        End Try
        Assert.IsTrue(er.HasSqlNativeErrorDuplicationPKey)
    End Sub

End Class