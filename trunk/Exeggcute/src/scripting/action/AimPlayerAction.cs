using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class AimPlayerAction : ActionBase
    {
        static AimPlayerAction()
        {
            docs[typeof(AimPlayerAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "aimplayer" },
                { Info.Args, null},
                { Info.Description, 
"Instructs the entity to aim at the player's current position."},
                { Info.Example, null }
            };
        }
        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase  Copy()
        {
            return new AimPlayerAction();
        }
    }
}
