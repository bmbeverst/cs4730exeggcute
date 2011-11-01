
namespace Exeggcute.src.contexts
{
    class PauseEvent : ContextEvent
    {
        public override void Process()
        {
            World.Process(this);
        }       
    }
}
