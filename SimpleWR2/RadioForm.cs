using Azure.LibCollection.CS;
using Azure.LibCollection.CS.AudioWorks;
using Azure.MediaUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SimpleWR2
{
	public class RadioForm : Form
	{
		private IContainer components = null;

		private channel_type[] _channels;

        private channel_type selectedChannel;

		private bool dragging;

		private Point offset;

		private Button btnPlay;

		private Panel panel1;

		private PictureBox fxBox;

		private Button btnStop;

		private CheckBox checkRec;

		private Timer timerTag;

		private ProgressBar progressBar2;

		private Label statusLabel;

		private Label infoLabel;

		private TrackBar volBar;

		private Button button4;

		private Button button6;

		private Timer timerLevels;

		private CheckBox volCheck;

		private Panel panel2;

		private Button closeBtn;

		private Button minimiseBtn;

		private ComboBox channelCombo;

		private ProgressBar progressBar1;

		private AudioEngine ae;

		public RadioForm()
		{
			this.InitializeComponent();
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
                string startChannel = SimpleWR2.Properties.Settings.Default.channel;
                ParseChannelsList();
                Point locationMainWindow = SimpleWR2.Properties.Settings.Default.locationMainWindow;
                int x = locationMainWindow.X;
                locationMainWindow = SimpleWR2.Properties.Settings.Default.locationMainWindow;
                base.Location = new Point(x, locationMainWindow.Y);
                this.volBar.Value = SimpleWR2.Properties.Settings.Default.volumeLevel;
                this.SetVolume(this.volBar);
                ae.Visuals = new Visualizer(ref this.fxBox);
                ae.Visuals.SetVisual(SimpleWR2.Properties.Settings.Default.visualType);
                ae.Visuals.Set("drawFull", false);
                this.loadFx();
                SelectChannel(string.IsNullOrWhiteSpace(startChannel) ? _channels[0].name : startChannel);
                if (selectedChannel == null)
                {
                    selectedChannel = _channels.FirstOrDefault();
                }
            }
        }

        private void SelectChannel(string channelName)
        {
            int ix;
            try
            {
                ix = Array.IndexOf(this._channels, this._channels.FirstOrDefault(c => c.name == channelName));
            }
            catch
            {
                ix = 0;
            }

            this.channelCombo.SelectedIndex = ix;
        }

        private void ParseChannelsList()
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(string.Format("{0}\\Channels.xList", Program.AssemblyDirectory));
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            using (Stream schemaStream = myAssembly.GetManifestResourceStream("SimpleWR2.Channels.xsd"))
            {
                XmlSchema schema = XmlSchema.Read(schemaStream, null);
                xml.Schemas.Add(schema);
            }
            ValidationEventHandler validEvHandler = new ValidationEventHandler(validateChannelsListCallBack);
            xml.Validate(validEvHandler);
            XmlReader reader = new XmlNodeReader(xml);
            XmlSerializer serializer = new XmlSerializer(typeof(channels_type));
            channels_type result = (channels_type)serializer.Deserialize(reader);

            if (result.channels.Length > 0)
            {
                this.channelCombo.DataSource = null;
                this._channels = result.channels.OrderBy(c => uint.Parse(c.order)).ToArray();
                this.channelCombo.DataSource = this._channels;
                this.channelCombo.DisplayMember = "name";
                this.channelCombo.ValueMember = "url";
            }
            else
            {
                throw new ArgumentException("There are no channels specified in Channels.xlist file!");
            }
        }

        private static void validateChannelsListCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                throw new Exception(string.Format("Warning: Matching schema not found.  No validation occurred." + args.Message));
            else
                throw new Exception(string.Format("Channel list validation error: " + args.Message));

        }

        private void BtnPlayClick(object sender, EventArgs e)
		{
            ae.InitOnlineStream(selectedChannel.url, this.checkRec.Checked);
            if (this.checkRec.Checked)
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog();
				try
				{
					saveFileDialog.Filter = string.Format("Stream Format|*{0}", ChannelFormat.GetExtension(ae));
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
			object obj = null;
			Image image = (Image)obj;
			this.fxBox.Image = (Image)obj;
			ProgressBar progressBar = this.progressBar1;
			int num = 0;
			int num1 = num;
			this.progressBar2.Value = num;
			progressBar.Value = num1;
			Label label = this.infoLabel;
			string str = "";
			string str1 = str;
			this.statusLabel.Text = str;
			label.Text = str1;
		}

		private void button6_Click(object sender, EventArgs e)
		{
			SimpleWR2.fxDialog fxDialog = new SimpleWR2.fxDialog(ae.Visuals);
			try
			{
				if (fxDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
                    SimpleWR2.Properties.Settings.Default.backColor = fxDialog.GetColor("back");
                    SimpleWR2.Properties.Settings.Default.baseColor = fxDialog.GetColor("base");
                    SimpleWR2.Properties.Settings.Default.peakColor = fxDialog.GetColor("peak");
                    SimpleWR2.Properties.Settings.Default.holdColor = fxDialog.GetColor("hold");
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

		private void channelCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
            ComboBox cb = (ComboBox)sender;
            if (cb.DataSource != null && cb.SelectedItem != null)
            {
                this.btnStop.PerformClick();
                selectedChannel = cb.SelectedItem as channel_type;
                SimpleWR2.Properties.Settings.Default.channel = selectedChannel.name;
                SimpleWR2.Properties.Settings.Default.Save();
            }
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

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RadioForm));
            this.btnPlay = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStop = new System.Windows.Forms.Button();
            this.checkRec = new System.Windows.Forms.CheckBox();
            this.timerTag = new System.Windows.Forms.Timer(this.components);
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.statusLabel = new System.Windows.Forms.Label();
            this.infoLabel = new System.Windows.Forms.Label();
            this.volBar = new System.Windows.Forms.TrackBar();
            this.button4 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.timerLevels = new System.Windows.Forms.Timer(this.components);
            this.volCheck = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.channelCombo = new System.Windows.Forms.ComboBox();
            this.minimiseBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.fxBox = new System.Windows.Forms.PictureBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.volBar)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fxBox)).BeginInit();
            this.SuspendLayout();
            //
            // btnPlay
            //
            this.btnPlay.BackColor = System.Drawing.Color.Black;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnPlay.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnPlay.Location = new System.Drawing.Point(3, 64);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 23);
            this.btnPlay.TabIndex = 16;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.BtnPlayClick);
            //
            // panel1
            //
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Location = new System.Drawing.Point(356, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(32, 82);
            this.panel1.TabIndex = 28;
            //
            // btnStop
            //
            this.btnStop.BackColor = System.Drawing.Color.Black;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnStop.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnStop.Location = new System.Drawing.Point(3, 92);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 17;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.BtnStopClick);
            //
            // checkRec
            //
            this.checkRec.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.checkRec.Location = new System.Drawing.Point(5, 35);
            this.checkRec.Name = "checkRec";
            this.checkRec.Size = new System.Drawing.Size(73, 24);
            this.checkRec.TabIndex = 27;
            this.checkRec.Text = "Record";
            this.checkRec.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkRec.UseVisualStyleBackColor = false;
            //
            // timerTag
            //
            this.timerTag.Interval = 5000;
            this.timerTag.Tick += new System.EventHandler(this.TimerTagTick);
            //
            // progressBar2
            //
            this.progressBar2.ForeColor = System.Drawing.Color.Silver;
            this.progressBar2.Location = new System.Drawing.Point(199, 121);
            this.progressBar2.Maximum = 32768;
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(190, 23);
            this.progressBar2.Step = 1;
            this.progressBar2.TabIndex = 20;
            //
            // statusLabel
            //
            this.statusLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.statusLabel.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.statusLabel.ForeColor = System.Drawing.Color.White;
            this.statusLabel.Location = new System.Drawing.Point(3, 147);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(83, 29);
            this.statusLabel.TabIndex = 21;
            this.statusLabel.Text = " ";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.statusLabel.UseCompatibleTextRendering = true;
            //
            // infoLabel
            //
            this.infoLabel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.infoLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.infoLabel.Font = new System.Drawing.Font("Arial Narrow", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.infoLabel.ForeColor = System.Drawing.Color.White;
            this.infoLabel.Location = new System.Drawing.Point(83, 147);
            this.infoLabel.Name = "infoLabel";
            this.infoLabel.Size = new System.Drawing.Size(306, 29);
            this.infoLabel.TabIndex = 22;
            this.infoLabel.Text = " ";
            this.infoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.infoLabel.UseCompatibleTextRendering = true;
            //
            // volBar
            //
            this.volBar.AutoSize = false;
            this.volBar.BackColor = System.Drawing.Color.Maroon;
            this.volBar.Location = new System.Drawing.Point(357, 35);
            this.volBar.Maximum = 100;
            this.volBar.Name = "volBar";
            this.volBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.volBar.Size = new System.Drawing.Size(30, 80);
            this.volBar.TabIndex = 23;
            this.volBar.Value = 100;
            this.volBar.Scroll += new System.EventHandler(this.volBar_Scroll);
            this.volBar.ValueChanged += new System.EventHandler(this.volBar_ValueChanged);
            //
            // button4
            //
            this.button4.BackColor = System.Drawing.Color.Black;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button4.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button4.Location = new System.Drawing.Point(314, 34);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(35, 23);
            this.button4.TabIndex = 24;
            this.button4.Text = "Opt";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Visible = false;
            //
            // button6
            //
            this.button6.BackColor = System.Drawing.Color.Black;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button6.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.button6.Location = new System.Drawing.Point(314, 92);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(35, 23);
            this.button6.TabIndex = 26;
            this.button6.Text = "fx";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            //
            // timerLevels
            //
            this.timerLevels.Tick += new System.EventHandler(this.timerLevels_Tick);
            //
            // volCheck
            //
            this.volCheck.Appearance = System.Windows.Forms.Appearance.Button;
            this.volCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.volCheck.Location = new System.Drawing.Point(314, 64);
            this.volCheck.Name = "volCheck";
            this.volCheck.Size = new System.Drawing.Size(35, 23);
            this.volCheck.TabIndex = 29;
            this.volCheck.Text = "Vol.";
            this.volCheck.ThreeState = true;
            this.volCheck.UseVisualStyleBackColor = false;
            this.volCheck.CheckStateChanged += new System.EventHandler(this.volCheck_CheckStateChanged);
            //
            // panel2
            //
            this.panel2.BackColor = System.Drawing.Color.Black;
            this.panel2.Controls.Add(this.channelCombo);
            this.panel2.Controls.Add(this.minimiseBtn);
            this.panel2.Controls.Add(this.closeBtn);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(394, 29);
            this.panel2.TabIndex = 30;
            this.panel2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startDragging);
            this.panel2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMovementHandler);
            this.panel2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.endDragging);
            //
            // channelCombo
            //
            this.channelCombo.BackColor = System.Drawing.Color.Black;
            this.channelCombo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.channelCombo.ForeColor = System.Drawing.Color.Silver;
            this.channelCombo.FormattingEnabled = true;
            this.channelCombo.Location = new System.Drawing.Point(5, 3);
            this.channelCombo.Name = "channelCombo";
            this.channelCombo.Size = new System.Drawing.Size(303, 21);
            this.channelCombo.TabIndex = 32;
            this.channelCombo.SelectedIndexChanged += new System.EventHandler(this.channelCombo_SelectedIndexChanged);
            //
            // minimiseBtn
            //
            this.minimiseBtn.BackColor = System.Drawing.Color.Black;
            this.minimiseBtn.BackgroundImage = global::SimpleWR2.Properties.Resources.minimize;
            this.minimiseBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.minimiseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimiseBtn.Location = new System.Drawing.Point(339, 3);
            this.minimiseBtn.Name = "minimiseBtn";
            this.minimiseBtn.Size = new System.Drawing.Size(23, 23);
            this.minimiseBtn.TabIndex = 1;
            this.minimiseBtn.UseVisualStyleBackColor = false;
            this.minimiseBtn.Click += new System.EventHandler(this.minimiseBtn_Click);
            //
            // closeBtn
            //
            this.closeBtn.BackColor = System.Drawing.Color.Black;
            this.closeBtn.BackgroundImage = global::SimpleWR2.Properties.Resources.close;
            this.closeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Location = new System.Drawing.Point(368, 3);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(23, 23);
            this.closeBtn.TabIndex = 0;
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            //
            // fxBox
            //
            this.fxBox.BackColor = System.Drawing.Color.Black;
            this.fxBox.BackgroundImage = global::SimpleWR2.Properties.Resources.purpleLCD;
            this.fxBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.fxBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.fxBox.Location = new System.Drawing.Point(84, 31);
            this.fxBox.Name = "fxBox";
            this.fxBox.Size = new System.Drawing.Size(224, 84);
            this.fxBox.TabIndex = 15;
            this.fxBox.TabStop = false;
            this.fxBox.Click += new System.EventHandler(this.fxBox_Click);
            //
            // progressBar1
            //
            this.progressBar1.BackColor = System.Drawing.Color.Black;
            this.progressBar1.ForeColor = System.Drawing.Color.Silver;
            this.progressBar1.Location = new System.Drawing.Point(3, 121);
            this.progressBar1.MarqueeAnimationSpeed = 50;
            this.progressBar1.Maximum = 32768;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.progressBar1.RightToLeftLayout = true;
            this.progressBar1.Size = new System.Drawing.Size(190, 23);
            this.progressBar1.Step = 1;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 19;
            //
            // RadioForm
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::SimpleWR2.Properties.Resources.background;
            this.ClientSize = new System.Drawing.Size(394, 181);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.volCheck);
            this.Controls.Add(this.checkRec);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.volBar);
            this.Controls.Add(this.infoLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.fxBox);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "RadioForm";
            this.Text = "SimpleWR2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RadioForm_FormClosing);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.startDragging);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MouseMovementHandler);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.endDragging);
            ((System.ComponentModel.ISupportInitialize)(this.volBar)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fxBox)).EndInit();
            this.ResumeLayout(false);

		}

		private void loadFx()
		{
			ae.Visuals.Set("backColor", SimpleWR2.Properties.Settings.Default.backColor);
            ae.Visuals.Set("baseColor", SimpleWR2.Properties.Settings.Default.baseColor);
            ae.Visuals.Set("peakColor", SimpleWR2.Properties.Settings.Default.peakColor);
            ae.Visuals.Set("holdColor", SimpleWR2.Properties.Settings.Default.holdColor);
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

		private void SetVolume(object sender)
		{
			ae.PlaybackVolume = ((float)((TrackBar)sender).Value / (float)((TrackBar)sender).Maximum);
            SimpleWR2.Properties.Settings.Default.volumeLevel = ((TrackBar)sender).Value;
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
			int[] channelLevels = ae.GetChannelLevels();
			this.progressBar1.Value = channelLevels[0];
			this.progressBar2.Value = channelLevels[1];
		}

		private void TimerTagTick(object sender, EventArgs e)
		{
			string[] onlineInfo = ae.GetOnlineInfo();
			this.infoLabel.Text = "";
			if (onlineInfo != null)
			{
				this.statusLabel.Text = "ONLINE";
				string[] strArrays = onlineInfo;
				for (int i = 0; i < (int)strArrays.Length; i++)
				{
					string str = strArrays[i];
					Label label = this.infoLabel;
					label.Text = string.Concat(label.Text, str);
				}
			}
			else
			{
				this.statusLabel.Text = "OFFLINE";
			}
		}

		private void volBar_Scroll(object sender, EventArgs e)
		{
			this.SetVolume(sender);
		}

		private void volBar_ValueChanged(object sender, EventArgs e)
		{
		}

		private void volCheck_CheckStateChanged(object sender, EventArgs e)
		{
			switch (((CheckBox)sender).CheckState)
			{
				case CheckState.Unchecked:
				{
					ae.PlaybackVolume = (1f);
					break;
				}
				case CheckState.Checked:
				{
					ae.PlaybackVolume = (0f);
					break;
				}
				case CheckState.Indeterminate:
				{
					ae.PlaybackVolume = (0.5f);
					break;
				}
			}
			this.volBar.Value = checked((int)Math.Floor((double)ae.PlaybackVolume * 100));
		}
	}
}