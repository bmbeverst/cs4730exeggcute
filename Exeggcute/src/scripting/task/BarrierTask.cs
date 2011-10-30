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
        public BarrierType Type { get; protected set; }

        public BarrierTask(BarrierType type)
        {
            this.Type = type;
        }

        public override void Process(Level level)
        {
            level.Process(this);
        }
    }
}
