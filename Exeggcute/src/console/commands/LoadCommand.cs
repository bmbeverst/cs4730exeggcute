using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    class LoadCommand : ConsoleCommand
    {

        public static string Usage = 
@"
    Load DATASET        Loads the primary dataset to be DATASET and saves this
                        the the manifest and then restarts the engine. 
";

                        
        public string Name { get; protected set; }

        public LoadCommand(DevConsole console, string name)
            : base(console)
        {
            this.Name = name;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }

    }
}
