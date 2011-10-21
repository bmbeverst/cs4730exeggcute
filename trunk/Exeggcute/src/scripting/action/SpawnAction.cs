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
        public float Distance { get; protected set; }
        public float Angle { get; protected set; }
        
        public SpawnAction(float distance, float angle)
        {
            this.Distance = distance;
            this.Angle = angle;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new SpawnAction(Distance, Angle);
        }
    }
}
