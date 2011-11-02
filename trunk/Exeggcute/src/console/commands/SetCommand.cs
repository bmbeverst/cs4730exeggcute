using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    class SetCommand : ConsoleCommand
    {
        public static string Usage =
@"    
    Set ID PARAM VALUE  
                        Sets parameter PARAM to VALUE for the entity whose id 
                        is ID. Try 'whatis param' for a list of settable 
                        parameters.";

        public int ID { get; protected set; }
        public int ParamIndex { get; protected set; }
        public FloatValue Value { get; protected set; }

        public SetCommand(DevConsole console, int id, int paramIndex, FloatValue value)
            : base(console)
        {
            this.ID = id;
            this.ParamIndex = paramIndex;
            this.Value = value;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }
    }
}
