
Imports System.Reflection
Imports System.Transactions
Imports Moca.Aop
Imports Moca.Attr

Namespace Db.Attr

	''' <summary>
	''' トランザクション属性
	''' </summary>
	''' <remarks>
	''' メソッド内の処理をトランザクションで括るときに指定する。
	''' </remarks>
	<AttributeUsage(AttributeTargets.Method, Inherited:=True)> _
	Public Class TransactionAttribute
		Inherits Attribute

		''' <summary>追加オプション</summary>
		Private _scopeOption As Object

		''' <summary>分離レベル</summary>
		Private _isolationLevel As Object

#Region " コンストラクタ "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			_scopeOption = Nothing
			_isolationLevel = Nothing
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New(ByVal scopeOption As TransactionScopeOption)
			Me.New()
			_scopeOption = scopeOption
		End Sub

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New(ByVal scopeOption As TransactionScopeOption, ByVal isolationLevel As Transactions.IsolationLevel)
			Me.New()
			_scopeOption = scopeOption
			_isolationLevel = isolationLevel
		End Sub

#End Region

		''' <summary>
		''' トランザクション用アスペクトを作成する
		''' </summary>
		''' <param name="method">メソッド</param>
		''' <returns>トランザクション用アスペクト</returns>
		''' <remarks></remarks>
		Public Shadows Function CreateAspect(ByVal method As MethodBase) As IAspect
			Dim pointcut As IPointcut
			Dim val As IAspect

			pointcut = New Pointcut(New String() {method.ToString})

			Select Case Config.MocaConfiguration.Section.TransactionType
				Case Config.TransactionType.Local
					val = New Aspect(New Tx.LocalTxInterceptor(_scopeOption, _isolationLevel), pointcut)

				Case Else
					val = New Aspect(New Tx.ScopeTxInterceptor(_scopeOption, _isolationLevel), pointcut)
					'TODO: スレッド処理にてもしトランザクションの挙動がおかしいときはこちらにしてみる。
					'val = New Aspect(GetType(Tx.TransactionInterceptor), pointcut)

			End Select

			Return val
		End Function

	End Class

End Namespace
