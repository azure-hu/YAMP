using Azure.LibCollection.CS;
using Azure.MediaUtils;
using Fusionbird.FusionToolkit.FusionTrackBar;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Azure.YAMP
{
	public partial class PlayerUI3 : Form
	{
		#region variables

		private bool dragging, playStarted, seeking;
		private Point offset;
		private Azure.LibCollection.CS.AudioWorks.tagWnd tagForm;
		public string currentFile;
		private string startDir;
		private repeatOption repeat;
		private double seekSet;

		private bool loadingFiles;
		//private string defaultTitle;
		public int currentIndex, selectedIndex;
		private int searchIndex;
		private string helpPlayList, helpSearchBox;
		public bool dragHappened;
		private const int cGrip = 16;
		private PrivateFontCollection pfc;
		private List<string> pfc_names;
		private const int withPlWidth = 656;
		private AudioEngine ae;
		private EqualiserEngine eq;
		private readonly string _asmName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
		private bool startup;
		private PlayList playList;
		private Dictionary<int, string> _outputs;
		private KeyValuePair<int, string> _activeOut;
		private short _posDisplay;
		private readonly Size sizeWithPlEditMin = new Size(720, 480);
		private readonly Size sizeWithPlEditMax = new Size(1080, 720);
		private readonly Size sizeWithNoPlEdit = new Size(348, 436);
		#endregion variables

		public String SupportedFileExtensions { get { return ae.SupportedFileExtensions; } }
		public String SupportedFileFilter { get { return ae.SupportedFileFilter; } }

		public PlayerUI3()
		{
			startup = true;
			SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			SetStyle(ControlStyles.UserPaint, true);
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			startDir = System.IO.Directory.GetCurrentDirectory() + "\\";
			InitializeComponent();
			loadingLabel.BackColor = Color.FromArgb(128, Color.Black);
			playList = new PlayList(this.playListView, this);

			//InitializeIcons(titIco, YAMP.Properties.Resources.TitleIcon, Color.FromArgb(128,128,128));
			//InitializeIcons(artIco, YAMP.Properties.Resources.ArtistIcon, Color.FromArgb(128, 128, 128));
			//InitializeIcons(albIco, YAMP.Properties.Resources.AlbumIcon, Color.FromArgb(128, 128, 128));
			mainLabelToDefault();
			playStarted = false;

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
					throw x;
				}
			}

			if (!initSuccess)
			{
				MBoxHelper.ShowErrorMsg(initErrorMsg, "BASS Init Error!");
				System.Diagnostics.Process proc = System.Diagnostics.Process.GetCurrentProcess();
				proc.Kill();
			}
			openMedia.Filter = ae.SupportedFileFilter;
			InitOutputDeviceList();

			tagForm = new Azure.LibCollection.CS.AudioWorks.tagWnd();
			//tagForm.BackgroundImage = global::Azure.YAMP.Properties.Resources.background;
			tagForm.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.tagWindow_FormClosing);

			if (!SettingsExist)
			{
				MBoxHelper.ShowWarnMsg("Settings file not found!", "Missing File!");
				SetWindowTheme(this.Handle, "", "");
			}
			else
			{
				if (File.Exists(".\\yamp.settings"))
				{
					if (!File.Exists(".\\" + _asmName + ".old_config"))
					{
						File.Move(".\\" + _asmName + ".exe.config", ".\\" + _asmName + ".old_config");
					}
					File.Copy(".\\yamp.settings", ".\\" + _asmName + ".exe.config", true);
				}
				this.SetWindowSize(true);
				if (yamp.Default.LastKnownOutDevice > 0)
				{
					if (_outputs.ContainsKey(yamp.Default.LastKnownOutDevice))
					{
						foreach (KeyValuePair<int, string> item in outputCombo.Items)
						{
							if (item.Key == yamp.Default.LastKnownOutDevice)
							{
								outputCombo.SelectedItem = item;
								break;
							}
						}

						SwitchOutBtn_Click(switchOutBtn, null);
					}
				}
				else
				{
					yamp.Default.LastKnownOutDevice = _activeOut.Key;
					yamp.Default.Save();
				}

				//if (!yamp.Default.SeekBarStyle)
				//    SetWindowTheme(this.Handle, "", "");
				//else
				//    Application.EnableVisualStyles();

				this.TopLevel = true;
				this.TopMost = yamp.Default.alwaysOnTop;
				setPinButton();
				this.Location = new Point(yamp.Default.locationMainWindow.X,
					yamp.Default.locationMainWindow.Y);

				ae.Visuals = new Visualizer(ref visualBox, GetPaddedRectangle(visualBox), yamp.Default.VisTime);
				ae.Visuals.SetVisual(yamp.Default.VisualSetting);
				ae.Visuals.Set("backColor", Color.Transparent);
				ae.Visuals.Set("baseColor", yamp.Default.VisColorBottom);
				ae.Visuals.Set("peakColor", yamp.Default.VisColorTop);
				ae.Visuals.Set("holdColor", yamp.Default.VisColorHold);

				this.seekBar.BarColor = yamp.Default.SeekBarColor;
				tagForm.Visible = yamp.Default.showTagForm;

				//if (yamp.Default.UseDigitFont)
				//{
				//  ChangeToResourceFont("Transponder AOE", "Transpond_ttf");
				//}
				//ChangeToResourceFont("Liberation Sans Narrow", "LiberationSansNarrow-Regular");
				this._posDisplay = yamp.Default.TimeDisplayMode;
			}

			setToolTips();
			SetVolumeText();
			volumeKnob.Value = yamp.Default.VolumeLevel;
			opacityKnob.Value = (float)(yamp.Default.opacity - 0.3) * 10F;
			loadingFiles = false;
			Application.DoEvents();
			startup = false;
		}

		private void InitOutputDeviceList()
		{
			_outputs = ae.GetAvailableOutputs();
			_activeOut = ae.GetActiveOutput();
			currDeviceBox.Text = _activeOut.Value;
			outputCombo.Items.Clear();
			int index = 0;
			for (int i = -1; i < _outputs.Count; i++)
			{
				if (_outputs.ContainsKey(i))
				{
					KeyValuePair<int, string> _pair = new KeyValuePair<int, string>(i, _outputs[i]);
					outputCombo.Items.Add(_pair);
					if (_pair.Value == _activeOut.Value)
					{
						index = i;
					}
				}
			}
			outputCombo.ValueMember = "Key";
			outputCombo.DisplayMember = "Value";

			outputCombo.SelectedIndex = index;
		}

		private void ChangeToResourceFont(string fontName, string resourceName)
		{
			List<Control> _controlsToChangeFont = new List<Control>() {
						this.displayTimeLabel, this.mainLabel, this.bitRateLabel, this.sampleRateLabel,
						this.chanLabel, this.volLabel, this.chLabel, this.khzLabel, this.kbpsLabel
					};
			SetTextFontFromResource(fontName, resourceName, _controlsToChangeFont);
		}

		private void SetTextFontFromResource(string font_name, string resource_name)
		{
			List<Control> _controls = FindControlsByFontName(this, font_name);
			SetTextFontFromResource(font_name, resource_name, _controls);
		}

		private void SetTextFontFromResource(string font_name, string resource_name, List<Control> _controls)
		{
			if (_controls.Count > 0)
			{
				GetPrivateFont(font_name, resource_name);

				int _fontIndex = pfc_names.IndexOf(font_name);

				ChangeFontStyle(_controls, pfc.Families[_fontIndex]);
			}
		}

		private void ChangeFontStyle(List<Control> _controls, FontFamily fontFamily)
		{
			foreach (var _control in _controls)
			{
				Font _oldStyle = _control.Font;
				_control.Font = new Font(fontFamily, _oldStyle.Size, _oldStyle.Style);
			}
		}

		private void GetPrivateFont(string font_name, string resource_name)
		{
			if (pfc == null)
			{
				pfc = new PrivateFontCollection();
				pfc_names = new List<String>();
			}

			if (!pfc_names.Contains(font_name))
			{
				Byte[] fontBytes = Azure.YAMP.Properties.Resources.ResourceManager.GetObject(resource_name) as byte[];
				IntPtr fontData = Marshal.AllocCoTaskMem(fontBytes.Length);
				Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);
				pfc.AddMemoryFont(fontData, fontBytes.Length);
				pfc_names.Add(font_name);
				Marshal.FreeCoTaskMem(fontData);
			}
		}

		private List<Control> FindControlsByFontName(Control root, string font_name)
		{
			List<Control> _controls = null;

			if (root != null)
			{
				_controls = new List<Control>();
				foreach (Control child in root.Controls)
				{
					if (child.Font.Name == font_name)
					{
						_controls.Add(child);
					}

					List<Control> found = FindControlsByFontName(child, font_name);
					if (found != null)
					{
						foreach (var item in found)
						{
							_controls.Add(item);
						}
					}
				}
			}

			return _controls;
		}

		private void openVisSettings(object sender, EventArgs e)
		{
			ChangeVisualSettings();
		}

		private Rectangle GetPaddedRectangle(Control control)
		{
			var rect = control.ClientRectangle;
			var pad = control.Padding;
			return new Rectangle(rect.X + pad.Left,
								  rect.Y + pad.Top,
								  rect.Width - (pad.Left + pad.Right),
								  rect.Height - (pad.Top + pad.Bottom));
		}




		#region assistMethods

		[System.Runtime.InteropServices.DllImport("uxtheme", ExactSpelling = true, CharSet = CharSet.Unicode)]
		public extern static Int32 SetWindowTheme(IntPtr hWnd,
					  String textSubAppName, String textSubIdList);

		#endregion assistMethods

		#region mainMethods

		private void InitializeIcons(PictureBox p, Bitmap b, Color transparentColor)
		{
			b.MakeTransparent(transparentColor);
			p.BackgroundImage = ((System.Drawing.Image)b);
		}

		private void mainLabelToDefault()
		{

			System.Reflection.Assembly _assembly = System.Reflection.Assembly.GetExecutingAssembly();
			System.Diagnostics.FileVersionInfo _info = System.Diagnostics.FileVersionInfo.GetVersionInfo(_assembly.Location);
			mainLabel.Text = String.Copy(_info.ProductName);
		}

		private void tagWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			((Form)sender).Hide();
			e.Cancel = true;
			if (System.IO.File.Exists(".\\yamp.exe.config"))
			{
				yamp.Default.showTagForm = false;
				yamp.Default.Save();
			}
		}

		private void setToolTips()
		{
			ToolTip tp = new ToolTip();
			tp.SetToolTip(visualBox, "Visualiser");
			tp.SetToolTip(plistBtn, "PlayList Window");
			tp.SetToolTip(visBtn, "Visuals Settings");
			tp.SetToolTip(tagfmBtn, "Tag Info Window");
			tp.SetToolTip(volumeKnob, "Adjust volume.");
			tp.SetToolTip(opacityKnob, "Adjust opacity betweem 30% and 100%.");
			helpPlayList = ("To move up (down) selected item, press '+' or 'W'  ('-' or 'S') button.");
			helpSearchBox = ("Type a substring to search, and press F3 to jump to the next occurrence.");
		}

		private void ShutDown(object sender, FormClosingEventArgs fce)
		{
			try
			{
				if ((playListView.Items.Count > 0) && (SettingsExist))
				{
					yamp.Default.lastPlayedIndex = (playListView.SelectedIndices.Count > 0 ? playListView.SelectedIndices[0] : 0);
					yamp.Default.Save();
					this.savePlayList(startDir + yamp.Default.lastPlayedList);
				}
			}
			catch (Exception x)
			{
				Azure.LibCollection.CS.MBoxHelper.ShowWarnMsg(x, "Warning!");
			}
			finally
			{
				ae.Dispose();
				Application.Exit();
			}
		}


		private void MouseMovementHandler(object sender, MouseEventArgs maus)
		{
			if (dragging)
			{
				Point currentScreenPos = PointToScreen(maus.Location);
				Location = new Point
					(currentScreenPos.X - offset.X,
					 currentScreenPos.Y - offset.Y);
			}
			if (seeking)
			{
				seekBar.Value = CalcSeek((double)maus.X);
			}
		}

		private int CalcSeek(double mouseX)
		{
			double seek = (mouseX / (double)seekBar.Width);
			return (int)((seekSet = seek > 1 ? 1 : seek < 0 ? 0 : seek) * seekBar.Maximum);
		}

		private void startDragging(object sender, MouseEventArgs me)
		{
			if (me.Button == MouseButtons.Left)
			{
				dragging = true;
				offset.X = me.X;
				offset.Y = me.Y;
			}
		}

		private void endDragging(object sender, MouseEventArgs me)
		{
			if (me.Button == MouseButtons.Left)
			{
				dragging = false;
				if (SettingsExist)
				{
					yamp.Default.locationMainWindow = this.Location;
					yamp.Default.Save();
				}
			}
		}

		#endregion mainMethods

		private void MainWindow_Shown(object sender, EventArgs e)
		{
			try
			{
				if (File.Exists(startDir + yamp.Default.lastPlayedList))
				{
					loadDefaultPlayList(startDir + yamp.Default.lastPlayedList);
					playList.ChangePlayListSelection(yamp.Default.lastPlayedIndex);
				}
			}
			catch (Exception)
			{
				File.Delete(yamp.Default.lastPlayedList);
			}
		}

		private void plistBtn_Click(object sender, EventArgs e)
		{
			SetWindowSize();
		}

		private void SetWindowSize(bool init = false)
		{
			if (this.SettingsExist)
			{
				if (yamp.Default.showPLEdit && !init)
				{
					minimizeWnd();
				}
				else
				{
					maximizeWnd();
				}
			}
			else
			{
				this.Size = (this.Size == this.MinimumSize ? this.MaximumSize : this.MinimumSize);
			}
		}

		private void maximizeWnd()
		{
			if (yamp.Default.prevSizeMainWindow.IsEmpty)
			{
				yamp.Default.sizeMainWindow = this.MaximumSize;
			}
			else
			{
				yamp.Default.sizeMainWindow = yamp.Default.prevSizeMainWindow;
			}

			yamp.Default.prevSizeMainWindow = this.Size;
			yamp.Default.showPLEdit = true;
			yamp.Default.Save();
			this.MinimumSize = this.sizeWithPlEditMin;
			this.MaximumSize = this.sizeWithPlEditMax;
			this.SizeGripStyle = SizeGripStyle.Show;
			this.statusStrip.SizingGrip = true;
			this.Size = yamp.Default.sizeMainWindow;
			searchBox.Visible = true;
		}

		private void minimizeWnd()
		{
			yamp.Default.prevSizeMainWindow = this.Size;
			yamp.Default.sizeMainWindow = this.sizeWithNoPlEdit;
			yamp.Default.showPLEdit = false;
			yamp.Default.Save();
			this.MinimumSize = this.sizeWithNoPlEdit;
			this.MaximumSize = this.sizeWithNoPlEdit;
			this.SizeGripStyle = SizeGripStyle.Hide;
			this.statusStrip.SizingGrip = false;
			this.Size = yamp.Default.sizeMainWindow;
			searchBox.Visible = false;

		}

		private void savePlayList(string plPath)
		{
			playList.SaveList(plPath, PlayListFormat.yspl);
		}

		private void playList_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		private void playList_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.Delete:
					{
						RemoveSelected();
						//e.SuppressKeyPress = true;
					}
					break;
				/*
                case Keys.S:
                    {
                        e.Handled = true;
                        SwapSelected(false);
                    }
                    break;
                case Keys.Subtract:
                    SwapSelected(false);
                    break;
                case Keys.W:
                    {
                        e.Handled = true;
                        SwapSelected(true);
                    }
                    break;
                case Keys.Add:
                    SwapSelected(true);
                    break;
                */
				case Keys.X:
					{
						playList_Prev(true);
						e.SuppressKeyPress = true;
					}
					break;
				case Keys.C:
				case Keys.Enter:
					{
						playList_DoubleClick(playListView, null);
						e.SuppressKeyPress = true;
					}
					break;
				case Keys.V:
					{
						pauseBtn_Click(pauseBtn, null);
						e.SuppressKeyPress = true;
					}
					break;
				case Keys.B:
					{
						stopBtn_Click(stopBtn, null);
						e.SuppressKeyPress = true;
					}
					break;
				case Keys.N:
					{
						playList_Next(true);
						e.SuppressKeyPress = true;
					}
					break;
				case Keys.J:
				case Keys.F3:
					{
						searchBox.Focus();
						//e.Handled = true;
						e.SuppressKeyPress = true;
					}
					break;
				default:
					break;
			}
		}



		public void playList_DoubleClick(object sender, EventArgs e)
		{
			/*
            if (playListView.SelectedIndices.Count > 0)
            {*/
			currentIndex = playList.FindSelected(); //playListView.SelectedIndices[0];
			if (currentIndex < 0 || (selectedIndex > -1 && selectedIndex != currentIndex))
			{
				currentIndex = selectedIndex;
			}
			this.PlayNewFile(playList.list[currentIndex].FilePath);
			playList.SelectNext(currentIndex);
			//}
		}

		public void playList_Next(bool rePlay)
		{
			if (string.IsNullOrEmpty(currentFile) && currentIndex < selectedIndex)
			{
				currentIndex = selectedIndex;
			}
			if (playList.list != null)
			{
				if (++currentIndex < playList.list.Count)
				{
					this.PlayNewFile(playList.list[currentIndex].FilePath);
					playList.SelectNext(true);
					//playList.SelectedIndex = currentIndex;
					playList.ChangePlayListSelection(currentIndex);
				}
				else
				{
					if (rePlay)
					{
						playList_First();
						playList.SelectNext(0);
					}
				}
			}
			else
			{
				stopBtn_Click(stopBtn, null);
			}
		}

		public void playList_Prev(bool rePlay)
		{

			if (string.IsNullOrEmpty(currentFile) && currentIndex < selectedIndex)
			{
				currentIndex = selectedIndex;
			}
			if (--currentIndex >= 0)
			{
				currentIndex = playList.SelectNext(false);
				this.PlayNewFile(playList.list[currentIndex].FilePath);
				//playList.SelectedIndex = currentIndex;
				playList.ChangePlayListSelection(currentIndex);
			}
			else
			{
				if (rePlay)
				{
					playList_Last();
					currentIndex = playList.SelectNext(playList.list.Count - 1);
				}
			}

		}

		public void playList_First()
		{
			currentIndex = 0;
			if (playList.list.Count > 0)
			{
				this.PlayNewFile(playList.list[currentIndex].FilePath);
				//playList.SelectedIndex = currentIndex;
				playList.SelectNext(currentIndex);
				playList.ChangePlayListSelection(currentIndex);
			}
		}

		public void playList_Last()
		{
			if (playList.list.Count > 0)
			{
				currentIndex = playList.list.Count - 1;
				this.PlayNewFile(playList.list[currentIndex].FilePath);
				//playList.SelectedIndex = currentIndex;
				playList.SelectNext(currentIndex);
				playList.ChangePlayListSelection(currentIndex);
			}
		}

		private void playListWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
			if (this.SettingsExist)
			{
				yamp.Default.showPLEdit = false;
				yamp.Default.Save();
			}
		}

		/*
        private void SwapSelected(bool p)
        {
            if (playList.Items.Count > 0)
            {
                int i = 1;
                if (p)
                {
                    i *= (-1);
                }
                int Index = playList.SelectedIndex;          //Selected Index
                object SwapText = playList.SelectedItem;      //Selected Item
                MediaItem SwapMedia = loaded[Index];
                if (Index > -1)
                {               //If something is selected...
                    playList.Items.RemoveAt(Index);                 //Remove it
                    loaded.RemoveAt(Index);
                    if (Index + i > playList.Items.Count)
                    {
                        Index = 0;
                    }
                    else
                    {
                        if (Index + i < 0)
                        {
                            Index = playList.Items.Count;
                        }
                        else
                        {
                            Index += i;
                        }
                    }
                    playList.Items.Insert(Index, SwapText);        //Add it back in one spot up
                    loaded.Insert(Index, SwapMedia);
                    playList.SelectedItem = SwapText;                   //Keep this item selected

                    //correct the index of the currently played file
                    for (int j = 0; j < loaded.Count; j++)
                    {
                        if (loaded[j].getFilePath == currentFile)
                        {
                            currentIndex = j;
                            break;
                        }
                    }

                }
            }
        }
        */

		private void RemoveSelected()
		{
			if (playListView.SelectedIndices.Count > 0)
			{
				Array _selectedI = Array.CreateInstance(typeof(int), playListView.SelectedIndices.Count);
				playListView.SelectedIndices.CopyTo(_selectedI, 0);
				Array.Reverse(_selectedI);

				foreach (int i in _selectedI)
				{
					//searchBox.AutoCompleteCustomSource.Remove(playList.list[i].ArtistTitleText);
					playList.list.RemoveAt(i);
				}
				//playList.RebuildList();
				playList.RebuildListView(PlaylistEditMode.RemoveFiles);
			}
		}

		private void ResetList()
		{
			//playList.Items.Clear();
			//searchBox.AutoCompleteCustomSource.Clear();
			playList.Release();
			currentIndex = -1;
			//loaded = null;
			/*
            playListView2.Items.Clear();
            loaded.Clear();
            */
		}

		private void fileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog loadFiles = new OpenFileDialog())
			{
				loadFiles.Filter = this.openMedia.Filter;
				loadFiles.Multiselect = true;
				if (loadFiles.ShowDialog() == DialogResult.OK)
				{
					playList.LoadFromDir(loadFiles.FileNames, true, 0, PlaylistEditMode.AddFiles);
					/*foreach (var mItem in playList.list)
					{
						searchBox.AutoCompleteCustomSource.Add(mItem.ArtistTitleText);
					}*/
				}
			}
		}

		private void playList_MouseEnter(object sender, EventArgs e)
		{
			hintLabel.Text = helpPlayList;
			hintLabel.Visible = true;
		}

		private void playList_MouseLeave(object sender, EventArgs e)
		{
			HideHintLabel();
		}

		private void HideHintLabel()
		{
			hintLabel.Text = "";
			hintLabel.Visible = false;
		}

		private void selectedToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (playListView.SelectedItems.Count > 0)
			{
				RemoveSelected();
			}
		}

		private void allToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ResetList();
		}

		public void LoadSingleFile(string file)
		{
			ResetList();
			List<string> _file = new List<string> { file };
			playList.BuildPlayList(_file, ref playList.list);
		}

		private void searchBox_Click(object sender, EventArgs e)
		{
			searchBox.SelectAll();
		}

		private void searchBox_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyCode)
			{
				case Keys.F3:
					searchFile();
					break;
				case Keys.Enter:
					playListView.Focus();
					playList_DoubleClick(playListView, null);
					break;
				default:
					break;
			}
		}

		private void playListWindow_ResizeEnd(object sender, EventArgs e)
		{
			if (this.SettingsExist)
			{
				yamp.Default.sizeMainWindow = this.Size;
				yamp.Default.Save();
			}
		}

		private void searchBox_TextChanged(object sender, EventArgs e)
		{
			if (searchBox.Text.Length > 0)
			{
				searchIndex = 0;
				searchFile();
			}
		}

		private void searchBox_MouseEnter(object sender, EventArgs e)
		{
			hintLabel.Text = helpSearchBox;
			hintLabel.Visible = true;
		}

		private void searchBox_MouseLeave(object sender, EventArgs e)
		{
			HideHintLabel();
		}

		private void playList_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}

		private void saveToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "YAMP Simplified PlayList|*.yspl|M3U PlayList|*.m3u;*.m3u8";
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				PlayListFormat format;
				FileInfo fInfo = new FileInfo(sfd.FileName);
				switch (fInfo.Extension)
				{
					case ".m3u":
						format = PlayListFormat.m3u;
						break;
					case ".m3u8":
						format = PlayListFormat.m3u8;
						break;
					case ".yspl":
						format = PlayListFormat.yspl;
						break;
					default:
						format = PlayListFormat.unsupported;
						break;
				}
				//this.savePlayList(sfd.FileName, format, loaded);
				PlayList.SaveList(sfd.FileName, format, playList.list.Select(x => x.FilePath));
			}
		}



		private void loadToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog ofd = new OpenFileDialog())
			{
				ofd.Filter = "YAMP Simplified PlayList|*.yspl|M3U PlayList|*.m3u;*.m3u8";
				if (ofd.ShowDialog() == DialogResult.OK)
				{
					FileInfo fInfo = new FileInfo(ofd.FileName);
					PlayListFormat format = PlayListFormat.unsupported;
					switch (fInfo.Extension)
					{
						case ".yspl":
							format = PlayListFormat.yspl;
							break;
						case ".m3u":
							format = PlayListFormat.m3u;
							break;
						case ".m3u8":
							format = PlayListFormat.m3u8;
							break;
						default:
							break;
					}
					//this.loadPlayList(ofd.FileName, ae.getSupFilter, format, ref loaded, playListView2.Items);
					//this.loadPlayList2(ofd.FileName, ae.getSupFilter, format, ref playList.list);
					playList.LoadList(ofd.FileName, ae.SupportedFileFilter, format, ref playList.list);
				}
			}
		}

		/*
        private void loadPlayList2(string path, string supFilters, PlayListFormat format, ref List<MediaItem> loaded)
        {
            playList.LoadList(path, supFilters, format, ref loaded);
            //playList.RebuildListView();
        }
        */
		internal void loadDefaultPlayList(string plPath)
		{
			//this.loadPlayList(plPath, ae.getSupFilter, PlayListFormat.yspl, ref loaded, playListView2.Items);
			//this.loadPlayList2(plPath, ae.getSupFilter, PlayListFormat.yspl, ref playList.list);
			playList.LoadList(plPath, ae.SupportedFileFilter, PlayListFormat.yspl, ref playList.list);
		}

		public bool LoadingFiles
		{
			get { return loadingFiles; }
		}

		private void PlayNewFile(string openThis)
		{
			currentFile = openThis;
			coverBox.Image = null;
			ae.Stop();
			//ae.Wipe();
			ae.Visuals.Set("drawFull", false);
			ae.PlayInitFile(openThis.ToLower());
			if (eq == null)
			{
				eq = new EqualiserEngine(ae.stream, 60f, 170f,
					310f, 600f, 1000f, 3000f, 6000f, 12000f, 14000f, 16000f);
			}
			else
			{
				eq.AttachEQ(ae.stream);
			}

			UpdateEqLevels();
			setInfoControls();
			seekBar.Maximum = (int)Math.Round(ae.TotalTime, MidpointRounding.AwayFromZero);
			drawFileInfo(openThis);
			playStarted = true;
			mainTimer.Enabled = true;
			//scrollTimer.Enabled = true;
			mainLabel.Switch(true);
			InitTagsForDisplay(openThis);
		}

		private bool setInfoControls()
		{
			try
			{
				PlaylistEntry _current = playList.list[currentIndex];
				_current.CheckIfRefreshNeeded();
				var extra = _current.ExtraAttributes;
				sampleRateLabel.Text = (Convert.ToDouble(extra[AudioFile.ExtraAttribute.SampleRate]) / 1000).ToString("F1", System.Globalization.CultureInfo.InvariantCulture);
				bitRateLabel.Text = extra[AudioFile.ExtraAttribute.BitRate];
				chanLabel.Text = extra[AudioFile.ExtraAttribute.Channels];
				artistBox.Text = _current.Artist;
				titleBox.Text = _current.Title;
				albumBox.Text = _current.Album;

				mainLabel.Text = String.Format(" *** {0} - {1}", _current.Artist, _current.Title);
				mainLabel.DuplicateIf();
				coverBox.Image = _current.AlbumArtwork;

				playList.UpdateInfo(currentIndex, _current.Title, _current.Artist, _current.DurationText);

				return true;
			}
			catch
			{
				return false;
			}
		}

		private void drawFileInfo(string openThis)
		{
			fileBox.Text = Path.GetFileName(openThis);

		}

		private void InitTagsForDisplay(string openThis)
		{
			tagForm.LoadTags(openThis);
		}

		private void searchFile()
		{
			if (playList.list.Count > 0)
			{
				int i;
				for (i = searchIndex; i < playList.list.Count; ++i)
				{
					/*
					if (playList.list[i].ArtistTitleText.ToLower().Contains(searchBox.Text.ToLower()))
					*/
					bool match = true;
					PlaylistEntry entry = playList.list[i];
					string[] filters = searchBox.Text.ToLower().Split(' ');
					foreach (string filter in filters)
					{
						if (!entry.Artist.ToLower().Contains(filter) && !entry.Title.ToLower().Contains(filter))
						{
							match = false;
							break;
						}
					}
					if(match)
					{
						//searchIndex = playList.SelectedIndex = i;
						playList.ChangePlayListSelection(i);
						searchBox.Focus();
						searchIndex = ++i;
						break;
					}
					else
					{
						//playList.SelectedIndex = -1;
						playListView.SelectedIndices.Clear();
					}

				}
				if (i >= playList.list.Count)
				{
					i = searchIndex = 0;
				}
			}
		}

		private void StartSeeking(object sender, MouseEventArgs me)
		{
			if ((me.Button == MouseButtons.Left) && (ae.CurrentState != AudioEngine.PlaybackState.Stopped))
			{
				seeking = true;
				seekBar.Value = CalcSeek((double)me.X);
			}
		}

		private void EndSeeking(object sender, MouseEventArgs me)
		{
			if ((me.Button == MouseButtons.Left) && (seeking))
			{
				//ae.setPositionSeconds(Math.Round(ae.TotalTime) * seekSet);
				ae.PositionInBytes = (long)((double)ae.LengthInBytes * seekSet);
				seeking = false;
			}
		}

		private void openBtn_Click(object sender, EventArgs e)
		{
			mainTimer.Enabled = false;
			//scrollTimer.Enabled = false;
			if (openMedia.ShowDialog() == DialogResult.OK)
			{
				currentFile = openMedia.FileName;
				LoadSingleFile(currentFile);
				PlayNewFile(currentFile);
			}
			mainTimer.Enabled = true;
			//scrollTimer.Enabled = true;
		}

		private void mainTimer_Tick(object sender, EventArgs e)
		{
			switch (ae.CurrentState)
			{
				case AudioEngine.PlaybackState.Playing:
					{
						switch (_posDisplay)
						{
							case 1: displayTimeLabel.Text = ae.ElapsedTimeString; break;
							case -1: displayTimeLabel.Text = "-" + ae.RemainingTimeString; break;
							//case 0: break;
							default:
								break;
						}
						if (!seeking)
						{
							seekBar.Value = (int)Math.Floor(ae.ElapsedTime);
						}
					}
					break;
				case AudioEngine.PlaybackState.Stopped:
					{
						mainTimer.Enabled = false;
						NextPlayState();
					}
					break;
			}
		}

		private void NextPlayState()
		{
			if (playStarted)
			{
				try
				{
					switch (repeat)
					{
						case repeatOption.none:
							NoRepeat();
							break;
						case repeatOption.all:
							RepeatAll();
							break;
						case repeatOption.actual:
							PlayCurrentFile();
							break;
					}
					if (playStarted)
					{
						mainTimer.Enabled = true;
					}
				}
				catch (Exception x)
				{
					playStarted = false;
					MBoxHelper.ShowErrorMsg(x, "Error occured via playlist!");
				}
			}
		}

		private void PlayCurrentFile()
		{
			ae.Play();
		}

		private void RepeatAll()
		{
			if (playList.list.Count == 1)
			{
				PlayCurrentFile();
			}
			else
			{
				if (playList.list.Count == this.currentIndex)
				{
					playList_First();
				}
				else
				{
					playList_Next(false);
				}
			}
		}

		private void NoRepeat()
		{
			if ((playList.list.Count > 1) && (playList.list.Count > currentIndex))
			{
				playList_Next(false);
			}
			else
			{
				if (playStarted)
				{
					stopBtn_Click(stopBtn, null);
				}
			}
		}

		private void closeBtn_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void stopBtn_Click(object sender, EventArgs e)
		{
			playStarted = false;
			ae.Stop();
			seekBar.Value = seekBar.Minimum;
			sampleRateLabel.Text = bitRateLabel.Text = displayTimeLabel.Text =
				chanLabel.Text = artistBox.Text = titleBox.Text = albumBox.Text = "";
			visualBox.Image = coverBox.Image = null;
			mainTimer.Enabled = false;
			mainLabel.Switch(false);
			mainLabelToDefault();
		}

		private void pauseBtn_Click(object sender, EventArgs e)
		{
			ae.Pause();
		}

		private void playBtn_Click(object sender, EventArgs e)
		{
			currentIndex = playList.FindSelected();
			if (currentFile != null /*&& playList.list.Where(x => x.FilePath == currentFile).Count() > 0*/)
			{
				PlayNewFile(currentFile);
				playList.ChangePlayListSelection(currentIndex);
			}
			else
			{
				if (playList.list.Count > 0)
				{
					if (currentIndex != -1)
					{
						PlayNewFile(playList.list[currentIndex].FilePath);
						playList.ChangePlayListSelection(currentIndex);
						playList.SelectNext(currentIndex);
					}
					else
					{
						playList_DoubleClick(playListView, null);
					}
				}
			}
		}

		private void ModifyRepeatOption()
		{
			switch (repeat)
			{
				case repeatOption.none:
					{

						repeatCheckBox.ForeColor = Color.FromArgb(128, 204, 128);
						repeat = repeatOption.actual;
						repeatCheckBox.Text = "Repeat one";
					}
					break;
				case repeatOption.actual:
					{
						repeatCheckBox.ForeColor = Color.Goldenrod;
						repeat = repeatOption.all;
						repeatCheckBox.Text = "Repeat all";
					}
					break;
				case repeatOption.all:
					{
						repeatCheckBox.ForeColor = Color.FromArgb(192, 204, 192);
						repeat = repeatOption.none;
						repeatCheckBox.Text = "Repeat off";
					}
					break;
			}
		}

		private void nextBtn_Click(object sender, EventArgs e)
		{
			playList_Next(true);
		}

		private void prevBtn_Click(object sender, EventArgs e)
		{
			playList_Prev(true);
		}

		public bool SettingsExist
		{
			get
			{
				return (File.Exists(".\\yamp.settings") | File.Exists(".\\" + _asmName + ".exe.config"));
			}
		}

		private void tagfmBtn_Click(object sender, EventArgs e)
		{
			tagForm.Visible = !tagForm.Visible;
			//if (getSettingsExist)
			//{
			//    yamp.Default.showTagForm = tagForm.Visible;
			//    yamp.Default.Save();
			//}
		}

		private void visualBox_Click(object sender, EventArgs e)
		{
			ae.Visuals.SetVisual(ae.Visuals.GetVSType + 1);
			if (SettingsExist)
			{
				yamp.Default.VisualSetting = (byte)ae.Visuals.GetVSType;
				yamp.Default.Save();
			}
		}

		public string EngineSuppExts()
		{
			return ae.SupportedFileFilter;
		}

		private void minBtn_Click(object sender, EventArgs e)
		{
			this.WindowState = FormWindowState.Minimized;
		}

		private void topSwitchBtn_Click(object sender, EventArgs e)
		{
			SwitchTopMost();
		}

		private void SwitchTopMost()
		{
			this.TopMost = (!this.TopMost);
			if (SettingsExist)
			{
				yamp.Default.alwaysOnTop = this.TopMost;
				yamp.Default.Save();
			}
			setPinButton();
		}

		private void setPinButton()
		{
			switch (this.TopMost)
			{
				case true: pinMenuItem.Image = Azure.YAMP.Properties.Resources.pinOn; break;
				case false: pinMenuItem.Image = Azure.YAMP.Properties.Resources.pinOff; break;
				default:
					break;
			}
		}

		private void ChangeVisualSettings()
		{
			visSetWnd visSelect = new visSetWnd((byte)ae.Visuals.GetVSType, ae.Visuals.GetTimerInterval(),
				new Color[] {ae.Visuals.GetColor("base"), ae.Visuals.GetColor("peak"),
					ae.Visuals.GetColor("hold"), this.seekBar.BarColor}, yamp.Default.UseDigitFont);
			if (visSelect.ShowDialog() == DialogResult.OK)
			{
				ae.Visuals.SetVisual(visSelect.visMode);
				ae.Visuals.Set("timerInterval", visSelect.visTime);
				ae.Visuals.Set("baseColor", visSelect.Controls["btmColorBtn"].BackColor);
				ae.Visuals.Set("peakColor", visSelect.Controls["topColorBtn"].BackColor);
				ae.Visuals.Set("holdColor", visSelect.Controls["peakColorBtn"].BackColor);
				this.seekBar.BarColor = visSelect.Controls["seekColorBtn"].BackColor;

				bool useDigitFont = (visSelect.Controls["useDigitFontCB"] as CheckBox).Checked;

				if (yamp.Default.UseDigitFont != useDigitFont)
				{
					ChangeToResourceFont("Transponder AOE", "Transpond_ttf");
				}


				if (this.SettingsExist)
				{
					yamp.Default.VisualSetting = visSelect.visMode;
					yamp.Default.VisTime = ae.Visuals.GetTimerInterval();
					yamp.Default.VisColorBottom = ae.Visuals.GetColor("base");
					yamp.Default.VisColorTop = ae.Visuals.GetColor("peak");
					yamp.Default.VisColorHold = ae.Visuals.GetColor("hold");
					yamp.Default.SeekBarColor = this.seekBar.BarColor;
					yamp.Default.UseDigitFont = useDigitFont;
					yamp.Default.Save();
				}
			}
		}

		private void SetVolume(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e)
		{
			SetAudioVolume(volumeKnob.Value);
		}

		private void SetAudioVolume(float value)
		{
			ae.ChangeChannelVolume(value);
			SetVolumeText();
			if (!startup && SettingsExist)
			{
				yamp.Default.VolumeLevel = ae.ChannelVolume;
				yamp.Default.Save();
			}
		}

		private void SetVolumeText()
		{
			volLabel.Text = ((int)Math.Round(ae.ChannelVolume * 100, MidpointRounding.AwayFromZero)).ToString() + "%";
		}

		private void SetOpacity(object sender, LBSoft.IndustrialCtrls.Knobs.LBKnobEventArgs e)
		{
			tagForm.Opacity = this.Opacity = 0.3F + ((sender as LBSoft.IndustrialCtrls.Knobs.LBKnob).Value / 10F);
			if (!startup && SettingsExist)
			{
				yamp.Default.opacity = (float)this.Opacity;
				yamp.Default.Save();
			}
		}

		private void visualBox_Paint(object sender, PaintEventArgs e)
		{
			MakeTransparentVisual();

		}

		private void MakeTransparentVisual()
		{
			Image image = (visualBox.Image != null ? new Bitmap(visualBox.Image.Size.Width, visualBox.Image.Size.Height, PixelFormat.Format32bppArgb)
				: new Bitmap(visualBox.ClientRectangle.Width, visualBox.ClientRectangle.Height, PixelFormat.Format32bppArgb));
			using (var g = Graphics.FromImage(image))
			{
				g.Clear(Color.Transparent);
				g.DrawImage(image, 0, 0);
			}
		}

		private void visualBox_Validated(object sender, EventArgs e)
		{
			MakeTransparentVisual();
		}

		private void repeatCheckBox_CheckStateChanged(object sender, EventArgs e)
		{
			ModifyRepeatOption();
		}

		private void pinMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			SwitchTopMost();
		}

		private void UpdateEqLevels()
		{
			if (eq != null)
			{
				eq.UpdateFX(0, (float)freqBar0.Value);
				eq.UpdateFX(1, (float)freqBar1.Value);
				eq.UpdateFX(2, (float)freqBar2.Value);
				eq.UpdateFX(3, (float)freqBar3.Value);
				eq.UpdateFX(4, (float)freqBar4.Value);
				eq.UpdateFX(5, (float)freqBar5.Value);
				eq.UpdateFX(6, (float)freqBar6.Value);
				eq.UpdateFX(7, (float)freqBar7.Value);
				eq.UpdateFX(8, (float)freqBar8.Value);
				eq.UpdateFX(9, (float)freqBar9.Value);
			}
		}

		private void freqBar_ValueChanged(object sender, EventArgs e)
		{
			int _band = int.Parse((sender as FusionTrackBar).Name.Substring(7, 1));
			float _gain = (float)(sender as FusionTrackBar).Value;
			if (eq != null)
			{
				eq.UpdateFX(_band, _gain);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (eq != null)
			{
				eq.Reset(-1, 0f);
			}

			freqBar0.Value = 0;
			freqBar1.Value = 0;
			freqBar2.Value = 0;
			freqBar3.Value = 0;
			freqBar4.Value = 0;
			freqBar5.Value = 0;
			freqBar6.Value = 0;
			freqBar7.Value = 0;
			freqBar8.Value = 0;
			freqBar9.Value = 0;
		}

		private void freqLabel_Click(object sender, EventArgs e)
		{
			string _controlName = (sender as Label).Name.Replace("Label", "Bar");
			FusionTrackBar _ft = (eqPanel.Controls.Find(_controlName, false)[0]) as FusionTrackBar;
			_ft.Value = 0;
		}

		private void visBtn_Click(object sender, EventArgs e)
		{
			ChangeVisualSettings();
		}

		private void selectedOnlyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SelectDirectory(false);
		}

		private void SelectDirectory(bool recursive)
		{
			using (FolderBrowserDialog chooseFolder = new FolderBrowserDialog())
			{
				if (this.SettingsExist)
				{
					chooseFolder.RootFolder = System.Environment.SpecialFolder.MyComputer;
					chooseFolder.SelectedPath = yamp.Default.lastFolder;
					chooseFolder.RootFolder = System.Environment.SpecialFolder.Desktop;

				}
				if (chooseFolder.ShowDialog() == DialogResult.OK)
				{
					var files = playList.LoadFromDir(new string[] { chooseFolder.SelectedPath }, recursive, 0, PlaylistEditMode.AddFiles);
					if (this.SettingsExist)
					{
						yamp.Default.lastFolder = chooseFolder.SelectedPath;
						yamp.Default.Save();
					}
				}
			}
		}

		private void allSubFoldersToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SelectDirectory(true);
		}

		private void selectedOnlyToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			SelectDirectory(false);
		}

		internal void PlayNewFileOnDoubleClick()
		{
			this.PlayNewFile(playList.list[currentIndex].FilePath);
		}

		public void playList_DragDrop(object sender, DragEventArgs e)
		{
			playList.LoadFromDir((string[])e.Data.GetData(DataFormats.FileDrop), true, 0, PlaylistEditMode.AddFiles);
		}

		private void SwitchOutBtn_Click(object sender, EventArgs e)
		{
			ae.SwitchOutputDevice(((KeyValuePair<int, string>)outputCombo.SelectedItem).Key);
			InitOutputDeviceList();
			yamp.Default.LastKnownOutDevice = _activeOut.Key;
			yamp.Default.Save();
		}

		private void refreshOutBtn_Click(object sender, EventArgs e)
		{
			InitOutputDeviceList();
		}

		private void displayTimeLabel_Click(object sender, EventArgs e)
		{
			_posDisplay *= -1;
			yamp.Default.TimeDisplayMode = _posDisplay;
			yamp.Default.Save();
		}

		/*public void loadPlayList(string FileName, string supExt, PlayListFormat format, ref List<MediaItem> lContainer, ListView.ListViewItemCollection lviCollection, ref bool interrupt)
        {
            StreamReader srr = File.OpenText(FileName);
            Regex headText = null;
            switch (format)
            {
                case PlayListFormat.yspl:
                    headText = new Regex("#YAMP Simplified PlayList#");
                    break;
                case PlayListFormat.m3u:
                case PlayListFormat.m3u8:
                    headText = new Regex("#*M3U");
                    break;
                case PlayListFormat.unsupported:
                    break;
            }
            if (headText.IsMatch(srr.ReadLine()))
            {
                lContainer.Clear();
                //lCollection.Clear();
                lviCollection.Clear();

                string readIn;
                if (format == PlayListFormat.yspl)
                {
                    readIn = srr.ReadLine();
                }
                //bool ok = true;
                do
                {
                    readIn = srr.ReadLine();
                    if (readIn != null)
                    {
                        if (!readIn.Contains("#End ... Written "))
                        {
                            if (!readIn.Contains("#"))
                            {
                                if (readIn.StartsWith(@"\"))
                                {
                                    readIn = Environment.GetEnvironmentVariable("%SYSTEMDRIVE%",
                                        EnvironmentVariableTarget.Machine) + readIn;
                                }
                                string[] _file = new string[] { readIn };
                                LoadMultiFiles(ref _file, supExt, ref lContainer, lviCollection, ref interrupt);
                            }
                        }
                        else
                            break;
                    }
                    else
                    {
                        break;
                        //ok = false;
                    }
                }
                while (true); //(ok);
            }
            else
            {
                MBoxHelper.ShowWarnMsg("Not a valid YAMP PlayList!",
                    "Error on playlist load!");
            }
            srr.Close();
        }

        public bool LoadMultiFiles(ref string[] files, string supExt, ref List<MediaItem> Container, ListView.ListViewItemCollection ListViewCollection, ref bool interrupt)
        {
            try
            {
                if (files != null)
                {
                    foreach (string aElement in files)
                    {
                        if (!interrupt)
                        {
                            if (File.Exists(aElement))
                            {
                                string fileEnding = new FileInfo(aElement.ToLower()).Extension;
                                if (supExt.Contains(fileEnding))
                                {
                                    MediaItem mItem = MediaItem.Factory(aElement);

                                    if (TagReader.fileHasValidTags(mItem.getFilePath))
                                    {
                                        Container.Add(mItem);
                                        //Collection.Add(mItem.getFileName);

                                        string _performers = mItem.MetaInfo("get", "Performers").ToString();
                                        string _title = mItem.MetaInfo("get", "Title").ToString();
                                        string _album = mItem.MetaInfo("get", "Album").ToString();
                                        string _fileMeta = mItem.MetaInfo("get", "FileMeta").ToString();
                                        string _duration = mItem.MetaInfo("get", "Duration").ToString();
                                        string _fileName = mItem.getFileName;
                                        string _filePath = mItem.getFilePath;

                                        string[] _lviData =
                                        {
                                            (ListViewCollection.Count +1).ToString(),
                                            _fileMeta,
                                            _duration,
                                            _fileName,
                                            _filePath,
                                            _performers,
                                            _title,
                                            _album
                                        };

                                        ListViewItem _lvi = new ListViewItem(_lviData);
                                        ListViewCollection.Add(_lvi);
                                    }
                                }
                            }
                            Application.DoEvents();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal void LoadMultiFiles(ref Array files, string p, ref List<MediaItem> loaded, ListView.ListViewItemCollection lviCollection, ref bool interrupt)
        {
            string[] _files = (string[])files;
            LoadMultiFiles(ref _files, p, ref loaded, lviCollection, ref interrupt);
        }
         */
	}
}
