
Namespace Db

	''' <summary>
	''' 
	''' </summary>
	''' <remarks></remarks>
	Public Class ExecuteReaderResult
		Implements ISQLStatementResult

#Region " Declare "

		''' <summary>
		''' DBをオープンしたかどうか
		''' </summary>
		''' <remarks></remarks>
		Private _dbOpen As Boolean

		''' <summary>
		''' Select結果
		''' </summary>
		''' <remarks></remarks>
		Private _reader As IDataReader

		Private _builder As New EntityBuilder

		Private _cmd As IDbCommand

#Region " Logging For Log4net "
		''' <summary>Logging For Log4net</summary>
		Private Shared ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)
#End Region
#End Region

#Region " コンストラクタ "

		''' <summary>
		''' コンストラクタ
		''' </summary>
		''' <param name="cmd">DBコマンド</param>
		''' <param name="dbOpen">DBオープン有無</param>
		''' <param name="reader">Select結果の<see cref="IDataReader"></see></param>
		''' <remarks></remarks>
		Public Sub New(ByVal cmd As IDbCommand, ByVal dbOpen As Boolean, ByVal reader As IDataReader)
			_cmd = cmd
			_dbOpen = dbOpen
			_reader = reader
			_builder = New EntityBuilder
		End Sub

#End Region
#Region "IDisposable Support"
		Private disposedValue As Boolean ' 重複する呼び出しを検出するには

		' IDisposable
		Protected Overridable Sub Dispose(disposing As Boolean)
			If Not Me.disposedValue Then
				If disposing Then
					' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
				End If

				' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
				' TODO: 大きなフィールドを null に設定します。
				If _reader IsNot Nothing AndAlso Not _reader.IsClosed Then
					_reader.Close()
					_reader = Nothing
				End If
				If _cmd IsNot Nothing Then
					If _dbOpen Then
						_cmd.Connection.Close()
					End If
					_cmd.Dispose()
					_cmd = Nothing
				End If
			End If
			Me.disposedValue = True
		End Sub

		' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
		'Protected Overrides Sub Finalize()
		'    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
		'    Dispose(False)
		'    MyBase.Finalize()
		'End Sub

		' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
		Public Sub Dispose() Implements IDisposable.Dispose
			' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
#End Region

#Region " Property "

		''' <summary>
		''' DBをオープンしたかどうか
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property IsDBOpen As Boolean Implements ISQLStatementResult.IsDBOpen
			Get
				Return _dbOpen
			End Get
		End Property

#End Region

        Public Function NextResult(Of T)() As IList Implements ISQLStatementResult.NextResult
            If Not _reader.NextResult() Then
                Return Nothing
            End If

            Return Me.Result(Of T)()
        End Function

        Public Function Result(Of T)() As IList Implements ISQLStatementResult.Result
            Return _builder.Create(Of T)(_reader)
        End Function

    End Class

End Namespace
