
Imports System.Reflection
Imports Moca.Util
Imports Moca.Attr
Imports Moca.Db
Imports Moca.Db.Attr

Namespace Di

	''' <summary>
	''' 依存性の注入
	''' </summary>
	''' <remarks>
	''' インタフェースの属性に指定された実態クラスをインスタンス化しフィールドへ注入する。<br/>
	''' 実装クラスの指定は、<see cref="ImplementationAttribute"/> 属性を使用して指定します。<br/>
	''' 引数で指定されたインスタンスのフィールドに対して、この属性を指定されたインタフェースが存在したときは、
	''' 自動でインスタンス化してフィールドに注入します。<br/>
	''' よって、これらのフィールドはインスタンス化（<c>New</c>）する必要はありません。<br/>
	''' ※インスタンス化するなら <see cref="ImplementationAttribute"/> 属性は指定しないでください。<br/>
	''' </remarks>
	Public Class MocaInjector
		Implements IDisposable

		''' <summary>属性解析</summary>
		Private _analyzer As AttributeAnalyzer

		Private _targets As ArrayList


		''' <summary>Logging For Log4net</summary>
		Private ReadOnly _mylog As log4net.ILog = log4net.LogManager.GetLogger(String.Empty)

#Region " コンストラクタ "

		''' <summary>
		''' デフォルトコンストラクタ
		''' </summary>
		''' <remarks></remarks>
		Public Sub New()
			_analyzer = New AttributeAnalyzer

			' デフォルトの属性解析設定
			_analyzer.Add(AttributeAnalyzerTargets.Field, New ImplementationAttributeAnalyzer)
			_analyzer.Add(AttributeAnalyzerTargets.Field, New DaoAttributeAnalyzer)
			_analyzer.Add(AttributeAnalyzerTargets.Field, New TableAttributeAnalyzer)
            _analyzer.Add(AttributeAnalyzerTargets.Method, New TransactionAttributeAnalyzer)
            _analyzer.Add(AttributeAnalyzerTargets.Method, New AspectAttributeAnalyzer)

            _analyzer.AddIgnoreNamespace("System")
			_analyzer.AddIgnoreNamespace("Microsoft")
			_analyzer.AddIgnoreNamespace("log4net")
			_analyzer.AddIgnoreNamespace("Moca.Db.AbstractDao")

			_analyzer.FieldInject = AddressOf Me.fieldInject

			_targets = New ArrayList
		End Sub

#End Region
#Region " プロパティ "

		''' <summary>
		''' 属性解析プロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		Protected ReadOnly Property Analyzer() As AttributeAnalyzer
			Get
				Return _analyzer
			End Get
		End Property

#End Region
#Region " Dispose "

		Private disposedValue As Boolean = False		' 重複する呼び出しを検出するには

		' IDisposable
		Protected Overridable Sub Dispose(ByVal disposing As Boolean)
			If Not Me.disposedValue Then
				If disposing Then
					' TODO: 明示的に呼び出されたときにマネージ リソースを解放します
				End If

				' TODO: 共有のアンマネージ リソースを解放します
				For Each target As Object In _targets
					Dim targetDispose As IDisposable
					DaoDispose(target)
					targetDispose = TryCast(target, IDisposable)
					If targetDispose IsNot Nothing Then
						targetDispose.Dispose()
					End If
				Next
			End If
			Me.disposedValue = True
		End Sub

#Region " IDisposable Support "
		' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
		Public Sub Dispose() Implements IDisposable.Dispose
			' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
			Dispose(True)
			GC.SuppressFinalize(Me)
		End Sub
#End Region

#End Region

		''' <summary>
		''' 依存性の注入してインスタンスを生成する
		''' </summary>
		''' <param name="target"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function Create(ByVal target As Type) As Object
			Dim instance As Object
			instance = _analyzer.Create(target)
			_targets.Add(instance)
			_mylog.DebugFormat("Inject -> {0}", target.GetType.FullName)
			Return instance
		End Function

		''' <summary>
		''' 依存性の注入
		''' </summary>
		''' <param name="target"></param>
		''' <remarks>
		''' 指定されたインスタンスのフィールドで Interface のみに対応しています。
		''' </remarks>
		Public Overridable Sub Inject(ByVal target As Object)
			_analyzer.Analyze(target)
			_targets.Add(target)
			_mylog.DebugFormat("Inject -> {0}", target.GetType.FullName)
		End Sub

		''' <summary>
		''' DAO インスタンスの開放
		''' </summary>
		''' <param name="target"></param>
		''' <remarks>
		''' </remarks>
		Public Overridable Sub DaoDispose(ByVal target As Object)
			Dim fields() As FieldInfo

			If target Is Nothing Then
				Exit Sub
			End If

			' フィールドをチェックする
			fields = ClassUtil.GetFields(target)
			For Each field As FieldInfo In fields
				' Inject したオブジェクトなら再帰処理する
				If MocaContainerFactory.Container.GetComponent(field.GetType) IsNot Nothing Then
					DaoDispose(field.GetValue(target))
				End If

				' Dao 属性がある？
				Dim dbmsAttr As DbmsAttribute
				dbmsAttr = ClassUtil.GetCustomAttribute(Of DbmsAttribute)(field.FieldType)
				If dbmsAttr Is Nothing Then
					Continue For
				End If

				' Dispose 実行
				Dim dao As IDao
				dao = DirectCast(field.GetValue(target), IDao)
				If dao Is Nothing Then
					Continue For
				End If
				dao.Dispose()
				_mylog.DebugFormat("DAO Dispose -> {0} ({1})", target.GetType.FullName, field.ToString)
			Next
		End Sub

		''' <summary>
		''' フィールドへインスタンスの注入
		''' </summary>
		''' <param name="target">対象となるオブジェクト</param>
		''' <param name="field">対象となるフィールド</param>
		''' <param name="component">対象となるコンポーネント</param>
		''' <returns>生成したインスタンス</returns>
		''' <remarks></remarks>
		Protected Function fieldInject(ByVal target As Object, ByVal field As FieldInfo, ByVal component As MocaComponent) As Object
			Dim instance As Object
			instance = component.Create(target)
			ClassUtil.Inject(target, field, New Object() {instance})
			Return instance
		End Function

	End Class

End Namespace
