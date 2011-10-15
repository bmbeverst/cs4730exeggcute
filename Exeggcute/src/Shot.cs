using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.arsenal;

namespace Exeggcute.src
{
    class Shot : CommandEntity
    {
        protected ScriptName scriptName;
        protected ModelName modelName;

        public bool IsDestroyed { get; protected set; }

        public int Damage { get; protected set; }

        public Shot(ModelName modelName, ScriptName scriptName)
            : base(modelName, scriptName)
        {
            Damage = 10;
            this.modelName = modelName;
            this.scriptName = scriptName;
        }

        public Shot(ArsenalEntry entry)
            : base(entry.ModelName, entry.ScriptName)
        {
            Damage = 10;
            this.modelName = entry.ModelName;
            this.scriptName = entry.ScriptName;
        }

        public Shot Clone(Vector3 pos, float angle)
        {
            Shot clone = new Shot(modelName, scriptName);
            clone.Angle = angle;
            clone.Position = pos;
            return clone;
        }

        public virtual void Collide(CommandEntity entity)
        {
            IsDestroyed = true;
        }

    }
}
