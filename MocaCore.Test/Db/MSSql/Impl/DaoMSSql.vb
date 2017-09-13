
Imports Moca.Db

Namespace Db.Impl

	''' <summary>
	''' DaoMSSql データアクセス
	''' </summary>
	''' <remarks></remarks>
	Public Class DaoMSSql
		Inherits AbstractDao
		Implements IDaoMSSql

#Region " Select "

		Public Function Find() As IList(Of UserRow) Implements IDaoMSSql.Find
			Const sql As String = "SELECT * FROM mstUser"

			Using cmd As IDbCommandSelect = CreateCommandSelect(sql)
				'cmd.SetParameter("param name", Nothing)

				Return cmd.Execute(Of UserRow)()
			End Using
		End Function

		Public Function Find2(row As UserRow) As IList(Of UserRow) Implements IDaoMSSql.Find2
			Const sql As String = "SELECT * FROM mstUser WHERE Id = @Id"
			Return [Select](Of UserRow)(sql,
										New With {
											.Id = row.Id
										})
		End Function

		Public Function Find3(row As UserRow) As String Implements IDaoMSSql.Find3
			Const sql As String = "SELECT Name FROM mstUser WHERE Id = @Id"
			Return Scalar(sql,
							New With {
								.Id = row.Id
							})
		End Function

#End Region
#Region " Insert "

		Public Function Ins(row As UserRow) As Integer Implements IDaoMSSql.Ins
			Const sql As String = "INSERT INTO mstUser (Id,Name,InsertDate, UpdateDate) VALUES (@Id, @Name, GETDATE(), GETDATE())"

			Using cmd As IDbCommandInsert = CreateCommandInsert(sql)
				cmd.SetParameter("Name", row.Name)
				cmd.SetParameter("Id", row.Id)

				Dim rc As Integer
				rc = cmd.Execute()
				Return rc
			End Using
		End Function

		Public Function Ins2(row As UserRow) As Integer Implements IDaoMSSql.Ins2
			Const sql As String = "INSERT INTO mstUser (Id,Name,InsertDate, UpdateDate) VALUES (@Id, @Name, GETDATE(), GETDATE())"

			Return Insert(sql,
						  New With {
							  .Id = row.Id,
							  .Name = row.Name
						  })
		End Function

		Public Function Ins2(rows As IList(Of UserRow)) As Integer Implements IDaoMSSql.Ins2
			Const sql As String = "INSERT INTO mstUser (Id,Name,InsertDate, UpdateDate) VALUES (@Id, @Name, GETDATE(), GETDATE())"

			Return Insert(sql, rows.Select(
						  Function(usr)
							  Return New With {
									  .Id = usr.Id,
									  .Name = usr.Name
								  }
						  End Function
						  ).ToList)
		End Function

		Public Function Ins3(row As UserRow) As Integer Implements IDaoMSSql.Ins3
			Throw New NotImplementedException()
			'TODO:
			'Return Insert(row)
		End Function

#End Region
#Region " Update "

		Public Function Upd(row As UserRow) As Integer Implements IDaoMSSql.Upd
			Const sql As String = "UPDATE mstUser SET Name = @Name, Mail = @Mail, Note = @Note, Admin = @Admin, InsertDate = @InsertDate, UpdateDate = @UpdateDate WHERE Id = @Id"

			Using cmd As IDbCommandDelete = CreateCommandDelete(sql)
				cmd.SetParameter("Id", row.Id)
				cmd.SetParameter("Name", row.Name)
				cmd.SetParameter("Mail", row.Mail)
				cmd.SetParameter("Note", row.Note)
				cmd.SetParameter("Admin", row.Admin)
				cmd.SetParameter("InsertDate", row.InsertDate)
				cmd.SetParameter("UpdateDate", row.UpdateDate)

				Dim rc As Integer
				rc = cmd.Execute()
				Return rc
			End Using
		End Function

		Public Function Upd2(row As UserRow) As Integer Implements IDaoMSSql.Upd2
			Const sql As String = "UPDATE mstUser SET Name = @Name, Mail = @Mail, Note = @Note, Admin = @Admin, InsertDate = @InsertDate, UpdateDate = @UpdateDate WHERE Id = @Id"

			Return Update(sql,
						  New With {
								.Id = row.Id,
								.Name = row.Name,
								.Mail = row.Mail,
								.Note = row.Note,
								.Admin = row.Admin,
								.InsertDate = row.InsertDate,
								.UpdateDate = row.UpdateDate
						  })
		End Function

		Public Function Upd2(rows As IList(Of UserRow)) As Integer Implements IDaoMSSql.Upd2
			Const sql As String = "UPDATE mstUser SET Name = @Name, Mail = @Mail, Note = @Note, Admin = @Admin, InsertDate = @InsertDate, UpdateDate = @UpdateDate WHERE Id = @Id"

			Return Update(sql, rows.Select(
						  Function(usr)
							  Return New With {
									  .Id = usr.Id,
									  .Name = usr.Name,
									  .Mail = usr.Mail,
									  .Note = usr.Note,
									  .Admin = usr.Admin,
									  .InsertDate = usr.InsertDate,
									  .UpdateDate = usr.UpdateDate
								  }
						  End Function
						  ).ToList)
		End Function

		Public Function Upd3(row As UserRow) As Integer Implements IDaoMSSql.Upd3
			Throw New NotImplementedException()
		End Function

#End Region
#Region " Delete "

		Public Function Del(row As UserRow) As Integer Implements IDaoMSSql.Del
			Const sql As String = "DELETE FROM mstUser WHERE Id = @Id"

			Using cmd As IDbCommandDelete = CreateCommandDelete(sql)
				cmd.SetParameter("Id", row.Id)

				Dim rc As Integer
				rc = cmd.Execute()
				Return rc
			End Using
		End Function

		Public Function Del2(row As UserRow) As Integer Implements IDaoMSSql.Del2
			Const sql As String = "DELETE FROM mstUser WHERE Id = @Id"

			Return Delete(sql,
						  New With {
							  .Id = row.Id
						  })
		End Function

		Public Function Del2(rows As IList(Of UserRow)) As Integer Implements IDaoMSSql.Del2
			Const sql As String = "DELETE FROM mstUser WHERE Id = @Id"

			Return Delete(sql, rows.Select(
						  Function(usr)
							  Return New With {
									  .Id = usr.Id
								  }
						  End Function
						  ).ToList)
		End Function

		Public Function Del3(row As UserRow) As Integer Implements IDaoMSSql.Del3
			Throw New NotImplementedException()
		End Function

#End Region
#Region " Procedure "

		Public Function Procedure1() As IList(Of UserRow) Implements IDaoMSSql.Procedure1
			Return QueryProcedure(Of UserRow)("DaoMSSql_Procedure1", Nothing)
		End Function

		Public Function Procedure2(row As UserRow) As String Implements IDaoMSSql.Procedure2
			Return ScalarProcedure("DaoMSSql_Procedure2", row)
		End Function

		Public Function Procedure3(row As UserRow) As Integer Implements IDaoMSSql.Procedure3
			Return UpdateProcedure("DaoMSSql_Procedure3", row)
		End Function

#End Region

	End Class

End Namespace
