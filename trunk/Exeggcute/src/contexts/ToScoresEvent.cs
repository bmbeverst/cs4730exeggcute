
namespace Exeggcute.src.contexts
{
    class ToScoresEvent : ContextEvent
    {
        public override void Process()
        {
            World.Process(this);
        }
    }
}
