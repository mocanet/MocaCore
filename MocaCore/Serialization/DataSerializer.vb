
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Xml.Schema
Imports System.Reflection

Namespace Serialization

	''' <summary>
	''' �I�u�W�F�N�g��XML�t�@�C���ɃV���A�������͋t�V���A�������邽�߂̒��ۃN���X
	''' </summary>
	''' <remarks></remarks>
	<SerializableAttribute()> _
	Public MustInherit Class DataSerializer

		''' <summary>�ΏۂƂȂ�XML�t�@�C���p�X</summary>
		Protected dataFilename As String
		''' <summary>�ΏۂƂȂ�XML�t�@�C����XSD�X�L�[�}�t�@�C���p�X</summary>
		Protected schemaFilename As String

#Region " XmlIgnoreAttribute "

		''' <summary>
		''' �ΏۂƂȂ�XML�t�@�C���p�X�v���p�e�B
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
		''' �ΏۂƂȂ�XML�t�@�C����XSD�X�L�[�}�t�@�C���p�X�v���p�e�B
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
		''' XML�t�@�C�����t�V���A�������܂�
		''' </summary>
		''' <typeparam name="T">�V���A���C�Y�Ώۂ̃^�C�v</typeparam>
		''' <returns></returns>
		''' <remarks>
		''' ����̖��O��ԂƃX�L�[�}���؂͖����ŋt�V���A��������B
		''' </remarks>
		Public Function Deserialize(Of T)() As T
			Return Deserialize(Of T)(String.Empty)
		End Function

		''' <summary>
		''' XML�t�@�C�����t�V���A�������܂�
		''' </summary>
		''' <typeparam name="T">�V���A���C�Y�Ώۂ̃^�C�v</typeparam>
		''' <param name="defaultNamespace">����̖��O���</param>
		''' <returns>�t�V���A���C�Y���ꂽ�I�u�W�F�N�g�̃C���X�^���X</returns>
		''' <remarks>
		''' XsdFilename �v���p�e�B�ɂ� XML�X�L�[�}�t�@�C���iXSD�j���w�肳��Ă���Ƃ��́A�X�L�[�}���؂����{���܂��B<br/>
		''' �X�L�[�}���؂͖����ŋt�V���A��������B
		''' </remarks>
		''' <exception cref="ArgumentNullException">XmlFilename �v���p�e�B���w�肳��Ă��Ȃ��Ƃ�</exception>
		''' <exception cref="System.Xml.Schema.XmlSchemaValidationException">XML�X�L�[�}���؂ŃG���[���������Ƃ�</exception>
		Public Function Deserialize(Of T)(ByVal defaultNamespace As String) As T
			Return Deserialize(Of T)(defaultNamespace, False)
		End Function

		''' <summary>
		''' XML�t�@�C�����t�V���A�������܂�
		''' </summary>
		''' <typeparam name="T">�V���A���C�Y�Ώۂ̃^�C�v</typeparam>
		''' <param name="defaultNamespace">����̖��O���</param>
		''' <param name="varidate">�X�L�[�}���؂̗L��</param>
		''' <returns>�t�V���A���C�Y���ꂽ�I�u�W�F�N�g�̃C���X�^���X</returns>
		''' <remarks>
		''' XsdFilename �v���p�e�B�ɂ� XML�X�L�[�}�t�@�C���iXSD�j���w�肳��Ă���Ƃ��́A�X�L�[�}���؂����{���܂��B<br/>
		''' </remarks>
		''' <exception cref="ArgumentNullException">XmlFilename �v���p�e�B���w�肳��Ă��Ȃ��Ƃ�</exception>
		''' <exception cref="System.Xml.Schema.XmlSchemaValidationException">XML�X�L�[�}���؂ŃG���[���������Ƃ�</exception>
		Public Function Deserialize(Of T)(ByVal defaultNamespace As String, ByVal varidate As Boolean) As T
			Dim serializer As XmlSerializer
			Dim xml As T

			If dataFilename.Length.Equals(0) Then
				Throw New ArgumentNullException("XmlFilename", "The XML file name is not specified.")
			End If

			' �X�L�[�}����
			If varidate Then
				VaridateSchema()
			End If

			' XmlSerializer�I�u�W�F�N�g���쐬(�������ރI�u�W�F�N�g�̌^���w�肷��)
			If defaultNamespace.Length.Equals(0) Then
				serializer = New XmlSerializer(GetType(T))
			Else
				serializer = New XmlSerializer(GetType(T), defaultNamespace)
			End If

			' XML�ǂݍ���
			Using fs As FileStream = New FileStream(dataFilename, System.IO.FileMode.Open)
				Dim reader As New XmlTextReader(fs)
				xml = DirectCast(serializer.Deserialize(reader), T)
			End Using

			Return xml
		End Function

		''' <summary>
		''' XML�t�@�C���̃X�L�[�}���؂����܂�
		''' </summary>
		''' <remarks></remarks>
		''' <exception cref="ArgumentNullException">XsdFilename �v���p�e�B���w�肳��Ă��Ȃ��Ƃ�</exception>
		''' <exception cref="System.Xml.Schema.XmlSchemaValidationException">XML�X�L�[�}���؂ŃG���[���������Ƃ�</exception>
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
		''' XML�t�@�C���փV���A�������܂�
		''' </summary>
		''' <typeparam name="T">�V���A���C�Y�Ώۂ̃^�C�v</typeparam>
		''' <remarks></remarks>
		Public Sub Serialize(Of T)()
			Serialize(Of T)(String.Empty)
		End Sub

		''' <summary>
		''' XML�t�@�C���փV���A�������܂�
		''' </summary>
		''' <typeparam name="T">�V���A���C�Y�Ώۂ̃^�C�v</typeparam>
		''' <param name="defaultNamespace">����̖��O���</param>
		''' <remarks></remarks>
		''' <exception cref="ArgumentNullException">XmlFilename �v���p�e�B���w�肳��Ă��Ȃ��Ƃ�</exception>
		Public Sub Serialize(Of T)(ByVal defaultNamespace As String)
			Dim serializer As XmlSerializer

			If dataFilename.Length.Equals(0) Then
				Throw New ArgumentNullException("XmlFilename", "The XML file name is not specified.")
			End If

			' XmlSerializer�I�u�W�F�N�g���쐬(�������ރI�u�W�F�N�g�̌^���w�肷��)
			If defaultNamespace.Length.Equals(0) Then
				serializer = New XmlSerializer(GetType(T))
			Else
				serializer = New XmlSerializer(GetType(T), defaultNamespace)
			End If

			' XML��������
			Using fs As FileStream = New FileStream(dataFilename, FileMode.Create)
				serializer.Serialize(fs, Me)
			End Using
		End Sub

#End Region

	End Class

End Namespace
