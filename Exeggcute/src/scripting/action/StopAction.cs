using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class StopAction : ActionBase
    {
        static StopAction()
        {
            docs[typeof(StopAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "stop" },
                { Info.Args, null },
                { Info.Description, "Stops all motion for this entity."},
                { Info.Example, null }
            };
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new StopAction();
        }
    }
}
