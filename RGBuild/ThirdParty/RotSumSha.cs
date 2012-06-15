using System.Security.Cryptography;

namespace RGBuild.ThirdParty
{
    // props to the guy who wrote this and put this on pastebin :)
    public class RotSumSha
    {
        public static byte[] XeCryptRotSumSha(byte[] inp1, int inp1Size, byte[] inp2, int inp2Size) // r3 = data.  r4 = size
        {
            uint rotSumSize = (uint)(inp1Size >> 3); // the size of the rotsum data = size >> 3 (same thing as / 8)
            uint rotSumSize2 = (uint) (inp2Size >> 3);
            byte[] rotSumResult = XeCryptRotSum(new byte[0x20], inp1, (int)rotSumSize); // calculate the RotSum of the input data(r3)
            rotSumResult = XeCryptRotSum(rotSumResult, inp2, (int)rotSumSize2);
            // in the actual assembly, there's another rotsum call here, but r5 is 0, so it immediately returns.

            SHA1 sha = new SHA1Managed(); // create a sha thingy to do all that funky hashing

            // transform the hash with the RotSum result... twice O_o
            sha.TransformBlock(rotSumResult, 0, 0x20, null, 0);
            sha.TransformBlock(rotSumResult, 0, 0x20, null, 0);
            // now we transform it again with the original data
            sha.TransformBlock(inp1, 0, inp1Size, null, 0);
            sha.TransformBlock(inp2, 0, inp2Size, null, 0);
            // now there's another transform, but once again r5 is 0, so i'm going to ignore it

            // now we not every byte in the RotSum result
            for (int i = 0; i < 0x20; i++)
            {
                rotSumResult[i] = (byte)~rotSumResult[i];
            }

            // once we've not'd every byte, we use the result to transform the has 2 more times
            sha.TransformBlock(rotSumResult, 0, 0x20, null, 0);
            sha.TransformFinalBlock(rotSumResult, 0, 0x20); // this is the last transform, so we do transformfinalblock

            return sha.Hash; // now we return the result of out hashing
        }

        public static byte[] XeCryptRotSum(byte[] r3, byte[] r4, int r5)
        {
            /* r5 should be the size of r3 devided by 8 (aka shifted left by 3) */

            int r4P = 0; // the fake pointer of r4, since we can't increment r4 itself
            ulong r7 = LoadDouble(ref r3, 0);           // ld %r7, 0(%r3)
            ulong r9 = LoadDouble(ref r3, 8);           // ld %r9, 8(%r3)
            ulong r6 = LoadDouble(ref r3, 0x10);        // ld %r6, 0x10(%r3)
            ulong r10 = LoadDouble(ref r3, 0x18);       // ld %r10, 0x18(%r3)

            if (r5 == 0)                                // cmplwi cr6, %r5, 0
            {
                return r3;
            }

            for (int i = r5; i > 0; i--)                 // mtctr %r5
            {
                ulong r11 = LoadDouble(ref r4, r4P);    // ld %r11, 0(%r4)
                ulong r8 = r11 + r9;
                r9 = 1;                                 // li %r9, 1

                if (r8 >= r11)              // cmpld cr6, %r8, %r11
                {
                    r9 = 0;                             // li %r9, 1
                }

                r10 = r10 - r11;                        // subf %r10, %r11, %r10
                r7 = r9 + r7;                           // add %r7, %r9, %r7
                r9 = RotateDoubleLeft(r8, 29);    // rldicr %r9, %r8, 29, 63

                /* The code here does not exactly represent the original code.
                 * in the original assembly, a compare was done using r11 and r10
                 * r11 was then set to 1 before the branch.  You can see how this
                 * would cause a proble.  the way I've done it should work though */

                r11 = (ulong) (r10 <= r11 ? 0 : 1);

                r6 = r6 - r11;                          // subf %r6, %r11, %r6
                r10 = RotateDoubleLeft(r10, 31);  // rldicr %r10, %r10, 31, 63
                r4P = r4P + 8;                          // addi %r4, %r4, 8
            }
            StoreDouble(ref r3, 0, r7);
            StoreDouble(ref r3, 8, r9);
            StoreDouble(ref r3, 0x10, r6);
            StoreDouble(ref r3, 0x18, r10);
            return r3;
        }

        private static void StoreDouble(ref byte[] data, int address, ulong inLong)
        {
            data[address + 7] = (byte)(inLong & 0xFF);
            data[address + 6] = (byte)((inLong & 0xFF00) >> 8);
            data[address + 5] = (byte)((inLong & 0xFF0000) >> 16);
            data[address + 4] = (byte)((inLong & 0xFF000000) >> 24);
            data[address + 3] = (byte)((inLong & 0xFF00000000) >> 32);
            data[address + 2] = (byte)((inLong & 0xFF0000000000) >> 40);
            data[address + 1] = (byte)((inLong & 0xFF000000000000) >> 48);
            data[address] = (byte)((inLong & 0xFF00000000000000) >> 56);
        }

        private static uint LoadWord(ref byte[] data, int address)
        {
            uint retInt = 0;
            for (int i = 0; i < 4; i++)
            {
                retInt <<= 8;
                retInt |= data[address + i];
            }
            return retInt;
        }

        private static ulong LoadDouble(ref byte[] data, int address)
        {
            ulong retLong = 0;
            for (int i = 0; i < 8; i++)
            {
                retLong <<= 8;
                retLong |= data[address + i];
            }
            return retLong;//(ulong)((data[address + 7] << 56) | (data[address + 6] << 48) | (data[address + 5] << 40) | (data[address + 4] << 32) | (data[address + 3] << 24) | (data[address + 2] << 16) | (data[address + 1] << 8) | data[address]);
        }

        private static ulong RotateDoubleLeft(ulong inputLong, int rotate)
        {
            return (inputLong << rotate) | (inputLong >> 64 - rotate);
        }
        private static ulong ClearDoubleRight(ulong inputLong, int bits)
        {
            ulong mask = 0;
            for (int i = 0; i < bits; i++)
            {
                //mask <<= 1;
                //mask |= 1;
                ulong temp = 1;
                temp <<= 63 - i;
                mask |= temp;
            }
            return inputLong & mask;
        }
    }
}
