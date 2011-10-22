using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class ShootAction : ActionBase
    {
        public FloatValue ID { get; protected set; }
        public int Duration { get; protected set; }
        public bool TurnOn { get; protected set; }

        public ShootAction(FloatValue id, int dur, bool on)
        {
            this.ID = id;
            this.Duration = dur;
            this.TurnOn = on;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new ShootAction(ID, Duration, TurnOn);
        }

    }
}
