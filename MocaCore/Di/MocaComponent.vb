
Imports System.Reflection
Imports Moca.Aop
Imports Moca.Attr
Imports Moca.Util

Namespace Di

	''' <summary>
	''' コンテナに格納する標準的なコンポーネント
	''' </summary>
	''' <remarks></remarks>
	Public Class MocaComponent
		Implements IDisposable

		''' <summary>コンポーネントのキー</summary>
		Private _key As String
		''' <summary>実態の型</summary>
		Private _implType As Type
		''' <summary>フィールドの型</summary>
		Private _fieldType As Type
		''' <summary>アスペクト配列</summary>
		Private _aspects() As IAspect

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="implType">実態の型</param>
		''' <param name="fieldType">フィールドの型</param>
		''' <remarks></remarks>
		Public Sub New(ByVal implType As Type, ByVal fieldType As Type)
			_implType = implType
			_key = _implType.FullName
			_fieldType = fieldType
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="key">コンポーネントのキー</param>
		''' <param name="fieldType">フィールドの型</param>
		''' <remarks></remarks>
		Public Sub New(ByVal key As String, ByVal fieldType As Type)
			_implType = Nothing
			_key = key
			_fieldType = fieldType
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

				' 保持しているオブジェクトの開放
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
#Region " プロパティ "

		''' <summary>
		''' アスペクト配列プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property Aspects() As IAspect()
			Get
				Return _aspects
			End Get
			Set(ByVal value As IAspect())
				_aspects = value
			End Set
		End Property

		''' <summary>
		''' 実態の型プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ImplType() As Type
			Get
				Return _implType
			End Get
		End Property

		''' <summary>
		''' キープロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Key() As String
			Get
				Return _key
			End Get
		End Property

		''' <summary>
		''' フィールドの型プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property FieldType() As Type
			Get
				Return _fieldType
			End Get
		End Property

#End Region

		''' <summary>
		''' オブジェクトをインスタンス化して返します。
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function Create() As Object
			If _aspects Is Nothing Then
				Return createObject(Nothing)
			End If
			If _aspects.Length = 0 Then
				Return createObject(Nothing)
			End If
			Return createProxyObject(Nothing)
		End Function

		''' <summary>
		''' オブジェクトをインスタンス化して返します。
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function Create(ByVal target As Object) As Object
			If _aspects Is Nothing Then
				Return createObject(target)
			End If
			If _aspects.Length = 0 Then
				Return createObject(target)
			End If
			Return createProxyObject(target)
		End Function

		''' <summary>
		''' オブジェクトをインスタンス化して返します。
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Overridable Function createObject(ByVal target As Object) As Object
			Dim val As Object
			val = ClassUtil.NewInstance(_implType)
			Return val
		End Function

		''' <summary>
		''' オブジェクトをプロキシとしてインスタンス化して返します。
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Overridable Function createProxyObject(ByVal target As Object) As Object
			Dim val As Object
			Dim proxy As AopProxy

			proxy = New AopProxy(_implType, _aspects)
			val = proxy.Create()

			Return val
		End Function

	End Class

End Namespace
