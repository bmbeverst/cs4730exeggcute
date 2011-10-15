using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.task
{
    class WaitTask : Task
    {
        public int Duration { get; protected set; }

        public WaitTask(int duration)
        {
            Duration = duration;
        }

        public override void Process(Level level)
        {
            level.Process(this);
        }
    }
}
