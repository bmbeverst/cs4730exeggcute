using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class MoveRelAction : ActionBase
    {
        public Float3 Displacement { get; protected set; }
        public int Duration { get; protected set; }


        public MoveRelAction(Float3 displacement, int duration)
        {
            this.Displacement = displacement;
            this.Duration = duration;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new MoveRelAction(Displacement, Duration);
        }
    }
}
