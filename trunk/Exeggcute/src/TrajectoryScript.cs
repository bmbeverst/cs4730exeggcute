using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.action;

namespace Exeggcute.src
{
    class TrajectoryScript : Script
    {
        public readonly string[] names;
        public TrajectoryScript(string name, List<ActionBase> list)
            : base(name, list)
        {

        }
    }
}
