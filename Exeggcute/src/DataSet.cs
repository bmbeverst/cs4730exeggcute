using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace Exeggcute.src
{
    class DataSet
    {
        private string dataArchive;
        private string dataPath = "data";
        private string ext = "dat";
        private Dictionary<string, bool> allFiles = new Dictionary<string, bool>();
        public DataSet(string dataArchive, bool forceOverwrite)
        {
            this.dataArchive = dataArchive;
            if (forceOverwrite)
            {
                string tempname = dataPath + "-temp";
                if (Directory.Exists(tempname))
                {
                    Directory.Delete(tempname, true);
                }
                if (Directory.Exists(dataPath))
                {
                    Directory.Move(dataPath, tempname);
                }
                LoadAll(true);
                Directory.Delete(tempname, true);
            }
            else
            {
                LoadAll(false);
            }
           
        }

        public DataSet(string package)
        {
            this.dataArchive = string.Format("{0}.{1}", package, ext);
        }

        public void Save()
        {

            using(ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(dataPath);
                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestSpeed;
                zip.Save(dataArchive);
            }


        }
        public void MakeManifest()
        {
            List<string> filenames = new List<string>();
            using (ZipFile zip = new ZipFile(dataPath))
            {
                foreach (var file in zip)
                {
                    if (!file.IsDirectory)
                    {
                        Console.WriteLine(file);
                    }
                }

            }
        }
        public void Restore(string filename)
        {
            using (ZipFile zip = new ZipFile(dataPath))
            {
                foreach (var thing in zip)
                {
                    //thing.Extract("../data", ExtractExistingFileAction.OverwriteSilently);
                    //Console.WriteLine("{0}/{1}", root, thing.FileName);

                }
            }
        }

        public void LoadAll(bool forceCreate)
        {
            if ((!Directory.Exists(dataPath))|| forceCreate)
            {
                using (ZipFile zip = new ZipFile(dataArchive))
                {
                    foreach (var zipEntry in zip)
                    {
                        if (!zipEntry.IsDirectory)
                        {
                            Console.WriteLine("Extracting {0}", zipEntry.FileName);
                            zipEntry.Extract(dataPath, ExtractExistingFileAction.OverwriteSilently);

                        }
                    }
                }
            }
        }

        public static void Publish()
        {

        }
    }
}
