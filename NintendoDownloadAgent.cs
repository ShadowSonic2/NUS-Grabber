using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace NUS_File_Grabber
{
    public static class NintendoDownloadAgent
    {
        #region Constants
        private const string NUSURL = "http://nus.cdn.shop.wii.com/ccs/download/";
        private const string DSiNUSURL = "http://nus.cdn.t.shop.nintendowifi.net/ccs/download/";
        #endregion

        #region Public Methods
        public static void StatusChanged(string status)
        {
            Console.WriteLine(status);
        }

        public static void DownloadTitle(NintendoTitle currentTitle)
        {
            bool success = true;
            StatusChanged("Starting NUS Download. Please be patient!");
            StatusChanged("Prerequisites: (0/2)");
            // Download TMD before the rest...
            success = DownloadNUSFile(currentTitle, currentTitle.TMDFull);
            if (success)
            {
                StatusChanged("Prerequisites: (1/2)");
                // Download CETK before the rest...
                success = DownloadNUSFile(currentTitle, "cetk");
            }

            // Gather information...
            string[] tmdcontents = currentTitle.GetContentNames();
            string[] tmdsizes = currentTitle.GetContentSizes();
            string[] tmdhashes = currentTitle.GetContentHashes();

            float totalcontentsize = 0;
            float currentcontentlocation = 0;
            for (int i = 0; i < tmdsizes.Length; i++)
            {
                totalcontentsize += int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber);
            }
            bool continueTrans = true;
            for (int i = 0; i < tmdcontents.Length && continueTrans; i++)
            {
                continueTrans = DownloadNUSFile(currentTitle, tmdcontents[i]);
                currentcontentlocation += int.Parse(tmdsizes[i], System.Globalization.NumberStyles.HexNumber);
            }
            StatusChanged("NUS Download Finished.");
        }
        #endregion

        #region Private Methods

        private static string MakeProperLength(string hex)
        {
            if (hex.Length == 1)
                hex = "0" + hex;

            return hex;
        }

        private static string ConvertToHex(string decval)
        {
            int uiDecimal = System.Convert.ToInt32(decval);
            string hexval;
            // Format unsigned integer value to hex and show in another textbox
            hexval = String.Format("{0:x2}", uiDecimal);
            return hexval;
        }


        private static bool DownloadNUSFile(NintendoTitle currentTitle, string filename)
        {
            WebClient aWebClient = new WebClient();

            try
            {
                string nusfileurl;
                if (currentTitle.WiiMode)
                    nusfileurl = NUSURL + currentTitle.TitleId + @"/" + filename;
                else
                    nusfileurl = DSiNUSURL + currentTitle.TitleId + @"/" + filename;
                StatusChanged("Grabbing " + filename);
                byte[] currentContents = aWebClient.DownloadData(nusfileurl);
                currentTitle.Contents.Add(filename, currentContents);
                return true;
            }
            catch (Exception)
            {
                StatusChanged("NUS (404): " + filename);
                StatusChanged("Start NUS Download!");
                return false;
            }
        }
        #endregion
    }
}
