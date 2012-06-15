using System;
using System.Security.Cryptography;

namespace RGBuild.ThirdParty
{
    public static class Misc
    {
        internal static byte[] GetData(byte[] data, int pos, int length)
        {
            // Create a new output buffer
            byte[] outBuffer = new byte[length];

            // Loop and copy the data
            for (int x = pos, y = 0; x < pos + length; x++, y++)
                outBuffer[y] = data[x];

            // Return our data
            return outBuffer;
        }
        internal static byte[] Swap8(byte[] input)
        {
            byte[] buffer = new byte[input.Length];
            Array.Copy(input, buffer, input.Length);
            for (int x = 0; x < input.Length; x += 8)
                Array.Reverse(buffer, x, 8);

            return Swap(buffer);
        }
        internal static byte[] Swap(byte[] input)
        {
            byte[] buffer = new byte[input.Length];
            Array.Copy(input, buffer, input.Length);
            Array.Reverse(buffer);
            return buffer;
        }

        internal static byte[] SetInt32(int value)
        {
            return SetInt32(value, true);
        }
        internal static byte[] SetInt32(int value, bool bigEndian)
        {
            if (bigEndian)
                return new[] {
                    (byte)((value >> 24) & 0xFF), 
                    (byte)((value >> 16) & 0xFF), 
                    (byte)((value >> 8) & 0xFF),
                    (byte)(value & 0xFF)};

            return new[] { 
                (byte)(value  & 0xFF), 
                (byte)((value >> 8) & 0xFF), 
                (byte)((value >> 16) & 0xFF),
                (byte)((value >> 24) & 0xFF)};
        }
        internal static int GetInt32(byte[] data)
        {
            return GetInt32(data, true);
        }
        internal static int GetInt32(byte[] data, bool bigEndian)
        {
            if (bigEndian)
                return (data[0] << 24) | (data[1] << 16) |
                     (data[2] << 8) | data[3];

            return (data[3] << 24) | (data[2] << 16) |
               (data[1] << 8) | data[0];
        }
        internal static RSAParameters GenerateRSAParametersFromPublicKey(byte[] exponent, byte[] modulus)
        {
            return new RSAParameters
            {
                Exponent = exponent,
                Modulus = Swap8(modulus)
            };
        }
        internal static RSAParameters GenerateRSAParametersFromPublicKey(byte[] publicKey)
        {
            //return GenerateRSAParametersFromPublicKey(publicKey, false);
            return new RSAParameters
            {
                Exponent = GetData(publicKey, 0x04, 0x04),
                Modulus = Swap8(GetData(publicKey, 0x10, 0x100))
            };
        }
        internal static RSAParameters GenerateRSAParametersFromPrivateKey(byte[] privateKey)
        {
            // Get our keysize
            int keyLen = GetInt32(privateKey) / 16;

            // Create our params
            RSAParameters rsaParams = new RSAParameters();
            int pos = 4;
            rsaParams.Exponent = GetData(privateKey, pos, 0x04); pos += 0xC;
            rsaParams.Modulus = Swap8(GetData(privateKey, pos, 0x80 * keyLen)); pos += (0x80 * keyLen);
            rsaParams.P = Swap8(GetData(privateKey, pos, 0x40 * keyLen)); pos += (0x40 * keyLen);
            rsaParams.Q = Swap8(GetData(privateKey, pos, 0x40 * keyLen)); pos += (0x40 * keyLen);
            rsaParams.DP = Swap8(GetData(privateKey, pos, 0x40 * keyLen)); pos += (0x40 * keyLen);
            rsaParams.DQ = Swap8(GetData(privateKey, pos, 0x40 * keyLen)); pos += (0x40 * keyLen);
            rsaParams.InverseQ = Swap8(GetData(privateKey, pos, 0x40 * keyLen));
            rsaParams.D = new byte[0x80];

            // Return our params
            return rsaParams;
        }
        
    }
}
