
Imports System.Reflection

Namespace Util

	''' <summary>
	''' タイプを操作するのに便利なメソッド集
	''' </summary>
	''' <remarks></remarks>
	Public Class ClassUtil

#Region " CreateInstance "

		''' <summary>
		''' 指定された型のインスタンス化
		''' </summary>
		''' <param name="type">インスタンス化したい型</param>
		''' <returns>生成したインスタンス</returns>
		''' <remarks>
		''' 引数無しコンストラクタのとき
		''' </remarks>
		Public Shared Function NewInstance(ByVal type As Type) As Object
			Return Activator.CreateInstance(type)
		End Function

		''' <summary>
		''' 指定された型のインスタンス化
		''' </summary>
		''' <param name="type">インスタンス化したい型</param>
		''' <param name="args">コンストラクタの引数</param>
		''' <returns>生成したインスタンス</returns>
		''' <remarks>
		''' 引数有りコンストラクタのとき
		''' </remarks>
		Public Shared Function NewInstance(ByVal type As Type, ByVal args() As Object) As Object
			Return Activator.CreateInstance(type, args)
		End Function

#End Region
#Region " プロパティ定義の取得 "

		''' <summary>
		''' 引数タイプ内のプロパティ定義を返す
		''' </summary>
		''' <param name="typ">タイプ</param>
		''' <returns>プロパティ定義配列</returns>
		''' <remarks></remarks>
		Public Shared Function GetProperties(ByVal typ As Type) As PropertyInfo()
			Return typ.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
		End Function

		''' <summary>
		''' 引数インスタンス内のプロパティ情報を返す
		''' </summary>
		''' <param name="target">インスタンス</param>
		''' <returns>プロパティ定義配列</returns>
		''' <remarks></remarks>
		Public Shared Function GetProperties(ByVal target As Object) As PropertyInfo()
			Return GetProperties(target.GetType)
		End Function

		''' <summary>
		''' 引数タイプ内のプロパティ定義を返す
		''' </summary>
		''' <param name="typ">タイプ</param>
		''' <returns>プロパティ定義配列</returns>
		''' <remarks></remarks>
		Public Shared Function GetProperties(ByVal typ As Type, ByVal name As String) As PropertyInfo
			Return typ.GetProperty(name, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
		End Function

#End Region
#Region " フィールド定義の取得 "

		''' <summary>
		''' 引数タイプ内のフィールド定義を返す
		''' </summary>
		''' <param name="typ">タイプ</param>
		''' <returns>フィールド定義配列</returns>
		''' <remarks></remarks>
		Public Shared Function GetFields(ByVal typ As Type) As FieldInfo()
			Return typ.GetFields(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
		End Function

		''' <summary>
		''' 引数インスタンス内のフィールド情報を返す
		''' </summary>
		''' <param name="target">インスタンス</param>
		''' <returns>フィールド定義配列</returns>
		''' <remarks></remarks>
		Public Shared Function GetFields(ByVal target As Object) As FieldInfo()
			Return GetFields(target.GetType)
		End Function

#End Region
#Region " メソッド定義の取得 "

		''' <summary>
		''' 引数タイプ内のメソッド定義を返す
		''' </summary>
		''' <param name="typ">タイプ</param>
		''' <returns>メソッド定義配列</returns>
		''' <remarks></remarks>
		Public Shared Function GetMethods(ByVal typ As Type) As MethodInfo()
			Return typ.GetMethods(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
		End Function

		''' <summary>
		''' 引数インスタンス内のメソッド情報を返す
		''' </summary>
		''' <param name="target">インスタンス</param>
		''' <returns>メソッド定義配列</returns>
		''' <remarks></remarks>
		Public Shared Function GetMethods(ByVal target As Object) As MethodInfo()
			Return GetMethods(target.GetType)
		End Function

#End Region
#Region " イベント定義の取得 "

		''' <summary>
		''' 引数タイプ内のイベント定義を返す
		''' </summary>
		''' <param name="typ">タイプ</param>
		''' <returns>イベント定義配列</returns>
		''' <remarks></remarks>
		Public Shared Function GetEvents(ByVal typ As Type) As EventInfo()
			Return typ.GetEvents(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
		End Function

		''' <summary>
		''' 引数インスタンス内のイベント情報を返す
		''' </summary>
		''' <param name="target">インスタンス</param>
		''' <returns>イベント定義配列</returns>
		''' <remarks></remarks>
		Public Shared Function GetEvents(ByVal target As Object) As EventInfo()
			Return GetEvents(target.GetType)
		End Function

#End Region
#Region " カスタム属性の取得 "

#Region " Type "

		''' <summary>
		''' 指定された型に存在する指定されたカスタム属性を含む配列を返します。
		''' </summary>
		''' <typeparam name="T">対象となるカスタム属性</typeparam>
		''' <param name="typ">対象となる型</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCustomAttributes(Of T)(ByVal typ As Type) As T()
			Dim ary() As Object
			Dim aryLst As ArrayList
			ary = typ.GetCustomAttributes(GetType(T), False)
			aryLst = New ArrayList(ary)
			Return DirectCast(aryLst.ToArray(GetType(T)), T())
		End Function

		''' <summary>
		''' 指定された型に存在する指定されたカスタム属性を含む配列を返します。
		''' </summary>
		''' <typeparam name="T">対象となるカスタム属性</typeparam>
		''' <param name="typ">対象となる型</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCustomAttribute(Of T)(ByVal typ As Type) As T
			Dim ary() As T
			ary = GetCustomAttributes(Of T)(typ)
			If ary.Length = 0 Then
				Return Nothing
			End If
			Return ary(0)
		End Function

		''' <summary>
		''' 指定されたフィールドに指定されたカスタム属性を含む配列を返します。
		''' </summary>
		''' <param name="typ">対象となるフィールド</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCustomAttributes(ByVal typ As Type) As Attribute()
			Dim arylst As ArrayList
			arylst = New ArrayList(typ.GetCustomAttributes(False))
			Return DirectCast(arylst.ToArray(GetType(Attribute)), Attribute())
		End Function

#End Region
#Region " PropertyInfo "

		''' <summary>
		''' 指定されたプロパティに存在する指定されたカスタム属性を含む配列を返します。
		''' </summary>
		''' <typeparam name="T">対象となるカスタム属性</typeparam>
		''' <param name="prop">対象となるプロパティ</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCustomAttributes(Of T)(ByVal prop As PropertyInfo) As T()
			Dim ary() As Object
			Dim aryLst As ArrayList
			ary = prop.GetCustomAttributes(GetType(T), False)
			aryLst = New ArrayList(ary)
			Return DirectCast(aryLst.ToArray(GetType(T)), T())
		End Function

		''' <summary>
		''' 指定されたプロパティに存在する指定されたカスタム属性を返します。
		''' </summary>
		''' <typeparam name="T">対象となるカスタム属性</typeparam>
		''' <param name="prop">対象となるプロパティ</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCustomAttribute(Of T)(ByVal prop As PropertyInfo) As T
			Dim ary() As T
			ary = GetCustomAttributes(Of T)(prop)
			If ary.Length = 0 Then
				Return Nothing
			End If
			Return ary(0)
		End Function

#End Region
#Region " FieldInfo "

		''' <summary>
		''' 指定されたフィールドに指定されたカスタム属性を返します。
		''' </summary>
		''' <typeparam name="T">対象となるカスタム属性</typeparam>
		''' <param name="field">対象となるフィールド</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCustomAttribute(Of T)(ByVal field As FieldInfo) As T
			Dim ary() As T
			ary = GetCustomAttributes(Of T)(field)
			If ary.Length = 0 Then
				Return Nothing
			End If
			Return ary(0)
		End Function

		''' <summary>
		''' 指定されたフィールドに指定されたカスタム属性を含む配列を返します。
		''' </summary>
		''' <typeparam name="T">対象となるカスタム属性</typeparam>
		''' <param name="field">対象となるフィールド</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCustomAttributes(Of T)(ByVal field As FieldInfo) As T()
			Dim ary() As Object
			Dim aryLst As ArrayList
			ary = field.GetCustomAttributes(GetType(T), False)
			aryLst = New ArrayList(ary)
			Return DirectCast(aryLst.ToArray(GetType(T)), T())
		End Function

		''' <summary>
		''' 指定されたフィールドに指定されたカスタム属性を含む配列を返します。
		''' </summary>
		''' <param name="field">対象となるフィールド</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCustomAttributes(ByVal field As FieldInfo) As Attribute()
			Dim arylst As ArrayList
			arylst = New ArrayList(field.GetCustomAttributes(False))
			Return DirectCast(arylst.ToArray(GetType(Attribute)), Attribute())
		End Function

#End Region
#Region " MethodBase "

		''' <summary>
		''' 指定されたフィールドに存在するカスタム属性を含む配列を返します。
		''' </summary>
		''' <typeparam name="T">対象となるカスタム属性</typeparam>
		''' <param name="method">対象となるフィールド</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCustomAttributes(Of T)(ByVal method As MethodBase, Optional ByVal inherit As Boolean = False) As T()
			Dim ary() As Object
			Dim aryLst As ArrayList
			ary = method.GetCustomAttributes(GetType(T), inherit)
			aryLst = New ArrayList(ary)
			Return DirectCast(aryLst.ToArray(GetType(T)), T())
		End Function

		''' <summary>
		''' 指定されたフィールドに存在するカスタム属性を返します。
		''' </summary>
		''' <typeparam name="T">対象となるカスタム属性</typeparam>
		''' <param name="method">対象となるフィールド</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCustomAttribute(Of T)(ByVal method As MethodBase, Optional ByVal inherit As Boolean = False) As T
			Dim ary() As T
			ary = GetCustomAttributes(Of T)(method, inherit)
			If ary.Length = 0 Then
				Return Nothing
			End If
			Return ary(0)
		End Function

#End Region
#Region " Event "

		''' <summary>
		''' 指定されたフィールドに存在するカスタム属性を含む配列を返します。
		''' </summary>
		''' <typeparam name="T">対象となるカスタム属性</typeparam>
		''' <param name="method">対象となるフィールド</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCustomAttributes(Of T)(ByVal method As EventInfo, Optional ByVal inherit As Boolean = False) As T()
			Dim raiseMethod As MethodBase
			Dim ary() As Object
			Dim aryLst As ArrayList
			raiseMethod = method.GetRaiseMethod(inherit)
			If raiseMethod Is Nothing Then
				Return Nothing
			End If
			ary = raiseMethod.GetCustomAttributes(GetType(T), inherit)
			aryLst = New ArrayList(ary)
			Return DirectCast(aryLst.ToArray(GetType(T)), T())
		End Function

		''' <summary>
		''' 指定されたフィールドに存在するカスタム属性を返します。
		''' </summary>
		''' <typeparam name="T">対象となるカスタム属性</typeparam>
		''' <param name="method">対象となるフィールド</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Shared Function GetCustomAttribute(Of T)(ByVal method As EventInfo, Optional ByVal inherit As Boolean = False) As T
			Dim ary() As T
			ary = GetCustomAttributes(Of T)(method, inherit)
			If ary.Length = 0 Then
				Return Nothing
			End If
			Return ary(0)
		End Function

#End Region

#End Region
#Region " フィールドへインスタンスを注入 "

		''' <summary>
		''' フィールドへインスタンスを注入
		''' </summary>
		''' <param name="target">対象となるインスタンス</param>
		''' <param name="field">対象となるインスタンスのフィールド</param>
		''' <param name="args">フィールドへ設定するインスタンスの配列</param>
		''' <remarks></remarks>
		Public Shared Sub Inject(ByVal target As Object, ByVal field As FieldInfo, ByVal args() As Object)
			Dim bFlags As BindingFlags

			' フィールドに値をセットする為のBindingFlags
			bFlags = BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.SetField

			' インスタンスをフィールドへ注入！
			target.GetType().InvokeMember(field.Name, bFlags, Nothing, target, args)
		End Sub

#End Region
#Region " Check "

		''' <summary>
		''' インタフェースが実装されているかチェックする
		''' </summary>
		''' <param name="targetType">対象となる型</param>
		''' <param name="checkType">チェックする型</param>
		''' <returns>True は存在する、False は存在しない</returns>
		''' <remarks></remarks>
		Public Shared Function IsInterfaceImpl(ByVal targetType As Type, ByVal checkType As Type) As Boolean
			Dim ok As Boolean
			ok = False
			For Each item As Type In targetType.GetInterfaces
				If item.Equals(checkType) Then
					ok = True
					Exit For
				End If
			Next
			Return ok
		End Function

#End Region

	End Class

End Namespace
