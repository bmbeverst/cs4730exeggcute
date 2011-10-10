using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src.entity
{
    class Projectile : PlanarEntity3D
    {
        public Projectile(float x, float y, float speed, float angle)
            : base(ModelName.testcube, new Vector3(x, y, 0))
        {
            Speed = speed;
            Angle = angle;
        }

    }
}
