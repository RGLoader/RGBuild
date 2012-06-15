using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RGBuild
{
    public partial class CreateImageDialog : Form
    {
        public byte[] CPUKey;
        public byte[] BLKey;
        public string MfgYear;
        public int SU0Addr;
        public int BL2Addr;
        public int ImgSize;
        private bool Updating;
        public bool SwapBlockIdx;
        public CreateImageDialog()
        {
            InitializeComponent();
            UpdateSaved();
            cmb1BLType.SelectedIndex = 0;
        }
        private void UpdateSaved()
        {
            Updating = true;
            cmbSaved.Items.Clear();
            cmbSaved.Items.Add("Pre-1839");
            cmbSaved.Items.Add("New...");
            foreach (string str in Program.StoredKeys)
            {
                string[] split = str.Split(new[] { "|-|" }, StringSplitOptions.None);
                cmbSaved.Items.Add(split[0]);
            }
            Updating = false;
            cmbSaved.SelectedIndex = 0;
        }
        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
        public static int GetSizeFromStr(string str)
        {
            if (str == "16MB")
            {
                return 17301504;
            }
            if (str == "64MB")
            {
                return 69206016;
            }
            return 0;
        }
        private void cmdCreate_Click(object sender, EventArgs e)
        {
            ImgSize = GetSizeFromStr(cbImgSize.Text);
            if (String.IsNullOrEmpty(txtCPUKey.Text))
                txtCPUKey.Text = "00000000000000000000000000000000";
            if (txtCPUKey.Text.Length != 32)
            {
                MessageBox.Show("Invalid CPU Key.");
                return;
            }
            if (String.IsNullOrEmpty(txt1BLKey.Text))
            {
                MessageBox.Show("You need a 1BL key.");
                return;
            }
            if (txt1BLKey.Text.Length != 32)
            {
                MessageBox.Show("Invalid 1BL Key.");
                return;
            }
            if(ImgSize == 0)
            {
                MessageBox.Show("Invalid image size!");
                return;
            }


            string text = (string)cmbSaved.Items[cmbSaved.SelectedIndex];
            if (text != "Pre-1839" && text != "New...")
            {
                int index = -1;
                for (int i = 0; i < Program.StoredKeys.Count; i++)
                {
                    string str = Program.StoredKeys[i];
                    if (str.Split(new[] { "|-|" }, StringSplitOptions.None)[0] == text)
                    {
                        index = i;
                        break;
                    }
                }
                if (index >= 0)
                    Program.StoredKeys[index] = text + "|-|" + txtCPUKey.Text;
                else
                    Program.StoredKeys.Add(text + "|-|" + txtCPUKey.Text);

                Program.SaveStoredKeys();
            }
            if(ImgSize == 0 || String.IsNullOrEmpty(txtSU0Addr.Text) || String.IsNullOrEmpty(txt2BLAddr.Text) || String.IsNullOrEmpty(cbMfgYear.Text))
            {
                MessageBox.Show("Invalid data entered");
                return;
            }
            try
            {
                CPUKey = StringToByteArray(txtCPUKey.Text);
                BLKey = StringToByteArray(txt1BLKey.Text);
                MfgYear = cbMfgYear.Text;
                SU0Addr = int.Parse(txtSU0Addr.Text, NumberStyles.HexNumber);
                BL2Addr = int.Parse(txt2BLAddr.Text, NumberStyles.HexNumber);
                SwapBlockIdx = chkSwapBlkIdx.Checked;
            }
            catch
            {
                MessageBox.Show("Invalid data entered");
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void cmbSaved_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Updating)
            {
                string text = (string)cmbSaved.Items[cmbSaved.SelectedIndex];
                if (text == "Pre-1839")
                {
                    txtCPUKey.Text = "00000000000000000000000000000000";
                }
                else if (text == "New...")
                {
                    // open inputbox
                    string value = "";
                    if (OpenImageDialog.InputBox("New saved key", "New saved key name:", ref value) == DialogResult.OK)
                    {
                        Program.StoredKeys.Add(value + "|-|" + txtCPUKey.Text);
                        Program.SaveStoredKeys();
                        UpdateSaved();
                    }
                }
                else
                {
                    int index = -1;
                    for (int i = 0; i < Program.StoredKeys.Count; i++)
                    {
                        string str = Program.StoredKeys[i];
                        if (str.Split(new[] { "|-|" }, StringSplitOptions.None)[0] == text)
                            index = i;
                    }
                    if (index >= 0)
                    {
                        string str = Program.StoredKeys[index];
                        string[] split = str.Split(new[] { "|-|" }, StringSplitOptions.None);
                        txtCPUKey.Text = split[1];
                    }
                }
            }
        }

        private void cmb1BLType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb1BLType.SelectedIndex == 0)
            {
                txt1BLKey.Text = "DD88AD0C9ED669E7B56794FB68563EFA"; // retail
            }
            else
            {
                txt1BLKey.Text = "00000000000000000000000000000000";
            }
        }
    }
}
