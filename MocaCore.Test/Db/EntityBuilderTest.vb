Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class EntityBuilderTest
	Inherits MyTestBase

	Private _dao As Db.IDaoTest
	Private eb As Moca.Db.EntityBuilder

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
		eb = New Moca.Db.EntityBuilder
	End Sub

	''' <summary>
	''' 各テストを実行した後に、TestCleanup を使用してコードを実行してください
	''' </summary>
	<TestCleanup()>
	Public Sub MyTestCleanup()
	End Sub

#End Region

	<TestMethod()>
	Public Sub ConvertTest()
		Dim dt As DataTable
		Dim dr As DataRow
		Dim val As New mstUser() With {
				.Id = 999,
				.Nm = "Hoge",
				.Remarks = "Hoge note"
			}

		dt = _dao.Find()
		dr = dt.Rows(3)
		eb.Convert(val, dr)

		Assert.AreEqual(val.Id, dr.Item("Id"))
		Assert.AreEqual(val.Nm, dr.Item("Name"))
		Assert.AreEqual(val.Remarks, dr.Item("Note"))
		Assert.AreEqual(dr.Item("NullValue"), DBNull.Value)
		Assert.AreEqual(val.DBNullValue, Nothing)
	End Sub

	<TestMethod()>
	Public Sub CreateDataRowTest()
		Dim dt As DataTable
		Dim dr As DataRow
		Dim val As mstUser

		dt = _dao.Find()
		dr = dt.Rows(3)
		val = eb.Create(Of mstUser)(dr)

		Assert.AreEqual(val.Id, dr.Item("Id"))
		Assert.AreEqual(val.Nm, dr.Item("Name"))
		Assert.AreEqual(dr.Item("Note"), DBNull.Value)
		Assert.AreEqual(val.Remarks, Nothing)
		Assert.AreEqual(dr.Item("NullValue"), DBNull.Value)
		Assert.AreEqual(val.DBNullValue, DBNull.Value)
	End Sub

	<TestMethod()>
	Public Sub CreateDataTableTest()
		Dim dt As DataTable
		Dim dr As DataRow
		Dim lst As IList(Of mstUser)
		Dim val As mstUser

		dt = _dao.Find()
		dr = dt.Rows(3)
		lst = eb.Create(Of mstUser)(dt)
		val = lst(3)

		Assert.AreEqual(lst.Count, dt.Rows.Count)

		Assert.AreEqual(val.Id, dr.Item("Id"))
		Assert.AreEqual(val.Nm, dr.Item("Name"))
		Assert.AreEqual(dr.Item("Note"), DBNull.Value)
		Assert.AreEqual(val.Remarks, Nothing)
		Assert.AreEqual(dr.Item("NullValue"), DBNull.Value)
		Assert.AreEqual(val.DBNullValue, DBNull.Value)
	End Sub

	<TestMethod()>
	Public Sub MissingMemberExceptionTest()
		Try
			_dao.FindEntityNoProp()
		Catch ex As Exception
			Assert.IsInstanceOfType(ex, GetType(Moca.Db.DbAccessException))
			Assert.AreEqual("メンバー 'MocaCore.Test.UserRow.NoProp' が見つかりません。", ex.Message)
		End Try
	End Sub

End Class