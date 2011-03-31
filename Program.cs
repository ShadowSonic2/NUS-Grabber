using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NUS_File_Grabber
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Console.WriteLine("WiiHacks NUS File Grabber 0.2 - Blue Phantom and ShadowSonic2");
            Console.WriteLine("Includes code from wiiNinja and WB3000");
            Console.WriteLine("--------------------------------------------------------------");
            Console.WriteLine("Enter TitleID");
            string titleid = Console.ReadLine();
            Console.WriteLine("Enter Title Version (Leave blank for latest)");
            string titleversion = Console.ReadLine();

            // Do some reflection to figure the current directory.  Life is better
            // when you take time to reflect.
            string currentDirectory =
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

            //NintendoTitle aTitle = new NintendoTitle("0000000100000002", "449", true);
            NintendoTitle aTitle = new NintendoTitle(titleid, titleversion, true);
            WadWriterAgent.WriteWad(aTitle, currentDirectory);

            if (args.Length < 2)
            {
                Console.WriteLine("Usage:");
                Console.WriteLine("    nusd.exe titleID titleVersion");
                Console.WriteLine("\nWhere:");
                Console.WriteLine("    titleID = The ID of the title to be downloaded");
                Console.WriteLine("    titleVersion = The version of the title to be downloaded");
            }
        }
    }
}
