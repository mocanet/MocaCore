Imports System.Data.Common

Namespace Db.Helper

	''' <summary>
	''' DB�A�N�Z�X�̊e�v���p�C�_�[�ɑΉ������w���p�[�̒��ۃN���X
	''' </summary>
	''' <remarks>
	''' �eDB�x���_�[���ɈقȂ镔�����z������ׂ̃N���X�ł��B<br/>
	''' </remarks>
	Public MustInherit Class DbAccessHelper
		Implements IDisposable

		''' <summary>���ƂȂ�f�[�^�x�[�X�A�N�Z�X�N���X�C���X�^���X</summary>
		Protected targetDba As IDao
		''' <summary>���N���X�Ŏg�p����f�[�^�x�[�X�A�N�Z�X�N���X�C���X�^���X</summary>
		Protected myDba As IDao

		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="dba">�g�p����f�[�^�x�[�X�A�N�Z�X</param>
		''' <remarks></remarks>
		Public Sub New(ByVal dba As IDao)
			Me.targetDba = dba
			Me.myDba = New DbAccess(dba.Dbms)
		End Sub

#Region " IDisposable Support "

		Private disposedValue As Boolean = False		' �d������Ăяo�������o����ɂ�

		' IDisposable
		Protected Overridable Sub Dispose(ByVal disposing As Boolean)
			If Not Me.disposedValue Then
				If disposing Then
					' TODO: �����I�ɌĂяo���ꂽ�Ƃ��Ƀ}�l�[�W ���\�[�X��������܂�
				End If

				' TODO: ���L�̃A���}�l�[�W ���\�[�X��������܂�
				If Me.myDba IsNot Nothing Then
					Me.myDba.Dispose()
				End If
			End If
			Me.disposedValue = True
		End Sub

		' ���̃R�[�h�́A�j���\�ȃp�^�[���𐳂��������ł���悤�� Visual Basic �ɂ���Ēǉ�����܂����B
		Public Sub Dispose() Implements IDisposable.Dispose
			' ���̃R�[�h��ύX���Ȃ��ł��������B�N���[���A�b�v �R�[�h����� Dispose(ByVal disposing As Boolean) �ɋL�q���܂��B
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
#End Region

	End Class

End Namespace
