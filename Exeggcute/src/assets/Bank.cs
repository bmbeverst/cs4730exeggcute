using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Exeggcute.src.assets
{
    class Bank<TAsset>
    {
        protected Dictionary<string, TAsset> bank = new Dictionary<string, TAsset>();
        public string RootDir { get; protected set; }
        public ReadOnlyCollection<string> AllFiles;

        public string Ext { get; protected set; }
        public Bank(string root, string ext)
        {
            this.RootDir = root;
            this.Ext = ext;
            string[] files = GetFileList(RootDir);
            AllFiles = new ReadOnlyCollection<string>(files);
        }

        public string GetFilename(string name)
        {
            return string.Format("{0}/{1}.{2}", RootDir, name, Ext);
        }

        public virtual List<TAsset> GetAssets()
        {
            List<TAsset> result = new List<TAsset>();
            foreach (var pair in bank)
            {
                result.Add(pair.Value);
            }
            return result;
        }

        public virtual bool ContainsKey(string key)
        {
            return bank.ContainsKey(key);
        }

        public virtual List<string> GetLoadedNames()
        {
            return bank.Keys.ToList();
        }

        public virtual TAsset this[string name]
        {
            get { return bank[name]; }
        }

        public virtual void Insert(string name, TAsset asset)
        {
            bank[name] = asset;
        }

        public virtual string GetName(string filepath)
        {
            return Path.GetFileNameWithoutExtension(filepath);
        }

        public virtual string[] GetFileList(string filepath)
        {
            return Util.GetFiles(RootDir);
        }

        public virtual void PutByName(TAsset asset, string name)
        {
            bank[name] = asset;
        }

        public virtual void PutByFile(TAsset asset, string filepath)
        {
            string name = GetName(filepath);
            bank[name] = asset;
        }
    }
}
