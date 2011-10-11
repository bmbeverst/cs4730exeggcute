using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.commands
{
    enum CommandType
    {
        MoveTo,
        Move,
        Reset,
        SetPos,
        Wait,
        Vanish,
        Shoot
    }


    abstract class Command
    {
        public CommandType Type { get; protected set; }

        public Command(CommandType type)
        {
            Type = type;
        }

    }
}
