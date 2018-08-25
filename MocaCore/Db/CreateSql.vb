
Imports System.Text
Imports Moca.Db.Attr
Imports Moca.Entity

Namespace Db

    ''' <summary>
    ''' SQL 作成
    ''' </summary>
    Public Class CreateSql

#Region " Declare "

        Private _helper As IDbAccessHelper

#Region " Logging For Log4net "
        ''' <summary>Logging For Log4net</summary>
        Private Shared ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)
#End Region
#End Region

#Region " コンストラクタ "

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="helper"></param>
        Public Sub New(ByVal helper As IDbAccessHelper)
            _helper = helper
        End Sub

#End Region

        ''' <summary>
        ''' Insert SQL 作成
        ''' </summary>
        ''' <param name="info"></param>
        ''' <returns></returns>
        Public Function Insert(ByVal info As EntityInfo) As String
            Const cSql As String = "INSERT INTO {0} ({1}) VALUES ({2})"
            Dim tbl As String
            Dim def As Object
            Dim defCols As ICollection
            Dim defCache As EntityInfo
            Dim sqlInto As New StringBuilder(512)
            Dim sqlValues As New StringBuilder(512)
            Dim sql As String

            tbl = info.TableName1st
            If String.IsNullOrEmpty(tbl) Then
                Throw New ArgumentNullException("TableName", "テーブル定義情報がフィールドに存在しないときは SQL 自動生成は利用できません。")
            End If

            sql = info.SqlInsert(tbl)
            If Not String.IsNullOrEmpty(sql) Then
                _mylog.Debug(sql)
                Return sql
            End If

            def = info.TableDef(tbl)
            defCache = info.TableEntityInfo(tbl)
            defCols = defCache.ColumnNames
            For Each defCol As String In defCols
                If info.PropertyName(defCol) Is Nothing Then
                    Continue For
                End If

                Dim obj As Object
#If net20 Then
				obj = defCache.ProprtyInfo(defCol).GetValue(def, Nothing)
#Else
                obj = defCache.ProprtyAccessor(defCol).GetValue(def)
#End If

                If TypeOf obj IsNot DbInfoColumn Then
                    Continue For
                End If

                Dim attrCrud() As CrudAttribute = defCache.ColumnCrudConditions(defCol)
                If Not attrCrud.Length.Equals(0) AndAlso
                        Array.FindAll(Of CrudAttribute)(attrCrud, Function(x) x.Status = DataRowState.Added).Length.Equals(0) Then
                    Continue For
                End If

                Dim defVal As String = _helper.CnvStatmentParameterName(defCol)
                Dim attr As DbFunctionAttribute = defCache.ColumnFunctions(defCol)
                If attr IsNot Nothing Then
                    defVal = String.Format(attr.Function, defVal)
                End If

                sqlInto.Append(IIf(sqlInto.Length.Equals(0), String.Empty, ", "))
                sqlInto.Append(_helper.QuotationMarks(defCol))
                sqlValues.Append(IIf(sqlValues.Length.Equals(0), String.Empty, ", "))
                sqlValues.Append(defVal)
            Next

            sql = String.Format(cSql, _helper.QuotationMarks(tbl), sqlInto.ToString, sqlValues.ToString)
            _mylog.Debug(sql)
            info.SqlInsert(tbl) = sql
            Return sql
        End Function

        ''' <summary>
        ''' Update SQL 作成
        ''' </summary>
        ''' <param name="info"></param>
        ''' <returns></returns>
        Public Function Update(ByVal info As EntityInfo) As String
            Const cSql As String = "UPDATE {0} SET {1} WHERE {2}"
            Dim tbl As String
            Dim def As Object
            Dim defCols As ICollection
            Dim defCache As EntityInfo
            Dim sqlSet As New StringBuilder(512)
            Dim sqlWhere As New StringBuilder(512)
            Dim sql As String

            tbl = info.TableName1st
            If String.IsNullOrEmpty(tbl) Then
                Throw New ArgumentNullException("TableName", "テーブル定義情報がフィールドに存在しないときは SQL 自動生成は利用できません。")
            End If

            sql = info.SqlUpdate(tbl)
            If Not String.IsNullOrEmpty(sql) Then
                _mylog.Debug(sql)
                Return sql
            End If

            def = info.TableDef(tbl)
            defCache = info.TableEntityInfo(tbl)
            defCols = defCache.ColumnNames
            For Each defCol As String In defCols
                If info.PropertyName(defCol) Is Nothing Then
                    Continue For
                End If

                Dim obj As Object
#If net20 Then
				obj = defCache.ProprtyInfo(defCol).GetValue(def, Nothing)
#Else
                obj = defCache.ProprtyAccessor(defCol).GetValue(def)
#End If

                If TypeOf obj IsNot DbInfoColumn Then
                    Continue For
                End If

                Dim defVal As String = _helper.CnvStatmentParameterName(defCol)
                Dim attrDbFunction As DbFunctionAttribute = defCache.ColumnFunctions(defCol)
                If attrDbFunction IsNot Nothing Then
                    defVal = String.Format(attrDbFunction.Function, defVal)
                End If

                Dim infoCol As DbInfoColumn = obj
                If Not infoCol.PrimaryKey Then
                    Dim attrCrud() As CrudAttribute = defCache.ColumnCrudConditions(defCol)
                    If attrCrud.Length.Equals(0) OrElse
                        Array.FindAll(Of CrudAttribute)(attrCrud, Function(x) x.Status = DataRowState.Modified).Length.Equals(1) Then
                        sqlSet.Append(IIf(sqlSet.Length.Equals(0), String.Empty, ", "))
                        sqlSet.AppendFormat("{0} = {1}", _helper.QuotationMarks(defCol), defVal)
                    End If

                    Continue For
                End If

                sqlWhere.Append(IIf(sqlWhere.Length.Equals(0), String.Empty, " AND "))
                sqlWhere.AppendFormat("{0} = {1}", _helper.QuotationMarks(defCol), _helper.CnvStatmentParameterName(defCol))
            Next

            sql = String.Format(cSql, _helper.QuotationMarks(tbl), sqlSet.ToString, sqlWhere.ToString)
            _mylog.Debug(sql)
            info.SqlUpdate(tbl) = sql
            Return sql
        End Function

        ''' <summary>
        ''' Delete SQL 作成
        ''' </summary>
        ''' <param name="info"></param>
        ''' <returns></returns>
        Public Function Delete(ByVal info As EntityInfo) As String
            Const cSql As String = "DELETE FROM {0} WHERE {1}"
            Dim tbl As String
            Dim def As Object
            Dim defCols As ICollection
            Dim defCache As EntityInfo
            Dim sqlWhere As New StringBuilder(512)
            Dim sql As String

            tbl = info.TableName1st
            If String.IsNullOrEmpty(tbl) Then
                Throw New ArgumentNullException("TableName", "テーブル定義情報がフィールドに存在しないときは SQL 自動生成は利用できません。")
            End If

            sql = info.SqlDelete(tbl)
            If Not String.IsNullOrEmpty(sql) Then
                _mylog.Debug(sql)
                Return sql
            End If

            def = info.TableDef(tbl)
            defCache = info.TableEntityInfo(tbl)
            defCols = defCache.ColumnNames
            For Each defCol As String In defCols
                If info.PropertyName(defCol) Is Nothing Then
                    Continue For
                End If

                Dim obj As Object
#If net20 Then
				obj = defCache.ProprtyInfo(defCol).GetValue(def, Nothing)
#Else
                obj = defCache.ProprtyAccessor(defCol).GetValue(def)
#End If

                If TypeOf obj IsNot DbInfoColumn Then
                    Continue For
                End If

                Dim infoCol As DbInfoColumn = obj
                If Not infoCol.PrimaryKey Then
                    Continue For
                End If

                sqlWhere.Append(IIf(sqlWhere.Length.Equals(0), String.Empty, " AND "))
                sqlWhere.AppendFormat("{0} = {1}", _helper.QuotationMarks(infoCol.Name), _helper.CnvStatmentParameterName(infoCol.Name))
            Next

            sql = String.Format(cSql, _helper.QuotationMarks(tbl), sqlWhere.ToString)
            _mylog.Debug(sql)
            info.SqlDelete(tbl) = sql
            Return sql
        End Function

    End Class

End Namespace
