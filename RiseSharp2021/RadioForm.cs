using Azure.LibCollection.CS;
using Azure.MediaUtils;
using LedMatrixControlNamespace;
using SimpleWR2.Classes;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleWR2
{
    public class RadioForm : Form
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
                    IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);
        private PrivateFontCollection fonts = new PrivateFontCollection();

        private IContainer components = null;

        private channels_type _channels;
        private channel_type _currentCh;
        private bool dragging;
        private Point offset;
        private Button btnPlay;
        private PictureBox fxBox;
        private Button btnStop;
        private CheckBox checkRec;
        private System.Windows.Forms.Timer timerTag;
        private LedMatrixControl statusLabel;
        private System.Windows.Forms.Timer timerLevels;
        private Panel panel2;
        private Button closeBtn;
        private Button minimiseBtn;
        private PictureBox pictureBox1;
        private GroupBox channelChooserBox;
        private PictureBox pb_Deep;
        private PictureBox pb_House;
        private PictureBox pb_Classic;
        private RadioButton rb_Deep;
        private RadioButton rb_HitHouse;
        private RadioButton rb_Classic;
        private LedMatrixControl infoLabel;
        private LBSoft.IndustrialCtrls.Knobs.LBKnob volKnob;
        private BigInteger lmiStatusIndex = LedMatrixControl.DefaultItemId;
        private bool IsOnline = false;
        private BigInteger lmiInfoIndex = LedMatrixControl.DefaultItemId;
        private const int ledMatrixRows = 10;
        private readonly Point statusLabelPos = new Point(0, 0);
        private PictureBox pb_RnB;
        private RadioButton rb_RnB;
        private Button btnSwitchOutput;
        private readonly Point infoLabelPos = new Point(1, 1);
        private AudioOutputDialog aod;
        private bool volumeChanging = false;
        private readonly ManualResetEvent resetEvent = new ManualResetEvent(false);
        private History m_History;
        private bool channelChanged = false;
        private const string ch_Classic = "Rise FM Classic";
        private const string ch_HitHouse = "Rise FM Hit House";
        private const string ch_Deep = "Rise FM Deep House";
        private const string ch_RnB = "Rise FM RnB";
        private AudioEngine ae;

        public RadioForm()
        {
            this.InitializeComponent();
            //LoadDotFont();

            string[] libPaths = new string[] { string.Format("{0}\\AudioLib_{1}", Program.AssemblyDirectory, Utils.ProcessorArchitecture),
                string.Format("{0}\\AudioLib_{1}", Utils.AssemblyDirectory, Utils.ProcessorArchitecture)};
            bool initSuccess = false;
            string initErrorMsg = string.Empty;

            for (int i = 0; i < libPaths.Length && !initSuccess; i++)
            {
                try
                {
                    ae = new AudioEngine(-1, libPaths[1], this.Handle, true, libPaths[1]);
                    initSuccess = true;
                }
                catch (Exception x)
                {
                    initErrorMsg = x.Message;
                }
            }

            if (!initSuccess)
            {
                MBoxHelper.ShowErrorMsg(initErrorMsg, "BASS Init Error!");
                System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();
                proc.Kill();
            }
            else
            {
                aod = new AudioOutputDialog(ref ae);
                XmlSerializer xs = new XmlSerializer(typeof(SimpleWR2.channels_type));
                XmlReader xr = XmlReader.Create(string.Format("{0}\\Channels.xlist", Program.AssemblyDirectory));
                _channels = (channels_type)xs.Deserialize(xr);
                Point locationMainWindow = SimpleWR2.Properties.Settings.Default.locationMainWindow;
                int x = locationMainWindow.X;
                locationMainWindow = SimpleWR2.Properties.Settings.Default.locationMainWindow;
                base.Location = new Point(x, locationMainWindow.Y);
                this.volKnob.Value = SimpleWR2.Properties.Settings.Default.volumeLevel;
                this.SetVolume(this.volKnob.Value, this.volKnob.MaxValue);

                ae.Visuals = new Visualizer(ref this.fxBox);
                ae.Visuals.SetVisual(SimpleWR2.Properties.Settings.Default.visualType);
                ae.Visuals.Set("drawFull", false);

                this.loadFx();
                ResetLabels();
                this.m_History = new History(System.IO.Path.Combine(Program.AssemblyDirectory, "History"));
                string startChannel = SimpleWR2.Properties.Settings.Default.channel;
                SelectChannelRadioButton(string.IsNullOrWhiteSpace(startChannel) ? _channels.channels[0].name : startChannel);
                if (_currentCh == null)
                {
                    _currentCh = _channels.channels.Where(c => c.name == ch_Classic).FirstOrDefault();
                }
            }
        }

        private void ResetLabels()
        {
            if (!this.statusLabel.HasTextItem)
            {
                this.lmiStatusIndex = this.statusLabel.AddTextItem(string.Empty, statusLabelPos, ItemDirection.Left, ItemSpeed.Slow);
            }
            this.SetStatusOnline(false, false);

            if (!this.infoLabel.HasTextItem)
            {
                this.lmiInfoIndex = this.infoLabel.AddTextItem(string.Empty, infoLabelPos, ItemDirection.Left, ItemSpeed.Idle);
            }
            this.infoLabel.SetItemText(this.lmiInfoIndex, string.Empty);

        }

        private void BtnPlayClick(object sender, EventArgs e)
        {
            ae.InitOnlineStream(_currentCh.url, this.checkRec.Checked);
            this.checkRec.Enabled = false;
            if (this.checkRec.Checked)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                try
                {
                    saveFileDialog.Filter = "All Supported|*.mp3;*.aac";
                    if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ae.SetOutput(saveFileDialog.FileName);
                    }
                }
                finally
                {
                    if (saveFileDialog != null)
                    {
                        ((IDisposable)saveFileDialog).Dispose();
                    }
                }
            }
            if (ae.Play())
            {
                if (this.channelChanged)
                {
                    this.m_History.ChangeChannel(_currentCh);
                    this.channelChanged = false;
                }
                this.TimerTagTick(sender, e);
                if (!this.timerTag.Enabled)
                {
                    this.timerTag.Enabled = true;
                }
                this.timerTag.Start();
                if (!this.timerLevels.Enabled)
                {
                    this.timerLevels.Enabled = true;
                }
                this.timerLevels.Start();
            }
        }

        private void BtnStopClick(object sender, EventArgs e)
        {
            this.timerTag.Stop();
            this.timerLevels.Stop();
            ae.ShutDownNet();
            PictureBox pictureBox = this.fxBox;
            object obj = null;
            Image image = (Image)obj;
            this.fxBox.Image = (Image)obj;
            ResetLabels();
            this.checkRec.Enabled = true;
        }

        private void fxBox_ShowSettings(object sender, EventArgs e)
        {
            SimpleWR2.fxDialog fxDialog = new SimpleWR2.fxDialog(ae.Visuals);
            try
            {
                if (fxDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SimpleWR2.Properties.Settings.Default.backColor = fxDialog.GetColor(Visualizer.VisualItem.Background);
                    SimpleWR2.Properties.Settings.Default.baseColor = fxDialog.GetColor(Visualizer.VisualItem.Base);
                    SimpleWR2.Properties.Settings.Default.peakColor = fxDialog.GetColor(Visualizer.VisualItem.Peak);
                    SimpleWR2.Properties.Settings.Default.holdColor = fxDialog.GetColor(Visualizer.VisualItem.Hold);
                    SimpleWR2.Properties.Settings.Default.Save();
                    this.loadFx();
                }
                else
                {
                    return;
                }
            }
            finally
            {
                if (fxDialog != null)
                {
                    ((IDisposable)fxDialog).Dispose();
                }
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            base.Close();
        }

        protected override void Dispose(bool disposing)
        {
            if ((!disposing ? false : this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void endDragging(object sender, MouseEventArgs me)
        {
            if (me.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.dragging = false;
                SimpleWR2.Properties.Settings.Default.locationMainWindow = base.Location;
                SimpleWR2.Properties.Settings.Default.Save();
            }
        }

        private void fxBox_Click(object sender, EventArgs e)
        {
            SimpleWR2.Properties.Settings.Default.visualType = (byte)(ae.Visuals.GetVSType + 1);
            SimpleWR2.Properties.Settings.Default.Save();
            ae.Visuals.SetVisual(SimpleWR2.Properties.Settings.Default.visualType);
        }

        private void loadFx()
        {
            ae.Visuals.SetColor(Visualizer.VisualItem.Background, SimpleWR2.Properties.Settings.Default.backColor);
            ae.Visuals.SetColor(Visualizer.VisualItem.Base, SimpleWR2.Properties.Settings.Default.baseColor);
            ae.Visuals.SetColor(Visualizer.VisualItem.Peak, SimpleWR2.Properties.Settings.Default.peakColor);
            ae.Visuals.SetColor(Visualizer.VisualItem.Hold, SimpleWR2.Properties.Settings.Default.holdColor);
            this.fxBox.BackColor = SimpleWR2.Properties.Settings.Default.backColor;
        }

        private void minimiseBtn_Click(object sender, EventArgs e)
        {
            base.WindowState = FormWindowState.Minimized;
        }

        private void MouseMovementHandler(object sender, MouseEventArgs maus)
        {
            if (this.dragging)
            {
                Point point = base.PointToScreen(maus.Location);
                base.Location = new Point(checked(point.X - this.offset.X), checked(point.Y - this.offset.Y));
            }
        }

        private void RadioForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.btnStop.PerformClick();
        }

        private void SetVolume(float currentValue, float possibleMaximum = 1F)
        {
            float newVolume = currentValue / possibleMaximum;
            ae.PlaybackVolume = newVolume;

            SimpleWR2.Properties.Settings.Default.volumeLevel = currentValue;
            SimpleWR2.Properties.Settings.Default.Save();
        }

        private void startDragging(object sender, MouseEventArgs me)
        {
            if (me.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.dragging = true;
                this.offset.X = me.X;
                this.offset.Y = me.Y;
            }
        }

        private void timerLevels_Tick(object sender, EventArgs e)
        {
            //int[] channelLevels = AudioEngine.GetChannelLevels();
            //this.leftSpectrumBar.Value = channelLevels[0];
            //this.rightSpectrumBar.Value = channelLevels[1];
        }

        private void TimerTagTick(object sender, EventArgs e)
        {
            string[] onlineInfo = ae.GetOnlineInfo();
            this.infoLabel.Text = "";
            if (onlineInfo != null)
            {
                SetStatusOnline(true);
                string[] strArrays = onlineInfo;
                string onlineInfoConcat = string.Empty;
                for (int i = 0; i < (int)strArrays.Length; i++)
                {
                    onlineInfoConcat = string.Concat(onlineInfoConcat, strArrays[i]);
                }
                if (onlineInfoConcat != this.infoLabel.GetItemText(this.lmiInfoIndex))
                {
                    this.infoLabel.StopMove();
                    if (!this.infoLabel.HasTextItem)
                    {
                        this.lmiInfoIndex = this.infoLabel.AddTextItem(onlineInfoConcat, infoLabelPos, ItemDirection.Left, ItemSpeed.Idle);
                    }
                    else
                    {
                        this.infoLabel.SetItemText(this.lmiInfoIndex, string.Empty);
                        this.infoLabel.SetItemText(this.lmiInfoIndex, onlineInfoConcat);
                    }

                    this.infoLabel.StartMove(50);
                    this.m_History.Save(onlineInfoConcat);
                }
            }
            else
            {
                ResetLabels();
            }
        }

        private void SetStatusOnline(bool callFromTimer, bool newOnlineStatus = true)
        {
            if (callFromTimer && (this.IsOnline == newOnlineStatus))
            {
                return;
            }

            if (!this.volumeChanging)
            {
                this.IsOnline = newOnlineStatus;
                if (this.IsOnline)
                {
                    string onlineText = " * Online * ";
                    {
                        this.statusLabel.LedOffColor = Color.DarkGreen;
                        this.statusLabel.LedOnColor = Color.LimeGreen;
                        if (!this.statusLabel.HasTextItem)
                        {
                            this.lmiStatusIndex = this.statusLabel.AddTextItem(onlineText, statusLabelPos, ItemDirection.Left, ItemSpeed.Slow);
                        }
                        else
                        {
                            this.statusLabel.SetItemText(this.lmiInfoIndex, string.Empty);
                            this.statusLabel.SetItemText(this.lmiInfoIndex, onlineText);
                        }
                    }
                }
                else
                {
                    string offlineText = " - Offline - ";
                    this.statusLabel.LedOffColor = Color.DarkRed;
                    this.statusLabel.LedOnColor = Color.Crimson;
                    this.statusLabel.SetItemText(this.lmiStatusIndex, offlineText);
                }
            }
        }

        private void volCheck_CheckStateChanged(object sender, EventArgs e)
        {
            switch (((CheckBox)sender).CheckState)
            {
                case CheckState.Unchecked:
                    {
                        this.SetVolume(1f);
                        break;
                    }
                case CheckState.Checked:
                    {
                        this.SetVolume(0f);
                        break;
                    }
                case CheckState.Indeterminate:
                    {
                        this.SetVolume(0.5f);
                        break;
                    }
            }
            this.volKnob.Value = ae.PlaybackVolume;
        }

        private void volKnob_KnobChangeValue(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e)
        {
            LBSoft.IndustrialCtrls.Knobs.LBKnob knob = sender as LBSoft.IndustrialCtrls.Knobs.LBKnob;
            this.SetVolume(knob.Value, knob.MaxValue);
            this.statusLabel.Invoke((Action)(() =>
            {
                this.statusLabel.SetItemText(this.lmiStatusIndex, $"Vol.: {knob.Value.ToString("P1")}");
            }));

        }

        private void BeginVolumeChanging(object sender, EventArgs e)
        {
            this.volumeChanging = true;
        }

        private void EndVolumeChanging(object sender, EventArgs e)
        {
            this.volumeChanging = false;
            this.SetStatusOnline(false, this.IsOnline);
        }

        private void btnSwitchOutput_Click(object sender, EventArgs e)
        {
            aod.ShowDialog();
        }

        private void channelChooser_Click(object sender, EventArgs e)
        {
            string chosenChannel = string.Empty;
            switch ((sender as Control).Name)
            {
                case "pb_HitHouse": SelectChannelRadioButton(ch_HitHouse); break;
                case "pb_Deep": SelectChannelRadioButton(ch_Deep); break;
                case "pb_RnB": SelectChannelRadioButton(ch_RnB); break;
                default:
                    SelectChannelRadioButton(ch_Classic);
                    break;
            }
        }

        private void SelectChannelRadioButton(string channelName)
        {
            switch (channelName)
            {
                case ch_HitHouse: rb_HitHouse.Checked = true; break;
                case ch_Deep: rb_Deep.Checked = true; break;
                case ch_RnB: rb_RnB.Checked = true; break;
                default:
                    rb_Classic.Checked = true;
                    break;
            }
        }

        private void channelChooser_CheckedChanged(object sender, EventArgs e)
        {
            string chosenChannel = string.Empty;
            if ((sender as RadioButton).Checked)
            {
                switch ((sender as Control).Name)
                {
                    case "rb_Classic": chosenChannel = ch_Classic; break;
                    case "rb_HitHouse": chosenChannel = ch_HitHouse; break;
                    case "rb_Deep": chosenChannel = ch_Deep; break;
                    case "rb_RnB": chosenChannel = ch_RnB; break;
                    default:
                        break;
                }
                this.ChangeChannel(chosenChannel);
            }

        }

        private void ChangeChannel(string channelName)
        {
            channel_type ch = _channels.channels.FirstOrDefault(c => c.name == channelName);
            if (_currentCh != ch)
            {
                _currentCh = ch;
                this.btnStop.PerformClick();
                SimpleWR2.Properties.Settings.Default.channel = channelName;
                SimpleWR2.Properties.Settings.Default.Save();
                this.channelChanged = true;
            }
            else
            {
                this.channelChanged = false;
            }
        }

        #region InitializeComponent
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RadioForm));
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.checkRec = new System.Windows.Forms.CheckBox();
            this.timerTag = new System.Windows.Forms.Timer(this.components);
            this.timerLevels = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSwitchOutput = new System.Windows.Forms.Button();
            this.minimiseBtn = new System.Windows.Forms.Button();
            this.statusLabel = new LedMatrixControlNamespace.LedMatrixControl();
            this.closeBtn = new System.Windows.Forms.Button();
            this.fxBox = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.channelChooserBox = new System.Windows.Forms.GroupBox();
            this.pb_RnB = new System.Windows.Forms.PictureBox();
            this.rb_RnB = new System.Windows.Forms.RadioButton();
            this.pb_Deep = new System.Windows.Forms.PictureBox();
            this.pb_House = new System.Windows.Forms.PictureBox();
            this.pb_Classic = new System.Windows.Forms.PictureBox();
            this.rb_Deep = new System.Windows.Forms.RadioButton();
            this.rb_HitHouse = new System.Windows.Forms.RadioButton();
            this.rb_Classic = new System.Windows.Forms.RadioButton();
            this.infoLabel = new LedMatrixControlNamespace.LedMatrixControl();
            this.volKnob = new LBSoft.IndustrialCtrls.Knobs.LBKnob();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fxBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.channelChooserBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_RnB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Deep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_House)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Classic)).BeginInit();
            this.SuspendLayout();
            //
            // btnPlay
            //
            this.btnPlay.BackColor = System.Drawing.Color.Black;
            this.btnPlay.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnPlay.Image = global::SimpleWR2.Properties.Resources.PlayHS;
            this.btnPlay.Location = new System.Drawing.Point(408, 110);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(80, 23);
            this.btnPlay.TabIndex = 16;
            this.btnPlay.Text = "Play";
            this.btnPlay.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.BtnPlayClick);
            //
            // btnStop
            //
            this.btnStop.BackColor = System.Drawing.Color.Black;
            this.btnStop.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnStop.Image = global::SimpleWR2.Properties.Resources.StopHS;
            this.btnStop.Location = new System.Drawing.Point(408, 139);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(80, 23);
            this.btnStop.TabIndex = 17;
            this.btnStop.Text = "Stop";
            this.btnStop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.BtnStopClick);
            //
            // checkRec
            //
            this.checkRec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(224)))));
            this.checkRec.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.checkRec.Image = global::SimpleWR2.Properties.Resources.RecordHS;
            this.checkRec.Location = new System.Drawing.Point(408, 168);
            this.checkRec.Name = "checkRec";
            this.checkRec.Size = new System.Drawing.Size(80, 24);
            this.checkRec.TabIndex = 27;
            this.checkRec.Text = "Record";
            this.checkRec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkRec.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.checkRec.UseVisualStyleBackColor = false;
            //
            // timerTag
            //
            this.timerTag.Interval = 5000;
            this.timerTag.Tick += new System.EventHandler(this.TimerTagTick);
            //
            // timerLevels
            //
            this.timerLevels.Tick += new System.EventHandler(this.timerLevels_Tick);
            //
            // panel2
            //
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.btnSwitchOutput);
            this.panel2.Controls.Add(this.minimiseBtn);
            this.panel2.Controls.Add(this.statusLabel);
            this.panel2.Controls.Add(this.closeBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(500, 29);
            this.panel2.TabIndex = 30;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startDragging);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMovementHandler);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.endDragging);
            //
            // btnSwitchOutput
            //
            this.btnSwitchOutput.BackColor = System.Drawing.Color.Transparent;
            this.btnSwitchOutput.BackgroundImage = global::SimpleWR2.Properties.Resources.base_speaker_32;
            this.btnSwitchOutput.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSwitchOutput.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSwitchOutput.Location = new System.Drawing.Point(240, 3);
            this.btnSwitchOutput.Name = "btnSwitchOutput";
            this.btnSwitchOutput.Size = new System.Drawing.Size(23, 23);
            this.btnSwitchOutput.TabIndex = 22;
            this.btnSwitchOutput.UseVisualStyleBackColor = false;
            this.btnSwitchOutput.Click += new System.EventHandler(this.btnSwitchOutput_Click);
            //
            // minimiseBtn
            //
            this.minimiseBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minimiseBtn.BackColor = System.Drawing.Color.Black;
            this.minimiseBtn.BackgroundImage = global::SimpleWR2.Properties.Resources.minimize;
            this.minimiseBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.minimiseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimiseBtn.Location = new System.Drawing.Point(436, 0);
            this.minimiseBtn.Name = "minimiseBtn";
            this.minimiseBtn.Size = new System.Drawing.Size(23, 23);
            this.minimiseBtn.TabIndex = 1;
            this.minimiseBtn.UseVisualStyleBackColor = false;
            this.minimiseBtn.Click += new System.EventHandler(this.minimiseBtn_Click);
            //
            // statusLabel
            //
            this.statusLabel.BackColor = System.Drawing.Color.Black;
            this.statusLabel.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.statusLabel.ForeColor = System.Drawing.Color.White;
            this.statusLabel.LedOffColor = System.Drawing.Color.DarkRed;
            this.statusLabel.LedOnColor = System.Drawing.Color.Crimson;
            this.statusLabel.LedStyle = LedMatrixControlNamespace.LedStyle.Round;
            this.statusLabel.Location = new System.Drawing.Point(18, 2);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.NbLedLines = 8;
            this.statusLabel.NbLedRows = 71;
            this.statusLabel.Size = new System.Drawing.Size(214, 24);
            this.statusLabel.SizeCoeff = 0.67D;
            this.statusLabel.TabIndex = 21;
            this.statusLabel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startDragging);
            this.statusLabel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMovementHandler);
            this.statusLabel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.endDragging);
            //
            // closeBtn
            //
            this.closeBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.closeBtn.BackColor = System.Drawing.Color.Black;
            this.closeBtn.BackgroundImage = global::SimpleWR2.Properties.Resources.close;
            this.closeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Location = new System.Drawing.Point(465, 0);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(23, 23);
            this.closeBtn.TabIndex = 0;
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            //
            // fxBox
            //
            this.fxBox.BackColor = System.Drawing.Color.Transparent;
            this.fxBox.BackgroundImage = global::SimpleWR2.Properties.Resources.s106976g;
            this.fxBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.fxBox.Location = new System.Drawing.Point(15, 35);
            this.fxBox.Name = "fxBox";
            this.fxBox.Size = new System.Drawing.Size(219, 58);
            this.fxBox.TabIndex = 15;
            this.fxBox.TabStop = false;
            this.fxBox.Click += new System.EventHandler(this.fxBox_Click);
            this.fxBox.DoubleClick += new System.EventHandler(this.fxBox_ShowSettings);
            //
            // pictureBox1
            //
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::SimpleWR2.Properties.Resources.risefm_slogan_only;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(240, 32);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(158, 61);
            this.pictureBox1.TabIndex = 32;
            this.pictureBox1.TabStop = false;
            //
            // channelChooserBox
            //
            this.channelChooserBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.channelChooserBox.Controls.Add(this.pb_RnB);
            this.channelChooserBox.Controls.Add(this.rb_RnB);
            this.channelChooserBox.Controls.Add(this.pb_Deep);
            this.channelChooserBox.Controls.Add(this.pb_House);
            this.channelChooserBox.Controls.Add(this.pb_Classic);
            this.channelChooserBox.Controls.Add(this.rb_Deep);
            this.channelChooserBox.Controls.Add(this.rb_HitHouse);
            this.channelChooserBox.Controls.Add(this.rb_Classic);
            this.channelChooserBox.Location = new System.Drawing.Point(15, 99);
            this.channelChooserBox.Name = "channelChooserBox";
            this.channelChooserBox.Size = new System.Drawing.Size(380, 93);
            this.channelChooserBox.TabIndex = 34;
            this.channelChooserBox.TabStop = false;
            //
            // pb_RnB
            //
            this.pb_RnB.BackColor = System.Drawing.Color.Transparent;
            this.pb_RnB.BackgroundImage = global::SimpleWR2.Properties.Resources.rnb;
            this.pb_RnB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_RnB.Location = new System.Drawing.Point(234, 50);
            this.pb_RnB.Name = "pb_RnB";
            this.pb_RnB.Size = new System.Drawing.Size(134, 33);
            this.pb_RnB.TabIndex = 7;
            this.pb_RnB.TabStop = false;
            this.pb_RnB.Click += new System.EventHandler(this.channelChooser_Click);
            //
            // rb_RnB
            //
            this.rb_RnB.AutoSize = true;
            this.rb_RnB.Location = new System.Drawing.Point(214, 60);
            this.rb_RnB.Name = "rb_RnB";
            this.rb_RnB.Size = new System.Drawing.Size(14, 13);
            this.rb_RnB.TabIndex = 6;
            this.rb_RnB.UseVisualStyleBackColor = true;
            this.rb_RnB.CheckedChanged += new System.EventHandler(this.channelChooser_CheckedChanged);
            //
            // pb_Deep
            //
            this.pb_Deep.BackColor = System.Drawing.Color.Transparent;
            this.pb_Deep.BackgroundImage = global::SimpleWR2.Properties.Resources.deep;
            this.pb_Deep.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_Deep.Location = new System.Drawing.Point(234, 11);
            this.pb_Deep.Name = "pb_Deep";
            this.pb_Deep.Size = new System.Drawing.Size(140, 33);
            this.pb_Deep.TabIndex = 5;
            this.pb_Deep.TabStop = false;
            this.pb_Deep.Click += new System.EventHandler(this.channelChooser_Click);
            //
            // pb_House
            //
            this.pb_House.BackColor = System.Drawing.Color.Transparent;
            this.pb_House.BackgroundImage = global::SimpleWR2.Properties.Resources.hit_house;
            this.pb_House.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_House.Location = new System.Drawing.Point(31, 50);
            this.pb_House.Name = "pb_House";
            this.pb_House.Size = new System.Drawing.Size(152, 33);
            this.pb_House.TabIndex = 4;
            this.pb_House.TabStop = false;
            this.pb_House.Click += new System.EventHandler(this.channelChooser_Click);
            //
            // pb_Classic
            //
            this.pb_Classic.BackColor = System.Drawing.Color.Transparent;
            this.pb_Classic.BackgroundImage = global::SimpleWR2.Properties.Resources.classic;
            this.pb_Classic.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pb_Classic.Location = new System.Drawing.Point(31, 11);
            this.pb_Classic.Name = "pb_Classic";
            this.pb_Classic.Size = new System.Drawing.Size(146, 33);
            this.pb_Classic.TabIndex = 3;
            this.pb_Classic.TabStop = false;
            this.pb_Classic.Click += new System.EventHandler(this.channelChooser_Click);
            //
            // rb_Deep
            //
            this.rb_Deep.AutoSize = true;
            this.rb_Deep.Location = new System.Drawing.Point(214, 23);
            this.rb_Deep.Name = "rb_Deep";
            this.rb_Deep.Size = new System.Drawing.Size(14, 13);
            this.rb_Deep.TabIndex = 2;
            this.rb_Deep.UseVisualStyleBackColor = true;
            this.rb_Deep.CheckedChanged += new System.EventHandler(this.channelChooser_CheckedChanged);
            //
            // rb_HitHouse
            //
            this.rb_HitHouse.AutoSize = true;
            this.rb_HitHouse.Location = new System.Drawing.Point(11, 60);
            this.rb_HitHouse.Name = "rb_HitHouse";
            this.rb_HitHouse.Size = new System.Drawing.Size(14, 13);
            this.rb_HitHouse.TabIndex = 1;
            this.rb_HitHouse.UseVisualStyleBackColor = true;
            this.rb_HitHouse.CheckedChanged += new System.EventHandler(this.channelChooser_CheckedChanged);
            //
            // rb_Classic
            //
            this.rb_Classic.AutoSize = true;
            this.rb_Classic.Checked = true;
            this.rb_Classic.Location = new System.Drawing.Point(11, 23);
            this.rb_Classic.Name = "rb_Classic";
            this.rb_Classic.Size = new System.Drawing.Size(14, 13);
            this.rb_Classic.TabIndex = 0;
            this.rb_Classic.TabStop = true;
            this.rb_Classic.UseVisualStyleBackColor = true;
            this.rb_Classic.CheckedChanged += new System.EventHandler(this.channelChooser_CheckedChanged);
            //
            // infoLabel
            //
            this.infoLabel.BackColor = System.Drawing.Color.Black;
            this.infoLabel.LedOffColor = System.Drawing.Color.Sienna;
            this.infoLabel.LedOnColor = System.Drawing.Color.Gold;
            this.infoLabel.LedStyle = LedMatrixControlNamespace.LedStyle.Round;
            this.infoLabel.Location = new System.Drawing.Point(15, 198);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.NbLedLines = 10;
            this.infoLabel.NbLedRows = 118;
            this.infoLabel.Size = new System.Drawing.Size(474, 42);
            this.infoLabel.SizeCoeff = 0.5D;
            this.infoLabel.TabIndex = 35;
            //
            // volKnob
            //
            this.volKnob.BackColor = System.Drawing.Color.Transparent;
            this.volKnob.DrawRatio = 0.29F;
            this.volKnob.IndicatorColor = System.Drawing.Color.Gray;
            this.volKnob.IndicatorOffset = 10F;
            this.volKnob.KnobCenter = ((System.Drawing.PointF)(resources.GetObject("volKnob.KnobCenter")));
            this.volKnob.KnobColor = System.Drawing.Color.DarkGray;
            this.volKnob.KnobRect = ((System.Drawing.RectangleF)(resources.GetObject("volKnob.KnobRect")));
            this.volKnob.Location = new System.Drawing.Point(413, 35);
            this.volKnob.MaxValue = 1F;
            this.volKnob.MinValue = 0F;
            this.volKnob.Name = "volKnob";
            this.volKnob.Renderer = null;
            this.volKnob.ScaleColor = System.Drawing.Color.Silver;
            this.volKnob.Size = new System.Drawing.Size(58, 58);
            this.volKnob.StepValue = 0.01F;
            this.volKnob.Style = LBSoft.IndustrialCtrls.Knobs.LBKnob.KnobStyle.Circular;
            this.volKnob.TabIndex = 31;
            this.volKnob.Value = 0F;
            this.volKnob.KnobChangeValue += new LBSoft.IndustrialCtrls.Knobs.KnobChangeValue(this.volKnob_KnobChangeValue);
            this.volKnob.KeyDown += new System.Windows.Forms.KeyEventHandler(this.BeginVolumeChanging);
            this.volKnob.KeyUp += new System.Windows.Forms.KeyEventHandler(this.EndVolumeChanging);
            this.volKnob.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BeginVolumeChanging);
            this.volKnob.MouseUp += new System.Windows.Forms.MouseEventHandler(this.EndVolumeChanging);
            //
            // RadioForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SimpleWR2.Properties.Resources.wallpaperflare_600x400;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(500, 250);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.channelChooserBox);
            this.Controls.Add(this.volKnob);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.checkRec);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.fxBox);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RadioForm";
            this.Text = "SimpleWR2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RadioForm_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startDragging);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMovementHandler);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.endDragging);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fxBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.channelChooserBox.ResumeLayout(false);
            this.channelChooserBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_RnB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Deep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_House)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Classic)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
    }
}