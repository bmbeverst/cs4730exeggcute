using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting.action
{
    class SpawnAction : ActionBase
    {
        public int ID { get; protected set; }
        public EntityArgs Args { get; protected set; }
        
        public SpawnAction(int id, EntityArgs args)
        {
            ID = id;
            Args = args;
        }

        public override void Process(CommandEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new SpawnAction(ID, Args);
        }
    }
}
