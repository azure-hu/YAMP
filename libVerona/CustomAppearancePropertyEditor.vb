Imports System.Windows.Forms.Design
Imports System.Drawing.Design
Imports System.ComponentModel
Imports System.Windows.Forms

Public Class CustomAppearancePropertyEditor
    Inherits System.Drawing.Design.UITypeEditor

    Private WithEvents _appearanceEditor As frmAppearanceEditor
    Protected IEditorService As IWindowsFormsEditorService
    Private WithEvents m_EditControl As Control
    Private m_EscapePressed As Boolean

    Public Overrides Function GetEditStyle(ByVal context As System.ComponentModel.ITypeDescriptorContext) As UITypeEditorEditStyle
        Return UITypeEditorEditStyle.Modal
    End Function

    Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
        Try
            If context IsNot Nothing AndAlso provider IsNot Nothing Then

                'Uses the IWindowsFormsEditorService to display a modal dialog form
                IEditorService = DirectCast(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
                If IEditorService IsNot Nothing Then

                    Dim PropName As String = context.PropertyDescriptor.Name
                    m_EditControl = Me.GetEditControl(PropName, value)
                    If m_EditControl IsNot Nothing Then
                        IEditorService.ShowDialog(CType(m_EditControl, Form))

                        'Notify that our control has changed; otherwise changes are not stored
                        context.OnComponentChanged()

                        Return Me.GetEditedValue(m_EditControl, PropName, value)
                    End If
                End If
            End If
        Catch ex As Exception
        End Try
        Return MyBase.EditValue(context, provider, value)
    End Function

    Private Function GetEditControl(ByVal PropertyName As String, ByVal CurrentValue As Object) As System.Windows.Forms.Control
        Dim ap As AppearanceControl.AppearanceProperties = TryCast(CurrentValue, AppearanceControl.AppearanceProperties)
        If ap IsNot Nothing Then
            _appearanceEditor = New frmAppearanceEditor(ap)
            Return _appearanceEditor
        Else
            Return Nothing
        End If
    End Function

    Private Function GetEditedValue(ByVal EditControl As System.Windows.Forms.Control, ByVal PropertyName As String, ByVal OldValue As Object) As Object
        If _appearanceEditor Is Nothing _
        OrElse _appearanceEditor.DialogResult = DialogResult.Cancel Then
            Return OldValue
        Else
            Return _appearanceEditor.CustomAppearance
        End If
    End Function

End Class
