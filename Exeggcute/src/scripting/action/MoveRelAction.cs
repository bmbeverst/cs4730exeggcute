using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class MoveRelAction : ActionBase
    {
        static MoveRelAction()
        {
            docs[typeof(MoveRelAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "moverel VEC FRAMES" },
                { Info.Args, 
@"Float3 VEC
    The relative amount to move.
int FRAMES
    The number of frames it hsould take for the move to complete."},
                { Info.Description, 
"Moves the entity by the specified vector int he given amount of frames."},
                { Info.Example, 
@"moverel (10,0,0) 60
    Move ten units to the right in one second."}
            };
        }
        
        public Float3 Displacement { get; protected set; }
        public int Duration { get; protected set; }


        public MoveRelAction(Float3 displacement, int duration)
        {
            this.Displacement = displacement;
            this.Duration = duration;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new MoveRelAction(Displacement, Duration);
        }
    }
}
