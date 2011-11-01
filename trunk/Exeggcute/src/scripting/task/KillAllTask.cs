
namespace Exeggcute.src.scripting.task
{
    class KillAllTask : Task
    {
        public override void Process(Level level)
        {
            level.Process(this);
        }
    }
}
