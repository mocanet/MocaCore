Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class DaoOraTest
	Inherits MyTestBase

	Private _dao As Db.IDaoOra

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
	Public Sub SelectTest2()
		Dim row As New UserRowOra
		Dim lst As New List(Of UserRowOra)

		row.Id = "moca"

		lst = _dao.Find(row)

		Assert.AreEqual(lst.Count, 1)
		Assert.AreEqual(lst.First.Id, "moca")
		Assert.AreEqual(lst.First.Name, "Moca.NET")
	End Sub

End Class