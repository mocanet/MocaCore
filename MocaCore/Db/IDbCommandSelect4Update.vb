
Namespace Db

	''' <summary>
	''' SELECT文を実行し、DataSetを使ってUPDATEする為のDBCommandのラッピングするインタフェース
	''' </summary>
	''' <remarks>
	''' <example>
	''' <code lang="vb">
	''' Using dba As IDbAccess = New DbAccess("Connection String")
	''' 	Using cmd As IDbCommandSelect4Update = dba.CreateCommandSelect4Update("SELECT * FROM TableName")
	''' 		If cmd.Execute() &lt;= 0 Then
	''' 			Return Nothing
	''' 		End If
	''' 
	''' 		... cmd.ResultDataSet に対して更新処理 ...
	''' 
	''' 		Dim rc As Integer
	''' 		rc = cmd.Update()
	''' 		Debug.Print(rc)
	''' 	End Using
	''' End Using
	''' </code>
	''' </example>
	''' </remarks>
	Public Interface IDbCommandSelect4Update
		Inherits IDbCommandSelect

#Region " Property "

		''' <summary>
		''' アダプタインスタンス
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property Adapter() As IDbDataAdapter

#End Region

		''' <summary>
		''' Adapter Update 実行！
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function Update() As Integer

	End Interface

End Namespace
