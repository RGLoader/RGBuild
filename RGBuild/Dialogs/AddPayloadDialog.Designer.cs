namespace RGBuild
{
    partial class AddPayloadDialog
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
            this.lbl1BLKey = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblCPUKey = new System.Windows.Forms.Label();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.lblImage = new System.Windows.Forms.Label();
            this.txtPayload = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl1BLKey
            // 
            this.lbl1BLKey.AutoSize = true;
            this.lbl1BLKey.Location = new System.Drawing.Point(12, 121);
            this.lbl1BLKey.Name = "lbl1BLKey";
            this.lbl1BLKey.Size = new System.Drawing.Size(63, 13);
            this.lbl1BLKey.TabIndex = 12;
            this.lbl1BLKey.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(59, 140);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(332, 20);
            this.txtDescription.TabIndex = 11;
            // 
            // lblCPUKey
            // 
            this.lblCPUKey.AutoSize = true;
            this.lblCPUKey.Location = new System.Drawing.Point(12, 62);
            this.lblCPUKey.Name = "lblCPUKey";
            this.lblCPUKey.Size = new System.Drawing.Size(48, 13);
            this.lblCPUKey.TabIndex = 10;
            this.lblCPUKey.Text = "Address:";
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(59, 81);
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(84, 20);
            this.txtAddress.TabIndex = 9;
            // 
            // lblImage
            // 
            this.lblImage.AutoSize = true;
            this.lblImage.Location = new System.Drawing.Point(12, 9);
            this.lblImage.Name = "lblImage";
            this.lblImage.Size = new System.Drawing.Size(64, 13);
            this.lblImage.TabIndex = 8;
            this.lblImage.Text = "Payload file:";
            // 
            // txtPayload
            // 
            this.txtPayload.Location = new System.Drawing.Point(59, 28);
            this.txtPayload.Name = "txtPayload";
            this.txtPayload.Size = new System.Drawing.Size(354, 20);
            this.txtPayload.TabIndex = 7;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(419, 27);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(34, 21);
            this.button1.TabIndex = 13;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 84);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "0x";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(68, 171);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 15;
            this.button2.Text = "Add";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(338, 171);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 16;
            this.button3.Text = "Cancel";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // AddPayloadDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 206);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lbl1BLKey);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.lblCPUKey);
            this.Controls.Add(this.txtAddress);
            this.Controls.Add(this.lblImage);
            this.Controls.Add(this.txtPayload);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddPayloadDialog";
            this.Text = "Add Payload";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl1BLKey;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblCPUKey;
        private System.Windows.Forms.TextBox txtAddress;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.TextBox txtPayload;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}