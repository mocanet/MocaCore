
Imports System.Data.Common
Imports System.Data.SqlClient

Namespace Db.Helper

	''' <summary>ＳＱＬエラーコード</summary>
	<Flags()> _
	Public Enum SqlDbErrorNumbers
		''' <summary>重複エラーコード</summary>
		DuplicationPKey = 2627

		''' <summary>タイムアウトエラーコード</summary>
		''' <remarks>
		''' 接続時や排他ロック時のデッドロックでタイムアウトするとこのエラー<br/>
		''' 接続エラー時は「タイムアウトに達しました。操作が完了する前にタイムアウト期間が過ぎたか、またはサーバーが応答していません。」となり、
		''' SQL実行時は「タイムアウトに達しました。操作が完了する前にタイムアウト期間が過ぎたか、またはサーバーが応答していません。 ステートメントは終了されました。」となる。<br/>
		''' 
		''' ただし、デッドロック時は <see cref="SqlException.Number"></see> が変わる可能性がある。<br/>
		''' lock_timeout が 0 以外の時は、-2 となり、0 のときは、タイムアウトせずに直ぐにエラーが発生し、1222 が返ってくる。<br/>
		''' 1222は「ロック要求がタイムアウトしました。」
		''' </remarks>
		LockTimeOut = -2
		''' <summary>タイムアウトエラーコード</summary>
		''' <remarks>
		''' 接続時や排他ロック時のデッドロックでタイムアウトするとこのエラー<br/>
		''' 接続エラー時は「タイムアウトに達しました。操作が完了する前にタイムアウト期間が過ぎたか、またはサーバーが応答していません。」となり、
		''' SQL実行時は「タイムアウトに達しました。操作が完了する前にタイムアウト期間が過ぎたか、またはサーバーが応答していません。 ステートメントは終了されました。」となる。<br/>
		''' 
		''' ただし、デッドロック時は <see cref="SqlException.Number"></see> が変わる可能性がある。<br/>
		''' lock_timeout が 0 以外の時は、-2 となり、0 のときは、タイムアウトせずに直ぐにエラーが発生し、1222 が返ってくる。<br/>
		''' 1222は「ロック要求がタイムアウトしました。」
		''' </remarks>
		LockTimeOut0 = 1222

		''' <summary>
		''' ステートメントは終了されました。
		''' </summary>
		''' <remarks></remarks>
		StatementEnd = 3621
	End Enum

	''' <summary>
	''' SqlClientを使用したDBアクセス
	''' </summary>
	''' <remarks>
	''' データベース接続にSqlClientを使用するときは、当クラスを使用します。
	''' </remarks>
	Public Class SqlDbAccessHelper
		Inherits DbAccessHelper
		Implements IDbAccessHelper

		''' <summary>ＳＱＬエラーコード</summary>
		Public Const C_ERRORCODE As Integer = -2146232060

		''' <summary>ＳＱＬコネクション</summary>
		Private _conn As SqlConnection

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="dba">使用するデータベースアクセス</param>
		''' <remarks></remarks>
		Public Sub New(ByVal dba As IDao)
			MyBase.New(dba)
			_conn = DirectCast(Me.myDba.Connection, SqlConnection)
		End Sub

#End Region

#Region " Implements IDbAccessHelper "

		''' <summary>
		''' SQLステータスのパラメータ名を変換する。
		''' </summary>
		''' <param name="name">パラメータ名</param>
		''' <returns></returns>
		''' <remarks>
		''' パラメータ名の先頭文字が「＠」でないときは「＠」を付加する。
		''' </remarks>
		Public Function CDbParameterName(ByVal name As String) As String Implements IDbAccessHelper.CDbParameterName
			If Not name.StartsWith(PlaceholderMark) Then
				name = PlaceholderMark & name
			End If
			Return name
		End Function

		''' <summary>
		''' エラーの件数を返す
		''' </summary>
		''' <param name="ex">エラー件数を取得したい例外</param>
		''' <returns>エラー件数</returns>
		''' <remarks>
		''' </remarks>
		Public Function ErrorCount(ByVal ex As System.Exception) As Integer Implements IDbAccessHelper.ErrorCount
			Dim dbEx As SqlException

			If Not TypeOf ex Is SqlException Then
				Return 0
			End If

			dbEx = DirectCast(ex, SqlException)

			Return dbEx.Errors.Count
		End Function

		''' <summary>
		''' エラー番号を返す
		''' </summary>
		''' <param name="ex">エラー番号を取得したい例外</param>
		''' <returns>エラー番号配列</returns>
		''' <remarks>
		''' </remarks>
		Public Function ErrorNumbers(ex As System.Exception) As String() Implements IDbAccessHelper.ErrorNumbers
			If ErrorCount(ex) <= 0 Then
				Return Nothing
			End If

			Dim ary As ArrayList
			Dim dbEx As SqlException

			ary = New ArrayList
			dbEx = DirectCast(ex, SqlException)
			For Each err As SqlError In dbEx.Errors
				ary.Add(err.Number.ToString)
			Next

			Return DirectCast(ary.ToArray(GetType(String)), String())
		End Function

		''' <summary>
		''' 列情報を取得する
		''' </summary>
		''' <param name="table">取得対象となるテーブル情報のモデル</param>
		''' <returns>取得した列情報モデルのコレクション</returns>
		''' <remarks>
		''' 列名の取得は「COLUMN_NAME」列となる。<br/>
		''' その他は下記を参照してください。<br/>
		''' http://msdn.microsoft.com/ja-jp/library/ms254969(VS.80).aspx <br/>
		''' </remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Public Function GetSchemaColumns(ByVal table As DbInfoTable) As DbInfoColumnCollection Implements IDbAccessHelper.GetSchemaColumns
			Dim dt As DataTable
			Dim results As DbInfoColumnCollection
			Dim info As DbInfoColumn
			Dim openFlag As Boolean

			results = New DbInfoColumnCollection()

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If
				dt = Me._conn.GetSchema("Columns", New String() {table.Catalog, table.Schema, table.Name, Nothing})
				For ii As Integer = 0 To dt.Rows.Count - 1
					info = New DbInfoColumn( _
					   CStr(dt.Rows(ii).Item("TABLE_CATALOG")) _
					 , CStr(dt.Rows(ii).Item("TABLE_SCHEMA")) _
					 , CStr(dt.Rows(ii).Item("COLUMN_NAME")) _
					 , CStr(dt.Rows(ii).Item("DATA_TYPE")))
					Dim length As Integer
					Dim scale As Integer
					info.MaxLength = getColumnMaxLength(dt.Rows(ii), length, scale)
					info.Precision = length
					info.Scale = scale
					info.UniCode = isUniCode(info.Typ)
					info.ColumnType = getColumnDbType(Of SqlDbType)(dt.Rows(ii))
					results.Add(info.Name, info)
				Next

				Return results
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			Finally
				If openFlag Then
					Me._conn.Close()
				End If
			End Try
		End Function

		''' <summary>
		''' 関数情報を取得する
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Public Function GetSchemaFunctions() As DbInfoFunctionCollection Implements IDbAccessHelper.GetSchemaFunctions
			Const C_SQL As String = "SELECT sys_o.name AS name, sys_o.type AS type, sys_c.text AS src, sys_c.ctext FROM syscomments sys_c ,sysobjects sys_o WHERE(sys_c.id=sys_o.id)AND(sys_o.xtype in ('FN','TF'))AND(sys_o.schema_ver=0)"
			Dim results As DbInfoFunctionCollection

			results = New DbInfoFunctionCollection

			Try
				Using cmd As IDbCommandSelect = Me.myDba.CreateCommandSelect(C_SQL)
					If Me.myDba.Execute(cmd) <= 0 Then
						Return results
					End If

					Dim dt As DataTable
					dt = cmd.ResultDataSet.Tables(0)
					For ii As Integer = 0 To dt.Rows.Count - 1
						Dim item As DbInfoFunction
						item = New DbInfoFunction( _
						String.Empty _
						 , String.Empty _
						 , CStr(dt.Rows(ii).Item("name")) _
						 , CStr(dt.Rows(ii).Item("type")))
						If results.ContainsKey(item.ToString) Then
							item.Src &= CStr(dt.Rows(ii).Item("src"))
							Continue For
						End If
						item.Src = CStr(dt.Rows(ii).Item("src"))
						results.Add(item.ToString, item)
					Next
				End Using

				Return results
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			End Try
		End Function

		''' <summary>
		''' プロシージャ情報を取得する
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Public Function GetSchemaProcedures() As DbInfoProcedureCollection Implements IDbAccessHelper.GetSchemaProcedures
			Const C_SQL As String = "SELECT sys_o.name AS name, sys_o.type AS type, sys_c.text AS src, sys_c.ctext FROM syscomments sys_c ,sysobjects sys_o WHERE(sys_c.id=sys_o.id)AND(sys_o.xtype in ('P'))AND(sys_o.schema_ver=0)"
			Dim results As DbInfoProcedureCollection

			results = New DbInfoProcedureCollection

			Try
				Using cmd As IDbCommandSelect = Me.myDba.CreateCommandSelect(C_SQL)
					If Me.myDba.Execute(cmd) <= 0 Then
						Return results
					End If

					Dim dt As DataTable
					dt = cmd.ResultDataSet.Tables(0)
					For ii As Integer = 0 To dt.Rows.Count - 1
						Dim item As DbInfoProcedure
						item = New DbInfoProcedure( _
						String.Empty _
						 , String.Empty _
						 , CStr(dt.Rows(ii).Item("name")) _
						 , CStr(dt.Rows(ii).Item("type")))
						If results.ContainsKey(item.ToString) Then
							item.Src &= CStr(dt.Rows(ii).Item("src"))
							Continue For
						End If
						item.Src = CStr(dt.Rows(ii).Item("src"))
						results.Add(item.ToString, item)
					Next
				End Using

				Return results
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			End Try
		End Function

		''' <summary>
		''' テーブル情報を取得する
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Public Function GetSchemaTables() As DbInfoTableCollection Implements IDbAccessHelper.GetSchemaTables
			Dim dt As DataTable
			Dim results As DbInfoTableCollection
			Dim info As DbInfoTable
			Dim openFlag As Boolean

			results = New DbInfoTableCollection()

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If
				dt = Me._conn.GetSchema("Tables", New String() {Nothing, Nothing, Nothing, Nothing})
				For ii As Integer = 0 To dt.Rows.Count - 1
					info = New DbInfoTable( _
					   CStr(dt.Rows(ii).Item("TABLE_CATALOG")) _
					 , CStr(dt.Rows(ii).Item("TABLE_SCHEMA")) _
					 , CStr(dt.Rows(ii).Item("TABLE_NAME")) _
					 , CStr(dt.Rows(ii).Item("TABLE_TYPE")))
					results.Add(info.ToString, info)

					info.Columns = GetSchemaColumns(info)
				Next

				Return results
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			Finally
				If openFlag Then
					Me._conn.Close()
				End If
			End Try
		End Function

		''' <summary>
		''' テーブル情報を取得する
		''' </summary>
		''' <param name="tablename">テーブル名</param>
		''' <returns></returns>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Public Function GetSchemaTable(tablename As String) As DbInfoTable Implements IDbAccessHelper.GetSchemaTable
			Dim dt As DataTable
			Dim results As DbInfoTable
			Dim openFlag As Boolean

			results = Nothing

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If
				dt = Me._conn.GetSchema("Tables", New String() {Nothing, Nothing, tablename, Nothing})
				For ii As Integer = 0 To dt.Rows.Count - 1
					results = New DbInfoTable( _
					 CStr(dt.Rows(ii).Item("TABLE_CATALOG")) _
					  , CStr(dt.Rows(ii).Item("TABLE_SCHEMA")) _
					  , CStr(dt.Rows(ii).Item("TABLE_NAME")) _
					  , CStr(dt.Rows(ii).Item("TABLE_TYPE")))

					results.Columns = GetSchemaColumns(results)
				Next

				Return results
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			Finally
				If openFlag Then
					Me._conn.Close()
				End If
			End Try
		End Function

		''' <summary>
		''' 指定されたエラー番号が発生した例外に存在するか返す
		''' </summary>
		''' <param name="ex">対象となる例外</param>
		''' <param name="errorNumber">エラー番号</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Public Function HasSqlNativeError(ByVal ex As System.Exception, ByVal errorNumber As Long) As Boolean Implements IDbAccessHelper.HasSqlNativeError
			Dim err As SqlError

			If ErrorCount(ex) <= 0 Then
				Return False
			End If

			Dim dbEx As SqlException

			dbEx = DirectCast(ex, SqlException)
			For Each err In dbEx.Errors
				If err.Number = errorNumber Then
					Return True
				End If
			Next

			Return False
		End Function

		''' <summary>
		''' 重複エラーが発生した例外に存在するか返す
		''' </summary>
		''' <param name="ex">対象となる例外</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Public Function HasSqlNativeErrorDuplicationPKey(ByVal ex As System.Exception) As Boolean Implements IDbAccessHelper.HasSqlNativeErrorDuplicationPKey
			Return HasSqlNativeError(ex, SqlDbErrorNumbers.DuplicationPKey)
		End Function

		''' <summary>
		''' タイムアウトエラーが発生した例外に存在するか返す
		''' </summary>
		''' <param name="ex">対象となる例外</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Public Function HasSqlNativeErrorTimtout(ByVal ex As System.Exception) As Boolean Implements IDbAccessHelper.HasSqlNativeErrorTimtout
			Return (HasSqlNativeError(ex, SqlDbErrorNumbers.LockTimeOut) OrElse HasSqlNativeError(ex, SqlDbErrorNumbers.LockTimeOut0))
		End Function

		''' <summary>
		''' SQLプレースフォルダのマークを返す。
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property PlaceholderMark() As String Implements IDbAccessHelper.PlaceholderMark
			Get
				Return "@"
			End Get
		End Property

		''' <summary>
		''' プロシージャのパラメータ情報を取得する
		''' </summary>
		''' <param name="cmd"></param>
		''' <remarks></remarks>
		''' <exception cref="DbAccessException">
		''' DBアクセスでエラーが発生した
		''' </exception>
		Public Sub RefreshProcedureParameters(ByVal cmd As System.Data.IDbCommand) Implements IDbAccessHelper.RefreshProcedureParameters
			Try
				Dim openFlg As Boolean = False

				' コネクションが閉じてる場合は一旦接続する
				If cmd.Connection.State = ConnectionState.Closed Then
					cmd.Connection.Open()
					openFlg = True
				End If
				SqlCommandBuilder.DeriveParameters(DirectCast(cmd, SqlCommand))
				' コネクションが閉じてた場合は接続を切る
				If openFlg Then
					cmd.Connection.Close()
				End If

				For Each para As IDataParameter In cmd.Parameters
					para.Value = DBNull.Value
				Next
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			End Try
		End Sub

#End Region

#Region " Methods "

		''' <summary>
		''' 列の最大桁数を返す
		''' </summary>
		''' <param name="row">行データ</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Function getColumnMaxLength(ByVal row As DataRow, ByRef length As Integer, ByRef scale As Integer) As Integer
			Dim wlength As Object = getColumnLength(row)

			' 桁の指定がある時は、桁設定して終了
			' （バイナリ データ、文字データ、またはテキスト/イメージ データの最大長）
			If Not DBNull.Value.Equals(wlength) Then
				length = 0
				scale = 0
				Return CInt(wlength)
			End If

			' 桁の指定が無いとき数値系の桁数を取得
			wlength = getColumnPrecision(row)

			' 桁が存在しない時は桁指定なしで終了
			If DBNull.Value.Equals(wlength) Then
				length = 0
				scale = 0
				Return length
			End If

			' 概数データ、真数データ、整数データ、または通貨のとき
			Dim wscale As Object = getColumnScale(row)

			' 小数点がない時
			If DBNull.Value.Equals(wscale) Then
				length = CInt(wlength)
				scale = 0
				Return length
			End If

			' 小数点がある時
			'Return CInt(length) + CInt(scale) + 1
			length = CInt(wlength)
			scale = CInt(wscale)
			Return length + 1
		End Function

		''' <summary>
		''' 列の桁数を返します。
		''' </summary>
		''' <param name="row">行データ</param>
		''' <returns>テーブル又は列が存在しないときや、桁数の指定が不要な型の時は DBNull.Value を返します。</returns>
		''' <remarks>
		''' バイナリ データ、文字データ、またはテキスト/イメージ データの最大長 (文字単位)。
		''' それ以外の場合は、NULL が返されます。
		''' 詳細については、『Microsoft SQL Server 2000 Transact-SQL プログラマーズリファレンス上』の「第 3 章 Transact-SQL のデータ型」を参照してください。
		''' </remarks>
		Protected Function getColumnLength(ByVal row As DataRow) As Object
			If row Is Nothing Then
				Return DBNull.Value
			End If

			Return row.Item("character_maximum_length")
		End Function

		''' <summary>
		''' 列の桁数を返します。
		''' </summary>
		''' <param name="row">行データ</param>
		''' <returns>テーブル又は列が存在しないときや、桁数の指定が不要な型の時は DBNull.Value を返します。</returns>
		''' <remarks>
		''' 概数データ、真数データ、整数データ、または通貨データの有効桁数。それ以外の場合は、NULL が返されます。
		''' </remarks>
		Protected Function getColumnPrecision(ByVal row As DataRow) As Object
			If row Is Nothing Then
				Return DBNull.Value
			End If

			Return row.Item("NUMERIC_PRECISION")
		End Function

		''' <summary>
		''' 列の桁数を返します。
		''' </summary>
		''' <param name="row">行データ</param>
		''' <returns>テーブル又は列が存在しないときや、桁数の指定が不要な型の時は DBNull.Value を返します。</returns>
		''' <remarks>
		''' 概数データ、真数データ、整数データ、または通貨データの桁数。それ以外の場合は、NULL が返されます。
		''' </remarks>
		Protected Function getColumnScale(ByVal row As DataRow) As Object
			If row Is Nothing Then
				Return DBNull.Value
			End If

			Return row.Item("NUMERIC_SCALE")
		End Function

		''' <summary>
		''' 列の型をかえします。
		''' </summary>
		''' <typeparam name="T">使用する型のDbTypeを指定する</typeparam>
		''' <param name="row">行データ</param>
		''' <returns>テーブル又は列が存在しないときは DBNull.Value を返します。</returns>
		''' <remarks>
		''' SQLServer は numeric は SqlDbType には存在しないから Decimal にマップします。
		''' </remarks>
		Protected Function getColumnDbType(Of T)(ByVal row As DataRow) As T
			Dim typ As String

			If row Is Nothing Then
				Return Nothing
			End If

			typ = CStr(row.Item("DATA_TYPE"))

			If GetType(T).Name.Equals("SqlDbType") Then
				' numeric は SqlDbType には存在しないから Decimal にマップする
				If typ.Equals("numeric") Then
					typ = SqlDbType.Decimal.ToString
				End If
			End If

			Return DirectCast([Enum].Parse(GetType(T), typ, True), T)
		End Function

		''' <summary>
		''' 型がUniCodeか判定
		''' </summary>
		''' <param name="typ"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function isUniCode(ByVal typ As String) As Boolean
			Select Case typ
				Case "nvarchar", "nchar"
					Return True
				Case Else
					Return False
			End Select
		End Function

#End Region

#Region " Debug "

		Public Function ConnectionTest() As DataTable
			Dim dt As DataTable
			Dim openFlag As Boolean

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If

				dt = Me._conn.GetSchema("MetaDataCollections")

				Return dt
			Finally
				If openFlag Then
					Me._conn.Close()
				End If
			End Try
		End Function

#End Region

	End Class

End Namespace
