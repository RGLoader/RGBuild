using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using RGBuild.IO;
using RGBuild.Util;

namespace RGBuild.NAND
{
    public class ExtendedKeyVault : XeKeysSecuredFile
    {
        public ExtendedKeyVault(byte[] cpuKey)
            : base(cpuKey)
        {

        }

        public void Read(X360IO io, int size, bool isDecrypted)
        {
            base.Read(io);
            byte[] payload = io.Reader.ReadBytes(size - 0x10);
            if (!isDecrypted)
            {
                EncryptedData = payload;
                DecryptedData = HmacRc4(EncryptedData);
            }
            else
                DecryptedData = payload;
            X360IO io2 = new X360IO(base.DecryptedData);
            io2.Close();
        }
        public virtual byte[] GetData()
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, true);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public virtual void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io, data.Length, true);
            return;
        }
        public void Write(X360IO io, bool writeDecrypted)
        {
            HmacShaNonce = new HMACSHA1(CpuKey).ComputeHash(DecryptedData);
            Array.Resize(ref HmacShaNonce, 0x10);
            base.Write(io);

            if (!writeDecrypted)
            {
                EncryptedData = HmacRc4(DecryptedData);
                io.Writer.Write(EncryptedData);
            }
            else
                io.Writer.Write(DecryptedData);
        }
        public byte[] GetKey(Lists.XeKey key)
        {
            /*Lists.XeKeyBinding bind = Lists.XeKeysEx[key];
            X360IO io2 = new X360IO(DecryptedData) { Stream = { Position = bind.Address } };
            byte[] data = io2.Reader.ReadBytes(bind.Length);
            io2.Close();
            return data;*/
            return null;
        }
        public void SetKey(Lists.XeKey key, byte[] data)
        {
            /*Lists.XeKeyBinding bind = Lists.XeKeysEx[key];
            if (data.Length != bind.Length)
                return;
            X360IO io2 = new X360IO(DecryptedData) { Stream = { Position = bind.Address } };
            io2.Writer.Write(data);
            DecryptedData = ((MemoryStream) io2.Stream).ToArray();
            io2.Close();*/
        }
    }
    public class SecurityDataFile : XeKeysSecuredFile
    {
        public byte[] PairingData;
        public byte[] Unknown; // 0x5
        public byte SecurityInitialised;
        public byte LockDownValue;
        public ulong FileTimestamp; // ?
        public byte DetectedViolations;
        public ulong SecurityActivated;
        public ulong DvdNotConnectedCount;
        public ulong LockSystemUpdateCount;

        public SecurityDataFile(byte[] cpuKey)
            : base(cpuKey)
        {

        }

        public void Read(X360IO io, int size, bool isDecrypted)
        {
            base.Read(io);
            byte[] payload = io.Reader.ReadBytes(size - 0x10);
            if (!isDecrypted)
            {
                EncryptedData = payload;
                DecryptedData = HmacRc4(EncryptedData);
            }
            else
                DecryptedData = payload;
            X360IO io2 = new X360IO(base.DecryptedData);
            // read our stuff here

            PairingData = io2.Reader.ReadBytes(3);
            Unknown = io2.Reader.ReadBytes(5);
            SecurityInitialised = io2.Reader.ReadByte();
            LockDownValue = io2.Reader.ReadByte();
            FileTimestamp = io2.Reader.ReadUInt64();
            DetectedViolations = io2.Reader.ReadByte();
            SecurityActivated = io2.Reader.ReadUInt64();
            DvdNotConnectedCount = io2.Reader.ReadUInt64();
            LockSystemUpdateCount = io2.Reader.ReadUInt64();
            io2.Close();
        }
        public virtual byte[] GetData()
        {
            X360IO io = new X360IO(new MemoryStream(), true);
            Write(io, true);
            byte[] bldata = ((MemoryStream)io.Stream).ToArray();
            io.Close();
            return bldata;
        }
        public virtual void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io, data.Length, true);
            return;
        }
        public void Write(X360IO io, bool writeDecrypted)
        {
            X360IO io2 = new X360IO(DecryptedData, true);
            // write our stuff here

            io2.Writer.Write(PairingData);
            io2.Writer.Write(Unknown);
            io2.Writer.Write(SecurityInitialised);
            io2.Writer.Write(LockDownValue);
            io2.Writer.Write(FileTimestamp);
            io2.Writer.Write(DetectedViolations);
            io2.Writer.Write(SecurityActivated);
            io2.Writer.Write(DvdNotConnectedCount);
            io2.Writer.Write(LockSystemUpdateCount);
            DecryptedData = ((MemoryStream)io2.Stream).ToArray();
            io2.Close();

            HmacShaNonce = new HMACSHA1(CpuKey).ComputeHash(DecryptedData);
            Array.Resize(ref HmacShaNonce, 0x10);
            base.Write(io);

            if (!writeDecrypted)
            {
                EncryptedData = HmacRc4(DecryptedData);
                io.Writer.Write(EncryptedData);
            }
            else
                io.Writer.Write(DecryptedData);
        }
    }
    public class XeKeysSecuredFile
    {
        public byte[] CpuKey;

        public byte[] HmacShaNonce;
        public byte[] Rc4Key;

        public byte[] EncryptedData;
        public byte[] DecryptedData;
        public XeKeysSecuredFile(byte[] cpuKey)
        {
            CpuKey = cpuKey;
        }
        public virtual void Read(X360IO io)
        {
            HmacShaNonce = io.Reader.ReadBytes(0x10);
        }
        public virtual void Write(X360IO io)
        {
            io.Writer.Write(HmacShaNonce);
        }

        public byte[] HmacRc4(byte[] data)
        {
            Tuple<byte[], byte[]> enc = Shared.HmacRc4(data, CpuKey, HmacShaNonce);
            Rc4Key = enc.Item2;
            return enc.Item1;
        }

    }
}
