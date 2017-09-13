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

		Private disposedValue As Boolean = False        ' 重複する呼び出しを検出するには

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

		''' <summary>
		''' スキーマ情報を取得する
		''' </summary>
		''' <param name="tableName">テーブル名</param>
		''' <returns></returns>
		Public Function FillSchema(ByVal tableName As String) As DataTable
			Const cSQL As String = "SELECT * FROM {0}"
			Dim ds As New DataSet
			Dim dt As DataTable
			Dim sql As String

			sql = String.Format(cSQL, tableName)
			Using cmd As IDbCommandSelect = myDba.CreateCommandSelect(sql)
				myDba.Adapter.SelectCommand = cmd.Command
				myDba.Adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey
				myDba.Adapter.FillSchema(ds, SchemaType.Mapped)

				dt = ds.Tables(0)
				dt.TableName = tableName
				Return dt
			End Using
		End Function

	End Class

End Namespace
