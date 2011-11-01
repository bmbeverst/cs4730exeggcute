using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class LoopAction : ActionBase
    {
        public int Pointer { get; protected set; }

        public LoopAction(int ptr)
        {
            this.Pointer = ptr;
        }

        public LoopAction()
        {
            this.Pointer = 0;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new LoopAction(Pointer);
        }
    }
}
