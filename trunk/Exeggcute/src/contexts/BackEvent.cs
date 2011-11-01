
namespace Exeggcute.src.contexts
{
    class BackEvent : ContextEvent
    {
        public override void Process()
        {
            World.Process(this);
        }
    }

}
