using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entity;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;

namespace Exeggcute.src
{
    class Shot : PlanarEntity3D
    {
        public Shot(ModelName name, Vector3 pos)
            : base(name, pos)
        {

        }

        public Shot Clone(Vector3 pos, float angle, float speed)
        {
            Shot clone = new Shot(Name, pos);
            clone.Angle = angle;
            clone.Speed = speed;
            return clone;
        }
    }
}
