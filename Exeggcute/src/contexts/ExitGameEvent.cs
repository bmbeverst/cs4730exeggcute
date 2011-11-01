
namespace Exeggcute.src.contexts
{
    class ExitGameEvent : ContextEvent
    {
        public override void Process()
        {
            World.Process(this);
        }
    }
}
