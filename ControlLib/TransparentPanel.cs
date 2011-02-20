using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace libZoi
{

    public class TransparentPanel : Panel
    {
        public TransparentPanel()
        {
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return createParams;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not paint background.
        }

        static public void TPCreator(ref TransparentPanel panel, int left, int top, int width, int height)
        {
            panel = new TransparentPanel();
            panel.Left = left;
            panel.Top = top;
            panel.Size = new Size(width, height);
            panel.Cursor = Cursors.Hand;
        }

        static public void TPCreator(ref TransparentPanel panel, Point pt, Point size)
        {
            TPCreator(ref panel, pt.X, pt.Y, size.X, size.Y);
        }
    }

}
