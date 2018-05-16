
Namespace Db.Attr

    ''' <summary>
    ''' SQL 作成時に値ではなく関数を実行したいとき指定する
    ''' </summary>
    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=False)>
    Public Class DbFunctionAttribute
        Inherits Attribute

        Private _function As String

        Public Sub New(ByVal [function] As String)
            _function = [function]
        End Sub

        Public ReadOnly Property [Function] As String
            Get
                Return _function
            End Get
        End Property

    End Class

End Namespace
