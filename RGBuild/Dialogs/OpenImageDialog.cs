using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using RGBuild.Util;

namespace RGBuild
{
    public partial class OpenImageDialog : Form
    {
        public byte[] CPUKey;
        public byte[] BLKey;
        public static DialogResult InputBox(string title, string promptText, ref string value)
        {
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;
            label.Text = promptText;
            textBox.Text = value;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancel";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            label.SetBounds(9, 20, 372, 13);
            textBox.SetBounds(12, 36, 372, 20);
            buttonOk.SetBounds(228, 72, 75, 23);
            buttonCancel.SetBounds(309, 72, 75, 23);

            label.AutoSize = true;
            textBox.Anchor = textBox.Anchor | AnchorStyles.Right;
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.ClientSize = new Size(396, 107);
            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });
            form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            value = textBox.Text;
            return dialogResult;
        }

        private bool Updating;
        public OpenImageDialog(string path)
        {
            InitializeComponent();
            txtImage.Text = path;
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
                    if (InputBox("New saved key", "New saved key name:", ref value) == DialogResult.OK)
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

        private void cmdLoad_Click(object sender, EventArgs e)
        {
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
            try
            {
                CPUKey = Shared.HexStringToBytes(txtCPUKey.Text);
                BLKey = Shared.HexStringToBytes(txt1BLKey.Text);

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid data entered.");
                return;
            }
            DialogResult = DialogResult.OK;
        }
    }
}
