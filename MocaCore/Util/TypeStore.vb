
Imports System.Reflection
Imports System.Reflection.Emit
Imports System.Threading

Namespace Util

    Public Interface ITypeStore

        Function NewList() As IList

    End Interface

    ''' <summary>
    ''' タイプを生成
    ''' </summary>
    Public Class TypeStore
        Implements IDisposable, IEnumerable

#Region " Declare "

        ''' <summary>シングルトン用コンテナインスタンス</summary>
        Private Shared _instance As TypeStore

        Private _types As Dictionary(Of String, Type)

#If net20 Then
		''' <summary>ロック用</summary>
		Private _rwLock As New ReaderWriterLock()
#Else
        ''' <summary>ロック用</summary>
        Private _rwLock As New ReaderWriterLockSlim()
#End If

#End Region

#Region " コンストラクタ "

        Private Sub New()
            _types = New Dictionary(Of String, Type)()
        End Sub

#End Region
#Region "IDisposable Support"
        Private disposedValue As Boolean ' 重複する呼び出しを検出するには

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
                End If

                ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
                ' TODO: 大きなフィールドを null に設定します。
            End If
            disposedValue = True
        End Sub

        ' TODO: 上の Dispose(disposing As Boolean) にアンマネージ リソースを解放するコードが含まれる場合にのみ Finalize() をオーバーライドします。
        'Protected Overrides Sub Finalize()
        '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(disposing As Boolean) に記述します。
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
        Public Sub Dispose() Implements IDisposable.Dispose
            ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(disposing As Boolean) に記述します。
            Dispose(True)
            ' TODO: 上の Finalize() がオーバーライドされている場合は、次の行のコメントを解除してください。
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region

#Region " Property "

        Public Shared ReadOnly Property Store As TypeStore
            Get
                If _instance Is Nothing Then
                    _instance = New TypeStore()
                End If
                Return _instance
            End Get
        End Property

        Default Public ReadOnly Property Item(ByVal key As Type) As Type
            Get
                Dim className As String = _newClassName(key)

                If _types.ContainsKey(className) Then
                    Return _types(className)
                End If

                Return Nothing
            End Get
        End Property

        Public Function ContainsKey(key As Type) As Boolean
            Dim className As String = _newClassName(key)
            Return _types.ContainsKey(className)
        End Function

        Public Function Create(ByVal key As Type, ByVal propertyNames As KeyValuePair(Of String, Type)()) As Type
            Dim className As String = _newClassName(key)

            Try
                ' リーダーロックを取得
#If net20 Then
					_rwLock.AcquireReaderLock(Timeout.Infinite)
#Else
                _rwLock.EnterReadLock()
#End If

                If _types.ContainsKey(className) Then
                    Return _types(className)
                End If
            Finally
                ' リーダーロックを解放
#If net20 Then
					_rwLock.ReleaseReaderLock()
#Else
                _rwLock.ExitReadLock()
#End If
            End Try

            Return create(key, className, propertyNames)
        End Function

#End Region

#Region " Implements IEnumerable "

        Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Return _types
        End Function

#End Region

#Region " Method "

        Public Function NewInstance(Of T)(ByVal propertyNames As KeyValuePair(Of String, Type)()) As T
            Dim className As String = _newClassName(GetType(T))
            Return newInstance(Of T)(className, propertyNames)
        End Function

        Protected Function newInstance(Of T)(ByVal className As String, ByVal propertyNames As KeyValuePair(Of String, Type)()) As T
            Dim typ As Type

            If _types.ContainsKey(className) Then
                typ = _types(className)
            Else
                typ = create(GetType(T), className, propertyNames)
                _types(className) = typ
            End If

            Return ClassUtil.NewInstance(typ)
        End Function

        Protected Function create(baseType As Type, className As String, propertyNames As KeyValuePair(Of String, Type)()) As Type
            Try
                ' ライターロックを取得
#If net20 Then
				_rwLock.AcquireWriterLock(Timeout.Infinite)
#Else
                _rwLock.EnterWriteLock()
#End If

                Dim propAttr As MethodAttributes = MethodAttributes.[Public] Or MethodAttributes.SpecialName
                Dim asmblyName As New AssemblyName(String.Format("{0}.dll", className))
                Dim aBuilder As AssemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmblyName, AssemblyBuilderAccess.RunAndSave)
                Dim mBuilder As ModuleBuilder = aBuilder.DefineDynamicModule(asmblyName.Name)
                Dim typeName As String = Assembly.CreateQualifiedName(baseType.AssemblyQualifiedName, className)
                Dim tBuilder As TypeBuilder = mBuilder.DefineType(className, TypeAttributes.[Public] Or TypeAttributes.[Class], baseType)
                Dim onPropertyChangedMethod As MethodInfo = baseType.GetMethod("OnPropertyChanged", BindingFlags.Instance Or BindingFlags.NonPublic, Nothing, New Type() {GetType(String)}, Nothing)

                For Each kv As KeyValuePair(Of String, Type) In propertyNames
                    Dim name As String = kv.Key
                    Dim fBuilder As FieldBuilder = tBuilder.DefineField("_" + name, kv.Value, FieldAttributes.[Private])
                    'Dim pBuilder As PropertyBuilder = tBuilder.DefineProperty(name, PropertyAttributes.HasDefault, CallingConventions.HasThis, kv.Value, Type.EmptyTypes)
                    Dim pBuilder As PropertyBuilder = tBuilder.DefineProperty(name, PropertyAttributes.HasDefault, kv.Value, Type.EmptyTypes)

                    Dim getMethod As MethodBuilder = tBuilder.DefineMethod("get_" + pBuilder.Name, propAttr, kv.Value, Type.EmptyTypes)
                    Dim getIL As ILGenerator = getMethod.GetILGenerator()
                    Dim lbl As Label = getIL.DefineLabel()
                    getIL.DeclareLocal(kv.Value)
                    getIL.Emit(OpCodes.Nop)
                    getIL.Emit(OpCodes.Ldarg_0)
                    getIL.Emit(OpCodes.Ldfld, fBuilder)
                    getIL.Emit(OpCodes.Stloc_0)
                    getIL.Emit(OpCodes.Br_S, lbl)
                    getIL.MarkLabel(lbl)
                    getIL.Emit(OpCodes.Ldloc_0)
                    getIL.Emit(OpCodes.Ret)

                    Dim setMethod As MethodBuilder = tBuilder.DefineMethod("set_" + pBuilder.Name, propAttr, Nothing, New Type() {kv.Value})
                    setMethod.DefineParameter(1, ParameterAttributes.None, "Value")
                    Dim setIL As ILGenerator = setMethod.GetILGenerator()
                    setIL.Emit(OpCodes.Nop)
                    setIL.Emit(OpCodes.Ldarg_0)
                    setIL.Emit(OpCodes.Ldarg_1)
                    setIL.Emit(OpCodes.Stfld, fBuilder)
                    setIL.Emit(OpCodes.Ldarg_0)
                    setIL.Emit(OpCodes.Ldstr, name)
                    setIL.Emit(OpCodes.Callvirt, onPropertyChangedMethod)
                    setIL.Emit(OpCodes.Nop)
                    setIL.Emit(OpCodes.Ret)

                    pBuilder.SetGetMethod(getMethod)
                    pBuilder.SetSetMethod(setMethod)
                Next

                tBuilder.AddInterfaceImplementation(GetType(ITypeStore))
                Dim newListMethod As MethodBuilder = tBuilder.DefineMethod("NewList", MethodAttributes.[Public] Or MethodAttributes.Virtual Or MethodAttributes.NewSlot Or MethodAttributes.Final, GetType(IList), Type.EmptyTypes)
                Dim newListIL As ILGenerator = newListMethod.GetILGenerator()

                Dim lstType As Type = GetType(List(Of))
                Dim lstOfType As Type = lstType.MakeGenericType(tBuilder.UnderlyingSystemType)
                Dim ctor As ConstructorInfo = lstType.GetConstructor(New Type() {GetType(Integer)})
                Dim ctorList As ConstructorInfo = TypeBuilder.GetConstructor(lstOfType, ctor)

                Dim newListILLabel As Label = newListIL.DefineLabel()
                newListIL.DeclareLocal(GetType(IList))
                newListIL.Emit(OpCodes.Nop)
                newListIL.Emit(OpCodes.Ldc_I4, 256)
                newListIL.Emit(OpCodes.Newobj, ctorList)
                newListIL.Emit(OpCodes.Stloc_0)
                newListIL.Emit(OpCodes.Br_S, newListILLabel)
                newListIL.MarkLabel(newListILLabel)
                newListIL.Emit(OpCodes.Ldloc_0)
                newListIL.Emit(OpCodes.Ret)


                Dim typ As Type = tBuilder.CreateType()
                aBuilder.Save(asmblyName.Name)
                _types.Add(className, typ)
                Return typ
            Finally
                ' ライターロックを解放
#If net20 Then
				_rwLock.ReleaseWriterLock()
#Else
                _rwLock.ExitWriteLock()
#End If
            End Try
        End Function

        Private Function _newClassName(ByVal typ As Type) As String
            Dim className As String = typ.Name & "_"
            Return className
        End Function

#End Region

    End Class

End Namespace
