Imports System.Windows.Forms
Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO

Public Class frmAppearanceEditor

    Private _ap As AppearanceControl.AppearanceProperties = Nothing

    Public Sub New(ByVal ap As AppearanceControl.AppearanceProperties)
        InitializeComponent()
        _ap = ap
        Me.CustomizableMenuStrip1.Appearance.CustomAppearance = ap
        Me.CustomizableStatusStrip1.Appearance.CustomAppearance = ap
        Me.CustomizableToolStrip1.Appearance.CustomAppearance = ap
        Me.PropertyGrid1.SelectedObject = ap
    End Sub

    Public ReadOnly Property CustomAppearance() As AppearanceControl.AppearanceProperties
        Get
            Return _ap
        End Get
    End Property

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Load_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Load_Button.Click
        Using ofd As New OpenFileDialog
            ofd.Title = "Select XML File."
            ofd.Filter = "XML Files (*.xml)|*.xml|All Files|*.*"

            If ofd.ShowDialog = Windows.Forms.DialogResult.OK Then
                Me.LoadAppearance(ofd.FileName)
                CustomizableMenuStrip1.Invalidate()
                CustomizableToolStrip1.Invalidate()
                CustomizableStatusStrip1.Invalidate()
            End If
        End Using
    End Sub

    Private Sub Save_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Save_Button.Click
        Using sfd As New SaveFileDialog
            sfd.Title = "Select XML File."
            sfd.Filter = "XML Files (*.xml)|*.xml|All Files|*.*"

            If sfd.ShowDialog = Windows.Forms.DialogResult.OK Then
                Me.SaveAppearance(sfd.FileName, AppearanceControl1)
            End If
        End Using
    End Sub

    Public Sub LoadAppearance(ByVal xmlFile As String)
        Try
            Using fs As New FileStream(xmlFile, FileMode.Open, FileAccess.Read, FileShare.None)
                Dim ser As New XmlSerializer(GetType(AppearanceControl.AppearanceProperties))

                Dim ap As AppearanceControl.AppearanceProperties
                ap = DirectCast(ser.Deserialize(fs), AppearanceControl.AppearanceProperties)
                _ap = ap
                Me.CustomizableMenuStrip1.Appearance.CustomAppearance = ap
                Me.CustomizableStatusStrip1.Appearance.CustomAppearance = ap
                Me.CustomizableToolStrip1.Appearance.CustomAppearance = ap
                Me.PropertyGrid1.SelectedObject = ap
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub LoadFromStream(ByVal stream As Stream)
        Try
            Dim ser As New XmlSerializer(GetType(AppearanceControl.AppearanceProperties))

            Dim ap As AppearanceControl.AppearanceProperties
            ap = DirectCast(ser.Deserialize(stream), AppearanceControl.AppearanceProperties)
            ap.SetAppearanceControl(AppearanceControl1)
            _ap = ap
            Me.AppearanceControl1.CustomAppearance = _ap

            stream.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Public Sub SaveAppearance(ByVal xmlFile As String, ByVal ac As AppearanceControl)
        Try
            Using fs As New FileStream(xmlFile, FileMode.Create, FileAccess.Write, FileShare.None)
                Dim ser As New XmlSerializer(GetType(AppearanceControl.AppearanceProperties))
                ser.Serialize(fs, ac.CustomAppearance)
                fs.Close()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
    End Sub

    Private Sub PropertyGrid1_PropertyValueChanged(ByVal s As System.Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles PropertyGrid1.PropertyValueChanged
        Me.CustomizableMenuStrip1.Invalidate()
        Me.CustomizableStatusStrip1.Invalidate()
        Me.CustomizableToolStrip1.Invalidate()
    End Sub
End Class