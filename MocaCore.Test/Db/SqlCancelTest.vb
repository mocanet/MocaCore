Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class SqlCancelTest
	Inherits MyTestBase

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
		MyTestBaseInitialize()
	End Sub

	''' <summary>
	''' 各テストを実行した後に、TestCleanup を使用してコードを実行してください
	''' </summary>
	<TestCleanup()>
	Public Sub MyTestCleanup()
	End Sub

#End Region

	<TestMethod()>
	Public Sub SelectDataTableCancelTest()
		Try
			Dim tsk As Task = Task.Run(Sub() _dao.FindDataTable())

			Threading.Thread.Sleep(3000)

			Dim daoCancel As Moca.Db.IDaoCancel = _dao
			daoCancel.Cancel()
			tsk.Wait()
		Catch ex As Exception
			Console.WriteLine(ex.InnerException.ToString)
			Dim innerEx As SqlClient.SqlException = CType(ex.InnerException.InnerException, SqlClient.SqlException)
			Console.WriteLine("Error:{0}, Number:{1}, Errors:{2}", innerEx.ErrorCode, innerEx.Number, innerEx.Errors)
			Assert.IsTrue(ex.InnerException.Message.Contains("ユーザーによって操作がキャンセルされました。"))
			Return
		End Try
		Assert.Fail("例外が発生しませんでした")
	End Sub

	<TestMethod()>
	Public Sub SelectEntityCancelTest()
		Try
			Dim tsk As Task = Task.Run(Sub() _dao.FindEntity())

			Threading.Thread.Sleep(3000)

			Dim daoCancel As Moca.Db.IDaoCancel = _dao
			daoCancel.Cancel()
			tsk.Wait()
		Catch ex As Exception
			Console.WriteLine(ex.InnerException.ToString)
			Assert.IsTrue(ex.InnerException.Message.Contains("ユーザーによって操作がキャンセルされました。"))
			Return
		End Try
		Assert.Fail("例外が発生しませんでした")
	End Sub

End Class
