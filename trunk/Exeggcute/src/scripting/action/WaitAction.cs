using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class WaitAction : ActionBase
    {
        public int Duration { get; protected set; }
        public WaitAction(int duration)
        {
            Duration = duration;
        }

        public override void Process(CommandEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            WaitAction result = new WaitAction(Duration);

            return result;
        }
    }
}
