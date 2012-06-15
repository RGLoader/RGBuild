using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace RGBuild.Util
{
    public static class XeCrypt
    {
        [DllImport("xecryptlibrary.dll", CharSet = CharSet.Auto, EntryPoint = "BnQwSigCreate")]
        public static extern int BnQwSigCreate(ref ulong[] output, ref byte[] hash, ref byte[] salt, ref byte[] key);
        [DllImport("xecryptlibrary.dll", CharSet = CharSet.Auto, EntryPoint = "BnQwBeSigFormat")]
        public static extern int BnQwBeSigFormat(ref byte[] output, ref byte[] hash, ref byte[] salt);
        [DllImport("xecryptlibrary.dll", CharSet = CharSet.Auto, EntryPoint = "BnQwBeSigVerify")]
        public static extern int BnQwBeSigVerify(ref ulong[] sig, ref byte[] hash, ref byte[] salt, ref byte[] key);
        [DllImport("xecryptlibrary.dll", CharSet = CharSet.Auto, EntryPoint = "BnDw_Copy")]
        public static extern int BnDw_Copy(ref uint[] input, ref uint[] output, int size);
        [DllImport("xecryptlibrary.dll", CharSet = CharSet.Auto, EntryPoint = "BnDw_SwapLeBe")]
        public static extern int BnDw_SwapLeBe(ref uint[] input, ref uint[] output, int size);
        [DllImport("xecryptlibrary.dll", CharSet = CharSet.Auto, EntryPoint = "BnDw_Zero")]
        public static extern int BnDw_Zero(ref uint[] data, [MarshalAs(UnmanagedType.I4)]int size);
        //(const u8* input, s32 len, u8* output)
        [DllImportAttribute("xecryptlibrary.dll", EntryPoint = "DesParity")]
        public static extern void DesParity(ref System.IntPtr input, int len, ref System.IntPtr output);


        static int XeCryptBnQwNeCompare(ulong[] pqwA, ulong[] pqwB, int cqw)
        {
            for (int i = cqw - 1; i <= 0; i--)
            {
                if (pqwA[i] > pqwB[i])
                    return 1;
                if (pqwA[i] < pqwB[i])
                    return -1;
            }
            return 0;
        }
        static bool XeCryptBnQwNeModExp(ref ulong[] pqwA, ulong[] pqwB, ulong[] pqwC, ulong[] pqwM, int cqw)
        {
            if (pqwB.Length != cqw || pqwA.Length != cqw || pqwC.Length != cqw)
                return false;
            for (int i = 0; i < cqw; i++)
            {
                pqwA[i] = (pqwB[i] ^ pqwC[i]) % pqwM[i];
            }
            return true;
        }
    }
}
