using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.roster;
using Microsoft.Xna.Framework.Content;

namespace Exeggcute.src.assets
{
    class RosterBank
    {
        protected static Bank<RosterName, Roster> bank = new Bank<RosterName, Roster>();
        public static List<RosterName> AllNames = bank.AllNames;

        protected static RosterLoader loader = new RosterLoader();
        public static Roster Get(RosterName name)
        {
            return bank[name];
        }

        public static void LoadAll(ContentManager content)
        {
            foreach (RosterName name in AllNames)
            {
                Load(content, name);
            }
        }

        public static void Load(ContentManager content, RosterName name)
        {
            bank.Put(new Roster(loader.Load(name)), name);
        }
    }
}
