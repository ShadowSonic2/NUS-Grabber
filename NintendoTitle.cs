using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace NUS_File_Grabber
{
    public sealed class NintendoTitle
    {
        #region Private Fields
        private string _titleId;
        private string _titleVersion = "";
        private bool _wiiMode;
        private string _contentsStrNum
        {
            get
            {
                if (TMD == null)
                    return "";
                string contentstrnum = "";
                for (int x = 478; x < 480; x++)
                {
                    contentstrnum += TrimLeadingZeros(Convert.ToString(TMD[x]));
                }
                return contentstrnum;
            }
        }

        Dictionary<string, byte[]> _contents = new Dictionary<string, byte[]>();
        #endregion

        #region Public Properties

        /// <summary>
        /// {get;set;} the current TitleId
        /// </summary>
        public string TitleId
        {
            get { return _titleId; }
            set { _titleId = value; }
        }

        /// <summary>
        /// {get;set;} the current TitleVersion
        /// </summary>
        public string TitleVersion
        {
            get
            {
                if (TMD == null)
                    return _titleVersion;
                else
                {
                    // Read Title Version...
                    string tmdversion = "";
                    for (int x = 476; x < 478; x++)
                    {
                        tmdversion += MakeProperLength(ConvertToHex(Convert.ToString(TMD[x])));
                    }
                    return
                        Convert.ToString(int.Parse(tmdversion, System.Globalization.NumberStyles.HexNumber));
                }
            }
            set { _titleVersion = value; }
        }

        /// <summary>
        /// {get;set;} Is this Wii Mode?
        /// </summary>
        public bool WiiMode
        {
            get { return _wiiMode; }
            set { _wiiMode = value; }
        }

        public byte[] TMD
        {
            get
            {
                byte[] mytmd;
                if (_contents.TryGetValue(TMDFull, out mytmd))
                    return mytmd;
                else
                    return null;
            }
        }

        public byte[] cetk
        {
            get
            {
                byte[] mycetk;
                if (_contents.TryGetValue("cetk", out mycetk))
                    return mycetk;
                else
                    return null;
            }
        }

        public string TMDFull
        {
            get
            {
                if (_titleVersion == "")
                    return "tmd";
                else
                    return "tmd." + _titleVersion;
            }
        }

        public Dictionary<string, byte[]> Contents
        {
            get { return _contents; }
            set { _contents = value; }
        }

        #endregion

        #region Constructors
        /// <summary>
        /// This will construct a new nintendotitle and populate it from the 
        /// Nintendo web servers.
        /// </summary>
        /// <param name="titleid"></param>
        /// <param name="titleversion"></param>
        /// <param name="wiiMode">true if wii, false if DS.</param>
        public NintendoTitle(string titleid, string titleversion, bool wiiMode)
        {
            _titleId = titleid;
            _titleVersion = titleversion;
            _wiiMode = wiiMode;
            // Call to get the files from server
            NintendoDownloadAgent.DownloadTitle(this);
        }
        #endregion

        #region Public Methods
        public string[] GetContentNames()
        {
            string contentstrnum = "";
            for (int x = 478; x < 480; x++)
            {
                contentstrnum += TrimLeadingZeros(Convert.ToString(TMD[x]));
            }
            int length = Convert.ToInt32(_contentsStrNum);
            string[] contentnames = new string[length];
            int startoffset = 484;

            for (int i = 0; i < length; i++)
            {
                contentnames[i] = MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(
                    TMD[startoffset + 1]))) + MakeProperLength(ConvertToHex(Convert.ToString(
                    TMD[startoffset + 2]))) + MakeProperLength(ConvertToHex(Convert.ToString(
                    TMD[startoffset + 3])));
                startoffset += 36;
            }
            return contentnames;
        }

        public string[] GetContentSizes()
        {
            string contentstrnum = "";
            for (int x = 478; x < 480; x++)
            {
                contentstrnum += TrimLeadingZeros(Convert.ToString(TMD[x]));
            }
            int length = Convert.ToInt32(_contentsStrNum);
            string[] contentsizes = new string[length];
            int startoffset = 492;
            for (int i = 0; i < length; i++)
            {
                contentsizes[i] = MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 1]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 2]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 3]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 4]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 5]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 6]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 7])));
                contentsizes[i] = TrimLeadingZeros(contentsizes[i]);
                startoffset += 36;
            }
            return contentsizes;
        }

        public string[] GetContentHashes()
        {
            string contentstrnum = "";
            for (int x = 478; x < 480; x++)
            {
                contentstrnum += TrimLeadingZeros(Convert.ToString(TMD[x]));
            }
            int length = Convert.ToInt32(_contentsStrNum);
            string[] contenthashes = new string[length];
            int startoffset = 500;

            for (int i = 0; i < length; i++)
            {
                contenthashes[i] = MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 1]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 2]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 3]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 4]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 5]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 6]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 7]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 8]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 9]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 10]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 11]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 12]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 13]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 14]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 15]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 16]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 17]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 18]))) +
                    MakeProperLength(ConvertToHex(Convert.ToString(TMD[startoffset + 19])));
                //contentsizes[i] = TrimLeadingZeros(contentsizes[i]);
                startoffset += 36;
            }

            return contenthashes;
        }
        #endregion

        #region Private Methods
        private static string TrimLeadingZeros(string num)
        {
            int startindex = 0;
            for (int i = 0; i < num.Length; i++)
            {
                if ((num[i] == 0) || (num[i] == '0'))
                    startindex += 1;
                else
                    break;
            }
            return num.Substring(startindex, (num.Length - startindex));
        }

        private static string MakeProperLength(string hex)
        {
            if (hex.Length == 1)
                hex = "0" + hex;

            return hex;
        }

        private static string ConvertToHex(string decval)
        {
            //Console.Write("Decimal Value: " + decval); // mtp

            // Convert text string to unsigned integer
            int uiDecimal = System.Convert.ToInt32(decval);

            string hexval;
            // Format unsigned integer value to hex and show in another textbox
            // Console.Write("Hex Value: " + String.Format("{0:x2}", uiDecimal)); // mtp
            hexval = String.Format("{0:x2}", uiDecimal);
            return hexval;
        }

        #endregion

    }
}

