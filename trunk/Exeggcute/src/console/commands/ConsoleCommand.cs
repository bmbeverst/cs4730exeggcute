﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    enum Keyword
    {
        Help,
        Go,
        Spawn,
        Load,
        Package,
        List,
        Reset,
        Exit
    }

    abstract class ConsoleCommand
    {
        protected static string StaticUsage = 
@"
This member is implemented in a static constructor and was either referenced
too early in execution, or was not initialized properly.";

        public abstract void AcceptCommand(ConsoleContext context);
        public DevConsole devConsole;

        public ConsoleCommand(DevConsole devConsole)
        {
            this.devConsole = devConsole;
        }
    }
}