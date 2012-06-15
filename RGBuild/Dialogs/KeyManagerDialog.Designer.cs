namespace RGBuild
{
    partial class KeyManagerDialog
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
            this.cmdSave = new System.Windows.Forms.Button();
            this.lblImage = new System.Windows.Forms.Label();
            this.lblCPUKey = new System.Windows.Forms.Label();
            this.txtCPUKey = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.lbStored = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(331, 168);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(91, 23);
            this.cmdSave.TabIndex = 0;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // lblImage
            // 
            this.lblImage.AutoSize = true;
            this.lblImage.Location = new System.Drawing.Point(20, 17);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(66, 13);
            this.lblImage.TabIndex = 2;
            this.lblImage.Text = "Stored keys:";
            // 
            // lblCPUKey
            // 
            this.lblCPUKey.AutoSize = true;
            this.lblCPUKey.Location = new System.Drawing.Point(295, 83);
            this.lblCPUKey.Name = "lblCPUKey";
            this.lblCPUKey.Size = new System.Drawing.Size(53, 13);
            this.lblCPUKey.TabIndex = 4;
            this.lblCPUKey.Text = "CPU Key:";
            // 
            // txtCPUKey
            // 
            this.txtCPUKey.Location = new System.Drawing.Point(316, 108);
            this.txtCPUKey.Name = "txtCPUKey";
            this.txtCPUKey.Size = new System.Drawing.Size(237, 20);
            this.txtCPUKey.TabIndex = 3;
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(295, 27);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 8;
            this.lblName.Text = "Name:";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(316, 51);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(237, 20);
            this.txtName.TabIndex = 7;
            // 
            // cmdDelete
            // 
            this.cmdDelete.Location = new System.Drawing.Point(443, 168);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(91, 23);
            this.cmdDelete.TabIndex = 9;
            this.cmdDelete.Text = "Delete";
            this.cmdDelete.UseVisualStyleBackColor = true;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // lbStored
            // 
            this.lbStored.FormattingEnabled = true;
            this.lbStored.Location = new System.Drawing.Point(24, 44);
            this.lbStored.Name = "lbStored";
            this.lbStored.Size = new System.Drawing.Size(240, 238);
            this.lbStored.TabIndex = 10;
            this.lbStored.SelectedIndexChanged += new System.EventHandler(this.lbStored_SelectedIndexChanged);
            // 
            // KeyManagerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 306);
            this.Controls.Add(this.lbStored);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblCPUKey);
            this.Controls.Add(this.txtCPUKey);
            this.Controls.Add(this.lblImage);
            this.Controls.Add(this.cmdSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "KeyManagerDialog";
            this.Text = "Key manager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.Label lblCPUKey;
        private System.Windows.Forms.TextBox txtCPUKey;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.ListBox lbStored;
    }
}