using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class IfAction : ActionBase
    {
        static IfAction()
        {
            docs[typeof(IfAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "if PARAM OP VALUE then ACTION" },
                { Info.Args, 
@"string PARAM
    The entity's parameter to query.
string OP
    The comparison operator. Valid choices (<, >).
FloatValue VALUE
    The value to check against.
COMMAND
    The action to execute if the condition holds."},
                { Info.Description, 
@"Executes the given command if a specified condition holds.
Otherwise does nothing."},
                { Info.Example, 
@"if positionx <= 0 then loop
    If the entity's position is to the left of the x axis, then loop
    the action pointer to 0."}
            };
        }

        public int ParamIndex { get; protected set; }
        public string Op { get; protected set; }
        public FloatValue Value { get; protected set; }
        public ActionBase InnerAction { get; protected set; }

        public IfAction(int param, string op, FloatValue value, ActionBase inner)
        {
            this.ParamIndex = param;
            this.Op = op;
            this.Value = value;
            this.InnerAction = inner;
            Test(0);//makes sure our op is ok
        }

        public override ActionBase Copy()
        {
            return new IfAction(ParamIndex, Op, Value, InnerAction);
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public bool Test(float paramValue)
        {
            if (matches("<"))
            {
                return paramValue < Value.Value;
            }
            else if (matches(">"))
            {
                return paramValue > Value.Value;
            }
            else
            {
                throw new ParseError("Can't handle this operator \"{0}\"", Op);
            }
        }

        private bool matches(string input)
        {
            return Util.StrEq(Op, input);
        }
    }
}
