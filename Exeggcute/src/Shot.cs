using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;

namespace Exeggcute.src
{
    class Shot : CommandEntity
    {
        protected ScriptName scriptName;
        protected ModelName modelName;
        public Shot(ModelName modelName, ScriptName scriptName)
            : base(modelName, scriptName)
        {
            this.scriptName = scriptName;
            this.modelName = modelName;
        }

        public Shot Clone(Vector3 pos, float angle)
        {
            Shot clone = new Shot(modelName, scriptName);
            clone.Angle = angle;
            clone.Position = pos;
            return clone;
        }

    }
}
