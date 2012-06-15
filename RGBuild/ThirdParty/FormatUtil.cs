using System;
using System.Collections;
using System.IO;
using System.Security;
using System.Text;

namespace RGBuild.Util
{
    internal static class FormatUtil
    {
        public static int ConsoleBufferWidth
        {
            get
            {
                int result;
                try
                {
                    result = Console.BufferWidth;
                }
                catch (Exception ex)
                {
                    if (!(ex is InvalidOperationException) && !(ex is SecurityException) && !(ex is IOException))
                    {
                        throw;
                    }
                    result = 80;
                }
                return result;
            }
        }
        public static string FormatStringForWidth(string toFormat, int offset, int hangingIndent, int width, int firstLineWidth)
        {
            string[] array = MakeWords(toFormat);
            StringBuilder stringBuilder = new StringBuilder();
            string text = new string(' ', offset + hangingIndent);
            int num = 0;
            int num2;
            for (int i = 0; i < array.Length; i = num2 + 1)
            {
                num2 = i;
                int num3 = offset;
                int num4;
                if (num > 0)
                {
                    num4 = width;
                    num3 += hangingIndent;
                }
                else
                {
                    num4 = firstLineWidth;
                }
                num3 += array[i].Length;
                if (array[i].EndsWith("."))
                {
                    num3 += 2;
                }
                else
                {
                    num3++;
                }
                while (num2 + 1 < array.Length && num3 + array[num2 + 1].Length < num4)
                {
                    num2++;
                    num3 += array[num2].Length;
                    if (array[num2].EndsWith("."))
                    {
                        num3 += 2;
                    }
                    else
                    {
                        num3++;
                    }
                }
                stringBuilder.Append((num > 0) ? text : new string(' ', offset));
                for (int num5 = i; num5 <= num2; num5++)
                {
                    if (num5 > i)
                    {
                        stringBuilder.Append(array[num5 - 1].EndsWith(".") ? "  " : " ");
                    }
                    stringBuilder.Append(array[num5]);
                }
                stringBuilder.Append(Environment.NewLine);
                num++;
            }
            return stringBuilder.ToString();
        }
        public static string[] MakeWords(string toParse)
        {
            char[] array = toParse.ToCharArray();
            StringBuilder stringBuilder = new StringBuilder();
            ArrayList arrayList = new ArrayList();
            char[] array2 = array;
            foreach (char c in array2)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (stringBuilder.Length > 0)
                    {
                        arrayList.Add(stringBuilder.ToString());
                        stringBuilder.Length = 0;
                    }
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }
            if (stringBuilder.Length > 0)
            {
                arrayList.Add(stringBuilder.ToString());
            }
            return (string[])arrayList.ToArray(typeof(string));
        }
    }
}
