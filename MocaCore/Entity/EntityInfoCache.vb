
Imports System.Threading

Namespace Entity

	''' <summary>
	''' 
	''' </summary>
	Public NotInheritable Class EntityInfoCache
		Implements IDisposable, IEnumerable

#Region " Declare "

		''' <summary>シングルトン用コンテナインスタンス</summary>
		Private Shared _instance As EntityInfoCache

		''' <summary>格納</summary>
		Private _cache As Dictionary(Of Type, EntityInfo)

#If net20 Then
		''' <summary>ロック用</summary>
		Private _rwLock As New ReaderWriterLock()
#Else
		''' <summary>ロック用</summary>
		Private _rwLock As New ReaderWriterLockSlim()
#End If

#End Region
#Region " コンストラクタ "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		Private Sub New()
			_cache = New Dictionary(Of Type, EntityInfo)
		End Sub

#End Region
#Region "IDisposable Support"
		Private disposedValue As Boolean ' 重複する呼び出しを検出するには

		' IDisposable
		Protected Sub Dispose(disposing As Boolean)
			If Not disposedValue Then
				If disposing Then
					' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
				End If

				' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
				' TODO: 大きなフィールドを null に設定します。
			End If
			disposedValue = True
		End Sub

		' TODO: 上の Dispose(disposing As Boolean) にアンマネージ リソースを解放するコードが含まれる場合にのみ Finalize() をオーバーライドします。
		'Protected Overrides Sub Finalize()
		'    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(disposing As Boolean) に記述します。
		'    Dispose(False)
		'    MyBase.Finalize()
		'End Sub

		' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
		Public Sub Dispose() Implements IDisposable.Dispose
			' このコードを変更しないでください。クリーンアップ コードを上の Dispose(disposing As Boolean) に記述します。
			Dispose(True)
			' TODO: 上の Finalize() がオーバーライドされている場合は、次の行のコメントを解除してください。
			' GC.SuppressFinalize(Me)
		End Sub

#End Region

#Region " Property "

		Public Shared ReadOnly Property Store As EntityInfoCache
			Get
				If _instance Is Nothing Then
					_instance = New EntityInfoCache()
				End If
				Return _instance
			End Get
		End Property

		Default Public Property Item(ByVal key As Type) As EntityInfo
			Get
				Try
					' リーダーロックを取得
#If net20 Then
					_rwLock.AcquireReaderLock(Timeout.Infinite)
#Else
					_rwLock.EnterReadLock()
#End If

					If _cache.ContainsKey(key) Then
						Return _cache(key)
					End If
				Finally
					' リーダーロックを解放
#If net20 Then
					_rwLock.ReleaseReaderLock()
#Else
					_rwLock.ExitReadLock()
#End If
				End Try
				Return _cacheAdd(key)
			End Get
			Set(value As EntityInfo)
				_cache.Add(key, value)
			End Set
		End Property

#End Region
#Region " Implements IEnumerable "

		Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
			Return _cache
		End Function

#End Region
#Region " Method "

		Private Function _cacheAdd(ByVal key As Type) As EntityInfo
			Try
				' ライターロックを取得
#If net20 Then
				_rwLock.AcquireWriterLock(Timeout.Infinite)
#Else
				_rwLock.EnterWriteLock()
#End If

				Dim info As EntityInfo = New EntityInfo(key)
				_cache.Add(key, info)
				Return info
			Finally
				' ライターロックを解放
#If net20 Then
				_rwLock.ReleaseWriterLock()
#Else
				_rwLock.ExitWriteLock()
#End If
			End Try
		End Function

#End Region

	End Class

End Namespace
