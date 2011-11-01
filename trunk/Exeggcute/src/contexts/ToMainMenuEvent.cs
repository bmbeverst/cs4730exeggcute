
namespace Exeggcute.src.contexts
{
    class ToMainMenuEvent : ContextEvent
    {
        public override void Process()
        {
            World.Process(this);
        }  
    }
}
