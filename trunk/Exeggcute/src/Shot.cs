using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;

namespace Exeggcute.src
{
    class Shot : CommandEntity
    {
        public Shot(ModelName model, ScriptName script)
            : base(model, script)
        {

        }

        public Shot Clone(Vector3 pos, float angle, float speed)
        {
            Shot clone = new Shot(Name, Script);
            clone.Angle = angle;
            clone.Speed = speed;
            clone.Position = pos;
            return clone;
        }
    }
}
