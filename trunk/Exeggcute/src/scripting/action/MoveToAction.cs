﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class MoveToAction : ActionBase
    {
        public Float3 Destination { get; protected set; }
        public int Duration { get; protected set; }

        public MoveToAction(Float3 destination, int duration)
        {
            this.Destination = destination;
            this.Duration = duration;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new MoveToAction(Destination, Duration);
        }
    }
}
