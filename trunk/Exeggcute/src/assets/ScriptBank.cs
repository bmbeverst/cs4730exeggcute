using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.action;

namespace Exeggcute.src.assets
{
    class ScriptBank
    {
        private static Bank<ScriptName, ActionList> bank = new Bank<ScriptName, ActionList>();
        public static List<ScriptName> AllNames = bank.AllNames;
        public static ActionListLoader loader = new ActionListLoader();
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
            List<ActionBase> actions = loader.Load(name);
            ActionList list = new ActionList(actions);
            bank.Put(list, name);
        }
    }
}
