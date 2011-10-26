using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.action;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting
{
    class TrajectoryScript : ScriptInstance
    {
        public TrajectoryScript(ScriptBase script)
            : base(script)
        {

        }

        public static TrajectoryScript Parse(string name)
        {
            return ScriptBank.GetTrajectory(name);
        }
    }
}
