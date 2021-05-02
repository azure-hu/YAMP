namespace Azure.YAMP
{
    partial class PlayerUI3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerUI3));
			this.openMedia = new System.Windows.Forms.OpenFileDialog();
			this.mainTimer = new System.Windows.Forms.Timer(this.components);
			this.statusStrip = new Azure.LibCollection.VB.CustomizableStatusStrip();
			this.appearanceControl1 = new Azure.LibCollection.VB.AppearanceControl();
			this.hintLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.topSwitchBtn = new System.Windows.Forms.Button();
			this.minBtn = new System.Windows.Forms.Button();
			this.closeBtn = new System.Windows.Forms.Button();
			this.playBtn = new System.Windows.Forms.Button();
			this.pauseBtn = new System.Windows.Forms.Button();
			this.stopBtn = new System.Windows.Forms.Button();
			this.openBtn = new System.Windows.Forms.Button();
			this.repLabel = new System.Windows.Forms.Label();
			this.plistBtn = new System.Windows.Forms.Button();
			this.nextBtn = new System.Windows.Forms.Button();
			this.prevBtn = new System.Windows.Forms.Button();
			this.tagfmBtn = new System.Windows.Forms.Button();
			this.volLabel = new System.Windows.Forms.Label();
			this.chanLabel = new System.Windows.Forms.Label();
			this.sampleRateLabel = new System.Windows.Forms.Label();
			this.bitRateLabel = new System.Windows.Forms.Label();
			this.visualBox = new System.Windows.Forms.PictureBox();
			this.displayTimeLabel = new System.Windows.Forms.Label();
			this.mainPanel = new System.Windows.Forms.Panel();
			this.kbpsLabel = new System.Windows.Forms.Label();
			this.khzLabel = new System.Windows.Forms.Label();
			this.chLabel = new System.Windows.Forms.Label();
			this.visBtn = new System.Windows.Forms.Button();
			this.repeatCheckBox = new System.Windows.Forms.CheckBox();
			this.mainLabel = new Azure.LibCollection.CS.MarqueeLabel();
			this.volumeKnob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
			this.opacityKnob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.infoPage = new System.Windows.Forms.TabPage();
			this.infoPanel = new System.Windows.Forms.Panel();
			this.fileBox = new System.Windows.Forms.Label();
			this.albumBox = new System.Windows.Forms.Label();
			this.artistBox = new System.Windows.Forms.Label();
			this.titleBox = new System.Windows.Forms.Label();
			this.coverBox = new System.Windows.Forms.PictureBox();
			this.titIco = new System.Windows.Forms.PictureBox();
			this.albIco = new System.Windows.Forms.PictureBox();
			this.artIco = new System.Windows.Forms.PictureBox();
			this.fileLabel = new System.Windows.Forms.Label();
			this.eqPage = new System.Windows.Forms.TabPage();
			this.eqPanel = new System.Windows.Forms.Panel();
			this.resetEqBtn = new System.Windows.Forms.Button();
			this.freqLabel9 = new System.Windows.Forms.Label();
			this.freqLabel8 = new System.Windows.Forms.Label();
			this.freqLabel7 = new System.Windows.Forms.Label();
			this.freqLabel6 = new System.Windows.Forms.Label();
			this.freqLabel5 = new System.Windows.Forms.Label();
			this.freqLabel4 = new System.Windows.Forms.Label();
			this.freqLabel3 = new System.Windows.Forms.Label();
			this.freqLabel2 = new System.Windows.Forms.Label();
			this.freqLabel1 = new System.Windows.Forms.Label();
			this.freqLabel0 = new System.Windows.Forms.Label();
			this.freqBar9 = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
			this.freqBar8 = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
			this.freqBar7 = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
			this.freqBar6 = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
			this.freqBar5 = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
			this.freqBar4 = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
			this.freqBar3 = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
			this.freqBar2 = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
			this.freqBar1 = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
			this.freqBar0 = new Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar();
			this.outputPage = new System.Windows.Forms.TabPage();
			this.outputPanel = new System.Windows.Forms.Panel();
			this.refreshOutBtn = new System.Windows.Forms.Button();
			this.switchOutBtn = new System.Windows.Forms.Button();
			this.currDeviceBox = new System.Windows.Forms.TextBox();
			this.currDeviceLabel = new System.Windows.Forms.Label();
			this.outputCombo = new System.Windows.Forms.ComboBox();
			this.outputLabel = new System.Windows.Forms.Label();
			this.mainMenuStrip = new Azure.LibCollection.VB.CustomizableMenuStrip();
			this.addToListMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.folderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allSubFoldersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectedOnlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeFromListMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.selectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.listMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.pinMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchBox = new Azure.LibCollection.CS.Controls.ToolStripSpringTextBox();
			this.seekBar = new Azure.LibCollection.CS.ColorProgressBar();
			this.playListView = new Azure.LibCollection.CS.ListViewEx();
			this.titleHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.artistHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.durationHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.loadingLabel = new System.Windows.Forms.Label();
			this.playlistLoadingBar = new System.Windows.Forms.ProgressBar();
			this.statusStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.visualBox)).BeginInit();
			this.mainPanel.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.infoPage.SuspendLayout();
			this.infoPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.coverBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.titIco)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.albIco)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.artIco)).BeginInit();
			this.eqPage.SuspendLayout();
			this.eqPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.freqBar9)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar0)).BeginInit();
			this.outputPage.SuspendLayout();
			this.outputPanel.SuspendLayout();
			this.mainMenuStrip.SuspendLayout();
			this.playListView.SuspendLayout();
			this.loadingLabel.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainTimer
			// 
			this.mainTimer.Interval = 500;
			this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
			// 
			// statusStrip
			// 
			this.statusStrip.Appearance = this.appearanceControl1;
			this.statusStrip.BackgroundImage = global::Azure.YAMP.Properties.Resources.gradientMenuStripBack;
			this.statusStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.statusStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hintLabel});
			this.statusStrip.Location = new System.Drawing.Point(0, 419);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(704, 22);
			this.statusStrip.TabIndex = 71;
			// 
			// appearanceControl1
			// 
			this.appearanceControl1.CustomAppearance.ButtonAppearance.CheckedAppearance.intBackground = -16273;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.CheckedAppearance.intBorderHighlight = -13410648;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.CheckedAppearance.intGradientBegin = -8294;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.CheckedAppearance.intGradientEnd = -22964;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.CheckedAppearance.intGradientMiddle = -15500;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.CheckedAppearance.intHighlight = -3878683;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.CheckedAppearance.intPressedBackground = -98242;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.CheckedAppearance.intSelectedBackground = -98242;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.PressedAppearance.Border = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
			this.appearanceControl1.CustomAppearance.ButtonAppearance.PressedAppearance.intBorder = -16777088;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.PressedAppearance.intBorderHighlight = -13410648;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.PressedAppearance.intGradientBegin = -98242;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.PressedAppearance.intGradientEnd = -8294;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.PressedAppearance.intGradientMiddle = -20115;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.PressedAppearance.intHighlight = -6771246;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.SelectedAppearance.Border = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
			this.appearanceControl1.CustomAppearance.ButtonAppearance.SelectedAppearance.BorderHighlight = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
			this.appearanceControl1.CustomAppearance.ButtonAppearance.SelectedAppearance.intBorder = -16777088;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.SelectedAppearance.intBorderHighlight = -16777088;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.SelectedAppearance.intGradientBegin = -34;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.SelectedAppearance.intGradientEnd = -13432;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.SelectedAppearance.intGradientMiddle = -7764;
			this.appearanceControl1.CustomAppearance.ButtonAppearance.SelectedAppearance.intHighlight = -3878683;
			this.appearanceControl1.CustomAppearance.GripAppearance.intDark = -14204554;
			this.appearanceControl1.CustomAppearance.GripAppearance.intLight = -1;
			this.appearanceControl1.CustomAppearance.GripAppearance.Light = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
			this.appearanceControl1.CustomAppearance.ImageMarginAppearance.Normal.intGradientBegin = -1839105;
			this.appearanceControl1.CustomAppearance.ImageMarginAppearance.Normal.intGradientEnd = -8674080;
			this.appearanceControl1.CustomAppearance.ImageMarginAppearance.Normal.intGradientMiddle = -3415556;
			this.appearanceControl1.CustomAppearance.ImageMarginAppearance.Revealed.intGradientBegin = -3416586;
			this.appearanceControl1.CustomAppearance.ImageMarginAppearance.Revealed.intGradientEnd = -9266217;
			this.appearanceControl1.CustomAppearance.ImageMarginAppearance.Revealed.intGradientMiddle = -6175239;
			this.appearanceControl1.CustomAppearance.MenuItemAppearance.Border = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(128)))));
			this.appearanceControl1.CustomAppearance.MenuItemAppearance.intBorder = -16777088;
			this.appearanceControl1.CustomAppearance.MenuItemAppearance.intPressedGradientBegin = -1839105;
			this.appearanceControl1.CustomAppearance.MenuItemAppearance.intPressedGradientEnd = -8674080;
			this.appearanceControl1.CustomAppearance.MenuItemAppearance.intPressedGradientMiddle = -6175239;
			this.appearanceControl1.CustomAppearance.MenuItemAppearance.intSelected = -4414;
			this.appearanceControl1.CustomAppearance.MenuItemAppearance.intSelectedGradientBegin = -34;
			this.appearanceControl1.CustomAppearance.MenuItemAppearance.intSelectedGradientEnd = -13432;
			this.appearanceControl1.CustomAppearance.MenuStripAppearance.intBorder = -16765546;
			this.appearanceControl1.CustomAppearance.MenuStripAppearance.intGradientBegin = -6373643;
			this.appearanceControl1.CustomAppearance.MenuStripAppearance.intGradientEnd = -3876102;
			this.appearanceControl1.CustomAppearance.OverflowButtonAppearance.intGradientBegin = -8408582;
			this.appearanceControl1.CustomAppearance.OverflowButtonAppearance.intGradientEnd = -16763503;
			this.appearanceControl1.CustomAppearance.OverflowButtonAppearance.intGradientMiddle = -11370544;
			this.appearanceControl1.CustomAppearance.RaftingContainerAppearance.intGradientBegin = -6373643;
			this.appearanceControl1.CustomAppearance.RaftingContainerAppearance.intGradientEnd = -3876102;
			this.appearanceControl1.CustomAppearance.SeparatorAppearance.intDark = -9794357;
			this.appearanceControl1.CustomAppearance.SeparatorAppearance.intLight = -919041;
			this.appearanceControl1.CustomAppearance.StatusStripAppearance.intGradientBegin = -6373643;
			this.appearanceControl1.CustomAppearance.StatusStripAppearance.intGradientEnd = -3876102;
			this.appearanceControl1.CustomAppearance.ToolStripAppearance.intBorder = -12885604;
			this.appearanceControl1.CustomAppearance.ToolStripAppearance.intContentPanelGradientBegin = -6373643;
			this.appearanceControl1.CustomAppearance.ToolStripAppearance.intContentPanelGradientEnd = -3876102;
			this.appearanceControl1.CustomAppearance.ToolStripAppearance.intDropDownBackground = -592138;
			this.appearanceControl1.CustomAppearance.ToolStripAppearance.intGradientBegin = -1839105;
			this.appearanceControl1.CustomAppearance.ToolStripAppearance.intGradientEnd = -8674080;
			this.appearanceControl1.CustomAppearance.ToolStripAppearance.intGradientMiddle = -3415556;
			this.appearanceControl1.CustomAppearance.ToolStripAppearance.intPanelGradientBegin = -6373643;
			this.appearanceControl1.CustomAppearance.ToolStripAppearance.intPanelGradientEnd = -3876102;
			this.appearanceControl1.Preset = Azure.LibCollection.VB.AppearanceControl.enumPresetStyles.Office2003Blue;
			this.appearanceControl1.Renderer.RoundedEdges = true;
			// 
			// hintLabel
			// 
			this.hintLabel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.hintLabel.ForeColor = System.Drawing.Color.Black;
			this.hintLabel.Name = "hintLabel";
			this.hintLabel.Size = new System.Drawing.Size(658, 17);
			this.hintLabel.Spring = true;
			this.hintLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// topSwitchBtn
			// 
			this.topSwitchBtn.BackColor = System.Drawing.Color.Transparent;
			this.topSwitchBtn.Enabled = false;
			this.topSwitchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.topSwitchBtn.ForeColor = System.Drawing.Color.Transparent;
			this.topSwitchBtn.Location = new System.Drawing.Point(310, 1);
			this.topSwitchBtn.Name = "topSwitchBtn";
			this.topSwitchBtn.Size = new System.Drawing.Size(10, 10);
			this.topSwitchBtn.TabIndex = 38;
			this.topSwitchBtn.UseVisualStyleBackColor = false;
			this.topSwitchBtn.Visible = false;
			this.topSwitchBtn.Click += new System.EventHandler(this.topSwitchBtn_Click);
			// 
			// minBtn
			// 
			this.minBtn.BackColor = System.Drawing.Color.Transparent;
			this.minBtn.Enabled = false;
			this.minBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.minBtn.ForeColor = System.Drawing.Color.Transparent;
			this.minBtn.Location = new System.Drawing.Point(300, 1);
			this.minBtn.Name = "minBtn";
			this.minBtn.Size = new System.Drawing.Size(10, 10);
			this.minBtn.TabIndex = 38;
			this.minBtn.UseVisualStyleBackColor = false;
			this.minBtn.Visible = false;
			this.minBtn.Click += new System.EventHandler(this.minBtn_Click);
			// 
			// closeBtn
			// 
			this.closeBtn.BackColor = System.Drawing.Color.Transparent;
			this.closeBtn.Enabled = false;
			this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.closeBtn.ForeColor = System.Drawing.Color.Transparent;
			this.closeBtn.Location = new System.Drawing.Point(320, 1);
			this.closeBtn.Name = "closeBtn";
			this.closeBtn.Size = new System.Drawing.Size(10, 10);
			this.closeBtn.TabIndex = 2;
			this.closeBtn.UseVisualStyleBackColor = false;
			this.closeBtn.Visible = false;
			this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
			// 
			// playBtn
			// 
			this.playBtn.BackColor = System.Drawing.Color.Transparent;
			this.playBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.playBtn.ForeColor = System.Drawing.Color.Transparent;
			this.playBtn.Image = global::Azure.YAMP.Properties.Resources.playBtn1;
			this.playBtn.Location = new System.Drawing.Point(176, 109);
			this.playBtn.Name = "playBtn";
			this.playBtn.Size = new System.Drawing.Size(26, 20);
			this.playBtn.TabIndex = 3;
			this.playBtn.UseVisualStyleBackColor = true;
			this.playBtn.Click += new System.EventHandler(this.playBtn_Click);
			// 
			// pauseBtn
			// 
			this.pauseBtn.BackColor = System.Drawing.Color.Transparent;
			this.pauseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.pauseBtn.ForeColor = System.Drawing.Color.Transparent;
			this.pauseBtn.Image = global::Azure.YAMP.Properties.Resources.pauseBtn1;
			this.pauseBtn.Location = new System.Drawing.Point(202, 109);
			this.pauseBtn.Name = "pauseBtn";
			this.pauseBtn.Size = new System.Drawing.Size(26, 20);
			this.pauseBtn.TabIndex = 4;
			this.pauseBtn.UseVisualStyleBackColor = true;
			this.pauseBtn.Click += new System.EventHandler(this.pauseBtn_Click);
			// 
			// stopBtn
			// 
			this.stopBtn.BackColor = System.Drawing.Color.Transparent;
			this.stopBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.stopBtn.ForeColor = System.Drawing.Color.Transparent;
			this.stopBtn.Image = global::Azure.YAMP.Properties.Resources.stopBtn1;
			this.stopBtn.Location = new System.Drawing.Point(228, 109);
			this.stopBtn.Name = "stopBtn";
			this.stopBtn.Size = new System.Drawing.Size(26, 20);
			this.stopBtn.TabIndex = 5;
			this.stopBtn.UseVisualStyleBackColor = true;
			this.stopBtn.Click += new System.EventHandler(this.stopBtn_Click);
			// 
			// openBtn
			// 
			this.openBtn.BackColor = System.Drawing.Color.Transparent;
			this.openBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.openBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.openBtn.ForeColor = System.Drawing.Color.Transparent;
			this.openBtn.Image = global::Azure.YAMP.Properties.Resources.openBtn1;
			this.openBtn.Location = new System.Drawing.Point(286, 109);
			this.openBtn.Name = "openBtn";
			this.openBtn.Size = new System.Drawing.Size(26, 20);
			this.openBtn.TabIndex = 0;
			this.openBtn.UseVisualStyleBackColor = true;
			this.openBtn.Click += new System.EventHandler(this.openBtn_Click);
			// 
			// repLabel
			// 
			this.repLabel.AutoSize = true;
			this.repLabel.BackColor = System.Drawing.Color.Transparent;
			this.repLabel.Font = new System.Drawing.Font("Tahoma", 6.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.repLabel.ForeColor = System.Drawing.Color.Gray;
			this.repLabel.Location = new System.Drawing.Point(170, 24);
			this.repLabel.Name = "repLabel";
			this.repLabel.Size = new System.Drawing.Size(48, 11);
			this.repLabel.TabIndex = 12;
			this.repLabel.Text = "Repeat off";
			this.repLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// plistBtn
			// 
			this.plistBtn.BackColor = System.Drawing.Color.Transparent;
			this.plistBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.plistBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.plistBtn.ForeColor = System.Drawing.Color.Transparent;
			this.plistBtn.Image = global::Azure.YAMP.Properties.Resources.plIcon;
			this.plistBtn.Location = new System.Drawing.Point(287, 87);
			this.plistBtn.Margin = new System.Windows.Forms.Padding(0);
			this.plistBtn.Name = "plistBtn";
			this.plistBtn.Size = new System.Drawing.Size(24, 18);
			this.plistBtn.TabIndex = 18;
			this.plistBtn.UseVisualStyleBackColor = true;
			this.plistBtn.Click += new System.EventHandler(this.plistBtn_Click);
			// 
			// nextBtn
			// 
			this.nextBtn.BackColor = System.Drawing.Color.Transparent;
			this.nextBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.nextBtn.ForeColor = System.Drawing.Color.Transparent;
			this.nextBtn.Image = global::Azure.YAMP.Properties.Resources.nextBtn1;
			this.nextBtn.Location = new System.Drawing.Point(254, 109);
			this.nextBtn.Name = "nextBtn";
			this.nextBtn.Size = new System.Drawing.Size(26, 20);
			this.nextBtn.TabIndex = 19;
			this.nextBtn.UseVisualStyleBackColor = true;
			this.nextBtn.Click += new System.EventHandler(this.nextBtn_Click);
			// 
			// prevBtn
			// 
			this.prevBtn.BackColor = System.Drawing.Color.Transparent;
			this.prevBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.prevBtn.ForeColor = System.Drawing.Color.Transparent;
			this.prevBtn.Image = global::Azure.YAMP.Properties.Resources.prevBtn1;
			this.prevBtn.Location = new System.Drawing.Point(150, 109);
			this.prevBtn.Name = "prevBtn";
			this.prevBtn.Size = new System.Drawing.Size(26, 20);
			this.prevBtn.TabIndex = 20;
			this.prevBtn.UseVisualStyleBackColor = true;
			this.prevBtn.Click += new System.EventHandler(this.prevBtn_Click);
			// 
			// tagfmBtn
			// 
			this.tagfmBtn.BackColor = System.Drawing.Color.Transparent;
			this.tagfmBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.tagfmBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.tagfmBtn.ForeColor = System.Drawing.Color.Transparent;
			this.tagfmBtn.Image = global::Azure.YAMP.Properties.Resources.tagIcon;
			this.tagfmBtn.Location = new System.Drawing.Point(287, 40);
			this.tagfmBtn.Name = "tagfmBtn";
			this.tagfmBtn.Size = new System.Drawing.Size(24, 18);
			this.tagfmBtn.TabIndex = 21;
			this.tagfmBtn.UseVisualStyleBackColor = true;
			this.tagfmBtn.Click += new System.EventHandler(this.tagfmBtn_Click);
			// 
			// volLabel
			// 
			this.volLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
			this.volLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.volLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.volLabel.ForeColor = System.Drawing.Color.Black;
			this.volLabel.Location = new System.Drawing.Point(153, 87);
			this.volLabel.Margin = new System.Windows.Forms.Padding(0);
			this.volLabel.Name = "volLabel";
			this.volLabel.Size = new System.Drawing.Size(38, 19);
			this.volLabel.TabIndex = 39;
			this.volLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chanLabel
			// 
			this.chanLabel.BackColor = System.Drawing.Color.Transparent;
			this.chanLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.chanLabel.ForeColor = System.Drawing.Color.Black;
			this.chanLabel.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.chanLabel.Location = new System.Drawing.Point(106, 40);
			this.chanLabel.Name = "chanLabel";
			this.chanLabel.Size = new System.Drawing.Size(26, 24);
			this.chanLabel.TabIndex = 38;
			this.chanLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.chanLabel.UseCompatibleTextRendering = true;
			// 
			// sampleRateLabel
			// 
			this.sampleRateLabel.BackColor = System.Drawing.Color.Transparent;
			this.sampleRateLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.sampleRateLabel.ForeColor = System.Drawing.Color.Black;
			this.sampleRateLabel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.sampleRateLabel.Location = new System.Drawing.Point(70, 60);
			this.sampleRateLabel.Margin = new System.Windows.Forms.Padding(0);
			this.sampleRateLabel.MinimumSize = new System.Drawing.Size(10, 10);
			this.sampleRateLabel.Name = "sampleRateLabel";
			this.sampleRateLabel.Size = new System.Drawing.Size(34, 24);
			this.sampleRateLabel.TabIndex = 6;
			this.sampleRateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.sampleRateLabel.UseCompatibleTextRendering = true;
			// 
			// bitRateLabel
			// 
			this.bitRateLabel.BackColor = System.Drawing.Color.Transparent;
			this.bitRateLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.bitRateLabel.ForeColor = System.Drawing.Color.Black;
			this.bitRateLabel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.bitRateLabel.Location = new System.Drawing.Point(4, 60);
			this.bitRateLabel.Margin = new System.Windows.Forms.Padding(0);
			this.bitRateLabel.MinimumSize = new System.Drawing.Size(10, 10);
			this.bitRateLabel.Name = "bitRateLabel";
			this.bitRateLabel.Size = new System.Drawing.Size(40, 24);
			this.bitRateLabel.TabIndex = 7;
			this.bitRateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.bitRateLabel.UseCompatibleTextRendering = true;
			// 
			// visualBox
			// 
			this.visualBox.BackColor = System.Drawing.Color.Transparent;
			this.visualBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.visualBox.ErrorImage = global::Azure.YAMP.Properties.Resources.transparent1;
			this.visualBox.Image = global::Azure.YAMP.Properties.Resources.transparent1;
			this.visualBox.InitialImage = global::Azure.YAMP.Properties.Resources.transparent1;
			this.visualBox.Location = new System.Drawing.Point(7, 87);
			this.visualBox.Name = "visualBox";
			this.visualBox.Size = new System.Drawing.Size(126, 42);
			this.visualBox.TabIndex = 1;
			this.visualBox.TabStop = false;
			this.visualBox.Click += new System.EventHandler(this.visualBox_Click);
			this.visualBox.Paint += new System.Windows.Forms.PaintEventHandler(this.visualBox_Paint);
			this.visualBox.Validated += new System.EventHandler(this.visualBox_Validated);
			// 
			// displayTimeLabel
			// 
			this.displayTimeLabel.BackColor = System.Drawing.Color.Transparent;
			this.displayTimeLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.displayTimeLabel.ForeColor = System.Drawing.Color.Black;
			this.displayTimeLabel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.displayTimeLabel.Location = new System.Drawing.Point(7, 40);
			this.displayTimeLabel.Margin = new System.Windows.Forms.Padding(0);
			this.displayTimeLabel.MinimumSize = new System.Drawing.Size(10, 10);
			this.displayTimeLabel.Name = "displayTimeLabel";
			this.displayTimeLabel.Size = new System.Drawing.Size(57, 24);
			this.displayTimeLabel.TabIndex = 9;
			this.displayTimeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.displayTimeLabel.UseCompatibleTextRendering = true;
			this.displayTimeLabel.Click += new System.EventHandler(this.displayTimeLabel_Click);
			// 
			// mainPanel
			// 
			this.mainPanel.BackColor = System.Drawing.Color.Transparent;
			this.mainPanel.BackgroundImage = global::Azure.YAMP.Properties.Resources.blue_main;
			this.mainPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.mainPanel.Controls.Add(this.bitRateLabel);
			this.mainPanel.Controls.Add(this.sampleRateLabel);
			this.mainPanel.Controls.Add(this.kbpsLabel);
			this.mainPanel.Controls.Add(this.khzLabel);
			this.mainPanel.Controls.Add(this.chLabel);
			this.mainPanel.Controls.Add(this.visBtn);
			this.mainPanel.Controls.Add(this.repeatCheckBox);
			this.mainPanel.Controls.Add(this.visualBox);
			this.mainPanel.Controls.Add(this.displayTimeLabel);
			this.mainPanel.Controls.Add(this.mainLabel);
			this.mainPanel.Controls.Add(this.volumeKnob);
			this.mainPanel.Controls.Add(this.opacityKnob);
			this.mainPanel.Controls.Add(this.chanLabel);
			this.mainPanel.Controls.Add(this.volLabel);
			this.mainPanel.Controls.Add(this.tagfmBtn);
			this.mainPanel.Controls.Add(this.prevBtn);
			this.mainPanel.Controls.Add(this.nextBtn);
			this.mainPanel.Controls.Add(this.plistBtn);
			this.mainPanel.Controls.Add(this.repLabel);
			this.mainPanel.Controls.Add(this.openBtn);
			this.mainPanel.Controls.Add(this.stopBtn);
			this.mainPanel.Controls.Add(this.pauseBtn);
			this.mainPanel.Controls.Add(this.playBtn);
			this.mainPanel.Controls.Add(this.closeBtn);
			this.mainPanel.Controls.Add(this.minBtn);
			this.mainPanel.Controls.Add(this.topSwitchBtn);
			this.mainPanel.Location = new System.Drawing.Point(0, 24);
			this.mainPanel.Name = "mainPanel";
			this.mainPanel.Size = new System.Drawing.Size(330, 140);
			this.mainPanel.TabIndex = 73;
			this.mainPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startDragging);
			this.mainPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMovementHandler);
			this.mainPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.endDragging);
			// 
			// kbpsLabel
			// 
			this.kbpsLabel.BackColor = System.Drawing.Color.Transparent;
			this.kbpsLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.kbpsLabel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.kbpsLabel.Location = new System.Drawing.Point(35, 60);
			this.kbpsLabel.Margin = new System.Windows.Forms.Padding(0);
			this.kbpsLabel.Name = "kbpsLabel";
			this.kbpsLabel.Size = new System.Drawing.Size(46, 24);
			this.kbpsLabel.TabIndex = 79;
			this.kbpsLabel.Text = "kb/s";
			this.kbpsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// khzLabel
			// 
			this.khzLabel.BackColor = System.Drawing.Color.Transparent;
			this.khzLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.khzLabel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.khzLabel.Location = new System.Drawing.Point(97, 60);
			this.khzLabel.Margin = new System.Windows.Forms.Padding(0);
			this.khzLabel.Name = "khzLabel";
			this.khzLabel.Size = new System.Drawing.Size(42, 24);
			this.khzLabel.TabIndex = 78;
			this.khzLabel.Text = "kHz";
			this.khzLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// chLabel
			// 
			this.chLabel.BackColor = System.Drawing.Color.Transparent;
			this.chLabel.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.chLabel.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
			this.chLabel.Location = new System.Drawing.Point(82, 40);
			this.chLabel.Name = "chLabel";
			this.chLabel.Size = new System.Drawing.Size(27, 24);
			this.chLabel.TabIndex = 77;
			this.chLabel.Text = "CH";
			this.chLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// visBtn
			// 
			this.visBtn.BackColor = System.Drawing.Color.Transparent;
			this.visBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.visBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.visBtn.ForeColor = System.Drawing.Color.Transparent;
			this.visBtn.Image = global::Azure.YAMP.Properties.Resources.visIcon;
			this.visBtn.Location = new System.Drawing.Point(287, 64);
			this.visBtn.Name = "visBtn";
			this.visBtn.Size = new System.Drawing.Size(24, 18);
			this.visBtn.TabIndex = 76;
			this.visBtn.UseVisualStyleBackColor = true;
			this.visBtn.Click += new System.EventHandler(this.visBtn_Click);
			// 
			// repeatCheckBox
			// 
			this.repeatCheckBox.AutoSize = true;
			this.repeatCheckBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(204)))), ((int)(((byte)(192)))));
			this.repeatCheckBox.Location = new System.Drawing.Point(198, 88);
			this.repeatCheckBox.Name = "repeatCheckBox";
			this.repeatCheckBox.Size = new System.Drawing.Size(76, 17);
			this.repeatCheckBox.TabIndex = 75;
			this.repeatCheckBox.Text = "Repeat off";
			this.repeatCheckBox.ThreeState = true;
			this.repeatCheckBox.UseVisualStyleBackColor = true;
			this.repeatCheckBox.CheckStateChanged += new System.EventHandler(this.repeatCheckBox_CheckStateChanged);
			// 
			// mainLabel
			// 
			this.mainLabel.BackColor = System.Drawing.Color.Transparent;
			this.mainLabel.Font = new System.Drawing.Font("Arial Narrow", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.mainLabel.ForeColor = System.Drawing.Color.Black;
			this.mainLabel.Location = new System.Drawing.Point(5, 16);
			this.mainLabel.Name = "mainLabel";
			this.mainLabel.Size = new System.Drawing.Size(321, 24);
			this.mainLabel.TabIndex = 61;
			this.mainLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			this.mainLabel.UseCompatibleTextRendering = true;
			// 
			// volumeKnob
			// 
			this.volumeKnob.BackColor = System.Drawing.Color.Transparent;
			this.volumeKnob.DrawRatio = 0.21F;
			this.volumeKnob.IndicatorColor = System.Drawing.Color.Gainsboro;
			this.volumeKnob.IndicatorOffset = 5F;
			this.volumeKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("volumeKnob.KnobCenter")));
			this.volumeKnob.KnobColor = System.Drawing.Color.Silver;
			this.volumeKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("volumeKnob.KnobRect")));
			this.volumeKnob.Location = new System.Drawing.Point(150, 43);
			this.volumeKnob.MaxValue = 1F;
			this.volumeKnob.MinValue = 0F;
			this.volumeKnob.Name = "volumeKnob";
			this.volumeKnob.Renderer = null;
			this.volumeKnob.ScaleColor = System.Drawing.Color.Gainsboro;
			this.volumeKnob.Size = new System.Drawing.Size(42, 42);
			this.volumeKnob.StepValue = 0.01F;
			this.volumeKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
			this.volumeKnob.TabIndex = 73;
			this.volumeKnob.Value = 0F;
			this.volumeKnob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.SetVolume);
			// 
			// opacityKnob
			// 
			this.opacityKnob.BackColor = System.Drawing.Color.Transparent;
			this.opacityKnob.DrawRatio = 0.17F;
			this.opacityKnob.IndicatorColor = System.Drawing.Color.Gainsboro;
			this.opacityKnob.IndicatorOffset = 5F;
			this.opacityKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("opacityKnob.KnobCenter")));
			this.opacityKnob.KnobColor = System.Drawing.Color.Silver;
			this.opacityKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("opacityKnob.KnobRect")));
			this.opacityKnob.Location = new System.Drawing.Point(201, 43);
			this.opacityKnob.MaxValue = 7F;
			this.opacityKnob.MinValue = 0F;
			this.opacityKnob.Name = "opacityKnob";
			this.opacityKnob.Renderer = null;
			this.opacityKnob.ScaleColor = System.Drawing.Color.Gainsboro;
			this.opacityKnob.Size = new System.Drawing.Size(34, 36);
			this.opacityKnob.StepValue = 0.1F;
			this.opacityKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
			this.opacityKnob.TabIndex = 74;
			this.opacityKnob.Value = 0F;
			this.opacityKnob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.SetOpacity);
			// 
			// tabControl1
			// 
			this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
			this.tabControl1.Controls.Add(this.infoPage);
			this.tabControl1.Controls.Add(this.eqPage);
			this.tabControl1.Controls.Add(this.outputPage);
			this.tabControl1.Location = new System.Drawing.Point(7, 191);
			this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
			this.tabControl1.Multiline = true;
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(319, 172);
			this.tabControl1.TabIndex = 74;
			// 
			// infoPage
			// 
			this.infoPage.BackColor = System.Drawing.Color.Transparent;
			this.infoPage.Controls.Add(this.infoPanel);
			this.infoPage.Location = new System.Drawing.Point(4, 4);
			this.infoPage.Margin = new System.Windows.Forms.Padding(0);
			this.infoPage.Name = "infoPage";
			this.infoPage.Size = new System.Drawing.Size(311, 146);
			this.infoPage.TabIndex = 0;
			this.infoPage.Text = "Now Playing";
			// 
			// infoPanel
			// 
			this.infoPanel.BackColor = System.Drawing.Color.Transparent;
			this.infoPanel.BackgroundImage = global::Azure.YAMP.Properties.Resources.purpleLCD;
			this.infoPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.infoPanel.Controls.Add(this.fileBox);
			this.infoPanel.Controls.Add(this.albumBox);
			this.infoPanel.Controls.Add(this.artistBox);
			this.infoPanel.Controls.Add(this.titleBox);
			this.infoPanel.Controls.Add(this.coverBox);
			this.infoPanel.Controls.Add(this.titIco);
			this.infoPanel.Controls.Add(this.albIco);
			this.infoPanel.Controls.Add(this.artIco);
			this.infoPanel.Controls.Add(this.fileLabel);
			this.infoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.infoPanel.Location = new System.Drawing.Point(0, 0);
			this.infoPanel.Margin = new System.Windows.Forms.Padding(0);
			this.infoPanel.Name = "infoPanel";
			this.infoPanel.Size = new System.Drawing.Size(311, 146);
			this.infoPanel.TabIndex = 75;
			this.infoPanel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startDragging);
			this.infoPanel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMovementHandler);
			this.infoPanel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.endDragging);
			// 
			// fileBox
			// 
			this.fileBox.BackColor = System.Drawing.Color.Transparent;
			this.fileBox.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.fileBox.ForeColor = System.Drawing.Color.DimGray;
			this.fileBox.Location = new System.Drawing.Point(35, 120);
			this.fileBox.Name = "fileBox";
			this.fileBox.Size = new System.Drawing.Size(270, 20);
			this.fileBox.TabIndex = 65;
			// 
			// albumBox
			// 
			this.albumBox.BackColor = System.Drawing.Color.Transparent;
			this.albumBox.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.albumBox.ForeColor = System.Drawing.Color.Gainsboro;
			this.albumBox.Location = new System.Drawing.Point(32, 94);
			this.albumBox.Name = "albumBox";
			this.albumBox.Size = new System.Drawing.Size(274, 20);
			this.albumBox.TabIndex = 64;
			// 
			// artistBox
			// 
			this.artistBox.BackColor = System.Drawing.Color.Transparent;
			this.artistBox.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.artistBox.ForeColor = System.Drawing.Color.Gainsboro;
			this.artistBox.Location = new System.Drawing.Point(32, 52);
			this.artistBox.Name = "artistBox";
			this.artistBox.Size = new System.Drawing.Size(180, 30);
			this.artistBox.TabIndex = 63;
			// 
			// titleBox
			// 
			this.titleBox.BackColor = System.Drawing.Color.Transparent;
			this.titleBox.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.titleBox.ForeColor = System.Drawing.Color.Gainsboro;
			this.titleBox.Location = new System.Drawing.Point(32, 10);
			this.titleBox.Name = "titleBox";
			this.titleBox.Size = new System.Drawing.Size(180, 30);
			this.titleBox.TabIndex = 62;
			// 
			// coverBox
			// 
			this.coverBox.BackColor = System.Drawing.Color.Transparent;
			this.coverBox.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("coverBox.BackgroundImage")));
			this.coverBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.coverBox.Location = new System.Drawing.Point(218, 8);
			this.coverBox.Name = "coverBox";
			this.coverBox.Size = new System.Drawing.Size(80, 80);
			this.coverBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.coverBox.TabIndex = 50;
			this.coverBox.TabStop = false;
			// 
			// titIco
			// 
			this.titIco.BackColor = System.Drawing.Color.Transparent;
			this.titIco.BackgroundImage = global::Azure.YAMP.Properties.Resources.TitleIcon1;
			this.titIco.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.titIco.Location = new System.Drawing.Point(7, 10);
			this.titIco.Name = "titIco";
			this.titIco.Size = new System.Drawing.Size(20, 20);
			this.titIco.TabIndex = 59;
			this.titIco.TabStop = false;
			// 
			// albIco
			// 
			this.albIco.BackColor = System.Drawing.Color.Transparent;
			this.albIco.BackgroundImage = global::Azure.YAMP.Properties.Resources.AlbumIcon1;
			this.albIco.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.albIco.Location = new System.Drawing.Point(8, 94);
			this.albIco.Name = "albIco";
			this.albIco.Size = new System.Drawing.Size(20, 20);
			this.albIco.TabIndex = 58;
			this.albIco.TabStop = false;
			// 
			// artIco
			// 
			this.artIco.BackColor = System.Drawing.Color.Transparent;
			this.artIco.BackgroundImage = global::Azure.YAMP.Properties.Resources.ArtistIcon1;
			this.artIco.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.artIco.Location = new System.Drawing.Point(8, 52);
			this.artIco.Name = "artIco";
			this.artIco.Size = new System.Drawing.Size(20, 20);
			this.artIco.TabIndex = 57;
			this.artIco.TabStop = false;
			// 
			// fileLabel
			// 
			this.fileLabel.BackColor = System.Drawing.Color.Transparent;
			this.fileLabel.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.fileLabel.ForeColor = System.Drawing.Color.Lime;
			this.fileLabel.Location = new System.Drawing.Point(11, 91);
			this.fileLabel.Name = "fileLabel";
			this.fileLabel.Size = new System.Drawing.Size(28, 13);
			this.fileLabel.TabIndex = 54;
			// 
			// eqPage
			// 
			this.eqPage.Controls.Add(this.eqPanel);
			this.eqPage.Location = new System.Drawing.Point(4, 4);
			this.eqPage.Margin = new System.Windows.Forms.Padding(0);
			this.eqPage.Name = "eqPage";
			this.eqPage.Size = new System.Drawing.Size(311, 146);
			this.eqPage.TabIndex = 1;
			this.eqPage.Text = "Equalizer";
			this.eqPage.UseVisualStyleBackColor = true;
			// 
			// eqPanel
			// 
			this.eqPanel.BackColor = System.Drawing.Color.Transparent;
			this.eqPanel.BackgroundImage = global::Azure.YAMP.Properties.Resources.brushedMetal1;
			this.eqPanel.Controls.Add(this.resetEqBtn);
			this.eqPanel.Controls.Add(this.freqLabel9);
			this.eqPanel.Controls.Add(this.freqLabel8);
			this.eqPanel.Controls.Add(this.freqLabel7);
			this.eqPanel.Controls.Add(this.freqLabel6);
			this.eqPanel.Controls.Add(this.freqLabel5);
			this.eqPanel.Controls.Add(this.freqLabel4);
			this.eqPanel.Controls.Add(this.freqLabel3);
			this.eqPanel.Controls.Add(this.freqLabel2);
			this.eqPanel.Controls.Add(this.freqLabel1);
			this.eqPanel.Controls.Add(this.freqLabel0);
			this.eqPanel.Controls.Add(this.freqBar9);
			this.eqPanel.Controls.Add(this.freqBar8);
			this.eqPanel.Controls.Add(this.freqBar7);
			this.eqPanel.Controls.Add(this.freqBar6);
			this.eqPanel.Controls.Add(this.freqBar5);
			this.eqPanel.Controls.Add(this.freqBar4);
			this.eqPanel.Controls.Add(this.freqBar3);
			this.eqPanel.Controls.Add(this.freqBar2);
			this.eqPanel.Controls.Add(this.freqBar1);
			this.eqPanel.Controls.Add(this.freqBar0);
			this.eqPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.eqPanel.Location = new System.Drawing.Point(0, 0);
			this.eqPanel.Name = "eqPanel";
			this.eqPanel.Size = new System.Drawing.Size(311, 146);
			this.eqPanel.TabIndex = 0;
			// 
			// resetEqBtn
			// 
			this.resetEqBtn.BackColor = System.Drawing.SystemColors.Control;
			this.resetEqBtn.Location = new System.Drawing.Point(2, 46);
			this.resetEqBtn.Name = "resetEqBtn";
			this.resetEqBtn.Size = new System.Drawing.Size(18, 23);
			this.resetEqBtn.TabIndex = 23;
			this.resetEqBtn.Text = "0";
			this.resetEqBtn.UseVisualStyleBackColor = false;
			this.resetEqBtn.Click += new System.EventHandler(this.button1_Click);
			// 
			// freqLabel9
			// 
			this.freqLabel9.AutoSize = true;
			this.freqLabel9.ForeColor = System.Drawing.Color.Gainsboro;
			this.freqLabel9.Location = new System.Drawing.Point(250, 115);
			this.freqLabel9.Name = "freqLabel9";
			this.freqLabel9.Size = new System.Drawing.Size(25, 13);
			this.freqLabel9.TabIndex = 21;
			this.freqLabel9.Text = "16k";
			this.freqLabel9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.freqLabel9.Click += new System.EventHandler(this.freqLabel_Click);
			// 
			// freqLabel8
			// 
			this.freqLabel8.AutoSize = true;
			this.freqLabel8.ForeColor = System.Drawing.Color.Gainsboro;
			this.freqLabel8.Location = new System.Drawing.Point(225, 115);
			this.freqLabel8.Name = "freqLabel8";
			this.freqLabel8.Size = new System.Drawing.Size(25, 13);
			this.freqLabel8.TabIndex = 20;
			this.freqLabel8.Text = "14k";
			this.freqLabel8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.freqLabel8.Click += new System.EventHandler(this.freqLabel_Click);
			// 
			// freqLabel7
			// 
			this.freqLabel7.AutoSize = true;
			this.freqLabel7.ForeColor = System.Drawing.Color.Gainsboro;
			this.freqLabel7.Location = new System.Drawing.Point(201, 115);
			this.freqLabel7.Name = "freqLabel7";
			this.freqLabel7.Size = new System.Drawing.Size(25, 13);
			this.freqLabel7.TabIndex = 19;
			this.freqLabel7.Text = "12k";
			this.freqLabel7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.freqLabel7.Click += new System.EventHandler(this.freqLabel_Click);
			// 
			// freqLabel6
			// 
			this.freqLabel6.AutoSize = true;
			this.freqLabel6.ForeColor = System.Drawing.Color.Gainsboro;
			this.freqLabel6.Location = new System.Drawing.Point(178, 115);
			this.freqLabel6.Name = "freqLabel6";
			this.freqLabel6.Size = new System.Drawing.Size(19, 13);
			this.freqLabel6.TabIndex = 18;
			this.freqLabel6.Text = "6k";
			this.freqLabel6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.freqLabel6.Click += new System.EventHandler(this.freqLabel_Click);
			// 
			// freqLabel5
			// 
			this.freqLabel5.AutoSize = true;
			this.freqLabel5.ForeColor = System.Drawing.Color.Gainsboro;
			this.freqLabel5.Location = new System.Drawing.Point(148, 115);
			this.freqLabel5.Name = "freqLabel5";
			this.freqLabel5.Size = new System.Drawing.Size(19, 13);
			this.freqLabel5.TabIndex = 17;
			this.freqLabel5.Text = "3k";
			this.freqLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.freqLabel5.Click += new System.EventHandler(this.freqLabel_Click);
			// 
			// freqLabel4
			// 
			this.freqLabel4.AutoSize = true;
			this.freqLabel4.ForeColor = System.Drawing.Color.Gainsboro;
			this.freqLabel4.Location = new System.Drawing.Point(121, 115);
			this.freqLabel4.Name = "freqLabel4";
			this.freqLabel4.Size = new System.Drawing.Size(19, 13);
			this.freqLabel4.TabIndex = 16;
			this.freqLabel4.Text = "1k";
			this.freqLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.freqLabel4.Click += new System.EventHandler(this.freqLabel_Click);
			// 
			// freqLabel3
			// 
			this.freqLabel3.AutoSize = true;
			this.freqLabel3.ForeColor = System.Drawing.Color.Gainsboro;
			this.freqLabel3.Location = new System.Drawing.Point(95, 115);
			this.freqLabel3.Name = "freqLabel3";
			this.freqLabel3.Size = new System.Drawing.Size(25, 13);
			this.freqLabel3.TabIndex = 15;
			this.freqLabel3.Text = "600";
			this.freqLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.freqLabel3.Click += new System.EventHandler(this.freqLabel_Click);
			// 
			// freqLabel2
			// 
			this.freqLabel2.AutoSize = true;
			this.freqLabel2.ForeColor = System.Drawing.Color.Gainsboro;
			this.freqLabel2.Location = new System.Drawing.Point(69, 115);
			this.freqLabel2.Name = "freqLabel2";
			this.freqLabel2.Size = new System.Drawing.Size(25, 13);
			this.freqLabel2.TabIndex = 14;
			this.freqLabel2.Text = "310";
			this.freqLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.freqLabel2.Click += new System.EventHandler(this.freqLabel_Click);
			// 
			// freqLabel1
			// 
			this.freqLabel1.AutoSize = true;
			this.freqLabel1.ForeColor = System.Drawing.Color.Gainsboro;
			this.freqLabel1.Location = new System.Drawing.Point(43, 115);
			this.freqLabel1.Name = "freqLabel1";
			this.freqLabel1.Size = new System.Drawing.Size(25, 13);
			this.freqLabel1.TabIndex = 13;
			this.freqLabel1.Text = "170";
			this.freqLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.freqLabel1.Click += new System.EventHandler(this.freqLabel_Click);
			// 
			// freqLabel0
			// 
			this.freqLabel0.AutoSize = true;
			this.freqLabel0.ForeColor = System.Drawing.Color.Gainsboro;
			this.freqLabel0.Location = new System.Drawing.Point(23, 115);
			this.freqLabel0.Name = "freqLabel0";
			this.freqLabel0.Size = new System.Drawing.Size(19, 13);
			this.freqLabel0.TabIndex = 12;
			this.freqLabel0.Text = "60";
			this.freqLabel0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.freqLabel0.Click += new System.EventHandler(this.freqLabel_Click);
			// 
			// freqBar9
			// 
			this.freqBar9.AutoSize = false;
			this.freqBar9.Location = new System.Drawing.Point(256, 3);
			this.freqBar9.Maximum = 6;
			this.freqBar9.Minimum = -6;
			this.freqBar9.Name = "freqBar9";
			this.freqBar9.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.freqBar9.Size = new System.Drawing.Size(20, 109);
			this.freqBar9.TabIndex = 10;
			this.freqBar9.TickStyle = System.Windows.Forms.TickStyle.None;
			this.freqBar9.ValueChanged += new System.EventHandler(this.freqBar_ValueChanged);
			// 
			// freqBar8
			// 
			this.freqBar8.AutoSize = false;
			this.freqBar8.Location = new System.Drawing.Point(230, 3);
			this.freqBar8.Maximum = 6;
			this.freqBar8.Minimum = -6;
			this.freqBar8.Name = "freqBar8";
			this.freqBar8.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.freqBar8.Size = new System.Drawing.Size(20, 109);
			this.freqBar8.TabIndex = 9;
			this.freqBar8.TickStyle = System.Windows.Forms.TickStyle.None;
			this.freqBar8.ValueChanged += new System.EventHandler(this.freqBar_ValueChanged);
			// 
			// freqBar7
			// 
			this.freqBar7.AutoSize = false;
			this.freqBar7.Location = new System.Drawing.Point(204, 3);
			this.freqBar7.Maximum = 6;
			this.freqBar7.Minimum = -6;
			this.freqBar7.Name = "freqBar7";
			this.freqBar7.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.freqBar7.Size = new System.Drawing.Size(20, 109);
			this.freqBar7.TabIndex = 8;
			this.freqBar7.TickStyle = System.Windows.Forms.TickStyle.None;
			this.freqBar7.ValueChanged += new System.EventHandler(this.freqBar_ValueChanged);
			// 
			// freqBar6
			// 
			this.freqBar6.AutoSize = false;
			this.freqBar6.Location = new System.Drawing.Point(178, 3);
			this.freqBar6.Maximum = 6;
			this.freqBar6.Minimum = -6;
			this.freqBar6.Name = "freqBar6";
			this.freqBar6.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.freqBar6.Size = new System.Drawing.Size(20, 109);
			this.freqBar6.TabIndex = 7;
			this.freqBar6.TickStyle = System.Windows.Forms.TickStyle.None;
			this.freqBar6.ValueChanged += new System.EventHandler(this.freqBar_ValueChanged);
			// 
			// freqBar5
			// 
			this.freqBar5.AutoSize = false;
			this.freqBar5.Location = new System.Drawing.Point(152, 3);
			this.freqBar5.Maximum = 6;
			this.freqBar5.Minimum = -6;
			this.freqBar5.Name = "freqBar5";
			this.freqBar5.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.freqBar5.Size = new System.Drawing.Size(20, 109);
			this.freqBar5.TabIndex = 6;
			this.freqBar5.TickStyle = System.Windows.Forms.TickStyle.None;
			this.freqBar5.ValueChanged += new System.EventHandler(this.freqBar_ValueChanged);
			// 
			// freqBar4
			// 
			this.freqBar4.AutoSize = false;
			this.freqBar4.Location = new System.Drawing.Point(126, 3);
			this.freqBar4.Maximum = 6;
			this.freqBar4.Minimum = -6;
			this.freqBar4.Name = "freqBar4";
			this.freqBar4.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.freqBar4.Size = new System.Drawing.Size(20, 109);
			this.freqBar4.TabIndex = 5;
			this.freqBar4.TickStyle = System.Windows.Forms.TickStyle.None;
			this.freqBar4.ValueChanged += new System.EventHandler(this.freqBar_ValueChanged);
			// 
			// freqBar3
			// 
			this.freqBar3.AutoSize = false;
			this.freqBar3.Location = new System.Drawing.Point(100, 3);
			this.freqBar3.Maximum = 6;
			this.freqBar3.Minimum = -6;
			this.freqBar3.Name = "freqBar3";
			this.freqBar3.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.freqBar3.Size = new System.Drawing.Size(20, 109);
			this.freqBar3.TabIndex = 4;
			this.freqBar3.TickStyle = System.Windows.Forms.TickStyle.None;
			this.freqBar3.ValueChanged += new System.EventHandler(this.freqBar_ValueChanged);
			// 
			// freqBar2
			// 
			this.freqBar2.AutoSize = false;
			this.freqBar2.Location = new System.Drawing.Point(74, 3);
			this.freqBar2.Maximum = 6;
			this.freqBar2.Minimum = -6;
			this.freqBar2.Name = "freqBar2";
			this.freqBar2.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.freqBar2.Size = new System.Drawing.Size(20, 109);
			this.freqBar2.TabIndex = 3;
			this.freqBar2.TickStyle = System.Windows.Forms.TickStyle.None;
			this.freqBar2.ValueChanged += new System.EventHandler(this.freqBar_ValueChanged);
			// 
			// freqBar1
			// 
			this.freqBar1.AutoSize = false;
			this.freqBar1.Location = new System.Drawing.Point(48, 3);
			this.freqBar1.Maximum = 6;
			this.freqBar1.Minimum = -6;
			this.freqBar1.Name = "freqBar1";
			this.freqBar1.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.freqBar1.Size = new System.Drawing.Size(20, 109);
			this.freqBar1.TabIndex = 2;
			this.freqBar1.TickStyle = System.Windows.Forms.TickStyle.None;
			this.freqBar1.ValueChanged += new System.EventHandler(this.freqBar_ValueChanged);
			// 
			// freqBar0
			// 
			this.freqBar0.AutoSize = false;
			this.freqBar0.Location = new System.Drawing.Point(22, 3);
			this.freqBar0.Maximum = 6;
			this.freqBar0.Minimum = -6;
			this.freqBar0.Name = "freqBar0";
			this.freqBar0.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.freqBar0.Size = new System.Drawing.Size(20, 109);
			this.freqBar0.TabIndex = 1;
			this.freqBar0.TickStyle = System.Windows.Forms.TickStyle.None;
			this.freqBar0.ValueChanged += new System.EventHandler(this.freqBar_ValueChanged);
			// 
			// outputPage
			// 
			this.outputPage.Controls.Add(this.outputPanel);
			this.outputPage.Location = new System.Drawing.Point(4, 4);
			this.outputPage.Margin = new System.Windows.Forms.Padding(0);
			this.outputPage.Name = "outputPage";
			this.outputPage.Size = new System.Drawing.Size(311, 146);
			this.outputPage.TabIndex = 2;
			this.outputPage.Text = "Output";
			this.outputPage.UseVisualStyleBackColor = true;
			// 
			// outputPanel
			// 
			this.outputPanel.BackColor = System.Drawing.Color.Black;
			this.outputPanel.BackgroundImage = global::Azure.YAMP.Properties.Resources.brushedMetal1;
			this.outputPanel.Controls.Add(this.refreshOutBtn);
			this.outputPanel.Controls.Add(this.switchOutBtn);
			this.outputPanel.Controls.Add(this.currDeviceBox);
			this.outputPanel.Controls.Add(this.currDeviceLabel);
			this.outputPanel.Controls.Add(this.outputCombo);
			this.outputPanel.Controls.Add(this.outputLabel);
			this.outputPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outputPanel.Location = new System.Drawing.Point(0, 0);
			this.outputPanel.Name = "outputPanel";
			this.outputPanel.Size = new System.Drawing.Size(311, 146);
			this.outputPanel.TabIndex = 0;
			// 
			// refreshOutBtn
			// 
			this.refreshOutBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.refreshOutBtn.ForeColor = System.Drawing.Color.White;
			this.refreshOutBtn.Location = new System.Drawing.Point(191, 58);
			this.refreshOutBtn.Name = "refreshOutBtn";
			this.refreshOutBtn.Size = new System.Drawing.Size(117, 23);
			this.refreshOutBtn.TabIndex = 6;
			this.refreshOutBtn.Text = "Refresh device list";
			this.refreshOutBtn.UseVisualStyleBackColor = true;
			this.refreshOutBtn.Click += new System.EventHandler(this.refreshOutBtn_Click);
			// 
			// switchOutBtn
			// 
			this.switchOutBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.switchOutBtn.ForeColor = System.Drawing.Color.White;
			this.switchOutBtn.Location = new System.Drawing.Point(6, 58);
			this.switchOutBtn.Name = "switchOutBtn";
			this.switchOutBtn.Size = new System.Drawing.Size(75, 23);
			this.switchOutBtn.TabIndex = 5;
			this.switchOutBtn.Text = "Switch";
			this.switchOutBtn.UseVisualStyleBackColor = true;
			this.switchOutBtn.Click += new System.EventHandler(this.SwitchOutBtn_Click);
			// 
			// currDeviceBox
			// 
			this.currDeviceBox.BackColor = System.Drawing.Color.Black;
			this.currDeviceBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.currDeviceBox.ForeColor = System.Drawing.Color.White;
			this.currDeviceBox.Location = new System.Drawing.Point(86, 4);
			this.currDeviceBox.Name = "currDeviceBox";
			this.currDeviceBox.ReadOnly = true;
			this.currDeviceBox.Size = new System.Drawing.Size(222, 20);
			this.currDeviceBox.TabIndex = 4;
			// 
			// currDeviceLabel
			// 
			this.currDeviceLabel.AutoSize = true;
			this.currDeviceLabel.ForeColor = System.Drawing.Color.White;
			this.currDeviceLabel.Location = new System.Drawing.Point(3, 7);
			this.currDeviceLabel.Name = "currDeviceLabel";
			this.currDeviceLabel.Size = new System.Drawing.Size(76, 13);
			this.currDeviceLabel.TabIndex = 3;
			this.currDeviceLabel.Text = "Current device";
			// 
			// outputCombo
			// 
			this.outputCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.outputCombo.FormattingEnabled = true;
			this.outputCombo.Location = new System.Drawing.Point(86, 30);
			this.outputCombo.Name = "outputCombo";
			this.outputCombo.Size = new System.Drawing.Size(222, 21);
			this.outputCombo.TabIndex = 2;
			// 
			// outputLabel
			// 
			this.outputLabel.AutoSize = true;
			this.outputLabel.ForeColor = System.Drawing.Color.White;
			this.outputLabel.Location = new System.Drawing.Point(3, 33);
			this.outputLabel.Name = "outputLabel";
			this.outputLabel.Size = new System.Drawing.Size(79, 13);
			this.outputLabel.TabIndex = 0;
			this.outputLabel.Text = "Output devices";
			// 
			// mainMenuStrip
			// 
			this.mainMenuStrip.Appearance = this.appearanceControl1;
			this.mainMenuStrip.BackgroundImage = global::Azure.YAMP.Properties.Resources.gradientMenuStripBack;
			this.mainMenuStrip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToListMenuItem,
            this.removeFromListMenuItem,
            this.listMenuItem,
            this.pinMenuItem,
            this.searchBox});
			this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			this.mainMenuStrip.Name = "mainMenuStrip";
			this.mainMenuStrip.Padding = new System.Windows.Forms.Padding(0);
			this.mainMenuStrip.Size = new System.Drawing.Size(704, 24);
			this.mainMenuStrip.TabIndex = 67;
			// 
			// addToListMenuItem
			// 
			this.addToListMenuItem.BackColor = System.Drawing.Color.Transparent;
			this.addToListMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.addToListMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.folderToolStripMenuItem});
			this.addToListMenuItem.ForeColor = System.Drawing.Color.Black;
			this.addToListMenuItem.Name = "addToListMenuItem";
			this.addToListMenuItem.Size = new System.Drawing.Size(41, 24);
			this.addToListMenuItem.Text = "Add";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.fileToolStripMenuItem.Text = "File";
			this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
			// 
			// folderToolStripMenuItem
			// 
			this.folderToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allSubFoldersToolStripMenuItem,
            this.selectedOnlyToolStripMenuItem});
			this.folderToolStripMenuItem.Name = "folderToolStripMenuItem";
			this.folderToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
			this.folderToolStripMenuItem.Text = "Folder";
			// 
			// allSubFoldersToolStripMenuItem
			// 
			this.allSubFoldersToolStripMenuItem.Name = "allSubFoldersToolStripMenuItem";
			this.allSubFoldersToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.allSubFoldersToolStripMenuItem.Text = "All subfolders";
			this.allSubFoldersToolStripMenuItem.Click += new System.EventHandler(this.allSubFoldersToolStripMenuItem_Click);
			// 
			// selectedOnlyToolStripMenuItem
			// 
			this.selectedOnlyToolStripMenuItem.Name = "selectedOnlyToolStripMenuItem";
			this.selectedOnlyToolStripMenuItem.Size = new System.Drawing.Size(146, 22);
			this.selectedOnlyToolStripMenuItem.Text = "Selected only";
			this.selectedOnlyToolStripMenuItem.Click += new System.EventHandler(this.selectedOnlyToolStripMenuItem_Click_1);
			// 
			// removeFromListMenuItem
			// 
			this.removeFromListMenuItem.BackColor = System.Drawing.Color.Transparent;
			this.removeFromListMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedToolStripMenuItem,
            this.allToolStripMenuItem});
			this.removeFromListMenuItem.ForeColor = System.Drawing.Color.Black;
			this.removeFromListMenuItem.Name = "removeFromListMenuItem";
			this.removeFromListMenuItem.Size = new System.Drawing.Size(62, 24);
			this.removeFromListMenuItem.Text = "Remove";
			// 
			// selectedToolStripMenuItem
			// 
			this.selectedToolStripMenuItem.Name = "selectedToolStripMenuItem";
			this.selectedToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.selectedToolStripMenuItem.Text = "Selected";
			this.selectedToolStripMenuItem.Click += new System.EventHandler(this.selectedToolStripMenuItem_Click);
			// 
			// allToolStripMenuItem
			// 
			this.allToolStripMenuItem.Name = "allToolStripMenuItem";
			this.allToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
			this.allToolStripMenuItem.Text = "All";
			this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
			// 
			// listMenuItem
			// 
			this.listMenuItem.BackColor = System.Drawing.Color.Transparent;
			this.listMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadToolStripMenuItem1,
            this.saveToolStripMenuItem1});
			this.listMenuItem.ForeColor = System.Drawing.Color.Black;
			this.listMenuItem.Name = "listMenuItem";
			this.listMenuItem.Size = new System.Drawing.Size(37, 24);
			this.listMenuItem.Text = "List";
			// 
			// loadToolStripMenuItem1
			// 
			this.loadToolStripMenuItem1.Name = "loadToolStripMenuItem1";
			this.loadToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
			this.loadToolStripMenuItem1.Text = "Load";
			this.loadToolStripMenuItem1.Click += new System.EventHandler(this.loadToolStripMenuItem1_Click);
			// 
			// saveToolStripMenuItem1
			// 
			this.saveToolStripMenuItem1.Name = "saveToolStripMenuItem1";
			this.saveToolStripMenuItem1.Size = new System.Drawing.Size(100, 22);
			this.saveToolStripMenuItem1.Text = "Save";
			this.saveToolStripMenuItem1.Click += new System.EventHandler(this.saveToolStripMenuItem1_Click);
			// 
			// pinMenuItem
			// 
			this.pinMenuItem.BackColor = System.Drawing.Color.Transparent;
			this.pinMenuItem.CheckOnClick = true;
			this.pinMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.pinMenuItem.Image = global::Azure.YAMP.Properties.Resources.pinOff;
			this.pinMenuItem.ImageTransparentColor = System.Drawing.Color.Transparent;
			this.pinMenuItem.Margin = new System.Windows.Forms.Padding(164, 0, 0, 0);
			this.pinMenuItem.Name = "pinMenuItem";
			this.pinMenuItem.ShowShortcutKeys = false;
			this.pinMenuItem.Size = new System.Drawing.Size(28, 24);
			this.pinMenuItem.CheckedChanged += new System.EventHandler(this.pinMenuItem_CheckedChanged);
			// 
			// searchBox
			// 
			this.searchBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
			this.searchBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
			this.searchBox.AutoToolTip = true;
			this.searchBox.BackColor = System.Drawing.Color.Black;
			this.searchBox.Font = new System.Drawing.Font("Tahoma", 8F);
			this.searchBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(204)))), ((int)(((byte)(192)))));
			this.searchBox.Name = "searchBox";
			this.searchBox.Size = new System.Drawing.Size(250, 24);
			this.searchBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.searchBox_KeyDown);
			this.searchBox.Click += new System.EventHandler(this.searchBox_Click);
			this.searchBox.MouseEnter += new System.EventHandler(this.searchBox_MouseEnter);
			this.searchBox.MouseLeave += new System.EventHandler(this.searchBox_MouseLeave);
			this.searchBox.TextChanged += new System.EventHandler(this.searchBox_TextChanged);
			// 
			// seekBar
			// 
			this.seekBar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(119)))), ((int)(((byte)(134)))));
			this.seekBar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.seekBar.BarColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
			this.seekBar.BorderColor = System.Drawing.Color.Black;
			this.seekBar.FillStyle = Azure.LibCollection.CS.ColorProgressBar.FillStyles.Solid;
			this.seekBar.ForeColor = System.Drawing.Color.GreenYellow;
			this.seekBar.Location = new System.Drawing.Point(5, 164);
			this.seekBar.Margin = new System.Windows.Forms.Padding(4);
			this.seekBar.Maximum = 100;
			this.seekBar.Minimum = 0;
			this.seekBar.Name = "seekBar";
			this.seekBar.Size = new System.Drawing.Size(321, 20);
			this.seekBar.Step = 10;
			this.seekBar.TabIndex = 48;
			this.seekBar.Value = 0;
			this.seekBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.StartSeeking);
			this.seekBar.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMovementHandler);
			this.seekBar.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EndSeeking);
			// 
			// playListView
			// 
			this.playListView.AllowDrop = true;
			this.playListView.AllowItemDrag = true;
			this.playListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.playListView.AutoArrange = false;
			this.playListView.BackColor = System.Drawing.Color.Black;
			this.playListView.BackgroundImage = global::Azure.YAMP.Properties.Resources.brushedMetal1;
			this.playListView.BackgroundImageTiled = true;
			this.playListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.artistHeader,
            this.titleHeader,
            this.durationHeader});
			this.playListView.Controls.Add(this.loadingLabel);
			this.playListView.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.playListView.ForeColor = System.Drawing.Color.PaleTurquoise;
			this.playListView.FullRowSelect = true;
			this.playListView.HideSelection = false;
			this.playListView.InsertionLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
			this.playListView.Location = new System.Drawing.Point(332, 30);
			this.playListView.MinimumSize = new System.Drawing.Size(300, 330);
			this.playListView.Name = "playListView";
			this.playListView.ShowGroups = false;
			this.playListView.Size = new System.Drawing.Size(366, 383);
			this.playListView.TabIndex = 70;
			this.playListView.UseCompatibleStateImageBehavior = false;
			this.playListView.View = System.Windows.Forms.View.Details;
			this.playListView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.playList_KeyDown);
			// 
			// titleHeader
			// 
			this.titleHeader.Text = "Title";
			this.titleHeader.Width = 180;
			// 
			// artistHeader
			// 
			this.artistHeader.Text = "Artist";
			this.artistHeader.Width = 100;
			// 
			// durationHeader
			// 
			this.durationHeader.Text = "Duration";
			this.durationHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// loadingLabel
			// 
			this.loadingLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.loadingLabel.BackColor = System.Drawing.Color.Black;
			this.loadingLabel.Controls.Add(this.playlistLoadingBar);
			this.loadingLabel.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
			this.loadingLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
			this.loadingLabel.Location = new System.Drawing.Point(0, 24);
			this.loadingLabel.Name = "loadingLabel";
			this.loadingLabel.Size = new System.Drawing.Size(362, 357);
			this.loadingLabel.TabIndex = 75;
			this.loadingLabel.Text = "Building playlist...\r\nPlease wait.\r\n";
			this.loadingLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.loadingLabel.Visible = false;
			// 
			// playlistLoadingBar
			// 
			this.playlistLoadingBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.playlistLoadingBar.Location = new System.Drawing.Point(25, 200);
			this.playlistLoadingBar.Name = "playlistLoadingBar";
			this.playlistLoadingBar.Size = new System.Drawing.Size(288, 27);
			this.playlistLoadingBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.playlistLoadingBar.TabIndex = 75;
			this.playlistLoadingBar.Visible = false;
			// 
			// PlayerUI3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Black;
			this.BackgroundImage = global::Azure.YAMP.Properties.Resources.background;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(704, 441);
			this.Controls.Add(this.mainMenuStrip);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.seekBar);
			this.Controls.Add(this.mainPanel);
			this.Controls.Add(this.playListView);
			this.DoubleBuffered = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.mainMenuStrip;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(1080, 720);
			this.MinimumSize = new System.Drawing.Size(720, 480);
			this.Name = "PlayerUI3";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.Text = "YAMP";
			this.TransparencyKey = System.Drawing.Color.Magenta;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShutDown);
			this.Shown += new System.EventHandler(this.MainWindow_Shown);
			this.DragEnter += new System.Windows.Forms.DragEventHandler(this.playList_DragEnter);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.playList_KeyDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMovementHandler);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.visualBox)).EndInit();
			this.mainPanel.ResumeLayout(false);
			this.mainPanel.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.infoPage.ResumeLayout(false);
			this.infoPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.coverBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.titIco)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.albIco)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.artIco)).EndInit();
			this.eqPage.ResumeLayout(false);
			this.eqPanel.ResumeLayout(false);
			this.eqPanel.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.freqBar9)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.freqBar0)).EndInit();
			this.outputPage.ResumeLayout(false);
			this.outputPanel.ResumeLayout(false);
			this.outputPanel.PerformLayout();
			this.mainMenuStrip.ResumeLayout(false);
			this.mainMenuStrip.PerformLayout();
			this.playListView.ResumeLayout(false);
			this.loadingLabel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.OpenFileDialog openMedia;
        private System.Windows.Forms.Timer mainTimer;
        private Azure.LibCollection.CS.ColorProgressBar seekBar;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem listMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeFromListMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToListMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem folderToolStripMenuItem;
        private Azure.LibCollection.CS.ListViewEx playListView;
        private System.Windows.Forms.ColumnHeader titleHeader;
        private System.Windows.Forms.ColumnHeader durationHeader;
        private Azure.LibCollection.VB.CustomizableStatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel hintLabel;
        private Azure.LibCollection.CS.MarqueeLabel mainLabel;
        private System.Windows.Forms.Button topSwitchBtn;
        private System.Windows.Forms.Button minBtn;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Button playBtn;
        private System.Windows.Forms.Button pauseBtn;
        private System.Windows.Forms.Button stopBtn;
        private System.Windows.Forms.Button openBtn;
        private System.Windows.Forms.Label repLabel;
        private System.Windows.Forms.Button plistBtn;
        private System.Windows.Forms.Button nextBtn;
        private System.Windows.Forms.Button prevBtn;
        private System.Windows.Forms.Button tagfmBtn;
        private System.Windows.Forms.Label volLabel;
        private System.Windows.Forms.Label chanLabel;
        private System.Windows.Forms.Label sampleRateLabel;
        private System.Windows.Forms.Label bitRateLabel;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob opacityKnob;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob volumeKnob;
        private System.Windows.Forms.PictureBox visualBox;
        private System.Windows.Forms.Label displayTimeLabel;
        private System.Windows.Forms.Panel mainPanel;
        private System.Windows.Forms.CheckBox repeatCheckBox;
        private System.Windows.Forms.ToolStripMenuItem pinMenuItem;
        private Azure.LibCollection.VB.CustomizableMenuStrip mainMenuStrip;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage infoPage;
        private System.Windows.Forms.Panel infoPanel;
        private System.Windows.Forms.Label fileBox;
        private System.Windows.Forms.Label albumBox;
        private System.Windows.Forms.Label artistBox;
        private System.Windows.Forms.Label titleBox;
        private System.Windows.Forms.PictureBox coverBox;
        private System.Windows.Forms.PictureBox titIco;
        private System.Windows.Forms.PictureBox albIco;
        private System.Windows.Forms.PictureBox artIco;
        private System.Windows.Forms.Label fileLabel;
        private System.Windows.Forms.TabPage eqPage;
        private System.Windows.Forms.Panel eqPanel;
        private System.Windows.Forms.Label freqLabel9;
        private System.Windows.Forms.Label freqLabel8;
        private System.Windows.Forms.Label freqLabel7;
        private System.Windows.Forms.Label freqLabel6;
        private System.Windows.Forms.Label freqLabel5;
        private System.Windows.Forms.Label freqLabel4;
        private System.Windows.Forms.Label freqLabel3;
        private System.Windows.Forms.Label freqLabel2;
        private System.Windows.Forms.Label freqLabel1;
        private System.Windows.Forms.Label freqLabel0;
        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar freqBar9;
        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar freqBar8;
        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar freqBar7;
        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar freqBar6;
        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar freqBar5;
        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar freqBar4;
        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar freqBar3;
        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar freqBar2;
        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar freqBar1;
        private Fusionbird.FusionToolkit.FusionTrackBar.FusionTrackBar freqBar0;
        private System.Windows.Forms.Button resetEqBtn;
        private Azure.LibCollection.VB.AppearanceControl appearanceControl1;
        private System.Windows.Forms.Button visBtn;
        private System.Windows.Forms.TabPage outputPage;
        private System.Windows.Forms.Panel outputPanel;
        private System.Windows.Forms.TextBox currDeviceBox;
        private System.Windows.Forms.Label currDeviceLabel;
        private System.Windows.Forms.ComboBox outputCombo;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.Button switchOutBtn;
        private System.Windows.Forms.Button refreshOutBtn;
        private System.Windows.Forms.Label kbpsLabel;
        private System.Windows.Forms.Label khzLabel;
        private System.Windows.Forms.Label chLabel;
        private System.Windows.Forms.ToolStripMenuItem allSubFoldersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectedOnlyToolStripMenuItem;
        public System.Windows.Forms.Label loadingLabel;
        public Azure.LibCollection.CS.Controls.ToolStripSpringTextBox searchBox;
		public System.Windows.Forms.ProgressBar playlistLoadingBar;
		private System.Windows.Forms.ColumnHeader artistHeader;
	}
}