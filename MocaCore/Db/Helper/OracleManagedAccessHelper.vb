﻿
Imports System.Data.Common

Namespace Db.Helper

    ''' <summary>
    ''' Oracle 純正ドライバー用ヘルパークラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class OracleManagedAccessHelper
        Inherits DbAccessHelper
        Implements IDbAccessHelper

        ''' <summary>ＳＱＬコネクション</summary>
        Private _conn As IDbConnection

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

        Public Function CDbParameterName(ByVal name As String) As String Implements IDbAccessHelper.CDbParameterName
            If Not name.StartsWith(PlaceholderMark) Then
                name = PlaceholderMark & name
            End If
            Return name
        End Function

        Public Function ErrorCount(ByVal ex As System.Exception) As Integer Implements IDbAccessHelper.ErrorCount
            If ex.GetType.Name <> "OracleException" Then
                Return 0
            End If

            Dim value As Object
            value = ex.GetType.GetProperty("Errors").GetValue(ex, Nothing)

            Return value.GetType.GetProperty("Count").GetValue(value, Nothing)
        End Function

        Public Function GetSchemaColumns(ByVal table As DbInfoTable) As DbInfoColumnCollection Implements IDbAccessHelper.GetSchemaColumns
			Dim dtSchema As DataTable
			Dim dt As DataTable
			Dim results As DbInfoColumnCollection
            Dim info As DbInfoColumn
            Dim openFlag As Boolean

			dtSchema = Nothing
			dt = Nothing
			results = New DbInfoColumnCollection()

			Try
                If Me._conn.State <> ConnectionState.Open Then
                    Me._conn.Open()
                    openFlag = True
                End If

				dtSchema = FillSchema(table.Name)

				dt = getSchema("Columns", New String() {table.Catalog, table.Name, Nothing})
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
                    info.ColumnType = getColumnDbType(dt.Rows(ii))
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
				If dt IsNot Nothing Then
					dt.Dispose()
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
                dt = getSchema("Functions", New String() {Me.myDba.Dbms.Setting.User, Nothing})
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
                dt = getSchema("Procedures", New String() {Me.myDba.Dbms.Setting.User, Nothing})
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
                dt = getSchema("Tables", New String() {Me.myDba.Dbms.Setting.User, Nothing})
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
                dt = getSchema("Tables", New String() {Me.myDba.Dbms.Setting.User, tablename})
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

        Public Function HasSqlNativeError(ByVal ex As System.Exception, ByVal errorNumber As Long) As Boolean Implements IDbAccessHelper.HasSqlNativeError
            If ErrorCount(ex) <= 0 Then
                Return False
            End If

            Dim ary() As String

            ary = Me.ErrorNumbers(ex)

            Return (Array.IndexOf(ary, errorNumber.ToString) >= 0)
        End Function

        Public Function HasSqlNativeErrorDuplicationPKey(ByVal ex As System.Exception) As Boolean Implements IDbAccessHelper.HasSqlNativeErrorDuplicationPKey
            Return HasSqlNativeError(ex, OracleErrorNumbers.DuplicationPKey)
        End Function

        Public Function HasSqlNativeErrorTimtout(ByVal ex As System.Exception) As Boolean Implements IDbAccessHelper.HasSqlNativeErrorTimtout
            Return HasSqlNativeError(ex, OracleErrorNumbers.LockTimeOut)
        End Function

        Public ReadOnly Property PlaceholderMark() As String Implements IDbAccessHelper.PlaceholderMark
            Get
                Return ":"
            End Get
        End Property

		Public Function CnvStatmentParameterName(name As String) As String Implements IDbAccessHelper.CnvStatmentParameterName
			Return PlaceholderMark & name
		End Function

		Public Sub RefreshProcedureParameters(ByVal cmd As System.Data.IDbCommand) Implements IDbAccessHelper.RefreshProcedureParameters
			Try
				Dim openFlg As Boolean = False

				' コネクションが閉じてる場合は一旦接続する
				If cmd.Connection.State = ConnectionState.Closed Then
					cmd.Connection.Open()
					openFlg = True
				End If

				Dim typ As Type
				typ = _conn.GetType.Assembly.GetType("Oracle.ManagedDataAccess.Client.OracleCommandBuilder")
				typ.InvokeMember("DeriveParameters", Reflection.BindingFlags.InvokeMethod, Nothing, Nothing, New Object() {cmd})

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

		Public Function ErrorNumbers(ex As System.Exception) As String() Implements IDbAccessHelper.ErrorNumbers
            If ErrorCount(ex) <= 0 Then
                Return Nothing
            End If

            Dim ary As ArrayList = New ArrayList
            Dim value As Object
            value = ex.GetType.GetProperty("Errors").GetValue(ex, Nothing)

            For Each err As Object In value
                Dim number As Object
                number = err.GetType.GetProperty("Number").GetValue(err, Nothing)
                ary.Add(number.ToString)
            Next

            Return DirectCast(ary.ToArray(GetType(String)), String())
        End Function

        Public Function QuotationMarks(name As String) As String Implements IDbAccessHelper.QuotationMarks
            Return name
        End Function

        Public Function CreateDataAdapter() As IDbDataAdapter Implements IDbAccessHelper.CreateDataAdapter
            Return Util.ClassUtil.NewInstance(_conn.GetType.Assembly.GetType("Oracle.ManagedDataAccess.Client.OracleDataAdapter"))
        End Function

#End Region
#Region " Methods "

        ''' <summary>
        ''' 列の最大桁数を返す
        ''' </summary>
        ''' <param name="row">行データ</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function getColumnMaxLength(ByVal row As DataRow, ByRef length As Integer, ByRef scale As Integer) As Integer
            Dim wlength As Object = getColumnLength(row)        ' 桁数
            Dim wprecision As Object = getColumnPrecision(row)  ' 精度（全体の桁数）
            Dim wscale As Object = getColumnScale(row)          ' スケール（小数点以下の桁数）

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

            Dim obj As Object
            obj = row.Item("LENGTHINCHARS")

            If obj.Equals(0D) Then
                obj = row.Item("LENGTH")
            End If

            Return obj
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
        ''' <param name="row">行データ</param>
        ''' <returns>テーブル又は列が存在しないときは DBNull.Value を返します。</returns>
        ''' <remarks>
        ''' SQLServer は numeric は SqlDbType には存在しないから Decimal にマップします。
        ''' </remarks>
        Protected Function getColumnDbType(ByVal row As DataRow) As Object
            Dim typ As String

            If row Is Nothing Then
                Return Nothing
            End If

            typ = CStr(row.Item("DATATYPE"))

            ' NUMBER は OracleDbType には存在しないから Decimal にマップする
            If typ.ToUpper.Equals("NUMBER") Then
                typ = "Decimal"
            End If

            Return [Enum].Parse(_conn.GetType.Assembly.GetType("Oracle.ManagedDataAccess.Client.OracleDbType"), typ, True)
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

        Protected Function getSchema(ByVal collectionName As String, Optional ByVal args() As String = Nothing) As DataTable
            Dim method As System.Reflection.MethodInfo
            Dim params() As Object

            If args Is Nothing Then
                Dim typParam As Type() = {Type.GetType("System.String")}
                method = _conn.GetType.GetMethod("GetSchema", typParam)
                params = New Object() {collectionName}
            Else
                Dim typParam As Type() = {Type.GetType("System.String"), Type.GetType("System.String[]")}
                method = _conn.GetType.GetMethod("GetSchema", typParam)
                params = New Object() {collectionName, args}
            End If

            Return method.Invoke(_conn, params)
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

                dt = getSchema(DbMetaDataCollectionNames.Restrictions)

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

                dt = getSchema("MetaDataCollections")

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
