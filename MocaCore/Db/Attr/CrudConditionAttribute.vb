
Namespace Db.Attr

    ''' <summary>
    ''' 列に対してCURDの可能なタイミングを指定する
    ''' </summary>
    <AttributeUsage(AttributeTargets.Property, AllowMultiple:=True)>
    Public Class CrudConditionAttribute
        Inherits Attribute

#Region " コンストラクタ "

        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        Public Sub New(ByVal status As DataRowState)
            _Status = status
        End Sub

#End Region

#Region " Property "

        ''' <summary>
        ''' オブジェクトの状態
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property Status As DataRowState

#End Region

    End Class

End Namespace
