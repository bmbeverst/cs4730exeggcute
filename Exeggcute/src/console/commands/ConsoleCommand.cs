﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    abstract class ConsoleCommand
    {
        protected static string StaticUsage = 
@"This member is implemented in a static constructor and was either referenced
too early in execution, or was not initialized properly.";

        private static string Header =
@"A console command is an instruction processed by the developer console.
Available commands include:
";

        public abstract void AcceptCommand(ConsoleContext context);
        public DevConsole devConsole;

        public ConsoleCommand(DevConsole devConsole)
        {
            this.devConsole = devConsole;
        }

        public static string MakeDocs(DevConsole console)
        {
            return Header + HelpCommand.MakeAll(console).Output;
        }
    }
}