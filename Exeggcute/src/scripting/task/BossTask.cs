
namespace Exeggcute.src.scripting.task
{
    class BossTask : Task
    {
        public override void Process(Level level)
        {
            level.Process(this);
        }
    }
}
