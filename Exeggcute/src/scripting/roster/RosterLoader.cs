using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting.roster
{
    class RosterLoader : ListParser<RosterName, RosterEntry>
    {
        public RosterLoader()
        {
            Delim = ' ';
        }

        protected override string getFilepath(RosterName name)
        {
            return string.Format("data/rosters/{0}.roster", name);
        }

        protected override RosterEntry parseEntry(Stack<string> tokens)
        {
            ModelName modelname = Util.ParseEnum<ModelName>(tokens.Pop());
            ScriptName scriptname = Util.ParseEnum<ScriptName>(tokens.Pop());
            ArsenalName arsenalname = Util.ParseEnum<ArsenalName>(tokens.Pop());
            return new RosterEntry(modelname, scriptname, arsenalname);
        }
    }
}
