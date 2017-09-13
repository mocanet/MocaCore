Imports System.IO
Imports Moca.Di

Public MustInherit Class MocaTestBase

#Region " Declare "

	Private testContextInstance As TestContext

	Protected Shared injector As New MocaInjector()

	Protected outputFileCount As Integer

#End Region

#Region "追加のテスト属性"

	'''<summary>
	'''現在のテストの実行についての情報および機能を
	'''提供するテスト コンテキストを取得または設定します。
	'''</summary>
	Public Property TestContext() As TestContext
		Get
			Return testContextInstance
		End Get
		Set(ByVal value As TestContext)
			testContextInstance = value
		End Set
	End Property

	Public Shared Sub MocaInitialize(ByVal testContext As TestContext)
		Moca.Di.MocaContainerFactory.Init()
	End Sub

	Public Shared Sub MocaCleanup()
		injector.Dispose()
		Moca.Di.MocaContainerFactory.Destroy()
	End Sub

#End Region

#Region " Property "

	''' <summary>
	''' 現在のテスト結果出力先フォルダパス
	''' </summary>
	''' <returns></returns>
	Protected ReadOnly Property CurrentTestResultsDirectory As String
		Get
			Dim rc As String
			rc = Path.Combine(Me.TestContext.ResultsDirectory, Me.TestContext.FullyQualifiedTestClassName.Replace(".", "\"))
			Return Path.Combine(rc, Me.TestContext.TestName)
		End Get
	End Property

	Protected ReadOnly Property SavePath As String
		Get
			Dim path As String = CurrentTestResultsDirectory
			If Not Directory.Exists(path) Then
				Directory.CreateDirectory(path)
			End If
			Return path
		End Get
	End Property

#End Region

#Region " Methods "

	''' <summary>
	''' 
	''' </summary>
	''' <param name="fileName">Excel ファイル名</param>
	''' <param name="sheetName">シート名</param>
	''' <returns></returns>
	Protected Function GetExcelData(ByVal fileName As String, ByVal sheetName As String) As DataTable
		Dim connectionString As String
		Dim sql As String
		Dim ds As New DataSet

		connectionString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0; Data Source={0}; Extended Properties='Excel 12.0;HDR=yes';", fileName)
		sql = String.Format("SELECT * FROM [{0}$]", sheetName)
		Using adapter As OleDb.OleDbDataAdapter = New OleDb.OleDbDataAdapter(sql, connectionString)
			adapter.Fill(ds)
		End Using
		Return ds.Tables(0)
	End Function

	''' <summary>
	''' 連番を使ったファイル名を取得
	''' </summary>
	''' <param name="name"></param>
	''' <param name="extension"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Protected Function GetOutputFilename(ByVal name As String, ByVal extension As String) As String
		Dim filename As String = String.Format("{0}_{1}.{2}", name, outputFileCount.ToString("00"), extension)
		outputFileCount += 1
		Return filename
	End Function

	''' <summary>
	''' エビデンスファイル保存
	''' </summary>
	''' <param name="folder"></param>
	''' <param name="filename"></param>
	''' <param name="saveFilename"></param>
	''' <remarks></remarks>
	Protected Sub SaveFile(ByVal folder As Environment.SpecialFolder, ByVal fileName As String, ByVal saveFileName As String)
		Dim file As FileInfo = New FileInfo(Path.Combine(System.Environment.GetFolderPath(folder), fileName))
		SaveFile(file, saveFileName)
	End Sub

	''' <summary>
	''' エビデンスファイル保存
	''' </summary>
	''' <param name="orgFile"></param>
	''' <param name="saveFilename"></param>
	Protected Sub SaveFile(ByVal orgFile As FileInfo, ByVal saveFilename As String)
		Dim savePath As String = Me.SavePath
		Dim fullPath As String = Path.Combine(savePath, saveFilename)

		File.Move(orgFile.FullName, fullPath)

		Me.TestContext.AddResultFile(fullPath)
	End Sub

#End Region

End Class
