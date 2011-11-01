﻿using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class AimPlayerAction : ActionBase
    {
        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase  Copy()
        {
            return new AimPlayerAction();
        }
    }
}
