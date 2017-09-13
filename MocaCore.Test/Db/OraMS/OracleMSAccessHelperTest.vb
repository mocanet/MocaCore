Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class OracleMSAccessHelperTest
	Inherits MyTestBase

	Private _helper As Moca.Db.Helper.OracleMSAccessHelper

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
		Dim dbms As New Moca.Db.Dbms("MocaCore.Test.My.MySettings.OraMS")
		Dim dba As New Moca.Db.DbAccess(dbms)
		_helper = New Moca.Db.Helper.OracleMSAccessHelper(dba)
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

		Assert.AreEqual(1, tables.Count)
		Assert.IsTrue(tables.ContainsKey("MIYABIS.MIYABIS.User.MST_USER"))
	End Sub

	<TestMethod()>
	Public Sub GetSchemaTableTest()
		Dim table As Moca.Db.DbInfoTable
		table = _helper.GetSchemaTable("MST_USER")

		Assert.AreEqual("MST_USER", table.Name)
		Assert.AreEqual(7, table.Columns.Count)
	End Sub

	<TestMethod()>
	Public Sub GetSchemaColumnsTest()
		Dim table As New Moca.Db.DbInfoTable("MIYABIS", "MIYABIS", "MST_USER", "TABLE")
		Dim columns As Moca.Db.DbInfoColumnCollection
		columns = _helper.GetSchemaColumns(table)

		Assert.AreEqual(7, columns.Count)
		Assert.IsTrue(columns.ContainsKey("ID"))
		Assert.IsTrue(columns.ContainsKey("NAME"))
		Assert.IsTrue(columns.ContainsKey("MAIL"))
		Assert.IsTrue(columns.ContainsKey("NOTE"))
		Assert.IsTrue(columns.ContainsKey("ADMIN"))
		Assert.IsTrue(columns.ContainsKey("INSERTDATE"))
		Assert.IsTrue(columns.ContainsKey("UPDATEDATE"))

		Assert.IsTrue(columns("ID").PrimaryKey)
	End Sub

End Class
