using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using RGBuild.IO;
using RGBuild.Util;

namespace RGBuild.NAND
{
    public enum NANDBootloaderMagic : ushort
    {
        FlashHeader = 0xFF4F,
        _1BL = 0x0341,
        CB = 0x4342,
        CD = 0x4344,
        CE = 0x4345,
        CF = 0x4346,
        CG = 0x4347,
        SB = 0x5342,
        SC = 0x5343,
        SD = 0x5344,
        SE = 0x5345,
        SF = 0x5346,
        SG = 0x5347
    }
    public class MobileXFile
    {
        public NANDImage Image;
        public FileSystemExEntries Type;
        public int StartPage;
        public int PageCount
        {
            get
            {
                return Data.Length/((NANDImageStream) Image.IO.Stream).PageLength +
                       (Data.Length%((NANDImageStream) Image.IO.Stream).PageLength > 0 ? 1 : 0);
            }
        }
        public byte[] Data;
        public int Length;
        public int Version;
        public List<ISpareData> PageEdc;

        public MobileXFile(NANDImage image)
        {
            Image = image;
        }
        public void Read()
        {
            Read(Image.IO, Length);
        }

        public void Read(X360IO io, int length)
        {
            Data = io.Reader.ReadBytes(length);
            Length = length;
        }

        public byte[] GetData()
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io, data.Length);
            return;
        }

        public void Write(X360IO io)
        {
            io.Writer.Write(Data);
            Length = Data.Length;
            if (io.Stream.GetType() == typeof(NANDImageStream))
            {
                io.Stream.Position -= Length;
                StartPage = ((NANDImageStream)io.Stream).CurrentPageNumber;
                // fix edc for mobile pages
                for (int i = 0; i < PageCount; i++)
                {
                    ISpareData data = ((NANDImageStream) io.Stream).GetPageSpare(i + StartPage);
                    data.FsSequence = Version;
                    data.FsSize = (short)Length;
                    data.FsPageCount = (byte)(((NANDImageStream)io.Stream).PagesPerBlock - PageCount);
                    data.FsBlockType = (byte)(data.FsBlockType | (byte)Type);
                    ((NANDImageStream) io.Stream).WritePageSpare(i + StartPage, data);
                }
                io.Stream.Position += Data.Length;
            }
        }

        public void Write()
        {
            Write(Image.IO);
        }


    }
    public class NANDImage
    {
        public byte[] _1BLKey = new byte[] { 0xDD, 0x88, 0xAD, 0x0C, 0x9E, 0xD6, 0x69, 0xE7, 0xB5, 0x67, 0x94, 0xFB, 0x68, 0x56, 0x3E, 0xFA };
        public byte[] CPUKey = new byte[16];

        public X360IO IO;
        public BootloaderFlashHeader Header;
        public List<Bootloader> Bootloaders = new List<Bootloader>();
        public KeyVault KeyVault;
        public SMC SMC;
        public RGBPayloadList Payloads;
        public List<FileSystemRoot> FileSystems = new List<FileSystemRoot>();
        public List<MobileXFile> MobileData = new List<MobileXFile>();
        public List<XeKeysSecuredFile> SecuredFiles = new List<XeKeysSecuredFile>();
        public FileSystemRoot CurrentFileSystem
        {
            get
            {
                try
                {
                    return FileSystems[FileSystems.Count - 1];

                }
                catch
                {
                    return null;
                }
            }
        }

        public byte[] ConfigBlock;
        public void LoadConfigBlocks()
        {
            if (((NANDImageStream)IO.Stream).ConfigBlockStart <= 0)
                return;
            ((NANDImageStream)IO.Stream).SeekToBlock(((NANDImageStream)IO.Stream).ConfigBlockStart);
            ConfigBlock = IO.Reader.ReadBytes((int)(((NANDImageStream) IO.Stream).BlockLength*4));
        }
        public void SaveConfigBlocks()
        {
            if (ConfigBlock == null || ((NANDImageStream)IO.Stream).ConfigBlockStart <= 0) //CurrentFileSystem == null || 
                return;

            //if (Header.ConfigBlockStart == 0) // need to allocate it some blocks
            //    Header.ConfigBlockStart = CurrentFileSystem.AllocateNewBlock(4, (ushort)(((NANDImageStream)IO.Stream).BlockCount - 0x30));
            
            for (int i = 0; i < 5; i++)
                CurrentFileSystem.BlockMap[((NANDImageStream)IO.Stream).ConfigBlockStart + i] = 0x1ffb;

            ((NANDImageStream)IO.Stream).SeekToBlock((int)((NANDImageStream)IO.Stream).ConfigBlockStart);
            IO.Writer.Write(ConfigBlock);
        }
        public static byte[] CheckKeysAgainstImage(string path, byte[][] keys)
        {
            NANDImage img = new NANDImage
                                {
                                    IO = new X360IO(new NANDImageStream(path, FileMode.Open, 0x200), true)
                                };
            img.LoadHeader();
            foreach (byte[] t in keys)
            {
                img.CPUKey = t;
                try
                {
                    img.LoadKeyVault();
                    if (img.KeyVault.IsNonceValid)
                    {
                        img.IO.Close();
                        return t;
                    }
                }
                catch { }
                
            }
            img.IO.Close();
            return null;
        }
        public bool OpenImage(string path, int pageLength)
        {
            Console.WriteLine("Opening image from " + path);
            IO = new X360IO(new NANDImageStream(path, FileMode.Open, pageLength), true);
            Console.WriteLine("Loading image header...");
            LoadHeader();
            Console.WriteLine("Loading bootloaders...");
            LoadBootloaders();
            Console.WriteLine("Loading keyvault...");
            LoadKeyVault();
            Console.WriteLine("Loading SMC...");
            LoadSMC();
            if(Header.ContainsRGBP)
                LoadPayloadList();
            if (((NANDImageStream)IO.Stream).SpareDataType != SpareDataType.None)
            {
                Console.WriteLine("Loading filesystems...");
                LoadFileSystem();
                //LoadSecuredFiles();
                Console.WriteLine("Loading config blocks...");
                LoadConfigBlocks();
            }
            //bool bewl = Bootloaders[1].VerifyLoader(Bootloaders[2]);
            //byte[] sig = KeyVault.Signature;
            //byte[] sig2 = KeyVault.GenerateSignature();
            //File.WriteAllBytes("C:\\secdata.bin", SecurityFileDecrypt(CurrentFileSystem.Entries.Find(f => f.FileName == "secdata.bin").GetData()));
            return true;
        }
        public void LoadSecuredFiles()
        {
            if (CurrentFileSystem == null)
                return;

            List<FileSystemEntry> secentries =
                CurrentFileSystem.Entries.FindAll(
                    sec =>
                    sec.FileName == "secdata.bin" || sec.FileName == "fcrt.bin" || sec.FileName == "crl.bin" ||
                    sec.FileName == "dae.bin" || sec.FileName == "extended.bin");
            foreach(FileSystemEntry ent in secentries)
            {

                Console.WriteLine("In file loop");
                X360IO io = new X360IO(ent.GetData(), true);
                Console.WriteLine("After get data");
                switch(ent.FileName)
                {
                    case "secdata.bin":
                        Console.WriteLine("Found secdata.bin, making datafile object");
                        SecurityDataFile secdata = new SecurityDataFile(CPUKey);
                        Console.WriteLine("Reading data into object");
                        secdata.Read(io, (int)io.Stream.Length, false);

                        SecuredFiles.Add(secdata);
                        break;
                    case "extended.bin":
                        Console.WriteLine("Found extended.bin, making object");
                        ExtendedKeyVault ext = new ExtendedKeyVault(CPUKey);
                        ext.Read(io, (int)io.Stream.Length, false);
                        Console.WriteLine("Reading data into object");
                        SecuredFiles.Add(ext);
                        break;
                }
                Console.WriteLine("After switch");
                io.Close();
            }
        }

        public byte[] SecurityFileDecrypt(byte[] data)
        {
            byte[] nonce = new byte[0x10];
            byte[] fdata = new byte[data.Length - 0x10];
            Array.Copy(data, 0, nonce, 0, 0x10);
            Array.Copy(data, 0x10, fdata, 0, fdata.Length);
            Tuple<byte[], byte[]> hmacrc4 = Shared.HmacRc4(CPUKey, nonce, fdata);
            return hmacrc4.Item1;
        }
        public void CreateFileSystem()
        {
            // TODO: fix mobile data reading, make it so that it checks pages left in block in ecc data and checks size of file in ecc data (i did this wrong!)
            //int blockidx = 0x3d2;
            int blockidx = 0x110;
            
            byte ver = 0x3;
            if(CurrentFileSystem != null)
            {
                blockidx = CurrentFileSystem.AllocateNewBlock(1, (ushort)blockidx);
                ver = (byte)(CurrentFileSystem.Version + 1);
            }
            FileSystemRoot fs = new FileSystemRoot(this, blockidx, ver);
            fs.CreateDefaults();
            if(CurrentFileSystem != null)
            {
                fs.CopyData(CurrentFileSystem);
            }
            FileSystems.Add(fs);
        }
        private static bool CheckFlag(byte data, byte flag)
        {
            return (data & 0x3F) == flag;
        }
        public void SaveFileSystem()
        {
            if(CurrentFileSystem != null)
            {
                foreach (FileSystemEntry ent in CurrentFileSystem.Entries.FindAll(sec => sec.Deleted))
                    CurrentFileSystem.Entries.Remove(ent);
                CurrentFileSystem.Write();

                foreach (MobileXFile mobile in MobileData)
                {
                    // allocate its block

                    if (mobile.StartPage == 0)
                        mobile.StartPage = CurrentFileSystem.AllocateNewBlock() * ((NANDImageStream)IO.Stream).PagesPerBlock;

                    int block = mobile.StartPage / ((NANDImageStream)IO.Stream).PagesPerBlock;
                    CurrentFileSystem.BlockMap[block] = 0x1ffb;

                    ((NANDImageStream)IO.Stream).SeekToPage(mobile.StartPage);
                    mobile.Write();
                }
                CurrentFileSystem.Write();
            }
        }
        public void LoadFileSystem()
        {
            FileSystems = new List<FileSystemRoot>();
            int blocks = ((NANDImageStream) IO.Stream).BlockCount;
            for (int i = 0; i < blocks; i++)
            {
                ISpareData ecc = ((NANDImageStream) IO.Stream).GetBlockSpare(i);
                if (ecc.FsSequence != 0 && (CheckFlag(ecc.FsBlockType, (byte)FileSystemExEntries.FsRootEntry) || CheckFlag(ecc.FsBlockType, (byte)FileSystemExEntries.FsRootEntryAlt)))
                //if ((ecc.BlockId > 0 || CheckFlag(ecc.FsBlockType, (byte)FileSystemExEntries.FsRootEntry)))
                {
                    Console.WriteLine("* Found filesystem @ 0x" + (i * ((NANDImageStream)IO.Stream).BlockLength).ToString("X"));
                    FileSystemRoot root = new FileSystemRoot(this, i, ecc.FsSequence);
                    root.Read();
                    FileSystems.Add(root);
                    if(root.Entries.FindAll(sec => sec.FileName == "mfgbootlauncher.xex").Count > 0)
                    {
                        root = root;
                    }
                }
            }
            FileSystems.Sort((p1, p2) => p1.Version.CompareTo(p2.Version) | p1.BlockNumber.CompareTo(p2.BlockNumber));
            foreach (FileSystemRoot fs in FileSystems)
            
                CurrentFileSystem.BlockMap[fs.BlockNumber] = 0x1ffb;
            
            foreach (MobileXFile file in MobileData)
                CurrentFileSystem.BlockMap[(file.StartPage / ((NANDImageStream)IO.Stream).PagesPerBlock)] = 0x1fff;

            if (((NANDImageStream)IO.Stream).ConfigBlockStart > 0 && ConfigBlock != null)
                for (int i = 0; i < 5; i++)
                    CurrentFileSystem.BlockMap[i + ((NANDImageStream)IO.Stream).ConfigBlockStart] = 0x1ffb;

            // lets look for mobile files now... hacky way
            List<Tuple<int, FileSystemExEntries, ISpareData>> foundMobiles = new List<Tuple<int, FileSystemExEntries, ISpareData>>();
            for (int i = 0; i < ((NANDImageStream)IO.Stream).PageCount; i++)
            {
                ISpareData edcdata = ((NANDImageStream)IO.Stream).GetPageSpare(i);

                byte fspagecount = edcdata.FsPageCount;
                byte pageflags = (byte)((edcdata.FsBlockType << 2) >> 2); // clear the two edc bits
                if (fspagecount == 0)
                    continue;

                if ((pageflags & (byte)FileSystemExEntries.FsRootEntry) == (byte)FileSystemExEntries.FsRootEntry)
                {
                    if (CheckFlag(pageflags, (byte)FileSystemExEntries.MobileB))
                        foundMobiles.Add(new Tuple<int, FileSystemExEntries, ISpareData>(i, FileSystemExEntries.MobileB, edcdata));

                    else if (CheckFlag(pageflags, (byte)FileSystemExEntries.MobileC))
                        foundMobiles.Add(new Tuple<int, FileSystemExEntries, ISpareData>(i, FileSystemExEntries.MobileC, edcdata));

                    else if (CheckFlag(pageflags, (byte)FileSystemExEntries.MobileD))
                        foundMobiles.Add(new Tuple<int, FileSystemExEntries, ISpareData>(i, FileSystemExEntries.MobileD, edcdata));

                    else if (CheckFlag(pageflags, (byte)FileSystemExEntries.MobileE))
                        foundMobiles.Add(new Tuple<int, FileSystemExEntries, ISpareData>(i, FileSystemExEntries.MobileE, edcdata));

                    else if (CheckFlag(pageflags, (byte)FileSystemExEntries.MobileF))
                        foundMobiles.Add(new Tuple<int, FileSystemExEntries, ISpareData>(i, FileSystemExEntries.MobileF, edcdata));

                    else if (CheckFlag(pageflags, (byte)FileSystemExEntries.MobileG))
                        foundMobiles.Add(new Tuple<int, FileSystemExEntries, ISpareData>(i, FileSystemExEntries.MobileG, edcdata));

                    else if (CheckFlag(pageflags, (byte)FileSystemExEntries.MobileH))
                        foundMobiles.Add(new Tuple<int, FileSystemExEntries, ISpareData>(i, FileSystemExEntries.MobileH, edcdata));

                    else if (CheckFlag(pageflags, (byte)FileSystemExEntries.MobileI))
                        foundMobiles.Add(new Tuple<int, FileSystemExEntries, ISpareData>(i, FileSystemExEntries.MobileI, edcdata));

                    else if (CheckFlag(pageflags, (byte)FileSystemExEntries.MobileJ))
                        foundMobiles.Add(new Tuple<int, FileSystemExEntries, ISpareData>(i, FileSystemExEntries.MobileJ, edcdata));
                    else
                        continue;
                }
            }
            MobileData = new List<MobileXFile>();
            foreach (Tuple<int, FileSystemExEntries, ISpareData> mobile in foundMobiles)
            {
                int page = mobile.Item1;
                FileSystemExEntries type = mobile.Item2;
                ISpareData edcdata = mobile.Item3;
                /*
                 * 0x7		1 byte			byte			FS Size 0								Cert. Size = ((FS Size 0<<8))+FS Size 1 (used for Mobile*.dat)
                    0x8		1 byte			byte			FS Size 1
*/
                int datalength = edcdata.FsSize;
                int pagecount = datalength/((NANDImageStream) IO.Stream).PageLength + (
                    (datalength%((NANDImageStream) IO.Stream).PageLength) > 0 ? 1 : 0);
                int pagecount2 = ((NANDImageStream) IO.Stream).PagesPerBlock - edcdata.FsPageCount;
                if (pagecount != pagecount2 && pagecount == 1 && datalength >= 0x200)
                    continue;
                MobileXFile file = MobileData.Find(f => f.Type == type);
                if(file == null)
                {
                    file = new MobileXFile(this);
                    // check if each of the pages it says it uses are valid mobile pages
                    List<Tuple<int, FileSystemExEntries, ISpareData>> pagess =
                        foundMobiles.FindAll(sec => sec.Item2 == type && sec.Item1 < (page + pagecount) && sec.Item1 >= page);
                    if (pagess.Count != pagecount)
                    {
                        continue;
                    }

                    bool cont = false;
                    List<ISpareData> pageedc = new List<ISpareData>();
                    foreach (Tuple<int, FileSystemExEntries, ISpareData> mobpage in pagess)
                    {
                        pageedc.Add(mobpage.Item3);
                        int mobpagecount2 = ((NANDImageStream) IO.Stream).PagesPerBlock - mobpage.Item3.FsPageCount;
                        if (mobpagecount2 != pagecount2)
                        {
                            cont = true;
                            break;
                        }
                    }
                    if (cont)
                        continue;
                    file.PageEdc = pageedc;
                    file.Type = type;
                    file.StartPage = page;
                    file.Length = datalength;
                    file.Version = edcdata.FsSequence;
                    MobileData.Add(file);
                }
                else
                {
                    if (file.StartPage + file.PageCount > page)
                        continue;

                    // check if each of the pages it says it uses are valid mobile pages
                    List<Tuple<int, FileSystemExEntries, ISpareData>> pagess =
                        foundMobiles.FindAll(sec => sec.Item2 == type && sec.Item1 < (page + pagecount) && sec.Item1 >= page);
                    if (pagess.Count != pagecount)
                    {
                       continue;
                    }

                    bool cont = false;
                    List<ISpareData> pageedc = new List<ISpareData>();
                    foreach (Tuple<int, FileSystemExEntries, ISpareData> mobpage in pagess)
                    {
                        pageedc.Add(mobpage.Item3);
                        int mobpagecount2 = ((NANDImageStream) IO.Stream).PagesPerBlock - mobpage.Item3.FsPageCount;
                        if(mobpagecount2 != pagecount2)
                        {
                            cont = true;
                            break;
                        }
                    }
                    if (cont)
                        continue;

                    file.StartPage = page;
                    file.Length = datalength;
                    file.PageEdc = pageedc;
                    file.Version = edcdata.FsPageCount;
                }
                foreach (MobileXFile xfile in MobileData)
                {
                    ((NANDImageStream)IO.Stream).SeekToPage(xfile.StartPage);
                    xfile.Read();
                    if (CurrentFileSystem != null)
                        CurrentFileSystem.BlockMap[xfile.StartPage / ((NANDImageStream)IO.Stream).PagesPerBlock] = 0x1ffb;
                }
            }
            foreach (MobileXFile xfile in MobileData)
            {
                Console.WriteLine("* Found " + xfile.Type.ToString() + " @ 0x" + (xfile.StartPage * 0x200).ToString("X"));
            }
            if (CurrentFileSystem == null)
                return;

            //correct the FS block offset stuff...
            foreach(FileSystemEntry ent in CurrentFileSystem.Entries.FindAll(sec => sec.FileName.EndsWith(".xex") && !sec.Deleted))
            {
                byte[] xex2 = {0, 0, 0, 0};
                if (((NANDImageStream)IO.Stream).BlockCount >= ent.BlockNumber + 0xae0)
                {
                    ((NANDImageStream) IO.Stream).SeekToBlock(ent.BlockNumber + 0xae0);
                    xex2 = IO.Reader.ReadBytes(4);
                    if (xex2[0] == 0x58 && xex2[1] == 0x45 && xex2[2] == 0x58 && (xex2[3] == 0x32 || xex2[3] == 0x31))
                    {
                        CurrentFileSystem.BlockOffset = 0xae0;
                        break;
                    }
                }

                if (((NANDImageStream)IO.Stream).BlockCount >= ent.BlockNumber + 0x2e0)
                {
                    ((NANDImageStream) IO.Stream).SeekToBlock(ent.BlockNumber + 0x2e0);
                    xex2 = IO.Reader.ReadBytes(4);
                    if (xex2[0] == 0x58 && xex2[1] == 0x45 && xex2[2] == 0x58 && (xex2[3] == 0x32 || xex2[3] == 0x31))
                    {
                        CurrentFileSystem.BlockOffset = 0x2e0;
                        break;
                    }
                }

                ((NANDImageStream) IO.Stream).SeekToBlock(ent.BlockNumber);
                xex2 = IO.Reader.ReadBytes(4);
                if (xex2[0] == 0x58 && xex2[1] == 0x45 && xex2[2] == 0x58 && (xex2[3] == 0x32 || xex2[3] == 0x31))
                    CurrentFileSystem.BlockOffset = 0;
                
                break;
            }

            foreach (FileSystemEntry ent in CurrentFileSystem.Entries.FindAll(sec => !sec.Deleted))
            {
                ent.SetData(CurrentFileSystem.GetEntryData(ent));
                CurrentFileSystem.FreeBlockChain(ent.BlockNumber);
                ent.BlockNumber = 0;
            }
        }

        public void LoadKeyVault()
        {
            if (Header.KeyVaultAddress == 0)
                Header.KeyVaultAddress = 0x4000;
            if (Header.KeyVaultSize == 0)
                Header.KeyVaultSize = 0x4000;

            IO.Stream.Position = Header.KeyVaultAddress;
            KeyVault = new KeyVault(IO, CPUKey);
            KeyVault.Read(Header.KeyVaultSize);
        }

        public void SaveKeyVault()
        {
            if (KeyVault != null)
            {
                IO.Stream.Position = Header.KeyVaultAddress;
                KeyVault.Write();
            }
        }
        public void SaveSMC()
        {
            if(SMC != null)
            {
                Header.SmcAddress = (uint)(0x4000 - SMC.GetData(true).Length);
                IO.Stream.Position = Header.SmcAddress;
                SMC.Write();
            }
        }
        public void AddMobileFile(byte[] data, FileSystemExEntries type)
        {
            MobileXFile file = new MobileXFile(this) { Type = type, StartPage = 0 };
            file.SetData(data);
            file.Version = 3;
            if (MobileData == null)
                MobileData = new List<MobileXFile>();

            MobileData.Add(file);
        }
        public string AddBootloaderFromPath(string path)
        {
            int stage = Bootloaders.Count;

            if (stage == 7)
            {
                return "You can't add any more loaders to this ";
            }

            if (!File.Exists(path))
            {
                return "Unable to load bldr from: " + path;
            }

            X360IO io = new X360IO(path, FileMode.Open, true);
            NANDBootloaderMagic magic = (NANDBootloaderMagic)io.Reader.ReadUInt16();

            io.Stream.Position -= 2;
            int stg = stage;
            if (stg > 0 && Bootloaders[0].GetType() == typeof(Bootloader1BL))
                stg--;
            byte[] data = io.Reader.ReadBytes((int)io.Stream.Length);

            switch (magic)
            {
                case NANDBootloaderMagic.CB:
                case NANDBootloaderMagic.SB:
                    Bootloader2BL bl2 = stg == 0
                                            ? new Bootloader2BL(this, null)
                                            : new Bootloader2BL(this, Bootloaders[Bootloaders.Count - 1],
                                                                CPUKey);
                    bl2.SetData(data);
                    Bootloaders.Add(bl2);
                    break;
                case NANDBootloaderMagic.SC:
                    Bootloader3BL bl3 = new Bootloader3BL(this, Bootloaders[Bootloaders.Count - 1]);
                    bl3.SetData(data);
                    Bootloaders.Add(bl3);
                    break;
                case NANDBootloaderMagic.CD:
                case NANDBootloaderMagic.SD:
                    Bootloader4BL bl4 = new Bootloader4BL(this, Bootloaders[Bootloaders.Count - 1]);
                    bl4.SetData(data);
                    Bootloaders.Add(bl4);
                    break;
                case NANDBootloaderMagic.CE:
                case NANDBootloaderMagic.SE:
                    Bootloader5BL bl5 = new Bootloader5BL(this, Bootloaders[Bootloaders.Count - 1]);
                    bl5.SetData(data);
                    Bootloaders.Add(bl5);
                    break;
                case NANDBootloaderMagic.CF:
                case NANDBootloaderMagic.SF:
                    Bootloader6BL bl6 = new Bootloader6BL(this);
                    bl6.SetData(data);
                    Bootloaders.Add(bl6);
                    break;
                case NANDBootloaderMagic.CG:
                case NANDBootloaderMagic.SG:
                    Bootloader7BL bl7 = new Bootloader7BL(this, Bootloaders[Bootloaders.Count - 1]);
                    bl7.SetData(data);
                    Bootloaders.Add(bl7);
                    break;
            }
            return null;
        }
        public RGBPayloadEntry AddPayload(string description, uint address, byte[] data)
        {
            RGBPayloadEntry payload = new RGBPayloadEntry {Description = description, Address = address, Data = data, Size = (uint)data.Length};
            if(Payloads == null)
            {
                if (Header == null)
                    return null;
                Header.RGBPIndicator = 0x1337;
                Payloads = new RGBPayloadList(IO);
            }
            Payloads.Payloads.Add(payload);
            return payload;
        }
        public void LoadPayloadList()
        {
            IO.Stream.Position = 0x100;
            Payloads = new RGBPayloadList(IO);
            Payloads.Read();
        }
        public void SavePayloadList()
        {
            if(Payloads != null)
            {
                //reserve blocks for payloads
               /* foreach(RGBPayloadEntry entry in Payloads.Payloads)
                {
                    int startblk = (int)(entry.Address / ((NANDImageStream)IO.Stream).BlockLength);
                    int numblks =(int)((entry.Size / ((NANDImageStream)IO.Stream).BlockLength) + (entry.Size % ((NANDImageStream)IO.Stream).BlockLength > 0 ? 1 : 0));
                    for(int i=startblk; i<numblks; i++)
                        CurrentFileSystem.BlockMap[i] = 0x1ffb;
                }*/

                IO.Stream.Position = 0x100;
                Payloads.Write();
            }
        }
        public bool SaveImage()
        {
            // must be saved before filesystem


            Console.WriteLine("Writing bootloaders...");
            SaveBootloaders(Header.Entrypoint, Header.SysUpdateAddress);
            Console.WriteLine(" Pages per block: " + ((NANDImageStream)IO.Stream).PagesPerBlock);

            if (Payloads != null)
            {
                foreach (RGBPayloadEntry entry in Payloads.Payloads)
                {
                    int startblk = (int)(entry.Address / ((NANDImageStream)IO.Stream).BlockLength);
                    int numblks = (int)((entry.Size / ((NANDImageStream)IO.Stream).BlockLength) + (entry.Size % ((NANDImageStream)IO.Stream).BlockLength > 0 ? 1 : 0));
                    for (int i = 0; i < numblks; i++)
                        CurrentFileSystem.BlockMap[i + startblk] = 0x1ffb;
                }
            }

            if (((NANDImageStream)IO.Stream).PagesPerBlock <= 0x20 )
            {
                Console.WriteLine("Writing config blocks...");
                SaveConfigBlocks();

                Console.WriteLine("Writing filesystem...");
                SaveFileSystem();
            }

            if (Header.ContainsRGBP)
                SavePayloadList();
            
            Console.WriteLine("Writing SMC...");
            SaveSMC();
            Console.WriteLine("Writing keyvault...");
            SaveKeyVault();
            IO.Stream.Flush();
            return true;
        }
        public void Close()
        {
            ((NANDImageStream)IO.Stream).Close(true);
            //IO.Stream.Close();
        }

        public void CreateHeader()
        {
            Header = new BootloaderFlashHeader(this)
                         {
                             Magic = NANDBootloaderMagic.FlashHeader,
                             Build = 1888,
                             Entrypoint = 0x8000,
                             Size = 0x70000
                         };

            Bootloaders.Clear();
            if (File.Exists(Path.Combine(Application.StartupPath, "1bl.bin")))
            {
                Bootloader1BL bl = new Bootloader1BL(this);

                bl.SetData(File.ReadAllBytes(Path.Combine(Application.StartupPath, "1bl.bin")));
                Bootloaders.Add(bl);
            }
        }

        public void LoadSMC()
        {
            SMC = new SMC(IO);
            IO.Stream.Position = Header.SmcAddress;
            SMC.Read((int)Header.SmcSize);
        }
        public void SaveBootloaders(uint addr2BL, uint sysUpdateAddress)
        {
            IO.Stream.Position = 0x0;
            Header.Entrypoint = addr2BL;
            Header.SysUpdateAddress = sysUpdateAddress;
            Header.Size = sysUpdateAddress;
            Header.Write();
            long pos = addr2BL;
            bool slot0Inuse = false;
            for (int i = 0; i < Bootloaders.Count; i++)
            {
                IO.Stream.Position = pos;
                switch (Bootloaders[i].Magic)
                {
                    case NANDBootloaderMagic.CB:
                    case NANDBootloaderMagic.SB:
                        Bootloader2BL bl2 = (Bootloader2BL)Bootloaders[i];
                        bl2.HmacShaKey = (i == 0 || (Bootloaders[i - 1].GetType() == typeof(Bootloader1BL))) ? _1BLKey : Bootloaders[i - 1].Rc4Key;
                        bl2.Write();

                        IO.Stream.Position = pos;
                        if (Bootloaders[i + 1].GetType() != typeof(Bootloader2BL) && (bl2.PerBoxData.PairingData[0] | bl2.PerBoxData.PairingData[1] | bl2.PerBoxData.PairingData[2]) != 0)
                        {
                            // do a dirty rehash
                            Console.WriteLine("Recalculating perbox digest");
                            //Console.WriteLine("Zeroing SMC hash");
                            bl2.PerBoxData.PerBoxDigest = bl2.PerBoxData.CalculateDigest();
                            //bl2.PerBoxData.PerBoxDigest = new byte[0x10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            bl2.Write();
                        }
                        else
                        {
                            bl2.PerBoxData.PerBoxDigest = new byte[0x10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            bl2.Write();
                        }

                        break;
                    case NANDBootloaderMagic.SC:
                        Bootloader3BL bl3 = (Bootloader3BL) Bootloaders[i];
                        bl3.HmacShaKey = ((Bootloader2BL) Bootloaders[i - 1]).Nonce3BL;
                        bl3.Write();
                        break;
                    case NANDBootloaderMagic.CD:
                    case NANDBootloaderMagic.SD:
                        Bootloader4BL bl4 = (Bootloader4BL)Bootloaders[i];
                        bl4.UsingCpuKey = false;
                        if(Bootloaders[i - 1].GetType() == typeof(Bootloader2BL))
                            if(i - 1 == 0)
                                for(int y = 0; y < 3; y++)
                                    if (((Bootloader2BL)Bootloaders[i - 1]).PerBoxData.PairingData[i] != 0)
                                        bl4.UsingCpuKey = true;

                        bl4.HmacShaKey = Bootloaders[i - 1].Rc4Key;
                        bl4.Write();
                        break;
                    case NANDBootloaderMagic.CE:
                    case NANDBootloaderMagic.SE:
                        Bootloader5BL bl5 = (Bootloader5BL)Bootloaders[i];
                        bl5.HmacShaKey = Bootloaders[i - 1].Rc4Key;
                        bl5.Write();
                        break;
                    case NANDBootloaderMagic.CF:
                    case NANDBootloaderMagic.SF:
                        Bootloader6BL bl6 = (Bootloader6BL) Bootloaders[i];

                        if (Bootloaders.Count - 1 >= i && CurrentFileSystem != null)
                        {
                            int spaceLeft = 0x10000 - (int) bl6.Size;

                            long bufSize = Bootloaders[i + 1].Size - spaceLeft;


                            byte[] buffer = new byte[bufSize];

                            FileSystemEntry ent =
                                CurrentFileSystem.Entries.Find(
                                    f => f.FileName == "sysupdate.xexp" + (slot0Inuse ? "2" : "1")) ??
                                CurrentFileSystem.AddNewEntry("sysupdate.xexp" + (slot0Inuse ? "2" : "1"), true);

                            if (ent.BlockNumber == 0)
                                ent.BlockNumber = CurrentFileSystem.AllocateNewBlock();
                            CurrentFileSystem.SetEntryData(ent, buffer);

                            ushort[] chain = CurrentFileSystem.GetBlockChain(ent.BlockNumber);
                            bl6.PerBoxData7BL.UsedBlocksCount = (ushort) chain.Length;
                            Array.Resize(ref chain, bl6.PerBoxData7BL.BlockNumbers.Length);
                            for (int z = 0; z < chain.Length; z++)
                                bl6.PerBoxData7BL.BlockNumbers[z] = chain[z];
                        }
                        if (slot0Inuse)
                            pos = Header.SysUpdateAddress + 0x10000;
                        else
                        {
                            pos = pos > Header.SysUpdateAddress
                                  ? (pos / ((NANDImageStream)IO.Stream).BlockLength) +
                                    1 * ((NANDImageStream)IO.Stream).BlockLength
                                  : Header.SysUpdateAddress;
                            Header.SysUpdateAddress = (uint)pos;
                        }
                        IO.Stream.Position = pos;
                        bl6.HmacShaKey = _1BLKey;
                        bl6.Write();
                        // dirty rehash
                        IO.Stream.Position = pos;
                        if ((bl6.PerBoxData.PairingData[0] | bl6.PerBoxData.PairingData[1] | bl6.PerBoxData.PairingData[2]) != 0)
                        {
                            bl6.PerBoxData.PerBoxDigest = bl6.PerBoxData.CalculateDigest();
                            bl6.Write();
                        }
                        else
                        {
                            bl6.PerBoxData.PerBoxDigest = new byte[0x10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            bl6.PerBoxData.LockDownValue = 0;
                            bl6.PerBoxData.UpdateSlotNumber = 0;
                            bl6.Write();
                        }


                        break;
                    case NANDBootloaderMagic.CG:
                    case NANDBootloaderMagic.SG:

                        Bootloader7BL bl7 = (Bootloader7BL)Bootloaders[i];
                        bl7.HmacShaKey = ((Bootloader6BL) Bootloaders[i - 1]).Nonce7BL;


                        X360IO io = new X360IO(new MemoryStream(), true); 
                        bl7.Write(io, false); 
                        byte[] data = ((MemoryStream) io.Stream).ToArray(); 
                        io.Close();
                        
                        int spaceLeftt = 0x10000 - (int) Bootloaders[i - 1].Size;
                        IO.Stream.Write(data, 0, spaceLeftt);

                        if (CurrentFileSystem != null)
                        {
                            FileSystemEntry entt =
                                CurrentFileSystem.Entries.Find(
                                    f => f.FileName == "sysupdate.xexp" + (slot0Inuse ? "2" : "1"));
                            if (entt != null)
                            {
                                byte[] bufferr = new byte[bl7.Size_r - spaceLeftt];

                                Array.Copy(data, spaceLeftt, bufferr, 0, bufferr.Length);

                                Bootloader6BL bl6p = (Bootloader6BL)Bootloaders[i-1];

                                entt.SetData(bufferr);
                            }
                            
                        }
                        
                        if (!slot0Inuse)
                            slot0Inuse = true;
                        break;
                }
                if(Bootloaders[i].GetType() != typeof(Bootloader1BL))
                    pos += Bootloaders[i].Size;
            }
            //if no CF, fill in patch slots with 00
            if (!slot0Inuse){
                IO.Stream.Position= Header.SysUpdateAddress;
                byte[] blank = new byte[0x20000];
                IO.Stream.Write(blank, 0, 0x20000);
            }
               
        }
        public void LoadHeader()
        {
            IO.Stream.Position = 0x0;
            Header = new BootloaderFlashHeader(this);
            Header.Read();
        }
        public void LoadBootloaders()
        {
            Dictionary<long, NANDBootloaderMagic> bldrs = FindBootloaders();
            Bootloaders.Clear();
            int blmod = 0;
            if (File.Exists(Path.Combine(Application.StartupPath, "1bl.bin")))
            {
                Bootloader1BL bl = new Bootloader1BL(this);
                Console.WriteLine("* Found 1BL at RGBuild path");
                bl.SetData(File.ReadAllBytes(Path.Combine(Application.StartupPath, "1bl.bin")));
                Bootloaders.Add(bl);
                blmod = 1;
            }
            for (int i = Bootloaders.Count; i < bldrs.Count + blmod; i++)
            {
                IO.Stream.Position = bldrs.ElementAt(i - blmod).Key;
                NANDBootloaderMagic magic = bldrs.ElementAt(i - blmod).Value;
                switch(magic)
                {
                    case NANDBootloaderMagic.CB:
                    case NANDBootloaderMagic.SB:
                        Bootloader2BL bl2 = (i == 0 || (Bootloaders[i - 1].GetType() == typeof(Bootloader1BL))) ? new Bootloader2BL(this, null) : new Bootloader2BL(this, Bootloaders[i - 1], CPUKey);
                        
                        bl2.Read();
                        Bootloaders.Add(bl2);
                        break;
                    case NANDBootloaderMagic.SC:
                        Bootloader3BL bl3 = new Bootloader3BL(this, Bootloaders[i - 1]);
                        bl3.Read();
                        Bootloaders.Add(bl3);
                        break;
                    case NANDBootloaderMagic.CD:
                    case NANDBootloaderMagic.SD:
                        Bootloader4BL bl4 = new Bootloader4BL(this, Bootloaders[i - 1]);
                        bl4.Read();
                        Bootloaders.Add(bl4);
                        break;
                    case NANDBootloaderMagic.CE:
                    case NANDBootloaderMagic.SE:
                        Bootloader5BL bl5 = new Bootloader5BL(this, Bootloaders[i - 1]);
                        bl5.Read();
                        Bootloaders.Add(bl5);
                        break;
                    case NANDBootloaderMagic.CF:
                    case NANDBootloaderMagic.SF:
                        Bootloader6BL bl6 = new Bootloader6BL(this);
                        bl6.Read();
                        Bootloaders.Add(bl6);
                        break;
                    case NANDBootloaderMagic.CG:
                    case NANDBootloaderMagic.SG:
                        Bootloader7BL bl7 = new Bootloader7BL(this, Bootloaders[i - 1]);
                        bl7.Read();
                        Bootloaders.Add(bl7);
                        break;
                }
            }

            // for show
            foreach (Bootloader bl in Bootloaders)
            {
                Console.WriteLine("* Found " + bl.GetType().Name.Replace("Bootloader", "") + " @ 0x" + bl.Position.ToString("X"));
            }
        }
        public Dictionary<long, NANDBootloaderMagic> FindBootloaders()
        {
            Dictionary<long, NANDBootloaderMagic> bldrs = new Dictionary<long, NANDBootloaderMagic>();
            uint pos = Header.Entrypoint;
            bool secondpatchslot = false;
            while(true && (pos < IO.Stream.Length))
            {
                IO.Stream.Position = pos;
                Bootloader bootloader = new Bootloader(this);
                bootloader.Read();
                if ((ushort)bootloader.Magic == 0xFFFF)
                    break;
                if (Enum.IsDefined(typeof(NANDBootloaderMagic), bootloader.Magic))
                    bldrs.Add(pos, bootloader.Magic);
                pos += bootloader.Size;

                if (bootloader.Magic == NANDBootloaderMagic.CE || bootloader.Magic == NANDBootloaderMagic.SE)
                    pos = Header.SysUpdateAddress; // lets look for sysupdates

                // if we reach end of patchslot 0 or an invalid loader
                if (bootloader.Magic == NANDBootloaderMagic.CG || bootloader.Magic == NANDBootloaderMagic.SG || !Enum.IsDefined(typeof(NANDBootloaderMagic), bootloader.Magic))
                    if (!secondpatchslot) // check if we've looked at second patch slot
                    {
                        secondpatchslot = true;
                        pos = Header.SysUpdateAddress + 0x10000; // might need to fix this for big block jaspers, 0x20000 for them
                    }
                    else
                        break;
            }
            return bldrs;
        }
        public Bootloader GetLastBootloader(Bootloader loader)
        {
            int idx = Bootloaders.IndexOf(loader);
            return Bootloaders[idx - 1];
        }
        public Bootloader GetLastExecutableBootloader(Bootloader loader)
        {
            int idx = Bootloaders.IndexOf(loader);
            if (idx <= 0)
                return null;
            Type bltype = Bootloaders[idx - 1].GetType();
            if (idx > 1)
            {
                Type bltype2 = Bootloaders[idx - 2].GetType();
                if (bltype2 == typeof (Bootloader6BL) || bltype2 == typeof (Bootloader7BL))
                    idx -= 2;
            }
            if (bltype == typeof(Bootloader3BL) || bltype == typeof(Bootloader5BL))
                idx -= 1;
            return Bootloaders[idx - 1];

        }
        public Bootloader GetNextBootloader(Bootloader loader)
        {
            int idx = Bootloaders.IndexOf(loader);
            return Bootloaders[idx + 1];
        }

    }
}
