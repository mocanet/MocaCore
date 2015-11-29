
Namespace Db.CommandWrapper

	''' <summary>
	''' SELECT�������s���ADataSet���g����UPDATE����ׂ�DBCommand�̃��b�p�[�N���X
	''' </summary>
	''' <remarks></remarks>
	Public Class Select4UpdateCommandWrapper
		Inherits SelectCommandWrapper
		Implements IDbCommandSelect4Update

		''' <summary>�A�_�v�^�I�u�W�F�N�g</summary>
		Private _adp As IDbDataAdapter

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
			Me._adp = dba.Dbms.ProviderFactory.CreateDataAdapter()
		End Sub

#End Region

#Region " Property "

		''' <summary>
		''' �A�_�v�^�C���X�^���X
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Adapter() As IDbDataAdapter Implements IDbCommandSelect4Update.Adapter
			Get
				Return _adp
			End Get
		End Property

#End Region

		''' <summary>
		''' Select SQL���s�I
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overrides Function Execute() As Integer
			Return dba.Execute(Me)
		End Function

		''' <summary>
		''' Adapter Update ���s�I
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Update() As Integer Implements IDbCommandSelect4Update.Update
			Return dba.UpdateAdapter(Me.ResultDataSet, Me.Adapter)
		End Function

	End Class

End Namespace
