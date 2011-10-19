using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting.action;
using Exeggcute.src.scripting.arsenal;

namespace Exeggcute.src.entities
{
    /// <summary>
    /// A spawner is an entity which spawns shots. This is separate from the
    /// entity which is shooting because it may have its own movement/behavior
    /// script.
    /// </summary>
    class Spawner : CommandEntity
    {
        public EntityArgs Args { get; protected set; }
        public Vector3 PosOffset { get; protected set; }
        public float AngleOffset { get; protected set; }

        protected Arsenal arsenal;

        public Spawner(ScriptName script, ArsenalName arsenalName, Vector3 posOffset, float angleOffset, HashList<Shot> shotList)
            : base(script, arsenalName, shotList)
        {
            PosOffset = posOffset;
            AngleOffset = angleOffset;
            this.arsenal = ArsenalBank.Get(arsenalName);
        }

        public Spawner(ScriptName script, ArsenalName arsenalName, HashList<Shot> shotList)
            : base(script, arsenalName, shotList)
        {
            this.arsenal = ArsenalBank.Get(arsenalName);
        }

        public override void Process(SpawnAction spawn)
        {
            EntityArgs args = spawn.Args;
            float angle = args.AngleHeading + Angle;
            Vector3 pos = Position + args.SpawnPosition;
            Shot cloned = arsenal.Clone(spawn.ID, pos, angle);
            shotListHandle.Add(cloned);
            actionPtr += 1;
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
