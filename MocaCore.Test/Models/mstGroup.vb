Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("mstGroup")>
Partial Public Class mstGroup
    <DatabaseGenerated(DatabaseGeneratedOption.None)>
    Public Property Id As Integer

    <Required>
    <StringLength(50)>
    Public Property Name As String

    <StringLength(50)>
    Public Property Note As String

    Public Property InsertDate As Date

    Public Property UpdateDate As Date
End Class
