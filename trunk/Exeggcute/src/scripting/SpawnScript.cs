using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.action;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting
{
    class SpawnScript : ScriptInstance
    {
        public SpawnScript(ScriptBase script)
            : base(script)
        {
            
        }

        public static SpawnScript Parse(string name)
        {
            return ScriptBank.GetSpawn(name);
        }
    }
}
