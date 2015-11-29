Imports System
Imports System.Configuration

Namespace Security

	''' <summary>
	''' Dpapiを利用してapp.configを暗号化する為のクラス
	''' </summary>
	''' <remarks>
	''' 下記の暗号化/複合化をサポートしています。
	''' <list>
	''' <item>ConnectionStrings</item>
	''' </list>
	''' </remarks>
	Public Class DPAPIConfiguration

		''' <summary>使用するプロパイダー</summary>
		Protected Const C_PROVIDER As String = "DataProtectionConfigurationProvider"

		''' <summary>使用するapp.configファイル</summary>
		Protected config As System.Configuration.Configuration

		''' <summary>log4net logger</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

#Region " Constructor/DeConstructor "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks>
		''' 起動中アプリケーションの構成ファイルに対して処理します。
		''' </remarks>
		Public Sub New()
			config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None)
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="config">使用するapp.configファイル</param>
		''' <remarks>app.configファイルを指定する時に使用する。</remarks>
		Public Sub New(ByVal config As System.Configuration.Configuration)
			Me.config = config
		End Sub

#End Region

#Region " DataProtectionConfigurationProvider "

		''' <summary>
		''' 接続文字列を暗号化します。
		''' </summary>
		''' <remarks>
		''' アプリケーション構成ファイルの接続文字列セクションに対して暗号化を行います。<br/>
		''' ただし、下記の場合は暗号化は行いません。<br/>
		''' ・接続文字列が無い時<br/>
		''' ・既に暗号化されている時<br/>
		''' ・ロックされている時<br/>
		''' </remarks>
		Public Sub ProtectConnectionStrings()
			Dim section As ConfigurationSection = config.ConnectionStrings

			' 接続文字列が無い時は無視
			If section Is Nothing Then
				_mylog.Debug(String.Format("Can't get the section {0}", section.SectionInformation.Name))
				Exit Sub
			End If

			' 既に暗号化されている時は無視
			If section.SectionInformation.IsProtected Then
				_mylog.Debug(String.Format("Section {0} is already protected by {1}", section.SectionInformation.Name, section.SectionInformation.ProtectionProvider.Name))
				Exit Sub
			End If

			' ロックされている時は無視
			If section.ElementInformation.IsLocked Then
				_mylog.Debug(String.Format("Can't protect, section {0} is locked", section.SectionInformation.Name))
				Exit Sub
			End If

			' 暗号化！
			section.SectionInformation.ProtectSection(C_PROVIDER)
			section.SectionInformation.ForceSave = True
			config.Save(ConfigurationSaveMode.Full)

			_mylog.Debug(String.Format("Section {0} is now protected by {1}", section.SectionInformation.Name, section.SectionInformation.ProtectionProvider.Name))
		End Sub

		''' <summary>
		''' 接続文字列を複合化します。
		''' </summary>
		''' <remarks>
		''' アプリケーション構成ファイルの接続文字列セクションに対して複合化を行います。<br/>
		''' ただし、下記の場合は複合化は行いません。<br/>
		''' ・接続文字列が無い時<br/>
		''' ・既に暗号化されている時<br/>
		''' ・ロックされている時<br/>
		''' </remarks>
		Public Sub UnProtectConnectionStrings()
			Dim section As ConfigurationSection = config.ConnectionStrings

			' 接続文字列が無い時は無視
			If section Is Nothing Then
				_mylog.Debug(String.Format("Can't get the section {0}", section.SectionInformation.Name))
				Exit Sub
			End If

			' 既に複合化されている時は無視
			If Not section.SectionInformation.IsProtected Then
				_mylog.Debug(String.Format("Section {0} is already unprotected.", section.SectionInformation.Name))
				Exit Sub
			End If

			' ロックされている時は無視
			If section.ElementInformation.IsLocked Then
				_mylog.Debug(String.Format("Can't unprotect, section {0} is locked", section.SectionInformation.Name))
				Exit Sub
			End If

			' 複合化！
			section.SectionInformation.UnprotectSection()
			section.SectionInformation.ForceSave = True
			config.Save(ConfigurationSaveMode.Full)

			_mylog.Debug(String.Format("Section {0} is now unprotected.", section.SectionInformation.Name))
		End Sub

#End Region

	End Class

End Namespace
