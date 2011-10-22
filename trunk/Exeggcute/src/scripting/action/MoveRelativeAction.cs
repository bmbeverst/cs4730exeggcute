﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class MoveRelativeAction : ActionBase
    {
        public Float3 Displacement { get; protected set; }
        public int Duration { get; protected set; }


        public MoveRelativeAction(Float3 displacement, int duration)
        {
            this.Displacement = displacement;
            this.Duration = duration;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new MoveRelativeAction(Displacement, Duration);
        }
    }
}
