using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.task
{
    enum BarrierType
    {
        FadeOut
    }
    class BarrierTask : Task
    {
        static BarrierTask()
        {
            docs[typeof(BarrierTask)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "Barrier TYPE" },
                { Info.Args, 
@"BarrierType TYPE
    The type of barrier to erect."},
                { Info.Description, 
@"Erects a barrier in the task list which an only be passed when a particular
condition is met."},
                { Info.Example, 
@"Barrier FadeOut
    Wait until the current song is finished fading before processing the next
    task."}
            };
        }

        public BarrierType Type { get; protected set; }

        public BarrierTask(BarrierType type)
        {
            this.Type = type;
        }

        public override void Process(Sandbox level)
        {
            level.Process(this);
        }
    }
}
