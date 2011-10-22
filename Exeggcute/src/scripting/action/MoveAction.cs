using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class MoveAction : ActionBase
    {
        public FloatValue Speed { get; protected set; }
        public FloatValue AngularVelocity { get; protected set; }
        public FloatValue LinearAccel { get; protected set; }
        public FloatValue AngularAccel { get; protected set; }
        public FloatValue VelocityZ { get; protected set; }

        public MoveAction(FloatValue speed,
                          FloatValue angularVelocity,
                          FloatValue linearAccel,
                          FloatValue angularAccel,
                          FloatValue velocityZ)
        {
            this.Speed = speed;
            this.AngularVelocity = angularVelocity;
            this.LinearAccel = linearAccel;
            this.AngularAccel = angularAccel;
            this.VelocityZ = velocityZ;
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
