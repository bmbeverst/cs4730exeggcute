using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    class EntityInfo
    {
        public ModelName Model { get; protected set; }
        public ScriptName Script { get; protected set; }
        public List<EntityInfo> Children { get; protected set; }

        public EntityInfo(ModelName model, ScriptName script, List<EntityInfo> children)
        {
            Model = model;
            Script = script;
            Children = children;
        }
    }
}
