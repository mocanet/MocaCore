
Imports System.Reflection
Imports Moca.Aop
Imports Moca.Attr
Imports Moca.Di
Imports Moca.Interceptor
Imports Moca.Util

Namespace Db.Attr

	''' <summary>
	''' DAO属性
	''' </summary>
	''' <remarks>
	''' <see cref="IDao"/> を実装したクラスを指定する属性
	''' </remarks>
	<AttributeUsage(AttributeTargets.Interface)> _
	Public Class DaoAttribute
		Inherits DbmsAttribute

		''' <summary>指定された実体化するクラスタイプ</summary>
		Private _type As Type

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="appkey">アプリケーション構成のキー</param>
		''' <param name="typ">クラスタイプ</param>
		''' <remarks></remarks>
		Public Sub New(ByVal appkey As String, ByVal typ As Type)
			MyBase.New(appkey)
			_type = typ
		End Sub

#End Region
#Region " プロパティ "

		''' <summary>
		''' クラスタイププロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property ImplType() As Type
			Get
				Return _type
			End Get
		End Property

#End Region

		''' <summary>
		''' コンポーネント作成
		''' </summary>
		''' <param name="field">フィールド</param>
		''' <returns>DBMSコンポーネント</returns>
		''' <remarks></remarks>
		Public Function CreateComponent(ByVal field As FieldInfo) As MocaComponent
			' 型チェック
			If Not ClassUtil.IsInterfaceImpl(ImplType, GetType(IDao)) Then
				Throw New ArgumentException(ImplType.FullName & " は、" & GetType(IDao).FullName & " を実装したクラスではありません。")
			End If

			' DBMS 特定
			Dim targetDbms As Dbms
			targetDbms = DbmsManager.GetDbms(Appkey)

			Dim aspects As ArrayList
			Dim fields() As FieldInfo

			aspects = New ArrayList()

			' さらにフィールドを解析
			fields = ClassUtil.GetFields(_type)
			For Each fi As FieldInfo In fields

				If ClassUtil.GetCustomAttribute(Of DaoAttribute)(fi.FieldType) Is Nothing Then
					If ClassUtil.GetCustomAttribute(Of ImplementationAttribute)(fi.FieldType) Is Nothing Then
						Continue For
					End If
				End If

				' Getter/Setter メソッドのアスペクト作成（フィールドへアクセスするために必要！）
				Dim pointcut As IPointcut
				pointcut = New Pointcut(New String() {"Void FieldGetter(System.String, System.String, System.Object ByRef)"})
				aspects.Add(New Aspect(New FieldGetterInterceptor(), pointcut))
				pointcut = New Pointcut(New String() {"Void FieldSetter(System.String, System.String, System.Object)"})
				aspects.Add(New Aspect(New FieldSetterInterceptor(), pointcut))
			Next

			Dim component As MocaComponent4Db
			component = New MocaComponent4Db(_type, field.FieldType, targetDbms)
			component.Aspects = DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())
			Return component
		End Function

	End Class

End Namespace
