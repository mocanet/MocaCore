Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class SpeedTest
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
	Public Sub SelectSpeedTest()
#If net20 Then
		Console.WriteLine(".NET 2.0")
#End If
#If net35 Then
		Console.WriteLine(".NET 3.5")
#End If
#If net40 Then
		Console.WriteLine(".NET 4.0")
#End If
#If net462 Then
		Console.WriteLine(".NET 4.6.2")
#End If
		_dataTable()
		_moca()
		_ef()
	End Sub

	Private Sub _dataTable()
		Dim dt As DataTable
		Dim sw As Stopwatch

		sw = Stopwatch.StartNew()
		dt = _dao.FindDataTable()
		sw.Stop()
		_log(sw, dt.Rows.Count, "Adapter Fill DataTable")

		sw = Stopwatch.StartNew()
		dt = _dao.FindDataTable()
		sw.Stop()
		_log(sw, dt.Rows.Count, "Adapter Fill DataTable")
	End Sub

	Private Sub _moca()
		Dim lst As IList
		Dim sw As Stopwatch

		sw = Stopwatch.StartNew()
		lst = _dao.FindEntity()
		sw.Stop()
		_log(sw, lst.Count, "Moca.NET")

		sw = Stopwatch.StartNew()
		lst = _dao.FindEntity()
		sw.Stop()
		_log(sw, lst.Count, "Moca.NET")
	End Sub

	Private Sub _ef()
		Const cSql As String = "select a.* from mstUser a , mstUser b , mstUser c , mstUser d , mstUser e , mstUser f"
		'Const cSql As String = "select a.* from mstUser a , mstUser b , mstUser c , mstUser d , mstUser e"
		Dim sw As Stopwatch

		sw = Stopwatch.StartNew()
		Dim rows
		Using db As New MocaTestDb
			rows = db.Database.SqlQuery(Of UserRow)(cSql).ToList
		End Using
		sw.Stop()
		_log(sw, CInt(rows.Count), "EntityFramework")

		sw = Stopwatch.StartNew()
		Using db As New MocaTestDb
			rows = db.Database.SqlQuery(Of UserRow)(cSql).ToList
		End Using
		sw.Stop()
		_log(sw, CInt(rows.Count), "EntityFramework")
	End Sub

	Private Sub _log(ByVal sw As Stopwatch, ByVal count As Integer, ByVal msg As String)
		Console.WriteLine()
		Console.WriteLine("{0} = {1} 件", msg, count.ToString("#,0"))
		Console.WriteLine("経過時間の合計 = {0}", sw.Elapsed)
		Console.WriteLine("ミリ秒単位の経過時間の合計 = {0}", sw.ElapsedMilliseconds)
		Console.WriteLine("経過時間の分解能 (1秒あたり) = {0}", Stopwatch.Frequency)
	End Sub

	<TestMethod()>
	Public Sub SelectSpeedTest2()
		Dim sw As Stopwatch
		Dim lst As IList
		sw = Stopwatch.StartNew()
		lst = _dao.FindEntity()
		sw.Stop()

#If net20 Then
		Console.WriteLine(".NET 2.0")
#Else
		Console.WriteLine(".NET 3.5 over")
#End If
		Console.WriteLine()
		Console.WriteLine("Entity = {0} 件", lst.Count.ToString("#,0"))
		Console.WriteLine("経過時間の合計 = {0}", sw.Elapsed)
		Console.WriteLine("ミリ秒単位の経過時間の合計 = {0}", sw.ElapsedMilliseconds)
		Console.WriteLine("経過時間の分解能 (1秒あたり) = {0}", Stopwatch.Frequency)
	End Sub

End Class
