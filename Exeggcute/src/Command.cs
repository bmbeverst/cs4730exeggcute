using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

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
        public int Duration { get; protected set; }
        public Vector2 Target { get; protected set; }
        public bool IsDone
        {
            get { return Duration <= 0; }
        }

        protected Command(CommandType type, int duration, Vector2 target)
        {
            Type = type;
            Duration = duration;
            Target = target;
        }

        public static Command MoveTo(Vector2 target)
        {
            return new Command(CommandType.MoveTo, 0, target);
        }

    }
}
