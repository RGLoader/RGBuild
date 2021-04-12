using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using IniParser;
using RGBuild.IO;
using RGBuild.NAND;
using RGBuild.ThirdParty;
using RGBuild.Util;

namespace RGBuild
{
    static class Program
    {
       

        public const string Version = "4.1";
        public const string RGversion = "0v800";

        private static readonly string[] CmdLineOptions = new[] { 
            "/guided", "/banner", "/crc32", 
            "/cpukey", "/1blkey", 
            "/create", "/copy", 
            "/extract", "/extractfile", 
            "/inject", "/injectfile", 

            /* creation */
            /* should take precidence(?) over the ini settings */
            "/infoini", "/sparetype", "/pagesperblock",
            "/2bladdr", "/2blpairing",
            "/6bladdr", "/6blpairing", "/6blldv", 
            "/length", "/copyright",
            "/smc", "/smcconfig",
            "/kv", "/altkv",
            "/mobileb", "/mobilec", "/mobilee", "/mobilej",
            "/2bl", "/3bl", "/4bl", "/5bl", "/6bl", "/7bl", "/hack", "/kernel",

        };
        public static string LoadFromIniError = "";

        public static StringCollection StoredKeys = new StringCollection();
        public static bool LoadStoredKeys()
        {
            try
            {
                StoredKeys = new StringCollection();
                string key = RegistryHelper.ReadKey("StoredKeys");
                if (String.IsNullOrEmpty(key))
                    return false;
                string[] keys = key.Split(new[] { "\r\n" }, StringSplitOptions.None);
                foreach (string str in keys)
                    if (!String.IsNullOrEmpty(str))
                        StoredKeys.Add(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static bool SaveStoredKeys()
        {
            string keydata = StoredKeys.Cast<string>().Aggregate("", (current, str) => current + (str + "\r\n"));
            return RegistryHelper.WriteKey("StoredKeys", keydata);
        }
        public static string LoadFromIni(ref NANDImage image, string inipath)
        {
            string path = Path.GetDirectoryName(inipath);
            //Parse the ini file
            FileIniDataParser parser = new FileIniDataParser();
            IniData parsedData = parser.LoadFile(inipath);

            if (parsedData == null)
            {
                PrintError("Error opening ini file.");
                return String.Empty;
            }
            
            /* load our values from the ini first, then check the arguments! >.> */

            //if (!String.IsNullOrEmpty(parsedData["ConsoleSpecific"]["ImageInfoIni"]))
            string imageinfo = parsedData["ConsoleSpecific"]["ImageInfoIni"];
            if (Arguments.Recognized.ContainsKey("/infoini"))

                imageinfo = Arguments.Recognized["/infoini"];

            string cpukey = "00000000000000000000000000000000";
            string sparetype = "SmallBlock";
            string blockcount = "0x400";
            string pagesperblock = "0x20";
// ReSharper disable InconsistentNaming
            string addr2bl = "0x8000";
            string addr6bl = "0x70000";
            string copyright = " Copyright ms bleh";

            string perboxpairing = "0x000000";
            string pairing2bl = "0x000000";
            string pairing6bl = "0x000000";
            string ldv2bl = "0x0";
            string ldv6bl = "0x0";
            string SMC_size = "0x3000";
            string SMC_location = "0x1000";
            string blockoffset = "0x0";
            string smc = "smc.bin";
            string smcconfig = "smc_config.bin";
            string kv = "kv_dec.bin";
            string altkv = "altkv_dec.bin";
            string mobileb = "";
            string mobilec = "";
            string mobilee = "";
            string mobilej = "";
            string _2BL = "";
            string _3BL = "";
            string _4BL = "";
            string _5BL = "";
            string _6BL = "";
            string _7BL = "";
// ReSharper restore InconsistentNaming

            string exploitType = "RGH";
            string consoleType = "";


            string jtag_pairing2bl = "0x000000";
			
			string jtag_syscall = "0x350";
            string jtag_bldrLoc = "0x6D000";
            string jtag_payload = "jtag_payload.bin";


            List<int> badblocks = new List<int>();

            if (!String.IsNullOrEmpty(parsedData["Image"]["exploit"]))
                exploitType = parsedData["Image"]["exploit"];

            if (!String.IsNullOrEmpty(imageinfo))
            {
                IniData parsedData2 = parser.LoadFile(Path.Combine(path, imageinfo));
                if (!String.IsNullOrEmpty(parsedData2["Image"]["Motherboard"]))
                    consoleType = parsedData2["Image"]["Motherboard"];
                if (!String.IsNullOrEmpty(parsedData2["Image"]["SpareDataType"]))
                    sparetype = parsedData2["Image"]["SpareDataType"];
                if (!String.IsNullOrEmpty(parsedData2["Image"]["PagesPerBlock"]))
                    pagesperblock = parsedData2["Image"]["PagesPerBlock"];
                if (!String.IsNullOrEmpty(parsedData2["Image"]["CPUKey"]))
                    cpukey = parsedData2["Image"]["CPUKey"];
                if (!String.IsNullOrEmpty(parsedData2["Image"]["2BLAddress"]))
                    addr2bl = parsedData2["Image"]["2BLAddress"];
                if (!String.IsNullOrEmpty(parsedData2["Image"]["6BLAddress"]))
                    addr6bl = parsedData2["Image"]["6BLAddress"];
                if (!String.IsNullOrEmpty(parsedData2["Image"]["SMCSize"]))
                    SMC_size = parsedData2["Image"]["SMCSize"];
                if (!String.IsNullOrEmpty(parsedData2["Image"]["SMCLocation"]))
                    SMC_location = parsedData2["Image"]["SMCLocation"];

                if (!String.IsNullOrEmpty(parsedData2["Image"]["BlockCount"]))
                    blockcount = parsedData2["Image"]["BlockCount"];
                if (!String.IsNullOrEmpty(parsedData2["Image"]["BlockOffset"]))
                    blockoffset = parsedData2["Image"]["BlockOffset"];
                if (!String.IsNullOrEmpty(parsedData2["Image"]["Copyright"]))
                    copyright = parsedData2["Image"]["Copyright"].Replace("\"", "");
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["2BLPairing"]))
                    perboxpairing = parsedData2["ConsoleSpecific"]["2BLPairing"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["2BLPairing"]))
                    pairing2bl = parsedData2["ConsoleSpecific"]["2BLPairing"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["6BLPairing"]))
                    pairing6bl = parsedData2["ConsoleSpecific"]["6BLPairing"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["6BLLDV"]))
                    ldv6bl = parsedData2["ConsoleSpecific"]["6BLLDV"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["2BLLDV"]))
                    ldv2bl = parsedData2["ConsoleSpecific"]["2BLLDV"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["ConsoleType"]))
                    consoleType = parsedData2["ConsoleSpecific"]["ConsoleType"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["SMC"]))
                    smc = parsedData2["ConsoleSpecific"]["SMC"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["SMCConfig"]))
                    smcconfig = parsedData2["ConsoleSpecific"]["SMCConfig"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["KeyVault"]))
                    kv = parsedData2["ConsoleSpecific"]["KeyVault"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["AltKeyVault"]))
                    altkv = parsedData2["ConsoleSpecific"]["AltKeyVault"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["MobileB"]))
                    mobileb = parsedData2["ConsoleSpecific"]["MobileB"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["MobileC"]))
                    mobilec = parsedData2["ConsoleSpecific"]["MobileC"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["MobileE"]))
                    mobilee = parsedData2["ConsoleSpecific"]["MobileE"];
                if (!String.IsNullOrEmpty(parsedData2["ConsoleSpecific"]["MobileJ"]))
                    mobilej = parsedData2["ConsoleSpecific"]["MobileJ"];
                if (!String.IsNullOrEmpty(parsedData2["Bootloaders"]["2BL"]))
                    _2BL = parsedData2["Bootloaders"]["2BL"];
                if (!String.IsNullOrEmpty(parsedData2["Bootloaders"]["3BL"]))
                    _3BL = parsedData2["Bootloaders"]["3BL"];
                if (!String.IsNullOrEmpty(parsedData2["Bootloaders"]["4BL"]))
                    _4BL = parsedData2["Bootloaders"]["4BL"];
                if (!String.IsNullOrEmpty(parsedData2["Bootloaders"]["5BL"]))
                    _5BL = parsedData2["Bootloaders"]["5BL"];
                if (!String.IsNullOrEmpty(parsedData2["Bootloaders"]["6BL"]))
                    _6BL = parsedData2["Bootloaders"]["6BL"];
                if (!String.IsNullOrEmpty(parsedData2["Bootloaders"]["7BL"]))
                    _7BL = parsedData2["Bootloaders"]["7BL"];

                //badblocks.AddRange(parsedData2["BadBlocks"].Select(kd => int.Parse(kd.KeyName, NumberStyles.HexNumber)));

                string temp="";
                foreach (KeyData kd in parsedData2["BadBlocks"])
                {
                    temp = kd.KeyName.Replace("0x", "");
                    badblocks.Add(Convert.ToInt32(temp, 16));
                }
            }

            if(exploitType!="JTAG"){
                IniData parsedData3 = parser.LoadFile(Path.Combine(path, "bootloaders.ini"));

                if (!String.IsNullOrEmpty(parsedData3[consoleType + exploitType]["2BL"]))
                    _2BL = parsedData3[consoleType + exploitType]["2BL"];
                if (!String.IsNullOrEmpty(parsedData3[consoleType + exploitType]["3BL"]))
                    _3BL = parsedData3[consoleType + exploitType]["3BL"];
                if (!String.IsNullOrEmpty(parsedData3[consoleType + exploitType]["4BL"]))
                    _4BL = parsedData3[consoleType + exploitType]["4BL"];
                if (!String.IsNullOrEmpty(parsedData3[consoleType + exploitType]["5BL"]))
                    _5BL = parsedData3[consoleType + exploitType]["5BL"];
                if (!String.IsNullOrEmpty(parsedData3[consoleType + exploitType]["6BL"]))
                    _6BL = parsedData3[consoleType + exploitType]["6BL"];
                if (!String.IsNullOrEmpty(parsedData3[consoleType + exploitType]["7BL"]))
                    _7BL = parsedData3[consoleType + exploitType]["7BL"];
            }

            if (!String.IsNullOrEmpty(parsedData["Image"]["2BLAddress"]))
                addr2bl = parsedData["Image"]["2BLAddress"];
            if (!String.IsNullOrEmpty(parsedData["Image"]["6BLAddress"]))
                addr6bl = parsedData["Image"]["6BLAddress"];
            if (!String.IsNullOrEmpty(parsedData["Image"]["SMCSize"]))
                SMC_size = parsedData["Image"]["SMCSize"];
            if (!String.IsNullOrEmpty(parsedData["Image"]["SMCLocation"]))
                SMC_location = parsedData["Image"]["SMCLocation"];
            if (!String.IsNullOrEmpty(parsedData["Image"]["BlockCount"]))
                blockcount = parsedData["Image"]["BlockCount"];
            if (!String.IsNullOrEmpty(parsedData["Image"]["BlockOffset"]))
                blockoffset = parsedData["Image"]["BlockOffset"];
            if (!String.IsNullOrEmpty(parsedData["Image"]["Copyright"]))
                copyright = parsedData["Image"]["Copyright"].Replace("\"", "");
            

            if (!String.IsNullOrEmpty(parsedData["Bootloaders"]["2BL"]))
                _2BL = parsedData["Bootloaders"]["2BL"];
            if (!String.IsNullOrEmpty(parsedData["Bootloaders"]["3BL"]))
                _3BL = parsedData["Bootloaders"]["3BL"];
            if (!String.IsNullOrEmpty(parsedData["Bootloaders"]["4BL"]))
                _4BL = parsedData["Bootloaders"]["4BL"];
            if (!String.IsNullOrEmpty(parsedData["Bootloaders"]["5BL"]))
                _5BL = parsedData["Bootloaders"]["5BL"];
            if (!String.IsNullOrEmpty(parsedData["Bootloaders"]["6BL"]))
                _6BL = parsedData["Bootloaders"]["6BL"];
            if (!String.IsNullOrEmpty(parsedData["Bootloaders"]["7BL"]))
                _7BL = parsedData["Bootloaders"]["7BL"];
            if (_6BL == "None") _6BL = "";
            if (_7BL == "None") _7BL = "";


            if (!String.IsNullOrEmpty(parsedData["ConsoleSpecific"]["2BLPairing"]))
                pairing2bl = parsedData["ConsoleSpecific"]["2BLPairing"];
            if (!String.IsNullOrEmpty(parsedData["ConsoleSpecific"]["6BLPairing"]))
                pairing6bl = parsedData["ConsoleSpecific"]["6BLPairing"];
            if (!String.IsNullOrEmpty(parsedData["ConsoleSpecific"]["6BLLDV"]))
                pairing6bl = parsedData["ConsoleSpecific"]["6BLLDV"];


            if (Arguments.Recognized.ContainsKey("/cpukey"))
                cpukey = Arguments.Recognized["/cpukey"];
            if (Arguments.Recognized.ContainsKey("/copyright"))
                copyright = Arguments.Recognized["/copyright"].Replace("\"", "");
            if (Arguments.Recognized.ContainsKey("/2bladdr"))
                addr2bl = Arguments.Recognized["/2bladdr"];
            if (Arguments.Recognized.ContainsKey("/6bladdr"))
                addr6bl = Arguments.Recognized["/6bladdr"];
            if (Arguments.Recognized.ContainsKey("/pairing"))
            {
                pairing2bl = Arguments.Recognized["/pairing"];
                pairing6bl = pairing2bl;
            }
            if (Arguments.Recognized.ContainsKey("/2blpairing"))
                pairing2bl = Arguments.Recognized["/2blpairing"];
            if (Arguments.Recognized.ContainsKey("/6blpairing"))
                pairing6bl = Arguments.Recognized["/6blpairing"];
            if (Arguments.Recognized.ContainsKey("/pagesperblock"))
                pagesperblock = Arguments.Recognized["/pagesperblock"];
            if (Arguments.Recognized.ContainsKey("/sparetype"))
                sparetype = Arguments.Recognized["/sparetype"];
            if (Arguments.Recognized.ContainsKey("/ldv"))
                ldv6bl = Arguments.Recognized["/ldv"];
            if (Arguments.Recognized.ContainsKey("/blockcount"))
                blockcount = Arguments.Recognized["/blockcount"];
            if (Arguments.Recognized.ContainsKey("/blockoffset"))
                blockcount = Arguments.Recognized["/blockoffset"];
            if (Arguments.Recognized.ContainsKey("/smc"))
                smc = Arguments.Recognized["/smc"];
            if (Arguments.Recognized.ContainsKey("/smcconfig"))
                smcconfig = Arguments.Recognized["/smcconfig"];
            if (Arguments.Recognized.ContainsKey("/kv"))
                kv = Arguments.Recognized["/kv"];
            if (Arguments.Recognized.ContainsKey("/altkv"))
                altkv = Arguments.Recognized["/altkv"];
            if (Arguments.Recognized.ContainsKey("/mobileb"))
                mobileb = Arguments.Recognized["/mobileb"];
            if (Arguments.Recognized.ContainsKey("/mobilec"))
                mobilec = Arguments.Recognized["/mobilec"];
            if (Arguments.Recognized.ContainsKey("/mobilee"))
                mobilee = Arguments.Recognized["/mobilee"];
            if (Arguments.Recognized.ContainsKey("/mobilej"))
                mobilej = Arguments.Recognized["/mobilej"];
            if (Arguments.Recognized.ContainsKey("/2bl"))
                _2BL = Arguments.Recognized["/2bl"];
            if (Arguments.Recognized.ContainsKey("/3bl"))
                _2BL = Arguments.Recognized["/3bl"];
            if (Arguments.Recognized.ContainsKey("/4bl"))
                _4BL = Arguments.Recognized["/4bl"];
            if (Arguments.Recognized.ContainsKey("/5bl"))
                _5BL = Arguments.Recognized["/5bl"];
            if (Arguments.Recognized.ContainsKey("/6bl"))
                _6BL = Arguments.Recognized["/6bl"];
            if (Arguments.Recognized.ContainsKey("/7bl"))
                _7BL = Arguments.Recognized["/7bl"];

            perboxpairing = perboxpairing.Replace("0x", "");
            pairing2bl = pairing2bl.Replace("0x", "");
            pairing6bl = pairing6bl.Replace("0x", "");
            ldv6bl = ldv6bl.Replace("0x", "");
            ldv2bl = ldv6bl.Replace("0x", "");
            pagesperblock = pagesperblock.Replace("0x", "");
            blockcount = blockcount.Replace("0x", "");
            blockoffset = blockoffset.Replace("0x", "");

            jtag_bldrLoc = jtag_bldrLoc.Replace("0x", "");
            jtag_syscall = jtag_syscall.Replace("0x", "");
            jtag_payload = jtag_payload.Replace("0x", "");
            jtag_pairing2bl = jtag_pairing2bl.Replace("0x", "");

            uint bl2Addr = uint.Parse(addr2bl.Replace("0x", ""),
                                          addr2bl.Contains("0x") ? NumberStyles.HexNumber : NumberStyles.Integer);
            uint bl6addr = uint.Parse(addr6bl.Replace("0x", ""),
                                      addr6bl.Contains("0x") ? NumberStyles.HexNumber : NumberStyles.Integer);

            if (!Lists.ExploitTypes.Contains(exploitType)) PrintError("Exploit type " + exploitType + " not supported/recognized.");

            byte[] jtagchaindata = null;
            byte[] SMCdata = null;

            if (!String.IsNullOrEmpty(smc))
            {

                byte[] data = File.ReadAllBytes(Path.Combine(path, smc));
                if (data.Length < 0x3000 || data.Length > 0x4000)
                {
                    return "SMC is incorrect size: 0x" + data.Length.ToString("X");
                }
                SMCdata = data;
            }
            
            if (exploitType == "JTAG")
            {
                if (!String.IsNullOrEmpty(parsedData["JTAG"]["payload"]))
                    jtag_payload = parsedData["JTAG"]["payload"];
                if (!String.IsNullOrEmpty(parsedData["JTAG"]["syscall"]))
                    jtag_syscall = parsedData["JTAG"]["syscall"];
                if (!String.IsNullOrEmpty(parsedData["JTAG"]["bldr_location"]))
                    jtag_bldrLoc = parsedData["JTAG"]["bldr_location"];
                if (!String.IsNullOrEmpty(parsedData["JTAG"]["2BLPairing"]))
                    jtag_pairing2bl = parsedData["JTAG"]["2BLPairing"];

                jtag_bldrLoc = jtag_bldrLoc.Replace("0x", "");
                jtag_syscall = jtag_syscall.Replace("0x", "");
                jtag_payload = jtag_payload.Replace("0x", "");
                jtag_pairing2bl = jtag_pairing2bl.Replace("0x", "");

                Console.WriteLine("Building jtag bootchain");
                NANDImage jtagchain = new NANDImage();

                if (!String.IsNullOrEmpty(cpukey))
                    jtagchain.CPUKey = Shared.HexStringToBytes(cpukey.Replace("0x", ""));

                MemoryStream stream = new MemoryStream(new byte[int.Parse(blockcount, NumberStyles.HexNumber) * 0x4200]);

                jtagchain.IO = new X360IO(stream, true);

                //add header
                jtagchain.CreateHeader();
                jtagchain.Header.Entrypoint = bl2Addr;
                jtagchain.Header.Size = bl6addr;
                jtagchain.Header.SysUpdateAddress = bl6addr;
                if (!String.IsNullOrEmpty(copyright))
                    jtagchain.Header.Copyright = copyright;

                //add smc to temporary image
                jtagchain.SMC = new SMC(jtagchain.IO);

                jtagchain.SMC.SetData(SMCdata);


                Console.WriteLine("Parsing INI");
                string jtag2bl = Path.Combine(path, parsedData[consoleType]["jtag2BL"]);
                string jtag4bl = Path.Combine(path, _4BL);
                string jtag5bl = Path.Combine(path, _5BL);

                Console.WriteLine("Loading 4bl from: " + jtag4bl);

                string result = jtagchain.AddBootloaderFromPath(jtag2bl);
                if (result != null) PrintError(result);
                jtagchain.AddBootloaderFromPath(jtag4bl);
                if (result != null) PrintError(result);
                jtagchain.AddBootloaderFromPath(jtag5bl);
                if (result != null) PrintError(result);


                Bootloader2BL bl2 = (Bootloader2BL)jtagchain.Bootloaders.Find(s => s.GetType() == typeof(Bootloader2BL));
                if (bl2 == null) PrintError("Error Loading JTAG 2BL");
                Bootloader4BL bl4 = (Bootloader4BL)jtagchain.Bootloaders.Find(s => s.GetType() == typeof(Bootloader4BL));
                if (bl4 == null) PrintError("Error Loading JTAG 4BL");
                Bootloader5BL bl5 = (Bootloader5BL)jtagchain.Bootloaders.Find(s => s.GetType() == typeof(Bootloader5BL));
                if (bl5 == null) PrintError("Error Loading JTAG 5BL");

                bl2.PerBoxData.PairingData = new byte[]
                                                     {
                                                         byte.Parse(jtag_pairing2bl.Substring(0, 2), NumberStyles.HexNumber),
                                                         byte.Parse(jtag_pairing2bl.Substring(2, 2), NumberStyles.HexNumber),
                                                         byte.Parse(jtag_pairing2bl.Substring(4, 2), NumberStyles.HexNumber)
                                                     };

                if ((bl2.PerBoxData.PairingData[0] | bl2.PerBoxData.PairingData[1] | bl2.PerBoxData.PairingData[2]) != 0)
                {
                    bl2.PerBoxData.PerBoxDigest = bl2.PerBoxData.CalculateDigest();
                }
                else
                {
                    bl2.PerBoxData.PerBoxDigest = new byte[0x10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                }

                Console.WriteLine("Writing to binary file");
                //Console.WriteLine("");


                string jtagBinPath = "jtagchain.bin";
                if (File.Exists(Path.Combine(path, jtagBinPath))) File.Delete(Path.Combine(path, jtagBinPath));
                FileStream jtagChainBin = new FileStream(Path.Combine(path, jtagBinPath), FileMode.Create, System.IO.FileAccess.Write);


                Console.WriteLine("Saving bootloaders");

                jtagchain.SaveBootloaders(bl2Addr, bl6addr);

                byte[] bl2Data = bl2.GetData(false);
                byte[] bl4Data = bl4.GetData(false);
                byte[] bl5Data = bl5.GetData(false);

                int chainsize = (int)(bl2.Size_r + bl4.Size_r + bl5.Size_r);


                jtagchaindata = new byte[chainsize + 0x10];

                Array.Copy(bl2Data, 0, jtagchaindata, 0, bl2.Size_r);
                Array.Copy(bl4Data, 0, jtagchaindata, bl2.Size_r, bl4.Size_r);
                Array.Copy(bl5Data, 0, jtagchaindata, bl4.Size_r + bl2.Size_r, bl5.Size_r);

                /*jtagchain.IO.Stream.Flush();

                jtagchain.IO.Stream.Position = 0x8000; //read from CB start to just read bootchain part of nand image
                jtagchain.IO.Stream.Read(jtagchaindata, 0, chainsize/2 );

                jtagchain.IO.Stream.Flush();
                jtagchain.IO.Stream.Position = 0x8000 + chainsize/2;
                jtagchain.IO.Stream.Read(jtagchaindata, chainsize / 2, (chainsize / 2) + 1);*/


                if (jtagChainBin.CanWrite)
                {

                    jtagChainBin.Write(jtagchaindata, 0, chainsize); //save it to a file for later, just in case

                }
                else Console.WriteLine("ERROR: Unable to save jtag bootchain binary to file.");

                jtagChainBin.Close();

                _2BL = parsedData[consoleType]["2BL"];
                _4BL = parsedData[consoleType]["4BL"];
                _5BL = parsedData[consoleType]["5BL"];
                _6BL = parsedData[consoleType]["6BL"];
                _7BL = parsedData[consoleType]["7BL"];
            }

            if (image == null || image.IO == null)
            {

                if (image == null)
                    image = new NANDImage();
                if (!String.IsNullOrEmpty(cpukey))
                    image.CPUKey = Shared.HexStringToBytes(cpukey.Replace("0x", ""));
                //Image._1BLKey = dialog.BLKey;
                NANDImageStream stream =
                    new NANDImageStream(new MemoryStream(new byte[int.Parse(blockcount, NumberStyles.HexNumber) * 0x4200]), 0x200);
                stream.PagesPerBigBlock = int.Parse(pagesperblock, NumberStyles.HexNumber);
                stream.SpareDataType = (SpareDataType)Enum.Parse(typeof(SpareDataType), sparetype);
                Console.WriteLine("NAND SpareDataType: " + stream.SpareDataType.ToString());

                image.IO = new X360IO(stream, true);
                image.CreateHeader();
                image.Header.Entrypoint = bl2Addr;
                image.Header.Size = bl6addr;
                image.Header.SysUpdateAddress = bl6addr;

                //image.Header.kdNetIP = Util.Shared.LocalIPAddress();
                image.Header.kdNetIP = new byte[] {192, 168, 0, 125};
                //image.Header.kdNetData = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                //image.Header.kdNetIP = new byte[] { 0, 0, 0, 0 };
                {
                    SMC_location = SMC_location.Replace("0x", "");
                    if (SMC_location.Length % 2 != 0) SMC_location = "0" + SMC_location;
                    SMC_size = SMC_size.Replace("0x", "");
                    if (SMC_size.Length % 2 != 0) SMC_size = "0" + SMC_size;

                    byte[] smcbytes=Shared.HexStringToBytes(SMC_location);
                    Array.Reverse(smcbytes, 0, 2);
                    image.Header.SmcAddress = (uint)BitConverter.ToInt16(smcbytes, 0);

                    smcbytes = Shared.HexStringToBytes(SMC_size);
                    Array.Reverse(smcbytes, 0, 2);
                    image.Header.SmcSize = (uint)BitConverter.ToInt16(smcbytes, 0);
                }

                if (!String.IsNullOrEmpty(copyright))
                    image.Header.Copyright = copyright;
            }

            ((NANDImageStream)image.IO.Stream).badblocks = badblocks;

            if (SMCdata != null)
            {

                if (image.SMC == null)
                    image.SMC = new SMC(image.IO);

                image.SMC.SetData(SMCdata);
            }

            if (!String.IsNullOrEmpty(kv))
            {
                byte[] data = File.ReadAllBytes(Path.Combine(path, kv));
                if (data.Length != 0x4000 && data.Length != 0x3ff0)
                {
                    return "KeyVault is incorrect size.";
                }
                byte[] data2 = new byte[0x3ff0];
                Array.Copy(data, data.Length == 0x4000 ? 0x10 : 0x0, data2, 0, 0x3ff0);
                if (image.KeyVault == null)
                    image.KeyVault = new KeyVault(image.IO, image.CPUKey);

                image.KeyVault.SetData(data);
            }
            string ret;
            if (!String.IsNullOrEmpty(_2BL))
            {
                string[] split = _2BL.Split(new[] { ":" }, StringSplitOptions.None);
                foreach (string str in split)
                {
                    ret = image.AddBootloaderFromPath(Path.Combine(path, str));
                    if (!String.IsNullOrEmpty(ret))
                        return ret;
                }
            }
            if (!String.IsNullOrEmpty(_3BL))
            {
                ret = image.AddBootloaderFromPath(Path.Combine(path, _3BL));
                if (!String.IsNullOrEmpty(ret))
                    return ret;
            }
            if (!String.IsNullOrEmpty(_4BL))
            {
                ret = image.AddBootloaderFromPath(Path.Combine(path, _4BL));
                if (!String.IsNullOrEmpty(ret))
                    return ret;
            }
            if (!String.IsNullOrEmpty(_5BL))
            {
                try
                {
                    ret = image.AddBootloaderFromPath(Path.Combine(path, _5BL));

                    if (!String.IsNullOrEmpty(ret))
                        return ret;
                }
                catch
                {
                    PrintError("Unable to find 5BL: " + _5BL + ". Make sure its in loaders/5bl");
                }

            }
            if (!String.IsNullOrEmpty(_6BL))
            {
                ret = image.AddBootloaderFromPath(Path.Combine(path, _6BL));

                if (!String.IsNullOrEmpty(ret))
                    return ret;
            }
            if (!String.IsNullOrEmpty(_7BL))
            {
                ret = image.AddBootloaderFromPath(Path.Combine(path, _7BL));
                if (!String.IsNullOrEmpty(ret))
                    return ret;
            }

            image.CreateFileSystem();
            //Image.CurrentFileSystem.BlockOffset = ushort.Parse(pagesperblock, NumberStyles.HexNumber);
            image.CurrentFileSystem.BlockOffset = ushort.Parse(blockoffset, NumberStyles.HexNumber);

            foreach (KeyData kd in parsedData["Payloads"])
            {
                string[] dat = kd.KeyName.Split(new[] { ":" }, StringSplitOptions.None);
                dat[0] = dat[0].Replace("0x", "");
                uint addr = uint.Parse(dat[0], NumberStyles.HexNumber);
                byte[] data = File.ReadAllBytes(Path.Combine(path, dat[1]));
                uint size = (uint)data.Length;
                image.AddPayload(kd.Value, addr, data);

                int startblk = (int)(addr / ((NANDImageStream)image.IO.Stream).BlockLength);
                int numblks = (int)((size / ((NANDImageStream)image.IO.Stream).BlockLength) + (size % ((NANDImageStream)image.IO.Stream).BlockLength > 0 ? 1 : 0));
                for (int i = 0; i < numblks; i++)
                    image.CurrentFileSystem.BlockMap[i+startblk] = 0x1ffb;
            }

            foreach (KeyData kd in parsedData["Files"])
            {
                try
                {
                    if (File.Exists(Path.Combine(path, "files\\" + kd.Value)))
                    {

                        FileSystemEntry ent = image.CurrentFileSystem.AddNewEntry(kd.Value, false);
                        ent.SetData(File.ReadAllBytes(Path.Combine(path, "files\\" + kd.Value)));
                        if (File.Exists(Path.Combine(path, "files\\" + kd.Value + ".meta")))
                        {
                            ent.SetMeta(File.ReadAllBytes(Path.Combine(path, "files\\" + kd.Value + ".meta")));
                        }
                    }
                    else
                    {
                        Console.WriteLine("WRN: Unable to load filesystem file: " + kd.Value);
                    }
                }
                catch
                {
                    Console.WriteLine("WRN: Unable to load filesystem file: " + kd.Value);
                }
            }

            if (!String.IsNullOrEmpty(mobileb))
            {
                image.AddMobileFile(File.ReadAllBytes(Path.Combine(path, mobileb)), FileSystemExEntries.MobileB);
            }
            if (!String.IsNullOrEmpty(mobilec))
            {
                image.AddMobileFile(File.ReadAllBytes(Path.Combine(path, mobilec)), FileSystemExEntries.MobileC);
            }
            if (!String.IsNullOrEmpty(mobilee))
            {
                image.AddMobileFile(File.ReadAllBytes(Path.Combine(path, mobilee)), FileSystemExEntries.MobileE);
            }
            if (!String.IsNullOrEmpty(mobilej))
            {
                image.AddMobileFile(File.ReadAllBytes(Path.Combine(path, mobilej)), FileSystemExEntries.MobileJ);
            }
            if (!String.IsNullOrEmpty(smcconfig))
            {
                byte[] data = File.ReadAllBytes(Path.Combine(path, smcconfig));
                if (data.Length != 0x10000)
                {
                    return "SMC config is incorrect size.";
                }
                image.ConfigBlock = data;
            }
            if (image.Bootloaders.Count > 1)
            {

                int bl2idx = 1;
                if (image.Bootloaders.Count > bl2idx &&
                    image.Bootloaders[bl2idx + 1].GetType() == typeof(Bootloader2BL))
                    bl2idx++;

                for (int blI = image.Bootloaders.Count - 1; blI >= 0 ; blI--)
                {
                    if (image.Bootloaders[blI].GetType() == typeof(Bootloader2BL))
                    {
                        ;

                        if (!String.IsNullOrEmpty(pairing2bl))
                        {
                            ((Bootloader2BL)image.Bootloaders[blI]).PerBoxData.PairingData = new byte[]
                                                     {
                                                         byte.Parse(pairing2bl.Substring(0, 2), NumberStyles.HexNumber),
                                                         byte.Parse(pairing2bl.Substring(2, 2), NumberStyles.HexNumber),
                                                         byte.Parse(pairing2bl.Substring(4, 2), NumberStyles.HexNumber)
                                                     };

                            if ((((Bootloader2BL)image.Bootloaders[blI]).PerBoxData.PairingData[0] | ((Bootloader2BL)image.Bootloaders[blI]).PerBoxData.PairingData[1] | ((Bootloader2BL)image.Bootloaders[blI]).PerBoxData.PairingData[2]) == 0)
                            {
                                Console.WriteLine("*** Zero pairing image");
                                ((Bootloader2BL)image.Bootloaders[blI]).PerBoxData.PerBoxDigest = new byte[0x10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                            }
                        }

                        break;
                    }
                }
                
                
            }
            if (image.Bootloaders.Count > 4)
            {

                Bootloader6BL bl6 = (Bootloader6BL)image.Bootloaders.Find(s => s.GetType() == typeof(Bootloader6BL));
                if (!String.IsNullOrEmpty(pairing6bl) && bl6 != null)
                {
                    bl6.PerBoxData.PairingData = new byte[]
                                                     {
                                                         byte.Parse(pairing6bl.Substring(0, 2), NumberStyles.HexNumber),
                                                         byte.Parse(pairing6bl.Substring(2, 2), NumberStyles.HexNumber),
                                                         byte.Parse(pairing6bl.Substring(4, 2), NumberStyles.HexNumber)
                                                     };
                    bl6.PerBoxData.LockDownValue = Convert.ToByte(ldv6bl);



                }
                if ((bl6.PerBoxData.PairingData[0] | bl6.PerBoxData.PairingData[1] | bl6.PerBoxData.PairingData[2]) == 0)
                {
                    bl6.PerBoxData.PerBoxDigest = new byte[0x10] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                }

                if (!String.IsNullOrEmpty(ldv6bl) && bl6 != null)
                {
                    bl6.PerBoxData.LockDownValue = byte.Parse(ldv6bl, NumberStyles.HexNumber);
                }
            }

            //add second bootloader chain for jtag
            if (exploitType == "JTAG")
            {
                if (jtagchaindata != null)
                {
                    Console.WriteLine("Adding second jtag bootloader chain to image at 0x" + jtag_bldrLoc);
                    image.AddPayload("Jtag Bootchain", uint.Parse(jtag_bldrLoc, NumberStyles.HexNumber), jtagchaindata);
                }
                string jtag_payload_path = Path.Combine(path, jtag_payload);


                if (File.Exists(jtag_payload_path))
                {
                    int pagenumber = 1;
                    uint jtagpayloadoffset = 0x200;

                    Console.WriteLine("Adding jtag payload to image");
                    image.AddPayload("Jtag Payload", jtagpayloadoffset, File.ReadAllBytes(jtag_payload_path));

                    ISpareData edc = (ISpareData)((NANDImageStream)image.IO.Stream).GetPageSpare(pagenumber);
                    //SpareDataSmallBlock edc = (SpareDataSmallBlock)((NANDImageStream)Image.IO.Stream).GetPageSpare(pagenumber);


                    //Console.WriteLine("Parsing data: " + jtag_syscall);
                    edc._unused1 = (short)uint.Parse(jtag_syscall, NumberStyles.HexNumber);
                    ((NANDImageStream)image.IO.Stream).WritePageSpare(pagenumber, edc);
                }
                else PrintError("Unable to load jtag payload at: " + jtag_payload_path);

            }



            return null;
        }

        public static void showBanner()
        {
            Console.WriteLine("  \\___________  _________  _______________  __________________/");
            Console.WriteLine("              \\\\\\        \\/        \\     ///");
            Console.WriteLine("                \\\\/  _    \\         \\  \\//");
            Console.WriteLine("                 /   /____/   ______/   \\");
            Console.WriteLine("                /       \\    /__    \\    \\");
            Console.WriteLine("   ____        /___  /   \\      \\   /     \\____           ____");
            Console.WriteLine("       \\        ///__\\    \\   _____/   __//    \\         /");
            Console.WriteLine("        \\_____________\\____\\_///||\\\\\\__________/________/");
            Console.WriteLine("     __________________________/  \\__________________________");
            Console.WriteLine("    /               - RESET GLITCH LOADER-DEV - "+RGversion+"        \\");
            Console.WriteLine(" __/                      RGBuild "+Version+"                        \\___");
            Console.WriteLine("   \\          by stoker25, tydye81 & #RGLoader@EFnet          /");
            Console.WriteLine("    \\________________________________________________________/");

        }


        public static CmdLine Arguments;

        public static void pause(string msg=""){
            if (msg != "") Console.Write(msg);
            else Console.Write("Press any key to continue");

            Console.ReadKey();
        }


        public static string getConsoleType(NANDImage image)
        {
            string smcConType = image.SMC.GetConsoleType();
            string cbConType = getConsoleTypeCB(image);

            if (cbConType == null) return smcConType;

            return cbConType;
        }


        public static string getConsoleTypeCB(NANDImage image)
        {
            if (image.Bootloaders.Count < 1) return null;

            Bootloader bl2 = image.Bootloaders[1];

            if (Lists.XENON_CB.Contains(bl2.Build)) return "Xenon";
            if (Lists.ZEPHYR_CB.Contains(bl2.Build)) return "Zephyr";
            if (Lists.FALCON_CB.Contains(bl2.Build)) return "Falcon";
            if (Lists.JASPER_CB.Contains(bl2.Build)) return "Jasper";

            return null;
        }
        public static bool copyFile(string source, string dest)
        {
            if (!File.Exists(source))
                return false;
            File.Copy(source, dest, true);
            if (!File.Exists(source + ".meta"))
                return true;
            File.Copy(source + ".meta", dest + ".meta", true);
            return true;
        }
        public static bool copyDir(string source, string dest)
        {
            if (!System.IO.Directory.Exists(dest)) System.IO.Directory.CreateDirectory(dest);

            string[] fileArray = Directory.GetFiles(source);
            for (int i = 0; i < fileArray.Length; i++)
            {
                try
                {
                    File.Copy(fileArray[i], dest + "\\" + Path.GetFileName(fileArray[i]), true);
                }

                catch(Exception E)
                {
                    Console.WriteLine("WRN: Unable to copy file: " + Path.GetFileName(fileArray[i]));
                    Console.WriteLine("     Exception: " + E.ToString());
                    continue;
                }
            }
            return true;
        }


        public static void extractImage(NANDImage image, string types, string folder)
        {
            
                // determine what to extract
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (char i in types)
            {
                if (i == 'b') // extract bootloaders
                {
                    foreach (Bootloader bl in image.Bootloaders)
                    {
                        if (bl.GetType() != typeof(Bootloader1BL))
                        {
                            string magic = bl.Magic.ToString();
                            if (bl.GetType() == typeof(Bootloader2BL))
                            {
                                magic += "_";
                                if (((Bootloader2BL)bl).CPUKey == null || ((Bootloader2BL)bl).CPUKey[0] == 0 && ((Bootloader2BL)bl).CPUKey[1] == 0 && ((Bootloader2BL)bl).CPUKey[2] == 0 && ((Bootloader2BL)bl).CPUKey[3] == 0)
                                    magic += "A";
                                else
                                    magic += "B";
                            }
                            Console.WriteLine("* Extracting " + magic + " (ver " + bl.Build + ")");

                            File.WriteAllBytes(
                                Path.Combine(folder, magic + "." + bl.Build.ToString() + ".bin"),
                                bl.GetData());
                        }
                    }
                }
                if (i == 'f') // extract files
                {
                    if (image.CurrentFileSystem == null)
                        //PrintError("No file system in image.");
                        Console.WriteLine("No file system in image");
                    else
                    {
                        foreach (FileSystemEntry ent in image.CurrentFileSystem.Entries)
                        {
                            if (ent.Deleted)
                                continue;
                            string dir = Path.Combine(folder, @"files\");
                            if (!Directory.Exists(dir))
                                Directory.CreateDirectory(dir);
                            Console.WriteLine("* Extracting {0} size 0x{1:X}", ent.FileName, ent.Size);
                            File.WriteAllBytes(Path.Combine(dir, ent.FileName), ent.GetData());
                            File.WriteAllBytes(Path.Combine(dir, ent.FileName) + ".meta", ent.GetMeta());
                        }
                    }
                }

                if (i == 'i')
                {
                    Console.WriteLine("* Extracting image info");
                    StringBuilder ini = new StringBuilder();
                    ini.AppendLine("[Image]");
                    ini.AppendLine("Motherboard = " + getConsoleType(image));
                    ini.AppendLine("CPUKey = " + Shared.BytesToHexString(image.CPUKey, ""));
                    ini.AppendLine("2BLAddress = 0x" + image.Header.Entrypoint.ToString("X"));
                    ini.AppendLine("6BLAddress = 0x" + image.Header.SysUpdateAddress.ToString("X"));
                    ini.AppendLine("SMCSize = 0x" + image.Header.SmcSize.ToString("X"));
                    ini.AppendLine("SMCLocation = 0x" + image.Header.SmcAddress.ToString("X"));
                    ini.AppendLine("Copyright = \"" + image.Header.Copyright + "\"");
                    ini.AppendLine("PagesPerBlock = 0x" + ((NANDImageStream)image.IO.Stream).PagesPerBigBlock.ToString("X"));
                    ini.AppendLine("SpareDataType = " + ((NANDImageStream)image.IO.Stream).SpareDataType.ToString());
                    ini.AppendLine("BlockCount = 0x" + ((NANDImageStream)image.IO.Stream).BlockCount.ToString("X"));
                    if (image.CurrentFileSystem != null)
                        ini.AppendLine("BlockOffset = 0x" + image.CurrentFileSystem.BlockOffset.ToString("X"));
                    ini.AppendLine();
                    ini.AppendLine("[ConsoleSpecific]");
                    // TODO: this



                    Bootloader2BL bl2 = null;
                    for( int blI=0; blI< image.Bootloaders.Count -1; blI++)
                    {
                        if (image.Bootloaders[blI].GetType() == typeof(Bootloader2BL) && image.Bootloaders[blI + 1].GetType() != typeof(Bootloader2BL))
                        {
                            bl2 = (Bootloader2BL)image.Bootloaders[blI];
                        }
                    }
                    
                    Bootloader6BL bl6= null;
                    if(image.Bootloaders.Count>3)
                        bl6= (Bootloader6BL)image.Bootloaders.Find(s => s.GetType() == typeof(Bootloader6BL));

                    ini.AppendLine("2BLPairing = 0x" + Shared.BytesToHexString(bl2.PerBoxData.PairingData, ""));
                    ini.AppendLine("2BLLDV = 0x" + bl2.PerBoxData.LockDownValue.ToString("X"));

                    ini.AppendLine("6BLPairing = 0x"+ ((bl6!=null)?(Shared.BytesToHexString(bl6.PerBoxData.PairingData, "")):"000000"));
                    ini.AppendLine("6BLLDV = 0x" + ((bl6!=null)?(bl6.PerBoxData.LockDownValue.ToString("X")):"00"));
                    ini.AppendLine("SMC = SMC_dec.bin");
                    ini.AppendLine("SMCConfig = SMC_Config.bin");
                    ini.AppendLine("KeyVault = KV_dec.bin");
                    ini.AppendLine("ConsoleType = " + getConsoleType(image));
                    // TODO: this
                    //ini.AppendLine("AltKeyVault = AltKV_dec.bin");
                    foreach (MobileXFile mob in image.MobileData)
                    {
                        ini.AppendLine(mob.Type.ToString() + " = " + mob.Type.ToString() + ".bin");
                    }
                    ini.AppendLine();
                    ini.AppendLine("[Bootloaders]");


                    int count = 0;
                    foreach (Bootloader bl in image.Bootloaders)
                    {
                        //Console.WriteLine("Found Bld: " + bl.GetType().ToString());

                        //only add one pair of CF/CG
                        if (bl.GetType() == typeof(Bootloader6BL) && image.Bootloaders[count - 1].GetType() == typeof(Bootloader7BL)) break;

                        if (bl.GetType() != typeof(Bootloader1BL) && !(bl.GetType() == typeof(Bootloader2BL) && count > 0))
                        {
                            if (bl.GetType() == typeof(Bootloader2BL))
                            {
                                if (image.Bootloaders[count + 1].GetType() == typeof(Bootloader2BL))
                                {
                                    ini.AppendLine(bl.GetType().Name.Replace("Bootloader", "") + " = " + bl.Magic.ToString() + "_A." + bl.Build.ToString() + ".bin:" + bl.Magic.ToString() + "_B." + bl.Build.ToString() + ".bin");
                                }
                                else ini.AppendLine(bl.GetType().Name.Replace("Bootloader", "") + " = " + bl.Magic.ToString() + "_A." + bl.Build.ToString() + ".bin");
                                {

                                }
                            }

                            else ini.AppendLine(bl.GetType().Name.Replace("Bootloader", "") + " = " + bl.Magic.ToString() + "." + bl.Build.ToString() + ".bin");

                        }
                        count++;
                    }
                    ini.AppendLine();
                    ini.AppendLine("[BadBlocks]");
                    for (short y = 0; y < ((NANDImageStream)image.IO.Stream).BlockCount; y++)
                    {
                        ISpareData spare = ((NANDImageStream)image.IO.Stream).GetBlockSpare(y);
                        if (spare.BadBlock)
                        {
                            ini.AppendLine(y.ToString("X") + " = bad");
                        }
                    }
                    File.WriteAllText(Path.Combine(folder, "imageinfo.ini"), ini.ToString());
                }
                if (i == 'v')
                {
                    if (image.KeyVault == null)
                        PrintError("No valid KeyVault in image.");
                    KeyVault kv = image.KeyVault;
                    Console.WriteLine("* Extracting KV");
                    File.WriteAllBytes(Path.Combine(folder, "kv.bin"), kv.GetData(false));
                    File.WriteAllBytes(Path.Combine(folder, "kv_dec.bin"), kv.GetData(true));
                }
                if (i == 's')
                {
                    if (image.SMC == null)
                        PrintError("No valid SMC in image");
                    SMC smc = image.SMC;
                    Console.WriteLine("* Extracting SMC");
                    File.WriteAllBytes(Path.Combine(folder, "smc.bin"), smc.GetData(false));
                    File.WriteAllBytes(Path.Combine(folder, "smc_dec.bin"), smc.GetData(true));
                    if (image.ConfigBlock != null)
                        File.WriteAllBytes(Path.Combine(folder, "smc_config.bin"), image.ConfigBlock);
                }
                if (i == 'm')
                {
                    if (image.MobileData == null)
                        PrintError("No valid mobile data in image");
                    foreach (MobileXFile mfile in image.MobileData)
                    {
                        Console.WriteLine("* Extracting {0}.bin size 0x{1:X} from page 0x{2:X}", mfile.Type, mfile.Length, mfile.StartPage);
                        File.WriteAllBytes(Path.Combine(folder, mfile.Type + ".bin"), mfile.GetData());
                    }
                }
            }
            
        }


        public static string calculate_crc32(string file, string offset)
        {
            if (String.IsNullOrEmpty(offset))
                offset = "0";
            byte[] data = File.ReadAllBytes(file);
            byte[] crc = new Crc32().ComputeHash(data, int.Parse(offset), data.Length - int.Parse(offset));
 
            return Shared.BytesToHexString(crc, "");
        }

        public static void print_Notice()
        {
            Console.WriteLine("\n----=== NOTICE ===----");
            Console.WriteLine("READ THE README");
            Console.WriteLine("THIS IS A BETA!  Please leave feedback on the rgloader forums or in #rgloader!");
            Console.WriteLine("----------------------");

        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            LoadStoredKeys();
            // print help if no arguments or /help is specified)))
            showBanner();
            if (args.Length == 1 && (args[0].ToLower() == "/help" || args[0].ToLower() == "/h" || args[0] == "/?"))
            {
                HelpWriter.ShowFullHelp();
                return;
            }

            // parse the arguments into a nice list
            Arguments = ParseArguments(args, 0);
            string file = "";
            if(Arguments.UnrecognizedOptions.Count > 0)
                file = Arguments.UnrecognizedOptions[0];

            if (Arguments.Recognized.ContainsKey("/banner")) return;

            bool crc32 = Arguments.Recognized.ContainsKey("/crc32");
            if (crc32)
            {
                string offset = Arguments.Recognized["/crc32"];
                if (String.IsNullOrEmpty(offset))
                    offset = "0";
                string file_crc32=calculate_crc32(file, offset);

                File.WriteAllBytes(file + ".crc", Encoding.ASCII.GetBytes(file_crc32));
                System.Diagnostics.Process.GetCurrentProcess().Kill();
            }

            

            if (Arguments.Recognized.Count > 0)
            {
                if(String.IsNullOrEmpty(file))
                    PrintError("You must specify a file.");

                string cpukey;
                if (!Arguments.Recognized.TryGetValue("/cpukey", out cpukey) || String.IsNullOrEmpty(cpukey))
                    PrintError("CPU key needs to be set.");

                bool guided = Arguments.Recognized.ContainsKey("/guided");
                bool create = Arguments.Recognized.ContainsKey("/create");
                bool extract = Arguments.Recognized.ContainsKey("/extract");
                bool extractfile = Arguments.Recognized.ContainsKey("/extractfile");
                bool inject = Arguments.Recognized.ContainsKey("/inject");
                bool injectfile = Arguments.Recognized.ContainsKey("/injectfile");
                
                Console.WriteLine("Opening image...");
                // open the nand img
                NANDImage image = new NANDImage();
                string blkey;
                if (Arguments.Recognized.TryGetValue("/1blkey", out blkey) && !String.IsNullOrEmpty(blkey))
                    try
                    {
                        image._1BLKey = Shared.HexStringToBytes(blkey);
                    }
                    catch
                    {
                        PrintError("Invalid 1BL key.");
                    }
                try
                {
                    image.CPUKey = Shared.HexStringToBytes(cpukey);
                }
                catch
                {
                    PrintError("Invalid CPU key.");
                }

                //new guided GUI installer
                if (guided)
                {
                    string NANDDATADIR = "..\\nanddata";
                    string LOADERDIR = "..\\loaders";
                    string PATCHESDIR = "..\\patches";
                    string RLSDIR = "..\\rls";
                    string BINDIR = "..\\bin";


                    string defaultsDir = "..\\builds\\defaults";
                    string buildDir = "..\\builds";
                    string xellBin = "xell-gggggg.bin";
                    string xellPath = "..\\" + xellBin;
                    string FSfileloc = "..\\Filesystems";

                    string jtagDir = "..\\jtag";

                    string phatINI = "fat.ini";
                    string phatRGH2INI = "fatRGH2.ini";
                    string slimINI = "slim.ini"; //path gets added after buildDir is initialized
                    string jtagINI = "jtag.ini";
                    string bldrINI = "bootloaders.ini";

                    string buildINI = ""; //selected ini (phatINI for phat, slimINI for slim, etc..)


                    string exploitType = "RGH";
                    string buildType = "-dev";
                    string buildVer = "";

                    string build = "";

                    string outputFile = "..\\Image."; //output image will be Image.14699-dev.bin (initialized after buildVer)

                    string loaderpatchpath = BINDIR + "\\loaderpatch.exe";

                    bool SMCpatched = false;
                    bool phatPrecompiledCD = true;
                    bool generateDummyKV = true;

                    List<string> extraPatches = new List<string>();

                    FileIniDataParser parser = null;
                    IniData parsedData = null;
                    try
                    {
                        parser = new FileIniDataParser();
                        parsedData = parser.LoadFile("..\\options.ini");
                    }
                    catch { parsedData = null; }

                    if (parsedData != null)
                    {

                        //options
                        if (!String.IsNullOrEmpty(parsedData["Options"]["Phat_Precompiled_CD"]))
                            phatPrecompiledCD = (parsedData["Options"]["Phat_Precompiled_CD"] == "yes") ? (true) : false;
                        if (!String.IsNullOrEmpty(parsedData["Options"]["Generate_Dummy_KV"]))
                            generateDummyKV = (parsedData["Options"]["Generate_Dummy_KV"] == "yes") ? (true) : false;

                        //patches
                        foreach (KeyData kd in parsedData["Patches"])
                        {
                            if (kd.Value == "1" || kd.Value == "true" || kd.Value == "yes")
                            {
                                extraPatches.Add(kd.KeyName);
                            }
                        }


                        //directories
                        if (!String.IsNullOrEmpty(parsedData["Directories"]["NAND_data"]))
                            NANDDATADIR = parsedData["Directories"]["NAND_data"];
                        if (!String.IsNullOrEmpty(parsedData["Directories"]["loaders"]))
                            LOADERDIR = parsedData["Directories"]["loaders"];
                        if (!String.IsNullOrEmpty(parsedData["Directories"]["patches"]))
                            PATCHESDIR = parsedData["Directories"]["patches"];
                        if (!String.IsNullOrEmpty(parsedData["Directories"]["release"]))
                            RLSDIR = parsedData["Directories"]["release"];
                        if (!String.IsNullOrEmpty(parsedData["Directories"]["bin"]))
                            BINDIR = parsedData["Directories"]["bin"];
                        if (!String.IsNullOrEmpty(parsedData["Directories"]["defaults"]))
                            defaultsDir = parsedData["Directories"]["defaults"];
                        if (!String.IsNullOrEmpty(parsedData["Directories"]["build"]))
                            buildDir = parsedData["Directories"]["build"];
                        if (!String.IsNullOrEmpty(parsedData["Directories"]["filesystems"]))
                            FSfileloc = parsedData["Directories"]["filesystems"];

                        //xell section
                        if (!String.IsNullOrEmpty(parsedData["Xell"]["xell_binary"]))
                        {
                            xellBin = parsedData["Xell"]["xell_binary"];
                            xellPath = "..\\" + xellBin;
                        }
                        if (!String.IsNullOrEmpty(parsedData["Xell"]["xell_path"]))
                        {
                            xellPath = parsedData["Xell"]["xell_path"];
                            xellPath = xellPath + xellBin;
                        }

                        //default inis
                        if (!String.IsNullOrEmpty(parsedData["Default_INI"]["fat"]))
                            phatINI = parsedData["Default_INI"]["fat"];
                        if (!String.IsNullOrEmpty(parsedData["Default_INI"]["fatRGH2"]))
                            phatRGH2INI = parsedData["Default_INI"]["fatRGH2"];
                        if (!String.IsNullOrEmpty(parsedData["Default_INI"]["slim"]))
                            slimINI = parsedData["Default_INI"]["slim"];
                        if (!String.IsNullOrEmpty(parsedData["Default_INI"]["jtag"]))
                            jtagINI = parsedData["Default_INI"]["jtag"];

                        //output location
                        if (!String.IsNullOrEmpty(parsedData["Output"]["output_file_location"]))
                        {
                            outputFile = parsedData["Output"]["output_file_location"] + "Image.";
                        }
                    }


                    /*Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new setup());*/

                    //Start Message
                    if (!Arguments.Recognized.ContainsKey("/kernel") || !Arguments.Recognized.ContainsKey("/hack"))
                    {
                        print_Notice();
                        pause();
                    }



                    if (Arguments.Recognized.TryGetValue("/kernel", out buildVer))
                    {
                        buildVer = Arguments.Recognized["/kernel"];
                        //if (!Lists.KernelVersions.Contains((ushort)Convert.ToUInt32(buildVer)))
                        //    buildVer = "";
                    }

                    //get info from user

                    while (buildVer == "")
                    {
                        Console.WriteLine("\n              Please select the version you want to build    ");
                        int i = 1;
                        foreach (ushort ver in Lists.KernelVersions)
                        {
                            Console.WriteLine("(" + i + ") " + Lists.KernelVersions[i - 1]);
                            i++;
                        }

                        int numSelections = Lists.KernelVersions.Count;
                        try
                        {
                            buildVer = Console.ReadLine();

                            int selection = Convert.ToInt32(buildVer);
                            if (selection < 1 || selection > numSelections)
                            {
                                buildVer = "";
                                Console.WriteLine("\nInvalid kernel version\n");
                            }
                            else
                            {
                                buildVer = Convert.ToString(Lists.KernelVersions[selection - 1]);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("Invalid selection! \n");
                            buildVer = "";
                        }
                    }






                    if (Arguments.Recognized.ContainsKey("/hack"))
                    {
                        exploitType = Arguments.Recognized["/hack"];
                        if (!Lists.ExploitTypes.Contains(exploitType))
                            exploitType = "";
                    }
                    else exploitType = "";


                    if (exploitType == "")
                    {
                        int numSelections = 2; //max number of selections
                        int selection = -1;
                        while (selection < 1 || selection > numSelections)
                        {
                            Console.WriteLine("\n              Please select the exploit type    ");
                            Console.WriteLine("(1) RGH");
                            Console.WriteLine("(2) JTAG");

                            //Console.Write("\n=");
                            try
                            {
                                selection = Convert.ToInt16(Console.ReadLine());

                                if (selection < 1 || selection > numSelections)
                                {
                                    selection = -1;
                                    Console.WriteLine("\nInvalid selection\n");
                                }
                                else
                                {
                                    exploitType = Convert.ToString(Lists.ExploitTypes[selection - 1]);
                                }
                            }
                            catch
                            {
                                Console.WriteLine("Invalid selection! \n");
                                selection = -1;
                            }
                        }
                    }

                    build = buildVer + buildType;
                    buildDir += "\\" + build;

                    outputFile += build + ".bin";

                    FSfileloc += "\\" + build;

                    if (!File.Exists(FSfileloc + "\\xam.xex"))
                    {
                        PrintError("=== UNABLE TO FIND FILESYSTEM FILES FOR THIS BUILD ===");
                    }

                    //init folders
                    if (!Directory.Exists("..\\tmp")) Directory.CreateDirectory("..\\tmp");
                    if (!Directory.Exists("..\\rls")) Directory.CreateDirectory("..\\rls");
                    if (!Directory.Exists(LOADERDIR + "\\SMC\\")) Directory.CreateDirectory(LOADERDIR + "\\SMC\\");
                    if (!Directory.Exists(LOADERDIR + "\\2BL\\")) Directory.CreateDirectory(LOADERDIR + "\\2BL\\");
                    if (!Directory.Exists(LOADERDIR + "\\3BL\\")) Directory.CreateDirectory(LOADERDIR + "\\3BL\\");
                    if (!Directory.Exists(LOADERDIR + "\\4BL\\")) Directory.CreateDirectory(LOADERDIR + "\\4BL\\");
                    if (!Directory.Exists(LOADERDIR + "\\5BL\\")) Directory.CreateDirectory(LOADERDIR + "\\5BL\\");
                    if (!Directory.Exists(LOADERDIR + "\\6BL\\")) Directory.CreateDirectory(LOADERDIR + "\\6BL\\");
                    if (!Directory.Exists(LOADERDIR + "\\7BL\\")) Directory.CreateDirectory(LOADERDIR + "\\7BL\\");

                    //Open image
                    bool opened = image.OpenImage(file, 0x200);
                    if (!opened)
                        PrintError(String.Format("Can't open image {0}", file));

                    Bootloader CB = null, CD = null, CE = null;

                    foreach (Bootloader bl in image.Bootloaders)
                    {
                        if (bl.GetType() == typeof(Bootloader2BL) && CB == null) CB = bl;
                        if (bl.GetType() == typeof(Bootloader4BL) && CD == null) CD = bl;
                        if (bl.GetType() == typeof(Bootloader5BL) && CE == null) CE = bl;
                    }

                    Console.WriteLine("\n*** Extracting data..\n");

                    //init nand data dir & extract nand data
                    if (System.IO.Directory.Exists(NANDDATADIR)) System.IO.Directory.Delete(NANDDATADIR, true);
                    System.IO.Directory.CreateDirectory(NANDDATADIR);
                    extractImage(image, "bvfsmi", NANDDATADIR);
                    //calculate crc32 of smc
                    Console.WriteLine("*** Calculating SMC crc32\n");
                    string smc_crc32 = calculate_crc32(NANDDATADIR + "\\SMC_dec.bin", "4");

                    //get info
                    string consoleType = getConsoleType(image);
                    Console.WriteLine("*** Extraction complete..\n");
                    foreach (Bootloader bl in image.Bootloaders)
                    {
                        if (bl.GetType() != typeof(Bootloader1BL))
                        {
                            string magic = bl.Magic.ToString();
                            if (bl.GetType() == typeof(Bootloader2BL))
                            {
                                magic += "_";
                                if (((Bootloader2BL)bl).CPUKey == null || ((Bootloader2BL)bl).CPUKey[0] == 0 && ((Bootloader2BL)bl).CPUKey[1] == 0 && ((Bootloader2BL)bl).CPUKey[2] == 0 && ((Bootloader2BL)bl).CPUKey[3] == 0)
                                    magic += "A";
                                else
                                    magic += "B";
                            }
                            if(!File.Exists(LOADERDIR + "\\" + (bl.GetType().Name.Replace("Bootloader", "")) + "\\" + magic + "." + bl.Build.ToString() + ".bin"))
                            {
                                Console.WriteLine("* Moving " + magic + " (ver " + bl.Build + ") to loaders directory");
                                File.Copy(Path.Combine(NANDDATADIR, magic + "." + bl.Build.ToString() + ".bin"),
                                          Path.Combine(LOADERDIR + "\\" + (bl.GetType().Name.Replace("Bootloader", "")) +
                                                       "\\" + magic + "." + bl.Build.ToString() + ".bin"), true);
                            }

                        }
                    }

                    //File.Copy(NANDDATADIR + "\\CB_A." + CB.Build + ".bin", LOADERDIR + "\\2BL\\CB_A." + CB.Build + ".bin", true);
                    //File.Copy(NANDDATADIR + "\\CD." + CD.Build + ".bin", LOADERDIR + "\\4BL\\CD." + CD.Build + ".bin", true);
                    //File.Copy(NANDDATADIR + "\\" + CE.Magic + "." + CE.Build + ".bin", LOADERDIR + "\\5BL\\" + CE.Magic + "." + CE.Build + ".bin", true);

                    if (exploitType == "JTAG" && !Lists.JTAG_CB.Contains(CB.Build))
                    {
                        PrintError("ERROR: CB " + CB.Build + " is not compatible with JTAG!");
                    }
            

                    /*if (exploitType == "RGH" && !RGH_CB.Contains(CB.Build))
                    {
                        if (Lists.TRINITY_CB.Contains(CB.Build))
                        {
                            CB.Build = 9188;
                            CD.Build = 9452;
                        }

                        else
                        {
                            PrintError("ERROR: CB " + CB.Build + " is not compatible with RGH!");
                        }
                    }*/

                    //load bootloader data from ini
                    if(exploitType!="JTAG"){

                        parser = new FileIniDataParser();
                        parsedData = parser.LoadFile(defaultsDir + "\\" + bldrINI);

                        if (parsedData == null) PrintError("Unable to parse bootloaders ini: " + defaultsDir + "\\" + bldrINI);

                        //Console.WriteLine("Consoletype: " + consoleType);
                        if (!String.IsNullOrEmpty(parsedData[consoleType + exploitType]["2BL"]))
                        {
                            string[] split = parsedData[consoleType + exploitType]["2BL"].Split(new[] { ":" }, StringSplitOptions.None);
                            byte[] data;

                            string loadersCB = LOADERDIR + "\\2BL\\";

                            if (split.Count() > 1)
                            {
                                if (File.Exists(loadersCB + split[1]))
                                {
                                    data = File.ReadAllBytes(loadersCB + split[1]);
                                    CB.SetData(data);
                                }
                            }
                            else
                            {
                                if (File.Exists(loadersCB + split[0]))
                                {
                                    data = File.ReadAllBytes(loadersCB + split[0]);
                                    CB.SetData(data);
                                }
                            }
                        }

                        if (!String.IsNullOrEmpty(parsedData[consoleType + exploitType]["4BL"]))
                        {
                            string cdPath = parsedData[consoleType + exploitType]["4BL"];
                            byte[] data;

                            string loadersCD = LOADERDIR + "\\4BL\\";

                            if (File.Exists(loadersCD + cdPath))
                            {
                                data = File.ReadAllBytes(loadersCD + cdPath);
                                CD.SetData(data);
                            }
                        }
                    }
                   

                    //init builds folder & copy default files
                    if (System.IO.Directory.Exists(buildDir)) System.IO.Directory.Delete(buildDir, true);
                        System.IO.Directory.CreateDirectory(buildDir);
                    //System.IO.Directory.CreateDirectory(buildDir);

                    if (consoleType == "Trinity" || consoleType == "Corona") buildINI = slimINI;
                    else if (exploitType == "JTAG") buildINI = jtagINI;
                    else if (exploitType == "RGH2") buildINI = phatRGH2INI;
                    else buildINI = phatINI;

                    if (File.Exists(defaultsDir + "\\" + build + "\\" + buildINI)) File.Copy(defaultsDir + "\\" + build + "\\" + buildINI, buildDir + "\\" + buildINI);
                    else PrintError("Unable to copy build ini from: "+defaultsDir + "\\" + build + "\\" + buildINI);

                    if (File.Exists(defaultsDir + "\\" + bldrINI)) File.Copy(defaultsDir + "\\" + bldrINI, buildDir + "\\" + bldrINI);
                    //copyDir(defaultsDir + "\\" + build, buildDir);

                    phatINI = buildDir + "\\" + phatINI;
                    phatRGH2INI = buildDir + "\\" + phatRGH2INI;
                    slimINI = buildDir + "\\" + slimINI;
                    jtagINI = buildDir + "\\" + jtagINI;
                    bldrINI = buildDir + "\\" + bldrINI;
                    buildINI = buildDir + "\\" + buildINI;







                    //copy jtag bootloaders & binaries
                    if (exploitType == "JTAG")
                    {
                        xellBin = "xell-2f.bin";
                        xellPath = "..\\" + xellBin;

                        Console.WriteLine("\n*** Copying jtag bootloaders");
                        string jtag2BL = "";
                        string jtag2BL_hacked = "";
                        string jtag4BL = "";
                        string jtag4BL_xdk = "";
                        string jtag5BL = "";
                        string jtag6BL = "";
                        string jtag7BL = "";

                        //try
                        // {
                        parser = new FileIniDataParser();
                        parsedData = parser.LoadFile(jtagINI);

                        if (parsedData == null) PrintError("Unable to parse jtag ini at: " + jtagINI);

                        //Console.WriteLine("Consoletype: " + consoleType);
                        if (!String.IsNullOrEmpty(parsedData[consoleType]["2BL"]))
                            jtag2BL = parsedData[consoleType]["2BL"];
                        if (!String.IsNullOrEmpty(parsedData[consoleType]["jtag2BL"]))
                            jtag2BL_hacked = parsedData[consoleType]["jtag2BL"];
                        if (!String.IsNullOrEmpty(parsedData[consoleType]["4BL"]))
                            jtag4BL = parsedData[consoleType]["4BL"];
                        if (!String.IsNullOrEmpty(parsedData["Bootloaders"]["4BL"]))
                            jtag4BL_xdk = parsedData["Bootloaders"]["4BL"];
                        if (!String.IsNullOrEmpty(parsedData[consoleType]["5BL"]))
                            jtag5BL = parsedData[consoleType]["5BL"];

                        if (!String.IsNullOrEmpty(parsedData[consoleType]["6BL"]))
                            jtag6BL = parsedData[consoleType]["6BL"];
                        if (!String.IsNullOrEmpty(parsedData[consoleType]["7BL"]))
                            jtag7BL = parsedData[consoleType]["7BL"];



                        if (File.Exists(LOADERDIR + "\\2BL\\" + jtag2BL)) File.Copy(LOADERDIR + "\\2BL\\" + jtag2BL, buildDir + "\\" + jtag2BL, true);
                        if (File.Exists(LOADERDIR + "\\2BL\\" + jtag2BL_hacked)) File.Copy(LOADERDIR + "\\2BL\\" + jtag2BL_hacked, buildDir + "\\" + jtag2BL_hacked, true);
                        if (File.Exists(LOADERDIR + "\\4BL\\" + jtag4BL)) File.Copy(LOADERDIR + "\\4BL\\" + jtag4BL, buildDir + "\\" + jtag4BL, true);
                        if (File.Exists(LOADERDIR + "\\4BL\\" + jtag4BL_xdk)) File.Copy(LOADERDIR + "\\4BL\\" + jtag4BL_xdk, buildDir + "\\" + jtag4BL_xdk, true);
                        if (File.Exists(LOADERDIR + "\\5BL\\" + jtag5BL)) File.Copy(LOADERDIR + "\\5BL\\" + jtag5BL, buildDir + "\\" + jtag5BL, true);
                        if (File.Exists(LOADERDIR + "\\6BL\\" + jtag6BL)) File.Copy(LOADERDIR + "\\6BL\\" + jtag6BL, buildDir + "\\" + jtag6BL, true);
                        if (File.Exists(LOADERDIR + "\\7BL\\" + jtag7BL)) File.Copy(LOADERDIR + "\\7BL\\" + jtag7BL, buildDir + "\\" + jtag7BL, true);

                        //copy jtag binaries
                        if (System.IO.Directory.Exists(jtagDir)) copyDir(jtagDir, buildDir);

                        string jtagpatchset = PATCHESDIR + "\\JTAG\\" + build + "\\jtag_patchset_" + consoleType + ".rglp";

                        if (!File.Exists(jtagpatchset)) PrintError("Unable to load jtag patchset for " + consoleType + " at " + jtagpatchset);
                        else File.Copy(jtagpatchset, buildDir + "\\jtag_patchset.rglp", true);
                    }

                    //patch SMC
                    {

                        string tmpsmc = "..\\tmp" + "\\SMC." + smc_crc32 + ".bin";
                        string smc_patch = PATCHESDIR + "\\SMC\\";
                        
                        if (exploitType == "JTAG") smc_patch += "JTAG\\" + smc_crc32 + ".txt";
                        else smc_patch += smc_crc32 + ".txt";

                        File.Copy(NANDDATADIR + "\\smc_dec.bin", LOADERDIR + "\\SMC\\SMC." + smc_crc32 + ".bin", true);
                        
                        byte[] SMC_data = File.ReadAllBytes(NANDDATADIR + "\\smc_dec.bin");
                        for (int i = 0; i < SMC_data.Length - 8; i++)
                        {
                            if (SMC_data[i] == 0x05)
                            {
                                if (SMC_data[i + 2] == 0xE5 && SMC_data[i + 4] == 0xB4 && SMC_data[i + 5] == 0x05)
                                {
                                    SMCpatched = true;
                                    SMC_data[i] = 0;
                                    SMC_data[i + 1] = 0;
                                }
                            }
                        }

                        File.WriteAllBytes(buildDir + "\\SMC_dec.bin", SMC_data);

                        if (!SMCpatched) Console.WriteLine("   ERROR: Unable to patch this SMC");
                        else Console.WriteLine("*** SMC Patch completed!");
                            
                    }//~patch SMC

                    //Copy devkit SE
                    {
                        string CEloc = LOADERDIR + "\\5BL\\SE." + buildVer + ".bin";
                        Console.WriteLine("\n*** Adding CE " + buildVer);
                        if (!File.Exists(CEloc))
                        {
                            PrintError("\n\nUnable to find compressed kernel (CE/SE) for " + buildVer + "! \nMake sure its located at " + CEloc);

                        }
                        File.Copy(CEloc, buildDir + "\\SE." + buildVer + ".bin", true);
                    }

                    //Copy xell binary to builds
                    if (File.Exists(xellPath))
                    {
                        Console.WriteLine("\n*** Adding xell binary");
                        File.Copy(xellPath, buildDir + "\\" + xellBin);
                    }
                    else
                    {
                        Console.WriteLine("\n ERROR: Unable to find xell binary! Skipping..");
                    }

                    try
                    {
                        //copy filesystem files
                        {
                            if (!System.IO.Directory.Exists(FSfileloc))
                            {
                                PrintError("Unable to locate filesystem files at: " + FSfileloc);
                            }
                            else
                            {
                                Console.WriteLine("\n*** Copying filesystem files from " + FSfileloc);
                                copyDir(FSfileloc, buildDir + "\\files");
                            }
                        }

                        //copy kv,smc_config
                        File.Copy(NANDDATADIR + "\\smc_config.bin", buildDir + "\\smc_config.bin", true);
                        File.Copy(NANDDATADIR + "\\KV_dec.bin", buildDir + "\\KV_dec.bin", true);

                        //copy mobiledata
                        copyFile(NANDDATADIR + "\\MobileB.bin", buildDir + "\\MobileB.bin");
                        copyFile(NANDDATADIR + "\\MobileC.bin", buildDir + "\\MobileC.bin");
                        copyFile(NANDDATADIR + "\\MobileE.bin", buildDir + "\\MobileE.bin");
                        copyFile(NANDDATADIR + "\\MobileJ.bin", buildDir + "\\MobileJ.bin");

                        //copy filesystem files
                        copyFile(NANDDATADIR + "\\files\\crl.bin", buildDir + "\\files\\crl.bin");
                        if (!copyFile(NANDDATADIR + "\\files\\fcrt.bin", buildDir + "\\files\\fcrt.bin"))
                        {
                            if ((consoleType == "Trinity" || consoleType == "Corona") && !extraPatches.Contains("NOFCRT"))
                            {
                                Console.WriteLine("\n   * Missing FCRT.bin, will attempt to patch out FCRT.");
                                extraPatches.Add("NOFCRT");
                            }
                        }
                        copyFile(NANDDATADIR + "\\files\\dae.bin", buildDir + "\\files\\dae.bin");
                        copyFile(NANDDATADIR + "\\files\\secdata.bin", buildDir + "\\files\\secdata.bin");
                        copyFile(NANDDATADIR + "\\files\\extended.bin", buildDir + "\\files\\extended.bin");

                        //copy imageinfo ini
                        if (File.Exists(NANDDATADIR + "\\imageinfo.ini")) File.Copy(NANDDATADIR + "\\imageinfo.ini", buildDir + "\\imageinfo.ini", true);


                    }
                    catch (System.IO.FileNotFoundException e)
                    {
                        PrintError("Unable to find " + e.FileName);
                    }


                    //Generate Dummy KV
                    if (generateDummyKV)
                    {
                        Console.WriteLine("\n*** Generating dummy dev KeyVault");
                        string dummyKVpath = defaultsDir + "\\KV_generic.bin";
                        if (File.Exists(dummyKVpath))
                        {
                            byte[] data = File.ReadAllBytes(dummyKVpath);
                            if (data.Length == 0x4000 || data.Length == 0x3ff0)
                            {

                                byte[] data2 = new byte[0x3ff0];
                                Array.Copy(data, data.Length == 0x4000 ? 0x10 : 0x0, data2, 0, 0x3ff0);

                                MemoryStream stream = new MemoryStream(new byte[0x40000]);
                                X360IO kvIO = new X360IO(stream, true);
                                KeyVault dummyKV = new KeyVault(kvIO, image.CPUKey);

                                dummyKV.SetData(data);

                                //transfer data to dummy KV
                                //dummyKV.ConsoleCertificate.ConsoleId = image.KeyVault.ConsoleCertificate.ConsoleId;
                                //dummyKV.ConsoleCertificate.ConsolePartNumber = image.KeyVault.ConsoleCertificate.ConsolePartNumber;
                                dummyKV.ConsoleCertificate.ConsoleType = image.KeyVault.ConsoleCertificate.ConsoleType;
                                dummyKV.ConsoleCertificate.ManufacturingDate = image.KeyVault.ConsoleCertificate.ManufacturingDate;
                                //dummyKV.ConsoleCertificate.Privileges = image.KeyVault.ConsoleCertificate.Privileges;
                                dummyKV.DVDKey = image.KeyVault.DVDKey;
                                dummyKV.GameRegion = image.KeyVault.GameRegion;
                                dummyKV.XeikaCertificate.DriveInquiry = image.KeyVault.XeikaCertificate.DriveInquiry;

                                dummyKV.XeikaCertificate.DrivePhaseLevel = image.KeyVault.XeikaCertificate.DrivePhaseLevel;
                                dummyKV.XeikaCertificate.OSIGdesc = image.KeyVault.XeikaCertificate.OSIGdesc;
                                dummyKV.XeikaCertificate.ODDDataVersion = image.KeyVault.XeikaCertificate.ODDDataVersion;
                                dummyKV.OSIG = image.KeyVault.OSIG;

                                File.WriteAllBytes(buildDir + "\\KV_dec.bin", dummyKV.GetData(true));

                            }
                            else
                            {
                                Console.WriteLine("ERROR: Unable to load dummy_KV file from: " + dummyKVpath);
                            }
                        }
                        else
                        {
                            Console.WriteLine("ERROR: Unable to load dummy_KV file from: " + dummyKVpath);
                        }
                    }
                    else Console.WriteLine("\n*** NOT Generating dummy dev KeyVault!");


                    if (exploitType != "JTAG")
                    {
                        //compile KHV patch

                        string KHV = "RGLoader-" + build;
                        string KHVpatch = PATCHESDIR + "\\KHV\\" + build + "\\" + KHV;


                        {
                            string xenon_as = BINDIR + "\\xenon-as.exe";
                            string xenon_objcopy = BINDIR + "\\xenon-objcopy.exe";
                            string KHVpatchtmp = "..\\tmp\\RGLoader-" + build;

                            if (File.Exists(KHVpatchtmp + ".rglp")) File.Delete(KHVpatchtmp + ".rglp");
                            if (File.Exists(KHVpatchtmp + ".elf")) File.Delete(KHVpatchtmp + ".elf");

                            //File.Copy(KHVpatch + ".txt", KHVpatchtmp + ".txt", true);
                            Console.WriteLine("\n*** Compiling KHV Patches for " + build + "...");

                            ProcessStartInfo pInfo = new ProcessStartInfo();
                            pInfo.FileName = @xenon_as;
                            pInfo.WorkingDirectory = Path.GetDirectoryName(KHVpatch);
                            string symdef = "";
                            foreach (string str in extraPatches)
                            {
                                symdef += "--defsym " + str + "=1 ";
                            }

                            pInfo.Arguments = " -be " + symdef + "-many " + KHV + ".S -o " + "..\\..\\" + KHVpatchtmp + ".elf";
                            pInfo.CreateNoWindow = false;
                            pInfo.UseShellExecute = false;
                            System.Diagnostics.Process patcher = Process.Start(pInfo);
                            patcher.WaitForExit();

                            if (!File.Exists(KHVpatchtmp + ".elf")) PrintError(KHVpatchtmp + ".elf did not assemble, cannot proceed.");

                            pInfo.WorkingDirectory = ".\\";
                            pInfo.FileName = @xenon_objcopy;
                            pInfo.Arguments = KHVpatchtmp + ".elf -O binary " + KHVpatchtmp + ".rglp";
                            pInfo.CreateNoWindow = false;
                            pInfo.UseShellExecute = false;
                            patcher = Process.Start(pInfo);
                            patcher.WaitForExit();

                            if (File.Exists(KHVpatchtmp + ".elf")) File.Delete(KHVpatchtmp + ".elf");

                            if (!File.Exists(KHVpatchtmp + ".rglp") || File.ReadAllBytes(KHVpatchtmp + ".rglp").Length <= 0) PrintError(KHVpatchtmp + ".rglp did not build, cannot proceed.");
                            else
                            {
                                if (File.Exists(buildDir + "\\patchset.rglp")) File.Delete(buildDir + "\\patchset.rglp");
                                if (File.Exists(RLSDIR + "\\RGLoader-" + build + ".rglp")) File.Delete(RLSDIR + "\\RGLoader-" + build + ".rglp");

                                File.Move(KHVpatchtmp + ".rglp", RLSDIR + "\\RGLoader-" + build + ".rglp");
                                File.Copy(RLSDIR + "\\RGLoader-" + build + ".rglp", buildDir + "\\patchset.rglp", true);

                                Console.WriteLine("*** KHV Patch build completed!");
                            }
                        }


                        //compile/patch files

                        string CBAPath = LOADERDIR + "\\2BL\\CB_A." + CB.Build + ".bin";
                        string CBBPath = null;
                        string CDPath = LOADERDIR + "\\4BL\\CD." + CD.Build + ".bin";

                        //Build CB_B patches
                        if (consoleType == "Trinity" || consoleType == "Corona" || exploitType == "RGH2")
                        {
                            string CBB = "CB_B." + CB.Build + ".bin";
                            string CBBPatch = PATCHESDIR + "\\2BL\\RGLoader-" + CB.Build + "_B-base.txt";
                            string patchpath = "..\\tmp" + "\\RGLoader-" + CB.Build + "_B-base.txt";
                            string tmpCBB = "..\\tmp\\" + CBB;

                            if (!File.Exists(CBBPatch))
                            {
                                Console.WriteLine("Unable to find patches for CB_B " + CB.Build);
                                File.Copy(NANDDATADIR + "\\CB_B." + CB.Build + ".bin", buildDir + "\\CB_B." + CB.Build + ".bin", true);

                            }
                            else
                            {
                                //File.Copy(LOADERDIR + "\\2BL\\CB_B." + CB.Build + ".bin", LOADERDIR + "\\2BL\\CB_B." + CB.Build + ".bin", true);
                                File.Copy(LOADERDIR + "\\2BL\\CB_B." + CB.Build + ".bin", tmpCBB, true);
                                File.Copy(CBBPatch, patchpath, true);
                                if (File.Exists(tmpCBB + ".new")) File.Delete(tmpCBB + ".new");

                                Console.WriteLine("\n*** Patching CB_B " + CB.Build + "...");
                                {

                                    ProcessStartInfo pInfo = new ProcessStartInfo();
                                    pInfo.FileName = @loaderpatchpath;
                                    pInfo.Arguments = " " + patchpath + " " + tmpCBB;
                                    pInfo.CreateNoWindow = false;
                                    pInfo.UseShellExecute = false;
                                    System.Diagnostics.Process patcher = Process.Start(pInfo);
                                    patcher.WaitForExit();
                                }

                                string patchedCBB = tmpCBB + ".new";
                                if (!File.Exists(patchedCBB))
                                {
                                    PrintError("Unable to patch CB_B " + CB.Build);

                                }
                                else
                                {
                                    File.Copy(patchedCBB, RLSDIR + "\\CB_B." + CB.Build + "_new.bin", true);
                                    File.Copy(patchedCBB, buildDir + "\\" + CBB, true);
                                    Console.WriteLine("*** CB_B Patch completed!");
                                }
                                CBBPath = buildDir + "\\" + CBB;
                            }
                        }

                        //build CD/4bl patches
                        if (consoleType == "Trinity" || consoleType == "Corona" || !phatPrecompiledCD || exploitType == "RGH2")
                        {
                            string _CD = "CD." + CD.Build + ".bin";
                            string CDBasePatch = PATCHESDIR + "\\4BL\\RGLoader-" + CD.Build + "-base.txt";
                            string CDPatch = PATCHESDIR + "\\4BL\\RGLoader-" + CD.Build + "-lfn.txt";

                            string basepatchpath = "..\\tmp" + "\\RGLoader-" + CD.Build + "-base.txt";
                            string patchpath = "..\\tmp" + "\\RGLoader-" + CD.Build + "-lfn.txt";
                            string tmpCD = "..\\tmp\\" + _CD;

                            if (!File.Exists(CDPatch))
                            {
                                Console.WriteLine("Unable to find patches for CD " + CD.Build);
                                File.Copy(LOADERDIR + "\\4BL\\" + _CD, buildDir + "\\" + _CD, true);

                            }
                            else
                            {
                                //File.Copy(NANDDATADIR + "\\" + _CD, LOADERDIR + "\\4BL\\" + _CD, true);
                                File.Copy(LOADERDIR + "\\4BL\\" + _CD, tmpCD, true);
                                File.Copy(CDPatch, patchpath, true);
                                File.Copy(CDBasePatch, basepatchpath, true);

                                if (File.Exists(tmpCD + ".new")) File.Delete(tmpCD + ".new");

                                Console.WriteLine("\n*** Patching 4BL " + CD.Build + "...");
                                {

                                    ProcessStartInfo pInfo = new ProcessStartInfo();
                                    pInfo.FileName = @loaderpatchpath;
                                    pInfo.WorkingDirectory = BINDIR;
                                    pInfo.Arguments = " " + patchpath + " " + tmpCD;
                                    pInfo.CreateNoWindow = false;
                                    pInfo.UseShellExecute = false;
                                    System.Diagnostics.Process patcher = Process.Start(pInfo);
                                    patcher.WaitForExit();
                                }

                                string patchedCD = tmpCD + ".new";
                                if (!File.Exists(patchedCD))
                                {
                                    PrintError("Unable to patch CD " + CD.Build);

                                }
                                else
                                {
                                    File.Copy(patchedCD, RLSDIR + "\\CD." + CD.Build + "_new.bin", true);
                                    File.Copy(patchedCD, buildDir + "\\" + _CD, true);
                                    Console.WriteLine("*** 4BL Patch completed!");
                                }
                                CDPath = buildDir + "\\" + _CD;
                            }
                        }
                        //don't compile CD for phats, copy precompiled
                        else if (consoleType == "Falcon" || consoleType == "Xenon" || consoleType == "Zephyr")
                        {
                            Console.WriteLine("\n*** Using pre-compiled 4bl for " + consoleType);
                            File.Copy(defaultsDir + "\\CD.9452_falcon.bin", buildDir + "\\CD.9452.bin", true);
                        }
                        else if (consoleType == "Jasper")
                        {
                            Console.WriteLine("\n*** Using pre-compiled 4bl for " + consoleType);
                            File.Copy(defaultsDir + "\\CD.9452_jasper.bin", buildDir + "\\CD.9452.bin", true);
                        }

                        //Copy CB_A
                        Console.WriteLine("\n*** Adding CB_A " + CB.Build);
                        File.Copy(LOADERDIR + "\\2BL\\CB_A." + CB.Build + ".bin", buildDir + "\\CB_A." + CB.Build + ".bin", true);
                    }




                    //Create image
                    Console.WriteLine("\n*** Compiling NAND image!");

                    

                    image = null;
                    string ret = LoadFromIni(ref image, buildINI);
                    if (ret != null)
                        PrintError("Creating image failed: " + ret);
                    string savedest = outputFile;
                    image.SaveImage();
                    image.Close();

                    if (((NANDImageStream)image.IO.Stream).TempReturnData != null)
                    {
                        File.WriteAllBytes(savedest, ((NANDImageStream)image.IO.Stream).TempReturnData);
                        ((NANDImageStream)image.IO.Stream).TempReturnData = null;
                    }
                    Console.WriteLine("\nImage created successfully!");




                    //Show finish message

                    showBanner();

                    Console.WriteLine("\n----------=== BUILD INFO ===--------------------------             ");
                    Console.WriteLine("Console Type: " + consoleType);
                    Console.WriteLine("Target Kernel: " + buildVer);
                    Console.WriteLine("Target Kernel Type: " + ((buildType == "-dev") ? ("Devkit") : ("Retail")));


                    Console.WriteLine("CB: " + CB.Build);
                    Console.WriteLine("CD: " + CD.Build);
                    Console.WriteLine("SMC: " + smc_crc32 + ((SMCpatched) ? "    (patched)" : "   (unable to patch)"));
                    Console.WriteLine("Exploit: " + exploitType);
                    Console.WriteLine("-------------------------------------------------------");
                    Console.WriteLine("*** Image created at: " + outputFile);

                    Console.WriteLine("\nFlash image to nand using any NAND writing tool.");
                    Console.WriteLine("*** Enjoy :) ");

                    if (Arguments.Recognized.ContainsKey("/kernel") && Arguments.Recognized.ContainsKey("/hack"))
                    {
                        print_Notice();
                    }

                    pause("Press any key to close");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();

                }

                if(create)
                {
                    // load the stuff from specified per-build folder
                    Console.WriteLine("Loading ini file...");
                    string ret = LoadFromIni(ref image, Arguments.Recognized["/create"]);
                    if (ret != null)
                        PrintError("Creating image failed: " + ret);
                    string savedest = file;
                    image.SaveImage();
                    image.Close();

                    if (((NANDImageStream)image.IO.Stream).TempReturnData != null)
                    {
                        File.WriteAllBytes(savedest, ((NANDImageStream)image.IO.Stream).TempReturnData);
                        ((NANDImageStream)image.IO.Stream).TempReturnData = null;
                    }
                    Console.WriteLine("Image created successfully!");
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    //string copypath;
                    //if(Arguments.Recognized.TryGetValue("/copy", out copypath) && !String.IsNullOrEmpty(copypath))
                    //{
                        // open another nand img and shit
                    //}
                }
                else
                {
                    //try
                    //{
                        bool opened = image.OpenImage(file, 0x200);
                        if (!opened)
                            PrintError(String.Format("Can't open image {0}", file));
                    //}
                    //catch
                    //{
                    //    PrintError(String.Format("Can't open image {0}", file));
                    //}
                }

                if(extract)
                {
                    // determine what to extract
                    string types;
                    if (!Arguments.Recognized.TryGetValue("/extract", out types) || String.IsNullOrEmpty(types))
                        PrintError("No types to extract specified.");

                    string folder = Arguments.MultiParameter["/extract"][1];

                    extractImage(image, types, folder);
                }

                if(extractfile)
                {
                    if(image.CurrentFileSystem == null)
                        PrintError("No file system in image.");

                    FileSystemEntry ent =
                        image.CurrentFileSystem.Entries.Find(
                            f => f.FileName.ToLower() == Arguments.Recognized["/extractfile"]);

                    if (ent == null)
                        PrintError(String.Format("Can't find file {0} in image.", Arguments.Recognized["/extractfile"]));
                    string path = Arguments.MultiParameter["/extractfile"][1];
                    Console.WriteLine("* Extracting {0} from block 0x{1:X} size 0x{2:X}", ent.FileName, ent.BlockNumber, ent.Size);
                    System.IO.File.WriteAllBytes(System.IO.Path.Combine(path, ent.FileName), ent.GetData());
                }
                if(inject)
                {
                    // determine what to inject
                }
                if(injectfile)
                {
                    // open file
                }
            }
            else
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Main());
            }
        }
        static CmdLine ParseArguments(string[] args, int firstOptionIndex)
        {
            // store the abbreviated options here
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { "/1bl", "/1blkey"    },
                { "/cpu", "/cpukey"    },
                { "/c",   "/create"    },
                { "/d", "/dest"        },
                { "/cpy", "/copy"      },
                { "/e",   "/extract"   },
                { "/ef",  "/extractfile" }, 
                { "/i",   "/inject"    },
                { "/if",  "/injectfile"},
                { "/crc", "/crc32"}
            };
            // add options for - and / to a new list
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> kvp in dictionary)
            {
                dictionary2.Add(kvp.Key, kvp.Value);
                dictionary2.Add("-" + kvp.Key.Substring(1, kvp.Key.Length - 1), kvp.Value);
            }
            // memory!
            dictionary.Clear();
            return CmdLine.Parse(args, firstOptionIndex, CmdLineOptions, dictionary2);
        }
        static void PrintError(string msg)
        {
            Console.WriteLine();
           
            Console.WriteLine("                    ________________________");
            Console.WriteLine("___________________/      /  ERROR  \\      \\___________________");
            
            Console.WriteLine(msg);
            Console.ReadKey();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
