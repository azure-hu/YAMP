using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace libIsh
{
    public partial class tagWnd : Form
    {
        private string filePath;

        public tagWnd()
        {
            InitializeComponent(); 
        }

        public void LoadTags(string FilePath)
        {
            filePath = FilePath;
            SelectedSwitch();
        }


        private void SelectedSwitch()
        {
            if (TagReader.Init(filePath))
            {
                switch (tagTabControl.SelectedTab.Text)
                {
                    case "ID3v1":
                        {
                            id3v1Grid.SelectedObject =
                                (new TagProperty(TagReader.getID3()[0]).FormatSupports
                                ? new TagProperty(TagReader.getID3()[0]) : null);
                        } break;
                    case "ID3v2":
                        {
                            id3v2Grid.SelectedObject =
                                (new TagProperty(TagReader.getID3()[1]).FormatSupports
                                ? new TagProperty(TagReader.getID3()[1]) : null);
                        } break;
                    case "Apple MP4/AAC":
                        {
                            appleGrid.SelectedObject =
                                (new TagProperty(TagReader.getApple()).FormatSupports
                                ? new TagProperty(TagReader.getApple()) : null);
                        } break;
                    case "Ogg, Xiph":
                        {
                            xiphGrid.SelectedObject = 
                                (new TagProperty(TagReader.getXiph()).FormatSupports
                                ? new TagProperty(TagReader.getXiph()) : null);
                        } break;
                    case "Ape":
                        {
                            apeGrid.SelectedObject =

                                (new TagProperty(TagReader.getApe()).FormatSupports
                                ? new TagProperty(TagReader.getApe()) : null);
                        } break;
                    case "Asf":
                        {
                            asfGrid.SelectedObject =
                                (new TagProperty(TagReader.getAsf()).FormatSupports
                                ? new TagProperty(TagReader.getAsf()) : null);
                        } break;
                }
            }
        }

        private void tagTabControl_Selected(object sender, TabControlEventArgs e)
        {
            SelectedSwitch();
        }

    }
}

    