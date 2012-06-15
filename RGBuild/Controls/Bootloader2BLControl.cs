using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RGBuild.NAND;
using RGBuild.Util;

namespace RGBuild
{
    public partial class Bootloader2BLControl : UserControl
    {
        private Bootloader2BL Bootloader;

        public Bootloader2BLControl(Bootloader2BL bootloader)
        {
            Bootloader = bootloader;
            InitializeComponent();
            loadData();
        }
        private void loadData()
        {
            txtLockDown.Text = Bootloader.PerBoxData.LockDownValue.ToString("X");
            txtConsoleType.Text = Bootloader.ConsoleTypeSeqAllowData.ConsoleType.ToString("X2");
            txtConsoleSeq.Text = Bootloader.ConsoleTypeSeqAllowData.ConsoleSequence.ToString("X2");
            txtConsoleSeqAllow.Text = Bootloader.ConsoleTypeSeqAllowData.ConsoleSequenceAllow.ToString("X4");

            txtPairing.Text = Shared.BytesToHexString(Bootloader.PerBoxData.PairingData, "");
            txtReserved.Text = Shared.BytesToHexString(Bootloader.PerBoxData.Reserved, "");
            txtPerBoxDigest.Text = Shared.BytesToHexString(Bootloader.PerBoxData.PerBoxDigest, "");
            txtCalcPerBoxDigest.Text = Shared.BytesToHexString(Bootloader.PerBoxData.CalculateDigest(), "");
            txt3BLNonce.Text = Shared.BytesToHexString(Bootloader.Nonce3BL, "");
            txt3BLSalt.Text = Shared.BytesToHexString(Bootloader.Salt3BL, "");
            txt4BLSalt.Text = Shared.BytesToHexString(Bootloader.Salt4BL, "");
            txt4BLDigest.Text = Shared.BytesToHexString(Bootloader.Digest4BL, "");
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            Bootloader.PerBoxData.LockDownValue = byte.Parse(txtLockDown.Text, NumberStyles.HexNumber);
            Bootloader.ConsoleTypeSeqAllowData.ConsoleType = byte.Parse(txtConsoleType.Text, NumberStyles.HexNumber);
            Bootloader.ConsoleTypeSeqAllowData.ConsoleSequence = byte.Parse(txtConsoleSeq.Text, NumberStyles.HexNumber);
            Bootloader.ConsoleTypeSeqAllowData.ConsoleSequenceAllow = ushort.Parse(txtConsoleSeqAllow.Text, NumberStyles.HexNumber);
            Bootloader.PerBoxData.PairingData = Shared.HexStringToBytes(txtPairing.Text);
            Bootloader.PerBoxData.Reserved = Shared.HexStringToBytes(txtReserved.Text);
            Bootloader.PerBoxData.PerBoxDigest = Shared.HexStringToBytes(txtPerBoxDigest.Text);
            Bootloader.Nonce3BL = Shared.HexStringToBytes(txt3BLNonce.Text);
            Bootloader.Salt3BL = Shared.HexStringToBytes(txt3BLSalt.Text);
            Bootloader.Salt4BL = Shared.HexStringToBytes(txt4BLSalt.Text);
            Bootloader.Digest4BL = Shared.HexStringToBytes(txt4BLDigest.Text);
            loadData();
        }

        private void cmdConSeqWhatWhat_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is the actual data the console uses to check against the fuses, it's algo is:\r\n\r\nbool ConSequenceCheck(byte sequence, ushort seqallow, ulong fuseline)\r\n" +
          "{\r\n" +
          "    if(fuseline == 0)\r\n" +
          "        return true;\r\n" +
          "    int i = 0;\r\n" +
          "    for(i = 0; i < 0x10; i++)\r\n" +
          "    {\r\n" +
          "        if((fuseline & 0xF) == 0xF)\r\n" +
          "            break;\r\n" +
          "        fuseline = fuseline >> 4;\r\n" +
          "    }\r\n" +
          "    if(i == 0 || (0x10 - i) == sequence)\r\n" +
          "        return true;\r\n\r\n" +
          "    return ((1<<((0x10 - i) - 1)) & seqallow) > 0;\r\n" +
          "}\r\n\r\n\r\nAs described by cory1492 at http://www.xboxhacker.org/index.php?topic=16935.msg126116#msg126116");
        }

        private void cmdCheck_Click(object sender, EventArgs e)
        {
            // open inputbox
            string value = "";
            if (OpenImageDialog.InputBox("Check fuses against loader", "Fuseline 01 (should end in f0 or 0f, only last 2 chars are needed):", ref value) == DialogResult.OK)
            {
                ulong fuseline1 = ulong.Parse(value, NumberStyles.HexNumber);
                if (OpenImageDialog.InputBox("Check fuses against loader", "Fuseline 02 (2BL LDV):", ref value) == DialogResult.OK)
                {
                    ulong fuseline2 = ulong.Parse(value, NumberStyles.HexNumber);
                    bool type = Bootloader.ConsoleTypeSeqAllowData.ConsoleTypeIsAllowed(fuseline1);
                    bool seq = Bootloader.ConsoleTypeSeqAllowData.ConsoleSequenceIsAllowed(fuseline2);
                    if(!type)
                        MessageBox.Show("You probably won't be able to run this loader as the ConsoleType field is different to the fuses");
                    if(!seq)
                        MessageBox.Show("You probably won't be able to run this loader as the ConsoleSequence and ConsoleSequenceAllow fields don't match the fuses");

                    if(type && seq)
                        MessageBox.Show("Looks like this loader would run on your console.");
                }
            }
        }
    }
}
