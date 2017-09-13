
Imports System.Data.Common
Imports System.Data.OleDb

Namespace Db.Helper

	''' <summary>ＳＱＬエラーコード</summary>
	Public Enum OleDbErrorNumbers
		''' <summary>重複エラーコード</summary>
		DuplicationPKey = 2627
		''' <summary>タイムアウトエラーコード</summary>
		TimeOut = -2147217871
		'TimeOut = -2146232060

		''' <summary>
		''' 指定した値は、列またはテーブルの整合性制約に違反しました。
		''' </summary>
		''' <remarks></remarks>
		TableCompatibilityRestrictions = -2147217873

		''' <summary>
		''' ステートメントは終了されました。
		''' </summary>
		''' <remarks></remarks>
		StatementEnd = 3621
	End Enum

	''' <summary>
	''' OleDbを使用したDBアクセス
	''' </summary>
	''' <remarks>
	''' データベース接続にOleDbを使用するときは、当クラスを使用します。
	''' </remarks>
	Public MustInherit Class OleDbAccessHelper
		Inherits DbAccessHelper
		Implements IDbAccessHelper

		''' <summary>ＳＱＬコネクション</summary>
		Private _conn As OleDbConnection

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="dba">使用するデータベースアクセス</param>
		''' <remarks></remarks>
		Public Sub New(ByVal dba As IDao)
			MyBase.New(dba)
			_conn = DirectCast(Me.myDba.Connection, OleDbConnection)
		End Sub

#End Region

#Region " Property "

		Protected Friend ReadOnly Property Connection As OleDbConnection
			Get
				Return Me._conn
			End Get
		End Property

#End Region

#Region " Implements IDbAccessHelper "

		''' <summary>
		''' SQLステータスのパラメータ名を変換する。
		''' </summary>
		''' <param name="name">パラメータ名</param>
		''' <returns></returns>
		''' <remarks>
		''' プレースフォルダが「？」なので、指定された名称はそのまま利用する。
		''' </remarks>
		Public Function CDbParameterName(ByVal name As String) As String Implements IDbAccessHelper.CDbParameterName
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
			Dim dbEx As OleDbException

			If Not TypeOf ex Is OleDbException Then
				Return 0
			End If

			dbEx = DirectCast(ex, OleDbException)

			Return dbEx.Errors.Count
		End Function

		Public Function GetSchemaColumns(ByVal table As DbInfoTable) As DbInfoColumnCollection Implements IDbAccessHelper.GetSchemaColumns
			Dim openFlag As Boolean

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If

				Return getSchemaOleDbColumns(table)
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			Finally
				If openFlag Then
					Me._conn.Close()
				End If
			End Try
		End Function

		Public Function GetSchemaFunctions() As DbInfoFunctionCollection Implements IDbAccessHelper.GetSchemaFunctions
			Dim openFlag As Boolean

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If

				Return getSchemaOleDbFunctions()
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			Finally
				If openFlag Then
					Me._conn.Close()
				End If
			End Try
		End Function

		Public Function GetSchemaProcedures() As DbInfoProcedureCollection Implements IDbAccessHelper.GetSchemaProcedures
			Dim openFlag As Boolean

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If

				Return getSchemaOleDbProcedures()
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			Finally
				If openFlag Then
					Me._conn.Close()
				End If
			End Try
		End Function

		Public Function GetSchemaTables() As DbInfoTableCollection Implements IDbAccessHelper.GetSchemaTables
			Dim openFlag As Boolean

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If

				Return getSchemaOleDbTables()
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			Finally
				If openFlag Then
					Me._conn.Close()
				End If
			End Try
		End Function

		Public Function GetSchemaTable(tablename As String) As DbInfoTable Implements IDbAccessHelper.GetSchemaTable
			Dim openFlag As Boolean

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If

				Return getSchemaOleDbTable(tablename)
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
			If ErrorCount(ex) <= 0 Then
				Return False
			End If

			Dim ary() As String

			ary = Me.ErrorNumbers(ex)

			Return (Array.IndexOf(ary, errorNumber.ToString) >= 0)
		End Function

		''' <summary>
		''' 重複エラーが発生した例外に存在するか返す
		''' </summary>
		''' <param name="ex">対象となる例外</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Public Function HasSqlNativeErrorDuplicationPKey(ByVal ex As System.Exception) As Boolean Implements IDbAccessHelper.HasSqlNativeErrorDuplicationPKey
			Return hasSqlNativeErrorOleDbDuplicationPKey(ex)
		End Function

		''' <summary>
		''' タイムアウトエラーが発生した例外に存在するか返す
		''' </summary>
		''' <param name="ex">対象となる例外</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Public Function HasSqlNativeErrorTimtout(ByVal ex As System.Exception) As Boolean Implements IDbAccessHelper.HasSqlNativeErrorTimtout
			Return hasSqlNativeErrorOleDbTimtout(ex)
		End Function

		''' <summary>
		''' SQLプレースフォルダのマークを返す。
		''' </summary>
		''' <value></value>
		''' <returns>「？」</returns>
		''' <remarks>「？」1文字固定</remarks>
		Public ReadOnly Property PlaceholderMark() As String Implements IDbAccessHelper.PlaceholderMark
			Get
				Return "?"
			End Get
		End Property

		Public Function CnvStatmentParameterName(name As String) As String Implements IDbAccessHelper.CnvStatmentParameterName
			Return PlaceholderMark
		End Function

		''' <summary>
		''' ストアドのパラメータを取得する
		''' </summary>
		''' <param name="cmd">実行対象のDBコマンド</param>
		''' <remarks></remarks>
		Public Sub RefreshProcedureParameters(ByVal cmd As System.Data.IDbCommand) Implements IDbAccessHelper.RefreshProcedureParameters
			Try
				Dim openFlg As Boolean = False

				' コネクションが閉じてる場合は一旦接続する
				If cmd.Connection.State = ConnectionState.Closed Then
					cmd.Connection.Open()
					openFlg = True
				End If
				OleDbCommandBuilder.DeriveParameters(DirectCast(cmd, OleDbCommand))
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
			Dim dbEx As OleDbException

			ary = New ArrayList
			dbEx = DirectCast(ex, OleDbException)
			ary.Add(dbEx.ErrorCode.ToString)
			For Each err As OleDbError In dbEx.Errors
				ary.Add(err.NativeError.ToString)
			Next

			Return DirectCast(ary.ToArray(GetType(String)), String())
		End Function

#End Region

#Region " MustOverrides "

		''' <summary>
		''' スキーマからカラム情報を取得する
		''' </summary>
		''' <param name="table"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Friend MustOverride Function getSchemaOleDbColumns(ByVal table As DbInfoTable) As DbInfoColumnCollection

		Protected Friend MustOverride Function getSchemaOleDbFunctions() As DbInfoFunctionCollection

		Protected Friend MustOverride Function getSchemaOleDbProcedures() As DbInfoProcedureCollection

		Protected Friend MustOverride Function getSchemaOleDbTable(tablename As String) As DbInfoTable

		Protected Friend MustOverride Function getSchemaOleDbTables() As DbInfoTableCollection

		Protected Friend MustOverride Function hasSqlNativeErrorOleDbDuplicationPKey(ByVal ex As System.Exception) As Boolean

		Protected Friend MustOverride Function hasSqlNativeErrorOleDbTimtout(ByVal ex As System.Exception) As Boolean

#End Region

#Region " Methods "

		''' <summary>
		''' 列の最大桁数を返す
		''' </summary>
		''' <param name="row">行データ</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Overridable Function getColumnMaxLength(ByVal row As DataRow, ByRef length As Integer, ByRef scale As Integer) As Integer
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
		Protected Overridable Function getColumnLength(ByVal row As DataRow) As Object
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
		Protected Overridable Function getColumnPrecision(ByVal row As DataRow) As Object
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
		Protected Overridable Function getColumnScale(ByVal row As DataRow) As Object
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
		''' SQLServer は numeric は OleDbType には存在しないから Decimal にマップします。
		''' </remarks>
		Protected Overridable Function getColumnDbType(Of T)(ByVal row As DataRow) As T
			Dim typ As String

			If row Is Nothing Then
				Return Nothing
			End If

			typ = CStr(row.Item("DATA_TYPE"))

			If GetType(T).Name.Equals("OleDbType") Then
			End If

			Return DirectCast([Enum].Parse(GetType(T), typ, True), T)
		End Function

		''' <summary>
		''' 型がUniCodeか判定
		''' </summary>
		''' <param name="typ"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Overridable Function isUniCode(ByVal typ As String) As Boolean
			Select Case typ
				Case "VarWChar", "WChar", "BSTR", "LongVarWChar"
					Return True
				Case "202", "130", "8", "203"
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

		Public Function Restrictions() As DataTable
			Dim dt As DataTable
			Dim openFlag As Boolean

			Try
				If Me.Connection.State <> ConnectionState.Open Then
					Me.Connection.Open()
					openFlag = True
				End If

				dt = Me.Connection.GetSchema(DbMetaDataCollectionNames.Restrictions)

				Return dt
			Finally
				If openFlag Then
					Me.Connection.Close()
				End If
			End Try
		End Function

#End Region

	End Class

End Namespace
