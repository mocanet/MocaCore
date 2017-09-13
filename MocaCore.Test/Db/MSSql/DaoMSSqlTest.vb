Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class DaoMSSqlTest
	Inherits MyTestBase

	Private _daoMSSql As Db.IDaoMSSql

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
		MyTestBaseClassInitialize(testContext)
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
		MyTestBaseInitialize()
	End Sub

	''' <summary>
	''' 各テストを実行した後に、TestCleanup を使用してコードを実行してください
	''' </summary>
	<TestCleanup()>
	Public Sub MyTestCleanup()
	End Sub

#End Region

#Region " Select "

	<TestMethod()>
	Public Sub SelectTest()
		Dim lst As IList(Of UserRow)
		lst = _daoMSSql.Find()
	End Sub

	<TestMethod()>
	Public Sub SelectTest2()
		Dim row As New UserRow
		Dim lst As New List(Of UserRow)

		row.Id = "moca"

		lst = _daoMSSql.Find2(row)

		Assert.AreEqual(lst.Count, 1)
		Assert.AreEqual(lst.First.Id, "moca")
		Assert.AreEqual(lst.First.Name, "Moca.NET")
	End Sub

	<TestMethod()>
	Public Sub SelectTest3()
		Dim row As New UserRow
		Dim value As String

		row.Id = "moca20"

		value = _daoMSSql.Find3(row)

		Assert.AreEqual(value, "Moca.NET")
	End Sub

#End Region
#Region " Insert "

	Public Sub InsertTest()
		Dim row As New UserRow
		Dim count As Integer

		row.Id = "999"
		row.Name = "Moca.NET 999"

		count = _daoMSSql.Ins(row)
		Assert.AreEqual(1, count)
	End Sub

	<TestMethod()>
	Public Sub InsertTest2()
		Dim row As New UserRow
		Dim count As Integer

		row.Id = "888"
		row.Name = "Moca.NET 888"

		count = _daoMSSql.Ins2(row)
		Assert.AreEqual(1, count)
	End Sub

	<TestMethod()>
	Public Sub InsertTest3()
		Dim lst As New List(Of UserRow)
		Dim row As New UserRow
		Dim count As Integer

		lst.Add(New UserRow() With {
					.Id = "111",
					.Name = "Moca.NET 111"
				})
		lst.Add(New UserRow() With {
					.Id = "222",
					.Name = "Moca.NET 222"
				})
		lst.Add(New UserRow() With {
					.Id = "333",
					.Name = "Moca.NET 333"
				})

		count = _daoMSSql.Ins2(lst)
		Assert.AreEqual(3, count)
	End Sub

	<TestMethod()>
	Public Sub InsertTest4()
		Dim row As New UserRow
		Dim count As Integer

		row.Id = "888"
		row.Name = "Moca.NET 888"
		row.Mail = "test@hoge.com"
		row.Note = "ほげ"
		row.Admin = True
		row.InsertDate = Now
		row.UpdateDate = Now.AddMonths(2)

		count = _daoMSSql.Ins3(row)
		Assert.AreEqual(1, count)
	End Sub

#End Region
#Region " Update "

	<TestMethod()>
	Public Sub UpdateTest()
		Dim row As New UserRow
		Dim count As Integer

		row.Id = "moca110"
		row.Name = "test hoge !!"
		row.Mail = "test@hoge.com"
		row.Note = "ほげ"
		row.Admin = True
		row.InsertDate = Now
		row.UpdateDate = Now.AddMonths(2)

		count = _daoMSSql.Upd(row)
		Assert.AreEqual(1, count)
	End Sub

	<TestMethod()>
	Public Sub UpdateTest2()
		Dim row As New UserRow
		Dim count As Integer

		row.Id = "moca110"
		row.Name = "test hoge !!"
		row.Mail = "test@hoge.com"
		row.Note = "ほげ"
		row.Admin = True
		row.InsertDate = Now
		row.UpdateDate = Now.AddMonths(2)

		count = _daoMSSql.Upd2(row)
		Assert.AreEqual(1, count)
	End Sub

	<TestMethod()>
	Public Sub UpdateTest3()
		Dim lst As New List(Of UserRow)
		Dim count As Integer

		lst.Add(New UserRow() With {
					.Id = "moca20",
					.Name = "Moca.NET 111",
					.Mail = "test111@hoge.com",
					.Note = "ほげ111",
					.Admin = True,
					.InsertDate = Now,
					.UpdateDate = Now.AddMonths(2)
				})
		lst.Add(New UserRow() With {
					.Id = "moca50",
					.Name = "Moca.NET 222",
					.Mail = "test222@hoge.com",
					.Note = "ほげ222",
					.Admin = True,
					.InsertDate = Now,
					.UpdateDate = Now.AddMonths(2)
				})
		lst.Add(New UserRow() With {
					.Id = "moca80",
					.Name = "Moca.NET 333",
					.Mail = "test333@hoge.com",
					.Note = "ほげ333",
					.Admin = True,
					.InsertDate = Now,
					.UpdateDate = Now.AddMonths(2)
				})

		count = _daoMSSql.Upd2(lst)
		Assert.AreEqual(3, count)
	End Sub

#End Region
#Region " Delete "

	<TestMethod()>
	Public Sub DeleteTest()
		Dim row As New UserRow
		Dim count As Integer

		row.Id = "moca110"

		count = _daoMSSql.Del(row)
		Assert.AreEqual(1, count)
	End Sub

	<TestMethod()>
	Public Sub DeleteTest2()
		Dim row As New UserRow
		Dim count As Integer

		row.Id = "moca110"

		count = _daoMSSql.Del2(row)
		Assert.AreEqual(1, count)
	End Sub

	<TestMethod()>
	Public Sub DeleteTest3()
		Dim lst As New List(Of UserRow)
		Dim count As Integer

		lst.Add(New UserRow() With {
					.Id = "moca20"
				})
		lst.Add(New UserRow() With {
					.Id = "moca50"
				})
		lst.Add(New UserRow() With {
					.Id = "moca80"
				})

		count = _daoMSSql.Del2(lst)
		Assert.AreEqual(3, count)
	End Sub

#End Region
#Region " Procedure "

	<TestMethod()>
	Public Sub QueryProcedureTest()
	End Sub

	<TestMethod()>
	Public Sub UpdateProcedureTest()
	End Sub

	<TestMethod()>
	Public Sub ScalarProcedureTest()
	End Sub

#End Region

End Class
