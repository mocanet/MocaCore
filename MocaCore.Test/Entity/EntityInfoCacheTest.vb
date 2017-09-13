Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Moca.Entity

<TestClass()>
Public Class EntityInfoCacheTest
	Inherits MocaTestBase

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
	Public Sub CacheTest()
		Dim info As EntityInfo

		info = EntityInfoCache.Store(GetType(UserRow))
		For Each key As String In info.ColumnNames
			Console.WriteLine("{0}:{1}:{2}", key, info.PropertyName(key), info.ProprtyInfo(key).Name)
		Next
	End Sub

End Class