
Imports Moca.Db

Namespace Di

	''' <summary>
	''' コンテナに格納するデータベースを扱うコンポーネント
	''' </summary>
	''' <remarks></remarks>
	Public Class MocaComponent4Db
		Inherits MocaComponent

		''' <summary>扱うDBMS</summary>
		Private _dbms As Dbms

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="implType">実態の型</param>
		''' <param name="fieldType">フィールドの型</param>
		''' <param name="targetDbms">対象となるDBMS</param>
		''' <remarks></remarks>
		Public Sub New(ByVal implType As Type, ByVal fieldType As Type, ByVal targetDbms As Dbms)
			MyBase.New(implType, fieldType)
			_dbms = targetDbms
		End Sub

#End Region
#Region " プロパティ "

		''' <summary>
		''' DBMS
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property Dbms() As Dbms
			Get
				Return _dbms
			End Get
		End Property

#End Region

		''' <summary>
		''' オブジェクトをインスタンス化して返します。
		''' </summary>
		''' <returns></returns>
		''' <remarks>オーバーライドメソッド</remarks>
		Protected Overrides Function createObject(ByVal target As Object) As Object
			Dim val As Object
			val = MyBase.createObject(target)
			DirectCast(val, AbstractDao).TargetDbms = _dbms
			Return val
		End Function

		''' <summary>
		''' オブジェクトをプロキシとしてインスタンス化して返します。
		''' </summary>
		''' <returns></returns>
		''' <remarks>オーバーライドメソッド</remarks>
		Protected Overrides Function createProxyObject(ByVal target As Object) As Object
			Dim val As Object
			val = MyBase.createProxyObject(target)

			Dim dao As AbstractDao
			dao = DirectCast(val, AbstractDao)
			dao.TargetDbms = _dbms

			Dim daoTarget As AbstractDao
			daoTarget = TryCast(target, AbstractDao)
			If daoTarget Is Nothing Then
				Return val
			End If

			Return val
		End Function

	End Class

End Namespace
