
Imports Moca.Db
Imports Moca.Db.CommandWrapper
Imports Moca.Di
Imports Moca.Aop

Namespace Db.Interceptor

	''' <summary>
	''' DBアクセス実行時のインターセプター抽象クラス
	''' </summary>
	''' <remarks></remarks>
	Public MustInherit Class AbstractDaoInterceptor
		Implements IMethodInterceptor

#Region " log4net "
		''' <summary>log4net logger</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)
#End Region

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			Dim injector As MocaInjector = New MocaInjector
			injector.Inject(Me)
		End Sub

#End Region

		Public Function Invoke(ByVal invocation As Moca.Aop.IMethodInvocation) As Object Implements Moca.Aop.IMethodInterceptor.Invoke
			Dim rc As Object = Nothing
			Dim dao As IDao

			dao = Nothing
			If TryCast(invocation.This, IDao) IsNot Nothing Then
				dao = DirectCast(invocation.This, IDao)
			End If

			Try
				executeBegin(dao)

				' 実処理実行
				rc = invocation.Proceed()
			Catch ex As Exception
				executeError(dao, ex)
				Throw ex
			Finally
				If dao IsNot Nothing Then
					executeEnd(dao)
					dao.ExecuteHistory = False
					dao.ExecuteUpdateHistory = False
				End If
			End Try

			Return rc
		End Function

		''' <summary>
		''' 実行前
		''' </summary>
		''' <param name="dao"></param>
		''' <remarks></remarks>
		Protected MustOverride Sub executeBegin(ByVal dao As IDao)

		''' <summary>
		''' 実行後
		''' </summary>
		''' <param name="dao"></param>
		''' <remarks>
		''' エラー発生時でも実行されます。
		''' </remarks>
		Protected MustOverride Sub executeEnd(ByVal dao As IDao)

		''' <summary>
		''' エラー発生時
		''' </summary>
		''' <param name="dao"></param>
		''' <remarks>
		''' </remarks>
		Protected MustOverride Sub executeError(ByVal dao As IDao, ByVal ex As Exception)

	End Class

End Namespace
