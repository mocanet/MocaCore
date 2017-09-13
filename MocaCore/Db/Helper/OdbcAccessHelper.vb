
Imports System.Data.Common
Imports System.Data.Odbc

Namespace Db.Helper

    ''' <summary>ＳＱＬエラーコード</summary>
    Public Enum OdbcErrorNumbers
        '''' <summary>重複エラーコード</summary>
        'DuplicationPKey = 3621
        ''' <summary>タイムアウトエラーコード</summary>
        TimeOut = -2146232009
    End Enum

    ''' <summary>
    ''' ODBCを使用したDBアクセス
    ''' </summary>
    ''' <remarks>
    ''' データベース接続にODBCを使用するときは、当クラスを使用します。
    ''' </remarks>
    Public Class OdbcAccessHelper
        Inherits DbAccessHelper
        Implements IDbAccessHelper

        ''' <summary>ＳＱＬコネクション</summary>
        Private _conn As OdbcConnection

#Region " コンストラクタ "

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="dba">使用するデータベースアクセス</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal dba As IDao)
            MyBase.New(dba)
            _conn = DirectCast(Me.myDba.Connection, OdbcConnection)
        End Sub

#End Region

#Region " Implements "

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
            Dim dbEx As OdbcException

            If Not TypeOf ex Is OdbcException Then
                Return 0
            End If

            dbEx = DirectCast(ex, OdbcException)

            Return dbEx.Errors.Count
        End Function

		'TODO: 未実装
		Public Function GetSchemaColumns(ByVal table As DbInfoTable) As DbInfoColumnCollection Implements IDbAccessHelper.GetSchemaColumns
			Throw New NotImplementedException()
		End Function

		'TODO: 未実装
		Public Function GetSchemaFunctions() As DbInfoFunctionCollection Implements IDbAccessHelper.GetSchemaFunctions
			Throw New NotImplementedException()
		End Function

		'TODO: 未実装
		Public Function GetSchemaProcedures() As DbInfoProcedureCollection Implements IDbAccessHelper.GetSchemaProcedures
			Throw New NotImplementedException()
		End Function

		'TODO: 未実装
		Public Function GetSchemaTables() As DbInfoTableCollection Implements IDbAccessHelper.GetSchemaTables
			Throw New NotImplementedException()
		End Function

		'TODO: 未実装
		Public Function GetSchemaTable(tablename As String) As DbInfoTable Implements IDbAccessHelper.GetSchemaTable
			Throw New NotImplementedException()
		End Function

		'TODO: 未実装
		Public Function HasSqlNativeError(ByVal ex As System.Exception, ByVal errorNumber As Long) As Boolean Implements IDbAccessHelper.HasSqlNativeError
			Throw New NotImplementedException()
		End Function

		'TODO: 未実装
		Public Function HasSqlNativeErrorDuplicationPKey(ByVal ex As System.Exception) As Boolean Implements IDbAccessHelper.HasSqlNativeErrorDuplicationPKey
			Throw New NotImplementedException()
		End Function

		'TODO: 未実装
		Public Function HasSqlNativeErrorTimtout(ByVal ex As System.Exception) As Boolean Implements IDbAccessHelper.HasSqlNativeErrorTimtout
			Throw New NotImplementedException()
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
				OdbcCommandBuilder.DeriveParameters(DirectCast(cmd, OdbcCommand))
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
            Dim dbEx As OdbcException

            ary = New ArrayList
            dbEx = DirectCast(ex, OdbcException)
            ary.Add(dbEx.ErrorCode.ToString)
            For Each err As OdbcError In dbEx.Errors
                ary.Add(err.NativeError.ToString)
            Next

            Return DirectCast(ary.ToArray(GetType(String)), String())
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

#End Region

	End Class

End Namespace
