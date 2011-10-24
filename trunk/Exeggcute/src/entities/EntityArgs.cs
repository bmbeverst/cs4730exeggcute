using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.entities
{
    class EntityArgs
    {
        public Float3 SpawnPosition { get; protected set; }
        public FloatValue AngleHeading { get; protected set; }

        public EntityArgs(Float3 pos, FloatValue angle)
        {
            SpawnPosition = pos;
            AngleHeading = angle;
        }
    }
}
