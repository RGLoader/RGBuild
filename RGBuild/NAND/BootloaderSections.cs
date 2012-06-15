using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using RGBuild.IO;
using RGBuild.ThirdParty;
using RGBuild.Util;

namespace RGBuild.NAND
{
    public struct RC4Session
    {
        public byte[] Key;
        public int SBoxLen;
        public byte[] SBox;
        public int I;
        public int J;
    }
    public abstract class AccountRC4
    {
        public static RC4Session RC4CreateSession(byte[] key)
        {
            RC4Session session = new RC4Session { Key = key, I = 0, J = 0, SBoxLen = 0x100, SBox = new byte[0x100] };

            for (int x = 0; x < session.SBoxLen; x++)
                session.SBox[x] = (byte)x;

            int j = 0;
            for (int x = 0; x < session.SBoxLen; x++)
            {
                j = (j + session.SBox[x] + key[x % key.Length]) % session.SBoxLen;
                byte temp = session.SBox[j];
                session.SBox[j] = session.SBox[x];
                session.SBox[x] = temp;
            }
            return session;
        }
        public static void ResetSession(ref RC4Session session)
        {
            session = RC4CreateSession(session.Key);
        }

        public static void RC4Encrypt(ref RC4Session session, byte[] data, int index, int count)
        {
            int offset = index;

        begin:

            session.I = (session.I + 1) % 0x100;
            session.J = (session.J + session.SBox[session.I]) % 0x100;
            byte temp = session.SBox[session.I];
            session.SBox[session.I] = session.SBox[session.J];
            session.SBox[session.J] = temp;
            byte a = data[offset];
            byte b = session.SBox[(session.SBox[session.I] + session.SBox[session.J]) % 0x100];
            data[offset] = (byte)(a ^ b);
            offset++;
            if (offset != index + count)
                goto begin;
        }
        public static void RC4Decrypt(ref RC4Session session, byte[] data, int index, int count)
        {
            RC4Encrypt(ref session, data, index, count);
        }

    }

    public class SFCXECCData
    {
        public byte BlockId1;
        public byte BlockId0; // only 4 bits are used

        public short BlockId
        {
            get
            {
                return (short)(((BlockId0 & 0xF) << 8) + BlockId1);
            }
        }

        public byte FsSequence0;
        public byte FsSequence1;
        public byte FsSequence2;
        public byte BadBlockIndicator;
        public byte FsSequence3;
        public int FsSequence
        {
            get
            {
                return (FsSequence3 << 24) + (FsSequence2 << 16) + (FsSequence1 << 8) + FsSequence0;
            }
        }
        public byte FsSize0;
        public byte FsSize1;
        public byte FsPageCount;
        public short Unused1;
        public byte Edc3;
        public byte Edc2;
        public byte Edc1;
        public byte Edc0;
    }

    public class Bootloader
    {
        public NANDImage Image;
        public Bootloader PreviousLoader;
        public long Position;
        public BootloaderSecurityType SecureType
        {
            get
            {
                // get this from the image
                if (GetType() == typeof(Bootloader1BL))
                    return BootloaderSecurityType.Unknown;
                Bootloader last = Image.GetLastExecutableBootloader(this);

                if (last == null || (GetType() == typeof(Bootloader2BL) && last.GetType() == typeof(Bootloader1BL)))
                    return BootloaderSecurityType.Unknown;
                return last.VerifyLoader(this);
            }
        }

        public NANDBootloaderMagic Magic;
        public ushort Build;
        public ushort Qfe;
        public ushort Flags;
        public uint Entrypoint;
        public uint Size;

        public uint Size_r
        {
            get
            {
                return (Size + (uint)0xF) & (~(uint)0xF);
            }
        }

        public byte[] HmacShaNonce = new byte[0x10];
        public byte[] HmacShaKey = new byte[0x10];
        public byte[] Rc4Key = new byte[0x10];

        public byte[] EncryptedData;
        public byte[] DecryptedData;

        public int SecuredDataStart;

        public Bootloader(NANDImage image)
        {
            Image = image;
        }

        public Bootloader(NANDImage image, Bootloader prevLoader)
        {
            Image = image;
            PreviousLoader = prevLoader;
            HmacShaKey = PreviousLoader != null ? PreviousLoader.Rc4Key : image._1BLKey;
        }

        public virtual void Read()
        {
            Read(Image.IO);
        }
        public virtual void Read(X360IO io)
        {
            Position = io.Stream.Position;
            Magic = (NANDBootloaderMagic)io.Reader.ReadUInt16();
            Build = io.Reader.ReadUInt16();
            Qfe = io.Reader.ReadUInt16();
            Flags = io.Reader.ReadUInt16();
            Entrypoint = io.Reader.ReadUInt32();
            Size = io.Reader.ReadUInt32();
        }
        public virtual byte[] GetData(bool writeDecrypted=true)
        {
            return null;
        }
        public virtual byte[] GetData()
        {
            return null;
        }
        public virtual void SetData(byte[] data)
        {
            return;
        }
        public virtual BootloaderSecurityType VerifyLoader(Bootloader bootloader)
        {
            return BootloaderSecurityType.Unknown;
        }
        public virtual void Write()
        {
            Write(Image.IO);
            //new Random().NextBytes(HmacShaNonce);
        }
        public virtual void Write(X360IO io)
        {
            io.Writer.Write((ushort)Magic);
            io.Writer.Write(Build);
            io.Writer.Write(Qfe);
            io.Writer.Write(Flags);
            io.Writer.Write(Entrypoint);
            io.Writer.Write(Size);
        }
        public byte[] HmacRc4(byte[] data)
        {
            Tuple<byte[], byte[]> enc = Shared.HmacRc4(HmacShaKey, HmacShaNonce, data);
            Rc4Key = enc.Item2;
            return enc.Item1;
        }
    }
    public class RGBPayloadEntry
    {
        public uint Address;
        public uint Size;
        public byte DescSize;
        public string Description;
        public byte[] Data;
        public void Load(X360IO io)
        {
            Address = io.Reader.ReadUInt32();
            if (Address == 0xFFFFFFFF)
                return;
            Size = io.Reader.ReadUInt32();
            DescSize = io.Reader.ReadByte();
            Description = io.Reader.ReadAsciiString(DescSize);
            if(io.Stream.GetType() == typeof(NANDImageStream))
            {
                long pos = io.Stream.Position;
                io.Stream.Position = Address;
                Data = io.Reader.ReadBytes((int)Size);
                io.Stream.Position = pos;
            }
        }
        public void Write(X360IO io)
        {
            Size = (uint)Data.Length;
            io.Writer.Write(Address);
            io.Writer.Write(Size);
            if((Description.Length > 0xFF))
                Description = Description.Substring(0, 0xFF);
            DescSize = (byte) Description.Length;
            io.Writer.Write(DescSize);
            io.Writer.WriteAsciiString(Description, DescSize);
            if (io.Stream.GetType() == typeof(NANDImageStream))
            {
                long pos = io.Stream.Position;
                io.Stream.Position = Address;
                io.Writer.Write(Data);
                io.Stream.Position = pos;
            }
        }
    }
    public class RGBPayloadList
    {
        public X360IO IO;
        public int Length;
        public List<RGBPayloadEntry> Payloads = new List<RGBPayloadEntry>();

        public RGBPayloadList(X360IO io)
        {
            IO = io;
        }

        public void Read()
        {
            Read(IO);
        }

        public void Read(X360IO io)
        {
            int start = (int)io.Stream.Position;
            while(true)
            {
                RGBPayloadEntry entry = new RGBPayloadEntry();
                entry.Load(io);
                if (entry.Address == 0xFFFFFFFF)
                    break;
                Payloads.Add(entry);
            }
            Length = (int) io.Stream.Position - start;
        }

        public void Write()
        {
            Write(IO);
        }
        
        public void Write(X360IO io)
        {
            int start = (int)io.Stream.Position;
            foreach(RGBPayloadEntry entry in Payloads)
            {

                entry.Write(io);
            }
            io.Writer.Write(0xFFFFFFFF);
            Length = (int)io.Stream.Position - start;
        }
    }
    public class SMC
    {
        public X360IO IO;
        public byte[] EncryptedData;
        public byte[] DecryptedData;
        public SMC(X360IO io)
        {
            IO = io;
        }
        public void Read(int smcSize)
        {
            Read(IO, smcSize, false);
        }
        static byte[] CalculateSMCHash(byte[] smcdata, int length)
        {
            ulong s0 = 0;
            ulong s1 = 0;
            for (int i = 0; i < length / 4; i++)
            {
                uint tmp = BitConverter.ToUInt32(smcdata, i * 4);
                uint tmp2 = ((tmp & 0xFF) << 24) | ((tmp & 0xFF00) << 8) | ((tmp & 0xFF0000) >> 8) |
                       ((tmp & 0xFF000000) >> 24); // endian swap
                s0 += tmp2;
                s1 -= tmp2;
                s0 = (s0 << 29) | ((s0 & 0xFFFFFFF800000000) >> 35); // poor man's rotate left 29
                s1 = (s1 << 31) | ((s1 & 0xFFFFFFFE00000000) >> 33); // poor man's rotate left 31
            }
            byte[] csum = new byte[0x10];
            csum[7] = (byte)(s0 & 0xFF);
            csum[6] = (byte)((s0 & 0xFF00) >> 8);
            csum[5] = (byte)((s0 & 0xFF0000) >> 16);
            csum[4] = (byte)((s0 & 0xFF000000) >> 24);
            csum[3] = (byte)((s0 & 0xFF00000000) >> 32);
            csum[2] = (byte)((s0 & 0xFF0000000000) >> 40);
            csum[1] = (byte)((s0 & 0xFF000000000000) >> 48);
            csum[0] = (byte)((s0 & 0xFF00000000000000) >> 56);
            csum[15] = (byte)(s1 & 0xFF);
            csum[14] = (byte)((s1 & 0xFF00) >> 8);
            csum[13] = (byte)((s1 & 0xFF0000) >> 16);
            csum[12] = (byte)((s1 & 0xFF000000) >> 24);
            csum[11] = (byte)((s1 & 0xFF00000000) >> 32);
            csum[10] = (byte)((s1 & 0xFF0000000000) >> 40);
            csum[9] = (byte)((s1 & 0xFF000000000000) >> 48);
            csum[8] = (byte)((s1 & 0xFF00000000000000) >> 56);
            return csum;
        }
        public void Read(X360IO io, int smcSize, bool isDecrypted)
        {
            byte[] payload = io.Reader.ReadBytes(smcSize);
            if (!isDecrypted)
            {
                EncryptedData = payload;
                UnmungeSMC();
            }
            else
            {
                DecryptedData = payload;
                MungeSMC();
            }
        }
        public byte[] GetData(bool decrypted)
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, decrypted);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public byte[] GetHash()
        {
            return GetHash(true);
        }
        public byte[] GetHash(bool useDecrypted)
        {
            MungeSMC();
            return CalculateSMCHash(EncryptedData, EncryptedData.Length);
        }
        public void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io, data.Length, true);
            return;
        }
        public void Write()
        {
            Write(IO, false);
        }
        public void Write(X360IO io, bool writeDecrypted)
        {
            if(writeDecrypted)
                io.Writer.Write(DecryptedData);
            else
            {
                MungeSMC();
                io.Writer.Write(EncryptedData);
            }
        }
        public void UnmungeSMC()
        {
            byte[] key = new byte[] {0x42, 0x75, 0x4e, 0x79};
            DecryptedData = new byte[EncryptedData.Length];
            for(int i = 0; i < EncryptedData.Length; i++)
            {
                byte j = EncryptedData[i];
                int mod = j * 0xFB;
                byte decrypted = (byte)(j ^ (key[i & 3] & 0xFF));
                DecryptedData[i] = decrypted;
                key[(i + 1) & 3] += (byte)mod;
                key[(i + 2) & 3] += (byte)(mod >> 8);
            }
        }
        public void MungeSMC()
        {
            byte[] key = new byte[] { 0x42, 0x75, 0x4e, 0x79 };
            EncryptedData = new byte[DecryptedData.Length];
            for (int i = 0; i < DecryptedData.Length; i++)
            {
                byte j = (byte)(DecryptedData[i] ^ (key[i & 3] & 0xFF));
                int mod = j * 0xFB;
                EncryptedData[i] = j;
                key[(i + 1) & 3] += (byte)mod;
                key[(i + 2) & 3] += (byte)(mod >> 8);
            }
        }

        // thanks to cOz for this!
        public string GetConsoleType()
        {
            int typeIndex = (GetData(true)[0x100]) >> 4 & 0xF;
            string[] types = { "none/unk", "Xenon", "Zephyr", "Falcon", "Jasper", "Trinity", "Corona", "Winchester" };
            return types[typeIndex];
        }
    }

    public class BootloaderFlashHeader : Bootloader
    {
        public byte CopyrightSign = 0xa9;
        public string Copyright = " 2004-2011 Microsoft Corporation. All rights reserved."; // 0x40 bytes
        public byte[] Reserved = new byte[0x10];
        public uint KeyVaultSize = 0x4000;
        public uint SysUpdateAddress = 0x70000;
        public ushort SysUpdateCount = 2;
        public ushort KeyVaultVersion = 0x712;
        public uint KeyVaultAddress = 0x4000;
        public uint FileSystemAddress = 0x10000;
        public uint SmcConfigAddress;
        public uint SmcSize = 0x3000;
        public uint SmcAddress = 0x1000;
        public ushort RGBPIndicator;
        public bool ContainsRGBP
        {
            get
            {
                return RGBPIndicator == 0x1337;
            }
        }
        public int HeaderVersion
        {
            get
            {
                if (KeyVaultVersion != 0x712)
                    return 0;
                return 1;
            }
        }
        
        public BootloaderFlashHeader(NANDImage image)
            : base(image)
        {
            
        }

        public override void Read()
        {
            Read(Image.IO);
        }
        public override void Read(X360IO io)
        {
            base.Read(io);
            CopyrightSign = io.Reader.ReadByte();
            Copyright = io.Reader.ReadAsciiString(0x3F);
            Reserved = io.Reader.ReadBytes(0x10);
            KeyVaultSize = io.Reader.ReadUInt32();
            SysUpdateAddress = io.Reader.ReadUInt32();
            SysUpdateCount = io.Reader.ReadUInt16();
            KeyVaultVersion = io.Reader.ReadUInt16();
            KeyVaultAddress = io.Reader.ReadUInt32();
            FileSystemAddress = io.Reader.ReadUInt32();
            SmcConfigAddress = io.Reader.ReadUInt32();
            SmcSize = io.Reader.ReadUInt32();
            SmcAddress = io.Reader.ReadUInt32();
            RGBPIndicator = io.Reader.ReadUInt16();
            if (HeaderVersion == 0)
                Build = io.Reader.ReadUInt16();
        }

        public override byte[] GetData()
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public override void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io);
            return;
        }

        public override void Write()
        {
            Write(Image.IO);
        }
        public override void Write(X360IO io)
        {
            base.Write(io);
            io.Writer.Write(CopyrightSign);
            io.Writer.WriteAsciiString(Copyright, 0x3F);
            io.Writer.Write(Reserved);
            io.Writer.Write(KeyVaultSize);
            io.Writer.Write(SysUpdateAddress);
            io.Writer.Write(SysUpdateCount);
            io.Writer.Write(KeyVaultVersion);
            io.Writer.Write(KeyVaultAddress);
            io.Writer.Write(FileSystemAddress);
            io.Writer.Write(SmcConfigAddress);
            io.Writer.Write(SmcSize);
            io.Writer.Write(SmcAddress);
            io.Writer.Write(RGBPIndicator);
        }
    }
    
    public class Bootloader2BLPerBoxData
    {
        public Bootloader2BL Bootloader;
        public byte[] PairingData = new byte[3];
        public byte LockDownValue;
        public byte[] Reserved = new byte[0xC]; // 0xC
        public byte[] PerBoxDigest = new byte[0x10]; // 0x10

        public Bootloader2BLPerBoxData(Bootloader2BL bootloader)
        {
            Bootloader = bootloader;
        }
        public void Read(X360IO io)
        {
            PairingData = io.Reader.ReadBytes(3);
            LockDownValue = io.Reader.ReadByte();
            Reserved = io.Reader.ReadBytes(0xC);
            PerBoxDigest = io.Reader.ReadBytes(0x10);
        }
        public byte[] CalculateDigest()
        {
            byte[] smchash = Bootloader.Image.SMC.GetHash();
            X360IO io = new X360IO(new MemoryStream());
            io.Writer.Write(Bootloader.Rc4Key);
            io.Writer.Write(PairingData);
            io.Writer.Write(LockDownValue);
            io.Writer.Write(Reserved);
            io.Writer.Write(smchash);
            byte[] data = ((MemoryStream) io.Stream).ToArray();
            io.Close();
            data = new HMACSHA1(Bootloader.Image.CPUKey).ComputeHash(data);
            Array.Resize(ref data, 0x10);
            return data;
        }
        public void Write(X360IO io)
        {
            io.Writer.Write(PairingData);
            io.Writer.Write(LockDownValue);
            io.Writer.Write(Reserved);
            io.Writer.Write(PerBoxDigest);
        }
    }
    public class Bootloader2BLConsoleTypeSeqAllow
    {
        public byte ConsoleType;
        public bool ConsoleTypeIsAllowed(ulong fuseline) // line 1 of fuses
        {
            if ((fuseline & 0xF) == 0)
                return ConsoleType == 0;
            return ConsoleType == 1;
        }
        public byte ConsoleSequence;         
        public ushort ConsoleSequenceAllow;
        public bool ConsoleSequenceIsAllowed(ulong fuseline) // line 2 of fuses
        {
            if (fuseline == 0)
                return true;
            int i = 0;
            for (i = 0; i < 0x10; i++)
            {
                if ((fuseline & 0xF) == 0xF)
                    break;
                fuseline = fuseline >> 4;
            }

            if (i == 0 || (0x10 - i) == ConsoleSequence)
                return true;

            return ((1 << ((0x10 - i) - 1)) & ConsoleSequenceAllow) > 0;
        }
        public void Read(X360IO io)
        {
            ConsoleType = io.Reader.ReadByte();
            ConsoleSequence = io.Reader.ReadByte();
            ConsoleSequenceAllow = io.Reader.ReadUInt16();
        }
        public void Write(X360IO io)
        {
            io.Writer.Write(ConsoleType);
            io.Writer.Write(ConsoleSequence);
            io.Writer.Write(ConsoleSequenceAllow);
        }
    }
    public enum BootloaderSecurityType
    {
        Unknown = 0,
        Insecure = 1,
        SecureStock = 2,
        SecureGlitch = 4
        // time and a place
        // SecureJTAG = 8 
    }
    public class Bootloader1BL : Bootloader
    {
        public byte CopyrightSign; // 0xAC
        public string CopyrightString; // length 0xEB
        public uint MetaOffset; // usually set to 0x5730
        public byte[] RsaPublicKey; // MO+0
        public ulong POSTOutputAddress; // MO+0x110
        public ulong UnkAddress1; // MO+0x118
        public ulong UnkAddress2; // MO+0x120
        public ulong UnkAddress3; // MO+0x128
        public ulong SbFlashAddress; // MO+0x130
        public ulong SocMmioAddress; // MO+0x138
        public ulong UnkAddress4; // MO+0x140
        public byte[] Nonce2BL; // AKA 1BL Key
        public byte[] Salt2BL;

        public Bootloader1BL(NANDImage image) : base(image)
        {
            
        }

        public override void Read(X360IO io)
        {
            base.Read(io);
            CopyrightSign = io.Reader.ReadByte();
            CopyrightString = io.Reader.ReadAsciiString(0xEB);
            MetaOffset = io.Reader.ReadUInt32();
            DecryptedData = io.Reader.ReadBytes((int) Size - 0x100);
            X360IO io2 = new X360IO(DecryptedData, true) {Stream = {Position = MetaOffset - 0x100}};
            RsaPublicKey = io2.Reader.ReadBytes(0x110);
            POSTOutputAddress = io2.Reader.ReadUInt64();
            UnkAddress1 = io2.Reader.ReadUInt64();
            UnkAddress2 = io2.Reader.ReadUInt64();
            UnkAddress3 = io2.Reader.ReadUInt64();
            SbFlashAddress = io2.Reader.ReadUInt64();
            SocMmioAddress = io2.Reader.ReadUInt64();
            UnkAddress4 = io2.Reader.ReadUInt64();
            Nonce2BL = io2.Reader.ReadBytes(0x10);
            Salt2BL = io2.Reader.ReadBytes(0xA);
            io2.Close();
        }
        public override byte[] GetData()
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public override void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io);
        }
        public override void Write(X360IO io)
        {
            base.Write(io);
            io.Writer.Write(CopyrightSign);
            int pos = (int)io.Stream.Position;
            io.Writer.Write(CopyrightString);
            io.Stream.Position = pos + 0xEB;
            io.Writer.Write(MetaOffset);
            io.Writer.Write(DecryptedData); // lazy
        }
        public override BootloaderSecurityType VerifyLoader(Bootloader bootloader)
        {
            byte[] bldata = bootloader.GetData();
            byte[] headerdata = new byte[0x10];
            byte[] dgdata = new byte[bldata.Length - bootloader.SecuredDataStart];
            Array.Copy(bldata, headerdata, 0x10);
            Array.Copy(bldata, bootloader.SecuredDataStart, dgdata, 0, dgdata.Length);
            byte[] hash = RotSumSha.XeCryptRotSumSha(headerdata, 0x10, dgdata, dgdata.Length);

            if (bootloader.GetType() == typeof(Bootloader2BL))
            {
                // verify next loaders signature
                byte[] sig = new byte[0x100];
                Array.Copy(((Bootloader2BL) bootloader).Signature, sig, 0x100);

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(Misc.GenerateRSAParametersFromPublicKey(RsaPublicKey));
               // rsa.
                RSAPKCS1SignatureDeformatter sigDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                sigDeformatter.SetHashAlgorithm("SHA");
                Array.Reverse(sig);
                return sigDeformatter.VerifySignature(hash, sig) ? BootloaderSecurityType.SecureStock : BootloaderSecurityType.Unknown;
            }

            return VerifyLoader(bootloader);
        }
    }
    public class Bootloader2BL : Bootloader
    {
        public Bootloader2BLPerBoxData PerBoxData;
        public byte[] Signature = new byte[0x100]; // 0x100
        public byte[] AesInvData = new byte[0x110]; // 0x110
        public ulong POSTOutputAddress;
        public ulong SbFlashAddress;
        public ulong SocMmioAddress;
        public byte[] RsaPublicKey = new byte[0x110]; // 0x110
        public byte[] Nonce3BL = new byte[0x10]; // 0x10
        public byte[] Salt3BL = new byte[0xA]; // 0xA
        public byte[] Salt4BL = new byte[0xA]; // 0xA
        public byte[] Digest4BL = new byte[0x14]; // 0x14
        public Bootloader2BLConsoleTypeSeqAllow ConsoleTypeSeqAllowData = new Bootloader2BLConsoleTypeSeqAllow();
        public int Padding;

        // CB_B
        public byte[] CPUKey;
        public Bootloader2BL(NANDImage image, Bootloader prevbl) : base(image, prevbl)
        {
            PerBoxData = new Bootloader2BLPerBoxData(this);
            SecuredDataStart = 0x140;
        }
        public Bootloader2BL(NANDImage image, Bootloader prevbl, byte[] cpukey)
            : base(image, prevbl)
        {
            PerBoxData = new Bootloader2BLPerBoxData(this);
            CPUKey = cpukey;
            SecuredDataStart = 0x140;
        }
        public override void Read()
        {
            Read(Image.IO, false);
        }
        public void Read(X360IO io, bool isDecrypted)
        {
            Read(io);
            HmacShaNonce = io.Reader.ReadBytes(0x10);
            if(CPUKey != null)
            {
                Array.Resize(ref HmacShaNonce, 0x20);
                Array.Copy(CPUKey, 0, HmacShaNonce, 0x10, 0x10);
            }
            if (PreviousLoader !=null && PreviousLoader.GetType() == typeof(Bootloader2BL))
            {
                if ((PreviousLoader.Flags & (ushort)0x1000) != 0)
                {
                    Array.Resize(ref HmacShaNonce, HmacShaNonce.Length+0x10);
                    byte[] CBAdata = PreviousLoader.GetData(true);
                    CBAdata[6] = 0; CBAdata[7] = 0; 
                    Array.Copy(CBAdata, 0, HmacShaNonce, HmacShaNonce.Length-0x10, 0x10);
                }
            }
            byte[] payload = io.Reader.ReadBytes((int)Size - 0x20);
            if (!isDecrypted)
            {
                EncryptedData = payload;
                DecryptedData = HmacRc4(EncryptedData);
            }
            else
                DecryptedData = payload;
            
            X360IO io2 = new X360IO(DecryptedData, true);
            PerBoxData = new Bootloader2BLPerBoxData(this);
            PerBoxData.Read(io2);
            Signature = io2.Reader.ReadBytes(0x100);
            AesInvData = io2.Reader.ReadBytes(0x110);
            POSTOutputAddress = io2.Reader.ReadUInt64();
            SbFlashAddress = io2.Reader.ReadUInt64();
            SocMmioAddress = io2.Reader.ReadUInt64();
            RsaPublicKey = io2.Reader.ReadBytes(0x110);
            Nonce3BL = io2.Reader.ReadBytes(0x10);
            Salt3BL = io2.Reader.ReadBytes(0xA);
            Salt4BL = io2.Reader.ReadBytes(0xA);
            Digest4BL = io2.Reader.ReadBytes(0x14);
            ConsoleTypeSeqAllowData = new Bootloader2BLConsoleTypeSeqAllow();
            ConsoleTypeSeqAllowData.Read(io2);
            io2.Close();
        }
        public override byte[] GetData(bool writeDecrypted)
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, writeDecrypted);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public override byte[] GetData()
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, true);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public override void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io, true);
        }
        public override void Write()
        {
            Write(Image.IO, false);
        }
        public void Write(X360IO io, bool writeDecrypted)
        {
            Write(io);
            Array.Resize(ref HmacShaNonce, 0x10);
            io.Writer.Write(HmacShaNonce);
            X360IO io2 = new X360IO(DecryptedData, true);
            PerBoxData.Write(io2);
            io2.Writer.Write(Signature);
            io2.Writer.Write(AesInvData);
            io2.Writer.Write(POSTOutputAddress);
            io2.Writer.Write(SbFlashAddress);
            io2.Writer.Write(SocMmioAddress);
            io2.Writer.Write(RsaPublicKey);
            io2.Writer.Write(Nonce3BL);
            io2.Writer.Write(Salt3BL);
            io2.Writer.Write(Salt4BL);
            io2.Writer.Write(Digest4BL);
            ConsoleTypeSeqAllowData.Write(io2);
            DecryptedData = ((MemoryStream) io2.Stream).ToArray();
            io2.Close();
            if (!writeDecrypted)
            {
                if (CPUKey != null)
                {
                    Array.Resize(ref HmacShaNonce, 0x20);
                    Array.Copy(CPUKey, 0, HmacShaNonce, 0x10, 0x10);
                }
                EncryptedData = HmacRc4(DecryptedData);
                io.Writer.Write(EncryptedData);
            }
            else
                io.Writer.Write(DecryptedData);

        }
        public override BootloaderSecurityType VerifyLoader(Bootloader bootloader)
        {
            byte shapost = 0;
            byte sigpost = 0;
            byte[] bldata = bootloader.GetData();
            if (bootloader.GetType() == typeof(Bootloader2BL))
            {
                shapost = 0xDA;
            }
            else if (bootloader.GetType() == typeof(Bootloader3BL))
            {
                sigpost = 0x2D;
            }
            else if (bootloader.GetType() == typeof(Bootloader4BL))
            {
                shapost = 0x39;
                sigpost = 0x38;
            }
            bool sigcheck = sigpost != 0 && shapost == 0;
            if (sigpost != 0 && shapost != 0)
            {
                // scan the bastard
                byte[] data = new byte[] { 0x38, 0x80, 0x00, sigpost };
                for (int i = 0; i < bldata.Length; i += 4)
                {
                    if (bldata[i] == data[0] && bldata[i + 1] == data[1] && bldata[i + 2] == data[2] && bldata[i + 3] == data[3])
                    {
                        // found the sig post
                        sigcheck = true;
                        break;
                    }
                }
            }

            byte[] headerdata = new byte[0x10];
            byte[] dgdata = new byte[bldata.Length - bootloader.SecuredDataStart];
            Array.Copy(bldata, headerdata, 0x10);
            Array.Copy(bldata, bootloader.SecuredDataStart, dgdata, 0, dgdata.Length);
            byte[] hash = RotSumSha.XeCryptRotSumSha(headerdata, 0x10, dgdata, dgdata.Length);

            if (sigcheck)
            {

                // verify next loaders signature
                byte[] sig = (bootloader.GetType() == typeof(Bootloader3BL)
                                  ? ((Bootloader3BL)bootloader).Signature
                                  : ((Bootloader4BL)bootloader).Signature);

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(Misc.GenerateRSAParametersFromPublicKey(RsaPublicKey));
                RSAPKCS1SignatureDeformatter sigDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                sigDeformatter.SetHashAlgorithm("SHA");
                return sigDeformatter.VerifySignature(hash, sig) ? BootloaderSecurityType.SecureStock : BootloaderSecurityType.Unknown;
            }
            return hash.SequenceEqual(Digest4BL) ? BootloaderSecurityType.SecureStock : (Lists.GlitchableCBs.Contains(Build)) ? BootloaderSecurityType.SecureGlitch : BootloaderSecurityType.Insecure;
        }
    }
    public class Bootloader3BL : Bootloader
    {
        public byte[] Signature = new byte[0x100]; // 0x100
        public Bootloader3BL(NANDImage image, Bootloader prevbl)
            : base(image, prevbl)
        {
            HmacShaKey = new byte[16];
            SecuredDataStart = 0x120;
        }
        public override void Read()
        {
            Read(Image.IO, false);
        }
        public void Read(X360IO io, bool isDecrypted)
        {
            Read(io);
            HmacShaNonce = io.Reader.ReadBytes(0x10);
            byte[] payload = io.Reader.ReadBytes((int)Size - 0x20);
            if (!isDecrypted)
            {
                EncryptedData = payload;
                DecryptedData = HmacRc4(EncryptedData);
            }
            else
                DecryptedData = payload;

            X360IO io2 = new X360IO(DecryptedData, true);
            Signature = io2.Reader.ReadBytes(0x100);
            io2.Close();
        }
        public override byte[] GetData(bool writeDecrypted)
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, writeDecrypted);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public override byte[] GetData()
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, true);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public override void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io, true);
            return;
        }
        public override void Write()
        {
            Write(Image.IO, false);
        }
        public void Write(X360IO io, bool writeDecrypted)
        {
            Write(io);
            Array.Resize(ref HmacShaNonce, 0x10);
            io.Writer.Write(HmacShaNonce);
            X360IO io2 = new X360IO(DecryptedData, true);
            io2.Writer.Write(Signature);
            DecryptedData = ((MemoryStream)io2.Stream).ToArray();
            io2.Close();
            if (!writeDecrypted)
            {
                EncryptedData = HmacRc4(DecryptedData);
                io.Writer.Write(EncryptedData);
            }
            else
                io.Writer.Write(DecryptedData);
        }
    }

    public class Bootloader4BL : Bootloader
    {
        public byte[] Signature = new byte[0x100]; // 0x100
        public byte[] RsaPublicKey = new byte[0x110]; // 0x110
        public byte[] Nonce6BL = new byte[0x10]; // 0x10
        public byte[] Salt6BL = new byte[0xA]; // 0xA
        public ushort Padding;
        public byte[] Digest5BL = new byte[0x14]; // 0x14
        // CB_B
        public bool UsingCpuKey;
        public Bootloader4BL(NANDImage image, Bootloader prevbl)
            : base(image, prevbl)
        {
            SecuredDataStart = 0x120;
        }
        public override void Read()
        {
            Read(Image.IO, false);
        }
        public void Read(X360IO io, bool isDecrypted)
        {
            Read(io);
            HmacShaNonce = io.Reader.ReadBytes(0x10);
            byte[] payload = io.Reader.ReadBytes((int)Size - 0x20);
            //if(CPUKey != null)
            //{
            //    Array.Resize(ref HmacShaNonce, 0x20);
            //    Array.Copy(CPUKey, 0, HmacShaNonce, 0x10, 0x10);
            //}
            if (!isDecrypted)
            {
                EncryptedData = payload;
                DecryptedData = HmacRc4(EncryptedData);
                if (!((DecryptedData[0x20] == 0 && DecryptedData[0x21] == 0 && DecryptedData[0x22] == 0 &&
                      DecryptedData[0x23] == 0)||(DecryptedData[0x210] == 0 && DecryptedData[0x211] == 0 && DecryptedData[0x212] == 0 && DecryptedData[0x213] == 0)))
                {
                    UsingCpuKey = true;
                    HmacShaNonce = Rc4Key;
                    HmacShaKey = Image.CPUKey;
                    DecryptedData = HmacRc4(EncryptedData);
                }
            }
            else
                DecryptedData = payload;

            X360IO io2 = new X360IO(DecryptedData, true);
            Signature = io2.Reader.ReadBytes(0x100);
            RsaPublicKey = io2.Reader.ReadBytes(0x110);
            Nonce6BL = io2.Reader.ReadBytes(0x10);
            Salt6BL = io2.Reader.ReadBytes(0xA);
            Padding = io2.Reader.ReadUInt16();
            Digest5BL = io2.Reader.ReadBytes(0x14);
            io2.Close();
        }

        public override byte[] GetData(bool writeDecrypted)
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, writeDecrypted);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }

        public override byte[] GetData()
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, true);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public override void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io, true);
            return;
        }
        public override void Write()
        {
            Write(Image.IO, false);
        }
        public void Write(X360IO io, bool writeDecrypted)
        {
            Write(io);
            Array.Resize(ref HmacShaNonce, 0x10);
            io.Writer.Write(HmacShaNonce);
            X360IO io2 = new X360IO(DecryptedData, true);
            io2.Writer.Write(Signature);
            io2.Writer.Write(RsaPublicKey);
            io2.Writer.Write(Nonce6BL);
            io2.Writer.Write(Salt6BL);
            io2.Writer.Write(Padding);
            io2.Writer.Write(Digest5BL);
            DecryptedData = ((MemoryStream)io2.Stream).ToArray();
            io2.Close();
            if (!writeDecrypted)
            {
                EncryptedData = HmacRc4(DecryptedData);
                if (UsingCpuKey)
                {
                    HmacShaNonce = Rc4Key;
                    HmacShaKey = Image.CPUKey;
                    EncryptedData = HmacRc4(DecryptedData);
                }
                io.Writer.Write(EncryptedData);
            }
            else
                io.Writer.Write(DecryptedData);
        }

        public override BootloaderSecurityType VerifyLoader(Bootloader bootloader)
        {
            byte[] bldata = bootloader.GetData();
            int len = bootloader.GetType() == typeof (Bootloader6BL) ? 0x20 : 0x10;
            byte[] headerdata = new byte[len];
            byte[] dgdata = new byte[bldata.Length - bootloader.SecuredDataStart];
            Array.Copy(bldata, headerdata, 0x10);

            Array.Copy(bldata, bootloader.SecuredDataStart, dgdata, 0, dgdata.Length);
            byte[] hash = RotSumSha.XeCryptRotSumSha(headerdata, len, dgdata, dgdata.Length);

            if (bootloader.GetType() == typeof(Bootloader6BL))
            {
                // verify next loaders signature
                byte[] sig = ((Bootloader6BL)bootloader).Signature;

                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.ImportParameters(Misc.GenerateRSAParametersFromPublicKey(RsaPublicKey));
                RSAPKCS1SignatureDeformatter sigDeformatter = new RSAPKCS1SignatureDeformatter(rsa);
                sigDeformatter.SetHashAlgorithm("SHA");
                return sigDeformatter.VerifySignature(hash, sig)
                           ? BootloaderSecurityType.SecureStock
                           : BootloaderSecurityType.Unknown;
                          //: Lists.GlitchableCBs.Contains(Image.GetLastBootloader(this).Build) ? BootloaderSecurityType.SecureGlitch : BootloaderSecurityType.Insecure;
            }
            if (bootloader.GetType() == typeof(Bootloader5BL))
                return hash.SequenceEqual(Digest5BL)
                           ? BootloaderSecurityType.SecureStock
                           : Lists.GlitchableCBs.Contains(Image.GetLastBootloader(this).Build) ? BootloaderSecurityType.SecureGlitch : BootloaderSecurityType.Insecure;
            
            return VerifyLoader(bootloader);
        }
    }

    public class Bootloader5BL : Bootloader
    {
        public ulong ImageAddress;
        public uint ImageSize;
        public uint Padding;

        public Bootloader5BL(NANDImage image, Bootloader prevbl)
            : base(image, prevbl)
        {
            SecuredDataStart = 0x20;
        }
        public override void Read()
        {
            Read(Image.IO, false);
        }
        public void Read(X360IO io, bool isDecrypted)
        {
            Read(io);
            HmacShaNonce = io.Reader.ReadBytes(0x10);
            byte[] payload = io.Reader.ReadBytes((int)Size - 0x20);
            if (!isDecrypted)
            {
                EncryptedData = payload;
                DecryptedData = HmacRc4(EncryptedData);
            }
            else
                DecryptedData = payload;

            X360IO io2 = new X360IO(DecryptedData, true);
            ImageAddress = io2.Reader.ReadUInt64();
            ImageSize = io2.Reader.ReadUInt32();
            Padding = io2.Reader.ReadUInt32();
            io2.Close();
        }
        public override byte[] GetData(bool writeDecrypted)
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, writeDecrypted);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public override byte[] GetData()
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, true);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public override void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io, true);
            return;
        }
        public override void Write()
        {
            Write(Image.IO, false);
        }
        public void Write(X360IO io, bool writeDecrypted)
        {
            Write(io);
            Array.Resize(ref HmacShaNonce, 0x10);
            io.Writer.Write(HmacShaNonce);
            X360IO io2 = new X360IO(DecryptedData, true);
            io2.Writer.Write(ImageAddress);
            io2.Writer.Write(ImageSize);
            io2.Writer.Write(Padding);
            DecryptedData = ((MemoryStream)io2.Stream).ToArray();
            io2.Close();

            //round to next 0x10
            if (Size != Size_r)
            {
                byte[] newdata = new byte[Size_r - 0x20];
                Array.Copy(DecryptedData, newdata, DecryptedData.Length);
                DecryptedData = newdata;
            }

            if (!writeDecrypted)
            {
                EncryptedData = HmacRc4(DecryptedData);
                io.Writer.Write(EncryptedData);
            }
            else
                io.Writer.Write(DecryptedData);
        }

    }
   
    public class Bootloader5BLLZXChunk
    {
        public short CompressedSize;
        public short UncompressedSize;
        public byte[] CompressedData; // CompressedSize
    }
    public class Bootloader6BL : Bootloader
    {
        public uint BuildQfeSource;
        public uint BuildQfeTarget;
        public uint Reserved;
        public uint Size7BL;
        public Bootloader7BLPerBoxData PerBoxData7BL = new Bootloader7BLPerBoxData();
        public Bootloader6BLPerBoxData PerBoxData;
        public byte[] Signature = new byte[0x100]; // 0x100
        public byte[] Nonce7BL = new byte[0x10]; // 0x10
        public byte[] Digest7BL = new byte[0x14]; // 0x14
        public uint Padding;
        public Bootloader6BL(NANDImage image) : base(image, null)
        {
            PerBoxData = new Bootloader6BLPerBoxData(this);
            SecuredDataStart = 0x330; // 0x0 - 0x20 + 0x330 - end
        }

        public override void Read()
        {
            Read(Image.IO, false);
        }
        public void Read(X360IO io, bool isDecrypted)
        {
            Read(io);
            BuildQfeSource = io.Reader.ReadUInt32() >> 16;
            BuildQfeTarget = io.Reader.ReadUInt32() >> 16;
            Reserved = io.Reader.ReadUInt32();
            Size7BL = io.Reader.ReadUInt32();
            HmacShaNonce = io.Reader.ReadBytes(0x10);
            byte[] payload = io.Reader.ReadBytes((int)Size - 0x30);
            if (!isDecrypted)
            {
                EncryptedData = payload;
                DecryptedData = HmacRc4(EncryptedData);
            }
            else
                DecryptedData = payload;

            X360IO io2 = new X360IO(DecryptedData, true);
            PerBoxData7BL = new Bootloader7BLPerBoxData();
            PerBoxData7BL.Read(io2);
            PerBoxData = new Bootloader6BLPerBoxData(this);
            PerBoxData.Read(io2);
            Signature = io2.Reader.ReadBytes(0x100);
            Nonce7BL = io2.Reader.ReadBytes(0x10);
            Digest7BL = io2.Reader.ReadBytes(0x14);
            Padding = io2.Reader.ReadUInt32();
            io2.Close();
        }
        public override byte[] GetData()
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, true);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public override void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io, true);
            return;
        }
        public override void Write()
        {
            Write(Image.IO, false);
        }
        public void Write(X360IO io, bool writeDecrypted)
        {
            Write(io);
            io.Writer.Write(BuildQfeSource << 16);
            io.Writer.Write(BuildQfeTarget << 16);
            io.Writer.Write(Reserved);
            io.Writer.Write(Size7BL);
            Array.Resize(ref HmacShaNonce, 0x10);
            io.Writer.Write(HmacShaNonce);
            X360IO io2 = new X360IO(DecryptedData, true);
            PerBoxData7BL.Write(io2);
            PerBoxData.Write(io2);
            io2.Writer.Write(Signature);
            io2.Writer.Write(Nonce7BL);
            io2.Writer.Write(Digest7BL);
            io2.Writer.Write(Padding);
            DecryptedData = ((MemoryStream) io2.Stream).ToArray();
            io2.Close();
            if (!writeDecrypted)
            {
                EncryptedData = HmacRc4(DecryptedData);
                io.Writer.Write(EncryptedData);
            }
            else
                io.Writer.Write(DecryptedData);
        }

        public override BootloaderSecurityType VerifyLoader(Bootloader bootloader)
        {
            byte[] bldata = bootloader.GetData();
            byte[] headerdata = new byte[0x10];
            byte[] dgdata = new byte[bldata.Length - bootloader.SecuredDataStart];
            Array.Copy(bldata, headerdata, 0x10);

            Array.Copy(bldata, bootloader.SecuredDataStart, dgdata, 0, dgdata.Length);
            byte[] hash = RotSumSha.XeCryptRotSumSha(headerdata, 0x10, dgdata, dgdata.Length);

            if (bootloader.GetType() == typeof(Bootloader7BL))
                return hash.SequenceEqual(Digest7BL) ? BootloaderSecurityType.SecureStock : Lists.GlitchableCBs.Contains(Image.GetLastBootloader(Image.GetLastBootloader(this)).Build) ? BootloaderSecurityType.SecureGlitch : BootloaderSecurityType.SecureStock;

            return VerifyLoader(bootloader);
        }

    }
    public class Bootloader7BLPerBoxData
    {
        public ushort UsedBlocksCount;
        public ushort[] BlockNumbers = new ushort[223];

        public void Read(X360IO io)
        {
            UsedBlocksCount = io.Reader.ReadUInt16();
            for(ushort i = 0; i < 223; i++)
                BlockNumbers[i] = io.Reader.ReadUInt16();
        }
        public void Write(X360IO io)
        {
            io.Writer.Write(UsedBlocksCount);
            for(ushort i = 0; i < 223; i++)
                io.Writer.Write(BlockNumbers[i]);
        }
    }
    public class Bootloader6BLPerBoxData
    {
        public Bootloader6BL Bootloader;
        public byte[] Reserved = new byte[0x2B];
        public byte UpdateSlotNumber;
        public byte[] PairingData = new byte[3];
        public byte LockDownValue;
        public byte[] PerBoxDigest = new byte[0x10];
        public Bootloader6BLPerBoxData(Bootloader6BL bootloader)
        {
            Bootloader = bootloader;
        }
        public byte[] CalculateDigest()
        {
            byte[] data = Bootloader.GetData();
            Bootloader.Rc4Key = new HMACSHA1(Bootloader.HmacShaKey).ComputeHash(data, 0x20, 0x10);
            for (int i = 0; i < 0x10; i++)
                data[0x20 + i] = Bootloader.Rc4Key[i];
            data = new HMACSHA1(Bootloader.Image.CPUKey).ComputeHash(data, 0, 0x220);
            Array.Resize(ref data, 0x10);
            return data;
        }
        public void Read()
        {
            Read(Bootloader.Image.IO);
        }
        public void Write()
        {
            Write(Bootloader.Image.IO);
        }
        public void Read(X360IO io)
        {
            Reserved = io.Reader.ReadBytes(0x2B);
            UpdateSlotNumber = io.Reader.ReadByte();
            PairingData = io.Reader.ReadBytes(3);
            LockDownValue = io.Reader.ReadByte();
            PerBoxDigest = io.Reader.ReadBytes(0x10);
        }
        public void Write(X360IO io)
        {
            io.Writer.Write(Reserved);
            io.Writer.Write(UpdateSlotNumber);
            io.Writer.Write(PairingData);
            io.Writer.Write(LockDownValue);
            io.Writer.Write(PerBoxDigest);
        }
    }
    public class Bootloader7BL : Bootloader
    {
        public uint SourceImageSize;
        public byte[] SourceDigest = new byte[0x14]; // 0x14
        public uint TargetImageSize;
        public byte[] TargetDigest = new byte[0x14]; // 0x14
        public int Length6BL;
        public Bootloader7BL(NANDImage image, Bootloader prevbl) : base(image, prevbl)
        {
            if (PreviousLoader != null)
            {
                Length6BL = (int) PreviousLoader.Size;
                HmacShaKey = ((Bootloader6BL) PreviousLoader).Nonce7BL;
            }
            SecuredDataStart = 0x20;
        }
        public override void Read()
        {
            Read(Image.IO, false);
        }

        public static int LzxChunkLength = 0x4000;

        public void Read(X360IO io, bool isDecrypted)
        {
            Read(io);
            HmacShaNonce = io.Reader.ReadBytes(0x10);

            byte[] payload = new byte[Size - 0x20];
            if(io.Stream.GetType() == typeof(NANDImageStream))
            {
                int headerlen = 0x10000 - Length6BL - 0x20;
                io.Stream.Read(payload, 0, headerlen);
                int misc = (int)(Size - headerlen - 0x20) % LzxChunkLength;
                for(int i = 0; i < ((Bootloader6BL)PreviousLoader).PerBoxData7BL.UsedBlocksCount; i++)
                {

                    ((NANDImageStream) io.Stream).Seek(LzxChunkLength*((Bootloader6BL)PreviousLoader).PerBoxData7BL.BlockNumbers[i] , SeekOrigin.Begin);

                    int offset = headerlen + (i * LzxChunkLength);
                    io.Stream.Read(payload, offset,
                        ((Bootloader6BL)PreviousLoader).PerBoxData7BL.UsedBlocksCount - 1 == i ? misc : LzxChunkLength);
                }
            }
            else
            {
                payload = io.Reader.ReadBytes((int) io.Stream.Length - (int) io.Stream.Position);
            }

            if (!isDecrypted)
            {
                EncryptedData = payload;
                DecryptedData = HmacRc4(EncryptedData);
            }
            else
                DecryptedData = payload;

            

            X360IO io2 = new X360IO(DecryptedData, true);
            SourceImageSize = io2.Reader.ReadUInt32();
            SourceDigest = io2.Reader.ReadBytes(0x14);
            TargetImageSize = io2.Reader.ReadUInt32();
            TargetDigest = io2.Reader.ReadBytes(0x14);
            io2.Close();

        }

        public override byte[] GetData()
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, true);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();

            return bldata;
        }
        public override void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io, true);
            return;
        }
        public override void Write()
        {
            Write(Image.IO, false);
        }
        public void Write(X360IO io, bool writeDecrypted)
        {
            Write(io);
            Array.Resize(ref HmacShaNonce, 0x10);
            io.Writer.Write(HmacShaNonce);

            X360IO io2 = new X360IO(DecryptedData, true);
            io2.Writer.Write(SourceImageSize);
            io2.Writer.Write(SourceDigest);
            io2.Writer.Write(TargetImageSize);
            io2.Writer.Write(TargetDigest);
            DecryptedData = ((MemoryStream) io2.Stream).ToArray();

            io2.Close();

            //round to next 0x10
            if (Size != Size_r)
            {
                byte[] newdata = new byte[Size_r - 0x20];
                Array.Copy(DecryptedData, newdata, DecryptedData.Length);
                DecryptedData = newdata;
            }

            if (!writeDecrypted)
            {
                EncryptedData = HmacRc4(DecryptedData);
                io.Writer.Write(EncryptedData, 0, DecryptedData.Length);
            }
            else
            {
                io.Writer.Write(DecryptedData);
            }
        }
    }
}
