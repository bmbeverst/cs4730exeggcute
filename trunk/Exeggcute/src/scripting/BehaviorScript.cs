using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.action;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting
{
    class BehaviorScript : ScriptInstance
    {
        public BehaviorScript(ScriptBase script)
            : base(script)
        {

        }

        public static BehaviorScript Parse(string name)
        {
            return ScriptBank.GetBehavior(name);
        }
    }
}
