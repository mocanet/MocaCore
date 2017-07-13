Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Microsoft.Data.Tools.Schema.Sql.UnitTesting
Imports Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions
Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass()>
Public Class SqlServerUnitTest1
    Inherits SqlDatabaseTestClass

    Sub New()
        InitializeComponent()
    End Sub

    <TestInitialize()>
    Public Sub TestInitialize()
        InitializeTest()
    End Sub

    <TestCleanup()>
    Public Sub TestCleanup()
        CleanupTest()
    End Sub

#Region "Designer support code"

    'NOTE: The following procedure is required by the Designer
    'It can be modified using the Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim dbo_DaoUser_FindTest_TestAction As Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SqlServerUnitTest1))
        Dim InconclusiveCondition1 As Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition
        Me.dbo_DaoUser_FindTestData = New Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestActions()
        dbo_DaoUser_FindTest_TestAction = New Microsoft.Data.Tools.Schema.Sql.UnitTesting.SqlDatabaseTestAction()
        InconclusiveCondition1 = New Microsoft.Data.Tools.Schema.Sql.UnitTesting.Conditions.InconclusiveCondition()
        '
        'dbo_DaoUser_FindTestData
        '
        Me.dbo_DaoUser_FindTestData.PosttestAction = Nothing
        Me.dbo_DaoUser_FindTestData.PretestAction = Nothing
        Me.dbo_DaoUser_FindTestData.TestAction = dbo_DaoUser_FindTest_TestAction
        '
        'dbo_DaoUser_FindTest_TestAction
        '
        dbo_DaoUser_FindTest_TestAction.Conditions.Add(InconclusiveCondition1)
        resources.ApplyResources(dbo_DaoUser_FindTest_TestAction, "dbo_DaoUser_FindTest_TestAction")
        '
        'InconclusiveCondition1
        '
        InconclusiveCondition1.Enabled = true
        InconclusiveCondition1.Name = "InconclusiveCondition1"
    End Sub

#End Region

#Region "Additional test attributes"
    '
    ' You can use the following additional attributes as you write your tests:
    '
    ' Use ClassInitialize to run code before running the first test in the class
    ' <ClassInitialize()> Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    ' End Sub
    '
    ' Use ClassCleanup to run code after all tests in a class have run
    ' <ClassCleanup()> Public Shared Sub MyClassCleanup()
    ' End Sub
    '
#End Region

    <TestMethod()> _
    Public Sub dbo_DaoUser_FindTest()
        Dim testActions As SqlDatabaseTestActions = Me.dbo_DaoUser_FindTestData
        '事前テスト スクリプトを実行します
        '
        System.Diagnostics.Trace.WriteLineIf((Not (testActions.PretestAction) Is Nothing), "事前テスト スクリプトを実行しています...")
        Dim pretestResults() As SqlExecutionResult = TestService.Execute(Me.PrivilegedContext, Me.PrivilegedContext, testActions.PretestAction)
        Try
            'テスト スクリプトを実行します
            '
            System.Diagnostics.Trace.WriteLineIf((Not (testActions.TestAction) Is Nothing), "テスト スクリプトを実行しています...")
            Dim testResults() As SqlExecutionResult = TestService.Execute(Me.ExecutionContext, Me.PrivilegedContext, testActions.TestAction)
        Finally
            '事後テスト スクリプトを実行します
            '
            System.Diagnostics.Trace.WriteLineIf((Not (testActions.PosttestAction) Is Nothing), "事後テスト スクリプトを実行しています...")
            Dim posttestResults() As SqlExecutionResult = TestService.Execute(Me.PrivilegedContext, Me.PrivilegedContext, testActions.PosttestAction)
        End Try
    End Sub
    Private dbo_DaoUser_FindTestData As SqlDatabaseTestActions
End Class

