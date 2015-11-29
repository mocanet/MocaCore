
Imports System.Configuration

Namespace Config

	Public Enum TransactionType As Integer
		Scope
		Local
	End Enum

	''' <summary>
	''' app.config の moca セクション
	''' </summary>
	''' <remarks></remarks>
	Public Class MocaConfiguration

#Region " Declare "

		Private Const C_SECTION_NAME As String = "moca/settings"

		''' <summary>シングルトン用インスタンス</summary>
		Private Shared _instance As MocaConfiguration

		''' <summary>セクションハンドラー</summary>
		Private _mySection As MocaSectionHandler

		''' <summary>トランザクションタイプ</summary>
		Private _transactionType As TransactionElement

		Private _rootPath As String

#End Region

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Protected Sub New()
			_rootPath = String.Empty
			getConfig()
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Protected Sub New(ByVal rootPath As String)
			_rootPath = rootPath
			getConfig()
		End Sub

#End Region
#Region " Property "

		''' <summary>
		''' トランザクションタイプ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property TransactionType() As TransactionType
			Get
				If _transactionType Is Nothing Then
					Return Config.TransactionType.Local
				End If
				If _transactionType.Type = String.Empty Then
					Return Config.TransactionType.Local
				End If
				Try
					Return DirectCast([Enum].Parse(GetType(TransactionType), _transactionType.Type, True), TransactionType)
				Catch ex As Exception
					Throw New Moca.Exceptions.MocaRuntimeException(ex, "アプリケーション構成ファイルの指定に誤りがあります。(moka/transactionType)")
				End Try
			End Get
		End Property

#End Region
#Region " Method "

		''' <summary>
		''' セクション
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared ReadOnly Property Section(Optional ByVal rootPath As String = "") As MocaConfiguration
			Get
				If _instance Is Nothing Then
					_instance = New MocaConfiguration(rootPath)
					_instance.getConfig()
				End If
				Return _instance
			End Get
		End Property

		''' <summary>
		''' アプリケーション構成ファイルを取得する
		''' </summary>
		''' <remarks></remarks>
		Protected Sub getConfig()
			_getMySection()

			' トランザクション情報
			_transactionType = _getTransactionType()
		End Sub

		''' <summary>
		''' アプリケーション構成ファイルからカスタムセクションを取得する
		''' </summary>
		''' <remarks></remarks>
		Private Sub _getMySection()
			_mySection = DirectCast(ConfigurationManager.GetSection(C_SECTION_NAME), MocaSectionHandler)
		End Sub

		''' <summary>
		''' トランザクションタイプを取得
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function _getTransactionType() As TransactionElement
			If _mySection Is Nothing Then
				Return Nothing
			End If

			Return _mySection.Transaction
		End Function

#End Region

	End Class

End Namespace
