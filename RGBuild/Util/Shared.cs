using System;
using System.Globalization;
using System.Security.Cryptography;
using RGBuild.NAND;

namespace RGBuild.Util
{
    class Shared
    {
        public static byte[] HexStringToBytes(string hexString)
        {
            if (hexString == null)
            {
                throw new ArgumentNullException("hexString");
            }

            if ((hexString.Length & 1) != 0)
            {
                throw new ArgumentOutOfRangeException("hexString", hexString, "hexString must contain an even number of characters.");
            }

            byte[] result = new byte[hexString.Length / 2];

            for (int i = 0; i < hexString.Length; i += 2)
            {
                result[i / 2] = byte.Parse(hexString.Substring(i, 2), NumberStyles.HexNumber);
            }

            return result;
        }
        public static string BytesToHexString(byte[] data, string seperator)
        {
            string str = data[0].ToString("X2");
            for (int i = 1; i < data.Length; i++)
                str += seperator + data[i].ToString("X2");
            return str;
        }
        public static Tuple<byte[], byte[]> HmacRc4(byte[] hmackey, byte[] hmacnonce, byte[] fdata)
        {
            byte[] rc4Key = new HMACSHA1(hmackey).ComputeHash(hmacnonce, 0, hmacnonce.Length);

            Array.Resize(ref rc4Key, 0x10);
            RC4Session session = AccountRC4.RC4CreateSession(rc4Key);
            byte[] data = new byte[fdata.Length];

            Array.Copy(fdata, 0, data, 0, fdata.Length);
            AccountRC4.RC4Encrypt(ref session, data, 0, data.Length);

            return new Tuple<byte[], byte[]>(data, rc4Key);
        }
    }
}
