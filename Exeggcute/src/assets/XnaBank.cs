using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Content;

namespace Exeggcute.src.assets
{
    class XnaBank<TAsset> : Bank<TAsset>
    {

        public XnaBank(string root, string ext)
            : base(string.Format("{0}/{1}", Engine.ContentRoot, root), ext)
        {

        }


        public override string[] GetFileList(string filepath)
        {
            string[] files = Util.GetFiles(RootDir);
            List<string> names = new List<string>();
            for (int i = 0; i < files.Length; i += 1)
            {
                string raw = files[i];
                if (isFileMatch(raw))
                {
                    string extRemoved = removeExtension(raw);
                    names.Add(extRemoved);
                }
            }
            return names.ToArray();
        }

        private bool isFileMatch(string file)
        {
            string regex = string.Format("[.]{0}$", Ext);
            return Regex.IsMatch(file, regex);
        }

        private string removeExtension(string file)
        {
            string regex = string.Format("[.]{0}$", Ext);
            return Regex.Replace(file, regex, "");
        }

        public virtual void LoadAll(ContentManager content)
        {
            foreach (string file in AllFiles)
            {
                Load(content, file);
            }
        }

        public virtual void Load(ContentManager content, string filepath)
        {
            string name = GetName(filepath);
            string rootFormatted = string.Format("{0}/", Engine.ContentRoot);
            string relpath = Regex.Replace(filepath, rootFormatted, "");

            bank[name] = content.Load<TAsset>(relpath);
        }



        public virtual void PutWithFile(TAsset asset, string filename)
        {
            string name = GetName(filename);
            PutByName(asset, name);
        }
    }
}
