namespace RGBuild
{
    partial class Bootloader2BLControl
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
            this.gbPerBoxData = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCalcPerBoxDigest = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLockDown = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblPerBoxDigest = new System.Windows.Forms.Label();
            this.lblReserved = new System.Windows.Forms.Label();
            this.lblPairingData = new System.Windows.Forms.Label();
            this.txtPerBoxDigest = new System.Windows.Forms.TextBox();
            this.txtReserved = new System.Windows.Forms.TextBox();
            this.txtPairing = new System.Windows.Forms.TextBox();
            this.gbSecurity = new System.Windows.Forms.GroupBox();
            this.cmdRsaLoad = new System.Windows.Forms.Button();
            this.cmdRsaSave = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cmdAesLoad = new System.Windows.Forms.Button();
            this.cmdAesSave = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cmdSigLoad = new System.Windows.Forms.Button();
            this.cmdSigSave = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.gbBootloaders = new System.Windows.Forms.GroupBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txt4BLDigest = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txt4BLSalt = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt3BLSalt = new System.Windows.Forms.TextBox();
            this.lbl3BLNonce = new System.Windows.Forms.Label();
            this.txt3BLNonce = new System.Windows.Forms.TextBox();
            this.gbConsoleTypeAllow = new System.Windows.Forms.GroupBox();
            this.label23 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtConsoleSeqAllow = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtConsoleSeq = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtConsoleType = new System.Windows.Forms.TextBox();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdConSeqWhatWhat = new System.Windows.Forms.Button();
            this.cmdCheck = new System.Windows.Forms.Button();
            this.gbPerBoxData.SuspendLayout();
            this.gbSecurity.SuspendLayout();
            this.gbBootloaders.SuspendLayout();
            this.gbConsoleTypeAllow.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbPerBoxData
            // 
            this.gbPerBoxData.Controls.Add(this.label3);
            this.gbPerBoxData.Controls.Add(this.label7);
            this.gbPerBoxData.Controls.Add(this.txtCalcPerBoxDigest);
            this.gbPerBoxData.Controls.Add(this.label1);
            this.gbPerBoxData.Controls.Add(this.label2);
            this.gbPerBoxData.Controls.Add(this.txtLockDown);
            this.gbPerBoxData.Controls.Add(this.label16);
            this.gbPerBoxData.Controls.Add(this.label15);
            this.gbPerBoxData.Controls.Add(this.label14);
            this.gbPerBoxData.Controls.Add(this.lblPerBoxDigest);
            this.gbPerBoxData.Controls.Add(this.lblReserved);
            this.gbPerBoxData.Controls.Add(this.lblPairingData);
            this.gbPerBoxData.Controls.Add(this.txtPerBoxDigest);
            this.gbPerBoxData.Controls.Add(this.txtReserved);
            this.gbPerBoxData.Controls.Add(this.txtPairing);
            this.gbPerBoxData.Location = new System.Drawing.Point(14, 14);
            this.gbPerBoxData.Name = "gbPerBoxData";
            this.gbPerBoxData.Size = new System.Drawing.Size(308, 174);
            this.gbPerBoxData.TabIndex = 6;
            this.gbPerBoxData.TabStop = false;
            this.gbPerBoxData.Text = "Per-box/pairing data (hashed)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 153);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 13);
            this.label3.TabIndex = 25;
            this.label3.Text = "0x";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(96, 134);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 13);
            this.label7.TabIndex = 24;
            this.label7.Text = "Calculated Per-Box Digest";
            // 
            // txtCalcPerBoxDigest
            // 
            this.txtCalcPerBoxDigest.Location = new System.Drawing.Point(20, 150);
            this.txtCalcPerBoxDigest.Name = "txtCalcPerBoxDigest";
            this.txtCalcPerBoxDigest.ReadOnly = true;
            this.txtCalcPerBoxDigest.Size = new System.Drawing.Size(279, 20);
            this.txtCalcPerBoxDigest.TabIndex = 23;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(164, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "0x";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "LDV";
            // 
            // txtLockDown
            // 
            this.txtLockDown.Location = new System.Drawing.Point(182, 34);
            this.txtLockDown.Name = "txtLockDown";
            this.txtLockDown.Size = new System.Drawing.Size(90, 20);
            this.txtLockDown.TabIndex = 20;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(2, 115);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(18, 13);
            this.label16.TabIndex = 19;
            this.label16.Text = "0x";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(2, 76);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(18, 13);
            this.label15.TabIndex = 13;
            this.label15.Text = "0x";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(32, 37);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(18, 13);
            this.label14.TabIndex = 12;
            this.label14.Text = "0x";
            // 
            // lblPerBoxDigest
            // 
            this.lblPerBoxDigest.AutoSize = true;
            this.lblPerBoxDigest.Location = new System.Drawing.Point(38, 96);
            this.lblPerBoxDigest.Name = "lblPerBoxDigest";
            this.lblPerBoxDigest.Size = new System.Drawing.Size(242, 13);
            this.lblPerBoxDigest.TabIndex = 11;
            this.lblPerBoxDigest.Text = "Per-Box Digest (you should rehash if update SMC)";
            // 
            // lblReserved
            // 
            this.lblReserved.AutoSize = true;
            this.lblReserved.Location = new System.Drawing.Point(130, 57);
            this.lblReserved.Name = "lblReserved";
            this.lblReserved.Size = new System.Drawing.Size(53, 13);
            this.lblReserved.TabIndex = 10;
            this.lblReserved.Text = "Reserved";
            // 
            // lblPairingData
            // 
            this.lblPairingData.AutoSize = true;
            this.lblPairingData.Location = new System.Drawing.Point(59, 18);
            this.lblPairingData.Name = "lblPairingData";
            this.lblPairingData.Size = new System.Drawing.Size(65, 13);
            this.lblPairingData.TabIndex = 9;
            this.lblPairingData.Text = "Pairing Data";
            // 
            // txtPerBoxDigest
            // 
            this.txtPerBoxDigest.Location = new System.Drawing.Point(20, 112);
            this.txtPerBoxDigest.Name = "txtPerBoxDigest";
            this.txtPerBoxDigest.Size = new System.Drawing.Size(279, 20);
            this.txtPerBoxDigest.TabIndex = 8;
            // 
            // txtReserved
            // 
            this.txtReserved.Location = new System.Drawing.Point(20, 73);
            this.txtReserved.Name = "txtReserved";
            this.txtReserved.Size = new System.Drawing.Size(279, 20);
            this.txtReserved.TabIndex = 7;
            // 
            // txtPairing
            // 
            this.txtPairing.Location = new System.Drawing.Point(49, 34);
            this.txtPairing.Name = "txtPairing";
            this.txtPairing.Size = new System.Drawing.Size(90, 20);
            this.txtPairing.TabIndex = 6;
            // 
            // gbSecurity
            // 
            this.gbSecurity.Controls.Add(this.cmdRsaLoad);
            this.gbSecurity.Controls.Add(this.cmdRsaSave);
            this.gbSecurity.Controls.Add(this.label6);
            this.gbSecurity.Controls.Add(this.cmdAesLoad);
            this.gbSecurity.Controls.Add(this.cmdAesSave);
            this.gbSecurity.Controls.Add(this.label5);
            this.gbSecurity.Controls.Add(this.cmdSigLoad);
            this.gbSecurity.Controls.Add(this.cmdSigSave);
            this.gbSecurity.Controls.Add(this.label4);
            this.gbSecurity.Location = new System.Drawing.Point(328, 14);
            this.gbSecurity.Name = "gbSecurity";
            this.gbSecurity.Size = new System.Drawing.Size(184, 174);
            this.gbSecurity.TabIndex = 7;
            this.gbSecurity.TabStop = false;
            this.gbSecurity.Text = "Security";
            // 
            // cmdRsaLoad
            // 
            this.cmdRsaLoad.Location = new System.Drawing.Point(17, 132);
            this.cmdRsaLoad.Name = "cmdRsaLoad";
            this.cmdRsaLoad.Size = new System.Drawing.Size(60, 27);
            this.cmdRsaLoad.TabIndex = 18;
            this.cmdRsaLoad.Text = "Load";
            this.cmdRsaLoad.UseVisualStyleBackColor = true;
            // 
            // cmdRsaSave
            // 
            this.cmdRsaSave.Location = new System.Drawing.Point(109, 132);
            this.cmdRsaSave.Name = "cmdRsaSave";
            this.cmdRsaSave.Size = new System.Drawing.Size(60, 27);
            this.cmdRsaSave.TabIndex = 17;
            this.cmdRsaSave.Text = "Save";
            this.cmdRsaSave.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(53, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "RSA Public Key";
            // 
            // cmdAesLoad
            // 
            this.cmdAesLoad.Location = new System.Drawing.Point(17, 82);
            this.cmdAesLoad.Name = "cmdAesLoad";
            this.cmdAesLoad.Size = new System.Drawing.Size(60, 27);
            this.cmdAesLoad.TabIndex = 15;
            this.cmdAesLoad.Text = "Load";
            this.cmdAesLoad.UseVisualStyleBackColor = true;
            // 
            // cmdAesSave
            // 
            this.cmdAesSave.Location = new System.Drawing.Point(109, 82);
            this.cmdAesSave.Name = "cmdAesSave";
            this.cmdAesSave.Size = new System.Drawing.Size(60, 27);
            this.cmdAesSave.TabIndex = 14;
            this.cmdAesSave.Text = "Save";
            this.cmdAesSave.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(60, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "AES INV Data";
            // 
            // cmdSigLoad
            // 
            this.cmdSigLoad.Location = new System.Drawing.Point(17, 34);
            this.cmdSigLoad.Name = "cmdSigLoad";
            this.cmdSigLoad.Size = new System.Drawing.Size(60, 27);
            this.cmdSigLoad.TabIndex = 12;
            this.cmdSigLoad.Text = "Load";
            this.cmdSigLoad.UseVisualStyleBackColor = true;
            // 
            // cmdSigSave
            // 
            this.cmdSigSave.Location = new System.Drawing.Point(109, 34);
            this.cmdSigSave.Name = "cmdSigSave";
            this.cmdSigSave.Size = new System.Drawing.Size(60, 27);
            this.cmdSigSave.TabIndex = 11;
            this.cmdSigSave.Text = "Save";
            this.cmdSigSave.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(69, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Signature";
            // 
            // gbBootloaders
            // 
            this.gbBootloaders.Controls.Add(this.label20);
            this.gbBootloaders.Controls.Add(this.label19);
            this.gbBootloaders.Controls.Add(this.label18);
            this.gbBootloaders.Controls.Add(this.label17);
            this.gbBootloaders.Controls.Add(this.label10);
            this.gbBootloaders.Controls.Add(this.txt4BLDigest);
            this.gbBootloaders.Controls.Add(this.label9);
            this.gbBootloaders.Controls.Add(this.txt4BLSalt);
            this.gbBootloaders.Controls.Add(this.label8);
            this.gbBootloaders.Controls.Add(this.txt3BLSalt);
            this.gbBootloaders.Controls.Add(this.lbl3BLNonce);
            this.gbBootloaders.Controls.Add(this.txt3BLNonce);
            this.gbBootloaders.Location = new System.Drawing.Point(14, 194);
            this.gbBootloaders.Name = "gbBootloaders";
            this.gbBootloaders.Size = new System.Drawing.Size(498, 112);
            this.gbBootloaders.TabIndex = 8;
            this.gbBootloaders.TabStop = false;
            this.gbBootloaders.Text = "Bootloaders (signed)";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(257, 74);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(18, 13);
            this.label20.TabIndex = 20;
            this.label20.Text = "0x";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(257, 35);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(18, 13);
            this.label19.TabIndex = 19;
            this.label19.Text = "0x";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(2, 74);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(18, 13);
            this.label18.TabIndex = 19;
            this.label18.Text = "0x";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(2, 35);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(18, 13);
            this.label17.TabIndex = 19;
            this.label17.Text = "0x";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(103, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "4BL Digest";
            // 
            // txt4BLDigest
            // 
            this.txt4BLDigest.Location = new System.Drawing.Point(20, 71);
            this.txt4BLDigest.Name = "txt4BLDigest";
            this.txt4BLDigest.Size = new System.Drawing.Size(237, 20);
            this.txt4BLDigest.TabIndex = 16;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(354, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "4BL Salt";
            // 
            // txt4BLSalt
            // 
            this.txt4BLSalt.Location = new System.Drawing.Point(277, 71);
            this.txt4BLSalt.Name = "txt4BLSalt";
            this.txt4BLSalt.Size = new System.Drawing.Size(215, 20);
            this.txt4BLSalt.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(354, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "3BL Salt";
            // 
            // txt3BLSalt
            // 
            this.txt3BLSalt.Location = new System.Drawing.Point(277, 32);
            this.txt3BLSalt.Name = "txt3BLSalt";
            this.txt3BLSalt.Size = new System.Drawing.Size(215, 20);
            this.txt3BLSalt.TabIndex = 12;
            // 
            // lbl3BLNonce
            // 
            this.lbl3BLNonce.AutoSize = true;
            this.lbl3BLNonce.Location = new System.Drawing.Point(101, 16);
            this.lbl3BLNonce.Name = "lbl3BLNonce";
            this.lbl3BLNonce.Size = new System.Drawing.Size(61, 13);
            this.lbl3BLNonce.TabIndex = 11;
            this.lbl3BLNonce.Text = "3BL Nonce";
            // 
            // txt3BLNonce
            // 
            this.txt3BLNonce.Location = new System.Drawing.Point(21, 32);
            this.txt3BLNonce.Name = "txt3BLNonce";
            this.txt3BLNonce.Size = new System.Drawing.Size(237, 20);
            this.txt3BLNonce.TabIndex = 10;
            // 
            // gbConsoleTypeAllow
            // 
            this.gbConsoleTypeAllow.Controls.Add(this.cmdCheck);
            this.gbConsoleTypeAllow.Controls.Add(this.cmdConSeqWhatWhat);
            this.gbConsoleTypeAllow.Controls.Add(this.label23);
            this.gbConsoleTypeAllow.Controls.Add(this.label22);
            this.gbConsoleTypeAllow.Controls.Add(this.label21);
            this.gbConsoleTypeAllow.Controls.Add(this.label13);
            this.gbConsoleTypeAllow.Controls.Add(this.txtConsoleSeqAllow);
            this.gbConsoleTypeAllow.Controls.Add(this.label12);
            this.gbConsoleTypeAllow.Controls.Add(this.txtConsoleSeq);
            this.gbConsoleTypeAllow.Controls.Add(this.label11);
            this.gbConsoleTypeAllow.Controls.Add(this.txtConsoleType);
            this.gbConsoleTypeAllow.Location = new System.Drawing.Point(14, 312);
            this.gbConsoleTypeAllow.Name = "gbConsoleTypeAllow";
            this.gbConsoleTypeAllow.Size = new System.Drawing.Size(308, 83);
            this.gbConsoleTypeAllow.TabIndex = 9;
            this.gbConsoleTypeAllow.TabStop = false;
            this.gbConsoleTypeAllow.Text = "Console type/sequence allow data (signed)";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(2, 42);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(18, 13);
            this.label23.TabIndex = 21;
            this.label23.Text = "0x";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(101, 42);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(18, 13);
            this.label22.TabIndex = 20;
            this.label22.Text = "0x";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(203, 42);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(18, 13);
            this.label21.TabIndex = 19;
            this.label21.Text = "0x";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(209, 23);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(98, 13);
            this.label13.TabIndex = 17;
            this.label13.Text = "Console Seq. Allow";
            // 
            // txtConsoleSeqAllow
            // 
            this.txtConsoleSeqAllow.Location = new System.Drawing.Point(223, 39);
            this.txtConsoleSeqAllow.Name = "txtConsoleSeqAllow";
            this.txtConsoleSeqAllow.Size = new System.Drawing.Size(75, 20);
            this.txtConsoleSeqAllow.TabIndex = 16;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(124, 23);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 13);
            this.label12.TabIndex = 15;
            this.label12.Text = "Console Seq.";
            // 
            // txtConsoleSeq
            // 
            this.txtConsoleSeq.Location = new System.Drawing.Point(121, 39);
            this.txtConsoleSeq.Name = "txtConsoleSeq";
            this.txtConsoleSeq.Size = new System.Drawing.Size(75, 20);
            this.txtConsoleSeq.TabIndex = 14;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(25, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "Console Type";
            // 
            // txtConsoleType
            // 
            this.txtConsoleType.Location = new System.Drawing.Point(22, 39);
            this.txtConsoleType.Name = "txtConsoleType";
            this.txtConsoleType.Size = new System.Drawing.Size(75, 20);
            this.txtConsoleType.TabIndex = 12;
            // 
            // cmdSave
            // 
            this.cmdSave.Location = new System.Drawing.Point(389, 344);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(60, 27);
            this.cmdSave.TabIndex = 18;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdConSeqWhatWhat
            // 
            this.cmdConSeqWhatWhat.Location = new System.Drawing.Point(215, -1);
            this.cmdConSeqWhatWhat.Name = "cmdConSeqWhatWhat";
            this.cmdConSeqWhatWhat.Size = new System.Drawing.Size(18, 21);
            this.cmdConSeqWhatWhat.TabIndex = 22;
            this.cmdConSeqWhatWhat.Text = "?";
            this.cmdConSeqWhatWhat.UseVisualStyleBackColor = true;
            this.cmdConSeqWhatWhat.Click += new System.EventHandler(this.cmdConSeqWhatWhat_Click);
            // 
            // cmdCheck
            // 
            this.cmdCheck.Location = new System.Drawing.Point(240, -1);
            this.cmdCheck.Name = "cmdCheck";
            this.cmdCheck.Size = new System.Drawing.Size(58, 21);
            this.cmdCheck.TabIndex = 23;
            this.cmdCheck.Text = "Check";
            this.cmdCheck.UseVisualStyleBackColor = true;
            this.cmdCheck.Click += new System.EventHandler(this.cmdCheck_Click);
            // 
            // Bootloader2BLControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.gbConsoleTypeAllow);
            this.Controls.Add(this.gbBootloaders);
            this.Controls.Add(this.gbSecurity);
            this.Controls.Add(this.gbPerBoxData);
            this.Name = "Bootloader2BLControl";
            this.Size = new System.Drawing.Size(522, 398);
            this.gbPerBoxData.ResumeLayout(false);
            this.gbPerBoxData.PerformLayout();
            this.gbSecurity.ResumeLayout(false);
            this.gbSecurity.PerformLayout();
            this.gbBootloaders.ResumeLayout(false);
            this.gbBootloaders.PerformLayout();
            this.gbConsoleTypeAllow.ResumeLayout(false);
            this.gbConsoleTypeAllow.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbPerBoxData;
        private System.Windows.Forms.Label lblPerBoxDigest;
        private System.Windows.Forms.Label lblReserved;
        private System.Windows.Forms.Label lblPairingData;
        private System.Windows.Forms.TextBox txtPerBoxDigest;
        private System.Windows.Forms.TextBox txtReserved;
        private System.Windows.Forms.TextBox txtPairing;
        private System.Windows.Forms.GroupBox gbSecurity;
        private System.Windows.Forms.Button cmdRsaLoad;
        private System.Windows.Forms.Button cmdRsaSave;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button cmdAesLoad;
        private System.Windows.Forms.Button cmdAesSave;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button cmdSigLoad;
        private System.Windows.Forms.Button cmdSigSave;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbBootloaders;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txt4BLDigest;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txt4BLSalt;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txt3BLSalt;
        private System.Windows.Forms.Label lbl3BLNonce;
        private System.Windows.Forms.TextBox txt3BLNonce;
        private System.Windows.Forms.GroupBox gbConsoleTypeAllow;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtConsoleSeqAllow;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtConsoleType;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtConsoleSeq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLockDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtCalcPerBoxDigest;
        private System.Windows.Forms.Button cmdConSeqWhatWhat;
        private System.Windows.Forms.Button cmdCheck;

    }
}
