Imports System
Imports System.Configuration

Namespace Security

	''' <summary>
	''' Dpapi�𗘗p����app.config���Í�������ׂ̃N���X
	''' </summary>
	''' <remarks>
	''' ���L�̈Í���/���������T�|�[�g���Ă��܂��B
	''' <list>
	''' <item>ConnectionStrings</item>
	''' </list>
	''' </remarks>
	Public Class DPAPIConfiguration

		''' <summary>�g�p����v���p�C�_�[</summary>
		Protected Const C_PROVIDER As String = "DataProtectionConfigurationProvider"

		''' <summary>�g�p����app.config�t�@�C��</summary>
		Protected config As System.Configuration.Configuration

		''' <summary>log4net logger</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

#Region " Constructor/DeConstructor "

		''' <summary>
		''' �f�t�H���g�R���X�g���N�^
		''' </summary>
		''' <remarks>
		''' �N�����A�v���P�[�V�����̍\���t�@�C���ɑ΂��ď������܂��B
		''' </remarks>
		Public Sub New()
			config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
		End Sub

		''' <summary>
		''' �R���X�g���N�^
		''' </summary>
		''' <param name="config">�g�p����app.config�t�@�C��</param>
		''' <remarks>app.config�t�@�C�����w�肷�鎞�Ɏg�p����B</remarks>
		Public Sub New(ByVal config As System.Configuration.Configuration)
			Me.config = config
		End Sub

#End Region

#Region " DataProtectionConfigurationProvider "

		''' <summary>
		''' �ڑ���������Í������܂��B
		''' </summary>
		''' <remarks>
		''' �A�v���P�[�V�����\���t�@�C���̐ڑ�������Z�N�V�����ɑ΂��ĈÍ������s���܂��B<br/>
		''' �������A���L�̏ꍇ�͈Í����͍s���܂���B<br/>
		''' �E�ڑ������񂪖�����<br/>
		''' �E���ɈÍ�������Ă��鎞<br/>
		''' �E���b�N����Ă��鎞<br/>
		''' </remarks>
		Public Sub ProtectConnectionStrings()
			Dim section As ConfigurationSection = config.ConnectionStrings

			' �ڑ������񂪖������͖���
			If section Is Nothing Then
				_mylog.Debug(String.Format("Can't get the section {0}", section.SectionInformation.Name))
				Exit Sub
			End If

			' ���ɈÍ�������Ă��鎞�͖���
			If section.SectionInformation.IsProtected Then
				_mylog.Debug(String.Format("Section {0} is already protected by {1}", section.SectionInformation.Name, section.SectionInformation.ProtectionProvider.Name))
				Exit Sub
			End If

			' ���b�N����Ă��鎞�͖���
			If section.ElementInformation.IsLocked Then
				_mylog.Debug(String.Format("Can't protect, section {0} is locked", section.SectionInformation.Name))
				Exit Sub
			End If

			' �Í����I
			section.SectionInformation.ProtectSection(C_PROVIDER)
			section.SectionInformation.ForceSave = True
			config.Save(ConfigurationSaveMode.Full)

			_mylog.Debug(String.Format("Section {0} is now protected by {1}", section.SectionInformation.Name, section.SectionInformation.ProtectionProvider.Name))
		End Sub

		''' <summary>
		''' �ڑ�������𕡍������܂��B
		''' </summary>
		''' <remarks>
		''' �A�v���P�[�V�����\���t�@�C���̐ڑ�������Z�N�V�����ɑ΂��ĕ��������s���܂��B<br/>
		''' �������A���L�̏ꍇ�͕������͍s���܂���B<br/>
		''' �E�ڑ������񂪖�����<br/>
		''' �E���ɈÍ�������Ă��鎞<br/>
		''' �E���b�N����Ă��鎞<br/>
		''' </remarks>
		Public Sub UnProtectConnectionStrings()
			Dim section As ConfigurationSection = config.ConnectionStrings

			' �ڑ������񂪖������͖���
			If section Is Nothing Then
				_mylog.Debug(String.Format("Can't get the section {0}", section.SectionInformation.Name))
				Exit Sub
			End If

			' ���ɕ���������Ă��鎞�͖���
			If Not section.SectionInformation.IsProtected Then
				_mylog.Debug(String.Format("Section {0} is already unprotected.", section.SectionInformation.Name))
				Exit Sub
			End If

			' ���b�N����Ă��鎞�͖���
			If section.ElementInformation.IsLocked Then
				_mylog.Debug(String.Format("Can't unprotect, section {0} is locked", section.SectionInformation.Name))
				Exit Sub
			End If

			' �������I
			section.SectionInformation.UnprotectSection()
			section.SectionInformation.ForceSave = True
			config.Save(ConfigurationSaveMode.Full)

			_mylog.Debug(String.Format("Section {0} is now unprotected.", section.SectionInformation.Name))
		End Sub

#End Region

	End Class

End Namespace
