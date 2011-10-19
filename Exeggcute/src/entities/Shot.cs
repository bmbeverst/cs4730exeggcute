using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.arsenal;

namespace Exeggcute.src.entities
{
    class Shot : CommandEntity
    {

        public int Damage { get; protected set; }

        /*public Shot(ModelName modelName, ScriptName scriptName)
            : base(modelName, scriptName)
        {
            Damage = 10;
            this.modelName = modelName;
            this.scriptName = scriptName;
        }*/
        protected ArsenalEntry arsenalParams;
        public Shot(ArsenalEntry entry)
            : base(entry.ModelName, entry.ShotBehaviorScriptName)
        {
            Damage = 10;
            this.arsenalParams = entry;
        }

        public Shot Clone(Vector3 pos, float angle)
        {
            Shot clone = new Shot(arsenalParams);
            clone.Angle = angle;
            clone.Position = pos;
            return clone;
        }

        public virtual void Collide(CommandEntity entity)
        {
            IsTrash = true;
        }

    }
}
