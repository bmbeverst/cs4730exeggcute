using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.task
{
    class BossTask : Task
    {
        static BossTask()
        {
            docs[typeof(BossTask)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "boss" },
                { Info.Args, null },
                { Info.Description, "Spawns the level's boss in the current level"},
                { Info.Example, 
@"boss
    Spawns a boss."}
            };
        }
        public override void Process(Sandbox level)
        {
            level.Process(this);
        }
    }
}
