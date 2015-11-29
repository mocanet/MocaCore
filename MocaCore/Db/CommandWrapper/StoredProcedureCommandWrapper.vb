
Namespace Db.CommandWrapper

	''' <summary>
	''' StoredProcedureを実行する為のDBCommandのラッパークラス
	''' </summary>
	''' <remarks></remarks>
	Public Class StoredProcedureCommandWrapper
		Inherits SelectCommandWrapper
		Implements IDbCommandStoredProcedure

		''' <summary>パラメータカウンター</summary>
		Private _addParameterValueCount As Integer

#Region " Constructor/DeConstructor "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="dba">親となるDBAccessインスタンス</param>
		''' <param name="cmd">実行するDBCommandインスタンス</param>
		''' <remarks>
		''' </remarks>
		Protected Friend Sub New(ByVal dba As IDao, ByVal cmd As IDbCommand)
			MyBase.New(dba, cmd)
			getParameters()
			_addParameterValueCount = 0
		End Sub

#End Region

#Region " Property "

		''' <summary>
		''' 実行後の戻り値を返す
		''' </summary>
		''' <value></value>
		''' <returns>戻り値</returns>
		''' <remarks></remarks>
		Public ReadOnly Property ReturnValue() As Object Implements IDbCommandStoredProcedure.ReturnValue
			Get
				If Me.ResultOutputParam.ContainsKey("RETURN_VALUE") Then
					Return Me.ResultOutputParam("RETURN_VALUE")
				End If
				Return Nothing
			End Get
		End Property

#End Region

		''' <summary>
		''' 入力パラメータ値を設定する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="value">値</param>
		''' <remarks>
		''' ストアドのパラメータを設定するときのみ使用可能
		''' </remarks>
		Public Sub SetParameterValue(ByVal parameterName As String, ByVal value As Object) Implements IDbCommandStoredProcedure.SetParameterValue
			Dim param As IDbDataParameter

			parameterName = dba.Helper.CDbParameterName(parameterName)
			param = DirectCast(Me.cmd.Parameters.Item(parameterName), IDbDataParameter)
			param.Value = DbUtil.CNull(value)
		End Sub

		''' <summary>
		''' 入力パラメータ値を設定する
		''' </summary>
		''' <param name="index">パラメータ位置</param>
		''' <param name="value">値</param>
		''' <remarks>
		''' ストアドのパラメータを設定するときのみ使用可能です。
		''' パラメータ位置の０番目は@RETURN_VALUEになる為、指定された位置に＋１する。
		''' </remarks>
		Public Sub SetParameterValue(ByVal index As Integer, ByVal value As Object) Implements IDbCommandStoredProcedure.SetParameterValue
			Dim param As IDbDataParameter

			param = DirectCast(Me.cmd.Parameters.Item(index + 1), IDbDataParameter)
			param.Value = DbUtil.CNull(value)
		End Sub

		''' <summary>
		''' 入力パラメータ値の設定を追加する
		''' </summary>
		''' <param name="value"></param>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' 指定されたパラメータが多すぎます。
		''' </exception>
		Public Sub AddParameterValue(ByVal value As Object) Implements IDbCommandStoredProcedure.AddParameterValue
			Dim idx As Integer

			idx = _addParameterValueCount
			idx += 1
			If Me.cmd.Parameters.Count < idx Then
				Throw New DbAccessException(Me.dba, "指定されたパラメータが多すぎます。")
			End If

			_addParameterValueCount = idx
			SetParameterValue(_addParameterValueCount - 1, value)
		End Sub

		''' <summary>
		''' ストアド プロシージャからパラメータ情報を取得し、指定した SqlCommand オブジェクトの Parameters コレクションにパラメータを格納します。
		''' </summary>
		''' <remarks></remarks>
		Protected Sub getParameters()
			Me.dba.Helper.RefreshProcedureParameters(Me.cmd)
		End Sub

		''' <summary>
		''' 更新系のストアドを実行！
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ExecuteNonQuery() As Integer Implements IDbCommandStoredProcedure.ExecuteNonQuery
			_addParameterValueCount = 0
			Return dba.ExecuteNonQuery(Me)
		End Function

		''' <summary>
		''' SQL実行！
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overrides Function Execute() As Integer
			_addParameterValueCount = 0
			Return MyBase.Execute()
		End Function

		''' <summary>
		''' クエリを実行し、指定されたエンティティに変換して返します。
		''' </summary>
		''' <typeparam name="T">エンティティ</typeparam>
		''' <returns>エンティティのリスト</returns>
		''' <remarks>
		''' 当メソッドは予めデータベースをオープンしておく必要がありますが、
		''' オープンされていないときは、自動でオープンして終了時にクローズします。<br/>
		''' </remarks>
		Public Overrides Function Execute(Of T)() As System.Collections.Generic.IList(Of T)
			_addParameterValueCount = 0
			Return MyBase.Execute(Of T)()
		End Function

		''' <summary>
		''' クエリを実行し、そのクエリが返す結果セットの最初の行にある最初の列を返します。余分な列または行は無視されます。
		''' </summary>
		''' <returns>結果セットの最初の行にある最初の列。</returns>
		''' <remarks>
		''' 当メソッドは予めデータベースをオープンしておく必要がありますが、
		''' オープンされていないときは、自動でオープンして終了時にクローズします。<br/>
		''' 詳細は、<seealso cref="IDbCommand.ExecuteScalar"/> を参照してください。
		''' </remarks>
		Public Overrides Function ExecuteScalar() As Object
			_addParameterValueCount = 0
			Return MyBase.ExecuteScalar()
		End Function

	End Class

End Namespace
