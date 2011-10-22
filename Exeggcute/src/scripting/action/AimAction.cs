﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class AimAction : ActionBase
    {
        public FloatValue Angle { get; protected set; }
        public AimAction(FloatValue angle)
        {
            Angle = angle;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new AimAction(Angle);
        }
    }
}
