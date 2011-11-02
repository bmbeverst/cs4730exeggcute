using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.gui;

namespace Exeggcute.src.contexts
{
    class ReallyQuitEvent : ContextEvent
    {
        public QuitType Type { get; protected set; }

        public ReallyQuitEvent(QuitType type)
        {
            this.Type = type;
        }

        public override void Process()
        {
            Worlds.World.Process(this);
        }     
    }
}
