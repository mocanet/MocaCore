
Namespace Db

	''' <summary>
	''' SQL ステートメントの結果インタフェース
	''' </summary>
	''' <remarks></remarks>
	Public Interface ISQLStatementResult
		Inherits IDisposable

		''' <summary>
		''' DBをオープンしたかどうか
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property IsDBOpen As Boolean

		''' <summary>
		''' 結果を返す
		''' </summary>
		''' <typeparam name="T">エンティティ</typeparam>
		''' <returns></returns>
		''' <remarks></remarks>
		Function Result(Of T)() As IList(Of T)

		''' <summary>
		''' 次の結果を返す
		''' </summary>
		''' <typeparam name="T">エンティティ</typeparam>
		''' <returns>存在しないときは Nothing をかえす</returns>
		''' <remarks></remarks>
		Function NextResult(Of T)() As IList(Of T)

	End Interface

End Namespace
