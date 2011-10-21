using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.scripting;
using System.IO;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Exeggcute.src.assets
{
    /// <summary>
    /// This is more or less a hack to allow static classes to subclass.
    /// 
    /// A Bank class which maps Enum names to Assets.
    /// Used to create static containers for assets which can 
    /// be accessed where needed.
    /// </summary>
    /// <typeparam name="TAsset">the asset type to hold</typeparam>
    class Bank<TAsset> 
    {
        protected Dictionary<string, TAsset> bank = new Dictionary<string, TAsset>();
        protected Dictionary<string, int> seen = new Dictionary<string, int>();
        public readonly string rootDir;
        public ReadOnlyCollection<string> AllFiles;
        public Bank(string root, string[] exts)
        {
            rootDir = root;
            init(exts);
        }
        public Bank(string root, string ext)
        {
            rootDir = root;
            init(new string[] { ext });

        }
        private void init(string[] exts)
        {
            string[] files = Util.GetFiles(rootDir);
            List<string> names = new List<string>();
            for (int i = 0; i < files.Length; i += 1)
            {
                string raw = files[i];
                foreach (string ext in exts)
                {
                    string replaced = Regex.Replace(raw, "[.]" + ext, "");
                    if (!(replaced.Equals(raw)))
                    {
                        names.Add(replaced);
                    }
                }
            }
            AllFiles = new ReadOnlyCollection<string>(names);
        }

        public virtual TAsset this[string name]
        {
            get
            {
                if (seen.ContainsKey(name) && !bank.ContainsKey(name))
                {
                    throw new DeletedResourceError("Resource {0} was unloaded", name);
                }
                return bank[name];
            }
        }

        public virtual void LoadAll(ContentManager content)
        {
            foreach (string file in AllFiles)
            {
                Load(content, file);
            }
        }
        protected string getName(string filepath)
        {
            return Path.GetFileNameWithoutExtension(filepath);
        }

        /// <summary>
        /// Allows the ContentManager to handle the loading of an asset which
        /// XNA already knows how to load.
        /// </summary>
        public virtual void Load(ContentManager content, string filepath)
        {
            if (Regex.IsMatch(filepath, "model"))
            {
                string x;
            }
            string name = getName(filepath);
            // i am a terrible person
            string relpath = Regex.Replace(filepath, "ExeggcuteContent/", "");

            seen[name] = 1;
            bank[name] = content.Load<TAsset>(relpath);
        }

        /// <summary>
        /// A hackish method of loading a non-XNA managed asset, such as a 
        /// script or sprite.
        /// </summary>
        public virtual void Put(TAsset asset, string name)
        {
            seen[name] = 1;
            bank[name] = asset;
        }

        public virtual void Unload(ContentManager content, List<string> names)
        {
            names.ForEach(name => bank.Remove(name));
            throw new Exception("Not implemented");
        }




    }
}
