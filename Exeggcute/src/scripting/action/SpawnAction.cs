using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    enum AngleType
    {
        Abs,
        Rel
    }

    class SpawnAction : ActionBase
    {

        static SpawnAction()
        {
            docs[typeof(SpawnAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "spawn TYPE ANGLE" },
                { Info.Args, 
@"AngleType TYPE
    Whether or not the angle specified is relative to the spawner or
    absolute. Valid values are abs,rel.
FloatValue ANGLE
    The angle at which to aim the child entity."},
                { Info.Description, 
@"Spawns a shot with a given angular offset, which can be either 
absolute or relative to the entity's spawner's mover's current angle."},
                { Info.Example, 
@"spawn rel 45
    spawn a shot at 45 degrees relative to the entity's mover's angle."}
            };
        }
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
