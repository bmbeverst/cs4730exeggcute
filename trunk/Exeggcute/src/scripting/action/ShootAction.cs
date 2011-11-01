using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class ShootAction : ActionBase
    {
        static ShootAction()
        {
            docs[typeof(ShootAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "Shoot ID (FRAMES)" },
                { Info.Args, 
@"int ID
    The option in the entity's arsenal to begin firing.
optional int FRAMES
    The number of frames to fire for."},
                { Info.Description, 
@"Fires the specified option from the entity's arsenal for the given
duration, or indefinitely if no duration is specified."},
                { Info.Example, 
@"shoot 0 120
    Fire the 0th arsenal option for two seconds."}
            };
        }

        public FloatValue ID { get; protected set; }
        public int Duration { get; protected set; }

        public ShootAction(FloatValue id, int dur)
        {
            this.ID = id;
            this.Duration = dur;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new ShootAction(ID, Duration);
        }

    }
}
