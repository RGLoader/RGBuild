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
    public partial class KeyVaultControl : UserControl
    {
        private KeyVault keyVault;

        public KeyVaultControl(KeyVault KV)
        {
            keyVault = KV;
            InitializeComponent();
            loadData();
        }
        private void loadData()
        {
            txtConID.Text = Shared.BytesToHexString(keyVault.ConsoleCertificate.ConsoleId, "");
            txtCONType.Text = keyVault.ConsoleCertificate.ConsoleType.ToString();
            txtDVDKey.Text = Shared.BytesToHexString(keyVault.DVDKey, "");
            txtKVtype.Text = keyVault.Version.ToString();
            txtMFRdate.Text = keyVault.ConsoleCertificate.ManufacturingDate;
            txtOSIG.Text = keyVault.XeikaCertificate.OSIGdesc;
            txtOSIGhex.Text = Shared.BytesToHexString(keyVault.XeikaCertificate.DriveInquiry, "");

            byte[] regArray = new byte[3];
            regArray[0]= keyVault.GameRegion[1];
            regArray[1] = keyVault.GameRegion[0];

            UInt16 region = BitConverter.ToUInt16(regArray, 0);

            switch (region)
            {
                case 0x00FF:
                    ccRegionBox.SelectedIndex = 1;
                    break;
                case 0x01FE:
                    ccRegionBox.SelectedIndex = 2;
                    break;
                case 0x01FF:
                    ccRegionBox.SelectedIndex = 3;
                    break;
                case 0x01FC:
                    ccRegionBox.SelectedIndex = 4;
                    break;
                case 0x0101:
                    ccRegionBox.SelectedIndex = 5;
                    break;
                case 0x02FE:
                    ccRegionBox.SelectedIndex = 6;
                    break;
                case 0x0201:
                    ccRegionBox.SelectedIndex = 7;
                    break;
                case 0x7FFF:
                    ccRegionBox.SelectedIndex = 8;
                    break;
                default:
                    ccRegionBox.SelectedIndex = 0;
                    break;
            }


            txtSerial.Text = Encoding.ASCII.GetString(keyVault.ConsoleSerial);
            
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {

            keyVault.ConsoleCertificate.ConsoleId = Shared.HexStringToBytes(txtConID.Text);
            keyVault.ConsoleCertificate.ConsoleType = Convert.ToUInt32(txtCONType.Text);
            keyVault.DVDKey = Shared.HexStringToBytes(txtDVDKey.Text);
            keyVault.ConsoleCertificate.ManufacturingDate = txtMFRdate.Text;
            keyVault.XeikaCertificate.DriveInquiry = Shared.HexStringToBytes(txtOSIGhex.Text);
            keyVault.ConsoleSerial = Encoding.ASCII.GetBytes(txtSerial.Text);

            Console.WriteLine("Checking index: " + ccRegionBox.SelectedIndex);

            byte[] tempRegion = new byte[2];
            switch (ccRegionBox.SelectedIndex)
            {
                
                case 1:
                    tempRegion[0] = 0x00;
                    tempRegion[1] = 0xFF;
                    break;
                case 2:
                    tempRegion[0] = 0x01;
                    tempRegion[1] = 0xFE;
                    break;
                case 3:
                    tempRegion[0] = 0x01;
                    tempRegion[1] = 0xFF;
                    break;
                case 4:
                    tempRegion[0] = 0x01;
                    tempRegion[1] = 0xFC;
                    break;
                case 5:
                    tempRegion[0] = 0x01;
                    tempRegion[1] = 0x01;
                    break;
                case 6:
                    tempRegion[0] = 0x02;
                    tempRegion[1] = 0xFE;
                    break;
                case 7:
                    tempRegion[0] = 0x02;
                    tempRegion[1] = 0x01;
                    break;
                case 8:
                    tempRegion[0] = 0x7F;
                    tempRegion[1] = 0xFF;
                    break;
                default:
                    break;
            }
            keyVault.GameRegion = tempRegion;

            keyVault.XeikaCertificate.loadOSIG();
            loadData();
        }

       

    }
}
