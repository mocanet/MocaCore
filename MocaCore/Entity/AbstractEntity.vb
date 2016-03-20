
Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Namespace Entity

    ''' <summary>
    ''' エンティティの抽象クラス
    ''' </summary>
    ''' <remarks>
    ''' データ ソースとして使用されるオブジェクトの変更をコミットまたはロールバックする機能の共通処理を実装
    ''' </remarks>
    <Obsolete("EntityBase クラスを使用することを勧めます。")>
    Public MustInherit Class AbstractEntity
        Implements IEditableObject

        ''' <summary>バックアップ</summary>
        Private _backup As Object

        ''' <summary>トランザクションフラグ</summary>
        Private _inTxn As Boolean = False

#Region " Implements IEditableObject "

        ''' <summary>
        ''' オブジェクトの編集を開始
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub BeginEdit() Implements System.ComponentModel.IEditableObject.BeginEdit
            If _inTxn Then
                Return
            End If
            If Me._backup Is Nothing Then
                Me._backup = Me.MemberwiseClone
                Me.DeepCopy(Me._backup)
            End If
            _inTxn = True
        End Sub

        ''' <summary>
        ''' 最後に <see cref="BeginEdit"></see> が呼び出された後に行われた変更を破棄
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub CancelEdit() Implements System.ComponentModel.IEditableObject.CancelEdit
            If Not _inTxn Then
                Return
            End If
            _inTxn = False
            Me.Restore(_backup)
        End Sub

        ''' <summary>
        ''' 対象となるオブジェクトに、最後に <see cref="BeginEdit"></see> または <see cref="IBindingList.AddNew"></see> を呼び出した後に行われた変更を適用
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub EndEdit() Implements System.ComponentModel.IEditableObject.EndEdit
            If Not _inTxn Then
                Return
            End If
            _inTxn = False
            _backup = Nothing
        End Sub

        ''' <summary>
        ''' オブジェクトのバックアップ時にディープコピーするときはオーバーライドする
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks></remarks>
        Public Overridable Sub DeepCopy(ByVal value As Object)

        End Sub

        ''' <summary>
        ''' 変更を破棄する時のリストア処理を実装する
        ''' </summary>
        ''' <param name="backup"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub Restore(ByVal backup As Object)

#End Region

    End Class

End Namespace
