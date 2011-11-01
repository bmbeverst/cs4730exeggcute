using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class LoopAction : ActionBase
    {
        static LoopAction()
        {
            docs[typeof(LoopAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "loop (LINE)" },
                { Info.Args, 
@"optional int LINE
    The line in the script to jump to."},
                { Info.Description, 
@"Jumps the action pointer to the specified line number, or 0 if left
unspecified."},
                { Info.Example, 
@"loop 3
    Jump back to the third line in the script."}
            };
        }

        public int Pointer { get; protected set; }

        public LoopAction(int ptr)
        {
            this.Pointer = ptr;
        }

        public LoopAction()
        {
            this.Pointer = 0;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new LoopAction(Pointer);
        }
    }
}
