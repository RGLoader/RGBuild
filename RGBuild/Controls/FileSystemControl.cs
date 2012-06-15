using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RGBuild.NAND;

namespace RGBuild
{
    public partial class FileSystemControl : UserControl
    {
        private FileSystemRoot FileSystem;

        public FileSystemControl(FileSystemRoot filesystem)
        {
            FileSystem = filesystem;
            InitializeComponent();
            if (FileSystem == FileSystem.Image.CurrentFileSystem)
                lblNotCurrent.Visible = false;
            refreshFiles();
        }

        private void refreshFiles()
        {
            foreach (FileSystemEntry ent in FileSystem.Entries)
            {
                ListViewItem item = new ListViewItem(ent.FileName);
                //item.SubItems.Add("0x" + ent.BlockNumber.ToString("X"));
                item.SubItems.Add("0x" + ent.Size.ToString("X"));
                item.SubItems.Add("0x" + ent.Timestamp.ToString("X"));
                item.Tag = ent;
                lvFiles.Items.Add(item);
            }
        }

        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog {ShowNewFolderButton = true};
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                foreach (ListViewItem lvi in lvFiles.SelectedItems)
                {
                    FileSystemEntry entry = (FileSystemEntry) lvi.Tag;
                    File.WriteAllBytes(Path.Combine(fbd.SelectedPath, entry.FileName), entry.GetData());
                    File.WriteAllBytes(Path.Combine(fbd.SelectedPath, entry.FileName) + ".meta", entry.GetMeta());
                }
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count != 1)
                return;
            FileSystemEntry entry = (FileSystemEntry) lvFiles.SelectedItems[0].Tag;
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            byte[] data = File.ReadAllBytes(ofd.FileName);
            if(entry.Size - data.Length > FileSystem.FreeSpace)
            {
                MessageBox.Show("Not enough free space.");
                return;
            }
            entry.SetData(data);
            refreshFiles();
        }

        private void injectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            foreach (string filepath in ofd.FileNames)
            {
                if (filepath.EndsWith(".meta")) // probably an accident
                    continue;
                string filename = Path.GetFileName(filepath);
                if (filename.EndsWith("xexp") || filename.EndsWith("xttp"))
                    filename += "1";
                byte[] data = File.ReadAllBytes(filepath);
                if (data.Length > FileSystem.FreeSpace)
                {
                    MessageBox.Show("Not enough free space.");
                    return;
                }
                FileSystemEntry entry = FileSystem.AddNewEntry(filename, false);
                entry.SetData(data);
                //FileSystem.SetEntryData(entry, data);
                if(File.Exists(filepath + ".meta"))
                {
                    byte[] meta = File.ReadAllBytes(filepath + ".meta");
                    entry.SetMeta(meta);
                }
            }
            refreshFiles();
        }

        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            replaceToolStripMenuItem.Enabled = lvFiles.SelectedItems.Count == 1;
            extractToolStripMenuItem.Enabled = lvFiles.SelectedItems.Count > 0;
        }
    }
}
