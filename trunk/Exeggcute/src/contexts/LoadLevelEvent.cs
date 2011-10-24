﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.contexts
{
    class LoadLevelEvent : ContextEvent
    {
        public string Name { get; protected set; }

        public LoadLevelEvent(string name)
        {
            this.Name = name;
        }

        public override void Process()
        {
            World.Process(this);
        }
    }
}
