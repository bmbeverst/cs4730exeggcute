﻿using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    enum AngleType
    {
        Abs,
        Rel
    }

    class SpawnAction : ActionBase
    {
    
        public AngleType Type { get; protected set; }
        public FloatValue Angle { get; protected set; }
        

        public SpawnAction(AngleType type,FloatValue angle)
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
            return new SpawnAction(Type, Angle);
        }
    }
}
