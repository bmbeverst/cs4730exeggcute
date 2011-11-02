
namespace Exeggcute.src.contexts
{
    class ToMainMenuEvent : ContextEvent
    {
        public override void Process()
        {
            Worlds.World.Process(this);
        }  
    }
}
