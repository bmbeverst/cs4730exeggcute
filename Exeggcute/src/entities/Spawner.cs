using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.entities
{
    class Spawner : CommandEntity
    {
        public EntityArgs Args { get; protected set; }
        public Spawner(ScriptName script, ArsenalName arsenalName, EntityArgs args, HashList<Shot> shotList)
            : base(script, arsenalName, shotList)
        {
            Args = args;
        }

        public Spawner(ScriptName script, ArsenalName arsenalName, HashList<Shot> shotList)
            : base(script, arsenalName, shotList)
        {

        }
        public void Follow(CommandEntity entity, bool relAngle)
        {
            Position = entity.Position + Args.SpawnPosition;
            if (relAngle)
            {
                Angle = entity.Angle + Args.AngleHeading;
            }
            else
            {
                Angle = Args.AngleHeading;
            }
        }
    }
}
