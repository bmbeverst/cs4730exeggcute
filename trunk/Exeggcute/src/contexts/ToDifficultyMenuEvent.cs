using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.contexts
{
    enum GameType
    {
        Campaign,
        Arcade,
        Custom
    }
    class ToDifficultyMenuEvent : ContextEvent
    {
        public GameType GameType { get; protected set; }

        public ToDifficultyMenuEvent(GameType type)
        {
            this.GameType = type;
        }

        public override void Process()
        {
            Worlds.World.Process(this);
        }  
    }
}
