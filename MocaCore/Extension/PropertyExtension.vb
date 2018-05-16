
Imports System.Reflection
Imports System.Runtime.CompilerServices

Module PropertyExtension

    <Extension()>
    Public Function ToAccessor(ByVal pi As PropertyInfo) As IAccessor
        Dim getterDelegateType As Type = Nothing
        Dim getter As [Delegate] = Nothing

        Dim setterDelegateType As Type = Nothing
        Dim setter As [Delegate] = Nothing

        If pi.CanRead Then
            getterDelegateType = GetType(Func(Of ,)).MakeGenericType(pi.DeclaringType, pi.PropertyType)
            getter = [Delegate].CreateDelegate(getterDelegateType, pi.GetGetMethod())
        End If
        If pi.CanWrite Then
            setterDelegateType = GetType(Action(Of ,)).MakeGenericType(pi.DeclaringType, pi.PropertyType)
            setter = [Delegate].CreateDelegate(setterDelegateType, pi.GetSetMethod())
        End If

        Dim accessorType As Type = GetType(Accessor(Of ,)).MakeGenericType(pi.DeclaringType, pi.PropertyType)
        Dim accessor As IAccessor = Activator.CreateInstance(accessorType, getter, setter)

        Return accessor
    End Function

End Module
