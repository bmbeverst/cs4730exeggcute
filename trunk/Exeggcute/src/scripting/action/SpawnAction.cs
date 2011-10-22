using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting.action
{
    enum AngleType
    {
        Abs,
        Rel
    }
    class SpawnAction : ActionBase
    {
        public FloatValue Angle { get; protected set; }
        public AngleType Type { get; protected set; }

        public SpawnAction(FloatValue angle, AngleType type)
        {
            this.Angle = angle;
            this.Type = type;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new SpawnAction(Angle, Type);
        }
    }
}
