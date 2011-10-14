using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting.task
{
    class SpawnTask : Task
    {
        public int ID { get; protected set; }
        public SpawnTask(int id)
        {
            ID = id;
        }

        public override void Process(Level level)
        {
            level.Process(this);
        }


    }
}
