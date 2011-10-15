using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.task
{
    class SpawnTask : Task
    {
        public int ID { get; protected set; }
        public EntityArgs Args { get; protected set; }

        public SpawnTask(int id, EntityArgs args)
        {
            ID = id;
            Args = args;
        }

        public override void Process(Level level)
        {
            level.Process(this);
        }


    }
}
