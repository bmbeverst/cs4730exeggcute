using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    /// <summary>
    /// Set the entity's position to the given position.
    /// </summary>
    class SetAction : ActionBase
    {
        public Vector3 Position { get; protected set; }
        public SetAction(Vector3 pos)
        {
            Position = pos;
        }

        public SetAction(Vector2 pos)
        {
            Position = new Vector3(pos.X, pos.Y, 0);
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            SetAction result = new SetAction(Position);
            return result;
        }
    }
}
