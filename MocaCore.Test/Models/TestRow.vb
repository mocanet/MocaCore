'------------------------------------------------------------------------------
' <auto-generated>
'     このコードはツールによって生成されました。
'     ランタイム バージョン:4.0.30319.42000
'
'     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
'     コードが再生成されるときに損失したりします。
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports Moca.Db
Imports Moca.Db.Attr
Imports System.ComponentModel


''' <summary> 
''' TestRow Entity 
''' </summary> 
''' <remarks></remarks> 
''' <history> 
''' </history> 
Public Class TestRow
    Implements System.ComponentModel.INotifyPropertyChanged
    
    #Region " Declare "
    Shared _def As ITestRowDefinition
    
    Private _id As Integer
    
    Private _name As String
    
    Private _note As String
    #End Region
    
    #Region " Property "
    ''' <summary> 
    ''' Id (Id) Property. 
    ''' </summary> 
    <Column("Id")>  _
    Public Property Id() As Integer
        Get
            Return Me._id
        End Get
        Set
            Me._id = value
            OnPropertyChanged("Id")
        End Set
    End Property
    
    ''' <summary> 
    ''' Name (Name) Property. 
    ''' </summary> 
    <Column("Name")>  _
    Public Property Name() As String
        Get
            Return Me._name
        End Get
        Set
            Me._name = value
            OnPropertyChanged("Name")
        End Set
    End Property
    
    ''' <summary> 
    ''' Note (Note) Property. 
    ''' </summary> 
    <Column("Note")>  _
    Public Property Note() As String
        Get
            Return Me._note
        End Get
        Set
            Me._note = value
            OnPropertyChanged("Note")
        End Set
    End Property
    #End Region
    
    #Region " PropertyChanged "
    Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    
    Protected Overridable Sub OnPropertyChanged(ByVal name As String)
        RaiseEvent PropertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(name))
    End Sub
    #End Region
End Class

#Region " Definition "
''' <summary> 
''' TestRow Entity definition 
''' </summary> 
''' <remarks></remarks> 
''' <history> 
''' </history> 
<Table("app.configのconnectionStringsキーを入れる", "trnTable")>  _
Public Interface ITestRowDefinition
    
    ''' <summary> 
    ''' Table (Table) Property. 
    ''' </summary> 
    Property Table() As Moca.Db.DbInfoTable
    
    ''' <summary> 
    ''' Id (Id) Property. 
    ''' </summary> 
    <Column("Id")>  _
    Property Id() As Moca.Db.DbInfoColumn
    
    ''' <summary> 
    ''' Name (Name) Property. 
    ''' </summary> 
    <Column("Name")>  _
    Property Name() As Moca.Db.DbInfoColumn
    
    ''' <summary> 
    ''' Note (Note) Property. 
    ''' </summary> 
    <Column("Note")>  _
    Property Note() As Moca.Db.DbInfoColumn
End Interface
#End Region