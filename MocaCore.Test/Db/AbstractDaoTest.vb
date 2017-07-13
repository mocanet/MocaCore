Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class AbstractDaoTest
    Inherits MocaTestBase

    Private _daoMSSql As Db.IDaoMSSql
    Private _dao As Db.IDaoTest

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

    <TestMethod()>
    Public Sub InsertTest()
        Dim row As New UserRow
        Dim count As Integer

        row.Id = "moca"
        row.Name = "Moca.NET"

        count = _daoMSSql.Insert(row)
        Assert.AreEqual(1, count)
    End Sub

    <TestMethod()>
    Public Sub SelectTest()
        Dim lst As IList(Of UserRow)
        lst = _daoMSSql.Find()
    End Sub

    '<TestMethod()>
    'Public Sub SelectDataTableCancelTest()
    '    Dim task As New Threading.Thread(
    '        New Threading.ThreadStart(
    '        AddressOf _doSomething))
    '    'スレッドを開始する
    '    task.Start()

    '    Threading.Thread.Sleep(3000)

    '    Assert.IsTrue(task.IsAlive)
    '    Dim idao As Moca.Db.IDao = _dao
    '    idao.Cancel()
    '    Assert.IsFalse(task.IsAlive)
    'End Sub

    'Private Sub _doSomething()
    '    _dao.FindTakeTime()
    'End Sub

    '<TestMethod()>
    'Public Sub SelectEntityCancelTest()
    '    Dim task As New Threading.Thread(
    '        New Threading.ThreadStart(
    '        AddressOf _doSomething2))
    '    'スレッドを開始する
    '    task.Start()

    '    Threading.Thread.Sleep(3000)

    '    Assert.IsTrue(task.IsAlive)
    '    Dim idao As Moca.Db.IDao = _dao
    '    idao.Cancel()
    '    Assert.IsFalse(task.IsAlive)
    'End Sub

    'Private Sub _doSomething2()
    '    _dao.FindTakeTime2()
    'End Sub

End Class
