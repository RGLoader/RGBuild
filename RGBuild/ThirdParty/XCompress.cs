using System;
using System.IO;
using System.Runtime.InteropServices;
using RGBuild.IO;
using RGBuild.NAND;

namespace RGBuild.Util
{
    public static class XCompress
    {
        private static readonly bool IsMachine64Bit = (IntPtr.Size == 8);

        public static int LDICreateDecompression(ref int pcbDataBlockMax, ref LzxDecompress pvConfiguration, int pfnma, int pfnmf, IntPtr pcbSrcBufferMin, ref int unknown, ref int ldiContext)
        {
            if (Is64Bit)
            {
                return LDICreateDecompression64(ref pcbDataBlockMax, ref pvConfiguration, pfnma, pfnmf, pcbSrcBufferMin, ref unknown, ref ldiContext);
            }
            return LDICreateDecompression32(ref pcbDataBlockMax, ref pvConfiguration, pfnma, pfnmf, pcbSrcBufferMin, ref unknown, ref ldiContext);
        }

        [DllImport("xcompress32.dll", EntryPoint = "LDICreateDecompression")]
        private static extern int LDICreateDecompression32(ref int pcbDataBlockMax, ref LzxDecompress pvConfiguration, int pfnma, int pfnmf, IntPtr pcbSrcBufferMin, ref int unknown, ref int ldiContext);
        [DllImport("xcompress64.dll", EntryPoint = "LDICreateDecompression")]
        private static extern int LDICreateDecompression64(ref int pcbDataBlockMax, ref LzxDecompress pvConfiguration, int pfnma, int pfnmf, IntPtr pcbSrcBufferMin, ref int unknown, ref int ldiContext);
        public static int LDIDecompress(int context, byte[] pbSrc, int cbSrc, byte[] pbDst, ref int pcbDecompressed)
        {
            if (Is64Bit)
            {
                return LDIDecompress64(context, pbSrc, cbSrc, pbDst, ref pcbDecompressed);
            }
            return LDIDecompress32(context, pbSrc, cbSrc, pbDst, ref pcbDecompressed);
        }

        [DllImport("xcompress32.dll", EntryPoint = "LDIDecompress")]
        private static extern int LDIDecompress32(int context, byte[] pbSrc, int cbSrc, byte[] pbDst, ref int pcbDecompressed);
        [DllImport("xcompress64.dll", EntryPoint = "LDIDecompress")]
        private static extern int LDIDecompress64(int context, byte[] pbSrc, int cbSrc, byte[] pbDst, ref int pcbDecompressed);
        public static int LDIDestroyDecompression(int context)
        {
            if (Is64Bit)
            {
                return LDIDestroyDecompression64(context);
            }
            return LDIDestroyDecompression32(context);
        }

        [DllImport("xcompress32.dll", EntryPoint = "LDIDestroyDecompression")]
        private static extern int LDIDestroyDecompression32(int context);
        [DllImport("xcompress64.dll", EntryPoint = "LDIDestroyDecompression")]
        private static extern int LDIDestroyDecompression64(int context);
        public static int LDIResetDecompression(int context)
        {
            if (Is64Bit)
            {
                return LDIResetDecompression64(context);
            }
            return LDIResetDecompression32(context);
        }

        [DllImport("xcompress32.dll", EntryPoint = "LDIResetDecompression")]
        private static extern int LDIResetDecompression32(int context);
        [DllImport("xcompress64.dll", EntryPoint = "LDIResetDecompression")]
        private static extern int LDIResetDecompression64(int context);
        public static int LDISetWindowData(int context, byte[] window, int size)
        {
            if (Is64Bit)
            {
                return LDISetWindowData64(context, window, size);
            }
            return LDISetWindowData32(context, window, size);
        }

        [DllImport("xcompress32.dll", EntryPoint = "LDISetWindowData")]
        private static extern int LDISetWindowData32(int context, byte[] window, int size);
        [DllImport("xcompress64.dll", EntryPoint = "LDISetWindowData")]
        private static extern int LDISetWindowData64(int context, byte[] window, int size);

        private static bool Is64Bit
        {
            get
            {
                return IsMachine64Bit;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct LzxDecompress
        {
            public long WindowSize;
            public long CpuType;
        }
        public static byte[] DecompressPatchInChunks(byte[] origdata, byte[] bl7data, uint targetsize)
        {
            X360IO io = new X360IO(bl7data, true) { Stream = { Position = 0x50 } };
            X360IO patchedkernel = new X360IO(new MemoryStream(), true);
            patchedkernel.Stream.SetLength(targetsize);
            patchedkernel.Writer.Write(origdata);
            int num = 32768;
            int ldicontext = -1;
            int num3 = 0;
            LzxDecompress lzxDecompress;
            lzxDecompress.CpuType = 1L;
            lzxDecompress.WindowSize = num;
            IntPtr intPtr = Marshal.AllocHGlobal(176640);
            if (LDICreateDecompression(ref num, ref lzxDecompress, 0, 0, intPtr, ref num3, ref ldicontext) != 0)
            {
                throw new Exception("Failed to create patch decompression.");
            }
            while (io.Stream.Length - io.Stream.Position >= 0x0c)
            {
                int oldAddress = io.Reader.ReadInt32();
                int newAddress = io.Reader.ReadInt32();
                ushort uSize = io.Reader.ReadUInt16();
                ushort cSize = io.Reader.ReadUInt16();
                if (newAddress > patchedkernel.Length)
                    throw new Exception("Error during patch decompression.");
                switch (cSize)
                {
                    case 0:
                        patchedkernel.Stream.Position = newAddress;
                        patchedkernel.Writer.Write(new byte[uSize]);
                        break;
                    case 1:
                        for (int i = 0; i < uSize; i++)
                        {
                            patchedkernel.Stream.Position = oldAddress + i;
                            byte bit = patchedkernel.Reader.ReadByte();
                            patchedkernel.Stream.Position = newAddress + i;
                            patchedkernel.Writer.Write(bit);
                        }
                        break;
                    default:
                        byte[] patchData = io.Reader.ReadBytes(cSize);
                        patchedkernel.Stream.Position = oldAddress;
                        byte[] array = patchedkernel.Reader.ReadBytes(uSize);

                        if (LDISetWindowData(ldicontext, array, uSize) != 0)
                        {
                            throw new Exception("Failed to set patch decompression window data.");
                        }

                        int compressedLen = cSize;
                        byte[] compressedData = patchData;
                        int uncompressedLen = uSize;
                        byte[] decompressedData = new byte[uncompressedLen];
                        if (LDIDecompress(ldicontext, compressedData, compressedLen, decompressedData,
                                                    ref uncompressedLen) != 0)
                        {
                            throw new Exception("Failed to decompress.");
                        }
                        patchedkernel.Stream.Seek(newAddress, SeekOrigin.Begin);
                        patchedkernel.Writer.Write(decompressedData);
                        if (LDIResetDecompression(ldicontext) != 0)
                        {
                            throw new Exception("Failed reset decompression");
                        }
                        break;
                }
            }
            return ((MemoryStream)patchedkernel.Stream).ToArray();
        }
        public static byte[] DecompressPatchInChunks(byte[] origdata, Bootloader7BL bl7)
        {
            return DecompressPatchInChunks(origdata, bl7.GetData(), bl7.TargetImageSize);
        }
        public static byte[] DecompressInChunks(byte[] data, int offset = 0)
        {
            int maxSize = 0x800000, ctx = -1, unknown = 0;
            LzxDecompress lzx;
            lzx.CpuType = 0x1103;
            lzx.WindowSize = 0x20000;
            IntPtr allocate = Marshal.AllocHGlobal(0x23200);

            if (LDICreateDecompression(ref maxSize, ref lzx,
                0, 0, allocate, ref unknown, ref ctx) != 0)
                throw new Exception("Failed to create decompression.");

            MemoryStream decompressed = new MemoryStream();
            X360IO compressed = new X360IO(data, true) { Stream = { Position = offset } };

            while (compressed.Stream.Position < compressed.Stream.Length)
            {
                ushort cSize = compressed.Reader.ReadUInt16();
                if (cSize == 0)
                    break;

                int uSize = compressed.Reader.ReadUInt16();
                if (uSize == 0)
                    break;

                byte[] compressedData = compressed.Reader.ReadBytes(cSize);

                byte[] decData = new byte[uSize];
                if (LDIDecompress(ctx, compressedData,
                        cSize, decData, ref uSize) != 0)
                    throw new Exception("Failed to decompress data.");

                decompressed.Write(decData, 0, uSize);

            }

            if (LDIDestroyDecompression(ctx) != 0)
                throw new Exception("Failed to destroy decompression.");
            Marshal.FreeHGlobal(allocate);

            byte[] decompressedData = decompressed.ToArray();
            compressed.Close();
            return decompressedData;
        }
    }
}
