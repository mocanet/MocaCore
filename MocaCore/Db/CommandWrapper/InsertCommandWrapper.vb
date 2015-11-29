
Namespace Db.CommandWrapper

	''' <summary>
	''' INSERT�������s����ׂ�DBCommand�̃��b�p�[�N���X
	''' </summary>
	''' <remarks></remarks>
	Public Class InsertCommandWrapper
		Inherits SqlCommandWrapper
		Implements IDbCommandInsert

#Region " Constructor/DeConstructor "

		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="dba">�e�ƂȂ�DBAccess�C���X�^���X</param>
		''' <param name="cmd">���s����DBCommand�C���X�^���X</param>
		''' <remarks>
		''' </remarks>
		Friend Sub New(ByVal dba As IDao, ByVal cmd As IDbCommand)
			MyBase.New(dba, cmd)
		End Sub

#End Region

		''' <summary>
		''' SQL���s�I
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overrides Function Execute() As Integer
			Return dba.ExecuteNonQuery(Me)
		End Function

	End Class

End Namespace
