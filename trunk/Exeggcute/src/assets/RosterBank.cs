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
        protected static CustomBank<Roster> bank =
            new CustomBank<Roster>("data/rosters");

        protected static RosterLoader loader = new RosterLoader();
        public static Roster Get(string name)
        {
            return bank[name];
        }

        public static void LoadAll()
        {
            foreach (string file in bank.AllFiles)
            {
                bank.Put(loader.Make(file), file);
            }
        }
    }
}
