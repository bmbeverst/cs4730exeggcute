
namespace Exeggcute.src.contexts
{
    class ToPlayerMenuEvent : ContextEvent
    {
        public Difficulty Setting { get; protected set; }

        public ToPlayerMenuEvent(Difficulty setting)
        {
            this.Setting = setting;
        }

        public override void Process()
        {
            World.Process(this);
        }  
    }
}
