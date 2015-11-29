
Imports Moca.Exceptions

Namespace Db

	''' <summary>
	''' データベースアクセス関係の例外
	''' </summary>
	''' <remarks></remarks>
	Public Class DbAccessException
		Inherits MocaRuntimeException

		''' <summary>使用しているDBAccessインスタンス</summary>
		Protected useDBAccess As IDao

#Region " Constructor/DeConstructor "

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="useDBAccess">使用しているDBAccessインスタンス</param>
		''' <param name="Message">エラーメッセージ</param>
		''' <remarks>
		''' </remarks>
		''' -----------------------------------------------------------------------------
		Public Sub New(ByVal useDBAccess As IDao, ByVal Message As String)
			MyBase.New(Message)
			Me.useDBAccess = useDBAccess
		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="useDBAccess">使用しているDBAccessインスタンス</param>
		''' <param name="ex">例外インスタンス</param>
		''' <remarks>
		''' </remarks>
		''' -----------------------------------------------------------------------------
		Public Sub New(ByVal useDBAccess As IDao, ByVal ex As Exception)
			MyBase.New(ex)
			Me.useDBAccess = useDBAccess
		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="useDBAccess">使用しているDBAccessインスタンス</param>
		''' <param name="ex">例外インスタンス</param>
		''' <param name="Message">エラーメッセージ</param>
		''' <remarks>
		''' </remarks>
		''' -----------------------------------------------------------------------------
		Public Sub New(ByVal useDBAccess As IDbAccess, ByVal ex As Exception, ByVal Message As String)
			MyBase.New(ex, Message)
			Me.useDBAccess = useDBAccess
		End Sub

#End Region

		''' <summary>
		''' エラー番号たち
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetErrors() As String()
			If BaseException Is Nothing Then
				Return Nothing
			End If

			Return useDBAccess.Helper.ErrorNumbers(BaseException)
		End Function

		''' <summary>
		''' 重複エラーが発生した例外に存在するか返す
		''' </summary>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Public Function HasSqlNativeErrorDuplicationPKey() As Boolean
			If BaseException Is Nothing Then
				Return False
			End If

			Return useDBAccess.Helper.HasSqlNativeErrorDuplicationPKey(BaseException)
		End Function

		''' <summary>
		''' タイムアウトエラーが発生した例外に存在するか返す
		''' </summary>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Public Function HasSqlNativeErrorTimtout() As Boolean
			If BaseException Is Nothing Then
				Return False
			End If

			Return useDBAccess.Helper.HasSqlNativeErrorTimtout(BaseException)
		End Function

		''' <summary>
		''' 指定されたエラー番号が発生した例外に存在するか返す
		''' </summary>
		''' <param name="errorNumber">エラー番号</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Public Function HasSqlNativeError(ByVal errorNumber As Long) As Boolean
			If BaseException Is Nothing Then
				Return False
			End If

			Return HasSqlNativeError(BaseException, errorNumber)
		End Function

		''' <summary>
		''' 指定されたエラー番号が発生した例外に存在するか返す
		''' </summary>
		''' <param name="ex">例外</param>
		''' <param name="errorNumber">エラー番号</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Protected Function hasSqlNativeError(ByVal ex As Exception, ByVal errorNumber As Long) As Boolean
			Return useDBAccess.Helper.HasSqlNativeError(ex, errorNumber)
		End Function

	End Class

End Namespace
