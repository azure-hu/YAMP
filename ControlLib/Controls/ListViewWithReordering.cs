using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;

namespace Azure.LibCollection.CS
{
    /// <summary>
    /// A ListView with DragDrop reordering.
    /// <see cref="http://support.microsoft.com/kb/822483/en-us"/>
    /// </summary>
    public class ListViewWithReordering : ListView
    {
        private Timer tmrLVScroll;
        private System.ComponentModel.IContainer components;
        private int mintScrollDirection;
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        const int WM_VSCROLL = 277; // Vertical scroll
        const int SB_LINEUP = 0; // Scrolls one line up
        const int SB_LINEDOWN = 1; // Scrolls one line down

        public ListViewWithReordering()
        {
            InitializeComponent();
        }
        protected void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tmrLVScroll = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // tmrLVScroll
            // 
            this.tmrLVScroll.Tick += new System.EventHandler(this.tmrLVScroll_Tick);
            // 
            // ListViewBase
            // 
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.ListViewBase_DragOver);
            this.ResumeLayout(false);

        }

        protected void ListViewBase_DragOver(object sender, DragEventArgs e)
        {
            Point position = PointToClient(new Point(e.X, e.Y));

            if (position.Y <= (Font.Height / 2))
            {
                // getting close to top, ensure previous item is visible
                mintScrollDirection = SB_LINEUP;
                tmrLVScroll.Enabled = true;
            }
            else if (position.Y >= ClientSize.Height - Font.Height / 2)
            {
                // getting close to bottom, ensure next item is visible
                mintScrollDirection = SB_LINEDOWN;
                tmrLVScroll.Enabled = true;
            }
            else
            {
                tmrLVScroll.Enabled = false;
            }
        }

        private void tmrLVScroll_Tick(object sender, EventArgs e)
        {
            SendMessage(Handle, WM_VSCROLL, (IntPtr)mintScrollDirection, IntPtr.Zero);
        }



        protected override void OnItemDrag(ItemDragEventArgs e)
        {
            base.OnItemDrag(e);
            //Begins a drag-and-drop operation in the ListView control.
            this.DoDragDrop(this.SelectedItems, DragDropEffects.Move);
        }

        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            base.OnDragEnter(drgevent);
            int len = drgevent.Data.GetFormats().Length - 1;
            int i;
            for (i = 0; i <= len; i++)
            {
                if (drgevent.Data.GetFormats()[i].Equals("System.Windows.Forms.ListView+SelectedListViewItemCollection"))
                {
                    //The data from the drag source is moved to the target.	
                    drgevent.Effect = DragDropEffects.Move;
                }
            }
        }

        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            base.OnDragDrop(drgevent);
            //Return if the items are not selected in the ListView control.
            if (this.SelectedItems.Count == 0)
            {
                return;
            }
            //Returns the location of the mouse pointer in the ListView control.
            Point cp = this.PointToClient(new Point(drgevent.X, drgevent.Y));
            //Obtain the item that is located at the specified location of the mouse pointer.
            ListViewItem dragToItem = this.GetItemAt(cp.X, cp.Y);
            if (dragToItem == null)
            {
                return;
            }
            //Obtain the index of the item at the mouse pointer.
            int dragIndex = dragToItem.Index;
            ListViewItem[] sel = new ListViewItem[this.SelectedItems.Count];
            for (int i = 0; i <= this.SelectedItems.Count - 1; i++)
            {
                sel[i] = this.SelectedItems[i];
            }
            for (int i = 0; i < sel.GetLength(0); i++)
            {
                //Obtain the ListViewItem to be dragged to the target location.
                ListViewItem dragItem = sel[i];
                int itemIndex = dragIndex;
                if (itemIndex == dragItem.Index)
                {
                    return;
                }
                if (dragItem.Index < itemIndex)
                    itemIndex++;
                else
                    itemIndex = dragIndex + i;
                //Insert the item at the mouse pointer.
                ListViewItem insertItem = (ListViewItem)dragItem.Clone();
                this.Items.Insert(itemIndex, insertItem);
                //Removes the item from the initial location while 
                //the item is moved to the new location.
                this.Items.Remove(dragItem);
            }
            this.Invalidate();
        }
    }

    /*public class ListViewItemComparer : IComparer
    {
        private int col;
        public ListViewItemComparer()
        {
            col = 0;
        }
        public ListViewItemComparer(int column)
        {
            col = column;
        }
        public int Compare(object x, object y)
        {
            int returnVal = -1;
            string _xS = ((ListViewItem)x).SubItems[col].Text;
            string _yS = ((ListViewItem)y).SubItems[col].Text;
            int _IntX = 0, _IntY = 0;
            bool _isIntX = false;
            bool _isIntY = false;

            _isIntX = int.TryParse(_xS, out _IntX);
            _isIntY = int.TryParse(_yS, out _IntY);

            if (_isIntX && _isIntY)
                returnVal = _IntX.CompareTo(_IntY);
            else
                returnVal = String.Compare(((ListViewItem)x).SubItems[col].Text,
                ((ListViewItem)y).SubItems[col].Text);
            
            return returnVal;
        }
    }*/

}
