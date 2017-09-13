
Public Class Accessor(Of T, TProp)
	Implements IAccessor

	Private ReadOnly _getter As Func(Of T, TProp)
	Private ReadOnly _setter As Action(Of T, TProp)

	Public Sub New(ByVal getter As Func(Of T, TProp), ByVal setter As Action(Of T, TProp))
		_getter = getter
		_setter = setter
	End Sub

	Public Sub SetValue(target As Object, value As Object) Implements IAccessor.SetValue
		_setter(target, value)
	End Sub

	Public Function GetValue(target As Object) As Object Implements IAccessor.GetValue
		Return _getter(target)
	End Function

End Class
