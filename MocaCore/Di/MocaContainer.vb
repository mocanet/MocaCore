
Imports System.Threading

Namespace Di

	''' <summary>
	''' コンポーネントたちのコンテナ
	''' </summary>
	''' <remarks>
	''' <see cref="ReaderWriterLock"/> を使ってスレッドセーフにしてます。<br/>
	''' </remarks>
	Public Class MocaContainer
		Implements IContainer, IDisposable

		''' <summary>コンポーネント格納</summary>
		Private _components As Dictionary(Of Object, MocaComponent)

		''' <summary>ロック用</summary>
		Private _rwLock As New ReaderWriterLock()

#Region " コンストラクタ "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
		End Sub

#End Region
#Region " IDisposable Support "

		Private disposedValue As Boolean = False		' 重複する呼び出しを検出するには

		' IDisposable
		Protected Overridable Sub Dispose(ByVal disposing As Boolean)
			If Not Me.disposedValue Then
				If disposing Then
					' TODO: 明示的に呼び出されたときにマネージ リソースを解放します
				End If

				' TODO: 共有のアンマネージ リソースを解放します

				' コンポーネントたちを解放
				Destroy()
			End If
			Me.disposedValue = True
		End Sub

		' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
		Public Sub Dispose() Implements IDisposable.Dispose
			' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
#End Region

		''' <summary>
		''' 初期化処理
		''' </summary>
		''' <remarks></remarks>
		Public Sub Init() Implements IContainer.Init
			_components = New Dictionary(Of Object, MocaComponent)
		End Sub

		''' <summary>
		''' 格納しているコンポーネントを返す。
		''' </summary>
		''' <param name="componentType">取得する型</param>
		''' <returns>該当するコンポーネント。該当しないときは Nothing を返す。</returns>
		''' <remarks></remarks>
		Public Function GetComponent(ByVal componentType As System.Type) As MocaComponent Implements IContainer.GetComponent
			Try
				' リーダーロックを取得
				_rwLock.AcquireReaderLock(Timeout.Infinite)

				If Not _components.ContainsKey(componentType) Then
					Return Nothing
				End If
				Return _components(componentType)
			Finally
				' リーダーロックを解放
				_rwLock.ReleaseReaderLock()
			End Try
		End Function

		''' <summary>
		''' 格納しているコンポーネントを返す。
		''' </summary>
		''' <param name="componentKey">取得するキー</param>
		''' <returns>該当するコンポーネント。該当しないときは Nothing を返す。</returns>
		''' <remarks></remarks>
		Public Function GetComponent(ByVal componentKey As String) As MocaComponent Implements IContainer.GetComponent
			Try
				' リーダーロックを取得
				_rwLock.AcquireReaderLock(Timeout.Infinite)

				If Not _components.ContainsKey(componentKey) Then
					Return Nothing
				End If
				Return _components(componentKey)
			Finally
				' リーダーロックを解放
				_rwLock.ReleaseReaderLock()
			End Try
		End Function

		''' <summary>
		''' コンポーネントを格納する。
		''' </summary>
		''' <param name="component">対象のコンポーネント</param>
		''' <remarks></remarks>
		Public Sub SetComponent(ByVal component As MocaComponent) Implements IContainer.SetComponent
			Try
				' ライターロックを取得
				_rwLock.AcquireWriterLock(Timeout.Infinite)

				If component.ImplType Is Nothing Then
					' キーで格納
					If GetComponent(component.Key) IsNot Nothing Then
						Exit Sub
					End If
					_components.Add(component.Key, component)
					Exit Sub
				End If

				' 型で格納
				If GetComponent(component.ImplType) IsNot Nothing Then
					Exit Sub
				End If
				_components.Add(component.ImplType, component)
			Finally
				' ライターロックを解放
				_rwLock.ReleaseWriterLock()
			End Try
		End Sub

		''' <summary>
		''' コンポーネントの消去
		''' </summary>
		''' <remarks></remarks>
		Public Sub Destroy() Implements IContainer.Destroy
			If _components Is Nothing Then
				Exit Sub
			End If
			For Each component As MocaComponent In _components.Values
				If component Is Nothing Then
					Continue For
				End If
				component.Dispose()
			Next
		End Sub

		''' <summary>
		''' <see cref="MocaComponent"/> を反復処理する列挙子を返します。
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetEnumerator() As IEnumerator(Of MocaComponent) Implements IContainer.GetEnumerator
			Return _components.Values.GetEnumerator()
		End Function

	End Class

End Namespace
