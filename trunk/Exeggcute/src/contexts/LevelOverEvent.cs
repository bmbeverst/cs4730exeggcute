
namespace Exeggcute.src.contexts
{
    class LevelOverEvent : ContextEvent
    {
        public override void Process()
        {
            World.Process(this);
        }
    }
}
