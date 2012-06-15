namespace RGBuild
{
    partial class CreateImageDialog
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
            this.cbMfgYear = new System.Windows.Forms.ComboBox();
            this.lblMfgYear = new System.Windows.Forms.Label();
            this.txtSU0Addr = new System.Windows.Forms.TextBox();
            this.txt2BLAddr = new System.Windows.Forms.TextBox();
            this.lbl2BLAddr = new System.Windows.Forms.Label();
            this.lblSU0Addr = new System.Windows.Forms.Label();
            this.lblHx = new System.Windows.Forms.Label();
            this.lblHx1 = new System.Windows.Forms.Label();
            this.cmdCreate = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lblImgSize = new System.Windows.Forms.Label();
            this.cbImgSize = new System.Windows.Forms.ComboBox();
            this.cmb1BLType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.cmbSaved = new System.Windows.Forms.ComboBox();
            this.lblSaved = new System.Windows.Forms.Label();
            this.lbl1BLKey = new System.Windows.Forms.Label();
            this.txt1BLKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCPUKey = new System.Windows.Forms.TextBox();
            this.chkSwapBlkIdx = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbMfgYear
            // 
            this.cbMfgYear.FormattingEnabled = true;
            this.cbMfgYear.Items.AddRange(new object[] {
            "2014",
            "2013",
            "2012",
            "2011",
            "2010",
            "2009",
            "2008",
            "2007",
            "2006",
            "2005",
            "2004"});
            this.cbMfgYear.Location = new System.Drawing.Point(357, 149);
            this.cbMfgYear.Name = "cbMfgYear";
            this.cbMfgYear.Size = new System.Drawing.Size(73, 21);
            this.cbMfgYear.TabIndex = 2;
            this.cbMfgYear.Text = "2012";
            // 
            // lblMfgYear
            // 
            this.lblMfgYear.AutoSize = true;
            this.lblMfgYear.Location = new System.Drawing.Point(366, 132);
            this.lblMfgYear.Name = "lblMfgYear";
            this.lblMfgYear.Size = new System.Drawing.Size(64, 13);
            this.lblMfgYear.TabIndex = 3;
            this.lblMfgYear.Text = "Year of mfg.";
            // 
            // txtSU0Addr
            // 
            this.txtSU0Addr.Location = new System.Drawing.Point(153, 149);
            this.txtSU0Addr.Name = "txtSU0Addr";
            this.txtSU0Addr.Size = new System.Drawing.Size(84, 20);
            this.txtSU0Addr.TabIndex = 4;
            this.txtSU0Addr.Text = "70000";
            // 
            // txt2BLAddr
            // 
            this.txt2BLAddr.Location = new System.Drawing.Point(30, 149);
            this.txt2BLAddr.Name = "txt2BLAddr";
            this.txt2BLAddr.Size = new System.Drawing.Size(84, 20);
            this.txt2BLAddr.TabIndex = 5;
            this.txt2BLAddr.Text = "8000";
            // 
            // lbl2BLAddr
            // 
            this.lbl2BLAddr.AutoSize = true;
            this.lbl2BLAddr.Location = new System.Drawing.Point(38, 132);
            this.lbl2BLAddr.Name = "lbl2BLAddr";
            this.lbl2BLAddr.Size = new System.Drawing.Size(67, 13);
            this.lbl2BLAddr.TabIndex = 6;
            this.lbl2BLAddr.Text = "2BL Address";
            // 
            // lblSU0Addr
            // 
            this.lblSU0Addr.AutoSize = true;
            this.lblSU0Addr.Location = new System.Drawing.Point(160, 132);
            this.lblSU0Addr.Name = "lblSU0Addr";
            this.lblSU0Addr.Size = new System.Drawing.Size(67, 13);
            this.lblSU0Addr.TabIndex = 7;
            this.lblSU0Addr.Text = "CF0 Address";
            // 
            // lblHx
            // 
            this.lblHx.AutoSize = true;
            this.lblHx.Location = new System.Drawing.Point(133, 153);
            this.lblHx.Name = "lblHx";
            this.lblHx.Size = new System.Drawing.Size(18, 13);
            this.lblHx.TabIndex = 8;
            this.lblHx.Text = "0x";
            // 
            // lblHx1
            // 
            this.lblHx1.AutoSize = true;
            this.lblHx1.Location = new System.Drawing.Point(11, 153);
            this.lblHx1.Name = "lblHx1";
            this.lblHx1.Size = new System.Drawing.Size(18, 13);
            this.lblHx1.TabIndex = 9;
            this.lblHx1.Text = "0x";
            // 
            // cmdCreate
            // 
            this.cmdCreate.Location = new System.Drawing.Point(192, 182);
            this.cmdCreate.Name = "cmdCreate";
            this.cmdCreate.Size = new System.Drawing.Size(102, 23);
            this.cmdCreate.TabIndex = 10;
            this.cmdCreate.Text = "Create";
            this.cmdCreate.UseVisualStyleBackColor = true;
            this.cmdCreate.Click += new System.EventHandler(this.cmdCreate_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Location = new System.Drawing.Point(192, 219);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(246, 13);
            this.lblInfo.TabIndex = 11;
            this.lblInfo.Text = "You\'ll add bootloaders and files in the main window";
            // 
            // lblImgSize
            // 
            this.lblImgSize.AutoSize = true;
            this.lblImgSize.Location = new System.Drawing.Point(269, 132);
            this.lblImgSize.Name = "lblImgSize";
            this.lblImgSize.Size = new System.Drawing.Size(59, 13);
            this.lblImgSize.TabIndex = 13;
            this.lblImgSize.Text = "Image Size";
            // 
            // cbImgSize
            // 
            this.cbImgSize.FormattingEnabled = true;
            this.cbImgSize.Items.AddRange(new object[] {
            "16MB",
            "64MB"});
            this.cbImgSize.Location = new System.Drawing.Point(264, 149);
            this.cbImgSize.Name = "cbImgSize";
            this.cbImgSize.Size = new System.Drawing.Size(69, 21);
            this.cbImgSize.TabIndex = 12;
            this.cbImgSize.Text = "16MB";
            // 
            // cmb1BLType
            // 
            this.cmb1BLType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb1BLType.FormattingEnabled = true;
            this.cmb1BLType.Items.AddRange(new object[] {
            "Release",
            "Pre-release"});
            this.cmb1BLType.Location = new System.Drawing.Point(308, 73);
            this.cmb1BLType.Name = "cmb1BLType";
            this.cmb1BLType.Size = new System.Drawing.Size(103, 21);
            this.cmb1BLType.TabIndex = 22;
            this.cmb1BLType.SelectedIndexChanged += new System.EventHandler(this.cmb1BLType_SelectedIndexChanged);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(268, 76);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 13);
            this.lblType.TabIndex = 21;
            this.lblType.Text = "Type:";
            // 
            // cmbSaved
            // 
            this.cmbSaved.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSaved.FormattingEnabled = true;
            this.cmbSaved.Items.AddRange(new object[] {
            "Pre-1839"});
            this.cmbSaved.Location = new System.Drawing.Point(308, 17);
            this.cmbSaved.Name = "cmbSaved";
            this.cmbSaved.Size = new System.Drawing.Size(103, 21);
            this.cmbSaved.TabIndex = 20;
            this.cmbSaved.SelectedIndexChanged += new System.EventHandler(this.cmbSaved_SelectedIndexChanged);
            // 
            // lblSaved
            // 
            this.lblSaved.AutoSize = true;
            this.lblSaved.Location = new System.Drawing.Point(261, 20);
            this.lblSaved.Name = "lblSaved";
            this.lblSaved.Size = new System.Drawing.Size(41, 13);
            this.lblSaved.TabIndex = 19;
            this.lblSaved.Text = "Saved:";
            // 
            // lbl1BLKey
            // 
            this.lbl1BLKey.AutoSize = true;
            this.lbl1BLKey.Location = new System.Drawing.Point(32, 84);
            this.lbl1BLKey.Name = "lbl1BLKey";
            this.lbl1BLKey.Size = new System.Drawing.Size(50, 13);
            this.lbl1BLKey.TabIndex = 18;
            this.lbl1BLKey.Text = "1BL Key:";
            // 
            // txt1BLKey
            // 
            this.txt1BLKey.Location = new System.Drawing.Point(30, 103);
            this.txt1BLKey.Name = "txt1BLKey";
            this.txt1BLKey.Size = new System.Drawing.Size(400, 20);
            this.txt1BLKey.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "CPU Key:";
            // 
            // txtCPUKey
            // 
            this.txtCPUKey.Location = new System.Drawing.Point(30, 47);
            this.txtCPUKey.Name = "txtCPUKey";
            this.txtCPUKey.Size = new System.Drawing.Size(400, 20);
            this.txtCPUKey.TabIndex = 15;
            // 
            // chkSwapBlkIdx
            // 
            this.chkSwapBlkIdx.AutoSize = true;
            this.chkSwapBlkIdx.Location = new System.Drawing.Point(12, 186);
            this.chkSwapBlkIdx.Name = "chkSwapBlkIdx";
            this.chkSwapBlkIdx.Size = new System.Drawing.Size(160, 17);
            this.chkSwapBlkIdx.TabIndex = 23;
            this.chkSwapBlkIdx.Text = "Swap block idx. order in ecc";
            this.chkSwapBlkIdx.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "(hh ll instead of ll hh)";
            // 
            // CreateImageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 239);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkSwapBlkIdx);
            this.Controls.Add(this.cmb1BLType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cmbSaved);
            this.Controls.Add(this.lblSaved);
            this.Controls.Add(this.lbl1BLKey);
            this.Controls.Add(this.txt1BLKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCPUKey);
            this.Controls.Add(this.lblImgSize);
            this.Controls.Add(this.cbImgSize);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.cmdCreate);
            this.Controls.Add(this.lblHx1);
            this.Controls.Add(this.lblHx);
            this.Controls.Add(this.lblSU0Addr);
            this.Controls.Add(this.lbl2BLAddr);
            this.Controls.Add(this.txt2BLAddr);
            this.Controls.Add(this.txtSU0Addr);
            this.Controls.Add(this.lblMfgYear);
            this.Controls.Add(this.cbMfgYear);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateImageDialog";
            this.Text = "Create Image";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbMfgYear;
        private System.Windows.Forms.Label lblMfgYear;
        private System.Windows.Forms.TextBox txtSU0Addr;
        private System.Windows.Forms.TextBox txt2BLAddr;
        private System.Windows.Forms.Label lbl2BLAddr;
        private System.Windows.Forms.Label lblSU0Addr;
        private System.Windows.Forms.Label lblHx;
        private System.Windows.Forms.Label lblHx1;
        private System.Windows.Forms.Button cmdCreate;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Label lblImgSize;
        private System.Windows.Forms.ComboBox cbImgSize;
        private System.Windows.Forms.ComboBox cmb1BLType;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbSaved;
        private System.Windows.Forms.Label lblSaved;
        private System.Windows.Forms.Label lbl1BLKey;
        private System.Windows.Forms.TextBox txt1BLKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCPUKey;
        private System.Windows.Forms.CheckBox chkSwapBlkIdx;
        private System.Windows.Forms.Label label2;
    }
}