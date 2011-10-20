using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.contexts
{
    enum ScoreEventType
    {
        SeeLocal,
        SeeNetwork,
        Submit
    }
    class ScoreEvent : ContextEvent
    {
        public ScoreEventType Type { get; protected set; }
        public ScoreEvent(ScoreEventType type)
        {
            Type = type;
        }
        public override void Process()
        {
            World.Process(this);
        }
    }
}
