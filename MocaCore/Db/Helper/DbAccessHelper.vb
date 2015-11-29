Imports System.Data.Common

Namespace Db.Helper

	''' <summary>
	''' DBアクセスの各プロパイダーに対応したヘルパーの抽象クラス
	''' </summary>
	''' <remarks>
	''' 各DBベンダー毎に異なる部分を吸収する為のクラスです。<br/>
	''' </remarks>
	Public MustInherit Class DbAccessHelper
		Implements IDisposable

		''' <summary>元となるデータベースアクセスクラスインスタンス</summary>
		Protected targetDba As IDao
		''' <summary>当クラスで使用するデータベースアクセスクラスインスタンス</summary>
		Protected myDba As IDao

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="dba">使用するデータベースアクセス</param>
		''' <remarks></remarks>
		Public Sub New(ByVal dba As IDao)
			Me.targetDba = dba
			Me.myDba = New DbAccess(dba.Dbms)
		End Sub

#Region " IDisposable Support "

		Private disposedValue As Boolean = False		' 重複する呼び出しを検出するには

		' IDisposable
		Protected Overridable Sub Dispose(ByVal disposing As Boolean)
			If Not Me.disposedValue Then
				If disposing Then
					' TODO: 明示的に呼び出されたときにマネージ リソースを解放します
				End If

				' TODO: 共有のアンマネージ リソースを解放します
				If Me.myDba IsNot Nothing Then
					Me.myDba.Dispose()
				End If
			End If
			Me.disposedValue = True
		End Sub

		' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
		Public Sub Dispose() Implements IDisposable.Dispose
			' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
#End Region

	End Class

End Namespace
