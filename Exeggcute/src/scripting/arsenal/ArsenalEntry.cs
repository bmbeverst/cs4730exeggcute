using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting.arsenal
{
    class ArsenalEntry
    {
        public ModelName ModelName { get; protected set; }
        public ScriptName ScriptName { get; protected set; }
        public ArsenalEntry(ModelName modelName, ScriptName scriptName)
        {
            ModelName = modelName;
            ScriptName = scriptName;
        }
    }
}
