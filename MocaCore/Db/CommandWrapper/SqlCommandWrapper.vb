
Imports System.Text.RegularExpressions

Namespace Db.CommandWrapper

	''' <summary>
	''' DBCommandのラッピング抽象クラス
	''' </summary>
	''' <remarks></remarks>
	Public MustInherit Class SqlCommandWrapper
		Implements IDbCommandSql

		''' <summary>親となるDBAccessインスタンス</summary>
		Protected dba As IDao
		''' <summary>実行するDBCommandインスタンス</summary>
		Protected cmd As IDbCommand
		''' <summary>コンパイル済みのSQLを使うかどうか</summary>
		Private _preparedStatement As Boolean
		''' <summary>プレースフォルダ配列</summary>
		Private _placeholders() As String
		''' <summary>SQL文のオリジナル</summary>
		Private _originalCommandText As String
		''' <summary>実行後の戻り値</summary>
		Private _outputParams As Hashtable

		''' <summary>データベースから取得したデータの格納先となる Entity を作成する</summary>
		Protected entityBuilder As New EntityBuilder

#Region " Constructor/DeConstructor "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="dba">親となるDBAccessインスタンス</param>
		''' <param name="cmd">実行するDBCommandインスタンス</param>
		''' <remarks>
		''' </remarks>
		Friend Sub New(ByVal dba As IDao, ByVal cmd As IDbCommand)
			Me.dba = dba
			Me.cmd = cmd
			_preparedStatement = False
			_originalCommandText = cmd.CommandText
			_outputParams = New Hashtable
		End Sub

#End Region

#Region " IDisposable Support "

		Private _disposedValue As Boolean = False		' 重複する呼び出しを検出するには

		''' <summary>
		''' IDisposable
		''' </summary>
		''' <param name="disposing"></param>
		''' <remarks></remarks>
		Protected Overridable Sub Dispose(ByVal disposing As Boolean)
			If Not Me._disposedValue Then
				If disposing Then
					' TODO: 明示的に呼び出されたときにマネージ リソースを解放します
				End If

				' TODO: 共有のアンマネージ リソースを解放します
			End If
			Me._disposedValue = True
		End Sub

		' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
		Public Sub Dispose() Implements IDisposable.Dispose
			' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
#End Region

#Region " Implements IDbCommandSql "

#Region " Properties "

		''' <summary>
		''' 実行するDBCommandインスタンスを参照
		''' </summary>
		''' <value>実行するDBCommandインスタンス</value>
		''' <remarks>
		''' </remarks>
        Public ReadOnly Property Command() As IDbCommand Implements IDbCommandSql.Command
            Get
                Return cmd
            End Get
        End Property

		''' <summary>
		''' コンパイル済みのSQLを使うかどうかを指定
		''' </summary>
		''' <value>
		''' True:使用する
		''' False:使用しない
		''' </value>
		''' <remarks>
		''' </remarks>
        Public Property PreparedStatement() As Boolean Implements IDbCommandSql.PreparedStatement
            Get
                Return _preparedStatement
            End Get
            Set(ByVal Value As Boolean)
                _preparedStatement = Value
            End Set
        End Property

		''' <summary>
		''' SQL文
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property CommandText() As String Implements IDbCommandSql.CommandText
			Get
				Return _originalCommandText
			End Get
			Set(ByVal value As String)
				_originalCommandText = value
				Me.cmd.CommandText = value
			End Set
		End Property

		''' <summary>
		''' 実行後の戻り値を返す
		''' </summary>
		''' <value></value>
		''' <returns>戻り値</returns>
		''' <remarks></remarks>
		Public ReadOnly Property ResultOutputParam() As System.Collections.Hashtable Implements IDbCommandSql.ResultOutParameter
			Get
				Return _outputParams
			End Get
		End Property

#End Region

#Region " DbDataParameter "

		''' <summary>
		''' 入力パラメータを設定する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="value">値</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks>
		''' </remarks>
		Public Function SetParameter(ByVal parameterName As String, ByVal value As Object) As IDbDataParameter Implements IDbCommandSql.SetParameter
			Dim param As IDbDataParameter

			param = addParameter(parameterName, value)
			param.Direction = ParameterDirection.Input

			Return param
		End Function

		''' <summary>
		''' 入力パラメータを設定する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="values">値配列</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks>
		''' 当メソッドでは IN 句を作成します。
		''' IN 句はパラメータとしては扱えないので、SQL文内に存在するパラメータ名部分を文字列変換します。
		''' </remarks>
		Public Function SetParameter(ByVal parameterName As String, ByVal values As Array) As String Implements IDbCommandSql.SetParameter
			Dim sql As String
			Dim whereIn As String

			whereIn = String.Empty
			sql = cmd.CommandText
			parameterName = dba.Helper.CDbParameterName(parameterName)

			If sql.IndexOf(parameterName) < 0 Then
				sql = _originalCommandText
			End If

			Dim lst As ArrayList

			lst = New ArrayList
			For Each value As Object In values
				lst.Add("'" & CStr(value).Replace("'", "''") & "'")
			Next

			whereIn = "(" & String.Join(",", DirectCast(lst.ToArray(GetType(String)), String())) & ")"
			sql = sql.Replace(parameterName, whereIn)
			cmd.CommandText = sql

			Return whereIn
		End Function

		''' <summary>
		''' 入力パラメータを追加する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="dbTypeValue">パラメータの型</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks>
		''' </remarks>
		Public Function AddInParameter(ByVal parameterName As String, ByVal dbTypeValue As DbType) As IDbDataParameter Implements IDbCommandSql.AddInParameter
			Dim param As IDbDataParameter

			param = addParameter(parameterName, Nothing)
			param.Direction = ParameterDirection.Input
			param.DbType = dbTypeValue

			Return param
		End Function

		''' <summary>
		''' 入力パラメータを追加する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="dbTypeValue">パラメータの型</param>
		''' <param name="size">パラメータのサイズ</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks>
		''' </remarks>
		Public Function AddInParameter(ByVal parameterName As String, ByVal dbTypeValue As DbType, ByVal size As Integer) As IDbDataParameter Implements IDbCommandSql.AddInParameter
			Dim param As IDbDataParameter

			param = AddInParameter(parameterName, dbTypeValue)
			param.Size = size

			Return param
		End Function

		''' <summary>
		''' 出力パラメータを追加する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks>
		''' </remarks>
		Public Function AddOutParameter(ByVal parameterName As String) As IDbDataParameter Implements IDbCommandSql.AddOutParameter
			Dim param As IDbDataParameter

			param = addParameter(parameterName, Nothing)
			param.Direction = ParameterDirection.Output

			Return param
		End Function

		''' <summary>
		''' 出力パラメータを追加する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="dbTypeValue">パラメータの型</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks>
		''' </remarks>
		Public Function AddOutParameter(ByVal parameterName As String, ByVal dbTypeValue As DbType) As IDbDataParameter Implements IDbCommandSql.AddOutParameter
			Dim param As IDbDataParameter

			param = AddOutParameter(parameterName)
			param.Direction = ParameterDirection.Output
			param.DbType = dbTypeValue

			Return param
		End Function

		''' <summary>
		''' パラメータ内に戻り値があるか返す
		''' </summary>
		''' <returns>True は戻り値有り、False は戻り値無し</returns>
		''' <remarks></remarks>
		Public Function HaveOutParameter() As Boolean Implements IDbCommandSql.HaveOutParameter
			Dim ee As IEnumerator = cmd.Parameters.GetEnumerator
			While ee.MoveNext
				Dim param As IDbDataParameter
				param = DirectCast(ee.Current, IDbDataParameter)
				If param.Direction = ParameterDirection.InputOutput Or param.Direction = ParameterDirection.Output Then
					Return True
				End If
			End While
			Return False
		End Function

		''' <summary>
		''' 出力パラメータを参照する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <returns>出力パラメータ値</returns>
		''' <remarks>
		''' </remarks>
		Public Function GetParameterValue(ByVal parameterName As String) As Object Implements IDbCommandSql.GetParameterValue
			parameterName = dba.Helper.CDbParameterName(parameterName)
			Return cmd.Parameters.Item(parameterName)
		End Function

#End Region

		''' <summary>
		''' SQL実行
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public MustOverride Function Execute() As Integer Implements IDbCommandSql.Execute

		''' <summary>
		''' コンパイル済みのSQLにする
		''' </summary>
		''' <remarks>
		''' 当メソッド実行前に予め <see cref="AddInParameter"/> を使用してパラメータを設定しておいてください。<br/>
		''' </remarks>
		Public Sub Prepare() Implements IDbCommandSql.Prepare
			cmd.Prepare()
			PreparedStatement = True
		End Sub

#End Region

#Region " Methods "

		''' <summary>
		''' パラメータを追加又は取得する
		''' </summary>
		''' <param name="parameterName">パラメータ名</param>
		''' <param name="value">パラメータの値</param>
		''' <returns>パラメータインスタンス</returns>
		''' <remarks>
		''' </remarks>
		Protected Function addParameter(ByVal parameterName As String, ByVal value As Object) As IDbDataParameter
			Dim param As IDbDataParameter

			parameterName = dba.Helper.CDbParameterName(parameterName)

			If cmd.Parameters.Contains(parameterName) Then
				param = DirectCast(cmd.Parameters.Item(parameterName), IDbDataParameter)
				param.Value = DbUtil.CNull(value)
			Else
				param = dba.Dbms.ProviderFactory.CreateParameter()
				param.ParameterName = parameterName
				param.Value = DbUtil.CNull(value)
				cmd.Parameters.Add(param)
			End If

			Return param
		End Function

		''' <summary>
		''' プレースフォルダを取得
		''' </summary>
		''' <remarks>
		''' 今後の拡張のための実装<br/>
		''' だが、リリースするかは不明
		''' </remarks>
		Protected Sub cnvPlaceholder()
			_placeholders = _getPlaceholder()
		End Sub

		''' <summary>
		''' SQLコマンドのプレースフォルダを返す。
		''' </summary>
		''' <returns>プレースフォルダ名の配列</returns>
		''' <remarks>
		''' プレースフォルダは「/*name*/」としてください。<br/>
		''' </remarks>
		Private Function _getPlaceholder() As String()
			Dim params As ArrayList
			Dim r As New Regex("/\*(.*?)\*/", RegexOptions.IgnoreCase Or RegexOptions.Singleline)

			' 正規表現と一致する対象をすべて検索 
			Dim mc As MatchCollection = r.Matches(Me.CommandText)

			params = New ArrayList

			' 正規表現に一致したグループの文字列を表示 
			For Each m As Match In mc
				If (Not m.Groups(1).Value.StartsWith(" ")) Then
					params.Add(m.Groups(1).Value)
				End If
			Next
			Return DirectCast(params.ToArray(GetType(String)), String())
		End Function

#End Region

	End Class

End Namespace
