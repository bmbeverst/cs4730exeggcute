using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.task
{
    class ClearTask : Task
    {
        static ClearTask()
        {
            docs[typeof(ClearTask)] = new Dictionary<Info,string> 
            {
                { Info.Syntax, "Clear" },
                { Info.Args, null },
                { Info.Description, "Deletes all managed entities currently in the world"},
                { Info.Example, 
@"clear
    All Enemies, shots, gibs, etc are removed from the world."}
            };
        }

        public ClearTask()
        {

        }

        public override void Process(Sandbox level)
        {
            level.Process(this);
        }
    }
}
