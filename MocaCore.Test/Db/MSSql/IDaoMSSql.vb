
Imports Moca.Db.Attr

Namespace Db

    ''' <summary>
	''' DaoMSSql データアクセスインタフェース
    ''' </summary>
    ''' <remarks></remarks>
    <Dao("MocaCore.Test.My.MySettings.MSSQL", GetType(Impl.DaoMSSql))>
    Public Interface IDaoMSSql

		Function Find() As IList(Of UserRow)

		Function Find2(ByVal row As UserRow) As IList(Of UserRow)

		Function Find3(ByVal row As UserRow) As String



		<Transaction()>
		Function Ins(ByVal row As UserRow) As Integer
		<Transaction()>
		Function Ins2(ByVal row As UserRow) As Integer
		<Transaction()>
		Function Ins2(ByVal rows As IList(Of UserRow)) As Integer
		<Transaction()>
		Function Ins3(ByVal row As UserRow) As Integer


		<Transaction()>
		Function Upd(ByVal row As UserRow) As Integer
		<Transaction()>
		Function Upd2(ByVal row As UserRow) As Integer
		<Transaction()>
		Function Upd2(ByVal rows As IList(Of UserRow)) As Integer
		<Transaction()>
		Function Upd3(ByVal row As UserRow) As Integer


		<Transaction()>
		Function Del(ByVal row As UserRow) As Integer
		<Transaction()>
		Function Del2(ByVal row As UserRow) As Integer
		<Transaction()>
		Function Del2(ByVal rows As IList(Of UserRow)) As Integer
		<Transaction()>
		Function Del3(ByVal row As UserRow) As Integer


		Function Procedure1() As IList(Of UserRow)

		Function Procedure2(ByVal row As UserRow) As String
		<Transaction()>
		Function Procedure3(ByVal row As UserRow) As Integer

	End Interface

End Namespace
