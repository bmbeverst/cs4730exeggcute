using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.contexts
{
    class ExitGameEvent : ContextEvent
    {
        public override void Process()
        {
            Worlds.World.Process(this);
        }
    }
}
