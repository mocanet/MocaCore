
Namespace Di

	''' <summary>
	''' コンポーネントたちのコンテナインタフェース
	''' </summary>
	''' <remarks></remarks>
	Public Interface IContainer

		''' <summary>
		''' 初期化処理
		''' </summary>
		''' <remarks></remarks>
		Sub Init()

		''' <summary>
		''' コンポーネントの消去
		''' </summary>
		''' <remarks></remarks>
		Sub Destroy()

		''' <summary>
		''' 格納しているコンポーネントを返す。
		''' </summary>
		''' <param name="componentType">取得する型</param>
		''' <returns>該当するコンポーネント。該当しないときは Nothing を返す。</returns>
		''' <remarks></remarks>
		Function GetComponent(ByVal componentType As Type) As MocaComponent

		''' <summary>
		''' 格納しているコンポーネントを返す。
		''' </summary>
		''' <param name="componentKey">取得するキー</param>
		''' <returns>該当するコンポーネント。該当しないときは Nothing を返す。</returns>
		''' <remarks></remarks>
		Function GetComponent(ByVal componentKey As String) As MocaComponent

		''' <summary>
		''' コンポーネントを格納する。
		''' </summary>
		''' <param name="component">対象のコンポーネント</param>
		''' <remarks></remarks>
		Sub SetComponent(ByVal component As MocaComponent)

		''' <summary>
		''' <see cref="MocaComponent"/> を反復処理する列挙子を返します。
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetEnumerator() As IEnumerator(Of MocaComponent)

	End Interface

End Namespace
