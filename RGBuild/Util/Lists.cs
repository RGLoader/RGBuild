using System.Collections.Generic;

namespace RGBuild.Util
{
    public static class Lists
    {

        public static List<ushort> TRINITY_CB = new List<ushort> { 9188, 9230 };
        public static List<ushort> XENON_CB = new List<ushort> { 1888, 1902, 1903, 1920, 1921, 7363, 7371 };
        public static List<ushort> FALCON_CB = new List<ushort> { 5761, 5766, 5770, 5771, 5772, 5773 };
        public static List<ushort> ZEPHYR_CB = new List<ushort> { 4558, 4575, 4577, 4578 };
        public static List<ushort> JASPER_CB = new List<ushort> { 6712, 6723, 6750, 6751, 6752, 6753 };
        public static List<ushort> RGH_CB = new List<ushort> { 4578, 5771, 6750, 9188 };
        public static List<ushort> JTAG_CB = new List<ushort> { 1888, 1902, 1903, 1920, 1921, 4558, 4580, 5761, 5766, 5770, 6712, 6723, 7363, 7371 };
        public static List<ushort> KernelVersions = new List<ushort> { 13599, 14699, 14719 };
        public static List<string> ExploitTypes = new List<string> { "RGH", "RGH2", "JTAG" };

        public static Dictionary<XeKey, XeKeyBinding> XeKeys = new Dictionary<XeKey, XeKeyBinding>
                                                                   {
                             {XeKey.KEYVAULT_HASH, new XeKeyBinding(0x0, 0x8)},
                             {XeKey.MANUFACTURING_MODE, new XeKeyBinding(0x8, 1)},
                             {XeKey.ALTERNATE_KEY_VAULT, new XeKeyBinding(0x9, 1)},
                             {XeKey.RESTRICTED_PRIVILEGES_FLAGS, new XeKeyBinding(0xa, 1)},
                             {XeKey.RESERVED_BYTE3, new XeKeyBinding(0xb, 1)},
                             {XeKey.ODD_FEATURES, new XeKeyBinding(0xc, 2)},
                             {XeKey.ODD_AUTHTYPE, new XeKeyBinding(0xe, 2)},
                             {XeKey.RESTRICTED_HVEXT_LOADER, new XeKeyBinding(0x10, 4)},
                             {XeKey.POLICY_FLASH_SIZE, new XeKeyBinding(0x14, 4)},
                             {XeKey.POLICY_BUILTIN_USBMU_SIZE, new XeKeyBinding(0x18, 4)},
                             {XeKey.RESERVED_DWORD4, new XeKeyBinding(0x1c, 4)}, //fcrt flag stored here
                             {XeKey.RESTRICTED_PRIVILEGES, new XeKeyBinding(0x20, 8)},
                             {XeKey.RESERVED_QWORD2, new XeKeyBinding(0x28, 8)},
                             {XeKey.RESERVED_QWORD3, new XeKeyBinding(0x30, 8)},
                             {XeKey.RESERVED_QWORD4, new XeKeyBinding(0x38, 8)},
                             {XeKey.RESERVED_KEY1, new XeKeyBinding(0x40, 0x10)},
                             {XeKey.RESERVED_KEY2, new XeKeyBinding(0x50, 0x10)},
                             {XeKey.RESERVED_KEY3, new XeKeyBinding(0x60, 0x10)},
                             {XeKey.RESERVED_KEY4, new XeKeyBinding(0x70, 0x10)},
                             {XeKey.RESERVED_RANDOM_KEY1, new XeKeyBinding(0x80, 0x10)},
                             {XeKey.RESERVED_RANDOM_KEY2, new XeKeyBinding(0x90, 0x10)},
                             {XeKey.CONSOLE_SERIAL_NUMBER, new XeKeyBinding(0xa0, 0xC)},
                             {XeKey.MOBO_SERIAL_NUMBER, new XeKeyBinding(0xb0, 8)},
                             {XeKey.GAME_REGION, new XeKeyBinding(0xb8, 2)},
                             {XeKey.CONSOLE_OBFUSCATION_KEY, new XeKeyBinding(0xc0, 0x10)},
                             {XeKey.KEY_OBFUSCATION_KEY, new XeKeyBinding(0xd0, 0x10)},
                             {XeKey.ROAMABLE_OBFUSCATION_KEY, new XeKeyBinding(0xe0, 0x10)},
                             {XeKey.DVD_KEY, new XeKeyBinding(0xf0, 0x10)},
                             {XeKey.PRIMARY_ACTIVATION_KEY, new XeKeyBinding(0x100, 0x18)},
                             {XeKey.SECONDARY_ACTIVATION_KEY, new XeKeyBinding(0x118, 0x10)},
                             {XeKey.GLOBAL_DEVICE_2DES_KEY1, new XeKeyBinding(0x128, 0x10)},
                             {XeKey.GLOBAL_DEVICE_2DES_KEY2, new XeKeyBinding(0x138, 0x10)},
                             {XeKey.WIRELESS_CONTROLLER_MS_2DES_KEY1, new XeKeyBinding(0x148, 0x10)},
                             {XeKey.WIRELESS_CONTROLLER_MS_2DES_KEY2, new XeKeyBinding(0x158, 0x10)},
                             {XeKey.WIRED_WEBCAM_MS_2DES_KEY1, new XeKeyBinding(0x168, 0x10)},
                             {XeKey.WIRED_WEBCAM_MS_2DES_KEY2, new XeKeyBinding(0x178, 0x10)},
                             {XeKey.WIRED_CONTROLLER_MS_2DES_KEY1, new XeKeyBinding(0x188, 0x10)},
                             {XeKey.WIRED_CONTROLLER_MS_2DES_KEY2, new XeKeyBinding(0x198, 0x10)},
                             {XeKey.MEMORY_UNIT_MS_2DES_KEY1, new XeKeyBinding(0x1a8, 0x10)},
                             {XeKey.MEMORY_UNIT_MS_2DES_KEY2, new XeKeyBinding(0x1b8, 0x10)},
                             {XeKey.OTHER_XSM3_DEVICE_MS_2DES_KEY1, new XeKeyBinding(0x1c8, 0x10)},
                             {XeKey.OTHER_XSM3_DEVICE_MS_2DES_KEY2, new XeKeyBinding(0x1d8, 0x10)},
                             {XeKey.WIRELESS_CONTROLLER_3P_2DES_KEY1, new XeKeyBinding(0x1e8, 0x10)},
                             {XeKey.WIRELESS_CONTROLLER_3P_2DES_KEY2, new XeKeyBinding(0x1f8, 0x10)},
                             {XeKey.WIRED_WEBCAM_3P_2DES_KEY1, new XeKeyBinding(0x208, 0x10)},
                             {XeKey.WIRED_WEBCAM_3P_2DES_KEY2, new XeKeyBinding(0x218, 0x10)},
                             {XeKey.WIRED_CONTROLLER_3P_2DES_KEY1, new XeKeyBinding(0x228, 0x10)},
                             {XeKey.WIRED_CONTROLLER_3P_2DES_KEY2, new XeKeyBinding(0x238, 0x10)},
                             {XeKey.MEMORY_UNIT_3P_2DES_KEY1, new XeKeyBinding(0x248, 0x10)},
                             {XeKey.MEMORY_UNIT_3P_2DES_KEY2, new XeKeyBinding(0x258, 0x10)},
                             {XeKey.OTHER_XSM3_DEVICE_3P_2DES_KEY1, new XeKeyBinding(0x268, 0x10)},
                             {XeKey.OTHER_XSM3_DEVICE_3P_2DES_KEY2, new XeKeyBinding(0x278, 0x10)},
                             {XeKey.CONSOLE_PRIVATE_KEY, new XeKeyBinding(0x288, 0x1D0)},
                             {XeKey.XEIKA_PRIVATE_KEY, new XeKeyBinding(0x458, 0x390)},
                             {XeKey.CARDEA_PRIVATE_KEY, new XeKeyBinding(0x7e8, 0x1d0)},
                             {XeKey.CONSOLE_CERTIFICATE, new XeKeyBinding(0x9b8, 0x1a8)},
                             {XeKey.XEIKA_CERTIFICATE, new XeKeyBinding(0xb62, 0x140)},
                             {XeKey.CARDEA_CERTIFICATE, new XeKeyBinding(0x1EE8, 0x2108)},
                             {XeKey.SPECIAL_KEY_VAULT_SIGNATURE, new XeKeyBinding(0x1de8, 0x100)}
                         };
        public class XeKeyBinding
        {
            public int Address;
            public int Length;
            public XeKeyBinding(int addr, int len)
            {
                Address = addr;
                Length = len;
            }
        }
        public enum XBOX_BLDR_FLAG
        {
             Unpaired = 0x1, // set by 2BL
             MfgMode = 0x400, // set by HV
             DualCB = 0x800, // used on CB_A/CB_B
             NewEnc = 0x1000 // uses new crypto on CB_B, which includes the CB_A header (with flags nulled out)
        }
        public enum XeKey
        {
// ReSharper disable InconsistentNaming
            KEYVAULT_HASH = -1,
            MANUFACTURING_MODE = 0x0,
            ALTERNATE_KEY_VAULT = 0x1,
            RESTRICTED_PRIVILEGES_FLAGS = 0x2,

            RESERVED_BYTE3 = 0x3,

            ODD_FEATURES = 0x4,
            ODD_AUTHTYPE = 0x5,

            RESTRICTED_HVEXT_LOADER = 0x6,

            POLICY_FLASH_SIZE = 0x7,
            POLICY_BUILTIN_USBMU_SIZE = 0x8,

            RESERVED_DWORD4 = 0x9,

            RESTRICTED_PRIVILEGES = 0xA,

            RESERVED_QWORD2 = 0xB,
            RESERVED_QWORD3 = 0xC,
            RESERVED_QWORD4 = 0xD,
            RESERVED_KEY1 = 0xE,
            RESERVED_KEY2 = 0xF,
            RESERVED_KEY3 = 0x10,
            RESERVED_KEY4 = 0x11,
            RESERVED_RANDOM_KEY1 = 0x12,
            RESERVED_RANDOM_KEY2 = 0x13,

            CONSOLE_SERIAL_NUMBER = 0x14,
            MOBO_SERIAL_NUMBER = 0x15,
            GAME_REGION = 0x16,
            CONSOLE_OBFUSCATION_KEY = 0x17,
            KEY_OBFUSCATION_KEY = 0x18,
            ROAMABLE_OBFUSCATION_KEY = 0x19,
            DVD_KEY = 0x1A,
            PRIMARY_ACTIVATION_KEY = 0x1B,
            SECONDARY_ACTIVATION_KEY = 0x1C,

            GLOBAL_DEVICE_2DES_KEY1 = 0x1D,
            GLOBAL_DEVICE_2DES_KEY2 = 0x1E,
            WIRELESS_CONTROLLER_MS_2DES_KEY1 = 0x1F,
            WIRELESS_CONTROLLER_MS_2DES_KEY2 = 0x20,
            WIRED_WEBCAM_MS_2DES_KEY1 = 0x21,
            WIRED_WEBCAM_MS_2DES_KEY2 = 0x22,
            WIRED_CONTROLLER_MS_2DES_KEY1 = 0x23,
            WIRED_CONTROLLER_MS_2DES_KEY2 = 0x24,
            MEMORY_UNIT_MS_2DES_KEY1 = 0x25,
            MEMORY_UNIT_MS_2DES_KEY2 = 0x26,
            OTHER_XSM3_DEVICE_MS_2DES_KEY1 = 0x27,
            OTHER_XSM3_DEVICE_MS_2DES_KEY2 = 0x28,
            WIRELESS_CONTROLLER_3P_2DES_KEY1 = 0x29,
            WIRELESS_CONTROLLER_3P_2DES_KEY2 = 0x2A,
            WIRED_WEBCAM_3P_2DES_KEY1 = 0x2B,
            WIRED_WEBCAM_3P_2DES_KEY2 = 0x2C,
            WIRED_CONTROLLER_3P_2DES_KEY1 = 0x2D,
            WIRED_CONTROLLER_3P_2DES_KEY2 = 0x2E,
            MEMORY_UNIT_3P_2DES_KEY1 = 0x2F,
            MEMORY_UNIT_3P_2DES_KEY2 = 0x30,
            OTHER_XSM3_DEVICE_3P_2DES_KEY1 = 0x31,
            OTHER_XSM3_DEVICE_3P_2DES_KEY2 = 0x32,

            CONSOLE_PRIVATE_KEY = 0x33,
            XEIKA_PRIVATE_KEY = 0x34,
            CARDEA_PRIVATE_KEY = 0x35,

            CONSOLE_CERTIFICATE = 0x36,
            XEIKA_CERTIFICATE = 0x37,
            CARDEA_CERTIFICATE = 0x38,

            SPECIAL_KEY_VAULT_SIGNATURE = 0x44
            // ReSharper restore InconsistentNaming
        }
        public static List<ushort> GlitchableCBs = new List<ushort>
                                                       {
                                                           // Zephyr
                                                           4578,
                                                           4579,

                                                           // Falcon/Opus
                                                           5771,

                                                           // Jasper
                                                           6750,

                                                           // Trinity
                                                           9188
                                                       };

    }
}
