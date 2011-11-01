using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.task
{
    class KillAllTask : Task
    {
        static KillAllTask()
        {
            docs[typeof(KillAllTask)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "KillAll" },
                { Info.Args, null},
                { Info.Description, "Kill all entities in the world instantly."},
                { Info.Example, null }
            };
        }

        public override void Process(Sandbox level)
        {
            level.Process(this);
        }
    }
}
