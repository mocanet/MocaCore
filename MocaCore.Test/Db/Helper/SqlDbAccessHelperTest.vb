Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class SqlDbAccessHelperTest
    Inherits MocaTestBase

    Private _helper As Moca.Db.Helper.SqlDbAccessHelper

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
        Dim dbms As New Moca.Db.Dbms("MocaCore.Test.My.MySettings.MSSQL")
        Dim dba As New Moca.Db.DbAccess(dbms)
        _helper = New Moca.Db.Helper.SqlDbAccessHelper(dba)
    End Sub

    ''' <summary>
    ''' 各テストを実行した後に、TestCleanup を使用してコードを実行してください
    ''' </summary>
    <TestCleanup()>
    Public Sub MyTestCleanup()
    End Sub

#End Region

    <TestMethod()>
    Public Sub GetSchemaTablesTest()
        Dim tables As Moca.Db.DbInfoTableCollection
        tables = _helper.GetSchemaTables()

        Assert.AreEqual(4, tables.Count)
        Assert.IsTrue(tables.ContainsKey("MocaTest.dbo.BASE TABLE.__RefactorLog"))
        Assert.IsTrue(tables.ContainsKey("MocaTest.dbo.BASE TABLE.mstGroup"))
        Assert.IsTrue(tables.ContainsKey("MocaTest.dbo.BASE TABLE.mstUser"))
        Assert.IsTrue(tables.ContainsKey("MocaTest.dbo.BASE TABLE.mstUserGroupMap"))
    End Sub

    <TestMethod()>
    Public Sub GetSchemaTableTest()
        Dim table As Moca.Db.DbInfoTable
        table = _helper.GetSchemaTable("mstUser")

        Assert.AreEqual("mstUser", table.Name)
        Assert.AreEqual(7, table.Columns.Count)
    End Sub

    <TestMethod()>
    Public Sub GetSchemaColumnsTest()
        Dim table As New Moca.Db.DbInfoTable("MocaTest", "dbo", "mstUser", "BASE TABLE")
        Dim columns As Moca.Db.DbInfoColumnCollection
        columns = _helper.GetSchemaColumns(table)

        Assert.AreEqual(7, columns.Count)
        Assert.IsTrue(columns.ContainsKey("Id"))
        Assert.IsTrue(columns.ContainsKey("Name"))
        Assert.IsTrue(columns.ContainsKey("Mail"))
        Assert.IsTrue(columns.ContainsKey("Note"))
        Assert.IsTrue(columns.ContainsKey("Admin"))
        Assert.IsTrue(columns.ContainsKey("InsertDate"))
        Assert.IsTrue(columns.ContainsKey("UpdateDate"))
    End Sub

End Class
