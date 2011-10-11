using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.commands
{
    class ShootCommand : Command
    {
        public int ShotIndex { get; protected set; }

        public ShootCommand(int shotIndex)
            : base(CommandType.Shoot)
        {
            ShotIndex = shotIndex;
        }

    }
}
