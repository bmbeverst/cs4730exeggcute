using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    class PackageCommand : ConsoleCommand
    {
        public static string Usage =
@"    
    Package NAME        Package the currently loaded data into a distributable
                        .dat file with name NAME.dat";

        public string Name { get; protected set; }
        public PackageCommand(DevConsole devConsole, string name)
            :base(devConsole)
        {
            this.Name = name;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }

    }
}
