using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    class WhatIsCommand : ConsoleCommand
    {

        public WhatIsCommand(DevConsole devConsole)
            : base(devConsole)
        {
            
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }
    }
}
