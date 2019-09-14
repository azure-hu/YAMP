Option Strict On

Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Drawing.Design
Imports System.Drawing

Public Class CustomizableMenuStrip
    Inherits MenuStrip

    Public Event AppearanceControlChanged As EventHandler

    Public Sub New()
        MyBase.New()
    End Sub

    Private _Appearance As AppearanceControl
    Public Property Appearance() As AppearanceControl
        Get
            Return _Appearance
        End Get
        Set(ByVal value As AppearanceControl)
            _Appearance = value
            If value IsNot Nothing Then
                Me.Renderer = value.Renderer
            End If
            Me.Invalidate()
            Me.OnAppearanceControlChanged(EventArgs.Empty)
        End Set
    End Property

    Protected Overridable Sub OnAppearanceControlChanged(ByVal e As EventArgs)
        If Me.Appearance IsNot Nothing Then
            AddHandler Me.Appearance.AppearanceChanged, AddressOf AppearanceControl_AppearanceChanged
            AddHandler Me.Appearance.Disposed, AddressOf AppearanceControl_Disposed
            Me.Renderer = Me.Appearance.Renderer
        Else
            Me.Renderer = New ToolStripProfessionalRenderer()
        End If
        Me.Invalidate()

        RaiseEvent AppearanceControlChanged(Me, e)
    End Sub

    Private Sub AppearanceControl_Disposed(ByVal sender As Object, ByVal e As EventArgs)
        Me.Appearance = Nothing
        Me.OnAppearanceControlChanged(EventArgs.Empty)
    End Sub

    Private Sub AppearanceControl_AppearanceChanged(ByVal sender As Object, ByVal e As EventArgs)
        Me.Renderer = Me.Appearance.Renderer
        Me.Invalidate()
    End Sub

End Class
