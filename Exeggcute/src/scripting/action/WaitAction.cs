using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class WaitAction : ActionBase
    {

        static WaitAction()
        {
            docs[typeof(WaitAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "Wait FRAMES" },
                { Info.Args, 
@"int FRAMES
    The number of frames to delay before processing the next action"},
                { Info.Description, 
"Waits the specified number of frames before moving to the next action."},
                { Info.Example, 
@"wait 60
    Wait one second before processing the next action."}
            };
        }
        public int Duration { get; protected set; }
        public WaitAction(int duration)
        {
            this.Duration = duration;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new WaitAction(Duration);
        }
    }
}
