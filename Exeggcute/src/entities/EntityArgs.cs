using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.entities
{
    class EntityArgs
    {
        public Vector3 SpawnPosition { get; protected set; }
        public float AngleHeading { get; protected set; }

        public EntityArgs(Vector3 pos, float angle)
        {
            SpawnPosition = pos;
            AngleHeading = angle;
        }
    }
}
