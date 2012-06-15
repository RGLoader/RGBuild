using System;
using System.Text;

namespace RGBuild.Util
{
    internal class HelpWriter
    {
        static readonly string ExeName = AppDomain.CurrentDomain.FriendlyName.Replace(".vshost", "");
        internal static void ShowFullHelp()
        {
            Console.WriteLine("usage:    " + ExeName + " <options> <NAND image filename>");
            Console.WriteLine("options:");
            PrintNamedOption("-ui", "load the (beta) ui");
            PrintNamedOption("-cpu:<key>", "sets the cpukey");
            PrintNamedOption("-1bl:<key>", "sets the 1blkey");
            Console.WriteLine();

            PrintNamedOption("-c", "create image");
            Console.WriteLine();
            PrintNamedOption("-cpy:<types>:<filepath>", "copy from image");
            PrintNamedOption("     b", "bootloaders");
            PrintNamedOption("     f", "files");
            PrintNamedOption("     v", "keyvault");
            PrintNamedOption("     s", "smc");
            PrintNamedOption("     k", "kernel");
            Console.WriteLine();
            PrintNamedOption("-e:<types>:<folder>", "extract from image");
            PrintNamedOption("   b", "bootloaders");
            PrintNamedOption("   f", "files");
            PrintNamedOption("   v", "keyvault");
            PrintNamedOption("   s", "smc");
            PrintNamedOption("   k", "kernel");
            Console.WriteLine();
            PrintNamedOption("-ef:<file>:<folder>", "extract file");
            Console.WriteLine();

            PrintNamedOption("-i:<type>:<filepath>", "inserts section into image");
            PrintNamedOption("   b", "bootloader");
            PrintNamedOption("   v", "keyvault");
            PrintNamedOption("   s", "smc");
            Console.WriteLine();
            PrintNamedOption("-if:<filepath>", "inserts file into image, if directory will add recursively");
            Console.WriteLine();
        }
        internal static void PrintNamedOption(string name, string description)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(' ', 4);
            stringBuilder.Append(name);
            stringBuilder.Append(' ', Math.Max(16 - (4 + name.Length), 0));
            Console.Out.Write(stringBuilder.ToString());
            string value = FormatUtil.FormatStringForWidth(description, 0, 16, FormatUtil.ConsoleBufferWidth, FormatUtil.ConsoleBufferWidth - 16);
            Console.Out.Write(value);
        }
        internal static void PrintNamedOptionLevel2(string name, string description)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(' ', 16);
            stringBuilder.Append(name);
            stringBuilder.Append(' ', Math.Max(36 - (16 + name.Length), 0));
            Console.Out.Write(stringBuilder.ToString());
            string value = FormatUtil.FormatStringForWidth(description, 0, 36, FormatUtil.ConsoleBufferWidth, FormatUtil.ConsoleBufferWidth - 36);
            Console.Out.Write(value);
        }
    }
}
