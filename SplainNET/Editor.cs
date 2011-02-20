using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using libZoi;

namespace SplainNET
{
    public partial class Editor : Form
    {
        ChannelReader chRead;
        string file;
        int edit_index;

        public Editor(string FileName)
        {
            InitializeComponent();
            file = FileName;
            LoadChannels(file);
        }

        private void LoadChannels(string FileName)
        {
            chRead = new ChannelReader(FileName);
            channelList.Items.Clear();
            foreach (string i in chRead.Names)
                channelList.Items.Add(i);
            urlList.Items.Clear();
            foreach (string i in chRead.Links)
                urlList.Items.Add(i);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if((urlBox.Text.Length>8)&&(channelBox.Text.Length>0))
            {
                if (edit_index > 0)
                {

                    channelList.Items[edit_index] = channelBox.Text;
                    urlList.Items[edit_index] = urlBox.Text;
                    SaveAllChannels();
                }
                else
                {
                    StreamWriter SWriter = File.AppendText(chRead.StreamFile);
                    SWriter.WriteLine(channelBox.Text);
                    SWriter.WriteLine(urlBox.Text);
                    //SWriter.WriteLine();
                    SWriter.Close();
                }
            }
            MessageBox.Show("Channels Saved");
            chRead = null;
            LoadChannels(file);

           
        }

        private void SaveAllChannels()
        {
            StreamWriter SWriter = File.CreateText(chRead.StreamFile);
            for (int i = 0; i < urlList.Items.Count; i++)
            {
                SWriter.WriteLine(channelList.Items[i]);
                SWriter.WriteLine(urlList.Items[i]);
            }
            SWriter.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void channelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            edit_index = channelList.SelectedIndex;
            EditSelectedChannel();
        }

        private void EditSelectedChannel()
        {
            if (edit_index >= 0)
            {
                channelBox.Text = (string)channelList.Items[edit_index];
                urlBox.Text = (string)urlList.Items[edit_index];
            }
        }

        private void urlList_SelectedIndexChanged(object sender, EventArgs e)
        {
            edit_index = urlList.SelectedIndex;
            EditSelectedChannel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int rem_index = edit_index;
            channelList.Items.RemoveAt(rem_index);
            urlList.Items.RemoveAt(rem_index);
            SaveAllChannels();
            MessageBox.Show("Channel Removed");
        }
    }
}
