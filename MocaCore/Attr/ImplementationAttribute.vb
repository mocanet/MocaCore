
Imports System.Reflection
Imports Moca.Di
Imports Moca.Util

Namespace Attr

	''' <summary>
	''' 実態を指定する属性
	''' </summary>
	''' <remarks>
	''' Interface又は、Fieldのみに指定できます。<br/>
	''' この属性を指定されたインタフェースは、自動的に引数のクラスタイプをインスタンス化してフィールドへ注入することが出来ます。<br/>
	''' </remarks>
	<AttributeUsage(AttributeTargets.Interface Or AttributeTargets.Field)> _
	Public Class ImplementationAttribute
		Inherits Attribute

		''' <summary>指定された実体化するクラスタイプ</summary>
		Private _type As Type

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="typ">クラスタイプ</param>
		''' <remarks></remarks>
		Public Sub New(ByVal typ As Type)
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
		''' <returns>コンポーネント</returns>
		''' <remarks></remarks>
		Public Function CreateComponent(ByVal field As FieldInfo) As MocaComponent
			Dim component As MocaComponent
			component = New MocaComponent(_type, field.FieldType)
			Return component
		End Function

	End Class

End Namespace
