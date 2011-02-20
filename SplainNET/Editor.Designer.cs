namespace SplainNET

{
    partial class Editor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.channelList = new System.Windows.Forms.ListBox();
            this.urlList = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.urlBox = new System.Windows.Forms.TextBox();
            this.channelBox = new System.Windows.Forms.TextBox();
            this.urlLabel = new System.Windows.Forms.Label();
            this.chanLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // channelList
            // 
            this.channelList.FormattingEnabled = true;
            this.channelList.Location = new System.Drawing.Point(0, 0);
            this.channelList.Name = "channelList";
            this.channelList.Size = new System.Drawing.Size(185, 199);
            this.channelList.TabIndex = 0;
            this.channelList.SelectedIndexChanged += new System.EventHandler(this.channelList_SelectedIndexChanged);
            // 
            // urlList
            // 
            this.urlList.FormattingEnabled = true;
            this.urlList.Location = new System.Drawing.Point(191, 0);
            this.urlList.Name = "urlList";
            this.urlList.Size = new System.Drawing.Size(353, 199);
            this.urlList.TabIndex = 1;
            this.urlList.SelectedIndexChanged += new System.EventHandler(this.urlList_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.saveButton);
            this.groupBox1.Controls.Add(this.urlBox);
            this.groupBox1.Controls.Add(this.channelBox);
            this.groupBox1.Controls.Add(this.urlLabel);
            this.groupBox1.Controls.Add(this.chanLabel);
            this.groupBox1.Location = new System.Drawing.Point(0, 206);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(544, 65);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "New Stream";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(427, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 20);
            this.button1.TabIndex = 5;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(427, 13);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(46, 20);
            this.saveButton.TabIndex = 4;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // urlBox
            // 
            this.urlBox.Location = new System.Drawing.Point(58, 38);
            this.urlBox.Name = "urlBox";
            this.urlBox.Size = new System.Drawing.Size(357, 20);
            this.urlBox.TabIndex = 3;
            // 
            // channelBox
            // 
            this.channelBox.Location = new System.Drawing.Point(58, 13);
            this.channelBox.Name = "channelBox";
            this.channelBox.Size = new System.Drawing.Size(357, 20);
            this.channelBox.TabIndex = 2;
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Location = new System.Drawing.Point(3, 41);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(32, 13);
            this.urlLabel.TabIndex = 1;
            this.urlLabel.Text = "URL:";
            // 
            // chanLabel
            // 
            this.chanLabel.AutoSize = true;
            this.chanLabel.Location = new System.Drawing.Point(3, 16);
            this.chanLabel.Name = "chanLabel";
            this.chanLabel.Size = new System.Drawing.Size(49, 13);
            this.chanLabel.TabIndex = 0;
            this.chanLabel.Text = "Channel:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(479, 13);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(59, 20);
            this.button2.TabIndex = 6;
            this.button2.Text = "Remove";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 274);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.urlList);
            this.Controls.Add(this.channelList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Editor";
            this.Text = "Editor";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox channelList;
        private System.Windows.Forms.ListBox urlList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.TextBox urlBox;
        private System.Windows.Forms.TextBox channelBox;
        private System.Windows.Forms.Label urlLabel;
        private System.Windows.Forms.Label chanLabel;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;

    }
}