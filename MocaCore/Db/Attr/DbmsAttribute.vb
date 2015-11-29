
Namespace Db.Attr

	''' <summary>
	''' DBMS属性
	''' </summary>
	''' <remarks>
	''' <see cref="IDbAccess"/> クラスに DBMS を関連付けるときに使用する。
	''' </remarks>
	<AttributeUsage(AttributeTargets.Class Or AttributeTargets.Interface)> _
	Public Class DbmsAttribute
		Inherits Attribute

		''' <summary>キー値</summary>
		Private _appkey As String

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="appkey">アプリケーション構成のキー</param>
		''' <remarks></remarks>
		Public Sub New(ByVal appkey As String)
			_appkey = appkey
		End Sub

#End Region

#Region " プロパティ "

		''' <summary>キー値</summary>
		Public Property Appkey() As String
			Get
				Return _appkey
			End Get
			Set(ByVal value As String)
				_appkey = value
			End Set
		End Property

#End Region

		''' <summary>
		''' DBMSを返す
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetDbms() As Dbms
			Return DbmsManager.GetDbms(Appkey)
		End Function

	End Class

End Namespace
