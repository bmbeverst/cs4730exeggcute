
namespace Exeggcute.src.contexts
{
    class BossDeadEvent : ContextEvent
    {
        public override void Process()
        {
            World.Process(this);
        }
    }
}
