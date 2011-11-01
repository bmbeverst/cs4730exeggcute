
namespace Exeggcute.src.scripting.action
{
    class UpgradeAction : ActionBase
    {
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
