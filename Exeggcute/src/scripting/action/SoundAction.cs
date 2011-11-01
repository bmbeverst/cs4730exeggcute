using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class SoundAction : ActionBase
    {
        static SoundAction()
        {
            docs[typeof(SoundAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "sound" },
                { Info.Args, null },
                { Info.Description, 
@"Plays the sound associated with this enetity. Usually used by spawners."},
                { Info.Example, null }
            };
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            Util.Warn("dangerous!");
            return this;
        }
    }
}
