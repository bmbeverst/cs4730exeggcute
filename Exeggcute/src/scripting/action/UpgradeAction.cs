using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting.action
{
    class UpgradeAction : ActionBase
    {
        static UpgradeAction()
        {
            docs[typeof(UpgradeAction)] = new Dictionary<Info, string> 
            {
                { Info.Syntax, "upgrade MAX" },
                { Info.Args, 
@"int MAX
    The maximum level to go up to."},
                { Info.Description, 
@"Upgrades the entity's weapon until MAX is reached, at which point
it will loop back to zero."},
                { Info.Example, 
@"upgrade 5
    Upgrade's the entity's weapon until it reaches 5, where it will reset."}
            };
        }
        public int Max { get; protected set; }

        public UpgradeAction(int max)
        {
            this.Max = max;
        }

        public override void Process(entities.ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new UpgradeAction(Max);
        }
    }
}
