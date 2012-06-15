using System;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;

namespace RGBuild
{
    public partial class KeyManagerDialog : Form
    {
        private int selectedIndex;
        private bool updating;
        public KeyManagerDialog()
        {
            InitializeComponent();
            UpdateStored();
        }

        private void UpdateStored()
        {
            updating = true;
            lbStored.Items.Clear();
            lbStored.Items.Add("New...");
            foreach (string str in Program.StoredKeys)
            {
                string[] split = str.Split(new[] { "|-|" }, StringSplitOptions.None);
                lbStored.Items.Add(split[0]);
            }
            Program.SaveStoredKeys();
            updating = false;
            lbStored.SelectedIndex = 0;
        }

        private void lbStored_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!updating)
            {
                selectedIndex = lbStored.SelectedIndex;
                if (selectedIndex > 0)
                {
                    string[] split = Program.StoredKeys[selectedIndex - 1].Split(new[] { "|-|" }, StringSplitOptions.None);
                    txtName.Text = split[0];
                    txtCPUKey.Text = split[1];
                }
                else
                {
                    txtName.Text = "00000000000000000000000000000000";
                    txtCPUKey.Text = "00000000000000000000000000000000";
                }
            }

        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            if (selectedIndex > 0)
            {
                if (String.IsNullOrEmpty(txtCPUKey.Text))
                    txtCPUKey.Text = "00000000000000000000000000000000";
                if (txtCPUKey.Text.Length != 32)
                {
                    MessageBox.Show("Invalid CPU Key.");
                    return;
                }
                Program.StoredKeys[selectedIndex - 1] = txtName.Text + "|-|" + txtCPUKey.Text;
            }
            else
            {
                if (String.IsNullOrEmpty(txtCPUKey.Text))
                    txtCPUKey.Text = "00000000000000000000000000000000";
                if (txtCPUKey.Text.Length != 32)
                {
                    MessageBox.Show("Invalid CPU Key.");
                    return;
                }
                int index = -1;
                for (int i = 0; i < Program.StoredKeys.Count; i++)
                {
                    string str = Program.StoredKeys[i];
                    if (str.Split(new[] { "|-|" }, StringSplitOptions.None)[0] == txtName.Text)
                        index = i;
                }
                if (index >= 0)
                {
                    MessageBox.Show("Can't create, item with same name exists!");
                    return;
                }
                else
                    Program.StoredKeys.Add(txtName.Text + "|-|" + txtCPUKey.Text);
                
                UpdateStored();
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            if (selectedIndex > 0)
            {
                if (MessageBox.Show("Are you SURE you want to delete this key?", "Really?", MessageBoxButtons.YesNo) ==
                    DialogResult.Yes)
                {
                    Program.StoredKeys.RemoveAt(selectedIndex - 1);

                    UpdateStored();
                }
            }
        }
    }
}
