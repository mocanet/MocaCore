Imports System
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Linq

Partial Public Class MocaTestDb
	Inherits DbContext

	Public Sub New()
		MyBase.New("name=MocaTestDB")
	End Sub

	Public Overridable Property mstGroup As DbSet(Of mstGroup)
	Public Overridable Property mstUser As DbSet(Of mstUser)
	Public Overridable Property mstUserGroupMap As DbSet(Of mstUserGroupMap)

	Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
		modelBuilder.Entity(Of mstUser)() _
			.Property(Function(e) e.Id) _
			.IsUnicode(False)

		modelBuilder.Entity(Of mstUser)() _
			.Property(Function(e) e.Mail) _
			.IsUnicode(False)

		modelBuilder.Entity(Of mstUserGroupMap)() _
			.Property(Function(e) e.UserId) _
			.IsUnicode(False)
	End Sub
End Class
