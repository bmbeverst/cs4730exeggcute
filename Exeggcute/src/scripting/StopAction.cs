using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting
{
    class StopAction : ActionBase
    {
        public StopAction()
        {
            
        }

        public override void Process(CommandEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new StopAction();
        }
    }
}
