
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Xml.Schema
Imports System.Reflection

Namespace Serialization

	''' <summary>
	''' オブジェクトをXMLファイルにシリアル化又は逆シリアル化するための抽象クラス
	''' </summary>
	''' <remarks></remarks>
	<SerializableAttribute()> _
	Public MustInherit Class DataSerializer

		''' <summary>対象となるXMLファイルパス</summary>
		Protected dataFilename As String
		''' <summary>対象となるXMLファイルのXSDスキーマファイルパス</summary>
		Protected schemaFilename As String

#Region " XmlIgnoreAttribute "

		''' <summary>
		''' 対象となるXMLファイルパスプロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<XmlIgnoreAttribute()> _
		Public Property XmlFilename() As String
			Get
				Return dataFilename
			End Get
			Set(ByVal value As String)
				dataFilename = value
			End Set
		End Property

		''' <summary>
		''' 対象となるXMLファイルのXSDスキーマファイルパスプロパティ
		''' </summary>
		''' <value></value>
		''' <returns></returns>
		''' <remarks></remarks>
		<XmlIgnoreAttribute()> _
		Public Property XsdFilename() As String
			Get
				Return schemaFilename
			End Get
			Set(ByVal value As String)
				schemaFilename = value
			End Set
		End Property

#End Region

#Region " Deserialize "

		''' <summary>
		''' XMLファイルを逆シリアル化します
		''' </summary>
		''' <typeparam name="T">シリアライズ対象のタイプ</typeparam>
		''' <returns></returns>
		''' <remarks>
		''' 既定の名前空間とスキーマ検証は無しで逆シリアル化する。
		''' </remarks>
		Public Function Deserialize(Of T)() As T
			Return Deserialize(Of T)(String.Empty)
		End Function

		''' <summary>
		''' XMLファイルを逆シリアル化します
		''' </summary>
		''' <typeparam name="T">シリアライズ対象のタイプ</typeparam>
		''' <param name="defaultNamespace">既定の名前空間</param>
		''' <returns>逆シリアライズされたオブジェクトのインスタンス</returns>
		''' <remarks>
		''' XsdFilename プロパティにて XMLスキーマファイル（XSD）が指定されているときは、スキーマ検証を実施します。<br/>
		''' スキーマ検証は無しで逆シリアル化する。
		''' </remarks>
		''' <exception cref="ArgumentNullException">XmlFilename プロパティが指定されていないとき</exception>
		''' <exception cref="System.Xml.Schema.XmlSchemaValidationException">XMLスキーマ検証でエラーがあったとき</exception>
		Public Function Deserialize(Of T)(ByVal defaultNamespace As String) As T
			Return Deserialize(Of T)(defaultNamespace, False)
		End Function

		''' <summary>
		''' XMLファイルを逆シリアル化します
		''' </summary>
		''' <typeparam name="T">シリアライズ対象のタイプ</typeparam>
		''' <param name="defaultNamespace">既定の名前空間</param>
		''' <param name="varidate">スキーマ検証の有無</param>
		''' <returns>逆シリアライズされたオブジェクトのインスタンス</returns>
		''' <remarks>
		''' XsdFilename プロパティにて XMLスキーマファイル（XSD）が指定されているときは、スキーマ検証を実施します。<br/>
		''' </remarks>
		''' <exception cref="ArgumentNullException">XmlFilename プロパティが指定されていないとき</exception>
		''' <exception cref="System.Xml.Schema.XmlSchemaValidationException">XMLスキーマ検証でエラーがあったとき</exception>
		Public Function Deserialize(Of T)(ByVal defaultNamespace As String, ByVal varidate As Boolean) As T
			Dim serializer As XmlSerializer
			Dim xml As T

			If dataFilename.Length.Equals(0) Then
				Throw New ArgumentNullException("XmlFilename", "The XML file name is not specified.")
			End If

			' スキーマ検証
			If varidate Then
				VaridateSchema()
			End If

			' XmlSerializerオブジェクトを作成(書き込むオブジェクトの型を指定する)
			If defaultNamespace.Length.Equals(0) Then
				serializer = New XmlSerializer(GetType(T))
			Else
				serializer = New XmlSerializer(GetType(T), defaultNamespace)
			End If

			' XML読み込み
			Using fs As FileStream = New FileStream(dataFilename, System.IO.FileMode.Open)
				Dim reader As New XmlTextReader(fs)
				xml = DirectCast(serializer.Deserialize(reader), T)
			End Using

			Return xml
		End Function

		''' <summary>
		''' XMLファイルのスキーマ検証をします
		''' </summary>
		''' <remarks></remarks>
		''' <exception cref="ArgumentNullException">XsdFilename プロパティが指定されていないとき</exception>
		''' <exception cref="System.Xml.Schema.XmlSchemaValidationException">XMLスキーマ検証でエラーがあったとき</exception>
		Public Sub VaridateSchema()
			If schemaFilename.Length.Equals(0) Then
				Throw New ArgumentNullException("XsdFilename", "The XSD file name is not specified.")
			End If

			Dim sc As XmlSchemaSet = New XmlSchemaSet()

			If schemaFilename.IndexOf(Path.DirectorySeparatorChar) = -1 And schemaFilename.IndexOf(CChar("/")) = -1 Then
				Dim st As Stream = My.Application.GetType.Assembly.GetManifestResourceStream(schemaFilename)
				sc.Add(Nothing, XmlReader.Create(st))
			Else
				sc.Add(Nothing, schemaFilename)
			End If

			Dim doc As XmlDocument = New XmlDocument
			Dim settings As XmlReaderSettings = New XmlReaderSettings()

			settings.ProhibitDtd = False
			'settings.DtdProcessing = DtdProcessing.Prohibit
			settings.ValidationType = ValidationType.Schema
			settings.Schemas = sc

			Using reader As XmlReader = XmlReader.Create(dataFilename, settings)
				doc.Load(reader)
			End Using
		End Sub

#End Region

#Region " Serialize "

		''' <summary>
		''' XMLファイルへシリアル化します
		''' </summary>
		''' <typeparam name="T">シリアライズ対象のタイプ</typeparam>
		''' <remarks></remarks>
		Public Sub Serialize(Of T)()
			Serialize(Of T)(String.Empty)
		End Sub

		''' <summary>
		''' XMLファイルへシリアル化します
		''' </summary>
		''' <typeparam name="T">シリアライズ対象のタイプ</typeparam>
		''' <param name="defaultNamespace">既定の名前空間</param>
		''' <remarks></remarks>
		''' <exception cref="ArgumentNullException">XmlFilename プロパティが指定されていないとき</exception>
		Public Sub Serialize(Of T)(ByVal defaultNamespace As String)
			Dim serializer As XmlSerializer

			If dataFilename.Length.Equals(0) Then
				Throw New ArgumentNullException("XmlFilename", "The XML file name is not specified.")
			End If

			' XmlSerializerオブジェクトを作成(書き込むオブジェクトの型を指定する)
			If defaultNamespace.Length.Equals(0) Then
				serializer = New XmlSerializer(GetType(T))
			Else
				serializer = New XmlSerializer(GetType(T), defaultNamespace)
			End If

			' XML書き込み
			Using fs As FileStream = New FileStream(dataFilename, FileMode.Create)
				serializer.Serialize(fs, Me)
			End Using
		End Sub

#End Region

	End Class

End Namespace
