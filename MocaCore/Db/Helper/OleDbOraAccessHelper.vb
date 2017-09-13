
Imports System.Data.Common
Imports System.Data.OleDb

Namespace Db.Helper

#Region " OleDbOracleErrorNumbers "

	''' <summary>ＳＱＬエラーコード</summary>
	<Flags()> _
	Public Enum OleDbOracleErrorNumbers
		''' <summary>重複エラーコード</summary>
		DuplicationPKey = 1

		''' <summary>タイムアウトエラーコード</summary>
		''' <remarks>
		''' </remarks>
		LockTimeOut = -2147467259

		''' <summary>タイムアウトエラーコード(nowait)</summary>
		LockTimeOutNoWait = -2147467259

		''' <summary>
		''' 指定した値は、列またはテーブルの整合性制約に違反しました。
		''' </summary>
		''' <remarks></remarks>
		TableCompatibilityRestrictions = -2147217873
	End Enum

#End Region

	''' <summary>
	''' 
	''' </summary>
	''' <remarks>
	''' スキーマ コレクションについて<br/>
	''' http://msdn.microsoft.com/ja-jp/library/vstudio/ms254969(v=vs.80).aspx
	''' </remarks>
	Public Class OleDbOraAccessHelper
		Inherits OleDbAccessHelper

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="dba">使用するデータベースアクセス</param>
		''' <remarks></remarks>
		Public Sub New(ByVal dba As IDao)
			MyBase.New(dba)
		End Sub

#End Region

#Region " Overrides "

		Protected Friend Overrides Function getSchemaOleDbColumns(table As DbInfoTable) As DbInfoColumnCollection
			Dim dtSchema As DataTable
			Dim dt As DataTable
			Dim results As DbInfoColumnCollection
			Dim info As DbInfoColumn

			dtSchema = Nothing
			dt = Nothing
			results = New DbInfoColumnCollection()

			Try
				dtSchema = FillSchema(table.Name)

				dt = Me.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New Object() {Nothing, table.Schema, """" & table.Name & """"})
				For ii As Integer = 0 To dt.Rows.Count - 1
					info = New DbInfoColumn(
					   DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_CATALOG")) _
					 , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_SCHEMA")) _
					 , DbUtil.CStrValue(dt.Rows(ii).Item("COLUMN_NAME")) _
					 , DbUtil.CStrValue(dt.Rows(ii).Item("DATA_TYPE")))
					Dim length As Integer
					Dim scale As Integer
					info.MaxLength = getColumnMaxLength(dt.Rows(ii), length, scale)
					info.Precision = length
					info.Scale = scale
					info.UniCode = isUniCode(info.Typ)
					info.ColumnType = getColumnDbType(Of OleDbType)(dt.Rows(ii))
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
			Finally
				If dtSchema IsNot Nothing Then
					dtSchema.Dispose()
				End If
				If dt IsNot Nothing Then
					dt.Dispose()
				End If
			End Try
		End Function

		Protected Friend Overrides Function getSchemaOleDbFunctions() As DbInfoFunctionCollection
			Throw New NotSupportedException("関数は OLE DB (MSDAORA) では未サポートです")
		End Function

		Protected Friend Overrides Function getSchemaOleDbTable(tablename As String) As DbInfoTable
			Dim dt As DataTable
			Dim results As DbInfoTable

			results = Nothing

			dt = Me.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Me.myDba.Dbms.Setting.User.ToUpper, """" & tablename & """"})
			For ii As Integer = 0 To dt.Rows.Count - 1
				results = New DbInfoTable( _
				 DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_CATALOG")) _
				  , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_SCHEMA")) _
				  , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_NAME")) _
				  , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_TYPE")))

				results.Columns = GetSchemaColumns(results)
			Next

			Return results
		End Function

		Protected Friend Overrides Function getSchemaOleDbProcedures() As DbInfoProcedureCollection
			Dim dt As DataTable
			Dim results As DbInfoProcedureCollection
			Dim info As DbInfoProcedure

			results = New DbInfoProcedureCollection()

			dt = Me.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Procedures, New Object() {Nothing, Me.myDba.Dbms.Setting.User.ToUpper})
			For ii As Integer = 0 To dt.Rows.Count - 1
				info = New DbInfoProcedure( _
				DbUtil.CStrValue(dt.Rows(ii).Item("PROCEDURE_CATALOG")) _
				 , DbUtil.CStrValue(dt.Rows(ii).Item("PROCEDURE_SCHEMA")) _
				 , DbUtil.CStrValue(dt.Rows(ii).Item("PROCEDURE_NAME")).Split(";"c)(0) _
				 , DbUtil.CStrValue(dt.Rows(ii).Item("PROCEDURE_TYPE")))
				results.Add(info.ToString, info)
				Debug.Print(info.ToString)
			Next

			Return results
		End Function

		Protected Friend Overrides Function getSchemaOleDbTables() As DbInfoTableCollection
			Dim dt As DataTable
			Dim results As DbInfoTableCollection
			Dim info As DbInfoTable

			results = New DbInfoTableCollection()

			dt = Me.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Me.myDba.Dbms.Setting.User.ToUpper})
			For ii As Integer = 0 To dt.Rows.Count - 1
				If CStr(dt.Rows(ii).Item("TABLE_NAME")).StartsWith("BIN$") Then
					Continue For
				End If
				info = New DbInfoTable( _
				   DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_CATALOG")) _
				 , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_SCHEMA")) _
				 , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_NAME")) _
				 , DbUtil.CStrValue(dt.Rows(ii).Item("TABLE_TYPE")))
				results.Add(info.ToString, info)

				info.Columns = GetSchemaColumns(info)
			Next

			Return results
		End Function

		Protected Overrides Function getColumnMaxLength(row As System.Data.DataRow, ByRef length As Integer, ByRef scale As Integer) As Integer
			Dim wlength As Object = getColumnLength(row)		' 桁数
			Dim wprecision As Object = getColumnPrecision(row)	' 精度（全体の桁数）
			Dim wscale As Object = getColumnScale(row)			' スケール（小数点以下の桁数）

			' 小数点がある
			If Not DBNull.Value.Equals(wscale) AndAlso Not wscale.Equals(0S) Then
				length = CInt(wprecision)
				scale = CInt(wscale)
				Return length + 1
			End If

			' 小数点がなく精度がある時
			If Not DBNull.Value.Equals(wprecision) AndAlso Not wprecision.Equals(0S) Then
				length = CInt(wprecision)
				scale = 0
				Return length
			End If

			' 小数点と精度がない時
			length = 0
			scale = 0
			Return CInt(wlength)
		End Function

		Protected Friend Overrides Function hasSqlNativeErrorOleDbDuplicationPKey(ex As System.Exception) As Boolean
			Return HasSqlNativeError(ex, OleDbOracleErrorNumbers.TableCompatibilityRestrictions)
		End Function

		Protected Friend Overrides Function hasSqlNativeErrorOleDbTimtout(ex As System.Exception) As Boolean
			Return (HasSqlNativeError(ex, OleDbOracleErrorNumbers.LockTimeOut) OrElse HasSqlNativeError(ex, OleDbOracleErrorNumbers.LockTimeOutNoWait))
		End Function

#End Region

	End Class

End Namespace
