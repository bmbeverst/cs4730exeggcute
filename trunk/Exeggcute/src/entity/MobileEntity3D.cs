using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;

namespace Exeggcute.src.entity
{
    class MobileEntity3D : Entity3D
    {
        public Vector3 PrevPosition { get; protected set; }
        public Vector3 Velocity { get; protected set; }

        public MobileEntity3D(ModelName name, Vector3 pos)
            : base(name, pos)
        {

        }

        public override void Update()
        {
            PrevPosition = Position;
            Position += Velocity;
        }
    }
}
