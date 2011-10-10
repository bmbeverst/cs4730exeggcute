using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace Exeggcute.src.assets
{
    /// <summary>
    /// This is more or less a hack to allow static classes to subclass.
    /// 
    /// A Bank class which maps Enum names to Assets.
    /// Used to create static containers for assets which can 
    /// be accessed where needed.
    /// </summary>
    /// <typeparam name="TName">the enum name to map</typeparam>
    /// <typeparam name="TAsset">the asset type to hold</typeparam>
    class Bank<TName, TAsset> 
    {
        protected Dictionary<TName, TAsset> bank = new Dictionary<TName, TAsset>();
        protected Dictionary<TName, int> seen = new Dictionary<TName, int>();
        public List<TName> AllNames = new List<TName>((TName[])Enum.GetValues(typeof(TName)));

        public virtual TAsset this[TName name]
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
            foreach (TName name in AllNames)
            {
                Load(content, name);
            }
        }

        public virtual void Load(ContentManager content, TName name)
        {
            seen[name] = 1;
            bank[name] = content.Load<TAsset>(name.ToString());
        }

        public virtual void Unload(ContentManager content, List<TName> names)
        {
            names.ForEach(name => bank.Remove(name));
            throw new Exception("Not implemented");
        }




    }
}
