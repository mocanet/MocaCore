
Imports System.IO
Imports System.Text

Namespace Util

	''' <summary>
	''' VB.NET 便利メソッド集
	''' </summary>
	''' <remarks></remarks>
	Public Class VBUtil

		''' <summary>正規表現のメタ文字集</summary>
		Private Const C_REGEX_META As String = ".,^,$,[,],*,+,?,|,(,)"
		''' <summary>正規表現のメタ文字集配列</summary>
		Private Shared _regexMeata() As String = C_REGEX_META.Split(CChar(","))

		''' <summary>
		''' 現在展開されているディレクトリのカレントパスを返す
		''' </summary>
		''' <returns>カレントパス</returns>
		''' <remarks>
		''' VB6 以前でいう所の App.Path と同じです。
		''' </remarks>
		Public Shared Function AppPath() As String
			Return My.Application.Info.DirectoryPath
		End Function

		''' <summary>
		''' 現在展開されているディレクトリのカレントパスを返す
		''' </summary>
		''' <param name="value">カレントパスに付加するパス</param>
		''' <returns>カレントパス</returns>
		''' <remarks>
		''' VB6 以前でいう所の App.Path と同じですが、指定されたパスを付加した形で戻します。
		''' </remarks>
		Public Shared Function AppPath(ByVal value As String) As String
			Return Path.Combine(My.Application.Info.DirectoryPath, value)
		End Function

		''' <summary>
		''' 指定されたフォルダが存在するかを判定する
		''' </summary>
		''' <param name="path">対象となるフォルダ</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Public Shared Function ExistsDir(ByVal path As String) As Boolean
			Return ExistsDir(path, False)
		End Function

		''' <summary>
		''' 指定されたフォルダが存在するかを判定し、存在しない時は作成するかどうか指定できる
		''' </summary>
		''' <param name="path">対象となるフォルダ</param>
		''' <param name="autoMake">存在しないときの動作<br/>True:作成する、False:作成しない</param>
		''' <returns>True:存在する、False:存在しない</returns>
		''' <remarks>
		''' </remarks>
		Public Shared Function ExistsDir(ByVal path As String, ByVal autoMake As Boolean) As Boolean
			If Not Directory.Exists(path) Then
				If autoMake Then
					Directory.CreateDirectory(path)
				End If
			End If
			Return Directory.Exists(path)
		End Function

		''' <summary>
		''' 正規表現のメタ文字をエスケープする
		''' </summary>
		''' <param name="value">正規表現文字列</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function RegexMeataEscape(ByVal value As String) As String
			For Each meta As String In _regexMeata
				value = value.Replace(meta, "\" & meta)
			Next
			Return value
		End Function

		''' <summary>
		''' イメージをバイト型の配列に取得
		''' </summary>
		''' <param name="filePath"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetImageByte(ByVal filePath As String) As Byte()
			Return File.ReadAllBytes(filePath)
		End Function

		''' <summary>
		''' 定数値データを作成する為のConstantDataSetを作成する
		''' </summary>
		''' <param name="tableName">定数値テーブル名称</param>
		''' <param name="blankRow">ブランク行を追加するかどうか（デフォルト：作成）</param>
		''' <param name="blankValue">ブランク行の値（デフォルト：-1）</param>
		''' <returns></returns>
		''' <remarks>
		''' <c>blankRow</c> にTrueを設定すれば先頭へ空データを一行自動で追加します。<br/>
		''' 列情報は下記の通りです。<br/>
		''' <br/>
		''' <list type="table">
		'''  <item>
		'''   <term>Display</term>
		'''   <description>「値：名称」（Text &amp; ":" &amp; Value）にした文字列を保持します。</description>
		'''  </item>
		'''  <item>
		'''   <term>Text</term>
		'''   <description>名称を保持します。</description>
		'''  </item>
		'''  <item>
		'''   <term>Value</term>
		'''   <description>値を保持します。</description>
		'''  </item>
		'''  <item>
		'''   <term>ValueText</term>
		'''   <description>値の文字列を保持します。</description>
		'''  </item>
		''' </list>
		''' </remarks>
		Public Shared Function CreateConstantDataSet(ByVal tableName As String, Optional ByVal blankRow As Boolean = True, Optional ByVal blankValue As Object = -1) As ConstantDataSet
			Return New ConstantDataSet(tableName, blankRow, blankValue)
		End Function

		''' <summary>
		''' 列挙型の名称を返す
		''' </summary>
		''' <typeparam name="T">列挙型</typeparam>
		''' <param name="value">対象となる値</param>
		''' <returns>名称</returns>
		''' <remarks></remarks>
		Public Shared Function GetEnumName(Of T)(ByVal value As Object) As String
			Return [Enum].GetName(GetType(T), value)
		End Function

#Region "　LeftB メソッド　"

		''' <summary>
		''' 文字列の左端から指定したバイト数分の文字列を返します。
		''' </summary>
		''' <param name="stTarget">取り出す元になる文字列。</param>
		''' <param name="iByteSize">取り出すバイト数。</param>
		''' <returns>左端から指定されたバイト数分の文字列。</returns>
		Public Shared Function LeftB(ByVal stTarget As String, ByVal iByteSize As Integer) As String
			Return MidB(stTarget, 1, iByteSize)
		End Function

#End Region

#Region "　MidB メソッド (+1)　"

		' ''' <summary>
		' ''' 文字列の指定されたバイト位置以降のすべての文字列を返します。
		' ''' </summary>
		' ''' <param name="stTarget">取り出す元になる文字列。</param>
		' ''' <param name="iStart">取り出しを開始する位置。</param>
		' ''' <returns>指定されたバイト位置以降のすべての文字列。</returns>
		'Public Shared Function MidB(ByVal stTarget As String, ByVal iStart As Integer) As String
		'	Dim hEncoding As Encoding = Encoding.GetEncoding("Shift_JIS")
		'	Dim bBytes As Byte() = hEncoding.GetBytes(stTarget)

		'	Return hEncoding.GetString(bBytes, iStart - 1, bBytes.Length - iStart + 1)
		'End Function

		' ''' <summary>
		' ''' 文字列の指定されたバイト位置から、指定されたバイト数分の文字列を返します。
		' ''' </summary>
		' ''' <param name="stTarget">取り出す元になる文字列。</param>
		' ''' <param name="iStart">取り出しを開始する位置。</param>
		' ''' <param name="iByteSize">取り出すバイト数。</param>
		' ''' <returns>指定されたバイト位置から指定されたバイト数分の文字列。</returns>
		'Public Shared Function MidB _
		'(ByVal stTarget As String, ByVal iStart As Integer, ByVal iByteSize As Integer) As String
		'	Dim hEncoding As Encoding = Encoding.GetEncoding("Shift_JIS")
		'	Dim bBytes As Byte() = hEncoding.GetBytes(stTarget)

		'	Return hEncoding.GetString(bBytes, iStart - 1, iByteSize)
		'End Function

		''' <summary>
		''' 文字列の指定されたバイト位置以降のすべての文字列を返します。
		''' </summary>
		''' <param name="value">取り出す元になる文字列。</param>
		''' <param name="startPos">取り出しを開始する位置。</param>
		''' <returns>指定されたバイト位置以降のすべての文字列。</returns>
		Public Shared Function MidB(ByVal value As String, ByVal startPos As Integer) As String
			Dim enc As Encoding = Encoding.GetEncoding("Shift_JIS")
			Dim getLength As Integer = enc.GetByteCount(value) - startPos + 1
			Return MidB(value, startPos, getLength)
		End Function

		''' <summary>
		''' 文字列の指定されたバイト位置から、指定されたバイト数分の文字列を返します。
		''' </summary>
		''' <param name="value">取り出す元になる文字列。</param>
		''' <param name="startPos">取り出しを開始する位置。</param>
		''' <param name="getLength">取り出すバイト数。</param>
		''' <returns>指定されたバイト位置から指定されたバイト数分の文字列。</returns>
		Public Shared Function MidB(ByVal value As String, ByVal startPos As Integer, ByVal getLength As Integer) As String
			If value Is Nothing OrElse value.Length = 0 Then
				Return String.Empty
			End If

			Dim rc As String
			Dim enc As Encoding = Encoding.GetEncoding("Shift_JIS")
			Dim bytes As Byte() = enc.GetBytes(value)
			Dim len As Integer = enc.GetByteCount(value) - startPos + 1
			If getLength > len Then
				getLength = len
			End If

			'▼切り抜いた結果、最後の１バイトが全角文字の半分だった場合、その半分は切り捨てる。

			rc = enc.GetString(bytes, startPos - 1, getLength)
			Dim rcLength As Integer = enc.GetByteCount(rc)

			If getLength = rcLength - 1 Then
				Return rc.Substring(0, rc.Length - 1)
			End If

			Return rc
		End Function

#End Region

#Region "　RightB メソッド　"

		''' <summary>
		''' 文字列の右端から指定されたバイト数分の文字列を返します。
		''' </summary>
		''' <param name="stTarget">取り出す元になる文字列。</param>
		''' <param name="iByteSize">取り出すバイト数。</param>
		''' <returns>右端から指定されたバイト数分の文字列。</returns>
		Public Shared Function RightB(ByVal stTarget As String, ByVal iByteSize As Integer) As String
			Dim hEncoding As Encoding = Encoding.GetEncoding("Shift_JIS")
			Dim bBytes As Byte() = hEncoding.GetBytes(stTarget)

			Return hEncoding.GetString(bBytes, bBytes.Length - iByteSize, iByteSize)
		End Function

#End Region

#Region "　LenB メソッド　"

		''' <summary>
		''' 半角 1 バイト、全角 2 バイトとして、指定された文字列のバイト数を返します。
		''' </summary>
		''' <param name="stTarget">バイト数取得の対象となる文字列。</param>
		''' <returns>半角 1 バイト、全角 2 バイトでカウントされたバイト数。</returns>
		Public Shared Function LenB(ByVal stTarget As String) As Integer
			Return Encoding.GetEncoding("Shift_JIS").GetByteCount(stTarget)
		End Function

#End Region

		''' <summary>
		''' 文字列を文字コードを表す整数値に変換する
		''' </summary>
		''' <param name="targetValue">変換対象の文字列</param>
		''' <returns></returns>
		''' <remarks>
		''' </remarks>
		Public Shared Function CAsc(ByVal targetValue As String) As String
			Dim beforeData As String
			Dim afterData As String
			Dim chrValue As String
			Dim ii As Integer

			beforeData = targetValue
			afterData = ""
			If Len(beforeData) <> 0& Then
				For ii = 1 To Len(beforeData)
					chrValue = String.Empty
					'chrValue = Hex(Asc(Mid$(beforeData, ii, 1)))
					chrValue = CStr(Asc(Mid$(beforeData, ii, 1)))
					afterData = afterData & chrValue
				Next
			Else
				afterData = "0"
			End If

			Return afterData
		End Function

		Public Shared Function ByteCutString(ByVal str As String, ByVal cutByteLength As Integer) As String
			Dim srcB() As Byte
			Dim encB() As Byte
			Dim enc As Encoding
			Dim rc As String

			' 文字列をバイト配列に変換
			srcB = Encoding.Unicode.GetBytes(str)
			' UnicodeからシフトJISに変換
			enc = Encoding.GetEncoding(932)
			encB = Encoding.Convert(Encoding.Unicode, enc, srcB)

			Dim rcB(encB.Length - cutByteLength) As Byte
			Array.Copy(encB, rcB, rcB.Length)

			rc = Encoding.Unicode.GetString(Encoding.Convert(enc, Encoding.Unicode, rcB))

			Return rc.Substring(0, rc.Length - 1)
		End Function

		''' <summary>
		''' ShiftJISに変換できない文字が含まれているか返す
		''' </summary>
		''' <param name="val">対象の文字列</param>
		''' <returns>True:含まれていない、False:含まれている</returns>
		''' <remarks></remarks>
		Public Shared Function IsShiftJISOnlyText(ByVal val As String) As Boolean
			Dim encoderFallback As New EncoderExceptionFallback
			Dim decoderFallback As New DecoderExceptionFallback
			Dim sjis As Encoding = Encoding.GetEncoding("Shift_JIS", encoderFallback, decoderFallback)

			Try
				Dim bytes As Byte() = sjis.GetBytes(val)
			Catch ex As EncoderFallbackException
				Return False
			End Try
			Return True

		End Function

		''' <summary>
		''' イメージをバイトへ変換
		''' </summary>
		''' <param name="source"></param>
		''' <param name="format"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function CBytes(ByVal source As System.Drawing.Image, ByVal format As System.Drawing.Imaging.ImageFormat) As Byte()
			Dim blob() As Byte

			' ファイルストリームでイメージデータを読み込み
			Using tempStream As System.IO.MemoryStream = New System.IO.MemoryStream()
				' メモリストリーム(Byte[])にImageデータを保存
				source.Save(tempStream, format)
				' メモリストリームからByte[]データを取得
				blob = tempStream.ToArray()
			End Using

			Return blob
		End Function

	End Class

End Namespace
