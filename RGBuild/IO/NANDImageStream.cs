using System;
using System.IO;
using RGBuild.ThirdParty;
using System.Collections.Generic;

namespace RGBuild.IO
{
    public class SpareDataSmallBlock : ISpareData
    {
        public short BlockId { get; set; }
        public bool BadBlock
        {
            get
            {
                return _badBlockIndicator != 0xFF;
            }
            set
            {
                _badBlockIndicator = (byte)(value ? 0xFF : 0);
            }
        }
        public int FsSequence
        {
            get
            {
                return (_fsSequence3 << 24) + (_fsSequence2 << 16) + (_fsSequence1 << 8) + _fsSequence0;
            }
            set
            {
                _fsSequence0 = (byte)(value & 0xFF);
                _fsSequence1 = (byte)((value >> 8) & 0xFF);
                _fsSequence2 = (byte)((value >> 16) & 0xFF);
                _fsSequence3 = (byte)((value >> 24) & 0xFF);
            }
        }
        public short FsSize { get; set; }
        public byte FsBlockType
        {
            get
            {
                if (EDC == null || EDC.Length <= 0)
                    return 0;
                return EDC[0];
            }
            set
            {
                if (EDC == null || EDC.Length <= 0)
                    return;
                EDC[0] = value;
            }
        }

        private byte _fsSequence0;
        private byte _fsSequence1;
        private byte _fsSequence2;
        private byte _badBlockIndicator;
        private byte _fsSequence3;
        

        public byte FsPageCount { get; set; }

        private short _unusedshort1;
        public short _unused1
        {
            get
            {
                return _unusedshort1;
            }
            set
            {
                _unusedshort1 = (short)value;
            }
        }
        public byte[] EDC { get; set; } // 6 bits of EDC[0] are FsBlockType

        public void Read(byte[] data)
        {
            byte[] buffer = new byte[2];
            Array.Copy(data, buffer, 2);
            //Array.Reverse(buffer);
            buffer[1] = (byte)(buffer[1] & 0xF);
            BlockId = BitConverter.ToInt16(buffer, 0);
            _fsSequence0 = data[2];
            _fsSequence1 = data[3];
            _fsSequence2 = data[4];
            _badBlockIndicator = data[5];
            _fsSequence3 = data[6];
            Array.Copy(data, 7, buffer, 0, 2);
            Array.Reverse(buffer);
            FsSize = BitConverter.ToInt16(buffer, 0);
            FsPageCount = data[9];
            Array.Copy(data, 10, buffer, 0, 2);
            Array.Reverse(buffer);
            _unused1 = BitConverter.ToInt16(buffer, 0);
            EDC = new[] { data[12], data[13], data[14], data[15] };
        }
        public byte[] Write()
        {
            byte[] data = new byte[16];
            byte[] buffer = BitConverter.GetBytes(BlockId);
            //Array.Reverse(buffer);
            Array.Copy(buffer, data, 2);
            data[2] = _fsSequence0;
            data[3] = _fsSequence1;
            data[4] = _fsSequence2;
            data[5] = _badBlockIndicator;
            data[6] = _fsSequence3;
            buffer = BitConverter.GetBytes(FsSize);
            Array.Reverse(buffer);
            Array.Copy(buffer, 0, data, 7, 2);
            data[9] = FsPageCount;
            buffer = BitConverter.GetBytes(_unused1);
            Array.Reverse(buffer);
            Array.Copy(buffer, 0, data, 10, 2);
            Array.Copy(EDC, 0, data, 12, 4);
            return data;
        }
    }
    public class SpareDataBigBlock : ISpareData
    {
        public short BlockId { get; set; }
        public bool BadBlock
        {
            get
            {
                return _badBlockIndicator != 0xFF;
            }
            set
            {
                _badBlockIndicator = (byte)(value ? 0xFF : 0);
            }
        }
        public int FsSequence
        {
            get
            {
                return (_fsSequence2 << 16) + (_fsSequence1 << 8) + _fsSequence0;
            }
            set
            {
                _fsSequence0 = (byte)(value & 0xFF);
                _fsSequence1 = (byte)((value >> 8) & 0xFF);
                _fsSequence2 = (byte)((value >> 16) & 0xFF);
            }
        }
        public short FsSize { get; set; }
        public byte FsBlockType
        {
            get
            {
                if (EDC == null || EDC.Length <= 0)
                    return 0;
                return EDC[0];
            }
            set
            {
                if (EDC == null || EDC.Length <= 0)
                    return;
                EDC[0] = value;
            }
        }
        private byte _badBlockIndicator;
        private byte _fsSequence2;
        private byte _fsSequence1;
        private byte _fsSequence0;
        public byte FsPageCount { get; set; }

        private short _unusedshort1;
        public short _unused1
        {
            get
            {
                return _unusedshort1;
            }
            set
            {
                _unusedshort1 = (short)value;
            }
        }

        private byte _unused0;
        public byte[] EDC { get; set; } // 6 bits of EDC[0] are FsBlockType

        public void Read(byte[] data)
        {
            _badBlockIndicator = data[0];
            byte[] buffer = new byte[2];
            Array.Copy(data, 1, buffer, 0, 2);
            //Array.Reverse(buffer);
            buffer[1] = (byte)(buffer[1] & 0xF);
            BlockId = BitConverter.ToInt16(buffer, 0);
            _fsSequence2 = data[3];
            _fsSequence1 = data[4];
            _fsSequence0 = data[5];
            _unused0 = data[6];
            Array.Copy(data, 7, buffer, 0, 2);
            Array.Reverse(buffer);
            FsSize = BitConverter.ToInt16(buffer, 0);
            FsPageCount = data[9];
            Array.Copy(data, 10, buffer, 0, 2);
            Array.Reverse(buffer);
            _unused1 = BitConverter.ToInt16(buffer, 0);
            EDC = new[] { data[12], data[13], data[14], data[15] };
        }
        public byte[] Write()
        {
            byte[] data = new byte[16];
            data[0] = _badBlockIndicator;
            byte[] buffer = BitConverter.GetBytes(BlockId);
            Array.Reverse(buffer);
            Array.Copy(buffer, 0, data, 1, 2);
            data[3] = _fsSequence2;
            data[4] = _fsSequence1;
            data[5] = _fsSequence0;
            data[6] = _unused0;
            buffer = BitConverter.GetBytes(FsSize);
            Array.Reverse(buffer);
            Array.Copy(buffer, 0, data, 7, 2);
            data[9] = FsPageCount;
            buffer = BitConverter.GetBytes(_unused1);
            Array.Reverse(buffer);
            Array.Copy(buffer, 0, data, 10, 2);
            Array.Copy(EDC, 0, data, 12, 4);
            return data;
        }
    }
    public class SpareDataBigOnSmall : ISpareData
    {
        public short BlockId { get; set; }
        public bool BadBlock
        {
            get
            {
                return _badBlockIndicator != 0xFF;
            }
            set
            {
                _badBlockIndicator = (byte)(value ? 0xFF : 0);
            }
        }
        public int FsSequence
        {
            get
            {
                return (_fsSequence2 << 16) + (_fsSequence1 << 8) + _fsSequence0;
            }
            set
            {
                _fsSequence0 = (byte)(value & 0xFF);
                _fsSequence1 = (byte)((value >> 8) & 0xFF);
                _fsSequence2 = (byte)((value >> 16) & 0xFF);
            }
        }
        public short FsSize { get; set; }
        public byte FsBlockType
        {
            get
            {
                if (EDC == null || EDC.Length <= 0)
                    return 0;
                return EDC[0];
            }
            set
            {
                if (EDC == null || EDC.Length <= 0)
                    return;
                EDC[0] = value;
            }
        }
        private byte _fsSequence0;
        private byte _fsSequence1;
        private byte _fsSequence2;
        private byte _badBlockIndicator;
        private byte _fsSequence3;
        public byte FsPageCount { get; set; }

        private short _unusedshort1;
        public short _unused1
        {
            get
            {
                return _unusedshort1;
            }
            set
            {
                _unusedshort1 = (short)value;
            }
        }

        public byte[] EDC { get; set; } // 6 bits of EDC[0] are FsBlockType

        public void Read(byte[] data)
        {
            _fsSequence0 = data[0];
            byte[] buffer = new byte[2];
            Array.Copy(data, 1, buffer, 0, 2);
            //Array.Reverse(buffer);
            buffer[0] = (byte)(buffer[0] & 0xF);
            BlockId = BitConverter.ToInt16(buffer, 0);
            _fsSequence1 = data[3];
            _fsSequence2 = data[4];
            _badBlockIndicator = data[5];
            _fsSequence3 = data[6];
            Array.Copy(data, 7, buffer, 0, 2);
            Array.Reverse(buffer);
            FsSize = BitConverter.ToInt16(buffer, 0);
            FsPageCount = data[9];
            Array.Copy(data, 10, buffer, 0, 2);
            Array.Reverse(buffer);
            _unused1 = BitConverter.ToInt16(buffer, 0);
            EDC = new[] { data[12], data[13], data[14], data[15] };
        }
        public byte[] Write()
        {
            byte[] data = new byte[16];
            data[0] = _fsSequence0;
            byte[] buffer = BitConverter.GetBytes(BlockId);

            //Array.Reverse(buffer);
            Array.Copy(buffer, 0, data, 1, 2);
            data[3] = _fsSequence1;
            data[4] = _fsSequence2;
            data[5] = _badBlockIndicator;
            data[6] = _fsSequence3;
            buffer = BitConverter.GetBytes(FsSize);
            Array.Reverse(buffer);
            Array.Copy(buffer, 0, data, 7, 2);
            data[9] = FsPageCount;
            buffer = BitConverter.GetBytes(_unused1);
            Array.Reverse(buffer);
            Array.Copy(buffer, 0, data, 10, 2);
            Array.Copy(EDC, 0, data, 12, 4);
            return data;
        }
    }
    public enum SpareDataType
    {
        None,
        SmallBlock,
        BigOnSmall,
        BigBlock
    }
    public interface ISpareData
    {
        short BlockId
        {
            get;
            set;
        }

        bool BadBlock
        {
            get;
            set;
        }

        int FsSequence
        {
            get;
            set;
        }
        short FsSize
        {
            get;
            set;
        }

        byte FsPageCount
        {
            get;
            set;
        }

        byte FsBlockType
        {
            get;
            set;
        }

        byte[] EDC
        {
            get;
            set;
        }

        short _unused1
        {
            get;
            set;
        }

        void Read(byte[] data);
        byte[] Write();

       
    }

    public class NANDImageStream : Stream
    {
        private MemoryStream _dataStream;
        private MemoryStream _eccStream;
        private Stream _fileStream;
        private BinaryReader _reader;
        public int BlockCount;
        public int BigBlockCount
        {
            get
            {
                return BlockCount * BlocksPerBigBlock;
            }
        }
        public int PageCount;
        public int BlocksPerBigBlock
        {
            get
            {
                return PagesPerBigBlock / 0x20;
            }
        }
        public int PagesPerBigBlock = 0x20;
        public int PagesPerBlock
        {
            get
            {
                return PagesPerBigBlock / BlocksPerBigBlock;
            }
        }
        public int SpareLength = 0x10;
        public int PageLength;
        public bool BetaNAND = false;
        public SpareDataType SpareDataType = SpareDataType.None;
        public byte[] TempReturnData;
        public List<int> badblocks = new List<int>();
        public int CurrentPageNumber
        {
            get
            {
                long count = Position / PageLength;
                if (Position % PageLength > 0)
                    count++;
                return (int)count;
            }
        }
        public int BadBlockAreaStart
        {
            get
            {
                switch (_dataStream.Length)
                {
                    case 0x1000000: // 16MB;
                        return 0x3E0;
                    case 0x4000000: // 64MB;
                        return 0xF80;
                        
                    default:
                        switch (BigBlockLength)
                        {
                            case 0x4000:
                                return 0x3E0;
                            case 0x40000:
                                return 0x1e0;
                            case 0x20000:
                                return 0x1DC; //might need to be this?

                            case 0x800000:
                                return 0xf0;
                            default:
                                return 0x3E0;
                        }
                }
            }
        }
        public int ConfigBlockStart
        {
            get
            {
                return BadBlockAreaStart - 4;
            }
        }
        public NANDImageStream(Stream baseStream, int pageLength)
        {// creating an img
            PageLength = pageLength;
            _fileStream = baseStream;
            _reader = new BinaryReader(_fileStream);
            PageCount = (int)_fileStream.Length / (PageLength + SpareLength);
            _dataStream = new MemoryStream();
            _eccStream = new MemoryStream();
            bool foundFirstBlock = false;

            for (int i = 0; i < PageCount; i++)
            {
                byte[] data = _reader.ReadBytes(PageLength);
                byte[] eccData = _reader.ReadBytes(SpareLength);
                eccData[5] = 0xFF;
                _dataStream.Write(data, 0, data.Length);
                _eccStream.Write(eccData, 0, eccData.Length);
                if (!foundFirstBlock)
                {
                    if (i == 0 && eccData[1] == 0xF0)
                        BetaNAND = true;
                    if (BetaNAND)
                        eccData[1] = (byte)(eccData[1] ^ 0xF0);
                    byte[] eccrev = new byte[] { eccData[1], eccData[0] };
                    if (!BetaNAND && (BitConverter.ToUInt16(eccData, 0) == 1 || BitConverter.ToUInt16(eccrev, 0) == 1))
                    {
                        foundFirstBlock = true;
                        PagesPerBigBlock = i;
                    }

                }
            }
            BlockCount = (int)_fileStream.Length / (PagesPerBlock * (PageLength + SpareLength));
            _dataStream.Position = 0;
            _eccStream.Position = 0;
            _fileStream.Position = 0;
        }
        public NANDImageStream(string path, FileMode mode, int pageLength)
        {
            PageLength = pageLength;
            _fileStream = new FileStream(path, mode);
            _reader = new BinaryReader(_fileStream);
            _reader.BaseStream.Position = 0x4400;
            byte[] firstedc = _reader.ReadBytes(0x10);
            _reader.BaseStream.Position = 0;
            if (firstedc[0] == 0xFF)
            {
                SpareDataType = SpareDataType.BigBlock;
            }
            else if (firstedc[5] == 0xFF)
                SpareDataType = SpareDataType.SmallBlock;
            if (firstedc[1] == 1)
                SpareDataType = SpareDataType.BigOnSmall;

            if (SpareDataType == SpareDataType.None)
            {
                SpareLength = 0;
                PagesPerBigBlock = 0x20;
            }

            PageCount = (int)_fileStream.Length / (PageLength + SpareLength);
            _dataStream = new MemoryStream();
            _eccStream = new MemoryStream();
            bool foundFirstBlock = false;

            for (int i = 0; i < PageCount; i++)
            {
                byte[] data = _reader.ReadBytes(PageLength);
                byte[] eccData = _reader.ReadBytes(SpareLength);
                _dataStream.Write(data, 0, data.Length);
                _eccStream.Write(eccData, 0, eccData.Length);
                if (!foundFirstBlock && SpareDataType != IO.SpareDataType.None)
                {
                    int blockididx = SpareDataType != SpareDataType.SmallBlock ? 1 : 0;
                    if (i == 0 && eccData[blockididx + 1] == 0xF0)
                        BetaNAND = true;
                    if (BetaNAND)
                        eccData[blockididx + 1] = (byte)(eccData[blockididx + 1] ^ 0xF0);
                    byte[] eccrev = new byte[] { eccData[blockididx + 1], eccData[blockididx] };
                    byte[] eccblkid = new byte[] { eccData[blockididx], eccData[blockididx + 1] };
                    if (BitConverter.ToUInt16(eccblkid, 0) == 1 || BitConverter.ToUInt16(eccrev, 0) == 1)
                    {
                        foundFirstBlock = true;
                        PagesPerBigBlock = i;
                    }

                }
            }
            BlockCount = (int)_fileStream.Length / (PagesPerBigBlock * (PageLength + SpareLength));
            if (SpareDataType != SpareDataType.None)
            {
                // scan image for bad blocks and remap them in memory
                badblocks.Clear();
                for (int i = 0; i < BlockCount; i++)
                {
                    ISpareData spare = GetBigBlockSpare(i);

                    if (spare != null && spare.BadBlock)
                    {
                        badblocks.Add(i);
                        Console.WriteLine("Found bad block at " + i.ToString("X"));
                    }
                }

                int curBadBlock = BlockCount-1;

                foreach(int i in badblocks){
                    if(i<BadBlockAreaStart){

                        while (badblocks.Contains(curBadBlock))
                        {
                            curBadBlock--;
                        }

                        ISpareData spare2 = GetBigBlockSpare(curBadBlock);
                        if (spare2.BlockId == i)
                        {
                            SeekToBlock(curBadBlock);
                            byte[] goodblock = new byte[BigBlockLength];
                            _dataStream.Read(goodblock, 0, (int)BigBlockLength);
                            SeekToBlock(i);
                            _dataStream.Write(goodblock, 0, (int)BigBlockLength);
                            curBadBlock--;
                        }
                    }
                }
            }

            _dataStream.Position = 0;
            _eccStream.Position = 0;
            _fileStream.Position = 0;
        }
        public ISpareData GetBlockSpare(int blockNumber)
        {
            return GetPageSpare(blockNumber * PagesPerBlock);
        }
        public ISpareData GetBigBlockSpare(int blockNumber)
        {
            return GetPageSpare(blockNumber * PagesPerBigBlock);
        }
        public bool WriteBigBlockSpare(int blockNumber, ISpareData eccData)
        {
            return WritePageSpare(blockNumber * PagesPerBigBlock, eccData);
        }
        public bool WriteBlockSpare(int blockNumber, ISpareData eccData)
        {
            return WritePageSpare(blockNumber * PagesPerBlock, eccData);
        }
        public void SeekToBlock(int blockNumber)
        {
            SeekToPage(blockNumber * PagesPerBlock);
        }
        public void SeekToBigBlock(int blockNumber)
        {
            SeekToPage(blockNumber * PagesPerBigBlock);
        }
        public long BlockLength
        {
            get
            {
                return PagesPerBlock * PageLength;
            }
        }
        public long BigBlockLength
        {
            get
            {
                return PagesPerBigBlock * PageLength;
            }
        }
        public void SeekToPage(int pageNumber)
        {
            Position = pageNumber * PageLength;
        }
        public ISpareData GetPageSpare(int pageNumber)
        {
            int position = pageNumber * 0x10;
            if (position >= _eccStream.Length)
            {
                //Console.WriteLine("Corrupted nand image detected, did you slice off the end?");
                return null;
            }
            _eccStream.Position = position;
            byte[] ecc = new byte[0x10];
            int count = _eccStream.Read(ecc, 0, 0x10);
            if (count != 0x10)
                return null;
            ISpareData data;
            if (SpareDataType == SpareDataType.BigBlock)
                data = new SpareDataBigBlock();
            else if (SpareDataType == SpareDataType.BigOnSmall)
                data = new SpareDataBigOnSmall();
            else
                data = new SpareDataSmallBlock();

            data.Read(ecc);
            return data;
        }
        public bool WritePageSpare(int pageNumber, ISpareData eccData)
        {
            int position = pageNumber * 0x10;
            if (position >= _eccStream.Length)
                return false;
            _eccStream.Position = position;
            _eccStream.Write(eccData.Write(), 0, 0x10);
            return true;
        }
        public override void Flush()
        {
            _dataStream.Flush();
            _eccStream.Flush();
        }
        public override long Seek(long offset, SeekOrigin origin)
        {
            return _dataStream.Seek(offset, origin);
        }
        public override void SetLength(long length)
        {
            _dataStream.SetLength(length);
        }
        /*void calcecc(unsigned long *data, unsigned char  *ecc)
{
	int i=0, val=0;
	unsigned long v;
	
	for (i = 0; i < 0x1066; i++) {
		if (!(i & 31)) 
			v = ~data[i>>5];
		val ^= v & 1;
		v >>= 1;
		if (val & 1)
			val ^= 0x6954559;
		val >>= 1;
	}
	
	val = ~val;
	
	ecc[0] = (val << 6) & 0xFF;
	ecc[1] = (val >> 2) & 0xFF;
	ecc[2] = (val >> 10) & 0xFF;
	ecc[3] = (val >> 18) & 0xFF;
}*/
        public byte[] CalcEDC(byte[] data, byte[] eccdata)
        {
            byte[] dataa = new byte[PageLength + 0x10];
            Array.Copy(data, dataa, PageLength);
            Array.Copy(eccdata, 0, dataa, PageLength, 0x10);
            int val = 0;
            int v = 0;
            for (int i = 0; i < 0x1066; i++)
            {
                if ((i & 31) == 0)
                    v = ~BitConverter.ToInt32(dataa, i / 8);
                val ^= (v & 1);
                v >>= 1;
                if ((val & 1) > 0)
                    val ^= 0x6954559;
                val >>= 1;
            }
            val = ~val;
            byte[] edc = eccdata;
            edc[0xC] = (byte)(((val << 6) | (eccdata[0xC] & 0x3F)) & 0xFF);
            edc[0xD] = (byte)((val >> 2) & 0xFF);
            edc[0xE] = (byte)((val >> 10) & 0xFF);
            edc[0xF] = (byte)((val >> 18) & 0xFF);
            return edc;
        }
        public override void Close()
        {
            Close(false);
            base.Close();
        }
        public void Close(bool save)
        {
            MemoryStream _imageStream = new MemoryStream();
            _dataStream.Flush();
            _eccStream.Flush();
            if (save)
            {
                
                _dataStream.Position = 0;
                _eccStream.Position = 0;
                _fileStream.Position = 0;
                _imageStream.Position = 0;

              
                for (int i = 0; i < BlockCount; i++)
                {
                    for (int j = 0; j < PagesPerBlock; j++)
                    {
                        byte[] data = new byte[PageLength];
                        byte[] ecc = new byte[SpareLength];
                        _dataStream.Read(data, 0, PageLength);
                        _eccStream.Read(ecc, 0, SpareLength);
                        // if (SpareDataType == SpareDataType.None)
                        {

                            short block = (short)i;
                            ISpareData spdata;
                            if (SpareDataType == SpareDataType.BigBlock)
                                spdata = new SpareDataBigBlock();
                            else if (SpareDataType == SpareDataType.BigOnSmall)
                                spdata = new SpareDataBigOnSmall();
                            else
                                spdata = new SpareDataSmallBlock();
                            spdata.Read(ecc);
                            spdata.BlockId = (short)i;
                            ecc = spdata.Write();
                            ecc = CalcEDC(data, ecc);
                        }
                        _imageStream.Write(data, 0, PageLength);
                        _imageStream.Write(ecc, 0, SpareLength);
                    }
                }

                // now lets remap badblocks
                int curBadBlock = 0;
                int BigBlockLengthECC = PagesPerBigBlock * (PageLength + 0x10);

                byte[] blank = new byte[BigBlockLengthECC];
                for (int b = 0; b < BigBlockLengthECC; b++) blank[b] = 0x00;

                foreach (short i in badblocks)
                {
                    if (i < BadBlockAreaStart && i > 0)
                    {

                        ISpareData spare = GetBigBlockSpare(i);
                        Console.WriteLine("Remapping bad block "+i.ToString("X"));
                        int remappedBlock = BlockCount - (curBadBlock + 1);


                        while (badblocks.Contains(remappedBlock) && curBadBlock < 32)
                        {
                            Console.WriteLine("Skipping bad spare block " + remappedBlock.ToString("X"));
                            _imageStream.Position = (remappedBlock * PagesPerBigBlock * (PageLength + 0x10));
                            _imageStream.Write(blank, 0, (int)BigBlockLengthECC);

                            curBadBlock++;
                            remappedBlock--;
                        }
                        
                        //copy bad block
                        _imageStream.Position = (i * PagesPerBigBlock * (PageLength + 0x10));
                        byte[] badblock = new byte[BigBlockLengthECC];
                        _imageStream.Read(badblock, 0, (int)BigBlockLengthECC);

                        //wipe old spot
                        _imageStream.Position = (i * PagesPerBigBlock * (PageLength + 0x10));
                        _imageStream.Write(blank, 0, (int)BigBlockLengthECC);

                        //copy to new spot
                        _imageStream.Position = (remappedBlock * PagesPerBigBlock * (PageLength + 0x10));
                        _imageStream.Write(badblock, 0, (int)BigBlockLengthECC);
                        
                        curBadBlock++;
                        if (curBadBlock == 0x20) break; //need to change this for BigBlock

                    }
                    else if (i > BlockCount || i < 0)
                    {
                        Console.WriteLine("Invalid bad block ID " + i.ToString("X"));
                    } 
                }

                
                //fill in unused bad block reserves with FF
                for (int i = BadBlockAreaStart; i < BlockCount - curBadBlock; i++)
                {
                    _imageStream.Position = (i * PagesPerBigBlock * (PageLength + 0x10)); 

                    byte[] data = new byte[PageLength];
                    byte[] ecc = new byte[SpareLength];
                    for (int b = 0; b < PageLength; b++) data[b] = 0xFF;
                    for (int b = 0; b < SpareLength; b++) ecc[b] = 0xFF;

                    for (int j = 0; j < PagesPerBlock; j++)
                    {

                        _imageStream.Write(data, 0, PageLength);
                        _imageStream.Write(ecc, 0, SpareLength);
                    }
                    
                }

            }


            
            byte[] buff = new byte[0x10];
            int bytesRead = 0;
            _imageStream.Position = 0;
            _fileStream.Position = 0;
            try
            {
                while ((bytesRead = _imageStream.Read(buff, 0, buff.Length)) != 0) _fileStream.Write(buff, 0, bytesRead);
            }
            catch { }
            if (_fileStream.GetType() == typeof(MemoryStream))
                TempReturnData = ((MemoryStream)_fileStream).ToArray();
            

            _fileStream.Close();
            _imageStream.Close();
            _dataStream.Close();
            _eccStream.Close();
        }
        public override int Read(byte[] buffer, int offset, int count)
        {
            return _dataStream.Read(buffer, offset, count);
        }
        public override void Write(byte[] buffer, int offset, int count)
        {
            _dataStream.Write(buffer, offset, count);
        }
        public override bool CanRead
        {
            get { return _dataStream.CanRead; }
        }

        public override bool CanWrite
        {
            get { return _dataStream.CanWrite; }
        }

        public override bool CanSeek
        {
            get { return _dataStream.CanSeek; }
        }

        public override long Length
        {
            get { return _dataStream.Length; }
        }
        public override long Position
        {
            get { return _dataStream.Position; }
            set { _dataStream.Position = value; }
        }
    }
}
