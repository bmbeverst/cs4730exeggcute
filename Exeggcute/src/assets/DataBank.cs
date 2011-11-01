using System;
using System.Reflection;
using Exeggcute.src.loading;

namespace Exeggcute.src.assets
{
    class DataBank<TAsset> : Bank<TAsset>
    {
        protected Type assetType;
        protected MethodInfo parseMethod;
        public DataBank(string root, string ext)
            : base(string.Format("{0}/{1}", Engine.DataRoot, root), ext)
        {
            string parserName = "LoadFromFile";
            this.assetType = typeof(TAsset);
            Type[] typeSig = new Type[] { typeof(string) };
            this.parseMethod = assetType.GetMethod(parserName, UnknownType.Flags, null, typeSig, null);
            if (parseMethod == null)
            {
                throw new ExeggcuteError("Type {0} must have a method named {1}(string filename) returning a {0}", assetType.Name, parserName);
            }
        }

        public virtual void LoadAll()
        {
            foreach (string file in AllFiles)
            {
                Load(file);
            }
        }

        public virtual void Load(string filename)
        {
            string name = GetName(filename);
            bank[name] = (TAsset)parseMethod.Invoke(null, new object[] { filename });
        }
    }
}
