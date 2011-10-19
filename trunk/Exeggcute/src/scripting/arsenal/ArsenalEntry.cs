using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting.arsenal
{
    class ArsenalEntry
    {
        public ModelName ModelName { get; protected set; }
        public ScriptName SpawnerMoveScriptName { get; protected set; }
        public ScriptName ShotBehaviorScriptName { get; protected set; }
        public ScriptName SpawnScriptName { get; protected set; }
        public ArsenalEntry(ModelName modelName, ScriptName spawnerMoveScript, ScriptName spawnScript, ScriptName shotScript)
        {
            ModelName = modelName;
            SpawnerMoveScriptName = spawnerMoveScript;
            SpawnScriptName = spawnScript;
            ShotBehaviorScriptName = shotScript;
        }
    }
}
