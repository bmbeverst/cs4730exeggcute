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
        public Shot(ModelName model, ScriptName script, HashList<Shot> shotList)
            : base(model, script, new List<Shot>(), shotList)
        {

        }

        public Shot Clone(Vector3 pos, float angle)
        {
            Shot clone = new Shot(Name, Script, ShotList);
            clone.Angle = angle;
            clone.Position = pos;
            return clone;
        }

    }
}
