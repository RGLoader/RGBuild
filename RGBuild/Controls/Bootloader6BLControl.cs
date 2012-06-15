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
    public partial class Bootloader6BLControl : UserControl
    {
        private Bootloader6BL Bootloader;

        public Bootloader6BLControl(Bootloader6BL bootloader)
        {
            Bootloader = bootloader;
            InitializeComponent();
            loadData();
        }
        private void loadData()
        {
            txtBuildSource.Text = Bootloader.BuildQfeSource.ToString();
            txtBuildTarget.Text = Bootloader.BuildQfeTarget.ToString();
            txtReserved.Text = Bootloader.Reserved.ToString("X8");
            txt7BLSize.Text = Bootloader.Size7BL.ToString("X");
            txtLockDown.Text = Bootloader.PerBoxData.LockDownValue.ToString("X");
            txtUpdSlot.Text = Bootloader.PerBoxData.UpdateSlotNumber.ToString();
            txtPerBoxDigest.Text = Shared.BytesToHexString(Bootloader.PerBoxData.PerBoxDigest, "");
            txtCalcPerBoxDigest.Text = Shared.BytesToHexString(Bootloader.PerBoxData.CalculateDigest(), "");
            txt7BLNonce.Text = Shared.BytesToHexString(Bootloader.Nonce7BL, "");
            txt7BLDigest.Text = Shared.BytesToHexString(Bootloader.Digest7BL, "");
            txtReserved2.Text = Shared.BytesToHexString(Bootloader.PerBoxData.Reserved, "");
            txtPairing.Text = Shared.BytesToHexString(Bootloader.PerBoxData.PairingData, "");
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            Bootloader.BuildQfeSource = uint.Parse(txtBuildSource.Text);
            Bootloader.BuildQfeTarget = uint.Parse(txtBuildTarget.Text);
            Bootloader.Reserved = uint.Parse(txtReserved.Text, NumberStyles.HexNumber);
            Bootloader.Size7BL = uint.Parse(txt7BLSize.Text, NumberStyles.HexNumber);
            Bootloader.PerBoxData.LockDownValue = byte.Parse(txtLockDown.Text, NumberStyles.HexNumber);
            Bootloader.PerBoxData.UpdateSlotNumber = byte.Parse(txtUpdSlot.Text, NumberStyles.HexNumber);
            Bootloader.PerBoxData.PerBoxDigest = Shared.HexStringToBytes(txtPerBoxDigest.Text);
            Bootloader.Nonce7BL = Shared.HexStringToBytes(txt7BLNonce.Text);
            Bootloader.Digest7BL = Shared.HexStringToBytes(txt7BLDigest.Text);
            Bootloader.PerBoxData.Reserved = Shared.HexStringToBytes(txtReserved2.Text);
            Bootloader.PerBoxData.PairingData = Shared.HexStringToBytes(txtPairing.Text);
            loadData();
        }
    }
}
