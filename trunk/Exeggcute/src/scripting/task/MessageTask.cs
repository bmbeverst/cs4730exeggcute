using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.task
{
    class MessageTask : Task
    {
        public int ID { get; protected set; }

        public MessageTask(int id)
        {
            ID = id;
        }

        public override void Process(Level level)
        {
            level.Process(this);
        }
    }
}
