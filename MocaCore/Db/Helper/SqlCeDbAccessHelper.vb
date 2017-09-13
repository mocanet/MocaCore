
'Imports System.Data.SqlServerCe

Namespace Db.Helper

#Region " Enum SqlCeErrorNumbers "

	''' <summary>ＳＱＬエラーコード</summary>
	<Flags()> _
	Public Enum SqlCeErrorNumbers

		''' <summary>重複エラーコード</summary>
		DuplicationPKey = 25016

		''' <summary>タイムアウトエラーコード</summary>
		''' <remarks>
		''' </remarks>
		LockTimeOut = 25090

	End Enum

#End Region

	''' <summary>
	''' SqlServerCeを使用したDBアクセス
	''' </summary>
	''' <remarks>
	''' データベース接続にSqlServerCeを使用するときは、当クラスを使用します。
	''' </remarks>
	Public Class SqlCeDbAccessHelper
		Inherits DbAccessHelper
		Implements IDbAccessHelper

        ''' <summary>ＳＱＬコネクション</summary>
        Private _conn As IDbConnection
        'Private _conn As SqlCeConnection

#Region " コンストラクタ "

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="dba">使用するデータベースアクセス</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal dba As IDao)
			MyBase.New(dba)
            _conn = Me.myDba.Connection
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
        Public Function CDbParameterName(name As String) As String Implements IDbAccessHelper.CDbParameterName
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
        Public Function ErrorCount(ex As System.Exception) As Integer Implements IDbAccessHelper.ErrorCount
            If ex.GetType.Name <> "SqlCeException" Then
                Return 0
            End If

            Dim value As Object
            value = ex.GetType.GetProperty("Errors").GetValue(ex, Nothing)

            Return value.GetType.GetProperty("Count").GetValue(value, Nothing)
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

            Dim ary As ArrayList = New ArrayList
            Dim value As Object
            value = ex.GetType.GetProperty("Errors").GetValue(ex, Nothing)

            For Each err As Object In value
                Dim number As Object
                number = err.GetType.GetProperty("NativeError").GetValue(err, Nothing)
                ary.Add(number.ToString)
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
        Public Function GetSchemaColumns(table As DbInfoTable) As DbInfoColumnCollection Implements IDbAccessHelper.GetSchemaColumns
			Const C_SQL As String = "SELECT C.* FROM INFORMATION_SCHEMA.COLUMNS C WHERE C.TABLE_NAME = @TABLE_NAME ORDER BY C.ORDINAL_POSITION"
			Dim dtSchema As DataTable
			Dim dt As DataTable
			Dim results As DbInfoColumnCollection
			Dim info As DbInfoColumn

			dtSchema = Nothing
			dt = Nothing
			results = New DbInfoColumnCollection()

			Try
				dtSchema = FillSchema(table.Name)

				Using cmd As IDbCommandSelect = Me.myDba.CreateCommandSelect(C_SQL)
					cmd.SetParameter("TABLE_NAME", table.Name)
					If Me.myDba.Execute(cmd) <= 0 Then
						Return results
					End If

					dt = cmd.ResultDataSet.Tables(0)
					For ii As Integer = 0 To dt.Rows.Count - 1
						info = New DbInfoColumn(
						   DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_CATALOG"), Nothing) _
						 , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_SCHEMA"), Nothing) _
						 , DbUtil.CStrValue(dt.Rows(ii).Item("COLUMN_NAME"), Nothing) _
						 , DbUtil.CStrValue(dt.Rows(ii).Item("DATA_TYPE"), Nothing))
						Dim length As Integer
						Dim scale As Integer
						info.MaxLength = getColumnMaxLength(dt.Rows(ii), length, scale)
						info.Precision = length
						info.Scale = scale
						info.UniCode = isUniCode(info.Typ)
						info.ColumnType = getColumnDbType(Of SqlDbType)(dt.Rows(ii))
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
				End Using

				Return results
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			End Try
		End Function

		''' <summary>
		''' 関数情報を取得する
		''' </summary>
		''' <returns></returns>
		''' <remarks>
		''' SQL Server Compact Edition では未サポート
		''' </remarks>
		''' <exception cref="NotSupportedException">
		''' 未サポート
		''' </exception>
		Public Function GetSchemaFunctions() As DbInfoFunctionCollection Implements IDbAccessHelper.GetSchemaFunctions
			Try
				Throw New NotSupportedException("関数は SQL Server Compact Edition では未サポートです")
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
			End Try
		End Function

		''' <summary>
		''' プロシージャ情報を取得する
		''' </summary>
		''' <returns></returns>
		''' <remarks>
		''' SQL Server Compact Edition では未サポート
		''' </remarks>
		''' <exception cref="NotSupportedException">
		''' 未サポート
		''' </exception>
		Public Function GetSchemaProcedures() As DbInfoProcedureCollection Implements IDbAccessHelper.GetSchemaProcedures
			Try
				Throw New NotSupportedException("ストアドプロシージャは SQL Server Compact Edition では未サポートです")
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
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
			Const C_SQL As String = "SELECT T.* FROM INFORMATION_SCHEMA.TABLES T WHERE T.TABLE_NAME = @TABLE_NAME"
			Dim results As DbInfoTable

			results = Nothing

			Try
				Using cmd As IDbCommandSelect = Me.myDba.CreateCommandSelect(C_SQL)
					cmd.SetParameter("TABLE_NAME", tablename)
					If Me.myDba.Execute(cmd) <= 0 Then
						Return results
					End If

					Dim dt As DataTable
					dt = cmd.ResultDataSet.Tables(0)
					For ii As Integer = 0 To dt.Rows.Count - 1
						results = New DbInfoTable( _
						  DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_CATALOG"), Nothing) _
						   , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_SCHEMA"), Nothing) _
						   , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_NAME"), Nothing) _
						   , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_TYPE"), Nothing))

						results.Columns = GetSchemaColumns(results)
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
			Const C_SQL As String = "SELECT T.* FROM INFORMATION_SCHEMA.TABLES T"
			Dim results As DbInfoTableCollection
			Dim info As DbInfoTable

			results = New DbInfoTableCollection()

			Try
				Using cmd As IDbCommandSelect = Me.myDba.CreateCommandSelect(C_SQL)
					If Me.myDba.Execute(cmd) <= 0 Then
						Return results
					End If

					Dim dt As DataTable
					dt = cmd.ResultDataSet.Tables(0)
					For ii As Integer = 0 To dt.Rows.Count - 1
						info = New DbInfoTable( _
						   DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_CATALOG"), Nothing) _
						 , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_SCHEMA"), Nothing) _
						 , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_NAME"), Nothing) _
						 , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_TYPE"), Nothing))
						results.Add(info.ToString, info)

						info.Columns = GetSchemaColumns(info)
					Next
				End Using

				Return results
			Catch ex As Exception
				Throw New DbAccessException(Me.targetDba, ex)
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
        Public Function HasSqlNativeError(ex As System.Exception, errorNumber As Long) As Boolean Implements IDbAccessHelper.HasSqlNativeError
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
        Public Function HasSqlNativeErrorDuplicationPKey(ex As System.Exception) As Boolean Implements IDbAccessHelper.HasSqlNativeErrorDuplicationPKey
			Return HasSqlNativeError(ex, SqlCeErrorNumbers.DuplicationPKey)
		End Function

		''' <summary>
		''' タイムアウトエラーが発生した例外に存在するか返す
		''' </summary>
		''' <param name="ex">対象となる例外</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Public Function HasSqlNativeErrorTimtout(ex As System.Exception) As Boolean Implements IDbAccessHelper.HasSqlNativeErrorTimtout
			Return HasSqlNativeError(ex, SqlCeErrorNumbers.LockTimeOut)
		End Function

		''' <summary>
		''' SQLプレースフォルダのマークを返す。
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property PlaceholderMark As String Implements IDbAccessHelper.PlaceholderMark
			Get
				Return "@"
			End Get
		End Property

		Public Function CnvStatmentParameterName(name As String) As String Implements IDbAccessHelper.CnvStatmentParameterName
			Return PlaceholderMark & name
		End Function

		''' <summary>
		''' プロシージャのパラメータ情報を取得する
		''' </summary>
		''' <param name="cmd"></param>
		''' <remarks>
		''' SQL Server Compact Edition では未サポート
		''' </remarks>
		''' <exception cref="NotSupportedException">
		''' 未サポート
		''' </exception>
		Public Sub RefreshProcedureParameters(cmd As System.Data.IDbCommand) Implements IDbAccessHelper.RefreshProcedureParameters
			Try
				Throw New NotSupportedException("ストアドプロシージャは SQL Server Compact Edition では未サポートです")
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

				dt = Nothing

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
