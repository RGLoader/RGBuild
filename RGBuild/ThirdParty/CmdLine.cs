using System;
using System.Collections.Generic;

namespace RGBuild.Util
{
    internal class CmdLine
    {
        private readonly IDictionary<string, string> _recognized;
        private readonly IDictionary<string, string[]> _multiparameter;
        private readonly IList<string> _unrecognized;

        private CmdLine(IDictionary<string, string> recognized, IList<string> unrecognized, IDictionary<string, string[]> multiparameter)
        {
            if (recognized == null)
                throw new ArgumentNullException("recognized");

            if (unrecognized == null)
                throw new ArgumentNullException("unrecognized");

            if (multiparameter == null)
                throw new ArgumentNullException("multiparameter");

            _recognized = recognized;
            _unrecognized = unrecognized;
            _multiparameter = multiparameter;
        }

        internal static CmdLine Parse(string[] options, int startIndex, string[] expectedOptions, IDictionary<string, string> aliases)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }
            if (expectedOptions == null)
            {
                throw new ArgumentNullException("expectedOptions");
            }
            if (aliases == null)
            {
                throw new ArgumentNullException("aliases");
            }
            List<string> list = new List<string>(expectedOptions);
            list.Sort(StringComparer.OrdinalIgnoreCase);
            Dictionary<string, string> recognized = new Dictionary<string, string>(expectedOptions.Length, StringComparer.OrdinalIgnoreCase);
            Dictionary<string, string[]> multiparameter = new Dictionary<string, string[]>(expectedOptions.Length, StringComparer.OrdinalIgnoreCase);
            List<string> unrecognized = new List<string>();
            for (int i = startIndex; i < options.Length; i++)
            {
                string item = options[i];
                string str = options[i];
                if (!string.IsNullOrEmpty(str))
                {
                    int index = str.IndexOf(':');
                    string str3 = null;
                    string[] array = null;
                    if (index > 0)
                    {
                        str3 = str.Substring(index + 1);
                        if (((str3.Length > 1) && (str3[0] == '"')) && (str3[str3.Length - 1] == '"'))
                        {
                            str3 = str3.Substring(1, str3.Length - 2);
                        }
                        int index2 = str3.IndexOf(':');
                        if (index2 > 0)
                        {
                            array = new string[2];
                            array[0] = str3.Substring(0, index2);
                            if (((array[0].Length > 1) && (array[0][0] == '"')) && (array[0][array[0].Length - 1] == '"'))
                            {
                                array[0] = array[0].Substring(1, array[0].Length - 2);
                            }
                            array[1] = str3.Substring(index2 + 1);
                            if (((array[1].Length > 1) && (array[1][0] == '"')) && (array[1][array[1].Length - 1] == '"'))
                            {
                                array[1] = array[1].Substring(1, array[1].Length - 2);
                            }
                        }
                        str = str.Substring(0, index);
                    }
                    if (!string.IsNullOrEmpty(str))
                    {
                        string str4;
                        if (aliases.TryGetValue(str, out str4))
                        {
                            str = str4;
                        }
                        if (list.BinarySearch(str, StringComparer.OrdinalIgnoreCase) >= 0)
                        {
                            recognized[str] = str3;
                            if (array != null)
                                multiparameter[str] = array;
                        }
                        else
                        {
                            unrecognized.Add(item);
                        }
                    }
                }
            }
            return new CmdLine(recognized, unrecognized, multiparameter);
        }

        internal IDictionary<string, string> Recognized
        {
            get
            {
                return _recognized;
            }
        }

        internal IList<string> UnrecognizedOptions
        {
            get
            {
                return _unrecognized;
            }
        }

        internal IDictionary<string, string[]> MultiParameter
        {
            get
            {
                return _multiparameter;
            }
        }
    }
}
