using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class DeleteAction : ActionBase
    {

        public DeleteAction()
        {

        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
 	        return new DeleteAction();
        }
    }
}
