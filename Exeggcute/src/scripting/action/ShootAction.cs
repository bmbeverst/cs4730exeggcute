using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class ShootAction : ActionBase
    {
        public int ID { get; protected set; }
        public int Duration { get; protected set; }
        public bool Switch { get; protected set; }

        public ShootAction(int id, int dur, bool on)
        {
            this.ID = id;
            this.Duration = dur;
            this.Switch = on;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new ShootAction(ID, Duration, Switch);
        }

    }
}
