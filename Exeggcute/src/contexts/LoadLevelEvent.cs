using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.contexts
{
    class LoadLevelEvent : ContextEvent
    {
        public string LevelName { get; protected set; }
        public string PlayerName { get; protected set; }

        public LoadLevelEvent(string levelName, string playerName)
        {
            this.LevelName = levelName;
            this.PlayerName = playerName;
        }

        public override void Process()
        {
            World.Process(this);
        }
    }
}
