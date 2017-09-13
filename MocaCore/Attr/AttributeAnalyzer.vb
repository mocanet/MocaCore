
Imports System.Reflection
Imports Moca.Aop
Imports Moca.Di
Imports Moca.Util
Imports Moca.Interceptor

Namespace Attr

	''' <summary>
	''' フィールドインジェクトデリゲート
	''' </summary>
	''' <param name="target">インジェクト対象となるインスタンス</param>
	''' <param name="field">対象となるフィールド定義</param>
	''' <param name="component">インジェクトするコンポーネント</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Delegate Function MocaFieldInject(ByVal target As Object, ByVal field As FieldInfo, ByVal component As MocaComponent) As Object

	Public Delegate Function MocaFieldInjectType(ByVal target As Type, ByVal field As FieldInfo, ByVal component As MocaComponent) As Object

	''' <summary>
	''' 実装実験中
	''' </summary>
	''' <param name="parent"></param>
	''' <param name="obj"></param>
	''' <remarks></remarks>
	Friend Delegate Sub MocaEventDelegateInject(ByVal parent As Object, ByVal obj As Object)

#Region " 列挙型 "

	''' <summary>
	''' 属性解析するターゲット列挙型
	''' </summary>
	''' <remarks></remarks>
	Public Enum AttributeAnalyzerTargets
		''' <summary>クラス</summary>
		[Class] = AttributeTargets.Class
		''' <summary>フィールド</summary>
		Field = AttributeTargets.Field
		''' <summary>インタフェース</summary>
		[Interface] = AttributeTargets.Interface
		''' <summary>メソッド</summary>
		Method = AttributeTargets.Method
		''' <summary>プロパティ</summary>
		[Property] = AttributeTargets.Property
	End Enum

#End Region

	''' <summary>
	''' 属性解析
	''' </summary>
	''' <remarks></remarks>
	Public Class AttributeAnalyzer

		''' <summary>各種解析たち</summary>
		Private _analyzers As Dictionary(Of AttributeAnalyzerTargets, IList(Of IAttributeAnalyzer))
		''' <summary>解析を除外するNamespace</summary>
		Private _ignoreNamespace As IList(Of String)

		''' <summary>フィールドインジェクトデリゲート</summary>
		Private _injectMethod As MocaFieldInject
		Private _injectMethodType As MocaFieldInjectType

		''' <summary>実装中</summary>
		Private _injectEventDelegate As MocaEventDelegateInject

#Region " log4net "
		''' <summary>log4net logger</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)
#End Region

#Region " コンストラクタ "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			_analyzers = New Dictionary(Of AttributeAnalyzerTargets, IList(Of IAttributeAnalyzer))
			_ignoreNamespace = New List(Of String)
			_injectEventDelegate = Nothing
		End Sub

#End Region

#Region " 解析 "

#Region " 生成 "

		''' <summary>
		''' クラスを解析してインスタンスを生成する
		''' </summary>
		''' <param name="typ"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Create(ByVal typ As Type) As Object
			Dim component As MocaComponent
			Dim aspects As ArrayList

			component = MocaContainerFactory.Container().GetComponent(typ)
			If component IsNot Nothing Then
				Return getInstance(component)
			End If

			component = analyzeClass(typ)
			If component Is Nothing Then
				component = New Moca.Di.MocaComponent(typ, typ)
			End If

			' 生成したコンポーネントを Di へ
			MocaContainerFactory.Container().SetComponent(component)

			aspects = New ArrayList()
			aspects.AddRange(analyzeInterfaces(typ))				' インタフェース解析

			' Getter/Setter メソッドのアスペクト作成（フィールドへアクセスするために必要！）
			aspects.AddRange(_createFieldGetterSetterAspect())

			component.Aspects = DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())

			Dim instance As Object
			instance = getInstance(component)

			Analyze(instance)

			Return instance
		End Function

		''' <summary>
		''' フィールド解析
		''' </summary>
		''' <param name="target">対象となるオブジェクト</param>
		''' <returns>作成したコンポーネント</returns>
		''' <remarks></remarks>
		Protected Function analyzeClass(ByVal target As Type) As MocaComponent
			Dim component As MocaComponent

			component = Nothing

			If _analyzers.ContainsKey(AttributeAnalyzerTargets.Class) Then
				For Each analyzer As IAttributeAnalyzer In _analyzers(AttributeAnalyzerTargets.Class)
					component = analyzer.Analyze(target)
					If component IsNot Nothing Then
						Exit For
					End If
				Next
			End If

			Return component
		End Function

#End Region

		''' <summary>
		''' 解析
		''' </summary>
		''' <param name="target">対象となるオブジェクト</param>
		''' <remarks>
		''' 解析を開始する前に解析した属性のアナライザーを追加してください。
		''' </remarks>
		Public Sub Analyze(ByVal target As Object)
			For Each field As FieldInfo In ClassUtil.GetFields(target)
				Dim component As MocaComponent

				If _isIgnore(field.DeclaringType.ToString) Then
					Continue For
				End If

				' 既に存在するかチェック（フィールドの型で）
				component = MocaContainerFactory.Container().GetComponent(field.FieldType)
				If component IsNot Nothing Then
					Analyze(Me.FieldInject(target, field, component))
					Continue For
				End If

				' フィールドの解析
				Analyze(target, field)
			Next

			' イベントのデリゲートを解析するかどうか
			If Me.EventDelegateInject Is Nothing Then
				Return
			End If

			'_mylog.DebugFormat("AnalyzeEventDelegate Type={0},Name={1}", target.GetType.Name, "Nothing")
			'Me.EventDelegateInject.Invoke(target, target)

			For Each prop As PropertyInfo In ClassUtil.GetProperties(target)
				analyzeEventDelegate(target, prop)
			Next
		End Sub

		Public Sub Analyze(ByVal target As Type)
			For Each field As FieldInfo In ClassUtil.GetFields(target)
				Dim component As MocaComponent

				If _isIgnore(field.DeclaringType.ToString) Then
					Continue For
				End If

				' 既に存在するかチェック（フィールドの型で）
				component = MocaContainerFactory.Container().GetComponent(field.FieldType)
				If component IsNot Nothing Then
					Analyze(Me.FieldInjectType(target, field, component))
					Continue For
				End If

				' フィールドの解析
				analyze(target, field)
			Next

			' イベントのデリゲートを解析するかどうか
			If Me.EventDelegateInject Is Nothing Then
				Return
			End If

			'_mylog.DebugFormat("AnalyzeEventDelegate Type={0},Name={1}", target.GetType.Name, "Nothing")
			'Me.EventDelegateInject.Invoke(target, target)

			For Each prop As PropertyInfo In ClassUtil.GetProperties(target)
				analyzeEventDelegate(target, prop)
			Next
		End Sub

#Region " フィールド解析 "

		''' <summary>
		''' フィールド解析
		''' </summary>
		''' <param name="target">対象となるオブジェクト</param>
		''' <param name="field">対象となるフィールド</param>
		''' <remarks></remarks>
		Protected Overridable Sub analyze(ByVal target As Object, ByVal field As FieldInfo)
			Dim component As MocaComponent
			Dim aspects As ArrayList

			aspects = New ArrayList()

			' フィールド解析（仮コンポーネント生成）
			component = analyzeField(target, field)
			If component Is Nothing Then
				Exit Sub
			End If

			' 既に存在するかチェック（実態で）
			If component.ImplType Is Nothing Then
				If MocaContainerFactory.Container().GetComponent(component.Key) IsNot Nothing Then
					component = MocaContainerFactory.Container().GetComponent(component.Key)
					Analyze(Me.FieldInject(target, field, component))
					Exit Sub
				End If
			Else
				If MocaContainerFactory.Container().GetComponent(component.ImplType) IsNot Nothing Then
					component = MocaContainerFactory.Container().GetComponent(component.ImplType)
					Analyze(Me.FieldInject(target, field, component))
					Exit Sub
				End If
			End If

			' 生成したコンポーネントを Di へ
			MocaContainerFactory.Container().SetComponent(component)

			' メンバ解析
			aspects.AddRange(analyzeInterfaces(field.FieldType))

			' フィールドの型と実態の型が異なるか？
			' 異なるときは、実態でもメンバ解析
			If Not field.FieldType.Equals(component.ImplType) Then
				aspects.AddRange(analyzeProperty(component.ImplType))	' プロパティ解析
				aspects.AddRange(analyzeMethod(component.ImplType))		' メソッド解析
				'aspects.AddRange(analyzeEvent(component.ImplType))      ' イベント解析

				' Getter/Setter メソッドのアスペクト作成（フィールドへアクセスするために必要！）
				aspects.AddRange(_createFieldGetterSetterAspect())
			End If

			' コンポーネントへアスペクト設定
			If component.Aspects IsNot Nothing Then
				aspects.InsertRange(0, component.Aspects)
			End If
			component.Aspects = DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())

			' フィールドへインスタンスを注入し、
			' インスタンス化したオブジェクトで解析を再帰
			Analyze(Me.FieldInject(target, field, component))
		End Sub

		Protected Overridable Sub analyze(ByVal target As Type, ByVal field As FieldInfo)
			Dim component As MocaComponent
			Dim aspects As ArrayList

			aspects = New ArrayList()

			' フィールド解析（仮コンポーネント生成）
			component = analyzeField(target, field)
			If component Is Nothing Then
				Exit Sub
			End If

			' 既に存在するかチェック（実態で）
			If component.ImplType Is Nothing Then
				If MocaContainerFactory.Container().GetComponent(component.Key) IsNot Nothing Then
					component = MocaContainerFactory.Container().GetComponent(component.Key)
					Analyze(Me.FieldInject(target, field, component))
					Exit Sub
				End If
			Else
				If MocaContainerFactory.Container().GetComponent(component.ImplType) IsNot Nothing Then
					component = MocaContainerFactory.Container().GetComponent(component.ImplType)
					Analyze(Me.FieldInject(target, field, component))
					Exit Sub
				End If
			End If

			' 生成したコンポーネントを Di へ
			MocaContainerFactory.Container().SetComponent(component)

			' メンバ解析
			aspects.AddRange(analyzeInterfaces(field.FieldType))

			' フィールドの型と実態の型が異なるか？
			' 異なるときは、実態でもメンバ解析
			If Not field.FieldType.Equals(component.ImplType) Then
				aspects.AddRange(analyzeProperty(component.ImplType))	' プロパティ解析
				aspects.AddRange(analyzeMethod(component.ImplType))		' メソッド解析
				'aspects.AddRange(analyzeEvent(component.ImplType))      ' イベント解析

				' Getter/Setter メソッドのアスペクト作成（フィールドへアクセスするために必要！）
				aspects.AddRange(_createFieldGetterSetterAspect())
			End If

			' コンポーネントへアスペクト設定
			If component.Aspects IsNot Nothing Then
				aspects.InsertRange(0, component.Aspects)
			End If
			component.Aspects = DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())

			' フィールドへインスタンスを注入し、
			' インスタンス化したオブジェクトで解析を再帰
			Analyze(Me.FieldInjectType(target, field, component))
		End Sub

		Private Function _createFieldGetterSetterAspect() As IAspect()
			Dim aspects As ArrayList
			Dim pointcut As IPointcut

			aspects = New ArrayList()

			pointcut = New Pointcut(New String() {"Void FieldGetter(System.String, System.String, System.Object ByRef)"})
			aspects.Add(New Aspect(New FieldGetterInterceptor(), pointcut))
			pointcut = New Pointcut(New String() {"Void FieldSetter(System.String, System.String, System.Object)"})
			aspects.Add(New Aspect(New FieldSetterInterceptor(), pointcut))

			Return DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())
		End Function

		''' <summary>
		''' インタフェースの継承元を辿る
		''' </summary>
		''' <param name="targetTyp">対象のType</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Overridable Function analyzeInterfaces(ByVal targetTyp As Type) As IAspect()
			Dim aspects As ArrayList

			aspects = New ArrayList()

			_mylog.DebugFormat("AnalyzeInterfaces Type={0}", targetTyp.Name)

			aspects.AddRange(analyzeProperty(targetTyp))	' プロパティ解析
			aspects.AddRange(analyzeMethod(targetTyp))		' メソッド解析
			'aspects.AddRange(analyzeEvent(targetTyp))	   ' イベント解析

			If targetTyp.GetInterfaces().Length = 0 Then
				Return DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())
			End If
			For Each typ As Type In targetTyp.GetInterfaces
				aspects.AddRange(analyzeInterfaces(typ))
			Next

			Return DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())
		End Function

		''' <summary>
		''' フィールド解析
		''' </summary>
		''' <param name="target">対象となるオブジェクト</param>
		''' <param name="field">フィールド</param>
		''' <returns>作成したコンポーネント</returns>
		''' <remarks></remarks>
		Protected Overridable Function analyzeField(ByVal target As Object, ByVal field As FieldInfo) As MocaComponent
			Dim component As MocaComponent

			component = Nothing

			If _isIgnore(field.DeclaringType.FullName) Then
				Return component
			End If
			If Not String.IsNullOrEmpty(field.FieldType.Namespace) Then
				If field.FieldType.Namespace.StartsWith("log4net") Then
					Return component
				End If
			End If

			_mylog.DebugFormat("AnalyzeField Type={0},Name={1}", target.GetType.Name, field.ToString)

			If _analyzers.ContainsKey(AttributeAnalyzerTargets.Field) Then
				For Each analyzer As IAttributeAnalyzer In _analyzers(AttributeAnalyzerTargets.Field)
					component = analyzer.Analyze(target, field)
					If component IsNot Nothing Then
						Exit For
					End If
				Next
			End If

			Return component
		End Function

#End Region
#Region " プロパティ解析 "

		''' <summary>
		''' プロパティ解析
		''' </summary>
		''' <param name="targetType">対象となる型</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Overridable Function analyzeProperty(ByVal targetType As Type) As IAspect()
			Dim aspects As ArrayList

			aspects = New ArrayList()

			If targetType Is Nothing Then
				Return DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())
			End If

			For Each prop As PropertyInfo In ClassUtil.GetProperties(targetType)
				Dim rc() As IAspect
				rc = analyzeProperty(targetType, prop)
				If rc Is Nothing OrElse rc.Length = 0 Then
					Continue For
				End If
				aspects.AddRange(rc)
				aspects.AddRange(analyzeMethod(prop.PropertyType))		' メソッド解析
			Next

			Return DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())
		End Function

		''' <summary>
		''' プロパティ解析
		''' </summary>
		''' <param name="typ">対象となる型</param>
		''' <param name="prop">プロパティ</param>
		''' <returns>アスペクト配列</returns>
		''' <remarks></remarks>
		Protected Overridable Function analyzeProperty(ByVal typ As Type, ByVal prop As PropertyInfo) As IAspect()
			Dim results As ArrayList
			Dim aspects() As IAspect

			results = New ArrayList()

			If _isIgnore(prop.DeclaringType.FullName) Then
				Return DirectCast(results.ToArray(GetType(IAspect)), IAspect())
			End If

			_mylog.DebugFormat("AnalyzeProperty Type={0},Name={1}", typ.Name, prop.ToString)

			If _analyzers.ContainsKey(AttributeAnalyzerTargets.Property) Then
				For Each analyzer As IAttributeAnalyzer In _analyzers(AttributeAnalyzerTargets.Property)
					aspects = analyzer.Analyze(typ, prop)
					If aspects Is Nothing Then
						Continue For
					End If
					results.AddRange(aspects)
				Next
			End If

			Return DirectCast(results.ToArray(GetType(IAspect)), IAspect())
		End Function

#End Region
#Region " メソッド解析 "

		''' <summary>
		''' メソッド解析
		''' </summary>
		''' <param name="targetType">対象となる型</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Overridable Function analyzeMethod(ByVal targetType As Type) As IAspect()
			Dim aspects As ArrayList

			aspects = New ArrayList()

			If targetType Is Nothing Then
				Return DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())
			End If

			For Each method As MethodInfo In ClassUtil.GetMethods(targetType)
				Dim rc() As IAspect
				rc = analyzeMethod(targetType, method)
				If rc Is Nothing OrElse rc.Length = 0 Then
					Continue For
				End If
				aspects.AddRange(rc)
			Next

			Return DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())
		End Function

		''' <summary>
		''' メソッド解析
		''' </summary>
		''' <param name="typ">対象となる型</param>
		''' <param name="method">メソッド</param>
		''' <returns>アスペクト配列</returns>
		''' <remarks></remarks>
		Protected Overridable Function analyzeMethod(ByVal typ As Type, ByVal method As MethodInfo) As IAspect()
			Dim results As ArrayList
			Dim aspects() As IAspect

			results = New ArrayList()

			If _isIgnore(method.DeclaringType.ToString) Then
				Return DirectCast(results.ToArray(GetType(IAspect)), IAspect())
			End If

			_mylog.DebugFormat("AnalyzeMethod Type={0},Name={1}", typ.Name, method.ToString)

			If _analyzers.ContainsKey(AttributeAnalyzerTargets.Method) Then
				For Each analyzer As IAttributeAnalyzer In _analyzers(AttributeAnalyzerTargets.Method)
					aspects = analyzer.Analyze(typ, method)
					If aspects Is Nothing Then
						Continue For
					End If
                    For Each aspect As IAspect In aspects
                        Analyze(aspect.Advice)
                    Next
                    results.AddRange(aspects)
                Next
			End If

			Return DirectCast(results.ToArray(GetType(IAspect)), IAspect())
		End Function

#End Region
#Region " イベント解析 "

		''' <summary>
		''' イベントデリゲート解析
		''' </summary>
		''' <param name="parent"></param>
		''' <param name="prop"></param>
		''' <remarks></remarks>
		Protected Overridable Sub analyzeEventDelegate(ByVal parent As Object, ByVal prop As PropertyInfo)
			Dim obj As Object

			_mylog.DebugFormat("AnalyzeEventDelegate Type={0},Name={1}", parent.GetType.Name, prop.ToString)

			obj = prop.GetValue(parent, New Object() {})
			If obj Is Nothing Then
				Return
			End If

			Me.EventDelegateInject.Invoke(parent, obj)
		End Sub

		''' <summary>
		''' イベント解析
		''' </summary>
		''' <param name="targetType">対象となる型</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Overridable Function analyzeEvent(ByVal targetType As Type) As IAspect()
			Dim aspects As ArrayList

			aspects = New ArrayList()

			If targetType Is Nothing Then
				Return DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())
			End If

			For Each method As EventInfo In ClassUtil.GetEvents(targetType)
				Dim rc() As IAspect
				rc = analyzeEvent(targetType, method)
				If rc Is Nothing OrElse rc.Length = 0 Then
					Continue For
				End If
				aspects.AddRange(rc)
			Next

			Return DirectCast(aspects.ToArray(GetType(IAspect)), IAspect())
		End Function

		''' <summary>
		''' イベント解析
		''' </summary>
		''' <param name="typ">対象となる型</param>
		''' <param name="method">イベント</param>
		''' <returns>アスペクト配列</returns>
		''' <remarks></remarks>
		Protected Overridable Function analyzeEvent(ByVal typ As Type, ByVal method As EventInfo) As IAspect()
			Dim results As ArrayList
			Dim aspects() As IAspect

			results = New ArrayList()

			If _isIgnore(method.DeclaringType.FullName) Then
				Return DirectCast(results.ToArray(GetType(IAspect)), IAspect())
			End If

			_mylog.DebugFormat("AnalyzeEvent Type={0},Name={1}", typ.Name, method.ToString)

			If _analyzers.ContainsKey(AttributeAnalyzerTargets.Method) Then
				For Each analyzer As IAttributeAnalyzer In _analyzers(AttributeAnalyzerTargets.Method)
					aspects = analyzer.Analyze(typ, method)
					If aspects Is Nothing Then
						Continue For
					End If
					results.AddRange(aspects)
				Next
			End If

			Return DirectCast(results.ToArray(GetType(IAspect)), IAspect())
		End Function

#End Region

		' ''' <summary>
		' ''' フィールドへインスタンスの注入
		' ''' </summary>
		' ''' <param name="target">対象となるオブジェクト</param>
		' ''' <param name="field">対象となるフィールド</param>
		' ''' <param name="component">対象となるコンポーネント</param>
		' ''' <returns>生成したインスタンス</returns>
		' ''' <remarks></remarks>
		'Protected Overridable Function inject(ByVal target As Object, ByVal field As FieldInfo, ByVal component As MocaComponent) As Object
		'	Dim instance As Object
		'	instance = getInstance(target, component)
		'	ClassUtil.Inject(target, field, New Object() {instance})
		'	Return instance
		'End Function

		''' <summary>
		''' コンポーネントから実態化
		''' </summary>
		''' <param name="component"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Overridable Function getInstance(ByVal component As MocaComponent) As Object
			Return component.Create()
		End Function

		''' <summary>
		''' コンポーネントから実態化
		''' </summary>
		''' <param name="component"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected Overridable Function getInstance(ByVal target As Object, ByVal component As MocaComponent) As Object
			Return component.Create(target)
		End Function

		''' <summary>
		''' 解析除外チェック
		''' </summary>
		''' <param name="val"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function _isIgnore(ByVal val As String) As Boolean
			For Each chkVal As String In _ignoreNamespace
				If val.StartsWith(chkVal) Then
					Return True
				End If
			Next

			Return False
		End Function

#End Region

		''' <summary>
		''' フィールドインジェクションデリゲートプロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Property FieldInject As MocaFieldInject
			Get
				If _injectMethod Is Nothing Then
					_injectMethod = AddressOf Me.inject
				End If
				Return _injectMethod
			End Get
			Set(value As MocaFieldInject)
				_injectMethod = value
			End Set
		End Property

		Public Property FieldInjectType As MocaFieldInjectType
			Get
				If _injectMethodType Is Nothing Then
					_injectMethodType = AddressOf Me.inject
				End If
				Return _injectMethodType
			End Get
			Set(value As MocaFieldInjectType)
				_injectMethodType = value
			End Set
		End Property

		''' <summary>
		''' 実装実験中
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Property EventDelegateInject As MocaEventDelegateInject
			Get
				Return _injectEventDelegate
			End Get
			Set(value As MocaEventDelegateInject)
				_injectEventDelegate = value
			End Set
		End Property

		''' <summary>
		''' １属性解析を追加する
		''' </summary>
		''' <param name="attributeTarget">解析ターゲット</param>
		''' <param name="analyzer">属性解析機</param>
		''' <remarks></remarks>
		Public Sub Add(ByVal attributeTarget As AttributeAnalyzerTargets, ByVal analyzer As IAttributeAnalyzer)
			If Not _analyzers.ContainsKey(attributeTarget) Then
				_analyzers.Add(attributeTarget, New List(Of IAttributeAnalyzer))
			End If

			Dim val As IList(Of IAttributeAnalyzer)
			val = _analyzers(attributeTarget)
			val.Add(analyzer)
		End Sub

		''' <summary>
		''' 解析を除外するNamespaceを追加する
		''' </summary>
		''' <param name="val"></param>
		''' <remarks></remarks>
		Public Sub AddIgnoreNamespace(ByVal val As String)
			If _ignoreNamespace.Contains(val) Then
				Return
			End If

			_ignoreNamespace.Add(val)
		End Sub

		''' <summary>
		''' フィールドへインスタンスの注入
		''' </summary>
		''' <param name="target">対象となるオブジェクト</param>
		''' <param name="field">対象となるフィールド</param>
		''' <param name="component">対象となるコンポーネント</param>
		''' <returns>生成したインスタンス</returns>
		''' <remarks></remarks>
		Protected Function inject(ByVal target As Object, ByVal field As FieldInfo, ByVal component As MocaComponent) As Object
			Dim instance As Object
			instance = component.Create(target)
			ClassUtil.Inject(target, field, New Object() {instance})
			Return instance
		End Function
		Protected Function inject(ByVal target As Type, ByVal field As FieldInfo, ByVal component As MocaComponent) As Object
			Dim instance As Object
			instance = component.Create(target)
			ClassUtil.Inject(target, field, instance)
			Return instance
		End Function

	End Class

End Namespace
