namespace RGBuild
{
    partial class FileSystemControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gbPerBoxData = new System.Windows.Forms.GroupBox();
            this.lblNotCurrent = new System.Windows.Forms.Label();
            this.lvFiles = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chTimestamp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.injectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gbPerBoxData.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPerBoxData
            // 
            this.gbPerBoxData.Controls.Add(this.lblNotCurrent);
            this.gbPerBoxData.Controls.Add(this.lvFiles);
            this.gbPerBoxData.Location = new System.Drawing.Point(14, 14);
            this.gbPerBoxData.Name = "gbPerBoxData";
            this.gbPerBoxData.Size = new System.Drawing.Size(498, 367);
            this.gbPerBoxData.TabIndex = 6;
            this.gbPerBoxData.TabStop = false;
            this.gbPerBoxData.Text = "File System";
            // 
            // lblNotCurrent
            // 
            this.lblNotCurrent.AutoSize = true;
            this.lblNotCurrent.ForeColor = System.Drawing.Color.Red;
            this.lblNotCurrent.Location = new System.Drawing.Point(277, 0);
            this.lblNotCurrent.Name = "lblNotCurrent";
            this.lblNotCurrent.Size = new System.Drawing.Size(221, 13);
            this.lblNotCurrent.TabIndex = 1;
            this.lblNotCurrent.Text = "THIS IS NOT THE CURRENT FILESYSTEM";
            // 
            // lvFiles
            // 
            this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chSize,
            this.chTimestamp});
            this.lvFiles.ContextMenuStrip = this.contextMenuStrip1;
            this.lvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFiles.FullRowSelect = true;
            this.lvFiles.Location = new System.Drawing.Point(3, 16);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(492, 348);
            this.lvFiles.TabIndex = 0;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.SelectedIndexChanged += new System.EventHandler(this.lvFiles_SelectedIndexChanged);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 137;
            // 
            // chSize
            // 
            this.chSize.Text = "Size";
            this.chSize.Width = 77;
            // 
            // chTimestamp
            // 
            this.chTimestamp.Text = "Timestamp";
            this.chTimestamp.Width = 110;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.injectToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(116, 70);
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.extractToolStripMenuItem.Text = "Extract";
            this.extractToolStripMenuItem.Click += new System.EventHandler(this.extractToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.replaceToolStripMenuItem.Text = "Replace";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // injectToolStripMenuItem
            // 
            this.injectToolStripMenuItem.Name = "injectToolStripMenuItem";
            this.injectToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.injectToolStripMenuItem.Text = "Inject";
            this.injectToolStripMenuItem.Click += new System.EventHandler(this.injectToolStripMenuItem_Click);
            // 
            // FileSystemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbPerBoxData);
            this.Name = "FileSystemControl";
            this.Size = new System.Drawing.Size(522, 398);
            this.gbPerBoxData.ResumeLayout(false);
            this.gbPerBoxData.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbPerBoxData;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chSize;
        private System.Windows.Forms.ColumnHeader chTimestamp;
        private System.Windows.Forms.Label lblNotCurrent;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem injectToolStripMenuItem;

    }
}
