using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    class ReloadCommand : ConsoleCommand
    {
         public static string Usage =
@"    
    Reload TYPE NAME    Reloads the file of type TYPE with name NAME from
                        disk, refreshing dependencides when necessary";

        public ReloadCommand(DevConsole devConsole)
            : base(devConsole)
        {

        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }
    }
}
