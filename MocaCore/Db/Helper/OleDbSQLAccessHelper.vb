
Imports System.Data.OleDb

Namespace Db.Helper

	Public Class OleDbSQLAccessHelper
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
			Dim dt As DataTable
			Dim results As DbInfoColumnCollection
			Dim info As DbInfoColumn

			results = New DbInfoColumnCollection()

			dt = Me.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Columns, New String() {table.Catalog, table.Schema, table.Name, Nothing})
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
				info.ColumnType = getColumnDbType(Of OleDbType)(dt.Rows(ii))
				results.Add(info.Name, info)
			Next

			Return results
		End Function

		Protected Friend Overrides Function getSchemaOleDbFunctions() As DbInfoFunctionCollection
			Dim dt As DataTable
			Dim results As DbInfoFunctionCollection
			Dim info As DbInfoFunction

			results = New DbInfoFunctionCollection()

			dt = Me.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Procedures, New String() {Me.myDba.Dbms.Setting.Database, Nothing, Nothing, Nothing})
			For ii As Integer = 0 To dt.Rows.Count - 1
				If Not CStr(dt.Rows(ii).Item("PROCEDURE_NAME")).EndsWith("0") Then
					Continue For
				End If
				If CStr(dt.Rows(ii).Item("PROCEDURE_SCHEMA")).Equals("sys") Then
					Continue For
				End If
				info = New DbInfoFunction( _
				CStr(dt.Rows(ii).Item("PROCEDURE_CATALOG")) _
				 , CStr(dt.Rows(ii).Item("PROCEDURE_SCHEMA")) _
				 , CStr(dt.Rows(ii).Item("PROCEDURE_NAME")).Split(";"c)(0) _
				 , CStr(dt.Rows(ii).Item("PROCEDURE_TYPE")))
				results.Add(info.ToString, info)
			Next

			Return results
		End Function

		Protected Friend Overrides Function getSchemaOleDbTable(tablename As String) As DbInfoTable
			Dim dt As DataTable
			Dim results As DbInfoTable

			results = Nothing

			dt = Me.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New String() {Me.myDba.Dbms.Setting.Database, Nothing, tablename, Nothing})
			For ii As Integer = 0 To dt.Rows.Count - 1
				results = New DbInfoTable( _
				 CStr(dt.Rows(ii).Item("TABLE_CATALOG")) _
				  , CStr(dt.Rows(ii).Item("TABLE_SCHEMA")) _
				  , CStr(dt.Rows(ii).Item("TABLE_NAME")) _
				  , CStr(dt.Rows(ii).Item("TABLE_TYPE")))

				results.Columns = GetSchemaColumns(results)
			Next

			Return results
		End Function

		Protected Friend Overrides Function getSchemaOleDbProcedures() As DbInfoProcedureCollection
			Dim dt As DataTable
			Dim results As DbInfoProcedureCollection
			Dim info As DbInfoProcedure

			results = New DbInfoProcedureCollection()

			dt = Me.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Procedures, New String() {Me.myDba.Dbms.Setting.Database, Nothing, Nothing, Nothing})
			For ii As Integer = 0 To dt.Rows.Count - 1
				If Not CStr(dt.Rows(ii).Item("PROCEDURE_NAME")).EndsWith("1") Then
					Continue For
				End If
				If CStr(dt.Rows(ii).Item("PROCEDURE_SCHEMA")).Equals("sys") Then
					Continue For
				End If
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

			dt = Me.Connection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New String() {Me.myDba.Dbms.Setting.Database, Nothing, Nothing, Nothing})
			For ii As Integer = 0 To dt.Rows.Count - 1
				If CStr(dt.Rows(ii).Item("TABLE_SCHEMA")).ToLower.Equals("sys") Then
					Continue For
				End If
				If CStr(dt.Rows(ii).Item("TABLE_SCHEMA")).ToUpper.Equals("INFORMATION_SCHEMA") Then
					Continue For
				End If

				info = New DbInfoTable( _
				   CStr(dt.Rows(ii).Item("TABLE_CATALOG")) _
				 , CStr(dt.Rows(ii).Item("TABLE_SCHEMA")) _
				 , CStr(dt.Rows(ii).Item("TABLE_NAME")) _
				 , CStr(dt.Rows(ii).Item("TABLE_TYPE")))
				results.Add(info.ToString, info)

				info.Columns = GetSchemaColumns(info)
			Next

			Return results
		End Function

		Protected Friend Overrides Function hasSqlNativeErrorOleDbDuplicationPKey(ex As System.Exception) As Boolean
			Return HasSqlNativeError(ex, OleDbErrorNumbers.DuplicationPKey)
		End Function

		Protected Friend Overrides Function hasSqlNativeErrorOleDbTimtout(ex As System.Exception) As Boolean
			Return HasSqlNativeError(ex, OleDbErrorNumbers.TimeOut)
		End Function

#End Region

	End Class

End Namespace
