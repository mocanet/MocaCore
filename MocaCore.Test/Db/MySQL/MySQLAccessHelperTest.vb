Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Moca.Db

<TestClass()>
Public Class MySQLAccessHelperTest
    Inherits MyTestBase

    Private _helper As Moca.Db.Helper.MySQLAccessHelper
    Private dba As Moca.Db.DbAccess

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
        Dim dbms As New Moca.Db.Dbms("MocaCore.Test.My.MySettings.MySQL")
        dba = New Moca.Db.DbAccess(dbms)
        _helper = New Moca.Db.Helper.MySQLAccessHelper(dba)
    End Sub

    ''' <summary>
    ''' 各テストを実行した後に、TestCleanup を使用してコードを実行してください
    ''' </summary>
    <TestCleanup()>
    Public Sub MyTestCleanup()
    End Sub

#End Region

    <TestMethod()>
    Public Sub ConnectionTest()
        Dim dt As DataTable
        dt = _helper.ConnectionTest()

        For Each row As DataRow In dt.Rows
            Debug.Print("[{0}] = {1}, {2}", row(0), row(1), row(2))
        Next
    End Sub

    <TestMethod()>
    Public Sub GetSchemaTablesTest()
        Dim tables As Moca.Db.DbInfoTableCollection
        tables = _helper.GetSchemaTables()

        For Each tbl As DbInfoTable In tables.Values
            Debug.Print(tbl.ToString)
            For Each col As DbInfoColumn In tbl.Columns.Values
                Debug.Print(vbTab & col.ToString)
                Debug.Print(vbTab & "Typ : " & col.Typ)
                Debug.Print(vbTab & "ColumnType : " & col.ColumnType)
                Debug.Print(vbTab & "MaxLength : " & col.MaxLength)
                Debug.Print(vbTab & "MaxLengthB : " & col.MaxLengthB)
                Debug.Print(vbTab & "Precision : " & col.Precision)
                Debug.Print(vbTab & "PrimaryKey : " & col.PrimaryKey)
                Debug.Print(vbTab & "Scale : " & col.Scale)
                Debug.Print(vbTab & "UniCode : " & col.UniCode)
            Next
        Next

        Assert.AreEqual(1, tables.Count)
        Assert.IsTrue(tables.ContainsKey("def.MocaTest.BASE TABLE.mstUser"))
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
        Dim table As New Moca.Db.DbInfoTable("def", "MocaTest", "mstUser", "BASE TABLE")
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

        Assert.IsTrue(columns("Id").PrimaryKey)
    End Sub

    <TestMethod()>
    Public Sub GetSchemaFunctionsTest()
        Dim funcs As DbInfoFunctionCollection
        funcs = _helper.GetSchemaFunctions()

        For Each func As DbInfoFunction In funcs.Values
            Debug.Print("{0},{1},{2},{3}",
                func.Catalog _
             , func.Schema _
             , func.Name _
             , func.Typ)
        Next
    End Sub

    <TestMethod()>
    Public Sub GetSchemaProceduresTest()
        Dim procs As DbInfoProcedureCollection
        procs = _helper.GetSchemaProcedures()

        For Each proc As DbInfoProcedure In procs.Values
            Debug.Print("{0},{1},{2},{3}",
                proc.Catalog _
             , proc.Schema _
             , proc.Name _
             , proc.Typ)
        Next
    End Sub

End Class
