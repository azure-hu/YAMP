Imports System.Windows.Forms.Design
Imports System.ComponentModel, System.ComponentModel.Design
Imports System.Xml.Serialization
Imports System.Drawing.Design
Imports System.Drawing
Imports System.Windows.Forms

Public Class AppearanceControl
    Inherits Component

    Public Event AppearanceChanged As EventHandler

    Private customRenderer As ToolStripProfessionalRenderer = Nothing
    Private office2007Renderer As ToolStripProfessionalRenderer = Nothing
    Private blueRenderer As ToolStripProfessionalRenderer = Nothing
    Private silverRenderer As ToolStripProfessionalRenderer = Nothing
    Private oliveRenderer As ToolStripProfessionalRenderer = Nothing
    Private xpRenderer As ToolStripProfessionalRenderer = Nothing
    Private classicRenderer As ToolStripProfessionalRenderer = Nothing
    Private blackRenderer As ToolStripProfessionalRenderer = Nothing

    Public Enum enumPresetStyles
        Custom = 0I
        Office2007 = 1I
        Office2003Blue = 2I
        Office2003Silver = 3I
        Office2003Olive = 4I
        OfficeXP = 5I
        OfficeClassic = 6I
        Office2007Black = 7I
    End Enum

    Public Sub New()
        customRenderer = New ToolStripProfessionalRenderer(New CustomColorTable(Me))
        office2007Renderer = New ToolStripProfessionalRenderer(New Office2007ColorTable)
        blueRenderer = New ToolStripProfessionalRenderer(New Office2003BlueColorTable)
        silverRenderer = New ToolStripProfessionalRenderer(New Office2003SilverColorTable)
        oliveRenderer = New ToolStripProfessionalRenderer(New Office2003OliveColorTable)
        xpRenderer = New ToolStripProfessionalRenderer(New OfficeXPColorTable)
        classicRenderer = New ToolStripProfessionalRenderer(New OfficeClassicColorTable)
        blackRenderer = New ToolStripProfessionalRenderer(New Office2007BlackColorTable)

        _Renderer = customRenderer
        _CustomAppearance = New AppearanceProperties(Me)
    End Sub

    Private _Preset As enumPresetStyles
    <Category("Appearance")> _
    Public Property Preset() As enumPresetStyles
        Get
            Return _Preset
        End Get
        Set(ByVal value As enumPresetStyles)
            _Preset = value

            Select Case value
                Case enumPresetStyles.Custom
                    Me.Renderer = customRenderer
                Case enumPresetStyles.Office2003Blue
                    Me.Renderer = blueRenderer
                Case enumPresetStyles.Office2003Olive
                    Me.Renderer = oliveRenderer
                Case enumPresetStyles.Office2003Silver
                    Me.Renderer = silverRenderer
                Case enumPresetStyles.Office2007
                    Me.Renderer = office2007Renderer
                Case enumPresetStyles.OfficeClassic
                    Me.Renderer = classicRenderer
                Case enumPresetStyles.OfficeXP
                    Me.Renderer = xpRenderer
                Case enumPresetStyles.Office2007Black
                    Me.Renderer = blackRenderer
            End Select

            Me.OnAppearanceChanged(EventArgs.Empty)
        End Set
    End Property

    Private _Renderer As ToolStripProfessionalRenderer = Nothing
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    <Browsable(False)> _
    Public Property Renderer() As ToolStripProfessionalRenderer
        Get
            Return _Renderer
        End Get
        Set(ByVal value As ToolStripProfessionalRenderer)
            _Renderer = value
        End Set
    End Property

    Private _CustomAppearance As AppearanceProperties
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
    <Category("Appearance")> _
    <Editor(GetType(CustomAppearancePropertyEditor), GetType(UITypeEditor))> _
    Public Property CustomAppearance() As AppearanceProperties
        Get
            Return _CustomAppearance
        End Get
        Set(ByVal value As AppearanceProperties)
            _CustomAppearance = value
        End Set
    End Property

    <Serializable()> _
    Public Class AppearanceProperties

        'Parameterless ctor required for serialization
        Public Sub New()
        End Sub

        Public Sub SetAppearanceControl(ByVal ap As AppearanceControl)
            _ButtonAppearance = New ButtonAppearanceProperties(ap)
            _ButtonAppearance.SelectedAppearance.SetAppearanceControl(ap)
            _ButtonAppearance.PressedAppearance.SetAppearanceControl(ap)
            _ButtonAppearance.CheckedAppearance.SetAppearanceControl(ap)
            _GripAppearance.SetAppearanceControl(ap)
            _ImageMarginAppearance = New ImageMarginAppearanceProperties(ap)
            _ImageMarginAppearance.Normal.SetAppearanceControl(ap)
            _ImageMarginAppearance.Revealed.SetAppearanceControl(ap)
            _MenuStripAppearance.SetAppearanceControl(ap)
            _MenuItemAppearance.SetAppearanceControl(ap)
            _RaftingContainerAppearance.SetAppearanceControl(ap)
            _SeparatorAppearance.SetAppearanceControl(ap)
            _StatusStripAppearance.SetAppearanceControl(ap)
            _ToolStripAppearance.SetAppearanceControl(ap)
            _OverflowButtonAppearance.SetAppearanceControl(ap)
        End Sub

        Public Sub New(ByVal appearanceControl As AppearanceControl)
            _ButtonAppearance = New ButtonAppearanceProperties(appearanceControl)
            _GripAppearance = New GripAppearanceProperties(appearanceControl)
            _ImageMarginAppearance = New ImageMarginAppearanceProperties(appearanceControl)
            _MenuStripAppearance = New MenustripAppearanceProperties(appearanceControl)
            _MenuItemAppearance = New MenuItemAppearanceProperties(appearanceControl)
            _RaftingContainerAppearance = New RaftingContainerAppearanceProperties(appearanceControl)
            _SeparatorAppearance = New SeparatorAppearanceProperties(appearanceControl)
            _StatusStripAppearance = New StatusStripAppearanceProperties(appearanceControl)
            _ToolStripAppearance = New ToolstripAppearanceProperties(appearanceControl)
            _OverflowButtonAppearance = New OverflowButtonAppearanceProperties(appearanceControl)
        End Sub

        Private _ButtonAppearance As ButtonAppearanceProperties
        <TypeConverter(GetType(ExpandableObjectConverter))> _
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
        <Category("Appearance")> _
        Public Property ButtonAppearance() As ButtonAppearanceProperties
            Get
                Return _ButtonAppearance
            End Get
            Set(ByVal value As ButtonAppearanceProperties)
                _ButtonAppearance = value
            End Set
        End Property

        Private _GripAppearance As GripAppearanceProperties
        <TypeConverter(GetType(ExpandableObjectConverter))> _
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
        <Category("Appearance")> _
        Public Property GripAppearance() As GripAppearanceProperties
            Get
                Return _GripAppearance
            End Get
            Set(ByVal value As GripAppearanceProperties)
                _GripAppearance = value
            End Set
        End Property

        Private _ImageMarginAppearance As ImageMarginAppearanceProperties
        <TypeConverter(GetType(ExpandableObjectConverter))> _
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
        <Category("Appearance")> _
        Public Property ImageMarginAppearance() As ImageMarginAppearanceProperties
            Get
                Return _ImageMarginAppearance
            End Get
            Set(ByVal value As ImageMarginAppearanceProperties)
                _ImageMarginAppearance = value
            End Set
        End Property

        Private _MenuStripAppearance As MenustripAppearanceProperties
        <TypeConverter(GetType(ExpandableObjectConverter))> _
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
        <Category("Appearance")> _
        Public Property MenuStripAppearance() As MenustripAppearanceProperties
            Get
                Return _MenuStripAppearance
            End Get
            Set(ByVal value As MenustripAppearanceProperties)
                _MenuStripAppearance = value
            End Set
        End Property

        Private _MenuItemAppearance As MenuItemAppearanceProperties
        <TypeConverter(GetType(ExpandableObjectConverter))> _
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
        <Category("Appearance")> _
        Public Property MenuItemAppearance() As MenuItemAppearanceProperties
            Get
                Return _MenuItemAppearance
            End Get
            Set(ByVal value As MenuItemAppearanceProperties)
                _MenuItemAppearance = value
            End Set
        End Property

        Private _RaftingContainerAppearance As RaftingContainerAppearanceProperties
        <TypeConverter(GetType(ExpandableObjectConverter))> _
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
        <Category("Appearance")> _
        Public Property RaftingContainerAppearance() As RaftingContainerAppearanceProperties
            Get
                Return _RaftingContainerAppearance
            End Get
            Set(ByVal value As RaftingContainerAppearanceProperties)
                _RaftingContainerAppearance = value
            End Set
        End Property

        Private _SeparatorAppearance As SeparatorAppearanceProperties
        <TypeConverter(GetType(ExpandableObjectConverter))> _
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
        <Category("Appearance")> _
        Public Property SeparatorAppearance() As SeparatorAppearanceProperties
            Get
                Return _SeparatorAppearance
            End Get
            Set(ByVal value As SeparatorAppearanceProperties)
                _SeparatorAppearance = value
            End Set
        End Property

        Private _StatusStripAppearance As StatusStripAppearanceProperties
        <TypeConverter(GetType(ExpandableObjectConverter))> _
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
        <Category("Appearance")> _
        Public Property StatusStripAppearance() As StatusStripAppearanceProperties
            Get
                Return _StatusStripAppearance
            End Get
            Set(ByVal value As StatusStripAppearanceProperties)
                _StatusStripAppearance = value
            End Set
        End Property

        Private _ToolStripAppearance As ToolstripAppearanceProperties
        <TypeConverter(GetType(ExpandableObjectConverter))> _
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
        <Category("Appearance")> _
        Public Property ToolStripAppearance() As ToolstripAppearanceProperties
            Get
                Return _ToolStripAppearance
            End Get
            Set(ByVal value As ToolstripAppearanceProperties)
                _ToolStripAppearance = value
            End Set
        End Property

        Private _OverflowButtonAppearance As OverflowButtonAppearanceProperties
        <TypeConverter(GetType(ExpandableObjectConverter))> _
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
        <Category("Appearance")> _
        Public Property OverflowButtonAppearance() As OverflowButtonAppearanceProperties
            Get
                Return _OverflowButtonAppearance
            End Get
            Set(ByVal value As OverflowButtonAppearanceProperties)
                _OverflowButtonAppearance = value
            End Set
        End Property

        <Serializable()> _
        Public Class ButtonAppearanceProperties
            'Parameterless ctor required for serialization
            Public Sub New()
            End Sub

            Public Sub New(ByVal appearanceControl As AppearanceControl)
                _SelectedAppearance = New SelectedButtonAppearanceProperties(appearanceControl)
                _CheckedAppearance = New CheckedButtonAppearanceProperties(appearanceControl)
                _PressedAppearance = New PressedButtonAppearanceProperties(appearanceControl)
            End Sub

            Private _SelectedAppearance As SelectedButtonAppearanceProperties
            <TypeConverter(GetType(ExpandableObjectConverter))> _
            <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            Public Property SelectedAppearance() As SelectedButtonAppearanceProperties
                Get
                    Return _SelectedAppearance
                End Get
                Set(ByVal value As SelectedButtonAppearanceProperties)
                    _SelectedAppearance = value
                End Set
            End Property

            Private _CheckedAppearance As CheckedButtonAppearanceProperties
            <TypeConverter(GetType(ExpandableObjectConverter))> _
            <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            Public Property CheckedAppearance() As CheckedButtonAppearanceProperties
                Get
                    Return _CheckedAppearance
                End Get
                Set(ByVal value As CheckedButtonAppearanceProperties)
                    _CheckedAppearance = value
                End Set
            End Property

            Private _PressedAppearance As PressedButtonAppearanceProperties
            <TypeConverter(GetType(ExpandableObjectConverter))> _
            <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            Public Property PressedAppearance() As PressedButtonAppearanceProperties
                Get
                    Return _PressedAppearance
                End Get
                Set(ByVal value As PressedButtonAppearanceProperties)
                    _PressedAppearance = value
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function
        End Class

        <Serializable()> _
        Public Class ImageMarginAppearanceProperties

            'Parameterless ctor required for serialization
            Public Sub New()
            End Sub

            Public Sub New(ByVal appearanceControl As AppearanceControl)
                _Normal = New ImageMarginNormalAppearanceProperties(appearanceControl)
                _Revealed = New ImageMarginRevealedAppearanceProperties(appearanceControl)
            End Sub

            Private _Normal As ImageMarginNormalAppearanceProperties
            <TypeConverter(GetType(ExpandableObjectConverter))> _
            <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            Public Property Normal() As ImageMarginNormalAppearanceProperties
                Get
                    Return _Normal
                End Get
                Set(ByVal value As ImageMarginNormalAppearanceProperties)
                    _Normal = value
                End Set
            End Property

            Private _Revealed As ImageMarginRevealedAppearanceProperties
            <TypeConverter(GetType(ExpandableObjectConverter))> _
            <DesignerSerializationVisibility(DesignerSerializationVisibility.Content)> _
            Public Property Revealed() As ImageMarginRevealedAppearanceProperties
                Get
                    Return _Revealed
                End Get
                Set(ByVal value As ImageMarginRevealedAppearanceProperties)
                    _Revealed = value
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function
        End Class

#Region " Property Group Classes "

        <Serializable()> _
        Public Class SelectedButtonAppearanceProperties

            Public Sub New()
            End Sub

            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Private _GradientBegin As Color = Color.FromArgb(255, 255, 222)
            <DefaultValue(GetType(Color), "255, 255, 222")> _
            <XmlIgnore()> _
            Public Property GradientBegin() As Color
                Get
                    Return _GradientBegin
                End Get
                Set(ByVal value As Color)
                    _GradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientMiddle As Color = Color.FromArgb(255, 225, 172)
            <DefaultValue(GetType(Color), "255, 225, 172")> _
            <XmlIgnore()> _
            Public Property GradientMiddle() As Color
                Get
                    Return _GradientMiddle
                End Get
                Set(ByVal value As Color)
                    _GradientMiddle = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientEnd As Color = Color.FromArgb(255, 203, 136)
            <DefaultValue(GetType(Color), "255, 203, 136")> _
            <XmlIgnore()> _
            Public Property GradientEnd() As Color
                Get
                    Return _GradientEnd
                End Get
                Set(ByVal value As Color)
                    _GradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _Highlight As Color = Color.FromArgb(196, 208, 229)
            <DefaultValue(GetType(Color), "196, 208, 229")> _
            <XmlIgnore()> _
            Public Property Highlight() As Color
                Get
                    Return _Highlight
                End Get
                Set(ByVal value As Color)
                    _Highlight = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _BorderHighlight As Color = Color.FromArgb(0, 0, 128)
            <DefaultValue(GetType(Color), "0, 0, 128")> _
            <XmlIgnore()> _
            Public Property BorderHighlight() As Color
                Get
                    Return _BorderHighlight
                End Get
                Set(ByVal value As Color)
                    _BorderHighlight = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _Border As Color = Color.FromArgb(0, 0, 128)
            <DefaultValue(GetType(Color), "0, 0, 128")> _
            <XmlIgnore()> _
            Public Property Border() As Color
                Get
                    Return _Border
                End Get
                Set(ByVal value As Color)
                    _Border = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property
            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intGradientBegin() As Integer
                Get
                    Return Me.GradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientMiddle() As Integer
                Get
                    Return Me.GradientMiddle.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientMiddle = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientEnd() As Integer
                Get
                    Return Me.GradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientEnd = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intHighlight() As Integer
                Get
                    Return Me.Highlight.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Highlight = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intBorderHighlight() As Integer
                Get
                    Return Me.BorderHighlight.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.BorderHighlight = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intBorder() As Integer
                Get
                    Return Me.Border.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Border = Color.FromArgb(value)
                End Set
            End Property

        End Class

        <Serializable()> _
        Public Class PressedButtonAppearanceProperties
            Public Sub New()
            End Sub
            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Private _GradientBegin As Color = Color.FromArgb(254, 128, 62)
            <DefaultValue(GetType(Color), "254, 128, 62")> _
            <XmlIgnore()> _
            Public Property GradientBegin() As Color
                Get
                    Return _GradientBegin
                End Get
                Set(ByVal value As Color)
                    _GradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientMiddle As Color = Color.FromArgb(255, 177, 109)
            <DefaultValue(GetType(Color), "255, 177, 109")> _
            <XmlIgnore()> _
            Public Property GradientMiddle() As Color
                Get
                    Return _GradientMiddle
                End Get
                Set(ByVal value As Color)
                    _GradientMiddle = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientEnd As Color = Color.FromArgb(255, 223, 154)
            <DefaultValue(GetType(Color), "255, 223, 154")> _
            <XmlIgnore()> _
            Public Property GradientEnd() As Color
                Get
                    Return _GradientEnd
                End Get
                Set(ByVal value As Color)
                    _GradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _Highlight As Color = Color.FromArgb(152, 173, 210)
            <DefaultValue(GetType(Color), "152, 173, 210")> _
            <XmlIgnore()> _
            Public Property Highlight() As Color
                Get
                    Return _Highlight
                End Get
                Set(ByVal value As Color)
                    _Highlight = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _BorderHighlight As Color = Color.FromArgb(51, 94, 168)
            <DefaultValue(GetType(Color), "51, 94, 168")> _
            <XmlIgnore()> _
            Public Property BorderHighlight() As Color
                Get
                    Return _BorderHighlight
                End Get
                Set(ByVal value As Color)
                    _BorderHighlight = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _Border As Color = Color.FromArgb(0, 0, 128)
            <DefaultValue(GetType(Color), "0, 0, 128")> _
            <XmlIgnore()> _
            Public Property Border() As Color
                Get
                    Return _Border
                End Get
                Set(ByVal value As Color)
                    _Border = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property
            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intGradientBegin() As Integer
                Get
                    Return Me.GradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientMiddle() As Integer
                Get
                    Return Me.GradientMiddle.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientMiddle = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientEnd() As Integer
                Get
                    Return Me.GradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientEnd = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intHighlight() As Integer
                Get
                    Return Me.Highlight.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Highlight = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intBorderHighlight() As Integer
                Get
                    Return Me.BorderHighlight.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.BorderHighlight = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intBorder() As Integer
                Get
                    Return Me.Border.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Border = Color.FromArgb(value)
                End Set
            End Property


        End Class

        <Serializable()> _
        Public Class CheckedButtonAppearanceProperties
            Public Sub New()
            End Sub
            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub
            Private _GradientBegin As Color = Color.FromArgb(255, 223, 154)
            <DefaultValue(GetType(Color), "255, 223, 154")> _
            <XmlIgnore()> _
            Public Property GradientBegin() As Color
                Get
                    Return _GradientBegin
                End Get
                Set(ByVal value As Color)
                    _GradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientMiddle As Color = Color.FromArgb(255, 195, 116)
            <DefaultValue(GetType(Color), "255, 195, 116")> _
            <XmlIgnore()> _
            Public Property GradientMiddle() As Color
                Get
                    Return _GradientMiddle
                End Get
                Set(ByVal value As Color)
                    _GradientMiddle = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientEnd As Color = Color.FromArgb(255, 166, 76)
            <DefaultValue(GetType(Color), "255, 166, 76")> _
            <XmlIgnore()> _
            Public Property GradientEnd() As Color
                Get
                    Return _GradientEnd
                End Get
                Set(ByVal value As Color)
                    _GradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _Highlight As Color = Color.FromArgb(196, 208, 229)
            <DefaultValue(GetType(Color), "196, 208, 229")> _
            <XmlIgnore()> _
            Public Property Highlight() As Color
                Get
                    Return _Highlight
                End Get
                Set(ByVal value As Color)
                    _Highlight = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _BorderHighlight As Color = Color.FromArgb(51, 94, 168)
            <DefaultValue(GetType(Color), "51, 94, 168")> _
            <XmlIgnore()> _
            Public Property BorderHighlight() As Color
                Get
                    Return _BorderHighlight
                End Get
                Set(ByVal value As Color)
                    _BorderHighlight = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _Background As Color = Color.FromArgb(255, 192, 111)
            <DefaultValue(GetType(Color), "255, 192, 111")> _
            <XmlIgnore()> _
            Public Property Background() As Color
                Get
                    Return _Background
                End Get
                Set(ByVal value As Color)
                    _Background = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _SelectedBackground As Color = Color.FromArgb(254, 128, 62)
            <DefaultValue(GetType(Color), "254, 128, 62")> _
            <XmlIgnore()> _
            Public Property SelectedBackground() As Color
                Get
                    Return _SelectedBackground
                End Get
                Set(ByVal value As Color)
                    _SelectedBackground = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _PressedBackground As Color = Color.FromArgb(254, 128, 62)
            <DefaultValue(GetType(Color), "254, 128, 62")> _
            <XmlIgnore()> _
            Public Property PressedBackground() As Color
                Get
                    Return _PressedBackground
                End Get
                Set(ByVal value As Color)
                    _PressedBackground = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intGradientBegin() As Integer
                Get
                    Return Me.GradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientMiddle() As Integer
                Get
                    Return Me.GradientMiddle.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientMiddle = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientEnd() As Integer
                Get
                    Return Me.GradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientEnd = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intHighlight() As Integer
                Get
                    Return Me.Highlight.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Highlight = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intBorderHighlight() As Integer
                Get
                    Return Me.BorderHighlight.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.BorderHighlight = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intBackground() As Integer
                Get
                    Return Me.Background.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Background = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intSelectedBackground() As Integer
                Get
                    Return Me.SelectedBackground.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.SelectedBackground = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intPressedBackground() As Integer
                Get
                    Return Me.PressedBackground.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.PressedBackground = Color.FromArgb(value)
                End Set
            End Property

        End Class

        <Serializable()> _
        Public Class GripAppearanceProperties
            Public Sub New()
            End Sub
            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub
            Private _Dark As Color = Color.FromArgb(39, 65, 118)
            <DefaultValue(GetType(Color), "39, 65, 118")> _
            <XmlIgnore()> _
            Public Property Dark() As Color
                Get
                    Return _Dark
                End Get
                Set(ByVal value As Color)
                    _Dark = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _Light As Color = Color.FromArgb(255, 255, 255)
            <DefaultValue(GetType(Color), "255, 255, 255")> _
            <XmlIgnore()> _
            Public Property Light() As Color
                Get
                    Return _Light
                End Get
                Set(ByVal value As Color)
                    _Light = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intDark() As Integer
                Get
                    Return Me.Dark.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Dark = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intLight() As Integer
                Get
                    Return Me.Light.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Light = Color.FromArgb(value)
                End Set
            End Property

        End Class

        <Serializable()> _
        Public Class MenustripAppearanceProperties
            Public Sub New()
            End Sub
            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub
            Private _Border As Color = Color.FromArgb(0, 45, 150)
            <DefaultValue(GetType(Color), "0, 45, 150")> _
            <XmlIgnore()> _
            Public Property Border() As Color
                Get
                    Return _Border
                End Get
                Set(ByVal value As Color)
                    _Border = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientBegin As Color = Color.FromArgb(158, 190, 245)
            <DefaultValue(GetType(Color), "158, 190, 245")> _
            <XmlIgnore()> _
            Public Property GradientBegin() As Color
                Get
                    Return _GradientBegin
                End Get
                Set(ByVal value As Color)
                    _GradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientEnd As Color = Color.FromArgb(196, 218, 250)
            <DefaultValue(GetType(Color), "196, 218, 250")> _
            <XmlIgnore()> _
            Public Property GradientEnd() As Color
                Get
                    Return _GradientEnd
                End Get
                Set(ByVal value As Color)
                    _GradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intBorder() As Integer
                Get
                    Return Me.Border.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Border = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientBegin() As Integer
                Get
                    Return Me.GradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientEnd() As Integer
                Get
                    Return Me.GradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientEnd = Color.FromArgb(value)
                End Set
            End Property

        End Class

        <Serializable()> _
        Public Class MenuItemAppearanceProperties
            Public Sub New()
            End Sub
            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub
            Private _Selected As Color = Color.FromArgb(255, 238, 194)
            <DefaultValue(GetType(Color), "255, 238, 194")> _
            <XmlIgnore()> _
            Public Property Selected() As Color
                Get
                    Return _Selected
                End Get
                Set(ByVal value As Color)
                    _Selected = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _Border As Color = Color.FromArgb(0, 0, 128)
            <DefaultValue(GetType(Color), "0, 0, 128")> _
            <XmlIgnore()> _
            Public Property Border() As Color
                Get
                    Return _Border
                End Get
                Set(ByVal value As Color)
                    _Border = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _SelectedGradientBegin As Color = Color.FromArgb(255, 255, 222)
            <DefaultValue(GetType(Color), "255, 255, 222")> _
            <XmlIgnore()> _
            Public Property SelectedGradientBegin() As Color
                Get
                    Return _SelectedGradientBegin
                End Get
                Set(ByVal value As Color)
                    _SelectedGradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _SelectedGradientEnd As Color = Color.FromArgb(255, 203, 136)
            <DefaultValue(GetType(Color), "255, 203, 136")> _
            <XmlIgnore()> _
            Public Property SelectedGradientEnd() As Color
                Get
                    Return _SelectedGradientEnd
                End Get
                Set(ByVal value As Color)
                    _SelectedGradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _PressedGradientBegin As Color = Color.FromArgb(227, 239, 255)
            <DefaultValue(GetType(Color), "227, 239, 255")> _
            <XmlIgnore()> _
            Public Property PressedGradientBegin() As Color
                Get
                    Return _PressedGradientBegin
                End Get
                Set(ByVal value As Color)
                    _PressedGradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _PressedGradientMiddle As Color = Color.FromArgb(161, 197, 249)
            <DefaultValue(GetType(Color), "161, 197, 249")> _
            <XmlIgnore()> _
            Public Property PressedGradientMiddle() As Color
                Get
                    Return _PressedGradientMiddle
                End Get
                Set(ByVal value As Color)
                    _PressedGradientMiddle = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _PressedGradientEnd As Color = Color.FromArgb(123, 164, 224)
            <DefaultValue(GetType(Color), "123, 164, 224")> _
            <XmlIgnore()> _
            Public Property PressedGradientEnd() As Color
                Get
                    Return _PressedGradientEnd
                End Get
                Set(ByVal value As Color)
                    _PressedGradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intSelected() As Integer
                Get
                    Return Me.Selected.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Selected = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intBorder() As Integer
                Get
                    Return Me.Border.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Border = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intSelectedGradientBegin() As Integer
                Get
                    Return Me.SelectedGradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.SelectedGradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intSelectedGradientEnd() As Integer
                Get
                    Return Me.SelectedGradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.SelectedGradientEnd = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intPressedGradientBegin() As Integer
                Get
                    Return Me.PressedGradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.PressedGradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intPressedGradientMiddle() As Integer
                Get
                    Return Me.PressedGradientMiddle.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.PressedGradientMiddle = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intPressedGradientEnd() As Integer
                Get
                    Return Me.PressedGradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.PressedGradientEnd = Color.FromArgb(value)
                End Set
            End Property

        End Class

        <Serializable()> _
        Public Class RaftingContainerAppearanceProperties
            Public Sub New()
            End Sub
            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub
            Private _GradientBegin As Color = Color.FromArgb(158, 190, 245)
            <DefaultValue(GetType(Color), "158, 190, 245")> _
            <XmlIgnore()> _
            Public Property GradientBegin() As Color
                Get
                    Return _GradientBegin
                End Get
                Set(ByVal value As Color)
                    _GradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientEnd As Color = Color.FromArgb(196, 218, 250)
            <DefaultValue(GetType(Color), "196, 218, 250")> _
            <XmlIgnore()> _
            Public Property GradientEnd() As Color
                Get
                    Return _GradientEnd
                End Get
                Set(ByVal value As Color)
                    _GradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intGradientBegin() As Integer
                Get
                    Return Me.GradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientEnd() As Integer
                Get
                    Return Me.GradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientEnd = Color.FromArgb(value)
                End Set
            End Property

        End Class

        <Serializable()> _
        Public Class StatusStripAppearanceProperties
            Public Sub New()
            End Sub
            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub
            Private _GradientBegin As Color = Color.FromArgb(158, 190, 245)
            <DefaultValue(GetType(Color), "158, 190, 245")> _
            <XmlIgnore()> _
            Public Property GradientBegin() As Color
                Get
                    Return _GradientBegin
                End Get
                Set(ByVal value As Color)
                    _GradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientEnd As Color = Color.FromArgb(196, 218, 250)
            <DefaultValue(GetType(Color), "196, 218, 250")> _
            <XmlIgnore()> _
            Public Property GradientEnd() As Color
                Get
                    Return _GradientEnd
                End Get
                Set(ByVal value As Color)
                    _GradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intGradientBegin() As Integer
                Get
                    Return Me.GradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientEnd() As Integer
                Get
                    Return Me.GradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientEnd = Color.FromArgb(value)
                End Set
            End Property

        End Class

        <Serializable()> _
        Public Class SeparatorAppearanceProperties
            Public Sub New()
            End Sub
            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub
            Private _Dark As Color = Color.FromArgb(106, 140, 203)
            <DefaultValue(GetType(Color), "106, 140, 203")> _
            <XmlIgnore()> _
            Public Property Dark() As Color
                Get
                    Return _Dark
                End Get
                Set(ByVal value As Color)
                    _Dark = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _Light As Color = Color.FromArgb(241, 249, 255)
            <DefaultValue(GetType(Color), "241, 249, 255")> _
            <XmlIgnore()> _
            Public Property Light() As Color
                Get
                    Return _Light
                End Get
                Set(ByVal value As Color)
                    _Light = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intDark() As Integer
                Get
                    Return Me.Dark.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Dark = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intLight() As Integer
                Get
                    Return Me.Light.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Light = Color.FromArgb(value)
                End Set
            End Property

        End Class

        <Serializable()> _
        Public Class ToolstripAppearanceProperties

            Public Sub New()
            End Sub

            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Private _GradientBegin As Color = Color.FromArgb(227, 239, 255)
            <DefaultValue(GetType(Color), "227, 239, 255")> _
            <XmlIgnore()> _
            Public Property GradientBegin() As Color
                Get
                    Return _GradientBegin
                End Get
                Set(ByVal value As Color)
                    _GradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientMiddle As Color = Color.FromArgb(203, 225, 252)
            <DefaultValue(GetType(Color), "203, 225, 252")> _
            <XmlIgnore()> _
            Public Property GradientMiddle() As Color
                Get
                    Return _GradientMiddle
                End Get
                Set(ByVal value As Color)
                    _GradientMiddle = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientEnd As Color = Color.FromArgb(123, 164, 224)
            <DefaultValue(GetType(Color), "123, 164, 224")> _
            <XmlIgnore()> _
            Public Property GradientEnd() As Color
                Get
                    Return _GradientEnd
                End Get
                Set(ByVal value As Color)
                    _GradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _Border As Color = Color.FromArgb(59, 97, 156)
            <DefaultValue(GetType(Color), "59, 97, 156")> _
            <XmlIgnore()> _
            Public Property Border() As Color
                Get
                    Return _Border
                End Get
                Set(ByVal value As Color)
                    _Border = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _DropDownBackground As Color = Color.FromArgb(246, 246, 246)
            <DefaultValue(GetType(Color), "246, 246, 246")> _
            <XmlIgnore()> _
            Public Property DropDownBackground() As Color
                Get
                    Return _DropDownBackground
                End Get
                Set(ByVal value As Color)
                    _DropDownBackground = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _ContentPanelGradientBegin As Color = Color.FromArgb(158, 190, 245)
            <DefaultValue(GetType(Color), "158, 190, 245")> _
            <XmlIgnore()> _
            Public Property ContentPanelGradientBegin() As Color
                Get
                    Return _ContentPanelGradientBegin
                End Get
                Set(ByVal value As Color)
                    _ContentPanelGradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _ContentPanelGradientEnd As Color = Color.FromArgb(196, 218, 250)
            <DefaultValue(GetType(Color), "196, 218, 250")> _
            <XmlIgnore()> _
            Public Property ContentPanelGradientEnd() As Color
                Get
                    Return _ContentPanelGradientEnd
                End Get
                Set(ByVal value As Color)
                    _ContentPanelGradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _PanelGradientBegin As Color = Color.FromArgb(158, 190, 245)
            <DefaultValue(GetType(Color), "158, 190, 245")> _
            <XmlIgnore()> _
            Public Property PanelGradientBegin() As Color
                Get
                    Return _PanelGradientBegin
                End Get
                Set(ByVal value As Color)
                    _PanelGradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _PanelGradientEnd As Color = Color.FromArgb(196, 218, 250)
            <DefaultValue(GetType(Color), "196, 218, 250")> _
            <XmlIgnore()> _
            Public Property PanelGradientEnd() As Color
                Get
                    Return _PanelGradientEnd
                End Get
                Set(ByVal value As Color)
                    _PanelGradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intGradientBegin() As Integer
                Get
                    Return Me.GradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientMiddle() As Integer
                Get
                    Return Me.GradientMiddle.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientMiddle = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientEnd() As Integer
                Get
                    Return Me.GradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientEnd = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intBorder() As Integer
                Get
                    Return Me.Border.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.Border = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intDropDownBackground() As Integer
                Get
                    Return Me.DropDownBackground.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.DropDownBackground = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intContentPanelGradientBegin() As Integer
                Get
                    Return Me.ContentPanelGradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.ContentPanelGradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intContentPanelGradientEnd() As Integer
                Get
                    Return Me.ContentPanelGradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.ContentPanelGradientEnd = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intPanelGradientBegin() As Integer
                Get
                    Return Me.PanelGradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.PanelGradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intPanelGradientEnd() As Integer
                Get
                    Return Me.PanelGradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.PanelGradientEnd = Color.FromArgb(value)
                End Set
            End Property

        End Class

        <Serializable()> _
        Public Class OverflowButtonAppearanceProperties
            Public Sub New()
            End Sub
            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub
            Private _GradientBegin As Color = Color.FromArgb(127, 177, 250)
            <DefaultValue(GetType(Color), "127, 177, 250")> _
            <XmlIgnore()> _
            Public Property GradientBegin() As Color
                Get
                    Return _GradientBegin
                End Get
                Set(ByVal value As Color)
                    _GradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientMiddle As Color = Color.FromArgb(82, 127, 208)
            <DefaultValue(GetType(Color), "82, 127, 208")> _
            <XmlIgnore()> _
            Public Property GradientMiddle() As Color
                Get
                    Return _GradientMiddle
                End Get
                Set(ByVal value As Color)
                    _GradientMiddle = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientEnd As Color = Color.FromArgb(0, 53, 145)
            <DefaultValue(GetType(Color), "0, 53, 145")> _
            <XmlIgnore()> _
            Public Property GradientEnd() As Color
                Get
                    Return _GradientEnd
                End Get
                Set(ByVal value As Color)
                    _GradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intGradientBegin() As Integer
                Get
                    Return Me.GradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientMiddle() As Integer
                Get
                    Return Me.GradientMiddle.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientMiddle = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientEnd() As Integer
                Get
                    Return Me.GradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientEnd = Color.FromArgb(value)
                End Set
            End Property

        End Class

        <Serializable()> _
        Public Class ImageMarginNormalAppearanceProperties
            Public Sub New()
            End Sub
            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub
            Private _GradientBegin As Color = Color.FromArgb(227, 239, 255)
            <DefaultValue(GetType(Color), "227, 239, 255")> _
            <XmlIgnore()> _
            Public Property GradientBegin() As Color
                Get
                    Return _GradientBegin
                End Get
                Set(ByVal value As Color)
                    _GradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientMiddle As Color = Color.FromArgb(203, 225, 252)
            <DefaultValue(GetType(Color), "203, 225, 252")> _
            <XmlIgnore()> _
            Public Property GradientMiddle() As Color
                Get
                    Return _GradientMiddle
                End Get
                Set(ByVal value As Color)
                    _GradientMiddle = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientEnd As Color = Color.FromArgb(123, 164, 224)
            <DefaultValue(GetType(Color), "123, 164, 224")> _
            <XmlIgnore()> _
            Public Property GradientEnd() As Color
                Get
                    Return _GradientEnd
                End Get
                Set(ByVal value As Color)
                    _GradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intGradientBegin() As Integer
                Get
                    Return Me.GradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientMiddle() As Integer
                Get
                    Return Me.GradientMiddle.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientMiddle = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientEnd() As Integer
                Get
                    Return Me.GradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientEnd = Color.FromArgb(value)
                End Set
            End Property

        End Class

        <Serializable()> _
        Public Class ImageMarginRevealedAppearanceProperties
            Public Sub New()
            End Sub
            Private ap As AppearanceControl
            Public Sub New(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub

            Public Sub SetAppearanceControl(ByVal appearanceControl As AppearanceControl)
                ap = appearanceControl
            End Sub
            Private _GradientBegin As Color = Color.FromArgb(203, 221, 246)
            <DefaultValue(GetType(Color), "203, 221, 246")> _
            <XmlIgnore()> _
            Public Property GradientBegin() As Color
                Get
                    Return _GradientBegin
                End Get
                Set(ByVal value As Color)
                    _GradientBegin = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientMiddle As Color = Color.FromArgb(161, 197, 249)
            <DefaultValue(GetType(Color), "161, 197, 249")> _
            <XmlIgnore()> _
            Public Property GradientMiddle() As Color
                Get
                    Return _GradientMiddle
                End Get
                Set(ByVal value As Color)
                    _GradientMiddle = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Private _GradientEnd As Color = Color.FromArgb(114, 155, 215)
            <DefaultValue(GetType(Color), "114, 155, 215")> _
            <XmlIgnore()> _
            Public Property GradientEnd() As Color
                Get
                    Return _GradientEnd
                End Get
                Set(ByVal value As Color)
                    _GradientEnd = value
                    If ap IsNot Nothing Then ap.OnAppearanceChanged(EventArgs.Empty)
                End Set
            End Property

            Public Overrides Function ToString() As String
                Return String.Empty
            End Function

            <Browsable(False)> _
            Public Property intGradientBegin() As Integer
                Get
                    Return Me.GradientBegin.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientBegin = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientMiddle() As Integer
                Get
                    Return Me.GradientMiddle.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientMiddle = Color.FromArgb(value)
                End Set
            End Property

            <Browsable(False)> _
            Public Property intGradientEnd() As Integer
                Get
                    Return Me.GradientEnd.ToArgb()
                End Get
                Set(ByVal value As Integer)
                    Me.GradientEnd = Color.FromArgb(value)
                End Set
            End Property

        End Class

#End Region

        Public Overrides Function ToString() As String
            Return String.Empty
        End Function

    End Class

#Region " Color Tables "
    '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Public Class CustomColorTable
        Inherits ProfessionalColorTable

        Private ac As AppearanceControl = Nothing

        Public Sub New(ByVal appearanceControl As AppearanceControl)
            ac = appearanceControl
        End Sub

        Overrides ReadOnly Property ButtonSelectedHighlight() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.SelectedAppearance.Highlight
            End Get
        End Property

        Overrides ReadOnly Property ButtonSelectedHighlightBorder() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.SelectedAppearance.BorderHighlight
            End Get
        End Property

        Overrides ReadOnly Property ButtonPressedHighlight() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.PressedAppearance.Highlight
            End Get
        End Property

        Overrides ReadOnly Property ButtonPressedHighlightBorder() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.PressedAppearance.BorderHighlight
            End Get
        End Property

        Overrides ReadOnly Property ButtonCheckedHighlight() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.CheckedAppearance.Highlight
            End Get
        End Property

        Overrides ReadOnly Property ButtonCheckedHighlightBorder() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.CheckedAppearance.BorderHighlight
            End Get
        End Property

        Overrides ReadOnly Property ButtonPressedBorder() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.PressedAppearance.Border
            End Get
        End Property

        Overrides ReadOnly Property ButtonSelectedBorder() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.SelectedAppearance.Border
            End Get
        End Property

        Overrides ReadOnly Property ButtonCheckedGradientBegin() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.CheckedAppearance.GradientBegin
            End Get
        End Property

        Overrides ReadOnly Property ButtonCheckedGradientMiddle() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.CheckedAppearance.GradientMiddle
            End Get
        End Property

        Overrides ReadOnly Property ButtonCheckedGradientEnd() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.CheckedAppearance.GradientEnd
            End Get
        End Property

        Overrides ReadOnly Property ButtonSelectedGradientBegin() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.SelectedAppearance.GradientBegin
            End Get
        End Property

        Overrides ReadOnly Property ButtonSelectedGradientMiddle() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.SelectedAppearance.GradientMiddle
            End Get
        End Property

        Overrides ReadOnly Property ButtonSelectedGradientEnd() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.SelectedAppearance.GradientEnd
            End Get
        End Property

        Overrides ReadOnly Property ButtonPressedGradientBegin() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.PressedAppearance.GradientBegin
            End Get
        End Property

        Overrides ReadOnly Property ButtonPressedGradientMiddle() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.PressedAppearance.GradientMiddle
            End Get
        End Property

        Overrides ReadOnly Property ButtonPressedGradientEnd() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.PressedAppearance.GradientEnd
            End Get
        End Property

        Overrides ReadOnly Property CheckBackground() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.CheckedAppearance.Background
            End Get
        End Property

        Overrides ReadOnly Property CheckSelectedBackground() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.CheckedAppearance.SelectedBackground
            End Get
        End Property

        Overrides ReadOnly Property CheckPressedBackground() As Color
            Get
                Return ac.CustomAppearance.ButtonAppearance.CheckedAppearance.PressedBackground
            End Get
        End Property

        Overrides ReadOnly Property GripDark() As Color
            Get
                Return ac.CustomAppearance.GripAppearance.Dark
            End Get
        End Property

        Overrides ReadOnly Property GripLight() As Color
            Get
                Return ac.CustomAppearance.GripAppearance.Light
            End Get
        End Property

        Overrides ReadOnly Property ImageMarginGradientBegin() As Color
            Get
                Return ac.CustomAppearance.ImageMarginAppearance.Normal.GradientBegin
            End Get
        End Property

        Overrides ReadOnly Property ImageMarginGradientMiddle() As Color
            Get
                Return ac.CustomAppearance.ImageMarginAppearance.Normal.GradientMiddle
            End Get
        End Property

        Overrides ReadOnly Property ImageMarginGradientEnd() As Color
            Get
                Return ac.CustomAppearance.ImageMarginAppearance.Normal.GradientEnd
            End Get
        End Property

        Overrides ReadOnly Property ImageMarginRevealedGradientBegin() As Color
            Get
                Return ac.CustomAppearance.ImageMarginAppearance.Revealed.GradientBegin
            End Get
        End Property

        Overrides ReadOnly Property ImageMarginRevealedGradientMiddle() As Color
            Get
                Return ac.CustomAppearance.ImageMarginAppearance.Revealed.GradientMiddle
            End Get
        End Property

        Overrides ReadOnly Property ImageMarginRevealedGradientEnd() As Color
            Get
                Return ac.CustomAppearance.ImageMarginAppearance.Revealed.GradientEnd
            End Get
        End Property

        Overrides ReadOnly Property MenuStripGradientBegin() As Color
            Get
                Return ac.CustomAppearance.MenuStripAppearance.GradientBegin
            End Get
        End Property

        Overrides ReadOnly Property MenuStripGradientEnd() As Color
            Get
                Return ac.CustomAppearance.MenuStripAppearance.GradientEnd
            End Get
        End Property

        Overrides ReadOnly Property MenuItemSelected() As Color
            Get
                Return ac.CustomAppearance.MenuItemAppearance.Selected
            End Get
        End Property

        Overrides ReadOnly Property MenuItemBorder() As Color
            Get
                Return ac.CustomAppearance.MenuItemAppearance.Border
            End Get
        End Property

        Overrides ReadOnly Property MenuBorder() As Color
            Get
                Return ac.CustomAppearance.MenuStripAppearance.Border
            End Get
        End Property

        Overrides ReadOnly Property MenuItemSelectedGradientBegin() As Color
            Get
                Return ac.CustomAppearance.MenuItemAppearance.SelectedGradientBegin
            End Get
        End Property

        Overrides ReadOnly Property MenuItemSelectedGradientEnd() As Color
            Get
                Return ac.CustomAppearance.MenuItemAppearance.SelectedGradientEnd
            End Get
        End Property

        Overrides ReadOnly Property MenuItemPressedGradientBegin() As Color
            Get
                Return ac.CustomAppearance.MenuItemAppearance.PressedGradientBegin
            End Get
        End Property

        Overrides ReadOnly Property MenuItemPressedGradientMiddle() As Color
            Get
                Return ac.CustomAppearance.MenuItemAppearance.PressedGradientMiddle
            End Get
        End Property

        Overrides ReadOnly Property MenuItemPressedGradientEnd() As Color
            Get
                Return ac.CustomAppearance.MenuItemAppearance.PressedGradientEnd
            End Get
        End Property

        Overrides ReadOnly Property RaftingContainerGradientBegin() As Color
            Get
                Return ac.CustomAppearance.RaftingContainerAppearance.GradientBegin
            End Get
        End Property

        Overrides ReadOnly Property RaftingContainerGradientEnd() As Color
            Get
                Return ac.CustomAppearance.RaftingContainerAppearance.GradientEnd
            End Get
        End Property

        Overrides ReadOnly Property SeparatorDark() As Color
            Get
                Return ac.CustomAppearance.SeparatorAppearance.Dark
            End Get
        End Property

        Overrides ReadOnly Property SeparatorLight() As Color
            Get
                Return ac.CustomAppearance.SeparatorAppearance.Light
            End Get
        End Property

        Overrides ReadOnly Property StatusStripGradientBegin() As Color
            Get
                Return ac.CustomAppearance.StatusStripAppearance.GradientBegin
            End Get
        End Property

        Overrides ReadOnly Property StatusStripGradientEnd() As Color
            Get
                Return ac.CustomAppearance.StatusStripAppearance.GradientEnd
            End Get
        End Property

        Overrides ReadOnly Property ToolStripBorder() As Color
            Get
                Return ac.CustomAppearance.ToolStripAppearance.Border
            End Get
        End Property

        Overrides ReadOnly Property ToolStripDropDownBackground() As Color
            Get
                Return ac.CustomAppearance.ToolStripAppearance.DropDownBackground
            End Get
        End Property

        Overrides ReadOnly Property ToolStripGradientBegin() As Color
            Get
                Return ac.CustomAppearance.ToolStripAppearance.GradientBegin
            End Get
        End Property

        Overrides ReadOnly Property ToolStripGradientMiddle() As Color
            Get
                Return ac.CustomAppearance.ToolStripAppearance.GradientMiddle
            End Get
        End Property

        Overrides ReadOnly Property ToolStripGradientEnd() As Color
            Get
                Return ac.CustomAppearance.ToolStripAppearance.GradientEnd
            End Get
        End Property

        Overrides ReadOnly Property ToolStripContentPanelGradientBegin() As Color
            Get
                Return ac.CustomAppearance.ToolStripAppearance.ContentPanelGradientBegin
            End Get
        End Property

        Overrides ReadOnly Property ToolStripContentPanelGradientEnd() As Color
            Get
                Return ac.CustomAppearance.ToolStripAppearance.ContentPanelGradientEnd
            End Get
        End Property

        Overrides ReadOnly Property ToolStripPanelGradientBegin() As Color
            Get
                Return ac.CustomAppearance.ToolStripAppearance.PanelGradientBegin
            End Get
        End Property

        Overrides ReadOnly Property ToolStripPanelGradientEnd() As Color
            Get
                Return ac.CustomAppearance.ToolStripAppearance.PanelGradientEnd
            End Get
        End Property

        Overrides ReadOnly Property OverflowButtonGradientBegin() As Color
            Get
                Return ac.CustomAppearance.OverflowButtonAppearance.GradientBegin
            End Get
        End Property

        Overrides ReadOnly Property OverflowButtonGradientMiddle() As Color
            Get
                Return ac.CustomAppearance.OverflowButtonAppearance.GradientMiddle
            End Get
        End Property

        Overrides ReadOnly Property OverflowButtonGradientEnd() As Color
            Get
                Return ac.CustomAppearance.OverflowButtonAppearance.GradientEnd
            End Get
        End Property

    End Class
    '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Public Class Office2007ColorTable
        Inherits ProfessionalColorTable

        Overrides ReadOnly Property ButtonSelectedHighlight() As Color
            Get
                Return Color.White
            End Get
        End Property

        Overrides ReadOnly Property ButtonSelectedHighlightBorder() As Color
            Get
                Return Color.White
            End Get
        End Property

        Overrides ReadOnly Property ButtonPressedHighlight() As Color
            Get
                Return Color.White
            End Get
        End Property

        Overrides ReadOnly Property ButtonPressedHighlightBorder() As Color
            Get
                Return Color.White
            End Get
        End Property

        Overrides ReadOnly Property ButtonCheckedHighlight() As Color
            Get
                Return Color.White
            End Get
        End Property

        Overrides ReadOnly Property ButtonCheckedHighlightBorder() As Color
            Get
                Return Color.White
            End Get
        End Property

        Overrides ReadOnly Property ButtonPressedBorder() As Color
            Get
                Return Color.FromArgb(251, 140, 60)
            End Get
        End Property

        Overrides ReadOnly Property ButtonSelectedBorder() As Color
            Get
                Return Color.FromArgb(255, 189, 105)
            End Get
        End Property

        Overrides ReadOnly Property ButtonCheckedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 207, 146)
            End Get
        End Property

        Overrides ReadOnly Property ButtonCheckedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 189, 105)
            End Get
        End Property

        Overrides ReadOnly Property ButtonCheckedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 175, 73)
            End Get
        End Property

        Overrides ReadOnly Property ButtonSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 245, 204)
            End Get
        End Property

        Overrides ReadOnly Property ButtonSelectedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 230, 162)
            End Get
        End Property

        Overrides ReadOnly Property ButtonSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 218, 117)
            End Get
        End Property

        Overrides ReadOnly Property ButtonPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(252, 151, 61)
            End Get
        End Property

        Overrides ReadOnly Property ButtonPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 171, 63)
            End Get
        End Property

        Overrides ReadOnly Property ButtonPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 184, 94)
            End Get
        End Property

        Overrides ReadOnly Property CheckBackground() As Color
            Get
                Return Color.FromArgb(255, 171, 63) 'UNSURE
            End Get
        End Property

        Overrides ReadOnly Property CheckSelectedBackground() As Color
            Get
                Return Me.ButtonPressedGradientBegin 'UNSURE
            End Get
        End Property

        Overrides ReadOnly Property CheckPressedBackground() As Color
            Get
                Return Me.CheckSelectedBackground
            End Get
        End Property

        Overrides ReadOnly Property GripDark() As Color
            Get
                Return Color.FromArgb(111, 157, 217)
            End Get
        End Property

        Overrides ReadOnly Property GripLight() As Color
            Get
                Return Color.White
            End Get
        End Property

        Overrides ReadOnly Property ImageMarginGradientBegin() As Color
            Get
                Return Color.FromArgb(233, 238, 238)
            End Get
        End Property

        Overrides ReadOnly Property ImageMarginGradientMiddle() As Color
            Get
                Return Me.ImageMarginGradientBegin
            End Get
        End Property

        Overrides ReadOnly Property ImageMarginGradientEnd() As Color
            Get
                Return Me.ImageMarginGradientBegin
            End Get
        End Property

        Overrides ReadOnly Property ImageMarginRevealedGradientBegin() As Color
            Get
                Return Me.ImageMarginGradientBegin 'UNSURE
            End Get
        End Property

        Overrides ReadOnly Property ImageMarginRevealedGradientMiddle() As Color
            Get
                Return Me.ImageMarginRevealedGradientBegin 'UNSURE
            End Get
        End Property

        Overrides ReadOnly Property ImageMarginRevealedGradientEnd() As Color
            Get
                Return ImageMarginRevealedGradientBegin 'UNSURE
            End Get
        End Property

        Overrides ReadOnly Property MenuStripGradientBegin() As Color
            Get
                Return Color.FromArgb(191, 219, 255)
            End Get
        End Property

        Overrides ReadOnly Property MenuStripGradientEnd() As Color
            Get
                Return Me.MenuStripGradientBegin
            End Get
        End Property

        Overrides ReadOnly Property MenuItemSelected() As Color
            Get
                Return Color.FromArgb(255, 231, 162)
            End Get
        End Property

        Overrides ReadOnly Property MenuItemBorder() As Color
            Get
                Return Color.FromArgb(255, 189, 105)
            End Get
        End Property

        Overrides ReadOnly Property MenuBorder() As Color
            Get
                Return Color.FromArgb(101, 147, 207)
            End Get
        End Property

        Overrides ReadOnly Property MenuItemSelectedGradientBegin() As Color
            Get
                Return Me.ButtonSelectedGradientBegin
            End Get
        End Property

        Overrides ReadOnly Property MenuItemSelectedGradientEnd() As Color
            Get
                Return Me.ButtonSelectedGradientEnd
            End Get
        End Property

        Overrides ReadOnly Property MenuItemPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(226, 239, 255)
            End Get
        End Property

        Overrides ReadOnly Property MenuItemPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(190, 215, 247)
            End Get
        End Property

        Overrides ReadOnly Property MenuItemPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(153, 191, 240)
            End Get
        End Property

        Overrides ReadOnly Property RaftingContainerGradientBegin() As Color
            Get
                Return Color.White 'UNSURE
            End Get
        End Property

        Overrides ReadOnly Property RaftingContainerGradientEnd() As Color
            Get
                Return Color.White 'UNSURE
            End Get
        End Property

        Overrides ReadOnly Property SeparatorDark() As Color
            Get
                Return Color.FromArgb(154, 198, 255)
            End Get
        End Property

        Overrides ReadOnly Property SeparatorLight() As Color
            Get
                Return Color.White
            End Get
        End Property

        Overrides ReadOnly Property ToolStripBorder() As Color
            Get
                Return Color.FromArgb(111, 157, 217)
            End Get
        End Property

        Overrides ReadOnly Property ToolStripDropDownBackground() As Color
            Get
                Return Color.FromArgb(246, 246, 246)
            End Get
        End Property

        Overrides ReadOnly Property ToolStripGradientBegin() As Color
            Get
                Return Color.FromArgb(227, 239, 255)
            End Get
        End Property

        Overrides ReadOnly Property ToolStripGradientMiddle() As Color
            Get
                Return Color.FromArgb(218, 234, 255)
            End Get
        End Property

        Overrides ReadOnly Property ToolStripGradientEnd() As Color
            Get
                Return Color.FromArgb(177, 211, 255)
            End Get
        End Property

        Overrides ReadOnly Property ToolStripContentPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(215, 232, 255) 'UNSURE
            End Get
        End Property

        Overrides ReadOnly Property ToolStripContentPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(111, 157, 217) 'UNSURE
            End Get
        End Property

        Overrides ReadOnly Property ToolStripPanelGradientBegin() As Color
            Get
                Return Me.MenuStripGradientBegin
            End Get
        End Property

        Overrides ReadOnly Property ToolStripPanelGradientEnd() As Color
            Get
                Return Me.MenuStripGradientEnd
            End Get
        End Property

        Overrides ReadOnly Property OverflowButtonGradientBegin() As Color
            Get
                Return Color.FromArgb(215, 232, 255)
            End Get
        End Property

        Overrides ReadOnly Property OverflowButtonGradientMiddle() As Color
            Get
                Return Color.FromArgb(167, 204, 251)
            End Get
        End Property

        Overrides ReadOnly Property OverflowButtonGradientEnd() As Color
            Get
                Return Color.FromArgb(111, 157, 217)
            End Get
        End Property

    End Class
    '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Public Class Office2007BlackColorTable
        Inherits ProfessionalColorTable

        Public Overrides ReadOnly Property ButtonSelectedHighlight() As Color
            Get
                Return Color.FromArgb(223, 227, 213) '*
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedHighlightBorder() As Color
            Get
                Return Color.FromArgb(255, 189, 105) '
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedHighlight() As Color
            Get
                Return Color.FromArgb(200, 206, 182) '*
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedHighlightBorder() As Color
            Get
                Return Color.FromArgb(147, 160, 112) '*
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedHighlight() As Color
            Get
                Return Color.FromArgb(223, 227, 213) '*
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedHighlightBorder() As Color
            Get
                Return Color.FromArgb(147, 160, 112) '*
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedBorder() As Color
            Get
                Return Color.FromArgb(255, 189, 105) '
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedBorder() As Color
            Get
                Return Color.FromArgb(255, 189, 105) '
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 223, 154) '*
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 195, 116) '*
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 166, 76) '*
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 245, 204) '
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 231, 162) '
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 219, 117) '
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(248, 181, 106) '
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(251, 140, 60) '
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 208, 134) '
            End Get
        End Property

        Public Overrides ReadOnly Property CheckBackground() As Color
            Get
                Return Color.FromArgb(255, 227, 149) '
            End Get
        End Property

        Public Overrides ReadOnly Property CheckSelectedBackground() As Color
            Get
                Return Color.FromArgb(254, 128, 62) '
            End Get
        End Property

        Public Overrides ReadOnly Property CheckPressedBackground() As Color
            Get
                Return Color.FromArgb(254, 128, 62) '*
            End Get
        End Property

        Public Overrides ReadOnly Property GripDark() As Color
            Get
                Return Color.FromArgb(145, 153, 164) '
            End Get
        End Property

        Public Overrides ReadOnly Property GripLight() As Color
            Get
                Return Color.FromArgb(221, 224, 227) '
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientBegin() As Color
            Get
                'Return Color.FromArgb(239, 239, 239) '
                Return Color.FromArgb(255, 223, 154)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientMiddle() As Color
            Get
                'Return Color.FromArgb(239, 239, 239) '*
                Return Color.FromArgb(255, 223, 154)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientEnd() As Color
            Get
                'Return Color.FromArgb(239, 239, 239) '*
                Return Color.FromArgb(255, 223, 154)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientBegin() As Color
            Get
                Return Color.FromArgb(230, 230, 209) '*
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientMiddle() As Color
            Get
                Return Color.FromArgb(186, 201, 143) '*
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientEnd() As Color
            Get
                Return Color.FromArgb(160, 177, 116) '*
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientBegin() As Color
            Get
                Return Color.FromArgb(83, 83, 83) '
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientEnd() As Color
            Get
                Return Color.FromArgb(83, 83, 83) '
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelected() As Color
            Get
                Return Color.FromArgb(255, 238, 194) '
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemBorder() As Color
            Get
                Return Color.FromArgb(255, 189, 105) '
            End Get
        End Property

        Public Overrides ReadOnly Property MenuBorder() As Color
            Get
                Return Color.FromArgb(145, 153, 164) '
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 245, 204) '
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 223, 132) '
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(145, 153, 164) '
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(126, 135, 146) '
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(108, 117, 128) '
            End Get
        End Property

        Public Overrides ReadOnly Property RaftingContainerGradientBegin() As Color
            Get
                Return Color.FromArgb(83, 83, 83) '
            End Get
        End Property

        Public Overrides ReadOnly Property RaftingContainerGradientEnd() As Color
            Get
                Return Color.FromArgb(83, 83, 83) '
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorDark() As Color
            Get
                Return Color.FromArgb(145, 153, 164) '
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorLight() As Color
            Get
                Return Color.FromArgb(221, 224, 227) '
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientBegin() As Color
            Get
                Return Color.FromArgb(76, 83, 92) '
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientEnd() As Color
            Get
                Return Color.FromArgb(35, 38, 42) '
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripBorder() As Color
            Get
                Return Color.FromArgb(76, 83, 92) '
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripDropDownBackground() As Color
            Get
                Return Color.FromArgb(250, 250, 250) '
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientBegin() As Color
            Get
                Return Color.FromArgb(205, 208, 213) '
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientMiddle() As Color
            Get
                Return Color.FromArgb(188, 193, 200) '
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientEnd() As Color
            Get
                Return Color.FromArgb(148, 156, 166) '
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(82, 82, 82) '
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(10, 10, 10) '
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(83, 83, 83) '
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(83, 83, 83) '
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientBegin() As Color
            Get
                Return Color.FromArgb(178, 183, 191) '
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientMiddle() As Color
            Get
                Return Color.FromArgb(145, 153, 164) '
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientEnd() As Color
            Get
                Return Color.FromArgb(81, 88, 98) '
            End Get
        End Property

    End Class
    '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Public Class Office2003BlueColorTable
        Inherits ProfessionalColorTable

        Public Overrides ReadOnly Property ButtonSelectedHighlight() As Color
            Get
                Return Color.FromArgb(195, 211, 237)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedHighlightBorder() As Color
            Get
                Return Color.FromArgb(0, 0, 128)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedHighlight() As Color
            Get
                Return Color.FromArgb(150, 179, 225)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedHighlightBorder() As Color
            Get
                Return Color.FromArgb(49, 106, 197)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedHighlight() As Color
            Get
                Return Color.FromArgb(195, 211, 237)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedHighlightBorder() As Color
            Get
                Return Color.FromArgb(49, 106, 197)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedBorder() As Color
            Get
                Return Color.FromArgb(0, 0, 128)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedBorder() As Color
            Get
                Return Color.FromArgb(0, 0, 128)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 223, 154)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 195, 116)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 166, 76)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 255, 222)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 225, 172)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 203, 136)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(254, 128, 62)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 177, 109)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 223, 154)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckBackground() As Color
            Get
                Return Color.FromArgb(255, 192, 111)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckSelectedBackground() As Color
            Get
                Return Color.FromArgb(254, 128, 62)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckPressedBackground() As Color
            Get
                Return Color.FromArgb(254, 128, 62)
            End Get
        End Property

        Public Overrides ReadOnly Property GripDark() As Color
            Get
                Return Color.FromArgb(39, 65, 118)
            End Get
        End Property

        Public Overrides ReadOnly Property GripLight() As Color
            Get
                Return Color.FromArgb(255, 255, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientBegin() As Color
            Get
                Return Color.FromArgb(227, 239, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientMiddle() As Color
            Get
                Return Color.FromArgb(203, 225, 252)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientEnd() As Color
            Get
                Return Color.FromArgb(123, 164, 224)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientBegin() As Color
            Get
                Return Color.FromArgb(203, 221, 246)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientMiddle() As Color
            Get
                Return Color.FromArgb(161, 197, 249)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientEnd() As Color
            Get
                Return Color.FromArgb(114, 155, 215)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientBegin() As Color
            Get
                Return Color.FromArgb(158, 190, 245)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientEnd() As Color
            Get
                Return Color.FromArgb(196, 218, 250)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelected() As Color
            Get
                Return Color.FromArgb(255, 238, 194)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemBorder() As Color
            Get
                Return Color.FromArgb(0, 0, 128)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuBorder() As Color
            Get
                Return Color.FromArgb(0, 45, 150)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 255, 222)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 203, 136)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(227, 239, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(161, 197, 249)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(123, 164, 224)
            End Get
        End Property

        Public Overrides ReadOnly Property RaftingContainerGradientBegin() As Color
            Get
                Return Color.FromArgb(158, 190, 245)
            End Get
        End Property

        Public Overrides ReadOnly Property RaftingContainerGradientEnd() As Color
            Get
                Return Color.FromArgb(196, 218, 250)
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorDark() As Color
            Get
                Return Color.FromArgb(106, 140, 203)
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorLight() As Color
            Get
                Return Color.FromArgb(241, 249, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientBegin() As Color
            Get
                Return Color.FromArgb(158, 190, 245)
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientEnd() As Color
            Get
                Return Color.FromArgb(196, 218, 250)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripBorder() As Color
            Get
                Return Color.FromArgb(59, 97, 156)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripDropDownBackground() As Color
            Get
                Return Color.FromArgb(246, 246, 246)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientBegin() As Color
            Get
                Return Color.FromArgb(227, 239, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientMiddle() As Color
            Get
                Return Color.FromArgb(203, 225, 252)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientEnd() As Color
            Get
                Return Color.FromArgb(123, 164, 224)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(158, 190, 245)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(196, 218, 250)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(158, 190, 245)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(196, 218, 250)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientBegin() As Color
            Get
                Return Color.FromArgb(127, 177, 250)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientMiddle() As Color
            Get
                Return Color.FromArgb(82, 127, 208)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientEnd() As Color
            Get
                Return Color.FromArgb(0, 53, 145)
            End Get
        End Property

    End Class
    '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Public Class Office2003SilverColorTable
        Inherits ProfessionalColorTable

        Public Overrides ReadOnly Property ButtonSelectedHighlight() As Color
            Get
                Return Color.FromArgb(231, 232, 235)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedHighlightBorder() As Color
            Get
                Return Color.FromArgb(75, 75, 111)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedHighlight() As Color
            Get
                Return Color.FromArgb(215, 216, 222)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedHighlightBorder() As Color
            Get
                Return Color.FromArgb(178, 180, 191)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedHighlight() As Color
            Get
                Return Color.FromArgb(231, 232, 235)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedHighlightBorder() As Color
            Get
                Return Color.FromArgb(178, 180, 191)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedBorder() As Color
            Get
                Return Color.FromArgb(75, 75, 111)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedBorder() As Color
            Get
                Return Color.FromArgb(75, 75, 111)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 223, 154)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 195, 116)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 166, 76)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 255, 222)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 225, 172)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 203, 136)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(254, 128, 62)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 177, 109)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 223, 154)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckBackground() As Color
            Get
                Return Color.FromArgb(255, 192, 111)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckSelectedBackground() As Color
            Get
                Return Color.FromArgb(254, 128, 62)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckPressedBackground() As Color
            Get
                Return Color.FromArgb(254, 128, 62)
            End Get
        End Property

        Public Overrides ReadOnly Property GripDark() As Color
            Get
                Return Color.FromArgb(84, 84, 117)
            End Get
        End Property

        Public Overrides ReadOnly Property GripLight() As Color
            Get
                Return Color.FromArgb(255, 255, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientBegin() As Color
            Get
                Return Color.FromArgb(249, 249, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientMiddle() As Color
            Get
                Return Color.FromArgb(225, 226, 236)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientEnd() As Color
            Get
                Return Color.FromArgb(147, 145, 176)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientBegin() As Color
            Get
                Return Color.FromArgb(215, 215, 226)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientMiddle() As Color
            Get
                Return Color.FromArgb(184, 185, 202)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientEnd() As Color
            Get
                Return Color.FromArgb(118, 116, 151)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientBegin() As Color
            Get
                Return Color.FromArgb(215, 215, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientEnd() As Color
            Get
                Return Color.FromArgb(243, 243, 247)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelected() As Color
            Get
                Return Color.FromArgb(255, 238, 194)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemBorder() As Color
            Get
                Return Color.FromArgb(75, 75, 111)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuBorder() As Color
            Get
                Return Color.FromArgb(124, 124, 148)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 255, 222)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 203, 136)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(232, 233, 242)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(184, 185, 202)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(172, 170, 194)
            End Get
        End Property

        Public Overrides ReadOnly Property RaftingContainerGradientBegin() As Color
            Get
                Return Color.FromArgb(215, 215, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property RaftingContainerGradientEnd() As Color
            Get
                Return Color.FromArgb(243, 243, 247)
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorDark() As Color
            Get
                Return Color.FromArgb(110, 109, 143)
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorLight() As Color
            Get
                Return Color.FromArgb(255, 255, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientBegin() As Color
            Get
                Return Color.FromArgb(215, 215, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientEnd() As Color
            Get
                Return Color.FromArgb(243, 243, 247)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripBorder() As Color
            Get
                Return Color.FromArgb(124, 124, 148)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripDropDownBackground() As Color
            Get
                Return Color.FromArgb(253, 250, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientBegin() As Color
            Get
                Return Color.FromArgb(249, 249, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientMiddle() As Color
            Get
                Return Color.FromArgb(225, 226, 236)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientEnd() As Color
            Get
                Return Color.FromArgb(147, 145, 176)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(215, 215, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(243, 243, 247)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(215, 215, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(243, 243, 247)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientBegin() As Color
            Get
                Return Color.FromArgb(186, 185, 206)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientMiddle() As Color
            Get
                Return Color.FromArgb(156, 155, 180)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientEnd() As Color
            Get
                Return Color.FromArgb(118, 116, 146)
            End Get
        End Property

    End Class
    '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Public Class Office2003OliveColorTable
        Inherits ProfessionalColorTable
        Public Overrides ReadOnly Property ButtonSelectedHighlight() As Color
            Get
                Return Color.FromArgb(223, 227, 213)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedHighlightBorder() As Color
            Get
                Return Color.FromArgb(63, 93, 56)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedHighlight() As Color
            Get
                Return Color.FromArgb(200, 206, 182)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedHighlightBorder() As Color
            Get
                Return Color.FromArgb(147, 160, 112)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedHighlight() As Color
            Get
                Return Color.FromArgb(223, 227, 213)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedHighlightBorder() As Color
            Get
                Return Color.FromArgb(147, 160, 112)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedBorder() As Color
            Get
                Return Color.FromArgb(63, 93, 56)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedBorder() As Color
            Get
                Return Color.FromArgb(63, 93, 56)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 223, 154)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 195, 116)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 166, 76)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 255, 222)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 225, 172)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 203, 136)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(254, 128, 62)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(255, 177, 109)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 223, 154)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckBackground() As Color
            Get
                Return Color.FromArgb(255, 192, 111)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckSelectedBackground() As Color
            Get
                Return Color.FromArgb(254, 128, 62)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckPressedBackground() As Color
            Get
                Return Color.FromArgb(254, 128, 62)
            End Get
        End Property

        Public Overrides ReadOnly Property GripDark() As Color
            Get
                Return Color.FromArgb(81, 94, 51)
            End Get
        End Property

        Public Overrides ReadOnly Property GripLight() As Color
            Get
                Return Color.FromArgb(255, 255, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 255, 237)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientMiddle() As Color
            Get
                Return Color.FromArgb(206, 220, 167)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientEnd() As Color
            Get
                Return Color.FromArgb(181, 196, 143)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientBegin() As Color
            Get
                Return Color.FromArgb(230, 230, 209)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientMiddle() As Color
            Get
                Return Color.FromArgb(186, 201, 143)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientEnd() As Color
            Get
                Return Color.FromArgb(160, 177, 116)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientBegin() As Color
            Get
                Return Color.FromArgb(217, 217, 167)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientEnd() As Color
            Get
                Return Color.FromArgb(242, 241, 228)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelected() As Color
            Get
                Return Color.FromArgb(255, 238, 194)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemBorder() As Color
            Get
                Return Color.FromArgb(63, 93, 56)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuBorder() As Color
            Get
                Return Color.FromArgb(117, 141, 94)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 255, 222)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(255, 203, 136)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(237, 240, 214)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(186, 201, 143)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(181, 196, 143)
            End Get
        End Property

        Public Overrides ReadOnly Property RaftingContainerGradientBegin() As Color
            Get
                Return Color.FromArgb(217, 217, 167)
            End Get
        End Property

        Public Overrides ReadOnly Property RaftingContainerGradientEnd() As Color
            Get
                Return Color.FromArgb(242, 241, 228)
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorDark() As Color
            Get
                Return Color.FromArgb(96, 128, 88)
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorLight() As Color
            Get
                Return Color.FromArgb(244, 247, 222)
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientBegin() As Color
            Get
                Return Color.FromArgb(217, 217, 167)
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientEnd() As Color
            Get
                Return Color.FromArgb(242, 241, 228)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripBorder() As Color
            Get
                Return Color.FromArgb(96, 128, 88)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripDropDownBackground() As Color
            Get
                Return Color.FromArgb(244, 244, 238)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientBegin() As Color
            Get
                Return Color.FromArgb(255, 255, 237)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientMiddle() As Color
            Get
                Return Color.FromArgb(206, 220, 167)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientEnd() As Color
            Get
                Return Color.FromArgb(181, 196, 143)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(217, 217, 167)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(242, 241, 228)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(217, 217, 167)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(242, 241, 228)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientBegin() As Color
            Get
                Return Color.FromArgb(186, 204, 150)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientMiddle() As Color
            Get
                Return Color.FromArgb(141, 160, 107)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientEnd() As Color
            Get
                Return Color.FromArgb(96, 119, 107)
            End Get
        End Property

    End Class
    '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Public Class OfficeXPColorTable
        Inherits ProfessionalColorTable
        Public Overrides ReadOnly Property ButtonSelectedHighlight() As Color
            Get
                Return Color.FromArgb(196, 208, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedHighlightBorder() As Color
            Get
                Return Color.FromArgb(51, 94, 168)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedHighlight() As Color
            Get
                Return Color.FromArgb(152, 173, 210)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedHighlightBorder() As Color
            Get
                Return Color.FromArgb(51, 94, 168)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedHighlight() As Color
            Get
                Return Color.FromArgb(196, 208, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedHighlightBorder() As Color
            Get
                Return Color.FromArgb(51, 94, 168)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedBorder() As Color
            Get
                Return Color.FromArgb(51, 94, 168)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedBorder() As Color
            Get
                Return Color.FromArgb(51, 94, 168)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientBegin() As Color
            Get
                Return Color.FromArgb(226, 229, 238)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientMiddle() As Color
            Get
                Return Color.FromArgb(226, 229, 238)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientEnd() As Color
            Get
                Return Color.FromArgb(226, 229, 238)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(194, 207, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientMiddle() As Color
            Get
                Return Color.FromArgb(194, 207, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(194, 207, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(153, 175, 212)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(153, 175, 212)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(153, 175, 212)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckBackground() As Color
            Get
                Return Color.FromArgb(226, 229, 238)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckSelectedBackground() As Color
            Get
                Return Color.FromArgb(51, 94, 168)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckPressedBackground() As Color
            Get
                Return Color.FromArgb(51, 94, 168)
            End Get
        End Property

        Public Overrides ReadOnly Property GripDark() As Color
            Get
                Return Color.FromArgb(189, 188, 191)
            End Get
        End Property

        Public Overrides ReadOnly Property GripLight() As Color
            Get
                Return Color.FromArgb(255, 255, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientBegin() As Color
            Get
                Return Color.FromArgb(252, 252, 252)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientMiddle() As Color
            Get
                Return Color.FromArgb(245, 244, 246)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientEnd() As Color
            Get
                Return Color.FromArgb(235, 233, 237)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientBegin() As Color
            Get
                Return Color.FromArgb(247, 246, 248)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientMiddle() As Color
            Get
                Return Color.FromArgb(241, 240, 242)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientEnd() As Color
            Get
                Return Color.FromArgb(228, 226, 230)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientBegin() As Color
            Get
                Return Color.FromArgb(235, 233, 237)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientEnd() As Color
            Get
                Return Color.FromArgb(251, 250, 251)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelected() As Color
            Get
                Return Color.FromArgb(194, 207, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemBorder() As Color
            Get
                Return Color.FromArgb(51, 94, 168)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuBorder() As Color
            Get
                Return Color.FromArgb(134, 133, 136)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(194, 207, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(194, 207, 229)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(252, 252, 252)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(241, 240, 242)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(245, 244, 246)
            End Get
        End Property

        Public Overrides ReadOnly Property RaftingContainerGradientBegin() As Color
            Get
                Return Color.FromArgb(235, 233, 237)
            End Get
        End Property

        Public Overrides ReadOnly Property RaftingContainerGradientEnd() As Color
            Get
                Return Color.FromArgb(251, 250, 251)
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorDark() As Color
            Get
                Return Color.FromArgb(193, 193, 196)
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorLight() As Color
            Get
                Return Color.FromArgb(255, 255, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientBegin() As Color
            Get
                Return Color.FromArgb(235, 233, 237)
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientEnd() As Color
            Get
                Return Color.FromArgb(251, 250, 251)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripBorder() As Color
            Get
                Return Color.FromArgb(238, 237, 240)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripDropDownBackground() As Color
            Get
                Return Color.FromArgb(252, 252, 252)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientBegin() As Color
            Get
                Return Color.FromArgb(252, 252, 252)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientMiddle() As Color
            Get
                Return Color.FromArgb(245, 244, 246)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientEnd() As Color
            Get
                Return Color.FromArgb(235, 233, 237)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(235, 233, 237)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(251, 250, 251)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(235, 233, 237)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(251, 250, 251)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientBegin() As Color
            Get
                Return Color.FromArgb(242, 242, 242)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientMiddle() As Color
            Get
                Return Color.FromArgb(224, 224, 225)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientEnd() As Color
            Get
                Return Color.FromArgb(167, 166, 170)
            End Get
        End Property


    End Class
    '~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    Public Class OfficeClassicColorTable
        Inherits ProfessionalColorTable
        Public Overrides ReadOnly Property ButtonSelectedHighlight() As Color
            Get
                Return Color.FromArgb(184, 191, 211)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedHighlightBorder() As Color
            Get
                Return Color.FromArgb(10, 36, 106)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedHighlight() As Color
            Get
                Return Color.FromArgb(131, 144, 179)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedHighlightBorder() As Color
            Get
                Return Color.FromArgb(10, 36, 106)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedHighlight() As Color
            Get
                Return Color.FromArgb(184, 191, 211)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedHighlightBorder() As Color
            Get
                Return Color.FromArgb(10, 36, 106)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedBorder() As Color
            Get
                Return Color.FromArgb(10, 36, 106)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedBorder() As Color
            Get
                Return Color.FromArgb(10, 36, 106)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientBegin() As Color
            Get
                Return Color.FromArgb(0, 0, 0)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientMiddle() As Color
            Get
                Return Color.FromArgb(0, 0, 0)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonCheckedGradientEnd() As Color
            Get
                Return Color.FromArgb(0, 0, 0)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(182, 189, 210)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientMiddle() As Color
            Get
                Return Color.FromArgb(182, 189, 210)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(182, 189, 210)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(133, 146, 181)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(133, 146, 181)
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(133, 146, 181)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckBackground() As Color
            Get
                Return Color.FromArgb(10, 36, 106)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckSelectedBackground() As Color
            Get
                Return Color.FromArgb(133, 146, 181)
            End Get
        End Property

        Public Overrides ReadOnly Property CheckPressedBackground() As Color
            Get
                Return Color.FromArgb(133, 146, 181)
            End Get
        End Property

        Public Overrides ReadOnly Property GripDark() As Color
            Get
                Return Color.FromArgb(160, 160, 160)
            End Get
        End Property

        Public Overrides ReadOnly Property GripLight() As Color
            Get
                Return Color.FromArgb(255, 255, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientBegin() As Color
            Get
                Return Color.FromArgb(245, 244, 242)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientMiddle() As Color
            Get
                Return Color.FromArgb(234, 232, 228)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginGradientEnd() As Color
            Get
                Return Color.FromArgb(212, 208, 200)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientBegin() As Color
            Get
                Return Color.FromArgb(238, 236, 233)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientMiddle() As Color
            Get
                Return Color.FromArgb(225, 222, 217)
            End Get
        End Property

        Public Overrides ReadOnly Property ImageMarginRevealedGradientEnd() As Color
            Get
                Return Color.FromArgb(216, 213, 206)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientBegin() As Color
            Get
                Return Color.FromArgb(212, 208, 200)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuStripGradientEnd() As Color
            Get
                Return Color.FromArgb(246, 245, 244)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelected() As Color
            Get
                Return Color.FromArgb(255, 255, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemBorder() As Color
            Get
                Return Color.FromArgb(10, 36, 106)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuBorder() As Color
            Get
                Return Color.FromArgb(102, 102, 102)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientBegin() As Color
            Get
                Return Color.FromArgb(182, 189, 210)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemSelectedGradientEnd() As Color
            Get
                Return Color.FromArgb(182, 189, 210)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientBegin() As Color
            Get
                Return Color.FromArgb(245, 244, 242)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientMiddle() As Color
            Get
                Return Color.FromArgb(225, 222, 217)
            End Get
        End Property

        Public Overrides ReadOnly Property MenuItemPressedGradientEnd() As Color
            Get
                Return Color.FromArgb(234, 232, 228)
            End Get
        End Property

        Public Overrides ReadOnly Property RaftingContainerGradientBegin() As Color
            Get
                Return Color.FromArgb(212, 208, 200)
            End Get
        End Property

        Public Overrides ReadOnly Property RaftingContainerGradientEnd() As Color
            Get
                Return Color.FromArgb(246, 245, 244)
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorDark() As Color
            Get
                Return Color.FromArgb(166, 166, 166)
            End Get
        End Property

        Public Overrides ReadOnly Property SeparatorLight() As Color
            Get
                Return Color.FromArgb(255, 255, 255)
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientBegin() As Color
            Get
                Return Color.FromArgb(212, 208, 200)
            End Get
        End Property

        Public Overrides ReadOnly Property StatusStripGradientEnd() As Color
            Get
                Return Color.FromArgb(246, 245, 244)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripBorder() As Color
            Get
                Return Color.FromArgb(219, 216, 209)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripDropDownBackground() As Color
            Get
                Return Color.FromArgb(249, 248, 247)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientBegin() As Color
            Get
                Return Color.FromArgb(245, 244, 242)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientMiddle() As Color
            Get
                Return Color.FromArgb(234, 232, 228)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripGradientEnd() As Color
            Get
                Return Color.FromArgb(212, 208, 200)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(212, 208, 200)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripContentPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(246, 245, 244)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientBegin() As Color
            Get
                Return Color.FromArgb(212, 208, 200)
            End Get
        End Property

        Public Overrides ReadOnly Property ToolStripPanelGradientEnd() As Color
            Get
                Return Color.FromArgb(246, 245, 244)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientBegin() As Color
            Get
                Return Color.FromArgb(225, 222, 217)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientMiddle() As Color
            Get
                Return Color.FromArgb(216, 213, 206)
            End Get
        End Property

        Public Overrides ReadOnly Property OverflowButtonGradientEnd() As Color
            Get
                Return Color.FromArgb(128, 128, 128)
            End Get
        End Property

    End Class

#End Region

    Public Overridable Sub OnAppearanceChanged(ByVal e As EventArgs)
        RaiseEvent AppearanceChanged(Me, e)
    End Sub

End Class
