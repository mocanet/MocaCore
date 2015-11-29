
Imports Moca.Exceptions

Namespace Di

	''' <summary>
	''' コンテナのファクトリクラス
	''' </summary>
	''' <remarks></remarks>
	Public Class MocaContainerFactory

		''' <summary>シングルトン用コンテナインスタンス</summary>
		Private Shared _instance As IContainer

		''' <summary>
		''' 初期化処理
		''' </summary>
		''' <remarks></remarks>
		Public Shared Sub Init()
			If _instance IsNot Nothing Then
				Exit Sub
			End If
			_instance = New MocaContainer()
			_instance.Init()
		End Sub

		''' <summary>
		''' コンポーネントの消去
		''' </summary>
		''' <remarks></remarks>
		Public Shared Sub Destroy()
			If _instance Is Nothing Then
				Exit Sub
			End If
			_instance.Destroy()
		End Sub

		''' <summary>
		''' デフォルトのコンテナインスタンス作成
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared ReadOnly Property Container() As IContainer
			Get
				If _instance Is Nothing Then
					Throw New MocaRuntimeException("コンテナの初期化が実行されていません。")
				End If
				Return _instance
			End Get
		End Property

	End Class

End Namespace
