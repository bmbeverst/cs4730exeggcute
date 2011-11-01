using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class MoveAbsAction : ActionBase
    {
        static MoveAbsAction()
        {
            docs[typeof(MoveAbsAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "moveabs POS FRAMES" },
                { Info.Args, 
@"Float3 POS
    The position in levle space to move to.
int FRAMES
    The number of frames it should take to get there."},
                { Info.Description, 
@"Instructs the entity to move the the specified position in the 
specified number of frames."},
                { Info.Example, 
@"moveabs (0,0,0) 60
    Moves the entity to the center of the screen over one second."}
            };
        }

        public Float3 Destination { get; protected set; }
        public int Duration { get; protected set; }

        public MoveAbsAction(Float3 destination, int duration)
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
            return new MoveAbsAction(Destination, Duration);
        }
    }
}
