using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src
{
    enum CommandType
    {
        MoveTo,
        Wait
    }

    class Command
    {
        public CommandType Type { get; protected set; }
    }
}
