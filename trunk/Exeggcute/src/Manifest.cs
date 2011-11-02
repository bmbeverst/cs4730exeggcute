using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Exeggcute.src
{
    class Manifest
    {
        private const string manifestPath = "manifest";
        private const string defaultText = "default.dat";

        public string DataFileName { get; protected set; }
        public bool ForceOverwrite { get; protected set; }
        public Manifest(string dataSet)
        {
            if (dataSet == null)
            {
                List<string> lines;
                try
                {
                    lines = Util.ReadAndStrip("manifest");
                }
                catch (IOException ioe)
                {
                    Worlds.World.ConsoleWrite("{0}\nNo manifest found! Creating default.", ioe.Message);

                    Util.WriteFile(manifestPath, defaultText);
                    lines = new List<string> { defaultText };
                }
                DataFileName = lines[0];
                ForceOverwrite = false;
            }
            else
            {
                DataFileName = string.Format("{0}.dat", dataSet);
                Util.WriteFile(manifestPath, DataFileName);
                ForceOverwrite = true;
            }
        }

        public static bool VerifyExists(string name)
        {
            return File.Exists(string.Format("{0}.dat", name));
        }

        public static List<string> GetValidSets()
        {
            List<string> result = new List<string>();
            string[] found =  Directory.GetFiles(".", "*.dat");
            foreach (string file in found)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                result.Add(name);
            }
            return result;
        }

    }
}
