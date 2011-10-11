using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.commands
{
    class ResetCommand : Command
    {
        public Vector3 Position { get; protected set; }
        public ResetCommand(Vector3 pos)
            : base(CommandType.Reset)
        {
            Position = pos;
        }
    }
}
