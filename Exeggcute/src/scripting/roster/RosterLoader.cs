using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.entities;
using Exeggcute.src.scripting.arsenal;

namespace Exeggcute.src.scripting.roster
{
    class RosterLoader : ListParser<RosterEntry>
    {
        public RosterLoader()
        {
            Delim = ' ';
        }

        protected override string getFilepath(string name)
        {
            return string.Format("{0}.roster", name);
        }

        protected override RosterEntry parseEntry(Stack<string> tokens)
        {
            string modelname = tokens.Pop();
            string behaviorname = tokens.Pop();
            string arsenalname = tokens.Pop();
            Alignment alignment = Util.ParseEnum<Alignment>(tokens.Pop());
            HashList<Shot> shotHandle;
            if (alignment == Alignment.Enemy)
            {
                shotHandle = World.EnemyShots;
            }
            else if (alignment == Alignment.Player)
            {
                shotHandle = World.PlayerShots;
            }
            else
            {
                throw new ExeggcuteError("there is no third option!");
            }

            Model model = ModelBank.Get(modelname);
            BehaviorScript behavior = ScriptBank.GetBehavior(behaviorname);
            List<ArsenalEntry> entries = ArsenalBank.Get(arsenalname);
            NewArsenal arsenal = new NewArsenal(entries, shotHandle);
            return new RosterEntry(model, behavior, arsenal);
        }
    }
}
