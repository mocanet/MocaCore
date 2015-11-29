
Namespace Util

	''' <summary>
	''' �X�g�b�v�E�H�b�`
	''' </summary>
	''' <remarks>
	''' ���̃N���X�́A.NET Framework version 1.1 �p�ł��B<br/>
	''' .NET Framework version 2.0 �ł� <see cref="System.Diagnostics.Stopwatch"/> ���V�����ǉ�����Ă܂��B<br/>
	''' ��{�I�ɂ͕W�����g���Ă��������B�@�\�g�����������͂�������g���ċ@�\�g������̂����肩�ƁB
	''' </remarks>
	Public Class StopWatch

		''' <summary>�J�n����</summary>
		Private _sTime As DateTime
		''' <summary>�I������</summary>
		Private _eTime As DateTime

#Region " Constructor/DeConstructor "

		''' <summary>
		''' �f�t�H���g�R���X�g���N�^
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			_sTime = Nothing
			_eTime = Nothing
		End Sub

#End Region

#Region " Properties "

		''' <summary>
		''' �J�n����
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property StartTime() As DateTime
			Get
				Return _sTime
			End Get
		End Property

		''' <summary>
		''' �I������
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property StopTime() As DateTime
			Get
				Return _eTime
			End Get
		End Property

		''' <summary>
		''' �v�������b�����Q��
		''' </summary>
		''' <value>�v�����ʕb��</value>
		''' <remarks>
		''' </remarks>
		Public ReadOnly Property ElapsedMilliseconds() As Double
			Get
				Return _eTime.Subtract(_sTime).TotalSeconds
			End Get
		End Property

#End Region

		''' <summary>
		''' �X�^�[�g
		''' </summary>
		''' <remarks>
		''' </remarks>
		Public Sub Start()
			_sTime = Now
		End Sub

		''' <summary>
		''' �X�g�b�v
		''' </summary>
		''' <remarks>
		''' </remarks>
		Public Sub [Stop]()
			_eTime = Now
		End Sub

	End Class

End Namespace
