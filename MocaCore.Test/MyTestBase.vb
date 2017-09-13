

<DeploymentItem(".\MocaCoreTest.xlsx")>
Public MustInherit Class MyTestBase
	Inherits MocaTestBase

	Protected msdb As New MocaTestDb

	Protected Shared Sub MyTestBaseClassInitialize(ByVal testContext As TestContext)
		MocaInitialize(testContext)

		'Entity.Database.SetInitializer(New Entity.MigrateDatabaseToLatestVersion(Of MocaTestDb, Migrations.MocaTestDbConfiguration))
		'Entity.Database.SetInitializer(New Entity.MigrateDatabaseToLatestVersion(Of MocaTestOraDb, Migrations.MocaTestOraDbConfiguration)

		' 下記の例外が発生するときはおまじないが必要らしい
		' System.InvalidOperationException: 不変名が 'System.Data.SqlClient' の ADO.NET プロバイダーのアプリケーション構成ファイルに登録された Entity Framework プロバイダー型 'System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer' を読み込めませんでした。assembly-qualified 名が使用されていること、およびアセンブリを実行中のアプリケーションで使用できることを確認してください。詳細については、http://go.microsoft.com/fwlink/?LinkId=260882 を参照してください。
		' EntityFramework.SqlServer.dllがテスト時に配置されない事がエラーの原因らしい
		Dim instance = System.Data.Entity.SqlServer.SqlProviderServices.Instance
	End Sub

	Protected Sub MyTestBaseInitialize()
		msdb.mstUser.RemoveRange(msdb.mstUser)
		msdb.SaveChanges()

		Dim dt As DataTable
		dt = GetExcelData(".\MocaCoreTest.xlsx", "Init")
		For Each row As DataRow In dt.Rows
			msdb.mstUser.Add(
				New mstUser() With {
				.Id = row("Id"),
				.Nm = row("Name"),
				.Mail = IIf(IsDBNull(row("Mail")), Nothing, row("Mail")),
				.Remarks = IIf(IsDBNull(row("Note")), Nothing, row("Note")),
				.Admin = row("Admin"),
				.InsertDate = row("InsertDate"),
				.UpdateDate = row("UpdateDate")
				})
		Next
		msdb.SaveChanges()
	End Sub

	Protected Sub MyTestBaseCleanup()
	End Sub

End Class
