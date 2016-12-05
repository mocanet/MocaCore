
Namespace Util

	''' <summary>
	''' ストップウォッチ
	''' </summary>
	''' <remarks>
	''' このクラスは、.NET Framework version 1.1 用です。<br/>
	''' .NET Framework version 2.0 では <see cref="System.Diagnostics.Stopwatch"/> が新しく追加されてます。<br/>
	''' 基本的には標準を使ってください。機能拡張したい時はこちらを使って機能拡張するのもありかと。
	''' </remarks>
	Public Class StopWatch

		''' <summary>開始時刻</summary>
		Private _sTime As DateTime
		''' <summary>終了時刻</summary>
		Private _eTime As DateTime

#Region " Constructor/DeConstructor "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			_sTime = Nothing
			_eTime = Nothing
		End Sub

#End Region

#Region " Properties "

		''' <summary>
		''' 開始時刻
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property StartTime() As DateTime
			Get
				Return _sTime
			End Get
		End Property

		''' <summary>
		''' 終了時刻
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public ReadOnly Property StopTime() As DateTime
			Get
				Return _eTime
			End Get
		End Property

		''' <summary>
		''' 計測した秒数を参照
		''' </summary>
		''' <value>計測結果秒数</value>
		''' <remarks>
		''' </remarks>
		Public ReadOnly Property ElapsedMilliseconds() As Double
			Get
				Return _eTime.Subtract(_sTime).TotalSeconds
			End Get
		End Property

#End Region

		''' <summary>
		''' スタート
		''' </summary>
		''' <remarks>
		''' </remarks>
		Public Sub Start()
			_sTime = Now
		End Sub

		''' <summary>
		''' ストップ
		''' </summary>
		''' <remarks>
		''' </remarks>
		Public Sub [Stop]()
			_eTime = Now
		End Sub

	End Class

End Namespace
