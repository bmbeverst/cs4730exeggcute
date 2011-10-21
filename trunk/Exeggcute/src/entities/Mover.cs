using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.entities
{
    class Mover : ScriptedEntity
    {
        public Mover(BehaviorScript moveBehavior)
            : base(moveBehavior)
        {

        }

        public void Update(Vector3 parentPos, float parentAngle)
        {
            Position = parentPos;
            Angle = parentAngle;
            base.Update();
        }
    }
}
