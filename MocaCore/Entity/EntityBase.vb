
Imports System.ComponentModel
Imports System.IO
Imports System.Reflection
Imports System.Runtime.Serialization.Formatters.Binary
Imports Moca.Util

Namespace Entity

    ''' <summary>
    ''' エンティティの抽象クラス
    ''' </summary>
    ''' <remarks>
    ''' データ ソースとして使用されるオブジェクトの変更をコミットまたはロールバックする機能の共通処理を実装
    ''' </remarks>
    <Serializable()>
    Public MustInherit Class EntityBase(Of TEntity)
        Implements IEditableObject

#Region " Decalre "

        ''' <summary>オリジナルバージョン</summary>
        Private _original As TEntity

        ''' <summary>バックアップ</summary>
        Private _backup As TEntity

        ''' <summary>トランザクションフラグ</summary>
        Private _inTxn As Boolean = False

#End Region

#Region " Property "

#End Region

#Region " Implements IEditableObject "

        ''' <summary>
        ''' オブジェクトの編集を開始
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub BeginEdit() Implements System.ComponentModel.IEditableObject.BeginEdit
            If _inTxn Then
                Return
            End If
            If _original Is Nothing Then
                _original = Clone()
            End If
            If _backup Is Nothing Then
                _backup = Clone()
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
            _restore(_backup, CType(Me, Object))
            Restore(_backup)
            _backup = Nothing
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

#End Region

#Region " Method "

        ''' <summary>
        ''' 編集前のエンティティを返します。BeginEdit メソッドが実行されたあとに作成されます。
        ''' </summary>
        ''' <returns></returns>
        Public Function GetOriginal() As TEntity
            Return _original
        End Function

        ''' <summary>
        ''' 変更を破棄する時のリストア処理で自動でリストアしきれないときはオーバーライドする。
        ''' </summary>
        ''' <param name="backup"></param>
        ''' <remarks></remarks>
        Public Overridable Sub Restore(ByVal backup As TEntity)
        End Sub

        ''' <summary>
        ''' オブジェクトのバックアップ時に自動ディープコピーしきれないときはオーバーライドする。
        ''' MemberwiseClone は既に実行済みで、ICollection の実装系は新しいインスタンスを生成してあります。
        ''' </summary>
        ''' <param name="valueTo"></param>
        ''' <remarks></remarks>
        Public Overridable Sub DeepCopy(ByVal valueTo As TEntity)
            Dim fields() As FieldInfo
            fields = ClassUtil.GetFields(Me)

            For Each info As FieldInfo In fields
                If info.FieldType.IsPrimitive Then
                    Continue For
                End If
                If info.FieldType.IsValueType Then
                    Continue For
                End If

                Dim value As Object = info.GetValue(Me)
                Dim newValue As Object = Nothing

                For Each t As Type In info.FieldType.GetInterfaces()
                    If t.IsGenericType AndAlso t.GetGenericTypeDefinition() Is GetType(ICollection(Of )) Or t Is GetType(ICollection) Then
                        If value IsNot Nothing Then
                            If value.GetType.IsArray Then
                                Dim ary1 As Array = value
                                Dim ary2 As Array = ClassUtil.NewInstance(value.GetType, New Object() {ary1.Length})
                                Array.Copy(ary1, ary2, ary1.Length)
                                newValue = ary2
                            Else
                                newValue = ClassUtil.NewInstance(value.GetType, New Object() {value})
                            End If
                        End If
                    End If
                Next
                If newValue Is Nothing Then
                    Continue For
                End If

                info.SetValue(valueTo, newValue)
            Next
        End Sub

        ''' <summary>
        ''' クローン
        ''' </summary>
        ''' <returns></returns>
        Public Function Clone() As TEntity
            If Me.GetType.IsSerializable Then
                Return _copySerializable()
            End If

            Dim obj As TEntity
            obj = Me.MemberwiseClone
            DeepCopy(obj)
            Return obj
        End Function

        ''' <summary>
        ''' Serializable 属性ありのクローン
        ''' </summary>
        ''' <returns></returns>
        Private Function _copySerializable() As TEntity
            ' シリアル化した内容を保持
            Using stream As New MemoryStream()
                Dim f As New BinaryFormatter()

                ' 現在のインスタンスをシリアル化
                f.Serialize(stream, Me)

                ' ストリームの位置を先頭へ
                stream.Position = 0L

                ' 保存した内容を逆シリアル化
                Return DirectCast(f.Deserialize(stream), TEntity)
            End Using
        End Function

        ''' <summary>
        ''' 変更を破棄する時のリストア
        ''' </summary>
        Private Sub _restore(ByVal valueFrom As TEntity, ByVal valueTo As TEntity)
            Dim fields() As FieldInfo
            fields = ClassUtil.GetFields(Me)

            For Each info As FieldInfo In fields
                Dim value As Object = info.GetValue(valueFrom)

                If Not info.FieldType.IsValueType Then
                    For Each t As Type In info.FieldType.GetInterfaces()
                        If t.IsGenericType AndAlso t.GetGenericTypeDefinition() Is GetType(ICollection(Of )) Or t Is GetType(ICollection) Then
                            If value IsNot Nothing Then
                                value = ClassUtil.NewInstance(value.GetType, New Object() {value})
                            End If
                        End If
                    Next
                End If

                info.SetValue(valueTo, value)
            Next
        End Sub

#End Region

    End Class

End Namespace
