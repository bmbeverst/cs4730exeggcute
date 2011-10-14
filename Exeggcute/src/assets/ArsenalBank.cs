using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.scripting;

namespace Exeggcute.src.assets
{
    class ArsenalBank
    {
        private static Bank<ArsenalName, Arsenal> bank = new Bank<ArsenalName, Arsenal>();
        public static List<ArsenalName> AllNames = bank.AllNames;
        public static Arsenal Get(ArsenalName name)
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
            bank.Put(new Arsenal(ArsenalLoader.Load(name)), name);
        }
    }
}
