using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RGBuild.IO;

namespace RGBuild.NAND
{
    public enum FileSystemExEntries : byte
    {
        FsRootEntryAlt = 0x2c,
        FsRootEntry = 0x30,
        MobileB = 0x31,
        MobileC = 0x32,
        MobileD = 0x33,
        MobileE = 0x34,
        MobileF = 0x35,
        MobileG = 0x36,
        MobileH = 0x37,
        MobileI = 0x38,
        MobileJ = 0x39,
        InvalidMobileJ = 0x40,
        InUseMobileJ = 0x80
    }
    public class FileSystemEntry
    {
        internal FileSystemRoot FSRoot;
        internal int PageNumber;
        public string FileName;
        public ushort BlockNumber;
        public uint Size;
        public int Timestamp; // DOS date, not very important
        public bool Deleted;
        private byte[] _data;
        public FileSystemEntry EarlierEntry
        {
            get
            {
                FileSystemRoot root = FSRoot.Image.FileSystems.Find(sec => sec.Version < FSRoot.Version);
                if (root == null)
                    return null;
                FileSystemEntry entry = FSRoot.Entries.Find(sec => sec.FileName == FileName);
                if (entry == null)
                    return null;
                return entry;
            }
        }
        public FileSystemEntry(int page, FileSystemRoot root)
        {
            PageNumber = page;
            FSRoot = root;
        }
        public void Read()
        {
            Read(FSRoot.Image.IO);
        }
        public void Write()
        {
            Write(FSRoot.Image.IO);
        }
        public byte[] GetMeta()
        {
            byte[] data = BitConverter.GetBytes(Timestamp);
            Array.Reverse(data);
            return data;
        }
        public void SetMeta(byte[] data)
        {
            Array.Reverse(data);
            Timestamp = BitConverter.ToInt32(data, 0);
            return;
        }
        public byte[] GetData()
        {
            return _data;
        }
        public void SetData(byte[] data)
        {
            _data = data;
            Size = (uint)data.Length;
        }
        public void Read(X360IO io)
        {
            long pos = io.Stream.Position;
            //try
            //{
                FileName = io.Reader.ReadAsciiString(0x16).Trim('\0');
            //}
            //catch
            //{
            //    FileName = "ERROR";
            //    io.Stream.Position = pos + 0x16;
            //}

            if(FileName.Length > 0 && FileName[0] == 0x05)
            {
                FileName = "_" + FileName.Substring(1, FileName.Length - 1);
                Deleted = true;
            }
            BlockNumber = io.Reader.ReadUInt16();
            Size = io.Reader.ReadUInt32();
            Timestamp = io.Reader.ReadInt32();
        }
        public void Write(X360IO io)
        {
            io.Writer.WriteAsciiString(FileName, 0x16);
            if (Deleted)
            {
                io.Stream.Position -= 0x16;
                io.Writer.Write((byte) 0x05);
                io.Stream.Position += 0x15;
            }
            io.Writer.Write(BlockNumber);
            io.Writer.Write(Size);
            io.Writer.Write(Timestamp);
        }
    }
    public class FileSystemRoot
    {
        internal NANDImage Image;
        public int BlockNumber;
        public int Version;
        public long FreeSpace
        {
            get
            {
                return BlockMap.Where(block => (block & 0x7fff) == 0x1ffe).Aggregate<ushort, long>(0, (current, block) => current + ((NANDImageStream)Image.IO.Stream).BlockLength);
            }
        }
        public List<FileSystemEntry> Entries = new List<FileSystemEntry>();
        public ushort[] BlockMap;
        public ushort BlockOffset = 0; // used on bigblock nands so we dont need HUEG blockmaps

        public FileSystemRoot(NANDImage image, int block, int version)
        {
            BlockNumber = block;
            Image = image;
            Version = version;
        }
        public void CopyData(FileSystemRoot fs)
        {
            for (int i = 0; i < fs.BlockMap.Length; i++)
                BlockMap[i] = fs.BlockMap[i];
            foreach(FileSystemEntry entry in fs.Entries)
            {
                FileSystemEntry entry2 = new FileSystemEntry(0, this)
                                             {
                                                 FileName = entry.FileName,
                                                 BlockNumber = entry.BlockNumber,
                                                 Size = entry.Size,
                                                 Timestamp = entry.Timestamp
                                             };
                Entries.Add(entry2);
            }
        }
        public void CreateDefaults()
        {
            BlockMap = new ushort[((NANDImageStream)Image.IO.Stream).BlockCount];
            for (int i = 0; i < ((NANDImageStream)Image.IO.Stream).BlockCount; i++)
            {
                BlockMap[i] = 0x1ffe;
            }
            BlockMap[BlockNumber] = 0x1fff;
            

            //start block is after CG or last payload
            double fsStartBlock = ((double)(Image.Header.SysUpdateAddress + 0x20000)/(double)0x4000);

            /*if(Image.Payloads != null)
                foreach (RGBPayloadEntry entry in Image.Payloads.Payloads)
                {
                    double entryblock = ((double)((entry.Address + entry.Size) / (double)0x4000));

                    if (entryblock > fsStartBlock) fsStartBlock = entryblock;
                }*/

            //round up to next block
            if (fsStartBlock != (int)fsStartBlock) fsStartBlock = (int)fsStartBlock + 1;
           

            // reserve blocks for firmware instead of 0x34
            for (int i = 0; i < fsStartBlock; i++)
                BlockMap[i] = 0x1ffb;

            foreach (MobileXFile file in Image.MobileData)
                for (int i = 0; i < file.PageCount; i++)
                    BlockMap[i + file.StartPage] = 0x1fff;

            if (((NANDImageStream)Image.IO.Stream).ConfigBlockStart > 0 && Image.ConfigBlock != null)
                for (int i = 0; i < 5; i++)
                    BlockMap[i + ((NANDImageStream)Image.IO.Stream).ConfigBlockStart] = 0x1ffb;
        }
        public ushort AllocateNewBlock()
        {
            return AllocateNewBlock(1, 0);
        }
        public ushort AllocateNewBlock(int blocksNeeded, ushort minimumblock)
        {
            // loop through each cluster in the chainmap until we find an unallocated one
            for (ushort x = minimumblock; x < ((NANDImageStream)Image.IO.Stream).BlockCount; x++)
            {
                bool cont = false;
                for (int i = 0; i < blocksNeeded; i++)
                    if ((BlockMap[x + i] & 0x7fff) != 0x1ffe)
                    {
                        cont = true;
                        break;
                    }

                if (cont)
                    continue;

                // allocate the cluster and then return it
                BlockMap[x] = 0x1fff;
                ((NANDImageStream)Image.IO.Stream).SeekToBlock(x);
                Image.IO.Writer.Write(new byte[((NANDImageStream)Image.IO.Stream).BlockLength]);
                return x;
            }
            return 0;
        }
        internal void SetEntryData(FileSystemEntry entry, byte[] data)
        {
            SetChainData(entry.BlockNumber, data);
            entry.SetData(data);
        }
        public void SetChainData(ushort startBlock, byte[] data)
        {
            if (data.Length == 0)
                data = new byte[1];
            // lets get our current chain
            ushort[] currentChain = GetBlockChain(startBlock);
            // lets figure out how many blocks the data will take up
            ushort blocksNeeded = (ushort)((ulong)data.Length / (uint)((NANDImageStream)Image.IO.Stream).BlockLength);
            blocksNeeded += (ushort)((ulong)data.Length % (uint)((NANDImageStream)Image.IO.Stream).BlockLength > 0 ? 1 : 0);
            if (currentChain.Length == blocksNeeded)
            {
                using (MemoryStream dataStream = new MemoryStream(data))
                {
                    using (BinaryReader dataReader = new BinaryReader(dataStream))
                    {
                        int wrote = 0;
                        for (uint i = 0; i < blocksNeeded; i++)
                        {
                            
                            //Now lets write some data to it eh
                            int toWrite = (int)((NANDImageStream)Image.IO.Stream).BlockLength;
                            if (i == blocksNeeded - 1)
                            {
                                toWrite = data.Length - wrote;
                            }
                            SetBlockData(currentChain[i], dataReader.ReadBytes(toWrite));
                            wrote += toWrite;
                        }
                    }
                }
            }
            else
            {
                if (currentChain.Length < blocksNeeded)
                {
                    // How many blocks do we have to allocate?
                    uint blocksToAllocate = blocksNeeded - (uint)currentChain.Length;

                    ushort currentBlock = currentChain[currentChain.Length - 1];

                    // Now lets allocate the blocks
                    for (int x = 0; x < blocksToAllocate; x++)
                    {
                        ushort nextBlock = AllocateNewBlock();
                        BlockMap[currentBlock] = nextBlock;
                        currentBlock = nextBlock;
                    }

                    // Lets re-run this function now
                    SetChainData(startBlock, data);
                }
                else
                {
                    // we have to deallocate blocks :/

                    // Free all the blocks after the amount we need
                    FreeBlockChain(currentChain[blocksNeeded - 1]);

                    // Set the last block to 0xFFFFFFFF
                    BlockMap[currentChain[blocksNeeded - 1]] = 0x1fff;

                    // Lets re-run
                    SetChainData(startBlock, data);
                }
            }
        }
        public void FreeBlockChain(ushort startBlock)
        {
            // lets get the entire chain first
            ushort[] chain = GetBlockChain(startBlock);

            // loop through each block in the chain and free it
            foreach (ushort block in chain)
            {
                BlockMap[block] = 0x1FFE;
                //((NANDImageStream)Image.IO.Stream).SeekToBlock(block);
                //int page = ((NANDImageStream) Image.IO.Stream).CurrentPageNumber;
                //for(int i = 0; i < ((NANDImageStream)Image.IO.Stream).PagesPerBlock; i++)
                //{
                //    byte[] ecc = ((NANDImageStream) Image.IO.Stream).GetPageEDC(page + i);
                //    ecc[2] = 0;
                //    ((NANDImageStream) Image.IO.Stream).WritePageEDC(page + i, ecc);
                //}
            }
        }
        public byte[] GetEntryData(FileSystemEntry entry)
        {
            byte[] direntData1 = GetChainData(entry.BlockNumber);
            if (direntData1.Length < entry.Size)
                return direntData1; // guess we couldnt get it all :/
            byte[] direntData2 = new byte[entry.Size];
            Array.Copy(direntData1, 0, direntData2, 0, entry.Size);
            
            return direntData2;
        }
        public void DeleteEntry(FileSystemEntry entry)
        {
            FreeBlockChain(entry.BlockNumber);
            Entries.Remove(entry);
        }
        public FileSystemEntry AddNewEntry(string fileName, bool addToTop)
        {
            FileSystemEntry ent = new FileSystemEntry(-1, this)
                                  {
                                      FileName = fileName,
                                      Size = 0,
                                      BlockNumber = 0,
                                      Timestamp = 0
                                  };
            if(addToTop)
                Entries.Insert(0, ent);
            else
                Entries.Add(ent);

            return ent;
        }
        public byte[] GetChainData(ushort startBlock)
        {
            ushort[] chain = GetBlockChain(startBlock);
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter bw = new BinaryWriter(ms))
                {
                    foreach (ushort cluster in chain)
                    {
                        bw.Write(GetBlockData(cluster, 0));
                    }
                    bw.Flush();
                    return ms.ToArray();
                }
            }
        }
        public void SetBlockData(ushort block, byte[] data)
        {
            //First lets blank the block
            ((NANDImageStream)Image.IO.Stream).SeekToBlock(block + BlockOffset);
            int page = ((NANDImageStream)Image.IO.Stream).CurrentPageNumber;
            for (int j = 0; j < ((NANDImageStream)Image.IO.Stream).PagesPerBlock; j++)
            {
                ISpareData ecc = ((NANDImageStream)Image.IO.Stream).GetPageSpare(page + j);
                ecc.FsPageCount = 0;
                ecc.FsSequence = 0;
                ecc.FsSize = 0;
                ((NANDImageStream)Image.IO.Stream).WritePageSpare(page + j, ecc);
            }
            Image.IO.Writer.Write(new byte[((NANDImageStream)Image.IO.Stream).BlockLength]);
            
            ((NANDImageStream)Image.IO.Stream).SeekToBlock(block + BlockOffset);
            Image.IO.Writer.Write(data);
        }
        public byte[] GetBlockData(ushort block, int remove)
        {
            ((NANDImageStream)Image.IO.Stream).SeekToBlock(block + BlockOffset);
            return Image.IO.Reader.ReadBytes((int)((NANDImageStream)Image.IO.Stream).BlockLength - remove);
        }

        public ushort[] GetBlockChain(ushort startBlock)
        {
            return GetBlockChain(startBlock, ((NANDImageStream)Image.IO.Stream).BlockCount);
        }

        public ushort[] GetBlockChain(ushort startBlock, int limit)
        {
            if (startBlock > ((NANDImageStream)Image.IO.Stream).BlockCount)
                return null;
            ushort[] blockList = new ushort[0];
            ushort currentBlock = startBlock;
            int i = 0;
            do
            {
                Array.Resize(ref blockList, blockList.Length + 1);
                blockList[i] = currentBlock;
                //if (currentBlock == Image.)
                //    break;
                currentBlock = BlockMap[currentBlock];
                currentBlock = (ushort) (currentBlock & 0x7FFF);
                //if (currentBlock == 0)
                //    break;
                i++;
            } while (currentBlock != 0 && (currentBlock & 0x1FFE) != 0x1FFE && ((NANDImageStream)Image.IO.Stream).BlockCount > currentBlock && i < limit); //&& (currentBlock & ((NANDImageStream)Image.IO.Stream).BlockCount) == 0 
            return blockList;
        }


        public void Write()
        {
            
            //Console.WriteLine("");
           

            ((NANDImageStream)Image.IO.Stream).SeekToBlock(BlockNumber);
            int startpage = ((NANDImageStream)Image.IO.Stream).CurrentPageNumber;
            List<int> blockMapPages = new List<int>();
            List<int> fileNamePages = new List<int>();
            for (int i = 0; i < ((NANDImageStream)Image.IO.Stream).PagesPerBlock; i++)
                if (i % 2 == 0)
                    blockMapPages.Add(startpage + i);
                else
                    fileNamePages.Add(startpage + i);

            int fncount = ((NANDImageStream)Image.IO.Stream).PageLength/0x20;
            int bmcount = ((NANDImageStream)Image.IO.Stream).PageLength/0x2;
            int currentbmpage = -1;
            int currentfnpage = -1;
            int j = 0;
            foreach(int page in blockMapPages)
            {
                ((NANDImageStream)Image.IO.Stream).SeekToPage(page);
                Image.IO.Writer.Write(new byte[((NANDImageStream)Image.IO.Stream).PageLength]);
                ISpareData edc = ((NANDImageStream)Image.IO.Stream).GetPageSpare(page);
                edc.FsSequence = Version;
                edc.FsBlockType = (byte) (edc.FsBlockType | 0x30);
                ((NANDImageStream) Image.IO.Stream).WritePageSpare(page, edc);
            }
            foreach (int page in fileNamePages)
            {
                ((NANDImageStream)Image.IO.Stream).SeekToPage(page);
                Image.IO.Writer.Write(new byte[((NANDImageStream)Image.IO.Stream).PageLength]);
                ISpareData edc = ((NANDImageStream)Image.IO.Stream).GetPageSpare(page);
                edc.FsSequence = Version;
                edc.FsBlockType = (byte)(edc.FsBlockType | 0x30);
                ((NANDImageStream)Image.IO.Stream).WritePageSpare(page, edc);
            }
            // Entries.Sort((p1, p2) => p1.Timestamp.CompareTo(p2.Timestamp));
            foreach(FileSystemEntry entry in Entries.FindAll(sec => !sec.Deleted))
            {
                long pos = Image.IO.Stream.Position;
                if (entry.BlockNumber == 0)
                    entry.BlockNumber = AllocateNewBlock((int)(entry.Size / ((NANDImageStream)Image.IO.Stream).BlockLength) + (entry.Size % ((NANDImageStream)Image.IO.Stream).BlockLength > 0 ? 1 : 0), 0);
                SetEntryData(entry, entry.GetData());
                Image.IO.Stream.Position = pos;
                if (j % fncount == 0)
                {
                    currentfnpage++;
                    ((NANDImageStream) Image.IO.Stream).SeekToPage(fileNamePages[currentfnpage]);
                }
                entry.Write();
                j++;
            }
            j = 0;
            foreach(ushort block in BlockMap)
            {
                if (j % bmcount == 0)
                {
                    currentbmpage++;
                    ((NANDImageStream)Image.IO.Stream).SeekToPage(blockMapPages[currentbmpage]);
                }
                Image.IO.Writer.Write(block);
                j++;
            }
        }
        public void Read()
        {
            Entries = new List<FileSystemEntry>();
            ((NANDImageStream) Image.IO.Stream).SeekToBlock(BlockNumber);
            int startpage = ((NANDImageStream) Image.IO.Stream).CurrentPageNumber;
            List<int> blockMapPages = new List<int>();
            List<int> fileNamePages = new List<int>();
            for (int i = 0; i < ((NANDImageStream)Image.IO.Stream).PagesPerBlock; i++)
                if (i%2 == 0)
                    blockMapPages.Add(startpage + i);
                else
                    fileNamePages.Add(startpage + i);
            bool breakk = false;
            foreach (int page in fileNamePages)
            {
                if (breakk)
                    break;
                ((NANDImageStream) Image.IO.Stream).SeekToPage(page);
                int entrycount = ((NANDImageStream)Image.IO.Stream).PageLength/0x20;
                for (int i = 0; i < entrycount; i++)
                {
                    FileSystemEntry entry = new FileSystemEntry(page, this);
                    entry.Read();

                    if (String.IsNullOrEmpty(entry.FileName))
                    {
                        breakk = true;
                        break;
                    }
                    if(Entries.Find(sec => sec.FileName == entry.FileName) == null)
                        Entries.Add(entry);
                }
            }
            BlockMap = new ushort[((NANDImageStream)Image.IO.Stream).BlockCount];
            int j = 0;
            foreach (int page in blockMapPages)
            {
                ((NANDImageStream) Image.IO.Stream).SeekToPage(page);
                for(int i = 0; i < ((NANDImageStream)Image.IO.Stream).PageLength / 2; i++)
                {
                    if (j >= ((NANDImageStream)Image.IO.Stream).BlockCount)
                        break;
                    BlockMap[j] = Image.IO.Reader.ReadUInt16();
                    j++;
                }
                if (j > ((NANDImageStream)Image.IO.Stream).BlockCount)
                    break;
            }
            BlockMap[BlockNumber] = 0x1fff;
        }
    }
}
