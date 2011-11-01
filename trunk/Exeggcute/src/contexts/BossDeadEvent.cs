using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.contexts
{
    class BossDeadEvent : ContextEvent
    {
        public override void Process()
        {
            World.Process(this);
        }
    }
}
