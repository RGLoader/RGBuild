using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using IniParser;
using RGBuild.IO;
using RGBuild.NAND;
using RGBuild.Util;

namespace RGBuild
{
    
    public partial class Main : Form
    {
        public NANDImage Image;
        private readonly ListViewColumnSorter sorter = new ListViewColumnSorter();
        public Main()
        {
            InitializeComponent();
            lvLoaders.ListViewItemSorter = sorter;
        }

        private void openImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[][] keys = new byte[Program.StoredKeys.Count][];
                for(int i = 0; i < Program.StoredKeys.Count; i++)
                    keys[i] =
                        Shared.HexStringToBytes(Program.StoredKeys[i].Split(new[] {"|-|"}, StringSplitOptions.None)[1]);
                byte[] key = NANDImage.CheckKeysAgainstImage(ofd.FileName, keys);
                Image = new NANDImage();
                if (key == null || key.Length != 0x10)
                {
                    OpenImageDialog dia = new OpenImageDialog(ofd.FileName);
                    if (dia.ShowDialog() != DialogResult.OK)
                        return;
                    Image.CPUKey = dia.CPUKey;
                    Image._1BLKey = dia.BLKey;
                }
                else
                {
                    Image.CPUKey = key;
                }

                Image.OpenImage(ofd.FileName, 0x200);
                refreshBootloaders();
            }
        }

        private void refreshBootloaders()
        {
            lvLoaders.Items.Clear();
            if (Image.Header != null)
            {
                ListViewItem header = new ListViewItem("Header");
                header.SubItems.Add(Image.Header.Build.ToString());
                header.SubItems.Add("0x0");
                header.SubItems.Add("0x80");

                header.Tag = Image.Header;
                lvLoaders.Items.Add(header);
            }
            if (Image.Payloads != null)
            {
                ListViewItem payloads = new ListViewItem("Payload list");
                payloads.SubItems.Add(Image.Header.Build.ToString());
                payloads.SubItems.Add("0x80");
                payloads.SubItems.Add("0x" + (Image.Payloads.Length + 4).ToString("X"));
                lvLoaders.Items.Add(payloads);
                foreach (RGBPayloadEntry payload in Image.Payloads.Payloads)
                {
                    ListViewItem item = new ListViewItem(payload.Description);
                    item.SubItems.Add("Custom payload");
                    item.SubItems.Add("0x" + payload.Address.ToString("X"));
                    item.SubItems.Add("0x" + payload.Size.ToString("X"));
                    item.Tag = payload;
                    lvLoaders.Items.Add(item);
                }
            }
            if (Image.SMC != null)
            {
                ListViewItem smc = new ListViewItem("SMC");
                smc.SubItems.Add("?");
                smc.SubItems.Add("0x" + Image.Header.SmcAddress.ToString("X"));
                smc.SubItems.Add("0x" + Image.Header.SmcSize.ToString("X"));
                smc.Tag = Image.SMC;
                lvLoaders.Items.Add(smc);
            }
            if (Image.KeyVault != null)
            {
                ListViewItem kv = new ListViewItem("KeyVault");
                kv.SubItems.Add("0x" + Image.Header.KeyVaultVersion.ToString("X"));
                kv.SubItems.Add("0x" + Image.Header.KeyVaultAddress.ToString("X"));
                kv.SubItems.Add("0x" + Image.Header.KeyVaultSize.ToString("X"));
                kv.Tag = Image.KeyVault;
                lvLoaders.Items.Add(kv);
            }
            foreach (Bootloader bootloader in Image.Bootloaders)
            {
                ListViewItem item = new ListViewItem(getBootloaderName(bootloader));
                item.SubItems.Add(bootloader.Build.ToString());
                item.SubItems.Add("0x" + bootloader.Position.ToString("X"));
                item.SubItems.Add("0x" + bootloader.Size.ToString("X"));
                item.SubItems.Add(bootloader.SecureType.ToString());
                item.Tag = bootloader;
                lvLoaders.Items.Add(item);
            }
            foreach(FileSystemRoot fs in Image.FileSystems)
            {
                ListViewItem item = new ListViewItem("FileSystem");
                int loc = (int)(fs.BlockNumber * ((NANDImageStream)Image.IO.Stream).BlockLength);
                item.SubItems.Add(fs.Version.ToString());
                item.SubItems.Add("0x" + loc.ToString("X"));
                item.SubItems.Add("0x" + ((NANDImageStream) Image.IO.Stream).BlockLength.ToString("X"));
                item.Tag = fs;
                lvLoaders.Items.Add(item);
            }
            foreach(MobileXFile xfile in Image.MobileData)
            {
                ListViewItem item = new ListViewItem(xfile.Type.ToString());
                item.SubItems.Add(xfile.Version.ToString());
                int loc = xfile.StartPage * ((NANDImageStream)Image.IO.Stream).PageLength;
                int size = xfile.PageCount * ((NANDImageStream)Image.IO.Stream).PageLength;
                item.SubItems.Add("0x" + loc.ToString("X") + " (p:0x" + xfile.StartPage.ToString("X") + ")");
                item.SubItems.Add("0x" + size.ToString("X"));
                item.Tag = xfile;
                lvLoaders.Items.Add(item);
            }
           /* if (Image.CurrentFileSystem != null && Image.CurrentFileSystem.Entries != null)
            {
                foreach (FileSystemEntry entry in Image.CurrentFileSystem.Entries)
                {
                    if (entry.Deleted)
                        continue;
                    ListViewItem item = new ListViewItem(entry.FileName);
                    item.SubItems.Add("");
                    int loc = entry.BlockNumber*(int) ((NANDImageStream) Image.IO.Stream).BlockLength;
                    item.SubItems.Add("0x" + loc.ToString("X"));
                    item.SubItems.Add("0x" + entry.Size.ToString("X"));
                    item.Tag = Image.CurrentFileSystem;
                    lvLoaders.Items.Add(item);
                }
            }*/
            if(Image.ConfigBlock != null)
            {
                ListViewItem item = new ListViewItem("SMC config");
                item.SubItems.Add("");
                int loc = (int)((NANDImageStream)Image.IO.Stream).ConfigBlockStart * (int)((NANDImageStream)Image.IO.Stream).BlockLength;
                int size = 4 * (int)((NANDImageStream)Image.IO.Stream).BlockLength;
                item.SubItems.Add("0x" + loc.ToString("X") + " (b:0x" + ((NANDImageStream)Image.IO.Stream).ConfigBlockStart.ToString("X") + ")");
                item.SubItems.Add("0x" + size.ToString("X"));
                item.Tag = Image.ConfigBlock;
                lvLoaders.Items.Add(item);
            }
            lvLoaders.Sort();
        }
        private string getBootloaderName(Bootloader bootloader)
        {
            string name = bootloader.Magic.ToString();
            name = name.Replace("_", "");
            if (bootloader.GetType() == typeof(Bootloader2BL))
                if (((Bootloader2BL)bootloader).CPUKey != null)
                    name += "_B";
                else
                    name += "_A";
            return name;
        }
        private void extractToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog {ShowNewFolderButton = true};
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                
                
                foreach (ListViewItem lvi in lvLoaders.SelectedItems)
                {
                    bool use_cpu_key = true;
                    //for (int i = 0; i < 16; i++) if (Image.CPUKey[i] != 0) use_cpu_key = false;
                    if (lvi.Tag.GetType().BaseType == typeof(Bootloader))
                    {
                        Bootloader bl = (Bootloader)lvi.Tag;

                        File.WriteAllBytes(Path.Combine(fbd.SelectedPath, getBootloaderName(bl) + "." + bl.Build + ".bin"), bl.GetData(use_cpu_key));
                    }
                    else if (lvi.Tag.GetType() == typeof(KeyVault))
                    {
                        KeyVault kv = (KeyVault)lvi.Tag;
                        File.WriteAllBytes(Path.Combine(fbd.SelectedPath, "KV_dec.bin"), kv.GetData(true));
                        File.WriteAllBytes(Path.Combine(fbd.SelectedPath, "KV.bin"), kv.GetData(false));
                    }
                    else if (lvi.Tag.GetType() == typeof(SMC))
                    {
                        SMC smc = (SMC)lvi.Tag;
                        File.WriteAllBytes(Path.Combine(fbd.SelectedPath, "SMC_dec.bin"), smc.GetData(true));
                        File.WriteAllBytes(Path.Combine(fbd.SelectedPath, "SMC.bin"), smc.GetData(false));
                    }
                    else if (lvi.Tag.GetType() == typeof(MobileXFile))
                    {
                        MobileXFile file = (MobileXFile) lvi.Tag;
                        File.WriteAllBytes(Path.Combine(fbd.SelectedPath, file.Type.ToString() + ".bin"), file.GetData());
                    }
                    else if (lvi.Tag.GetType() == typeof(byte[]))
                    {
                        // config block
                        File.WriteAllBytes(Path.Combine(fbd.SelectedPath, "smc_config.bin"), ((byte[])lvi.Tag));
                    }
                }
            }
        }

        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvLoaders.SelectedItems.Count > 1)
            {
                MessageBox.Show("Only one loader can be replaced at a time.");
                return;
            }
            ListViewItem lvi = lvLoaders.SelectedItems[0];
            if (lvi.Tag.GetType().BaseType == typeof(Bootloader))
            {
                Bootloader bl = (Bootloader)lvi.Tag;
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    byte[] filedata = File.ReadAllBytes(ofd.FileName);
                    X360IO peekio = new X360IO(filedata, true);
                    NANDBootloaderMagic magic = (NANDBootloaderMagic)peekio.Reader.ReadUInt16();
                    if (magic != bl.Magic)
                        if (MessageBox.Show(
                            "You are attempting to replace with a different stage loader. Are you sure you want to do this?",
                            "Replace?", MessageBoxButtons.YesNo) == DialogResult.No)
                            return;
                    peekio.Close();
                    bl.SetData(filedata);
                    refreshBootloaders();
                }
            }
            if(lvi.Tag.GetType() == typeof(MobileXFile))
            {
                MobileXFile file = (MobileXFile) lvi.Tag;
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    byte[] filedata = File.ReadAllBytes(ofd.FileName);
                    file.SetData(filedata);
                    refreshBootloaders();
                }
            }
            if (lvi.Tag.GetType() == typeof(SMC))
            {
                SMC smc = (SMC) lvi.Tag;
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    byte[] filedata = File.ReadAllBytes(ofd.FileName);
                    if (filedata.Length != 0x3000)
                    {
                        MessageBox.Show("Invalid SMC");
                        return;
                    }
                    smc.SetData(filedata);
                    refreshBootloaders();
                }
            }
            if (lvi.Tag.GetType() == typeof(KeyVault))
            {
                KeyVault kv = (KeyVault) lvi.Tag;
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    byte[] filedata = File.ReadAllBytes(ofd.FileName);
                    //byte[] data = new byte[0x3ff0];
                    //Array.Copy(filedata, filedata.Length - 0x3ff0, data, 0x0, 0x3ff0);
                    //kv.SetData(data);
                    kv.SetData(filedata);
                    refreshBootloaders();
                }
            }
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image.SaveImage();
            Image.Close();
            if(((NANDImageStream)Image.IO.Stream).TempReturnData != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "Please choose where to save this image";
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllBytes(sfd.FileName, ((NANDImageStream)Image.IO.Stream).TempReturnData);
                }
                ((NANDImageStream)Image.IO.Stream).TempReturnData = null;
            }
            Image = null;
            // ui stuff
            lvLoaders.Items.Clear();
            scContainer.Panel2.Controls.Clear();
        }
        
        private void dumpBootloaderInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder str = new StringBuilder();
            if(Image.Header != null)
            {
                str.AppendLine("---------------------------------------------------------------------------------");
                str.AppendLine("Xbox 360 flash image, copyright" + Image.Header.Copyright);
                str.AppendLine("Build: " + Image.Header.Build + ", qfe: 0x" +
                               Image.Header.Qfe.ToString("X") + ", flags: 0x" + Image.Header.Flags.ToString("X") +
                               ", entrypoint: 0x" +
                               Image.Header.Entrypoint.ToString("X") + ", size: 0x" + Image.Header.Size.ToString("X"));
                str.AppendLine("---------------------------------------------------------------------------------");
                str.AppendLine("CPU key:");
                str.AppendLine("\t" + Shared.BytesToHexString(Image.CPUKey, " "));
                str.AppendLine();
                str.AppendLine("Reserved:");
                str.AppendLine("\t" + Shared.BytesToHexString(Image.Header.Reserved, " "));
                str.AppendLine();
                str.AppendLine("KeyVault size: 0x" + Image.Header.KeyVaultSize.ToString("X"));
                str.AppendLine();
                str.AppendLine("SysUpdate addr: 0x" + Image.Header.SysUpdateAddress.ToString("X"));
                str.AppendLine();
                str.AppendLine("SysUpdate count: " + Image.Header.SysUpdateCount);
                str.AppendLine();
                str.AppendLine("KeyVault version: 0x" + Image.Header.KeyVaultVersion.ToString("X"));
                str.AppendLine();
                str.AppendLine("KeyVault addr: 0x" + Image.Header.KeyVaultAddress.ToString("X"));
                str.AppendLine();
                str.AppendLine("FileSystem addr: 0x" + Image.Header.FileSystemAddress.ToString("X"));
                str.AppendLine();
                str.AppendLine("SMC config addr: 0x" + Image.Header.SmcConfigAddress.ToString("X"));
                str.AppendLine();
                str.AppendLine("SMC size: 0x" + Image.Header.SmcSize.ToString("X"));
                str.AppendLine();
                str.AppendLine("SMC address: 0x" + Image.Header.SmcAddress.ToString("X"));
                str.AppendLine();
            }
            foreach (Bootloader bl in Image.Bootloaders)
            {
                str.AppendLine("---------------------------------------------------------------------------------");
                str.AppendLine(getBootloaderName(bl) + " bootloader");
                str.AppendLine("Build: " + bl.Build + ", qfe: 0x" +
                               bl.Qfe.ToString("X") + ", flags: 0x" + bl.Flags.ToString("X") + ", entrypoint: 0x" +
                               bl.Entrypoint.ToString("X") + ", size: 0x" + bl.Size.ToString("X"));
                str.AppendLine("---------------------------------------------------------------------------------");
                switch (bl.Magic)
                {
                    /*
                         * 
    public byte[] Signature = new byte[0x100]; // 0x100
    public byte[] AesInvData = new byte[0x110]; // 0x110
    public byte[] RsaPublicKey = new byte[0x110]; // 0x110
                         * */
                    case NANDBootloaderMagic.CB:
                    case NANDBootloaderMagic.SB:
                        Bootloader2BL bl2 = (Bootloader2BL)bl;
                        str.AppendLine("POST output addr: 0x" + bl2.POSTOutputAddress.ToString("X") +
                                       ", Southbridge flash addr: 0x" + bl2.SbFlashAddress.ToString("X") +
                                       ", Soc MMIO addr: 0x" + bl2.SocMmioAddress.ToString("X"));
                        str.AppendLine();
                        str.AppendLine("3BL nonce:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl2.Nonce3BL, " "));
                        str.AppendLine();
                        str.AppendLine("3BL salt:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl2.Salt3BL, " "));
                        str.AppendLine();
                        str.AppendLine("4BL salt:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl2.Salt4BL, " "));
                        str.AppendLine();
                        str.AppendLine("4BL digest:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl2.Digest4BL, " "));
                        str.AppendLine();
                        str.AppendLine("Padding: 0x" + bl2.Padding.ToString("X8"));
                        str.AppendLine();
                        str.AppendLine("Console type and sequence allow data:");
                        str.AppendLine("\t" + "Console type: 0x" +
                                       bl2.ConsoleTypeSeqAllowData.ConsoleType.ToString("X2"));
                        str.AppendLine();
                        str.AppendLine("\t" + "Console sequence: 0x" +
                                       bl2.ConsoleTypeSeqAllowData.ConsoleSequence.ToString("X2"));
                        str.AppendLine();
                        str.AppendLine("\t" + "Console sequence allow: 0x" +
                                       bl2.ConsoleTypeSeqAllowData.ConsoleSequenceAllow.ToString("X4"));
                        str.AppendLine();
                        str.AppendLine("Per-box data:");
                        str.AppendLine("\t" + "Pairing data: 0x" +
                                       Shared.BytesToHexString(bl2.PerBoxData.PairingData, ""));
                        str.AppendLine();
                        str.AppendLine("\t" + "Lockdown value: 0x" +
                                       bl2.PerBoxData.LockDownValue.ToString("X"));
                        str.AppendLine();
                        str.AppendLine("\t" + "Reserved:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl2.PerBoxData.Reserved, " "));
                        str.AppendLine();
                        str.AppendLine("\t" + "Per-box digest:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl2.PerBoxData.PerBoxDigest, " "));
                        str.AppendLine();
                        break;
                    /*
                         *         public byte[] Signature = new byte[0x100]; // 0x100
    public byte[] RsaPublicKey = new byte[0x110]; // 0x110
                         * */
                    case NANDBootloaderMagic.CD:
                    case NANDBootloaderMagic.SD:
                        Bootloader4BL bl4 = (Bootloader4BL)bl;
                        str.AppendLine("6BL nonce:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl4.Nonce6BL, " "));
                        str.AppendLine();
                        str.AppendLine("6BL salt:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl4.Salt6BL, " "));
                        str.AppendLine();
                        str.AppendLine("Padding: 0x" + bl4.Padding.ToString("X8"));
                        str.AppendLine();
                        str.AppendLine("5BL digest:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl4.Digest5BL, " "));
                        str.AppendLine();
                        break;
                    case NANDBootloaderMagic.CE:
                    case NANDBootloaderMagic.SE:
                        Bootloader5BL bl5 = (Bootloader5BL)bl;
                        str.AppendLine("Image address: 0x" + bl5.ImageAddress.ToString("X16"));
                        str.AppendLine();
                        str.AppendLine("Image size: 0x" + bl5.ImageSize.ToString("X8"));
                        str.AppendLine();
                        str.AppendLine("Padding: 0x" + bl5.Padding.ToString("X8"));
                        str.AppendLine();
                        break;
                    /*
                         *         
    public byte[] Signature = new byte[0x100]; // 0x100
                         * */
                    case NANDBootloaderMagic.CF:
                    case NANDBootloaderMagic.SF:
                        Bootloader6BL bl6 = (Bootloader6BL)bl;
                        str.AppendLine("Build QFE source: " + bl6.BuildQfeSource);
                        str.AppendLine();
                        str.AppendLine("Build QFE target: " + bl6.BuildQfeTarget);
                        str.AppendLine();
                        str.AppendLine("Reserved: " + bl6.Reserved);
                        str.AppendLine();
                        str.AppendLine("7BL size: " + bl6.Size7BL);
                        str.AppendLine();
                        str.AppendLine("7BL nonce:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl6.Nonce7BL, " "));
                        str.AppendLine();
                        str.AppendLine("7BL digest:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl6.Digest7BL, " "));
                        str.AppendLine();
                        str.AppendLine("Padding: 0x" + bl6.Padding.ToString("X8"));
                        str.AppendLine();
                        str.AppendLine("7BL per-box data:");
                        str.AppendLine("\tUsed block count: " +
                                       bl6.PerBoxData7BL.UsedBlocksCount);
                        str.AppendLine();
                        str.AppendLine("\tUsed blocks:");
                        string data = "\t";
                        for (int i = 0; i < bl6.PerBoxData7BL.UsedBlocksCount; i++)
                        {
                            data += "0x" + bl6.PerBoxData7BL.BlockNumbers[i] + "  ";
                            if (i != 0 && i % 10 == 0)
                                data += "\r\n\t";
                        }
                        str.Append(data + "\r\n");
                        str.AppendLine();
                        str.AppendLine("6BL per-box data:");
                        str.AppendLine("\tReserved:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl6.PerBoxData.Reserved, " "));
                        str.AppendLine();
                        str.AppendLine("\tUpdate slot number: 0x" +
                                       bl6.PerBoxData.UpdateSlotNumber.ToString("X"));
                        str.AppendLine();
                        str.AppendLine("\tPairing data: 0x" + Shared.BytesToHexString(bl6.PerBoxData.PairingData, ""));
                        str.AppendLine();
                        str.AppendLine("\tLockdown value: 0x" + bl6.PerBoxData.LockDownValue.ToString("X"));
                        str.AppendLine();
                        str.AppendLine("\tPer-box digest:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl6.PerBoxData.PerBoxDigest, " "));
                        str.AppendLine();
                        break;
                    case NANDBootloaderMagic.CG:
                    case NANDBootloaderMagic.SG:
                        Bootloader7BL bl7 = (Bootloader7BL)bl;
                        str.AppendLine("Source image size: 0x" + bl7.SourceImageSize.ToString("X"));
                        str.AppendLine();
                        str.AppendLine("Source digest:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl7.SourceDigest, " "));
                        str.AppendLine();
                        str.AppendLine("Target image size: 0x" + bl7.TargetImageSize.ToString("X"));
                        str.AppendLine();
                        str.AppendLine("Target digest:");
                        str.AppendLine("\t" + Shared.BytesToHexString(bl7.TargetDigest, " "));
                        str.AppendLine();
                        break;
                }
            }
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(sfd.FileName, str.ToString());
            }
        }

        private void createImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateImageDialog dialog = new CreateImageDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Image = new NANDImage();
                Image.CPUKey = dialog.CPUKey;
                Image._1BLKey = dialog.BLKey;
                NANDImageStream stream = new NANDImageStream(new MemoryStream(new byte[dialog.ImgSize]), 0x200);
                //stream.SwapBlockIdx = dialog.SwapBlockIdx;
                Image.IO = new X360IO(stream, true);
                Image.CreateHeader();
                Image.Header.Entrypoint = (uint) dialog.BL2Addr;
                Image.Header.Size = (uint) dialog.SU0Addr;
                Image.Header.SysUpdateAddress = (uint) dialog.SU0Addr;
                Image.Header.Copyright = Image.Header.Copyright.Replace("2011", dialog.MfgYear);
                refreshBootloaders();
            }
        }

        
        private void addBootloaderToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Image.AddBootloaderFromPath(ofd.FileName);
                refreshBootloaders();
            }
        }

        private void lvLoaders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvLoaders.SelectedItems.Count != 1)
                return;
            ListViewItem item = lvLoaders.SelectedItems[0];
            scContainer.Panel2.Controls.Clear();
            try
            {
                if (item.Tag.GetType().BaseType == typeof(Bootloader))
                {
                    if (item.Tag.GetType() == typeof(Bootloader2BL))
                        scContainer.Panel2.Controls.Add(new Bootloader2BLControl((Bootloader2BL)item.Tag));
                    else if (item.Tag.GetType() == typeof(Bootloader6BL))
                        scContainer.Panel2.Controls.Add(new Bootloader6BLControl((Bootloader6BL)item.Tag));
                }
                else
                {
                    if (item.Tag.GetType() == typeof(FileSystemRoot))
                    {
                        scContainer.Panel2.Controls.Add(new FileSystemControl((FileSystemRoot)item.Tag));

                    }else if(item.Tag.GetType() == typeof(KeyVault)){
                        scContainer.Panel2.Controls.Add(new KeyVaultControl(Image.KeyVault));
                    }
                }
            }
            catch { }
        }

        private void lvLoaders_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            // Determine if clicked column is already the column that is being sorted.
            if (e.Column == sorter.SortColumn)
            {
                // Reverse the current sort direction for this column.
                if (sorter.Order == SortOrder.Ascending)
                {
                    sorter.Order = SortOrder.Descending;
                }
                else
                {
                    sorter.Order = SortOrder.Ascending;
                }
            }
            else
            {
                // Set the column number that is to be sorted; default to ascending.
                sorter.SortColumn = e.Column;
                sorter.Order = SortOrder.Ascending;
            }

            // Perform the sort with these new sort options.
            this.lvLoaders.Sort();
        }

        private void loadFromIniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() != DialogResult.OK)
                return;
            string ret = Program.LoadFromIni(ref Image, ofd.FileName);
            if (ret != null)
                MessageBox.Show(ret);
            refreshBootloaders();
        }

        private void addFileSystemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Image.CreateFileSystem();
            refreshBootloaders();
        }

        private void addSMCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = File.ReadAllBytes(ofd.FileName);
                if(data.Length < 0x3000)
                {
                    MessageBox.Show("SMC is incorrect size.");
                    return;
                }
                if(Image.SMC == null)
                    Image.SMC = new SMC(Image.IO);

                Image.SMC.SetData(data);
                refreshBootloaders();
            }
        }

        private void addKeyVaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = File.ReadAllBytes(ofd.FileName);
                if (data.Length != 0x4000 && data.Length != 0x3ff0)
                {
                    MessageBox.Show("KeyVault is incorrect size.");
                    return;
                }
                byte[] data2 = new byte[0x3ff0];
                Array.Copy(data, data.Length == 0x4000 ? 0x10 : 0x0, data2, 0, 0x3ff0);
                if(Image.KeyVault == null)
                    Image.KeyVault = new KeyVault(Image.IO, Image.CPUKey);

                Image.KeyVault.SetData(data);
                refreshBootloaders();
            }
        }
        
        private void addMobileFile(FileSystemExEntries type)
        {
            if(Image.CurrentFileSystem == null)
            {
                MessageBox.Show("You need a file system first...");
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = File.ReadAllBytes(ofd.FileName);
                Image.AddMobileFile(data, type);
                
                refreshBootloaders();
            }
        }
        private void addMobileBToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addMobileFile(FileSystemExEntries.MobileB);
        }

        private void addMobileCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addMobileFile(FileSystemExEntries.MobileC);
        }

        private void addMobileDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addMobileFile(FileSystemExEntries.MobileD);
        }

        private void addMobileEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addMobileFile(FileSystemExEntries.MobileE);
        }

        private void addMobileFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addMobileFile(FileSystemExEntries.MobileF);
        }

        private void addMobileGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addMobileFile(FileSystemExEntries.MobileG);
        }

        private void addMobileHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addMobileFile(FileSystemExEntries.MobileH);
        }

        private void addMobileIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addMobileFile(FileSystemExEntries.MobileI);
        }

        private void addMobileJToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addMobileFile(FileSystemExEntries.MobileJ);
        }

        private void addSMCConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Image.CurrentFileSystem == null)
            {
                MessageBox.Show("You need a file system first...");
                return;
            }
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = File.ReadAllBytes(ofd.FileName);
                if (data.Length != 0x10000)
                {
                    MessageBox.Show("This SMC config is invalid.");
                    return;
                }
                Image.ConfigBlock = data;
                refreshBootloaders();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void closeImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Image == null)
                return; // already closed

            ((NANDImageStream)Image.IO.Stream).Close(false);
            Image = null;
            lvLoaders.Items.Clear();
            scContainer.Panel2.Controls.Clear();

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("RGBuild "+Program.Version+"\nby stoker25, tydye81 and #RGLoader@EFnet");
        }

        private void addPayloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddPayloadDialog dia = new AddPayloadDialog();
            if (dia.ShowDialog() != DialogResult.OK)
                return;
            Image.AddPayload(dia.Description, dia.Address, File.ReadAllBytes(dia.FilePath));
            refreshBootloaders();
        }

        private void decompressFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = File.ReadAllBytes(ofd.FileName);
                File.WriteAllBytes(ofd.FileName + ".dec", XCompress.DecompressInChunks(data, 0x30));
            }
        }

        private void decompressPatchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Select base kernel 1888 (CE, NOT DECOMPRESSED)";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] data = File.ReadAllBytes(ofd.FileName);
                data = XCompress.DecompressInChunks(data, 0x30);
                ofd.Title = "Select CG patch";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    byte[] patchdata = File.ReadAllBytes(ofd.FileName);
                    byte[] trgtsize = new byte[] { patchdata[0x3B], patchdata[0x3A], patchdata[0x39], patchdata[0x38] };
                    uint size = BitConverter.ToUInt32(trgtsize, 0);
                    byte[] patcheddata = XCompress.DecompressPatchInChunks(data, patchdata, size);
                    SaveFileDialog sfd = new SaveFileDialog();
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllBytes(sfd.FileName, patcheddata);
                    }
                }
            }

        }

        private void extractBaseKernelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bootloader5BL bl5 = (Bootloader5BL)Image.Bootloaders.Find(f => f.GetType() == typeof(Bootloader5BL));
            if (bl5 == null)
            {
                MessageBox.Show("Not enough bootloaders for kernel");
                return;
            }
            byte[] kerneldata = XCompress.DecompressInChunks(bl5.GetData(), 0x30);
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            File.WriteAllBytes(sfd.FileName, kerneldata);
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //byte[] data = new byte[] { 1, 3, 3, 7 };
            //byte[] data2 = new byte[4];
            //IntPtr unmanagedPointer = Marshal.AllocHGlobal(data.Length);
            //Marshal.Copy(data, 0, unmanagedPointer, data.Length);
            //IntPtr unmanagedPointer2 = Marshal.AllocHGlobal(data2.Length);
            //Marshal.Copy(data, 0, unmanagedPointer2, data2.Length);
            //XeCryptWrapper.XeCrypt crypt = new XeCryptWrapper.XeCrypt();
            //UnmanagedType.
            //crypt.BnDw_Copy((uint*)byte[])
            //XeCrypt.BnDw_Copy(unmanagedPointer, unmanagedPointer2, 1);
            //Marshal.
            //uint[] arr1 = new uint[] { 1, 2, 3, 4 };
            //uint[] arr2 = new uint[] { 0, 0, 0, 0 };
            byte[] darr1 = new byte[] { 1, 2, 3, 4 };
            byte[] darr2 = new byte[] { 0, 0, 0, 0 };
            //XeCrypt.DesParity(ref darr1, 4, ref darr2);
            IntPtr unmanagedPointer = Marshal.AllocHGlobal(darr1.Length * 2);
            IntPtr ptr2 = unmanagedPointer + 4;
            Marshal.Copy(darr1, 0, unmanagedPointer, darr1.Length);
            XeCrypt.DesParity(ref unmanagedPointer, 4, ref ptr2);
            // Call unmanaged code
            Marshal.FreeHGlobal(unmanagedPointer);
        }

        private void scContainer_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.Text = "RGBuild "+Program.Version;
        }

        
    }
}