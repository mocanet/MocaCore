
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Moca.Db.Attr

<TestClass()> Public Class SqlDbAccessHelperTest
    Inherits MocaTestBase

    Protected dao As Db.IDaoTest

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
    'Public Sub GetTableInfoTest()
    '    Dim tbl As Moca.Db.DbInfoTable
    '    tbl = dao.GetTableInfo("tbMidasi")
    '    For Each col As Moca.Db.DbInfoColumn In tbl.Columns.Values
    '        Debug.Print(col.Name)
    '        Debug.Print(col.Typ)
    '        Debug.WriteLine("")
    '    Next

    '    Dim lst As IList(Of HogeRow)
    '    lst = dao.FindAll()
    '    For Each row As HogeRow In lst
    '        Debug.WriteLine(row.IdHierarchyid)
   '     Next
    'End Sub

End Class

Public Class HogeRow

    <Column("id")>
    Public Property Id As Integer
    <Column("midasi_id")>
    Public Property IdHierarchyid As Object
    <Column("level")>
    Public Property Level As Integer
    <Column("name")>
    Public Property TreeName As String

End Class
