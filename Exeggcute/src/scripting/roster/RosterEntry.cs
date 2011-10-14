using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting.roster
{
    class RosterEntry
    {
        public ModelName ModelName { get; protected set; }
        public ScriptName ScriptName { get; protected set; }
        public ArsenalName ArsenalName { get; protected set; }
        public ScriptName SpawnerName { get; protected set; }
        public RosterEntry(ModelName modelName, 
                           ScriptName scriptName, 
                           ArsenalName arsenalName,
                           ScriptName spawnerName)
        {
            ModelName = modelName;
            ScriptName = scriptName;
            ArsenalName = arsenalName;
            SpawnerName = spawnerName;
        }
    }
}
