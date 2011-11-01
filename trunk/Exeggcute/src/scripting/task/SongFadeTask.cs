
namespace Exeggcute.src.scripting.task
{
    class SongFadeTask : Task
    {
        public int NumFrames { get; protected set; }

        public SongFadeTask(int frames)
        {
            this.NumFrames = frames;
        }

        public override void Process(Level level)
        {
            level.Process(this);
        }
    }
}
