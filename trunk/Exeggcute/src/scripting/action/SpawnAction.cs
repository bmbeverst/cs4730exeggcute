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
        public float AngleOffset { get; protected set; }
        public float Distance { get; protected set; }
        public int ShotID { get; protected set; }
        
        public SpawnAction(float angleDeg, float distance, int id)
        {
            AngleOffset = angleDeg * FastTrig.degreesToRadians;
            Distance = distance;
            ShotID = id;
        }

        public override void Process(CommandEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new SpawnAction(AngleOffset, Distance, ShotID);
        }
    }
}
