namespace RGBuild
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dumpBootloaderInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decompressFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addPayloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addSMCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addKeyVaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addBootloaderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addFileSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.addMobileBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMobileCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMobileDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMobileEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMobileFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMobileGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMobileHToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMobileIToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addMobileJToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.addSMCConfigToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.loadFromIniToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.extractBaseKernelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.decompressPatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.extractToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scContainer = new System.Windows.Forms.SplitContainer();
            this.lvLoaders = new System.Windows.Forms.ListView();
            this.chBLName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBLBuild = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBLOffset = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBLSize = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chBLSecurity = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scContainer)).BeginInit();
            this.scContainer.Panel1.SuspendLayout();
            this.scContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.imageToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(897, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createImageToolStripMenuItem,
            this.openImageToolStripMenuItem,
            this.toolStripMenuItem1,
            this.saveImageToolStripMenuItem,
            this.closeImageToolStripMenuItem,
            this.toolStripMenuItem4,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // createImageToolStripMenuItem
            // 
            this.createImageToolStripMenuItem.Name = "createImageToolStripMenuItem";
            this.createImageToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.createImageToolStripMenuItem.Text = "Create image";
            this.createImageToolStripMenuItem.Click += new System.EventHandler(this.createImageToolStripMenuItem_Click);
            // 
            // openImageToolStripMenuItem
            // 
            this.openImageToolStripMenuItem.Name = "openImageToolStripMenuItem";
            this.openImageToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.openImageToolStripMenuItem.Text = "Open image";
            this.openImageToolStripMenuItem.Click += new System.EventHandler(this.openImageToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(184, 6);
            // 
            // saveImageToolStripMenuItem
            // 
            this.saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            this.saveImageToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.saveImageToolStripMenuItem.Text = "Save and close image";
            this.saveImageToolStripMenuItem.Click += new System.EventHandler(this.saveImageToolStripMenuItem_Click);
            // 
            // closeImageToolStripMenuItem
            // 
            this.closeImageToolStripMenuItem.Name = "closeImageToolStripMenuItem";
            this.closeImageToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.closeImageToolStripMenuItem.Text = "Close image";
            this.closeImageToolStripMenuItem.Click += new System.EventHandler(this.closeImageToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(184, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dumpBootloaderInfoToolStripMenuItem,
            this.decompressFileToolStripMenuItem,
            this.testToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // dumpBootloaderInfoToolStripMenuItem
            // 
            this.dumpBootloaderInfoToolStripMenuItem.Name = "dumpBootloaderInfoToolStripMenuItem";
            this.dumpBootloaderInfoToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.dumpBootloaderInfoToolStripMenuItem.Text = "Dump bootloader info";
            this.dumpBootloaderInfoToolStripMenuItem.Click += new System.EventHandler(this.dumpBootloaderInfoToolStripMenuItem_Click);
            // 
            // decompressFileToolStripMenuItem
            // 
            this.decompressFileToolStripMenuItem.Name = "decompressFileToolStripMenuItem";
            this.decompressFileToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.decompressFileToolStripMenuItem.Text = "Decompress file";
            this.decompressFileToolStripMenuItem.Click += new System.EventHandler(this.decompressFileToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.testToolStripMenuItem.Text = "Test";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.testToolStripMenuItem_Click);
            // 
            // imageToolStripMenuItem
            // 
            this.imageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addPayloadToolStripMenuItem,
            this.addSMCToolStripMenuItem,
            this.addKeyVaultToolStripMenuItem,
            this.addBootloaderToolStripMenuItem,
            this.addFileSystemToolStripMenuItem,
            this.toolStripMenuItem2,
            this.addMobileBToolStripMenuItem,
            this.addMobileCToolStripMenuItem,
            this.addMobileDToolStripMenuItem,
            this.addMobileEToolStripMenuItem,
            this.addMobileFToolStripMenuItem,
            this.addMobileGToolStripMenuItem,
            this.addMobileHToolStripMenuItem,
            this.addMobileIToolStripMenuItem,
            this.addMobileJToolStripMenuItem,
            this.toolStripMenuItem3,
            this.addSMCConfigToolStripMenuItem,
            this.toolStripMenuItem5,
            this.loadFromIniToolStripMenuItem,
            this.toolStripMenuItem6,
            this.extractBaseKernelToolStripMenuItem,
            this.decompressPatchToolStripMenuItem});
            this.imageToolStripMenuItem.Name = "imageToolStripMenuItem";
            this.imageToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.imageToolStripMenuItem.Text = "Image";
            // 
            // addPayloadToolStripMenuItem
            // 
            this.addPayloadToolStripMenuItem.Name = "addPayloadToolStripMenuItem";
            this.addPayloadToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addPayloadToolStripMenuItem.Text = "Add Payload";
            this.addPayloadToolStripMenuItem.Click += new System.EventHandler(this.addPayloadToolStripMenuItem_Click);
            // 
            // addSMCToolStripMenuItem
            // 
            this.addSMCToolStripMenuItem.Name = "addSMCToolStripMenuItem";
            this.addSMCToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addSMCToolStripMenuItem.Text = "Add SMC";
            this.addSMCToolStripMenuItem.Click += new System.EventHandler(this.addSMCToolStripMenuItem_Click);
            // 
            // addKeyVaultToolStripMenuItem
            // 
            this.addKeyVaultToolStripMenuItem.Name = "addKeyVaultToolStripMenuItem";
            this.addKeyVaultToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addKeyVaultToolStripMenuItem.Text = "Add KeyVault";
            this.addKeyVaultToolStripMenuItem.Click += new System.EventHandler(this.addKeyVaultToolStripMenuItem_Click);
            // 
            // addBootloaderToolStripMenuItem
            // 
            this.addBootloaderToolStripMenuItem.Name = "addBootloaderToolStripMenuItem";
            this.addBootloaderToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addBootloaderToolStripMenuItem.Text = "Add Bootloader";
            this.addBootloaderToolStripMenuItem.Click += new System.EventHandler(this.addBootloaderToolStripMenuItem_Click);
            // 
            // addFileSystemToolStripMenuItem
            // 
            this.addFileSystemToolStripMenuItem.Name = "addFileSystemToolStripMenuItem";
            this.addFileSystemToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addFileSystemToolStripMenuItem.Text = "Add FileSystem";
            this.addFileSystemToolStripMenuItem.Click += new System.EventHandler(this.addFileSystemToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(219, 6);
            // 
            // addMobileBToolStripMenuItem
            // 
            this.addMobileBToolStripMenuItem.Name = "addMobileBToolStripMenuItem";
            this.addMobileBToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addMobileBToolStripMenuItem.Text = "Add MobileB";
            this.addMobileBToolStripMenuItem.Click += new System.EventHandler(this.addMobileBToolStripMenuItem_Click);
            // 
            // addMobileCToolStripMenuItem
            // 
            this.addMobileCToolStripMenuItem.Name = "addMobileCToolStripMenuItem";
            this.addMobileCToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addMobileCToolStripMenuItem.Text = "Add MobileC";
            this.addMobileCToolStripMenuItem.Click += new System.EventHandler(this.addMobileCToolStripMenuItem_Click);
            // 
            // addMobileDToolStripMenuItem
            // 
            this.addMobileDToolStripMenuItem.Name = "addMobileDToolStripMenuItem";
            this.addMobileDToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addMobileDToolStripMenuItem.Text = "Add MobileD";
            this.addMobileDToolStripMenuItem.Click += new System.EventHandler(this.addMobileDToolStripMenuItem_Click);
            // 
            // addMobileEToolStripMenuItem
            // 
            this.addMobileEToolStripMenuItem.Name = "addMobileEToolStripMenuItem";
            this.addMobileEToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addMobileEToolStripMenuItem.Text = "Add MobileE";
            this.addMobileEToolStripMenuItem.Click += new System.EventHandler(this.addMobileEToolStripMenuItem_Click);
            // 
            // addMobileFToolStripMenuItem
            // 
            this.addMobileFToolStripMenuItem.Name = "addMobileFToolStripMenuItem";
            this.addMobileFToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addMobileFToolStripMenuItem.Text = "Add MobileF";
            this.addMobileFToolStripMenuItem.Click += new System.EventHandler(this.addMobileFToolStripMenuItem_Click);
            // 
            // addMobileGToolStripMenuItem
            // 
            this.addMobileGToolStripMenuItem.Name = "addMobileGToolStripMenuItem";
            this.addMobileGToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addMobileGToolStripMenuItem.Text = "Add MobileG";
            this.addMobileGToolStripMenuItem.Click += new System.EventHandler(this.addMobileGToolStripMenuItem_Click);
            // 
            // addMobileHToolStripMenuItem
            // 
            this.addMobileHToolStripMenuItem.Name = "addMobileHToolStripMenuItem";
            this.addMobileHToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addMobileHToolStripMenuItem.Text = "Add MobileH";
            this.addMobileHToolStripMenuItem.Click += new System.EventHandler(this.addMobileHToolStripMenuItem_Click);
            // 
            // addMobileIToolStripMenuItem
            // 
            this.addMobileIToolStripMenuItem.Name = "addMobileIToolStripMenuItem";
            this.addMobileIToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addMobileIToolStripMenuItem.Text = "Add MobileI";
            this.addMobileIToolStripMenuItem.Click += new System.EventHandler(this.addMobileIToolStripMenuItem_Click);
            // 
            // addMobileJToolStripMenuItem
            // 
            this.addMobileJToolStripMenuItem.Name = "addMobileJToolStripMenuItem";
            this.addMobileJToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addMobileJToolStripMenuItem.Text = "Add MobileJ";
            this.addMobileJToolStripMenuItem.Click += new System.EventHandler(this.addMobileJToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(219, 6);
            // 
            // addSMCConfigToolStripMenuItem
            // 
            this.addSMCConfigToolStripMenuItem.Name = "addSMCConfigToolStripMenuItem";
            this.addSMCConfigToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.addSMCConfigToolStripMenuItem.Text = "Add SMC config";
            this.addSMCConfigToolStripMenuItem.Click += new System.EventHandler(this.addSMCConfigToolStripMenuItem_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(219, 6);
            // 
            // loadFromIniToolStripMenuItem
            // 
            this.loadFromIniToolStripMenuItem.Name = "loadFromIniToolStripMenuItem";
            this.loadFromIniToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.loadFromIniToolStripMenuItem.Text = "Load from ini...";
            this.loadFromIniToolStripMenuItem.Click += new System.EventHandler(this.loadFromIniToolStripMenuItem_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(219, 6);
            // 
            // extractBaseKernelToolStripMenuItem
            // 
            this.extractBaseKernelToolStripMenuItem.Name = "extractBaseKernelToolStripMenuItem";
            this.extractBaseKernelToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.extractBaseKernelToolStripMenuItem.Text = "Extract base kernel";
            this.extractBaseKernelToolStripMenuItem.Click += new System.EventHandler(this.extractBaseKernelToolStripMenuItem_Click);
            // 
            // decompressPatchToolStripMenuItem
            // 
            this.decompressPatchToolStripMenuItem.Name = "decompressPatchToolStripMenuItem";
            this.decompressPatchToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            this.decompressPatchToolStripMenuItem.Text = "Extract decompressed patch";
            this.decompressPatchToolStripMenuItem.Click += new System.EventHandler(this.decompressPatchToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.extractToolStripMenuItem,
            this.replaceToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(116, 48);
            // 
            // extractToolStripMenuItem
            // 
            this.extractToolStripMenuItem.Name = "extractToolStripMenuItem";
            this.extractToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.extractToolStripMenuItem.Text = "Extract";
            this.extractToolStripMenuItem.Click += new System.EventHandler(this.extractToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.replaceToolStripMenuItem.Text = "Replace";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // scContainer
            // 
            this.scContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scContainer.Location = new System.Drawing.Point(0, 24);
            this.scContainer.Name = "scContainer";
            // 
            // scContainer.Panel1
            // 
            this.scContainer.Panel1.Controls.Add(this.lvLoaders);
            // 
            // scContainer.Panel2
            // 
            this.scContainer.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.scContainer_Panel2_Paint);
            this.scContainer.Size = new System.Drawing.Size(897, 405);
            this.scContainer.SplitterDistance = 359;
            this.scContainer.TabIndex = 2;
            // 
            // lvLoaders
            // 
            this.lvLoaders.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chBLName,
            this.chBLBuild,
            this.chBLOffset,
            this.chBLSize,
            this.chBLSecurity});
            this.lvLoaders.ContextMenuStrip = this.contextMenuStrip1;
            this.lvLoaders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLoaders.FullRowSelect = true;
            this.lvLoaders.Location = new System.Drawing.Point(0, 0);
            this.lvLoaders.Name = "lvLoaders";
            this.lvLoaders.Size = new System.Drawing.Size(359, 405);
            this.lvLoaders.TabIndex = 2;
            this.lvLoaders.UseCompatibleStateImageBehavior = false;
            this.lvLoaders.View = System.Windows.Forms.View.Details;
            this.lvLoaders.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvLoaders_ColumnClick);
            this.lvLoaders.SelectedIndexChanged += new System.EventHandler(this.lvLoaders_SelectedIndexChanged);
            // 
            // chBLName
            // 
            this.chBLName.Text = "Name";
            this.chBLName.Width = 67;
            // 
            // chBLBuild
            // 
            this.chBLBuild.Text = "Version";
            // 
            // chBLOffset
            // 
            this.chBLOffset.Text = "Offset";
            this.chBLOffset.Width = 78;
            // 
            // chBLSize
            // 
            this.chBLSize.Text = "Size";
            // 
            // chBLSecurity
            // 
            this.chBLSecurity.Text = "Security";
            this.chBLSecurity.Width = 71;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 429);
            this.Controls.Add(this.scContainer);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "RGBuild 2.93.2";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.scContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scContainer)).EndInit();
            this.scContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openImageToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem extractToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dumpBootloaderInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem imageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addBootloaderToolStripMenuItem;
        private System.Windows.Forms.SplitContainer scContainer;
        private System.Windows.Forms.ListView lvLoaders;
        private System.Windows.Forms.ColumnHeader chBLName;
        private System.Windows.Forms.ColumnHeader chBLBuild;
        private System.Windows.Forms.ColumnHeader chBLOffset;
        private System.Windows.Forms.ColumnHeader chBLSize;
        private System.Windows.Forms.ToolStripMenuItem addFileSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addSMCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addKeyVaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem addMobileBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMobileCToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMobileDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMobileEToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMobileFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMobileGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMobileHToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMobileIToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addMobileJToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem addSMCConfigToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addPayloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem decompressFileToolStripMenuItem;
        private System.Windows.Forms.ColumnHeader chBLSecurity;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
        private System.Windows.Forms.ToolStripMenuItem loadFromIniToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
        private System.Windows.Forms.ToolStripMenuItem decompressPatchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractBaseKernelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
    }
}

