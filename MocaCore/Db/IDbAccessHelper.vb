
Namespace Db

	''' <summary>
	''' DBアクセスの各プロパイダーに対応したヘルパーのインタフェース
	''' </summary>
	''' <remarks>
	''' 各DBベンダー毎に異なる部分を吸収する為のインタフェースです。<br/>
	''' </remarks>
	Public Interface IDbAccessHelper
		Inherits IDisposable

		''' <summary>
		''' エラーの件数を返す
		''' </summary>
		''' <param name="ex">エラー件数を取得したい例外</param>
		''' <returns>エラー件数</returns>
		''' <remarks>
		''' </remarks>
		Function ErrorCount(ByVal ex As Exception) As Integer

		''' <summary>
		''' エラー番号を返す
		''' </summary>
		''' <param name="ex">エラー番号を取得したい例外</param>
		''' <returns>エラー番号配列</returns>
		''' <remarks>
		''' </remarks>
		Function ErrorNumbers(ByVal ex As Exception) As String()

		''' <summary>
		''' 指定されたエラー番号が発生した例外に存在するか返す
		''' </summary>
		''' <param name="ex">対象となる例外</param>
		''' <param name="errorNumber">エラー番号</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Function HasSqlNativeError(ByVal ex As Exception, ByVal errorNumber As Long) As Boolean

		''' <summary>
		''' 重複エラーが発生した例外に存在するか返す
		''' </summary>
		''' <param name="ex">対象となる例外</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Function HasSqlNativeErrorDuplicationPKey(ByVal ex As Exception) As Boolean

		''' <summary>
		''' タイムアウトエラーが発生した例外に存在するか返す
		''' </summary>
		''' <param name="ex">対象となる例外</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Function HasSqlNativeErrorTimtout(ByVal ex As Exception) As Boolean

		''' <summary>
		''' スキーマに存在するテーブル情報を取得する
		''' </summary>
		''' <param name="tablename">取得したいテーブル名</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSchemaTable(ByVal tablename As String) As DbInfoTable

		''' <summary>
		''' スキーマに存在するテーブル情報を取得する
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSchemaTables() As DbInfoTableCollection

		''' <summary>
		''' スキーマに存在するテーブル情報を取得する
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSchemaColumns(ByVal table As DbInfoTable) As DbInfoColumnCollection

		''' <summary>
		''' スキーマに存在するプロシージャ情報を取得する
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSchemaProcedures() As DbInfoProcedureCollection

		''' <summary>
		''' スキーマに存在する関数情報を取得する
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function GetSchemaFunctions() As DbInfoFunctionCollection

		''' <summary>
		''' ストアドのパラメータを取得する
		''' </summary>
		''' <param name="cmd">実行対象のDBコマンド</param>
		''' <remarks></remarks>
		Sub RefreshProcedureParameters(ByVal cmd As IDbCommand)

		''' <summary>
		''' SQLステータスのパラメータ名を変換する。
		''' </summary>
		''' <param name="name">パラメータ名</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CDbParameterName(ByVal name As String) As String

		''' <summary>
		''' SQLプレースフォルダのマークを返す。
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		ReadOnly Property PlaceholderMark() As String

	End Interface

End Namespace
