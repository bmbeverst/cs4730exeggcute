using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    class ResetCommand : ConsoleCommand
    {
        public static string Usage =
@"    
    Reset               Reset the entire game by reinstantiating the engine  
                        object. This *should* clear all state and be otherwise
                        identical to closing and rebooting the game.";

        public ResetCommand(DevConsole devConsole)
            : base(devConsole)
        {

        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }

    }
}
