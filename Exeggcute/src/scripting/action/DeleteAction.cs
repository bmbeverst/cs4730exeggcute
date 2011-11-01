using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class DeleteAction : ActionBase
    {

        static DeleteAction()
        {
            docs[typeof(DeleteAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "delete" },
                { Info.Args, null},
                { Info.Description, 
"Instructs the level to remove this entity from the world as soon as possible."},
                { Info.Example, null }
            };
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
 	        return new DeleteAction();
        }
    }
}
