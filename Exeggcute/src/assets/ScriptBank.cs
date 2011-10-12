using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.scripting;

namespace Exeggcute.src.assets
{
    class ScriptBank
    {
        private static Bank<ScriptName, ActionList> bank = new Bank<ScriptName, ActionList>();
        public static List<ScriptName> AllNames = bank.AllNames;
        public static ActionList Get(ScriptName name)
        {
            return bank[name];
        }

        public static void LoadAll(ContentManager content)
        {
            AllNames.ForEach(name => Load(content, name));
        }

        public static void Load(ContentManager content, ScriptName name)
        {
            bank.Put(ActionListLoader.Load(name), name);
        }
    }
}
