using System;
using System.IO;
using System.Security.Cryptography;
using RGBuild.IO;
using RGBuild.Util;

namespace RGBuild.NAND
{
    public class XeConsoleCertificate
    {
        private KeyVault keyVault;

        public ushort CertSize;
        public byte[] ConsoleId = new byte[0x5]; // 0x5
        public byte[] ConsolePartNumber = new byte[0xb]; // 0xB
        public uint Reserved;
        public ushort Privileges;
        public uint ConsoleType;
        public string ManufacturingDate = "01-12-08"; // 0x8
        public byte[] ConsolePublicExponent = new byte[0x4]; // 0x4
        public byte[] ConsolePublicModulus = new byte[0x80]; // 0x80
        public byte[] Signature = new byte[0x100]; // 0x100
        public void Read(KeyVault kv)
        {
            keyVault = kv;
            Read();
        }
        public void Read()
        {
            X360IO io = new X360IO(keyVault.GetKey(Lists.XeKey.CONSOLE_CERTIFICATE), true);
            CertSize = io.Reader.ReadUInt16();
            if (CertSize != 0x1A8)
                return;
            ConsoleId = io.Reader.ReadBytes(0x5);
            ConsolePartNumber = io.Reader.ReadBytes(0xB);
            Reserved = io.Reader.ReadUInt32();
            Privileges = io.Reader.ReadUInt16();
            ConsoleType = io.Reader.ReadUInt32();

            ManufacturingDate = System.Text.ASCIIEncoding.ASCII.GetString(io.Reader.ReadBytes(0x8));
            ConsolePublicExponent = io.Reader.ReadBytes(4);
            ConsolePublicModulus = io.Reader.ReadBytes(0x80);
            Signature = io.Reader.ReadBytes(0x100);
            io.Close();
        }
        public void Write()
        {
            X360IO io = new X360IO(keyVault.GetKey(Lists.XeKey.CONSOLE_CERTIFICATE), true);
            io.Writer.Write(CertSize);
            io.Writer.Write(ConsoleId);
            io.Writer.Write(ConsolePartNumber);
            io.Writer.Write(Reserved);
            io.Writer.Write(Privileges);
            io.Writer.Write(ConsoleType);
            io.Writer.WriteAsciiString(ManufacturingDate, 0x8);
            io.Writer.Write(ConsolePublicExponent);
            io.Writer.Write(ConsolePublicModulus);
            io.Writer.Write(Signature);
            keyVault.SetKey(Lists.XeKey.CONSOLE_CERTIFICATE, ((MemoryStream)io.Stream).ToArray());
        }
    }


    public class XeikaCertificate
    {
        private KeyVault keyVault;

        public byte[] XeikaPublicKey = new byte[0x110]; // 0x110
        public byte[] OverlaySignature = new byte[0x4]; // 0x4
        public ushort OverlayVersion; //0x2
        public byte ODDDataVersion; //0x1
        public byte DrivePhaseLevel; //0x1
        public byte[] DriveInquiry = new byte[0x28]; //0x28  (OSIG)
        public byte[] Reserved = new byte[0x1146]; //0x1146 

        public string OSIGdesc;

        public void Read(KeyVault kv)
        {
            keyVault = kv;
            Read();
        }
        public void Read()
        {
            X360IO io = new X360IO(keyVault.GetKey(Lists.XeKey.XEIKA_CERTIFICATE), true);
            
            XeikaPublicKey = io.Reader.ReadBytes(0x110);
            OverlaySignature = io.Reader.ReadBytes(0x4);
            OverlayVersion = io.Reader.ReadUInt16();
            ODDDataVersion = io.Reader.ReadByte();
            DrivePhaseLevel = io.Reader.ReadByte();
            DriveInquiry = io.Reader.ReadBytes(0x28);
            Reserved = io.Reader.ReadBytes(0x1146);
            io.Close();

            loadOSIG();
            
        }

        public void loadOSIG()
        {
           OSIGdesc = System.Text.ASCIIEncoding.ASCII.GetString(DriveInquiry, 0x8, 0x28 - 0x8);

        }

        public void Write()
        {
            X360IO io = new X360IO(keyVault.GetKey(Lists.XeKey.XEIKA_CERTIFICATE), true);
            io.Writer.Write(XeikaPublicKey);
            io.Writer.Write(OverlaySignature);
            io.Writer.Write(OverlayVersion);
            io.Writer.Write(ODDDataVersion);
            io.Writer.Write(DrivePhaseLevel);
            io.Writer.Write(DriveInquiry);
            io.Writer.Write(Reserved);
            keyVault.SetKey(Lists.XeKey.XEIKA_CERTIFICATE, ((MemoryStream)io.Stream).ToArray());
        }
    }


    public class KeyVault
    {
        public X360IO IO;
        public byte[] EncryptedData;
        public byte[] DecryptedData;
        public byte[] Nonce;
        public byte[] CPUKey;
        public byte[] Rc4Key;

        public byte[] GameRegion
        {
            get
            {
                return GetKey(Lists.XeKey.GAME_REGION);
            }
            set
            {
                SetKey(Lists.XeKey.GAME_REGION, value);
            }
        }
        public byte[] DVDKey
        {
            get
            {
                return GetKey(Lists.XeKey.DVD_KEY);
            }
            set
            {
                SetKey(Lists.XeKey.DVD_KEY, value);
            }
        }
        public byte[] ConsoleSerial
        {
            get
            {
                return GetKey(Lists.XeKey.CONSOLE_SERIAL_NUMBER);
            }
            set
            {
                SetKey(Lists.XeKey.CONSOLE_SERIAL_NUMBER, value);
            }
        }
        public string OSIG;


        public XeikaCertificate XeikaCertificate;
        public XeConsoleCertificate ConsoleCertificate;
        public byte[] Signature
        {
            get
            {
                return GetKey(Lists.XeKey.SPECIAL_KEY_VAULT_SIGNATURE);
            }
            set
            {
                SetKey(Lists.XeKey.SPECIAL_KEY_VAULT_SIGNATURE, value);
            }
        }
        public bool IsValid
        {
            get
            {
                byte[] data = GetKey(Lists.XeKey.CARDEA_CERTIFICATE);
                return data[0x6c] == 0x44 && data[0x6d] == 0x52 && data[0x6e] == 0x4d; // check for "DRM" @ 0x6C in cardae cert, 0x1f74 in kv
            }
        }
        public bool IsNonceValid
        {
            get
            {
                return Shared.BytesToHexString(Nonce, "") == Shared.BytesToHexString(CalculateKeyVaultNonce(), "");
            }
        }
        public int Version
        {
            get
            {
                byte[] data = GetKey(Lists.XeKey.XEIKA_CERTIFICATE);
                return data[0x110] == 0x4F && data[0x111] == 0x53 && data[0x112] == 0x49 && data[0x113] == 0x47 ? 2 : 1; // check for "OSIG" @ 0x110 in xeika cert, 0xb72 in kv
            }
        }
        public KeyVault(X360IO io, byte[] cpuKey)
        {
            IO = io;
            CPUKey = cpuKey;
        }
        public void Read(uint kvLength)
        {
            Read(IO, kvLength, false);
        }
        public void Read(X360IO io, uint kvLength, bool isDecrypted)
        {
            Nonce = io.Reader.ReadBytes(0x10);
            byte[] payload = io.Reader.ReadBytes((int)kvLength - 0x10);
            if (!isDecrypted)
            {
                EncryptedData = payload;
                HmacRc4Decrypt();
            }
            else
                DecryptedData = payload;

            ConsoleCertificate = new XeConsoleCertificate();
            ConsoleCertificate.Read(this);
            XeikaCertificate = new XeikaCertificate();
            XeikaCertificate.Read(this);
        }

        


        public byte[] GenerateSignature()
        {
            byte[] digesthash = new byte[0x3ff0 - 0xc - 0x10 - 0x100];
            // random salt / mfg flags aren't hashed (0xC)
            Array.Copy(DecryptedData, 0xC, digesthash, 0, 0xD4);
            // random key placed by HV aren't hashed (0x1)
            Array.Copy(DecryptedData, 0xF0, digesthash, 0xD4, 0x1CF8);
            // special_keyvault_signature isn't hashed (0x100)
            Array.Copy(DecryptedData, 0x1ee8, digesthash, 0x1dcc, 0x2108);

            digesthash = new HMACSHA1(CPUKey).ComputeHash(digesthash);
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                //rsa.ImportParameters(PrivateKey);

                // Create a signature formatter
                //RSAPKCS1SignatureFormatter sigFormatter = new RSAPKCS1SignatureFormatter(rsa);
                //sigFormatter.SetHashAlgorithm("SHA1");
                //byte[] sig = 
                //Array.Resize(ref digesthash, 0x10);

                return rsa.Encrypt(digesthash, false);
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
        public void SetData(byte[] data)
        {
            X360IO io = new X360IO(data, true);
            Read(io, (uint)data.Length, true);
            return;
        }
        public void Write()
        {
            Write(IO, false);
        }
        public byte[] CalculateKeyVaultNonce()
        {
            byte[] data = new byte[0x3ff2];
            Array.Copy(DecryptedData, 0, data, 0, 0x3ff0);
            data[0x3ff0] = 0x7;
            data[0x3ff1] = 0x12;
            byte[] nonce = new HMACSHA1(CPUKey).ComputeHash(data);
            Array.Resize(ref nonce, 0x10);
            return nonce;
        }
        public void Write(X360IO io, bool writeDecrypted)
        {
            if (XeikaCertificate != null)
                XeikaCertificate.Write();
            if (ConsoleCertificate != null)
                ConsoleCertificate.Write();

            // recalc the nonce
            Nonce = CalculateKeyVaultNonce();
            io.Writer.Write(Nonce);
            if (!writeDecrypted)
            {
                HmacRc4Encrypt();
                io.Writer.Write(EncryptedData);
            }
            else
                io.Writer.Write(DecryptedData);
        }

        public void HmacRc4Decrypt()
        {
            Rc4Key = new HMACSHA1(CPUKey).ComputeHash(Nonce);
            Array.Resize(ref Rc4Key, 0x10);
            RC4Session session = AccountRC4.RC4CreateSession(Rc4Key);
            byte[] data = new byte[EncryptedData.Length];
            Array.Copy(EncryptedData, 0, data, 0, EncryptedData.Length);
            //DataHeader = Shared.CopyArrayPart(data, 0, headerLength + 0x10);
            AccountRC4.RC4Encrypt(ref session, data, 0, data.Length);
            DecryptedData = data;//data2, headerLength + 0x10, data2.Length - (headerLength + 0x10));
        }

        public void HmacRc4Encrypt()
        {
            Rc4Key = new HMACSHA1(CPUKey).ComputeHash(Nonce);
            Array.Resize(ref Rc4Key, 0x10);
            RC4Session session = AccountRC4.RC4CreateSession(Rc4Key);
            byte[] data = new byte[DecryptedData.Length];
            Array.Copy(DecryptedData, 0, data, 0, DecryptedData.Length);
            //DataHeader = Shared.CopyArrayPart(data, 0, headerLength + 0x10);
            AccountRC4.RC4Encrypt(ref session, data, 0, data.Length);
            EncryptedData = data;//data2, headerLength + 0x10, data2.Length - (headerLength + 0x10));
        }

        public byte[] GetKey(Lists.XeKey key)
        {
            Lists.XeKeyBinding bind = Lists.XeKeys[key];
            X360IO io2 = new X360IO(DecryptedData) {Stream = {Position = bind.Address}};
            byte[] data = io2.Reader.ReadBytes(bind.Length);
            io2.Close();
            return data;
        }

        public void SetKey(Lists.XeKey key, byte[] data)
        {
            Lists.XeKeyBinding bind = Lists.XeKeys[key];
            
            X360IO io2 = new X360IO(DecryptedData) { Stream = { Position = bind.Address } };
            //X360IO io2 = new X360IO(DecryptedData) { Stream = { Position = 0 } };

            Array.Resize<byte>(ref data, bind.Length);
            io2.Writer.Write(data);
            DecryptedData = ((MemoryStream)io2.Stream).ToArray();
            io2.Close();

            return;
        }
    }
}
