Imports System
Imports System.Data.Entity
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Linq

Public Class MocaTestOraDb
	Inherits DbContext

	Public Sub New()
		MyBase.New("name=MocaCore.Test.My.MySettings.Ora")
	End Sub

	Public Overridable Property mstUser As DbSet(Of mstUser)

	Protected Overrides Sub OnModelCreating(ByVal modelBuilder As DbModelBuilder)
		modelBuilder.Entity(Of mstUser)() _
			.Property(Function(e) e.Mail) _
			.IsUnicode(False)
	End Sub

End Class
