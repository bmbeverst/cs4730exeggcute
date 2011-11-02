using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    enum Builtin
    {
        bg,
        fg

    }
    class BuiltinCommand : ConsoleCommand
    {
         public static string Usage =
@"    
    COMMAND  
                        Executes a short program or function.";


        public Builtin Command { get; protected set; }

        public BuiltinCommand(DevConsole console, Builtin cmd)
            : base(console)
        {
            this.Command = cmd;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }
    }
}
