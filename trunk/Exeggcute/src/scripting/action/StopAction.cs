using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class StopAction : ActionBase
    {
        public StopAction()
        {
            
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new StopAction();
        }
    }
}
