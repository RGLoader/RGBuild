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
    public partial class AddPayloadDialog : Form
    {
        public string FilePath;
        public uint Address;
        public string Description;
        public AddPayloadDialog()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FilePath = txtPayload.Text;
            Description = txtDescription.Text;
            if(String.IsNullOrEmpty(FilePath))
            {
                MessageBox.Show("Specify a path!");
                return;
            }
            if(String.IsNullOrEmpty(Description))
            {
                MessageBox.Show("Specify a description!");
                return;
            }
            if(Description.Length >= 255)
            {
                MessageBox.Show("Description is too large.");
                return;
            }
            try
            {
                Address = uint.Parse(txtAddress.Text, NumberStyles.HexNumber);
            }
            catch
            {
                MessageBox.Show("Invalid address entered.");
                return;
            }
            DialogResult = DialogResult.OK;
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            txtPayload.Text = ofd.FileName;
        }
    }
}
