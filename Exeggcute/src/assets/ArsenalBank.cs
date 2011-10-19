using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.arsenal;

namespace Exeggcute.src.assets
{
    class ArsenalBank
    {
        protected static Bank<ArsenalName, List<ArsenalEntry>> bank = new Bank<ArsenalName, List<ArsenalEntry>>();
        public static List<ArsenalName> AllNames = bank.AllNames;

        protected static ArsenalLoader loader = new ArsenalLoader();
        public static List<ArsenalEntry> Get(ArsenalName name)
        {
            return bank[name];
        }

        public static void LoadAll(ContentManager content)
        {
            foreach (ArsenalName name in AllNames)
            {
                Load(content, name);
            }
        }

        public static void Load(ContentManager content, ArsenalName name)
        {
            bank.Put(loader.Load(name), name);
        }
    }
}
