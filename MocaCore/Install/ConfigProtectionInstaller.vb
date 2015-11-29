
Imports System.ComponentModel
Imports System.Configuration
Imports System.Configuration.Install
Imports System.Reflection
Imports Moca.Security

Namespace Install

	''' <summary>
	''' インストール時にアプリケーション構成ファイルを暗号化する
	''' </summary>
	''' <remarks></remarks>
	Public Class ConfigProtectionInstaller

#Region " コンストラクタ "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			MyBase.New()

			'この呼び出しは、コンポーネント デザイナで必要です。
			InitializeComponent()

			'InitializeComponent への呼び出し後、初期化コードを追加します

		End Sub

#End Region

		''' <summary>
		''' インストール時のカスタム動作「インストール」処理
		''' </summary>
		''' <param name="stateSaver"></param>
		''' <remarks></remarks>
		Public Overrides Sub Install(ByVal stateSaver As System.Collections.IDictionary)
			MyBase.Install(stateSaver)
			ProtectConfig()
		End Sub

		''' <summary>
		''' app.config ファイルの暗号化を行う
		''' </summary>
		''' <remarks></remarks>
		Protected Sub ProtectConfig()
			Dim asm As Assembly
			Dim config As System.Configuration.Configuration
			Dim dpapi As DPAPIConfiguration

			' 自分自身のAssemblyを取得
			asm = Assembly.GetExecutingAssembly()

			' 構成情報ファイルをフルパスで指定（.config拡張子は除く）
			config = ConfigurationManager.OpenExeConfiguration(asm.Location)

			' 暗号化
			dpapi = New DPAPIConfiguration(config)
			dpapi.ProtectConnectionStrings()
		End Sub

	End Class

End Namespace
