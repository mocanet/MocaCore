
Namespace Db.Helper

	''' <summary>
	''' ヘルパークラスのファクトリー
	''' </summary>
	''' <remarks></remarks>
	Public Class DbAccessHelperFactory

#Region " Declare "

		''' <summary>構成ファイルの接続文字列セクション又はDB接続文字列を管理</summary>
		Private _dbSetting As DbSetting

#End Region

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="dbSetting"></param>
		''' <remarks></remarks>
		Public Sub New(ByVal dbSetting As DbSetting)
			_dbSetting = dbSetting
		End Sub

#End Region

#Region " Methods "

		''' <summary>
		''' ヘルパークラス生成
		''' </summary>
		''' <param name="dba"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Create(ByVal dba As IDao) As IDbAccessHelper
            Dim asm As System.Reflection.Assembly
            Dim typ As Type

            Select Case _dbSetting.ProviderName
                Case "System.Data.SqlClient"
                    Return New SqlDbAccessHelper(dba)

                Case "System.Data.SqlServerCe.3.5", "System.Data.SqlServerCe.4.0"
                    asm = System.Reflection.Assembly.Load("MocaSQLCe")
                    typ = asm.GetType("Moca.Db.Helper.SqlCeDbAccessHelper")
                    Return CType(Util.ClassUtil.NewInstance(typ, New Object() {dba}), IDbAccessHelper)

                Case "Oracle.DataAccess.Client"
                    asm = System.Reflection.Assembly.Load("MocaOracle")
                    typ = asm.GetType("Moca.Db.Helper.OracleAccessHelper")
                    Return CType(Util.ClassUtil.NewInstance(typ, New Object() {dba}), IDbAccessHelper)

                Case "Oracle.ManagedDataAccess.Client"
                    asm = System.Reflection.Assembly.Load("MocaOracle")
                    typ = asm.GetType("Moca.Db.Helper.OracleManagedAccessHelper")
                    Return CType(Util.ClassUtil.NewInstance(typ, New Object() {dba}), IDbAccessHelper)

                Case "System.Data.OracleClient", "OraOLEDB.Oracle.1"
                    Return New OracleMSAccessHelper(dba)

                Case "System.Data.OleDb"
                    Select Case _dbSetting.OleDbProviderName
                        Case "SQLOLEDB"
                            Return New OleDbSQLAccessHelper(dba)

                        Case "MSDAORA"
                            Return New OleDbOraAccessHelper(dba)

                        Case Else
                            Return Nothing
                    End Select

                Case "System.Data.Odbc"
                    Return New OdbcAccessHelper(dba)

                Case Else
                    Return Nothing
            End Select
		End Function

#End Region

	End Class

End Namespace
