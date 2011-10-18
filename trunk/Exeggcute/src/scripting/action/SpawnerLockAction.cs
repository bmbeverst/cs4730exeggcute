using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class SpawnerLockAction : ActionBase
    {
        public bool LockPosition { get; protected set; }
        public bool LockAngle { get; protected set; }

        public SpawnerLockAction(bool lockPosition, bool lockAngle)
        {
            LockPosition = lockPosition;
            LockAngle = lockAngle;
        }

        public override void Process(CommandEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new SpawnerLockAction(LockPosition, LockAngle);
        }
    }
}
