Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("mstUserGroupMap")>
Partial Public Class mstUserGroupMap
    <Key>
    <Column(Order:=0)>
    <StringLength(50)>
    Public Property UserId As String

    <Key>
    <Column(Order:=1)>
    <DatabaseGenerated(DatabaseGeneratedOption.None)>
    Public Property GroupId As Integer
End Class
