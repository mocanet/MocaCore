
Imports System.Globalization
Imports System.Reflection
Imports System.Resources
Imports System.Threading

Public Class CultureUtil

	''' <summary>
	''' 実行しているOSのカルチャ情報の取得
	''' </summary>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function PCCulture() As Globalization.CultureInfo
		Return My.Computer.Info.InstalledUICulture()
	End Function

    ''' <summary>
    ''' カルチャーを取得
    ''' </summary>
    ''' <param name="cultureNames"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
	Public Shared Function GetCulture(ByVal cultureNames() As String) As CultureInfo()
		Dim cultures As ArrayList

		cultures = New ArrayList

		For Each name As String In cultureNames
			Dim info As CultureInfo
			info = CultureInfo.GetCultureInfoByIetfLanguageTag(name)
			If info Is Nothing Then
				Continue For
			End If
			cultures.Add(info)
		Next

		Return DirectCast(cultures.ToArray(GetType(CultureInfo)), CultureInfo())
	End Function

    ''' <summary>
    ''' カルチャーを設定
    ''' </summary>
    ''' <param name="cultureName"></param>
    ''' <remarks></remarks>
	Public Shared Sub SetCulture(ByVal cultureName As String)
		If cultureName.Length = 0 Then
			cultureName = PCCulture().Name
			My.Settings.Save()
		End If
		Debug.WriteLine(cultureName)

		' カルチャーを設定
		Try
			Thread.CurrentThread.CurrentCulture = New CultureInfo(cultureName)
			Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture
			My.Application.ChangeCulture(cultureName)
			My.Application.ChangeUICulture(cultureName)
		Catch ex As Exception
			Throw New Exceptions.MocaRuntimeException(ex, "カルチャーを設定の設定に失敗しました。")
		End Try
	End Sub

End Class
