using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class SetParamAction : ActionBase
    {
        public int ParamIndex { get; protected set; }
        public FloatValue Value { get; protected set; }
        
        public SetParamAction(string name, FloatValue value)
        {
            try
            {
                //HACK
                if (name.Equals("AngularVelocity"))
                {
                    value.FromDegrees();
                }
                this.ParamIndex = PlanarEntity3D.ParamMap[name];
            }
            catch (KeyNotFoundException knf)
            {
                throw new ParseError("{0}\n{1} is not a settable parameter", knf.Message, name);
            }
            this.Value = value;
        }

        public SetParamAction(int index, FloatValue value)
        {
            //HACK
            if (index == 4)
            {
                value = value.FromDegrees();
            }
            this.ParamIndex = index;
            this.Value = value;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new SetParamAction(ParamIndex, Value);
        }
    }
}
