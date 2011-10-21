using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class MoveAction : ActionBase
    {
        public float Speed { get; protected set; }
        public float AngularVelocity { get; protected set; }
        public float LinearAccel { get; protected set; }
        public float AngularAccel { get; protected set; }
        public float VelocityZ { get; protected set; }

        public MoveAction(float speed = 0,
                          float angularVelocity = 0,
                          float linearAccel = 0,
                          float angularAccel = 0,
                          float velocityZ = 0)
        {
            Speed = speed;
            AngularVelocity = angularVelocity;
            LinearAccel = linearAccel;
            AngularAccel = angularAccel;
            VelocityZ = velocityZ;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new MoveAction(Speed, AngularVelocity, LinearAccel, AngularAccel, VelocityZ);
        }
    }
}
