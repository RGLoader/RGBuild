namespace RGBuild
{
    partial class OpenImageDialog
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
            this.cmdLoad = new System.Windows.Forms.Button();
            this.txtImage = new System.Windows.Forms.TextBox();
            this.lblImage = new System.Windows.Forms.Label();
            this.lblCPUKey = new System.Windows.Forms.Label();
            this.txtCPUKey = new System.Windows.Forms.TextBox();
            this.lbl1BLKey = new System.Windows.Forms.Label();
            this.txt1BLKey = new System.Windows.Forms.TextBox();
            this.lblSaved = new System.Windows.Forms.Label();
            this.cmbSaved = new System.Windows.Forms.ComboBox();
            this.cmb1BLType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdLoad
            // 
            this.cmdLoad.Location = new System.Drawing.Point(181, 181);
            this.cmdLoad.Name = "cmdLoad";
            this.cmdLoad.Size = new System.Drawing.Size(91, 23);
            this.cmdLoad.TabIndex = 0;
            this.cmdLoad.Text = "Load";
            this.cmdLoad.UseVisualStyleBackColor = true;
            this.cmdLoad.Click += new System.EventHandler(this.cmdLoad_Click);
            // 
            // txtImage
            // 
            this.txtImage.Location = new System.Drawing.Point(67, 36);
            this.txtImage.Name = "txtImage";
            this.txtImage.ReadOnly = true;
            this.txtImage.Size = new System.Drawing.Size(332, 20);
            this.txtImage.TabIndex = 1;
            // 
            // lblImage
            // 
            this.lblImage.AutoSize = true;
            this.lblImage.Location = new System.Drawing.Point(20, 17);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(77, 13);
            this.lblImage.TabIndex = 2;
            this.lblImage.Text = "Chosen image:";
            // 
            // lblCPUKey
            // 
            this.lblCPUKey.AutoSize = true;
            this.lblCPUKey.Location = new System.Drawing.Point(20, 73);
            this.lblCPUKey.Name = "lblCPUKey";
            this.lblCPUKey.Size = new System.Drawing.Size(53, 13);
            this.lblCPUKey.TabIndex = 4;
            this.lblCPUKey.Text = "CPU Key:";
            // 
            // txtCPUKey
            // 
            this.txtCPUKey.Location = new System.Drawing.Point(67, 92);
            this.txtCPUKey.Name = "txtCPUKey";
            this.txtCPUKey.Size = new System.Drawing.Size(332, 20);
            this.txtCPUKey.TabIndex = 3;
            // 
            // lbl1BLKey
            // 
            this.lbl1BLKey.AutoSize = true;
            this.lbl1BLKey.Location = new System.Drawing.Point(20, 129);
            this.lbl1BLKey.Name = "lbl1BLKey";
            this.lbl1BLKey.Size = new System.Drawing.Size(50, 13);
            this.lbl1BLKey.TabIndex = 6;
            this.lbl1BLKey.Text = "1BL Key:";
            // 
            // txt1BLKey
            // 
            this.txt1BLKey.Location = new System.Drawing.Point(67, 148);
            this.txt1BLKey.Name = "txt1BLKey";
            this.txt1BLKey.Size = new System.Drawing.Size(332, 20);
            this.txt1BLKey.TabIndex = 5;
            // 
            // lblSaved
            // 
            this.lblSaved.AutoSize = true;
            this.lblSaved.Location = new System.Drawing.Point(249, 65);
            this.lblSaved.Name = "lblSaved";
            this.lblSaved.Size = new System.Drawing.Size(41, 13);
            this.lblSaved.TabIndex = 7;
            this.lblSaved.Text = "Saved:";
            // 
            // cmbSaved
            // 
            this.cmbSaved.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSaved.FormattingEnabled = true;
            this.cmbSaved.Items.AddRange(new object[] {
            "Pre-1839"});
            this.cmbSaved.Location = new System.Drawing.Point(296, 62);
            this.cmbSaved.Name = "cmbSaved";
            this.cmbSaved.Size = new System.Drawing.Size(103, 21);
            this.cmbSaved.TabIndex = 9;
            this.cmbSaved.SelectedIndexChanged += new System.EventHandler(this.cmbSaved_SelectedIndexChanged);
            // 
            // cmb1BLType
            // 
            this.cmb1BLType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb1BLType.FormattingEnabled = true;
            this.cmb1BLType.Items.AddRange(new object[] {
            "Release",
            "Pre-release"});
            this.cmb1BLType.Location = new System.Drawing.Point(296, 118);
            this.cmb1BLType.Name = "cmb1BLType";
            this.cmb1BLType.Size = new System.Drawing.Size(103, 21);
            this.cmb1BLType.TabIndex = 11;
            this.cmb1BLType.SelectedIndexChanged += new System.EventHandler(this.cmb1BLType_SelectedIndexChanged);
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(256, 121);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(34, 13);
            this.lblType.TabIndex = 10;
            this.lblType.Text = "Type:";
            // 
            // OpenImageDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 216);
            this.Controls.Add(this.cmb1BLType);
            this.Controls.Add(this.lblType);
            this.Controls.Add(this.cmbSaved);
            this.Controls.Add(this.lblSaved);
            this.Controls.Add(this.lbl1BLKey);
            this.Controls.Add(this.txt1BLKey);
            this.Controls.Add(this.lblCPUKey);
            this.Controls.Add(this.txtCPUKey);
            this.Controls.Add(this.lblImage);
            this.Controls.Add(this.txtImage);
            this.Controls.Add(this.cmdLoad);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpenImageDialog";
            this.Text = "Open an image";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdLoad;
        private System.Windows.Forms.TextBox txtImage;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.Label lblCPUKey;
        private System.Windows.Forms.TextBox txtCPUKey;
        private System.Windows.Forms.Label lbl1BLKey;
        private System.Windows.Forms.TextBox txt1BLKey;
        private System.Windows.Forms.Label lblSaved;
        private System.Windows.Forms.ComboBox cmbSaved;
        private System.Windows.Forms.ComboBox cmb1BLType;
        private System.Windows.Forms.Label lblType;
    }
}