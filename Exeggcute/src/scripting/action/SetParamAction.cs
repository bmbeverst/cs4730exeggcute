using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class SetParamAction : ActionBase
    {
        static SetParamAction()
        {

            docs[typeof(SetParamAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "setparam PARAM VALUE" },
                { Info.Args, 
string.Format(@"string PARAM
    The parameter to be set. Valid values are
    {0}
FloatValue VALUE
    The value to set it to.", Util.Join(PlanarEntity3D.validParameters, ',')) },
                { Info.Description, 
@"Sets the given parameter to the specified value."},
                { Info.Example, 
@"setparam angle 90.
    Sets the entity's angle to 90 degrees."}
            };
        }

        public int ParamIndex { get; protected set; }
        public FloatValue Value { get; protected set; }
        
        public SetParamAction(string name, FloatValue value)
        {
            try
            {
                //HACK
                if (name.Equals("AngularVelocity"))
                {
                    //whaaat? FIXME
                    value.FromDegrees();
                }
                this.ParamIndex = PlanarEntity3D.ParamMap[name.ToLower()];
            }
            catch (KeyNotFoundException knf)
            {
                throw new ParseError("{0}\n{1} is not a settable parameter. Valid values are ({2})", knf.Message, name, Util.Join(PlanarEntity3D.validParameters, ','));
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
