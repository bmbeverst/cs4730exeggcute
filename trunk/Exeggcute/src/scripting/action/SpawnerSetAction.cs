using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class SpawnerSetAction : ActionBase
    {
        public Vector3 RelPosition { get; protected set; }
        public float Angle { get; protected set; }

        protected float distance;
        protected float posAngle;

        public SpawnerSetAction(float posAngle, float distance, float aimAngle)
        {
            RelPosition = new Vector3(distance * FastTrig.Cos(posAngle),
                                      distance * FastTrig.Sin(posAngle),
                                      0);

            Angle = aimAngle;

            this.posAngle = posAngle;
            this.distance = distance;

        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new SpawnerSetAction(posAngle, distance, Angle);
        }
    }
}
