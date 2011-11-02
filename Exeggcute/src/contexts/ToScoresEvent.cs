
namespace Exeggcute.src.contexts
{
    class ToScoresEvent : ContextEvent
    {
        public override void Process()
        {
            Worlds.World.Process(this);
        }
    }
}
