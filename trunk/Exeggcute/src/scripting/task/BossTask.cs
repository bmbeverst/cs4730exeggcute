using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.task
{
    class BossTask : Task
    {
        public Float3 Position { get; protected set; }

        public BossTask(Float3 pos)
        {
            this.Position = pos;
        }

        public override void Process(Level level)
        {
            level.Process(this);
        }
    }
}
