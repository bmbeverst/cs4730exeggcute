using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class MoveToAction : ActionBase
    {
        public Vector3 Destination { get; protected set; }
        public int Duration { get; protected set; }

        public MoveToAction(Vector3 destination, int duration)
        {
            Destination = destination;
            Duration = duration;
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
