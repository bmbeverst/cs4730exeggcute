using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    class ClearCommand : ConsoleCommand
    {
        public static string Usage =
@"    
    clear THING  
                        Clears THING where thing is (trackers,console,all). If
                        no THING is specified, clears the console.";


        public string Thing { get; protected set; }

        public ClearCommand(DevConsole console, string thing)
            : base(console)
        {
            this.Thing = thing;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }
    }
}
