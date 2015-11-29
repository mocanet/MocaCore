
Namespace Db

	''' <summary>
	''' �e�[�u����񃂃f���̃R���N�V����
	''' </summary>
	''' <remarks></remarks>
	Public Class DbInfoTableCollection
		Inherits SortedList(Of String, DbInfoTable)

		Public ReadOnly Property Table(ByVal name As String) As DbInfoTable
			Get
				For Each key As String In Me.Keys
					If Me.Item(key).Name.Equals(name) Then
						Return Me.Item(key)
					End If
				Next
				Return Nothing
			End Get
		End Property
	End Class

End Namespace
