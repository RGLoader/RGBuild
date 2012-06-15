using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;      
namespace RGBuild.ThirdParty
{
    /// <summary>
    /// An useful class to read/write/delete/count registry keys
    /// </summary>
    public static class RegistryHelper
    {
        public static bool ShowError = false;

        public static string SubKey = "SOFTWARE\\" + Application.ProductName.ToUpper() + " UI";

        /// <summary>
        /// A property to set the BaseRegistryKey value.
        /// (default = Registry.LocalMachine)
        /// </summary>
        public static RegistryKey BaseRegistryKey = Registry.LocalMachine;

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// To read a registry key.
        /// input: KeyName (string)
        /// output: value (string) 
        /// </summary>
        public static string ReadKey(string KeyName)
        {
            // Opening the registry key
            RegistryKey rk = BaseRegistryKey;
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(SubKey);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return "";
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    // or null is returned.
                    return (string)sk1.GetValue(KeyName.ToUpper());
                }
                catch (Exception e)
                {
                    // AAAAAAAAAAARGH, an error!
                    ShowErrorMessage(e, "Reading registry " + KeyName.ToUpper());
                    return "";
                }
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// To write into a registry key.
        /// input: KeyName (string) , Value (object)
        /// output: true or false 
        /// </summary>
        public static bool WriteKey(string KeyName, object Value)
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegistryKey;
                // I have to use CreateSubKey 
                // (create or open it if already exits), 
                // 'cause OpenSubKey open a subKey as read-only
                RegistryKey sk1 = rk.CreateSubKey(SubKey);
                // Save the value
                sk1.SetValue(KeyName.ToUpper(), Value);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Writing registry " + KeyName.ToUpper());
                return false;
            }
        }
        public static bool WriteKey(string keyName, string[] value, string seperator)
        {
            string data = "";
            foreach (string str in value)
                data += value + seperator;

            return WriteKey(keyName, data);
        }
        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// To delete a registry key.
        /// input: KeyName (string)
        /// output: true or false 
        /// </summary>
        public static bool DeleteKey(string KeyName)
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegistryKey;
                RegistryKey sk1 = rk.CreateSubKey(SubKey);
                // If the RegistrySubKey doesn't exists -> (true)
                if (sk1 == null)
                    return true;
                else
                    sk1.DeleteValue(KeyName);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Deleting SubKey " + SubKey);
                return false;
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// To delete a sub key and any child.
        /// input: void
        /// output: true or false 
        /// </summary>
        public static bool DeleteSubKeyTree()
        {
            try
            {
                // Setting
                RegistryKey rk = BaseRegistryKey;
                RegistryKey sk1 = rk.OpenSubKey(SubKey);
                // If the RegistryKey exists, I delete it
                if (sk1 != null)
                    rk.DeleteSubKeyTree(SubKey);

                return true;
            }
            catch (Exception e)
            {
                // AAAAAAAAAAARGH, an error!
                ShowErrorMessage(e, "Deleting SubKey " + SubKey);
                return false;
            }
        }

        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// Retrive the count of subkeys at the current key.
        /// input: void
        /// output: number of subkeys
        /// </summary>
        public static int SubKeyCount
        {
            get
            {
                try
                {
                    // Setting
                    RegistryKey rk = BaseRegistryKey;
                    RegistryKey sk1 = rk.OpenSubKey(SubKey);
                    // If the RegistryKey exists...
                    if (sk1 != null)
                        return sk1.SubKeyCount;
                    else
                        return 0;
                }
                catch (Exception e)
                {
                    // AAAAAAAAAAARGH, an error!
                    ShowErrorMessage(e, "Retriving subkeys of " + SubKey);
                    return 0;
                }
            }
        }
        /* **************************************************************************
         * **************************************************************************/

        /// <summary>
        /// Retrive the count of values in the key.
        /// input: void
        /// output: number of keys
        /// </summary>
        public static int ValueCount
        {
            get
            {
                try
                {
                    // Setting
                    RegistryKey rk = BaseRegistryKey;
                    RegistryKey sk1 = rk.OpenSubKey(SubKey);
                    // If the RegistryKey exists...
                    if (sk1 != null)
                        return sk1.ValueCount;
                    else
                        return 0;
                }
                catch (Exception e)
                {
                    // AAAAAAAAAAARGH, an error!
                    ShowErrorMessage(e, "Retriving keys of " + SubKey);
                    return 0;
                }
            }
        }
        /* **************************************************************************
         * **************************************************************************/

        private static void ShowErrorMessage(Exception e, string Title)
        {
            if (ShowError == true)
                MessageBox.Show(e.Message,
                                Title
                                , MessageBoxButtons.OK
                                , MessageBoxIcon.Error);
        }
    }
}
