using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.commands
{
    class MoveCommand : Command
    {
        public float Angle { get; protected set; }
        public float Speed { get; protected set; }
        public float AngularVelocity { get; protected set; }
        public float LinearAccel { get; protected set; }
        public float AngularAccel { get; protected set; }
        public float VelocityZ { get; protected set; }

        public MoveCommand(float angle = 0,
                           float speed = 0,
                           float angularVelocity = 0,
                           float linearAccel = 0,
                           float angularAccel = 0,
                           float velocityZ = 0)
            : base(CommandType.Move)
        {
            Angle = angle;
            Speed = speed;
            AngularVelocity = angularVelocity;
            LinearAccel = linearAccel;
            AngularAccel = angularAccel;
            VelocityZ = velocityZ;
        }
    }
}
