using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.commands
{
    class WaitCommand : Command
    {
        public int Duration { get; protected set; }
        public WaitCommand(int duration)
            : base(CommandType.Wait)
        {
            Duration = duration;
        }
    }
}
