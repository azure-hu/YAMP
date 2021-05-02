using Azure.LibCollection.CS.AudioWorks;
using Azure.MediaUtils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleWR2
{
    public partial class AudioOutputDialog : Form
    {
        private Dictionary<int, string> _outputs;
        private KeyValuePair<int, string> _activeOut;
        private readonly AudioEngine ae;

        public AudioOutputDialog(ref AudioEngine ae)
        {
            this.ae = ae;
            InitializeComponent();
            InitOutputDeviceList();
        }

        private void switchOutBtn_Click(object sender, EventArgs e)
        {
            ae.SwitchOutputDevice(((KeyValuePair<int, string>)outputCombo.SelectedItem).Key);
            InitOutputDeviceList();
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

        private void refreshOutBtn_Click(object sender, EventArgs e)
        {
            InitOutputDeviceList();
        }
    }
}
