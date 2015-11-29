Imports System.Configuration
Imports System.Data.Common
Imports System.Reflection
Imports Moca.Db.CommandWrapper
Imports Moca.Util

Namespace Db

	''' <summary>
	''' �R�}���h��ʂ̗񋓌^
	''' </summary>
	Public Enum SQLCommandTypes
		''' <summary>SELECT�������s����</summary>
		SelectText = 0
		''' <summary>SELECT�����s���DataSet�ɂ��UPDATE���s����</summary>
		Select4Update
		''' <summary>UPDATE�������s����</summary>
		UpdateText
		''' <summary>INSERT�������s����</summary>
		InsertText
		''' <summary>DELETE�������s����</summary>
		DeleteText
		''' <summary>�X�g�A�h���s�����s����</summary>
		StoredProcedure
		''' <summary>DDL���s�����s����</summary>
		DDL
	End Enum

End Namespace
