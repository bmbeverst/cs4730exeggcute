using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.entities
{
    class Spawner : CommandEntity
    {
        public EntityArgs Args { get; protected set; }
        public Vector3 PosOffset { get; protected set; }
        public float AngleOffset { get; protected set; }
        public Spawner(ScriptName script, ArsenalName arsenalName, Vector3 posOffset, float angleOffset, HashList<Shot> shotList)
            : base(script, arsenalName, shotList)
        {
            PosOffset = posOffset;
            AngleOffset = angleOffset;
        }

        public Spawner(ScriptName script, ArsenalName arsenalName, HashList<Shot> shotList)
            : base(script, arsenalName, shotList)
        {

        }

        public void SetParams(Vector3 pos, float angle)
        {
            PosOffset = pos;
            AngleOffset = angle;
        }

        public void Follow(CommandEntity parent, bool lockPos, bool lockAngle)
        {
            if (lockPos)
                Position = parent.Position + PosOffset;

            if (lockAngle)
                Angle = parent.Angle + AngleOffset;

        }
    }
}
