using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class ShootAction : ActionBase
    {
        public int ShotIndex { get; protected set; }
        public bool Start { get; protected set; }

        public ShootAction(int shotIndex)
        {
            ShotIndex = shotIndex;
            Start = true;
        }

        public ShootAction()
        {
            Start = false;
        }

        public override void Process(CommandEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            ShootAction result = new ShootAction();
            result.Start = this.Start;
            result.ShotIndex = this.ShotIndex;
            return result;
        }

    }
}
