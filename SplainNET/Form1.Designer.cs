namespace SplainNET
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.ID_Grid = new System.Windows.Forms.PropertyGrid();
            this.streamList = new System.Windows.Forms.ComboBox();
            this.playBtn = new System.Windows.Forms.Button();
            this.listBtn = new System.Windows.Forms.Button();
            this.visBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.recButton = new System.Windows.Forms.Button();
            this.visBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.visBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ID_Grid
            // 
            this.ID_Grid.BackColor = System.Drawing.Color.Maroon;
            this.ID_Grid.CommandsBackColor = System.Drawing.Color.DarkRed;
            this.ID_Grid.CommandsDisabledLinkColor = System.Drawing.Color.DarkOliveGreen;
            this.ID_Grid.CommandsLinkColor = System.Drawing.Color.Fuchsia;
            this.ID_Grid.HelpBackColor = System.Drawing.Color.IndianRed;
            this.ID_Grid.LineColor = System.Drawing.Color.LightCoral;
            this.ID_Grid.Location = new System.Drawing.Point(0, 88);
            this.ID_Grid.Name = "ID_Grid";
            this.ID_Grid.PropertySort = System.Windows.Forms.PropertySort.NoSort;
            this.ID_Grid.Size = new System.Drawing.Size(414, 257);
            this.ID_Grid.TabIndex = 1;
            this.ID_Grid.ToolbarVisible = false;
            this.ID_Grid.UseCompatibleTextRendering = true;
            // 
            // streamList
            // 
            this.streamList.BackColor = System.Drawing.SystemColors.Window;
            this.streamList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.streamList.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.streamList.FormattingEnabled = true;
            this.streamList.Location = new System.Drawing.Point(2, 2);
            this.streamList.Name = "streamList";
            this.streamList.Size = new System.Drawing.Size(410, 21);
            this.streamList.TabIndex = 11;
            // 
            // playBtn
            // 
            this.playBtn.Location = new System.Drawing.Point(328, 25);
            this.playBtn.Name = "playBtn";
            this.playBtn.Size = new System.Drawing.Size(43, 30);
            this.playBtn.TabIndex = 12;
            this.playBtn.Text = "Play";
            this.playBtn.UseVisualStyleBackColor = true;
            this.playBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // listBtn
            // 
            this.listBtn.Location = new System.Drawing.Point(328, 56);
            this.listBtn.Name = "listBtn";
            this.listBtn.Size = new System.Drawing.Size(34, 28);
            this.listBtn.TabIndex = 13;
            this.listBtn.Text = "List";
            this.listBtn.UseVisualStyleBackColor = true;
            this.listBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // visBox
            // 
            this.visBox.BackColor = System.Drawing.Color.Black;
            this.visBox.Location = new System.Drawing.Point(2, 26);
            this.visBox.Name = "visBox";
            this.visBox.Size = new System.Drawing.Size(325, 58);
            this.visBox.TabIndex = 14;
            this.visBox.TabStop = false;
            this.visBox.Click += new System.EventHandler(this.visBox_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.visBtn);
            this.panel1.Controls.Add(this.recButton);
            this.panel1.Controls.Add(this.listBtn);
            this.panel1.Controls.Add(this.visBox);
            this.panel1.Controls.Add(this.playBtn);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(414, 89);
            this.panel1.TabIndex = 15;
            // 
            // recButton
            // 
            this.recButton.Location = new System.Drawing.Point(371, 25);
            this.recButton.Name = "recButton";
            this.recButton.Size = new System.Drawing.Size(43, 30);
            this.recButton.TabIndex = 15;
            this.recButton.Text = "Rec";
            this.recButton.UseVisualStyleBackColor = true;
            this.recButton.Click += new System.EventHandler(this.recButton_Click);
            // 
            // visBtn
            // 
            this.visBtn.Location = new System.Drawing.Point(363, 56);
            this.visBtn.Name = "visBtn";
            this.visBtn.Size = new System.Drawing.Size(48, 28);
            this.visBtn.TabIndex = 16;
            this.visBtn.Text = "Visual";
            this.visBtn.UseVisualStyleBackColor = true;
            this.visBtn.Click += new System.EventHandler(this.visBtn_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Magenta;
            this.ClientSize = new System.Drawing.Size(416, 87);
            this.Controls.Add(this.streamList);
            this.Controls.Add(this.ID_Grid);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Opacity = 0.95D;
            this.Text = "SplainNET";
            this.TransparencyKey = System.Drawing.Color.Magenta;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.visBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PropertyGrid ID_Grid;
        public System.Windows.Forms.ComboBox streamList;
        private System.Windows.Forms.Button playBtn;
        private System.Windows.Forms.Button listBtn;
        private System.Windows.Forms.PictureBox visBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button recButton;
        private System.Windows.Forms.Button visBtn;
    }
}

