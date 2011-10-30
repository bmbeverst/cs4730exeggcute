using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    class ContextCommand : ConsoleCommand
    {
        

        public static string Usage = 
@"Usage: Context
    Context NAME        Change contexts either to the file named NAME or 
                        to the sandbox by specifying 'sandbox' as the name.";

        public string Name { get; protected set; }

        public ContextCommand(DevConsole console, string name)
            : base(console)
        {
            this.Name = name;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this as ContextCommand);
            World.ContextSwitch(Name);
        }
    }
}
