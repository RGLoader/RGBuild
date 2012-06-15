namespace RGBuild
{
    partial class KeyVaultControl
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
            this.cmdSave = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ccRegionBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtKVtype = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtOSIGhex = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOSIG = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtDVDKey = new System.Windows.Forms.TextBox();
            this.groupbox4 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtCONType = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtMFRdate = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.txtConID = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupbox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(443, 339);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(73, 47);
            this.cmdSave.TabIndex = 18;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ccRegionBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtKVtype);
            this.groupBox1.Location = new System.Drawing.Point(12, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(203, 317);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "KeyVault";
            // 
            // ccRegionBox
            // 
            this.ccRegionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ccRegionBox.FormattingEnabled = true;
            this.ccRegionBox.Items.AddRange(new object[] {
            "Other",
            "0x00FF | NTSC/US",
            "0x01FE | NTSC/JAP",
            "0x01FF | NTSC/JAP",
            "0x01FC | NTSC/KOR",
            "0x0101 | NTSC/HK",
            "0x02FE | PAL/EU",
            "0x0201 | PAL/AUS",
            "0x7FFF | DEVKIT"});
            this.ccRegionBox.Location = new System.Drawing.Point(67, 53);
            this.ccRegionBox.Name = "ccRegionBox";
            this.ccRegionBox.Size = new System.Drawing.Size(121, 21);
            this.ccRegionBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Region";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "KV Type";
            // 
            // txtKVtype
            // 
            this.txtKVtype.Enabled = false;
            this.txtKVtype.Location = new System.Drawing.Point(67, 28);
            this.txtKVtype.Name = "txtKVtype";
            this.txtKVtype.Size = new System.Drawing.Size(25, 20);
            this.txtKVtype.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtOSIGhex);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtOSIG);
            this.groupBox2.Location = new System.Drawing.Point(221, 234);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(291, 93);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DVD Drive";
            // 
            // txtOSIGhex
            // 
            this.txtOSIGhex.Location = new System.Drawing.Point(9, 45);
            this.txtOSIGhex.Multiline = true;
            this.txtOSIGhex.Name = "txtOSIGhex";
            this.txtOSIGhex.Size = new System.Drawing.Size(270, 42);
            this.txtOSIGhex.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "OSIG";
            // 
            // txtOSIG
            // 
            this.txtOSIG.Enabled = false;
            this.txtOSIG.Location = new System.Drawing.Point(45, 19);
            this.txtOSIG.Name = "txtOSIG";
            this.txtOSIG.Size = new System.Drawing.Size(234, 20);
            this.txtOSIG.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.txtDVDKey);
            this.groupBox3.Location = new System.Drawing.Point(12, 328);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(421, 63);
            this.groupBox3.TabIndex = 21;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Keys";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "DVD key";
            // 
            // txtDVDKey
            // 
            this.txtDVDKey.Location = new System.Drawing.Point(65, 18);
            this.txtDVDKey.Name = "txtDVDKey";
            this.txtDVDKey.Size = new System.Drawing.Size(334, 20);
            this.txtDVDKey.TabIndex = 2;
            // 
            // groupbox4
            // 
            this.groupbox4.Controls.Add(this.label8);
            this.groupbox4.Controls.Add(this.txtCONType);
            this.groupbox4.Controls.Add(this.label7);
            this.groupbox4.Controls.Add(this.txtMFRdate);
            this.groupbox4.Controls.Add(this.label3);
            this.groupbox4.Controls.Add(this.label6);
            this.groupbox4.Controls.Add(this.txtSerial);
            this.groupbox4.Controls.Add(this.txtConID);
            this.groupbox4.Location = new System.Drawing.Point(221, 10);
            this.groupbox4.Name = "groupbox4";
            this.groupbox4.Size = new System.Drawing.Size(291, 218);
            this.groupbox4.TabIndex = 20;
            this.groupbox4.TabStop = false;
            this.groupbox4.Text = "Console";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "Console Type";
            // 
            // txtCONType
            // 
            this.txtCONType.Location = new System.Drawing.Point(90, 54);
            this.txtCONType.Name = "txtCONType";
            this.txtCONType.Size = new System.Drawing.Size(27, 20);
            this.txtCONType.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "MFR Date";
            // 
            // txtMFRdate
            // 
            this.txtMFRdate.Location = new System.Drawing.Point(90, 78);
            this.txtMFRdate.Name = "txtMFRdate";
            this.txtMFRdate.Size = new System.Drawing.Size(63, 20);
            this.txtMFRdate.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Serial";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 31);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Console ID";
            // 
            // txtSerial
            // 
            this.txtSerial.Location = new System.Drawing.Point(90, 101);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(157, 20);
            this.txtSerial.TabIndex = 2;
            // 
            // txtConID
            // 
            this.txtConID.Location = new System.Drawing.Point(90, 29);
            this.txtConID.Name = "txtConID";
            this.txtConID.Size = new System.Drawing.Size(79, 20);
            this.txtConID.TabIndex = 0;
            // 
            // KeyVaultControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupbox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdSave);
            this.Name = "KeyVaultControl";
            this.Size = new System.Drawing.Size(522, 398);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupbox4.ResumeLayout(false);
            this.groupbox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtKVtype;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtOSIG;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtDVDKey;
        private System.Windows.Forms.GroupBox groupbox4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtCONType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtMFRdate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSerial;
        private System.Windows.Forms.TextBox txtConID;
        private System.Windows.Forms.TextBox txtOSIGhex;
        internal System.Windows.Forms.ComboBox ccRegionBox;

    }
}
