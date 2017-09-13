
Imports System.Reflection
Imports Moca.Aop
Imports Moca.Db.Interceptor
Imports Moca.Di
Imports Moca.Util

Namespace Db.Attr

    ''' <summary>
    ''' 
    ''' </summary>
    <AttributeUsage(AttributeTargets.Class, Inherited:=True)>
    Public Class TableDefinitionAttribute
        Inherits Attribute

        ''' <summary>テーブル定義インタフェース</summary>
        Private _tableDef As Type

#Region " コンストラクタ "

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="tableDefinition">テーブル定義インタフェース</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal tableDefinition As Type)
            _tableDef = tableDefinition
        End Sub

#End Region
#Region " Property "

        ''' <summary>
        ''' テーブル定義インタフェースプロパティ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property TableDefinition() As Type
            Get
                Return _tableDef
            End Get
        End Property

#End Region
#Region " Method "

#End Region

    End Class

End Namespace
