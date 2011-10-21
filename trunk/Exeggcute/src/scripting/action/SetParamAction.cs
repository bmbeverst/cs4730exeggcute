using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class SetParamAction : ActionBase
    {
        public string ParamName { get; protected set; }
        public object Value { get; protected set; }

        public SetParamAction(string name, object value)
        {
            ParamName = name;
            Value = value;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new SetParamAction(ParamName, Value);
        }
    }
}
