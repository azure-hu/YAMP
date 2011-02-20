namespace libZoi
{
    partial class tagWnd
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(tagWnd));
            this.tagTabControl = new System.Windows.Forms.TabControl();
            this.id3v1Tab = new System.Windows.Forms.TabPage();
            this.id3v1Grid = new System.Windows.Forms.PropertyGrid();
            this.id3v2Tab = new System.Windows.Forms.TabPage();
            this.id3v2Grid = new System.Windows.Forms.PropertyGrid();
            this.appleTab = new System.Windows.Forms.TabPage();
            this.appleGrid = new System.Windows.Forms.PropertyGrid();
            this.xiphTab = new System.Windows.Forms.TabPage();
            this.xiphGrid = new System.Windows.Forms.PropertyGrid();
            this.apeTab = new System.Windows.Forms.TabPage();
            this.apeGrid = new System.Windows.Forms.PropertyGrid();
            this.asfTab = new System.Windows.Forms.TabPage();
            this.asfGrid = new System.Windows.Forms.PropertyGrid();
            this.tagTabControl.SuspendLayout();
            this.id3v1Tab.SuspendLayout();
            this.id3v2Tab.SuspendLayout();
            this.appleTab.SuspendLayout();
            this.xiphTab.SuspendLayout();
            this.apeTab.SuspendLayout();
            this.asfTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tagTabControl
            // 
            this.tagTabControl.Controls.Add(this.id3v1Tab);
            this.tagTabControl.Controls.Add(this.id3v2Tab);
            this.tagTabControl.Controls.Add(this.appleTab);
            this.tagTabControl.Controls.Add(this.xiphTab);
            this.tagTabControl.Controls.Add(this.apeTab);
            this.tagTabControl.Controls.Add(this.asfTab);
            this.tagTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tagTabControl.Location = new System.Drawing.Point(0, 0);
            this.tagTabControl.Name = "tagTabControl";
            this.tagTabControl.SelectedIndex = 0;
            this.tagTabControl.Size = new System.Drawing.Size(322, 303);
            this.tagTabControl.TabIndex = 1;
            this.tagTabControl.Selected += new System.Windows.Forms.TabControlEventHandler(this.tagTabControl_Selected);
            // 
            // id3v1Tab
            // 
            this.id3v1Tab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.id3v1Tab.Controls.Add(this.id3v1Grid);
            this.id3v1Tab.Location = new System.Drawing.Point(4, 22);
            this.id3v1Tab.Name = "id3v1Tab";
            this.id3v1Tab.Size = new System.Drawing.Size(314, 277);
            this.id3v1Tab.TabIndex = 0;
            this.id3v1Tab.Text = "ID3v1";
            this.id3v1Tab.UseVisualStyleBackColor = true;
            // 
            // id3v1Grid
            // 
            this.id3v1Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.id3v1Grid.Location = new System.Drawing.Point(0, 0);
            this.id3v1Grid.Name = "id3v1Grid";
            this.id3v1Grid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.id3v1Grid.Size = new System.Drawing.Size(314, 277);
            this.id3v1Grid.TabIndex = 1;
            // 
            // id3v2Tab
            // 
            this.id3v2Tab.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.id3v2Tab.Controls.Add(this.id3v2Grid);
            this.id3v2Tab.Location = new System.Drawing.Point(4, 22);
            this.id3v2Tab.Name = "id3v2Tab";
            this.id3v2Tab.Size = new System.Drawing.Size(314, 277);
            this.id3v2Tab.TabIndex = 1;
            this.id3v2Tab.Text = "ID3v2";
            this.id3v2Tab.UseVisualStyleBackColor = true;
            // 
            // id3v2Grid
            // 
            this.id3v2Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.id3v2Grid.Location = new System.Drawing.Point(0, 0);
            this.id3v2Grid.Name = "id3v2Grid";
            this.id3v2Grid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.id3v2Grid.Size = new System.Drawing.Size(314, 277);
            this.id3v2Grid.TabIndex = 2;
            // 
            // appleTab
            // 
            this.appleTab.Controls.Add(this.appleGrid);
            this.appleTab.Location = new System.Drawing.Point(4, 22);
            this.appleTab.Name = "appleTab";
            this.appleTab.Size = new System.Drawing.Size(314, 277);
            this.appleTab.TabIndex = 2;
            this.appleTab.Text = "Apple MP4/AAC";
            this.appleTab.UseVisualStyleBackColor = true;
            // 
            // appleGrid
            // 
            this.appleGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.appleGrid.Location = new System.Drawing.Point(0, 0);
            this.appleGrid.Name = "appleGrid";
            this.appleGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.appleGrid.Size = new System.Drawing.Size(314, 277);
            this.appleGrid.TabIndex = 2;
            // 
            // xiphTab
            // 
            this.xiphTab.Controls.Add(this.xiphGrid);
            this.xiphTab.Location = new System.Drawing.Point(4, 22);
            this.xiphTab.Name = "xiphTab";
            this.xiphTab.Size = new System.Drawing.Size(314, 277);
            this.xiphTab.TabIndex = 5;
            this.xiphTab.Text = "Ogg, Xiph";
            this.xiphTab.UseVisualStyleBackColor = true;
            // 
            // xiphGrid
            // 
            this.xiphGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xiphGrid.Location = new System.Drawing.Point(0, 0);
            this.xiphGrid.Name = "xiphGrid";
            this.xiphGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.xiphGrid.Size = new System.Drawing.Size(314, 277);
            this.xiphGrid.TabIndex = 3;
            // 
            // apeTab
            // 
            this.apeTab.Controls.Add(this.apeGrid);
            this.apeTab.Location = new System.Drawing.Point(4, 22);
            this.apeTab.Name = "apeTab";
            this.apeTab.Size = new System.Drawing.Size(314, 277);
            this.apeTab.TabIndex = 3;
            this.apeTab.Text = "Ape";
            this.apeTab.UseVisualStyleBackColor = true;
            // 
            // apeGrid
            // 
            this.apeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.apeGrid.Location = new System.Drawing.Point(0, 0);
            this.apeGrid.Name = "apeGrid";
            this.apeGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.apeGrid.Size = new System.Drawing.Size(314, 277);
            this.apeGrid.TabIndex = 3;
            // 
            // asfTab
            // 
            this.asfTab.Controls.Add(this.asfGrid);
            this.asfTab.Location = new System.Drawing.Point(4, 22);
            this.asfTab.Name = "asfTab";
            this.asfTab.Size = new System.Drawing.Size(314, 277);
            this.asfTab.TabIndex = 4;
            this.asfTab.Text = "Asf";
            this.asfTab.UseVisualStyleBackColor = true;
            // 
            // asfGrid
            // 
            this.asfGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.asfGrid.Location = new System.Drawing.Point(0, 0);
            this.asfGrid.Name = "asfGrid";
            this.asfGrid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.asfGrid.Size = new System.Drawing.Size(314, 277);
            this.asfGrid.TabIndex = 3;
            // 
            // tagWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(322, 303);
            this.Controls.Add(this.tagTabControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(330, 335);
            this.Name = "tagWindow";
            this.Text = "YAMP - File Info";
            this.tagTabControl.ResumeLayout(false);
            this.id3v1Tab.ResumeLayout(false);
            this.id3v2Tab.ResumeLayout(false);
            this.appleTab.ResumeLayout(false);
            this.xiphTab.ResumeLayout(false);
            this.apeTab.ResumeLayout(false);
            this.asfTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tagTabControl;
        private System.Windows.Forms.TabPage id3v1Tab;
        private System.Windows.Forms.PropertyGrid id3v1Grid;
        private System.Windows.Forms.TabPage id3v2Tab;
        private System.Windows.Forms.PropertyGrid id3v2Grid;
        private System.Windows.Forms.TabPage appleTab;
        private System.Windows.Forms.PropertyGrid appleGrid;
        private System.Windows.Forms.TabPage apeTab;
        private System.Windows.Forms.TabPage asfTab;
        private System.Windows.Forms.TabPage xiphTab;
        private System.Windows.Forms.PropertyGrid apeGrid;
        private System.Windows.Forms.PropertyGrid asfGrid;
        private System.Windows.Forms.PropertyGrid xiphGrid;

    }
}