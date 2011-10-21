using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.arsenal;
using System.IO;

namespace Exeggcute.src.assets
{
    class ArsenalBank
    {
        protected static Bank<List<ArsenalEntry>> bank =
            new Bank<List<ArsenalEntry>>("data/arsenals","arsenal");

        protected static ArsenalLoader loader = new ArsenalLoader();
        public static List<ArsenalEntry> Get(string name)
        {
            return bank[name];
        }

        public static void LoadAll(ContentManager content)
        {
            foreach (string file in bank.AllFiles)
            {
                Load(content, file);
            }
        }

        public static void Load(ContentManager content, string name)
        {
            //FIXME HACK
            string cutname = Path.GetFileNameWithoutExtension(name);
            bank.Put(loader.Load(name), cutname);
        }
    }
}
