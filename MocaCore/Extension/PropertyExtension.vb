
Imports System.Reflection
Imports System.Runtime.CompilerServices

Module PropertyExtension

	<Extension()>
	Public Function ToAccessor(ByVal pi As PropertyInfo) As IAccessor
		Dim getterDelegateType As Type = GetType(Func(Of ,)).MakeGenericType(pi.DeclaringType, pi.PropertyType)
		Dim getter As [Delegate] = [Delegate].CreateDelegate(getterDelegateType, pi.GetGetMethod())

		Dim setterDelegateType As Type = GetType(Action(Of ,)).MakeGenericType(pi.DeclaringType, pi.PropertyType)
		Dim setter As [Delegate] = [Delegate].CreateDelegate(setterDelegateType, pi.GetSetMethod())

		Dim accessorType As Type = GetType(Accessor(Of ,)).MakeGenericType(pi.DeclaringType, pi.PropertyType)
		Dim accessor As IAccessor = Activator.CreateInstance(accessorType, getter, setter)

		Return accessor
	End Function

End Module
