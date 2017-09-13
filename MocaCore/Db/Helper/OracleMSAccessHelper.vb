
Imports System.Data.Common
Imports System.Data.OracleClient

Namespace Db.Helper

#Region " OracleMSErrorNumbers "

	''' <summary>ＳＱＬエラーコード</summary>
	<Flags()> _
	Public Enum OracleMSErrorNumbers
		''' <summary>重複エラーコード</summary>
		DuplicationPKey = 1	'1452

		''' <summary>タイムアウトエラーコード</summary>
		''' <remarks>
		''' </remarks>
		LockTimeOut = 30006

		''' <summary>タイムアウトエラーコード(nowait)</summary>
		LockTimeOutNoWait = 54
	End Enum

#End Region

	''' <summary>
	''' MS Oracle ドライバー用ヘルパークラス
	''' </summary>
	''' <remarks></remarks>
	Public Class OracleMSAccessHelper
		Inherits DbAccessHelper
		Implements IDbAccessHelper

		''' <summary>ＳＱＬコネクション</summary>
		Private _conn As OracleConnection

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="dba">使用するデータベースアクセス</param>
		''' <remarks></remarks>
		Public Sub New(ByVal dba As IDao)
			MyBase.New(dba)
			_conn = DirectCast(Me.myDba.Connection, OracleConnection)
		End Sub

#End Region

#Region " Implements IDbAccessHelper "

		Public Function CDbParameterName(name As String) As String Implements IDbAccessHelper.CDbParameterName
			If Not name.StartsWith(PlaceholderMark) Then
				name = PlaceholderMark & name
			End If
			Return name
		End Function

		Public Function ErrorCount(ex As System.Exception) As Integer Implements IDbAccessHelper.ErrorCount
			Dim dbEx As OracleException

			If Not TypeOf ex Is OracleException Then
				Return 0
			End If

			dbEx = DirectCast(ex, OracleException)

			Return 1
		End Function

		Public Function ErrorNumbers(ex As System.Exception) As String() Implements IDbAccessHelper.ErrorNumbers
			If ErrorCount(ex) <= 0 Then
				Return Nothing
			End If

			Dim ary As ArrayList
			Dim dbEx As OracleException

			ary = New ArrayList
			dbEx = DirectCast(ex, OracleException)
			ary.Add(dbEx.Code.ToString)

			Return DirectCast(ary.ToArray(GetType(String)), String())
		End Function

		Public Function GetSchemaColumns(table As DbInfoTable) As DbInfoColumnCollection Implements IDbAccessHelper.GetSchemaColumns
			Dim dtSchema As DataTable
			Dim dt As DataTable
			Dim results As DbInfoColumnCollection
			Dim info As DbInfoColumn
			Dim openFlag As Boolean

			dtSchema = Nothing
			results = New DbInfoColumnCollection()

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If

				dtSchema = FillSchema(table.Name)

				dt = Me._conn.GetSchema("Columns", New String() {table.Catalog.ToUpper, table.Name, Nothing})
				For ii As Integer = 0 To dt.Rows.Count - 1
					info = New DbInfoColumn( _
					   CStr(dt.Rows(ii).Item("OWNER")) _
					 , CStr(dt.Rows(ii).Item("OWNER")) _
					 , CStr(dt.Rows(ii).Item("COLUMN_NAME")) _
					 , CStr(dt.Rows(ii).Item("DATATYPE")))
					Dim length As Integer
					Dim scale As Integer
					info.MaxLength = getColumnMaxLength(dt.Rows(ii), length, scale)
					info.Precision = length
					info.Scale = scale
					info.UniCode = isUniCode(info.Typ)
					info.ColumnType = getColumnDbType(Of OracleType)(dt.Rows(ii))
					info.DbColumn = dtSchema.Columns(info.Name)
#If net20 Then
					For Each item As DataColumn In dtSchema.PrimaryKey
						If item.ColumnName.Equals(info.Name) Then
							info.PrimaryKey = True
							Exit For
						End If
					Next
#Else
					info.PrimaryKey = dtSchema.PrimaryKey.Select(Function(x) x.ColumnName.Equals(info.Name)).Count.Equals(1)
#End If
					results.Add(info.Name, info)
				Next

				Return results
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			Finally
				If dtSchema IsNot Nothing Then
					dtSchema.Dispose()
				End If
				If openFlag Then
					Me._conn.Close()
				End If
			End Try
		End Function

		Public Function GetSchemaFunctions() As DbInfoFunctionCollection Implements IDbAccessHelper.GetSchemaFunctions
			Dim dt As DataTable
			Dim results As DbInfoFunctionCollection
			Dim info As DbInfoFunction
			Dim openFlag As Boolean

			results = New DbInfoFunctionCollection()

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If
				dt = Me._conn.GetSchema("Functions", New String() {Me.myDba.Dbms.Setting.User.ToUpper, Nothing})
				For ii As Integer = 0 To dt.Rows.Count - 1
					info = New DbInfoFunction( _
					CStr(dt.Rows(ii).Item("OWNER")) _
					 , CStr(dt.Rows(ii).Item("OWNER")) _
					 , CStr(dt.Rows(ii).Item("OBJECT_NAME")) _
					 , String.Empty)
					results.Add(info.ToString, info)
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

		Public Function GetSchemaProcedures() As DbInfoProcedureCollection Implements IDbAccessHelper.GetSchemaProcedures
			Dim dt As DataTable
			Dim results As DbInfoProcedureCollection
			Dim info As DbInfoProcedure
			Dim openFlag As Boolean

			results = New DbInfoProcedureCollection()

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If
				dt = Me._conn.GetSchema("Procedures", New String() {Me.myDba.Dbms.Setting.User.ToUpper, Nothing})
				For ii As Integer = 0 To dt.Rows.Count - 1
					info = New DbInfoProcedure( _
					CStr(dt.Rows(ii).Item("OWNER")) _
					 , CStr(dt.Rows(ii).Item("OWNER")) _
					 , CStr(dt.Rows(ii).Item("OBJECT_NAME")) _
					 , String.Empty)
					results.Add(info.ToString, info)
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
				dt = Me._conn.GetSchema("Tables", New String() {Me.myDba.Dbms.Setting.User.ToUpper, tablename})
				For ii As Integer = 0 To dt.Rows.Count - 1
					results = New DbInfoTable( _
					 CStr(dt.Rows(ii).Item("OWNER")) _
					  , CStr(dt.Rows(ii).Item("OWNER")) _
					  , CStr(dt.Rows(ii).Item("TABLE_NAME")) _
					  , CStr(dt.Rows(ii).Item("TYPE")))

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
				dt = Me._conn.GetSchema("Tables", New String() {Me.myDba.Dbms.Setting.User.ToUpper, Nothing})
				For ii As Integer = 0 To dt.Rows.Count - 1
					info = New DbInfoTable( _
					 CStr(dt.Rows(ii).Item("OWNER")) _
					  , CStr(dt.Rows(ii).Item("OWNER")) _
					  , CStr(dt.Rows(ii).Item("TABLE_NAME")) _
					  , CStr(dt.Rows(ii).Item("TYPE")))
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

		Public Function HasSqlNativeError(ex As System.Exception, errorNumber As Long) As Boolean Implements IDbAccessHelper.HasSqlNativeError
			If ErrorCount(ex) <= 0 Then
				Return False
			End If

			Dim ary() As String

			ary = Me.ErrorNumbers(ex)

			Return (Array.IndexOf(ary, errorNumber.ToString) >= 0)
		End Function

		Public Function HasSqlNativeErrorDuplicationPKey(ex As System.Exception) As Boolean Implements IDbAccessHelper.HasSqlNativeErrorDuplicationPKey
			Return HasSqlNativeError(ex, OracleMSErrorNumbers.DuplicationPKey)
		End Function

		Public Function HasSqlNativeErrorTimtout(ex As System.Exception) As Boolean Implements IDbAccessHelper.HasSqlNativeErrorTimtout
			Return (HasSqlNativeError(ex, OracleMSErrorNumbers.LockTimeOut) OrElse HasSqlNativeError(ex, OracleMSErrorNumbers.LockTimeOutNoWait))
		End Function

		Public ReadOnly Property PlaceholderMark As String Implements IDbAccessHelper.PlaceholderMark
			Get
				Return ":"
			End Get
		End Property

		Public Function CnvStatmentParameterName(name As String) As String Implements IDbAccessHelper.CnvStatmentParameterName
			Return PlaceholderMark & name
		End Function

		Public Sub RefreshProcedureParameters(cmd As System.Data.IDbCommand) Implements IDbAccessHelper.RefreshProcedureParameters
			Try
				Dim openFlg As Boolean = False

				' コネクションが閉じてる場合は一旦接続する
				If cmd.Connection.State = ConnectionState.Closed Then
					cmd.Connection.Open()
					openFlg = True
				End If
				OracleCommandBuilder.DeriveParameters(DirectCast(cmd, OracleCommand))
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
			Dim wlength As Object = getColumnLength(row)		' 桁数
			Dim wprecision As Object = getColumnPrecision(row)	' 精度（全体の桁数）
			Dim wscale As Object = getColumnScale(row)			' スケール（小数点以下の桁数）

			' 小数点がある
			If Not DBNull.Value.Equals(wscale) AndAlso Not wscale.Equals(0D) Then
				length = CInt(wprecision)
				scale = CInt(wscale)
				Return length + 1
			End If

			' 小数点がなく精度がある時
			If Not DBNull.Value.Equals(wprecision) AndAlso Not wprecision.Equals(0D) Then
				length = CInt(wprecision)
				scale = 0
				Return length
			End If

			' 小数点と精度がない時
			length = 0
			scale = 0
			Return CInt(wlength)
		End Function

		''' <summary>
		''' 列の桁数を返します。
		''' </summary>
		''' <param name="row">行データ</param>
		''' <returns>テーブル又は列が存在しないときや、桁数の指定が不要な型の時は DBNull.Value を返します。</returns>
		''' <remarks>
		''' バイナリ データ、文字データ、またはテキスト/イメージ データの最大長 (文字単位)。
		''' それ以外の場合は、NULL が返されます。
		''' </remarks>
		Protected Function getColumnLength(ByVal row As DataRow) As Object
			If row Is Nothing Then
				Return DBNull.Value
			End If

			Return row.Item("LENGTH")
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

			Return row.Item("PRECISION")
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

			Return row.Item("SCALE")
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

			typ = CStr(row.Item("DATATYPE"))

			If GetType(T).Name.Equals("OracleType") Then
				' NVARCHAR2 は OracleType には存在しないから NVarChar にマップする
				If typ.Equals("NVARCHAR2") Then
					typ = OracleType.NVarChar.ToString
				End If
				If typ.Equals("VARCHAR2") Then
					typ = OracleType.VarChar.ToString
				End If
                If typ.Equals("DATE") Then
                    typ = OracleType.DateTime.ToString
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
			Select Case typ.ToUpper
				Case "NVARCHAR2", "NCHAR"
					Return True
				Case Else
					Return False
			End Select
		End Function

#End Region

#Region " Debug "

		Public Function Restrictions() As DataTable
			Dim dt As DataTable
			Dim openFlag As Boolean

			Try
				If Me._conn.State <> ConnectionState.Open Then
					Me._conn.Open()
					openFlag = True
				End If

				dt = Me._conn.GetSchema(DbMetaDataCollectionNames.Restrictions)

				Return dt
			Finally
				If openFlag Then
					Me._conn.Close()
				End If
			End Try
		End Function

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
