Imports System.Reflection
Imports System.Text
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Moca.Entity
Imports Moca.Util
Imports MocaCore20Test

<TestClass()>
Public Class AbstractEntityTest

    Private testContextInstance As TestContext

    '''<summary>
    '''現在のテストの実行についての情報および機能を
    '''提供するテスト コンテキストを取得または設定します。
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(ByVal value As TestContext)
            testContextInstance = value
        End Set
    End Property

#Region "追加のテスト属性"
    '
    ' テストを作成する際には、次の追加属性を使用できます:
    '
    ' クラス内で最初のテストを実行する前に、ClassInitialize を使用してコードを実行してください
    ' <ClassInitialize()> Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    ' End Sub
    '
    ' クラス内のテストをすべて実行したら、ClassCleanup を使用してコードを実行してください
    ' <ClassCleanup()> Public Shared Sub MyClassCleanup()
    ' End Sub
    '
    ' 各テストを実行する前に、TestInitialize を使用してコードを実行してください
    ' <TestInitialize()> Public Sub MyTestInitialize()
    ' End Sub
    '
    ' 各テストを実行した後に、TestCleanup を使用してコードを実行してください
    ' <TestCleanup()> Public Sub MyTestCleanup()
    ' End Sub
    '
#End Region

    <TestMethod()>
    Public Sub TestMethod1()
        Dim values As New TestEntityS
        Dim init As String = "Hoge Test"
        Dim expected As String = init
        Dim actual As String = "ほげ　テスト"

        values.Id = 1
        values.Name = init
        values.Strings.Add("Test")
        values.Strings.Add("Hoge")
        values.Strings2.Add("Moca")
        values.Strings2.Add("Core")
        values.HogeList.Add(New HogeEntity() With {.Code = 1122, .Note = "Test 1122"})
        values.KeyVal = New KeyValuePair(Of Integer, String)(123, "Test Desu")
        values.Hoge.Code = 777
        values.Hoge.Note = "Test 777"

        values.BeginEdit()
        values.Name = actual
        values.Strings.Clear()
        values.Strings.Add("テスト")
        values.Strings.Add("ほげ")
        values.Strings2.Clear()
        values.Strings2.Add("もか")
        values.Strings2.Add("こあ")
        values.HogeList.Clear()
        values.HogeList.Add(New HogeEntity() With {.Code = 2211, .Note = "Test 2211"})
        values.KeyVal = New KeyValuePair(Of Integer, String)(987, "テスト　です")
        values.Hoge.Code = 888
        values.Hoge.Note = "Test 888"
        values.CancelEdit()

        Assert.AreEqual(expected, values.Name)
        Assert.AreEqual(2, values.GetOriginal.Strings.Count)
        Assert.AreEqual("Test", values.GetOriginal.Strings(0))
        Assert.AreEqual("Hoge", values.GetOriginal.Strings(1))
        Assert.AreEqual(2, values.GetOriginal.Strings2.Count)
        Assert.AreEqual("Moca", values.GetOriginal.Strings2(0))
        Assert.AreEqual("Core", values.GetOriginal.Strings2(1))
        Assert.AreEqual(1122, values.GetOriginal.HogeList(0).Code)
        Assert.AreEqual("Test 1122", values.GetOriginal.HogeList(0).Note)
        Assert.AreEqual(123, values.GetOriginal.KeyVal.Key)
        Assert.AreEqual("Test Desu", values.GetOriginal.KeyVal.Value)
        Assert.AreEqual(777, values.GetOriginal.Hoge.Code)
        Assert.AreEqual("Test 777", values.GetOriginal.Hoge.Note)

        Assert.AreEqual(2, values.Strings.Count)
        Assert.AreEqual("Test", values.Strings(0))
        Assert.AreEqual("Hoge", values.Strings(1))
        Assert.AreEqual(2, values.Strings2.Count)
        Assert.AreEqual("Moca", values.Strings2(0))
        Assert.AreEqual("Core", values.Strings2(1))
        Assert.AreEqual(1122, values.HogeList(0).Code)
        Assert.AreEqual("Test 1122", values.HogeList(0).Note)
        Assert.AreEqual(123, values.KeyVal.Key)
        Assert.AreEqual("Test Desu", values.KeyVal.Value)
        Assert.AreEqual(777, values.Hoge.Code)
        Assert.AreEqual("Test 777", values.Hoge.Note)

        values.BeginEdit()
        values.Name = actual
        values.Strings.Clear()
        values.Strings.Add("テスト")
        values.Strings.Add("ほげ")
        values.Strings2.Clear()
        values.Strings2.Add("もか")
        values.Strings2.Add("こあ")
        values.HogeList.Clear()
        values.HogeList.Add(New HogeEntity() With {.Code = 2211, .Note = "Test 2211"})
        values.KeyVal = New KeyValuePair(Of Integer, String)(987, "テスト　です")
        values.Hoge.Code = 888
        values.Hoge.Note = "Test 888"
        values.EndEdit()

        Assert.AreEqual(actual, values.Name)
        Assert.AreEqual(2, values.GetOriginal.Strings.Count)
        Assert.AreEqual("Test", values.GetOriginal.Strings(0))
        Assert.AreEqual("Hoge", values.GetOriginal.Strings(1))
        Assert.AreEqual(2, values.GetOriginal.Strings2.Count)
        Assert.AreEqual("Moca", values.GetOriginal.Strings2(0))
        Assert.AreEqual("Core", values.GetOriginal.Strings2(1))
        Assert.AreEqual(1122, values.GetOriginal.HogeList(0).Code)
        Assert.AreEqual("Test 1122", values.GetOriginal.HogeList(0).Note)
        Assert.AreEqual(123, values.GetOriginal.KeyVal.Key)
        Assert.AreEqual("Test Desu", values.GetOriginal.KeyVal.Value)
        Assert.AreEqual(777, values.GetOriginal.Hoge.Code)
        Assert.AreEqual("Test 777", values.GetOriginal.Hoge.Note)

        Assert.AreEqual(2, values.Strings.Count)
        Assert.AreEqual("テスト", values.Strings(0))
        Assert.AreEqual("ほげ", values.Strings(1))
        Assert.AreEqual(2, values.Strings2.Count)
        Assert.AreEqual("もか", values.Strings2(0))
        Assert.AreEqual("こあ", values.Strings2(1))
        Assert.AreEqual(2211, values.HogeList(0).Code)
        Assert.AreEqual("Test 2211", values.HogeList(0).Note)
        Assert.AreEqual(987, values.KeyVal.Key)
        Assert.AreEqual("テスト　です", values.KeyVal.Value)
        Assert.AreEqual(888, values.Hoge.Code)
        Assert.AreEqual("Test 888", values.Hoge.Note)
    End Sub

    <TestMethod()>
    Public Sub TestMethod2()
        Dim values As New TestEntity
        Dim init As String = "Hoge Test"
        Dim expected As String = init
        Dim actual As String = "ほげ　テスト"

        values.Id = 1
        values.Name = init
        values.Strings.Add("Test")
        values.Strings.Add("Hoge")
        values.Strings2.Add("Moca")
        values.Strings2.Add("Core")
        values.HogeList.Add(New HogeEntity() With {.Code = 1122, .Note = "Test 1122"})
        values.KeyVal = New KeyValuePair(Of Integer, String)(123, "Test Desu")
        values.Hoge.Code = 777
        values.Hoge.Note = "Test 777"

        values.BeginEdit()
        values.Name = actual
        values.Strings.Clear()
        values.Strings.Add("テスト")
        values.Strings.Add("ほげ")
        values.Strings2.Clear()
        values.Strings2.Add("もか")
        values.Strings2.Add("こあ")
        values.HogeList.Clear()
        values.HogeList.Add(New HogeEntity() With {.Code = 2211, .Note = "Test 2211"})
        values.KeyVal = New KeyValuePair(Of Integer, String)(987, "テスト　です")
        values.Hoge.Code = 888
        values.Hoge.Note = "Test 888"
        values.CancelEdit()

        Assert.AreEqual(expected, values.Name)
        Assert.AreEqual(2, values.GetOriginal.Strings.Count)
        Assert.AreEqual("Test", values.GetOriginal.Strings(0))
        Assert.AreEqual("Hoge", values.GetOriginal.Strings(1))
        Assert.AreEqual(2, values.GetOriginal.Strings2.Count)
        Assert.AreEqual("Moca", values.GetOriginal.Strings2(0))
        Assert.AreEqual("Core", values.GetOriginal.Strings2(1))
        Assert.AreEqual(1122, values.GetOriginal.HogeList(0).Code)
        Assert.AreEqual("Test 1122", values.GetOriginal.HogeList(0).Note)
        Assert.AreEqual(123, values.GetOriginal.KeyVal.Key)
        Assert.AreEqual("Test Desu", values.GetOriginal.KeyVal.Value)
        Assert.AreEqual(777, values.GetOriginal.Hoge.Code)
        Assert.AreEqual("Test 777", values.GetOriginal.Hoge.Note)

        Assert.AreEqual(2, values.Strings.Count)
        Assert.AreEqual("Test", values.Strings(0))
        Assert.AreEqual("Hoge", values.Strings(1))
        Assert.AreEqual(2, values.Strings2.Count)
        Assert.AreEqual("Moca", values.Strings2(0))
        Assert.AreEqual("Core", values.Strings2(1))
        Assert.AreEqual(1122, values.HogeList(0).Code)
        Assert.AreEqual("Test 1122", values.HogeList(0).Note)
        Assert.AreEqual(123, values.KeyVal.Key)
        Assert.AreEqual("Test Desu", values.KeyVal.Value)
        Assert.AreEqual(777, values.Hoge.Code)
        Assert.AreEqual("Test 777", values.Hoge.Note)

        values.BeginEdit()
        values.Name = actual
        values.Strings.Clear()
        values.Strings.Add("テスト")
        values.Strings.Add("ほげ")
        values.Strings2.Clear()
        values.Strings2.Add("もか")
        values.Strings2.Add("こあ")
        values.HogeList.Clear()
        values.HogeList.Add(New HogeEntity() With {.Code = 2211, .Note = "Test 2211"})
        values.KeyVal = New KeyValuePair(Of Integer, String)(987, "テスト　です")
        values.Hoge.Code = 888
        values.Hoge.Note = "Test 888"
        values.EndEdit()

        Assert.AreEqual(actual, values.Name)
        Assert.AreEqual(2, values.GetOriginal.Strings.Count)
        Assert.AreEqual("Test", values.GetOriginal.Strings(0))
        Assert.AreEqual("Hoge", values.GetOriginal.Strings(1))
        Assert.AreEqual(2, values.GetOriginal.Strings2.Count)
        Assert.AreEqual("Moca", values.GetOriginal.Strings2(0))
        Assert.AreEqual("Core", values.GetOriginal.Strings2(1))
        Assert.AreEqual(1122, values.GetOriginal.HogeList(0).Code)
        Assert.AreEqual("Test 1122", values.GetOriginal.HogeList(0).Note)
        Assert.AreEqual(123, values.GetOriginal.KeyVal.Key)
        Assert.AreEqual("Test Desu", values.GetOriginal.KeyVal.Value)
        Assert.AreEqual(777, values.GetOriginal.Hoge.Code)
        Assert.AreEqual("Test 777", values.GetOriginal.Hoge.Note)

        Assert.AreEqual(2, values.Strings.Count)
        Assert.AreEqual("テスト", values.Strings(0))
        Assert.AreEqual("ほげ", values.Strings(1))
        Assert.AreEqual(2, values.Strings2.Count)
        Assert.AreEqual("もか", values.Strings2(0))
        Assert.AreEqual("こあ", values.Strings2(1))
        Assert.AreEqual(2211, values.HogeList(0).Code)
        Assert.AreEqual("Test 2211", values.HogeList(0).Note)
        Assert.AreEqual(987, values.KeyVal.Key)
        Assert.AreEqual("テスト　です", values.KeyVal.Value)
        Assert.AreEqual(888, values.Hoge.Code)
        Assert.AreEqual("Test 888", values.Hoge.Note)
    End Sub

End Class


<Serializable()>
Public Class TestEntityS
    Inherits EntityBase(Of TestEntityS)

    Public Sub New()
        _Strings = New List(Of String)
        _Strings2 = New List(Of String)
        _Strings3 = New ArrayList
        _HogeList = New List(Of HogeEntity)
        _Hoge = New HogeEntity
    End Sub

    Public Property Id As Integer
    Public Property Name As String
    Public ReadOnly Property Strings As IList(Of String)
    Public Property Strings2 As IList(Of String)
    Public Property Strings3 As IList
    Public Property HogeList As IList(Of HogeEntity)
    Public Property KeyVal As KeyValuePair(Of Integer, String)
    Public Property Hoge As HogeEntity

    Public Overrides Sub DeepCopy(valueTo As TestEntityS)
        MyBase.DeepCopy(valueTo)

        valueTo.Hoge = New HogeEntity() With {
            .Code = Me.Hoge.Code,
            .Note = Me.Hoge.Note
        }
    End Sub

End Class

Public Class TestEntity
    Inherits EntityBase(Of TestEntity)

    Public Sub New()
        _Strings = New List(Of String)
        _Strings2 = New List(Of String)
        _Strings3 = New ArrayList
        _HogeList = New List(Of HogeEntity)
        _Hoge = New HogeEntity
    End Sub

    Public Property Id As Integer
    Public Property Name As String
    Public ReadOnly Property Strings As IList(Of String)
    Public Property Strings2 As IList(Of String)
    Public Property Strings3 As IList
    Public Property HogeList As IList(Of HogeEntity)
    Public Property KeyVal As KeyValuePair(Of Integer, String)
    Public Property Hoge As HogeEntity

    Public Overrides Sub DeepCopy(valueTo As TestEntity)
        MyBase.DeepCopy(valueTo)

        valueTo.Hoge = New HogeEntity() With {
            .Code = Me.Hoge.Code,
            .Note = Me.Hoge.Note
        }
    End Sub

End Class

<Serializable()>
Public Class HogeEntity

    Public Property Code As Integer
    Public Property Note As String

End Class