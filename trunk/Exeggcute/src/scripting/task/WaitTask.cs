using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.task
{
    class WaitTask : Task
    {
        static WaitTask()
        {
            docs[typeof(WaitTask)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "Wait FRAMES" },
                { Info.Args, 
@"int FRAMES
    The number of frames to delay before processing the next task."},
                { Info.Description, "Deletes all managed entities currently in the world"},
                { Info.Example, 
@"wait 60
    Wait one second before processing the next task."}
            };
        }
        public int Duration { get; protected set; }

        public WaitTask(int duration)
        {
            Duration = duration;
        }

        public override void Process(Sandbox level)
        {
            level.Process(this);
        }
    }
}
