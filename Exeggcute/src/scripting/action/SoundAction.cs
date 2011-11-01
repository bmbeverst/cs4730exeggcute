﻿using Exeggcute.src.entities;
namespace Exeggcute.src.scripting.action
{
    class SoundAction : ActionBase
    {

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            Util.Warn("dangerous!");
            return this;
        }
    }
}
