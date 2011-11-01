using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.action
{
    class MoveAbsAction : ActionBase
    {
        public Float3 Destination { get; protected set; }
        public int Duration { get; protected set; }

        public MoveAbsAction(Float3 destination, int duration)
        {
            this.Destination = destination;
            this.Duration = duration;
        }

        public override void Process(ScriptedEntity entity)
        {
            entity.Process(this);
        }

        public override ActionBase Copy()
        {
            return new MoveAbsAction(Destination, Duration);
        }
    }
}
