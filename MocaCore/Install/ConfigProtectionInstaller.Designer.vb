
Namespace Install

	<System.ComponentModel.RunInstaller(True)> Partial Class ConfigProtectionInstaller
		Inherits System.Configuration.Install.Installer

		'Installer は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
		<System.Diagnostics.DebuggerNonUserCode()> _
		Protected Overrides Sub Dispose(ByVal disposing As Boolean)
			Try
				If disposing AndAlso components IsNot Nothing Then
					components.Dispose()
				End If
			Finally
				MyBase.Dispose(disposing)
			End Try
		End Sub

		'コンポーネント デザイナで必要です。
		Private components As System.ComponentModel.IContainer

		'メモ: 以下のプロシージャはコンポーネント デザイナで必要です。
		'コンポーネント デザイナを使って変更できます。  
		'コード エディタを使って変更しないでください。
		<System.Diagnostics.DebuggerStepThrough()> _
		Private Sub InitializeComponent()
			components = New System.ComponentModel.Container()
		End Sub

	End Class

End Namespace
