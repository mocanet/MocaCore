
Imports System.Text
Imports System.Reflection
Imports System.Data

Imports Moca.Db.Attr
Imports Moca.Util

Namespace Db

	Public Enum SortDirectionValue As Integer
		ASC = 0
		DESC = 1
	End Enum

	''' <summary>
	''' データベースアクセス周りで使用するユーティリティメソッド集
	''' </summary>
	''' <remarks></remarks>
	Public Class DbUtil

		''' <summary>SQL のソート構文テンプレート</summary>
		Private Const C_ORDER_BY As String = " ORDER BY{0}"

		''' <summary>SQL のソート構文テンプレート</summary>
		Private Const C_ORDER_BY_COL As String = " [{0}] {1}"

		''' <summary>
		''' DbCommandインスタンスを終了する
		''' </summary>
		''' <param name="cmd"></param>
		''' <remarks></remarks>
		Public Shared Sub CommandDispose(ByVal cmd As IDbCommandSql)
			If cmd Is Nothing Then
				Exit Sub
			End If
			cmd.Dispose()
		End Sub

		'Public Shared Function CNull(ByVal Value As DBNull) As Object
		'	Return Nothing
		'End Function

		''' <summary>
		''' Null変換
		''' </summary>
		''' <param name="Value">検証値</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' 空の場合は、DBNull.Value を返します。
		''' 文字列が空の場合は、DBNull.Value を返します。
		''' </remarks>
		Public Shared Function CNull(ByVal value As Object, Optional ByVal emptyDataEqDBNull As Boolean = False) As Object
			If value Is Nothing Then
				Return DBNull.Value
			End If

			If TypeOf value Is String Then
				If CStr(value).Trim().Length = 0 Then
					If emptyDataEqDBNull Then
						Return DBNull.Value
					Else
						Return value
					End If
				Else
					Return value
				End If
			End If

			If IsDBNull(value) Then
				value = Nothing
			End If

			Return value
		End Function

		Public Shared Function CNothing(ByRef value As Object) As Object
			If IsDBNull(value) Then
				value = Nothing
			End If

			If value Is Nothing Then
				Return Nothing
			End If
			If TypeOf value Is String Then
				If CStr(value).Trim().Length = 0 Then
					Return Nothing
				End If
			End If
			Return value
		End Function

		Public Shared Function CNothing(ByRef value As Integer) As Object
			If value = 0 Then
				Return Nothing
			End If
			Return value
		End Function

		''' <summary>
		''' Null変換
		''' </summary>
		''' <param name="Value">検証値</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' 空の場合は、DBNull.Value を返します。
		''' 0の場合は、DBNull.Value を返します。
		''' </remarks>
		Public Shared Function CNull(ByVal Value As Integer) As Object
			If Value = 0 Then
				Return DBNull.Value
			Else
				Return Value
			End If
		End Function

		''' <summary>
		''' Null変換
		''' </summary>
		''' <param name="Value">検証値</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' 初期値（Date.MinValue）の時は、Nothingを返す
		''' </remarks>
		Public Shared Function CDateValue(ByVal Value As Date) As Object
			If Value.Equals(Date.MinValue) Then
				Return Nothing
			End If
			Return Value
		End Function

		''' <summary>
		''' DBから取得したデータがInteger値の場合の変換
		''' </summary>
		''' <param name="Value">対象値</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' Nullの場合は、０に変換する
		''' </remarks>
		Public Shared Function CIntValue(ByVal Value As Object) As Integer
			If Value Is DBNull.Value Then
				Return 0
			End If
			If Value.ToString.Length = 0 Then
				Return 0
			End If
			Return CInt(Value)
		End Function

		''' <summary>
		''' DBから取得したデータがLong値の場合の変換
		''' </summary>
		''' <param name="Value">対象値</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' Nullの場合は、０に変換する
		''' </remarks>
		Public Shared Function CLngValue(ByVal Value As Object) As Long
			If Value Is DBNull.Value Then
				Return 0
			Else
				Return CLng(Value)
			End If
		End Function

		''' <summary>
		''' DBから取得したデータがSingle値の場合の変換
		''' </summary>
		''' <param name="Value">対象値</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' Nullの場合は、０に変換する
		''' </remarks>
		Public Shared Function CSngValue(ByVal Value As Object, Optional ByVal defaultValue As Single = 0) As Single
			If Value Is DBNull.Value Then
				Return defaultValue
			Else
				Return CSng(Value)
			End If
		End Function

		''' <summary>
		''' DBから取得したデータがString値の場合の変換
		''' </summary>
		''' <param name="Value">対象値</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' Nullの場合は、String.Emptyに変換する
		''' </remarks>
		Public Shared Function CStrValue(ByVal Value As Object, Optional ByVal defaultValue As String = "") As String
			If Value Is DBNull.Value Then
				Return defaultValue
			Else
				Return CStr(Value)
			End If
		End Function

		''' <summary>
		''' DBから取得したデータがString値の場合の変換（Trimあり）
		''' </summary>
		''' <param name="Value">対象値</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' Nullの場合は、String.Emptyに変換する
		''' </remarks>
		Public Shared Function CStrTrimValue(ByVal Value As Object) As String
			Return CStrValue(Value).Trim()
		End Function

		''' <summary>
		''' DBから取得したデータが金額の場合の変換
		''' </summary>
		''' <param name="Value">対象値</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' Nullの場合は、String.Emptyに変換する。
		''' 3桁のカンマ区切り文字列として返します。（"###,###,###,###"）
		''' </remarks>
		Public Shared Function CMoneyValue(ByVal Value As Object) As String
			Return CMoneyValue(Value, "###,###,###,###")
		End Function

		''' <summary>
		''' DBから取得したデータが金額の場合の変換
		''' </summary>
		''' <param name="Value">対象値</param>
		''' <param name="formatString"></param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' Nullの場合は、String.Emptyに変換する。
		''' 指定されたフォーマット文字列に変換して返します。
		''' </remarks>
		Public Shared Function CMoneyValue(ByVal Value As Object, ByVal formatString As String) As String
			Dim strValue As String

			If Value Is DBNull.Value Then
				Return String.Empty
			End If

			strValue = CStr(Value)
			If strValue.Length = 0 Then
				strValue = "0"
			End If

			formatString = "{0:" & formatString & "}"
			Return String.Format(formatString, CLng(strValue))
		End Function

		''' <summary>
		''' DBから取得したデータがDate値の場合に日付のみの文字列へ変換
		''' </summary>
		''' <param name="Value">対象値</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' Nullの場合は、String.Emptyに変換する
		''' </remarks>
		Public Shared Function CDateValueToNoTimeString(ByVal Value As Object) As String
			If Value Is DBNull.Value Then
				Return String.Empty
			Else
				Return CDate(Value).ToShortDateString
			End If
		End Function

		''' <summary>
		''' DBから取得したデータがDate値の場合に日付のみの文字列へ変換
		''' </summary>
		''' <param name="Value">対象値</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' Nullの場合は、String.Emptyに変換する
		''' </remarks>
		Public Shared Function CDateValueToYYYYMM(ByVal Value As Object) As String
			If Value Is DBNull.Value Then
				Return String.Empty
			Else
				Return String.Format("{0:yyyy/MM}", Value)
			End If
		End Function

		''' <summary>
		''' DBから取得したデータがInteger値の場合に日付のみの文字列へ変換
		''' </summary>
		''' <param name="Value">対象値</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' Nullの場合は、String.Emptyに変換する
		''' </remarks>
		Public Shared Function CIntValueToYYYYMM(ByVal Value As Object) As String
			If Value Is DBNull.Value Then
				Return String.Empty
			Else
				Dim buf As String
				buf = CStr(Value)
				If buf.Length = 6 Then
					buf &= "01"
				End If
				buf = String.Format("{0:0000/00/00}", CLng(buf))
				Return String.Format("{0:yyyy/MM}", CDate(buf))
			End If
		End Function

		''' <summary>
		''' DBから取得したデータを指定されたフォーマットに変換する
		''' </summary>
		''' <param name="Value">対象値</param>
		''' <param name="formatString">フォーマット文字列</param>
		''' <returns>変換値</returns>
		''' <remarks>
		''' </remarks>
		Public Shared Function CFormat(ByVal Value As Object, ByVal formatString As String) As String
			If Value Is DBNull.Value Then
				Return String.Empty
			Else
				formatString = "{0:" & formatString & "}"
				Return String.Format(formatString, Value)
			End If
		End Function

		''' <summary>
		''' キーと値を保持したコレクションを作成する
		''' </summary>
		''' <param name="rows">作成元となるデータ</param>
		''' <param name="keyColumnName">キーとするデータの列名</param>
		''' <param name="valueColumnName">キーに対して値とするデータの列名</param>
		''' <returns>コレクション</returns>
		''' <remarks>
		''' </remarks>
		Public Shared Function CDictionary(ByVal rows As DataRowCollection, ByVal keyColumnName As String, ByVal valueColumnName As String) As IDictionary
			Dim ht As New Hashtable
			Dim dr As DataRow

			For Each dr In rows
				ht.Add(CStr(dr(keyColumnName)), CStr(dr(valueColumnName)).Trim())
			Next

			Return ht
		End Function

		''' <summary>
		''' SQL文の条件式LIKEを複数作成します。
		''' </summary>
		''' <param name="columnName">列名</param>
		''' <param name="arr"></param>
		''' <param name="op"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function MakeSQLWhereLikeList(ByRef columnName As String, ByRef arr() As String, ByRef op As String) As String
			Dim buf As ArrayList

			buf = New ArrayList

			For Each item As String In arr
				buf.Add(String.Format("({0} LIKE '%{1}%')", columnName, item))
			Next
			Return "(" & Join(buf.ToArray(), " " & op & " ") & ")"
		End Function

		''' <summary>データベースから取得したデータの格納先となる Entity を作成する</summary>
		Private Shared _entityBuilder As New EntityBuilder

		''' <summary>
		''' 指定されたオブジェクトのプロパティ名及び、属性から列名を取得する
		''' </summary>
		''' <param name="typ">列名を取得したいモデルのタイプ</param>
		''' <returns>列名配列</returns>
		''' <remarks></remarks>
		Public Shared Function GetColumnNames(ByVal typ As Type) As Hashtable
			Dim hash As Hashtable
			Dim pinfo() As PropertyInfo

			hash = New Hashtable

			' プロパティ定義を取得
			pinfo = ClassUtil.GetProperties(typ)
			For Each prop As PropertyInfo In pinfo
				' 未使用属性判定
				Dim ignore As ColumnIgnoreAttribute
				ignore = ClassUtil.GetCustomAttribute(Of ColumnIgnoreAttribute)(prop)
				If ignore IsNot Nothing Then
					If ignore.Ignore Then
						Continue For
					End If
				End If

				' 列名属性取得
				Dim attr As ColumnAttribute
				attr = ClassUtil.GetCustomAttribute(Of ColumnAttribute)(prop)
				If attr IsNot Nothing Then
					hash.Add(attr.ColumnName, prop.Name)
				Else
					hash.Add(prop.Name, prop.Name)
				End If
			Next
			Return hash
		End Function

		Public Shared Function GetColumnName(ByVal prop As PropertyInfo) As String
			' 未使用属性判定
			Dim ignore As ColumnIgnoreAttribute
			ignore = ClassUtil.GetCustomAttribute(Of ColumnIgnoreAttribute)(prop)
			If ignore IsNot Nothing Then
				If ignore.Ignore Then
					Return String.Empty
				End If
			End If

			' 列名属性取得
			Dim attr As ColumnAttribute
			attr = ClassUtil.GetCustomAttribute(Of ColumnAttribute)(prop)
			If attr IsNot Nothing Then
				Return attr.ColumnName
			Else
				Return prop.Name
			End If
		End Function

		''' <summary>
		''' 指定されたオブジェクトのプロパティ名及び、属性から列名を取得する
		''' </summary>
		''' <param name="target">列名を取得したいモデル</param>
		''' <returns>列名配列</returns>
		''' <remarks></remarks>
		Public Shared Function GetColumnNames(ByVal target As Object) As Hashtable
			Return GetColumnNames(target.GetType)
		End Function

		''' <summary>
		''' DataTableのカラム構成を構築する
		''' </summary>
		''' <typeparam name="T">エンティティとなるクラス</typeparam>
		''' <typeparam name="Order">項目の順序となる列挙型</typeparam>
		''' <param name="captions">列のキャプションとなる文字列配列</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function CreateTable(Of T, Order)(Optional ByVal captions() As String = Nothing) As DataTable
			SyncLock _entityBuilder
				Return _entityBuilder.CreateTable(Of T, Order)(captions)
			End SyncLock
		End Function

		''' <summary>
		''' 引数の DataTable から指定されたタイプのデータ配列へ変換して返す。
		''' </summary>
		''' <typeparam name="T">変換先のタイプ</typeparam>
		''' <param name="tbl">変換元テーブルデータ</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function Create(Of T)(ByVal tbl As DataTable) As T()
			SyncLock _entityBuilder
				Return _entityBuilder.Create(Of T)(tbl)
			End SyncLock
		End Function

		''' <summary>
		''' 引数の DataRow から指定されたタイプのデータ配列へ変換して返す。
		''' </summary>
		''' <typeparam name="T">変換先のタイプ</typeparam>
		''' <param name="row">変換元行データ</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function Create(Of T)(ByVal row As DataRow) As T
			SyncLock _entityBuilder
				Return _entityBuilder.Create(Of T)(row)
			End SyncLock
		End Function

		''' <summary>
		''' 引数の DataRow から指定されたタイプのデータ配列へ変換して返す。
		''' </summary>
		''' <typeparam name="T">変換先のタイプ</typeparam>
		''' <param name="row">変換元行データ</param>
		''' <param name="version">変換するデータのバージョン</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function Create(Of T)(ByVal row As DataRow, ByVal version As DataRowVersion) As T
			SyncLock _entityBuilder
				Return _entityBuilder.Create(Of T)(row, version)
			End SyncLock
		End Function

		''' <summary>
		''' 引数のオブジェクト内にTable属性のフィールドが存在する場合は列情報を設定する
		''' </summary>
		''' <param name="obj">対象のインスタンス</param>
		''' <remarks></remarks>
		Public Shared Sub SetColumnInfo(ByVal obj As Object)
			SyncLock _entityBuilder
				_entityBuilder.SetColumnInfo(obj)
			End SyncLock
		End Sub

		''' <summary>
		''' 引数のエンティティから DataRow へ変換
		''' </summary>
		''' <param name="entity">変換元</param>
		''' <param name="row">変換先</param>
		''' <remarks></remarks>
		Public Shared Sub Convert(ByVal entity As Object, ByVal row As DataRow)
			SyncLock _entityBuilder
				_entityBuilder.Convert(entity, row)
			End SyncLock
		End Sub

		''' <summary>
		''' SQL のソート部分で使用するカラムを作成
		''' </summary>
		''' <returns>SQL のソート文字列</returns>
		''' <remarks></remarks>
		Public Shared Function GetOrderBy(ByVal columns() As String) As String
			Dim value As String

			value = String.Empty
			If columns.Length = 0 Then
				Return value
			End If
			For Each col As String In columns
				If col.Length = 0 Then
					Continue For
				End If
				value &= CStr(IIf(value.Length = 0, String.Empty, ", ")) & col
			Next
			If value.Length = 0 Then
				Return String.Empty
			End If

			Return String.Format(C_ORDER_BY, value)
		End Function

		''' <summary>
		''' SQL のソート部分で使用するカラムを作成
		''' </summary>
		''' <param name="sortExpression">ソート項目名</param>
		''' <param name="sortDirection">ソート順</param>
		''' <returns>SQL のソートに使用するカラム文字列</returns>
		''' <remarks></remarks>
		Public Shared Function GetOrderByColumn(ByVal sortExpression As String, ByVal sortDirection As SortDirectionValue) As String
			If sortExpression.Length = 0 Then
				Return String.Empty
			End If
			Return String.Format(C_ORDER_BY_COL, sortExpression, sortDirection.ToString)
		End Function

		''' <summary>
		''' DBパラメータをカンマで連結して文字列に変換する
		''' </summary>
		''' <param name="params">DBパラメータ</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function ToStringParameter(ByVal params As IDataParameterCollection) As String
			Dim sb As New StringBuilder

			For Each param As IDataParameter In params
				sb.Append(IIf(sb.ToString.Length = 0, String.Empty, ","))
				sb.Append(Replace2NullWithSinQt(param.Value))
			Next

			If sb.ToString.Length = 0 Then
				Return String.Empty
			End If

			Return " " & sb.ToString
		End Function

		''' <summary>
		''' Null変換(シングルクォーテーション付き)
		''' </summary>
		''' <param name="val"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function Replace2NullWithSinQt(ByVal val As Object) As String
			If val Is Nothing Then
				Return "NULL"
			End If

			If val.ToString.Trim.Length = 0 Then
				Return "NULL"
			End If

			Return "'" & val.ToString & "'"
		End Function

#Region " 未使用ではあるが、サンプルとしてとっておきたいのでコメントにして残してます "

		'''' <summary>
		'''' 引数の DataRow から指定されたタイプのデータ配列へ変換して返す。
		'''' </summary>
		'''' <typeparam name="T">変換先のタイプ</typeparam>
		'''' <returns></returns>
		'''' <remarks></remarks>
		'Public Shared Function Create(Of T)() As T
		'	SyncLock _entityBuilder
		'		Return _entityBuilder.Create(Of T)()
		'	End SyncLock
		'End Function

#End Region

	End Class

End Namespace
