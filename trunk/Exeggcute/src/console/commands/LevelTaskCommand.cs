using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.console.commands
{
    class LevelTaskCommand : ConsoleCommand
    {
        public static string Usage = 
@"
    LevelTask TASK      Sends TASK to be processed by the current context
                        immediately.";

        public string TaskString { get; protected set; }

        public LevelTaskCommand(DevConsole console, string taskString)
            : base(console)
        {
            this.TaskString = taskString;
        }

        public override void AcceptCommand(ConsoleContext context)
        {
            context.AcceptCommand(this);
        }
    }

}
