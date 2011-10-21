using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.roster;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace Exeggcute.src.assets
{
    class RosterBank
    {
        protected static Bank<Roster> bank =
            new Bank<Roster>("data/rosters", "roster");

        protected static RosterLoader loader = new RosterLoader();
        public static Roster Get(string name)
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
            string cutname = Path.GetFileNameWithoutExtension(name);
            bank.Put(new Roster(loader.Load(name)), cutname);
        }
    }
}
