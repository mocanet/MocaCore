Imports System.Configuration
Imports System.Data.Common
Imports System.Reflection
Imports Moca.Db.CommandWrapper
Imports Moca.Util

Namespace Db

	''' <summary>
	''' コマンド種別の列挙型
	''' </summary>
	Public Enum SQLCommandTypes
		''' <summary>SELECT文を実行する</summary>
		SelectText = 0
		''' <summary>SELECT文実行後にDataSetによるUPDATE実行する</summary>
		Select4Update
		''' <summary>UPDATE文を実行する</summary>
		UpdateText
		''' <summary>INSERT文を実行する</summary>
		InsertText
		''' <summary>DELETE文を実行する</summary>
		DeleteText
		''' <summary>ストアド実行を実行する</summary>
		StoredProcedure
		''' <summary>DDL実行を実行する</summary>
		DDL
	End Enum

End Namespace
