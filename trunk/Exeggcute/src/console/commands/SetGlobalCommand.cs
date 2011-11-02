using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    class SetGlobalCommand : ConsoleCommand
    {

         public static string Usage =
@"    
    setglobal PARAM VALUE  
                        Sets the global config varaible PARAM to the given 
                        VALUE.";

        public string Param { get; protected set; }
        public string Value { get; protected set; }

        public SetGlobalCommand(DevConsole console, string param, string value)
            : base(console)
        {
            this.Param = param;
            this.Value = value;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }
    }
}
