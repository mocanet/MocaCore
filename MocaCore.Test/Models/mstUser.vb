Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Spatial

<Table("mstUser")>
<Moca.Db.Attr.TableDefinition(GetType(IMstUserTbInfo))>
Partial Public Class mstUser
    <StringLength(50)>
    Public Property Id As String

    <Required>
	<StringLength(50)>
	<Column("Name")>
	<Moca.Db.Attr.Column("Name")>
	Public Property Nm As String

	<StringLength(128)>
    Public Property Mail As String

	<StringLength(50)>
	<Column("Note")>
	<Moca.Db.Attr.Column("Note")>
	Public Property Remarks As String

	Public Property Admin As Boolean

    Public Property InsertDate As Date

    Public Property UpdateDate As Date

	<Column("NullValue")>
	<Moca.Db.Attr.Column("NullValue")>
	Public Property DBNullValue() As Object

End Class
