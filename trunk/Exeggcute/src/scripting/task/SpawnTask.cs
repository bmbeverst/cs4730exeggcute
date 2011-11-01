
namespace Exeggcute.src.scripting.task
{
    class SpawnTask : Task
    {
        public int ID { get; protected set; }
        public Float3 Position { get; protected set; }
        public FloatValue Angle { get; protected set; }

        public SpawnTask(int id, Float3 pos, FloatValue angle)
        {
            this.ID = id;
            this.Position = pos;
            this.Angle = angle;
        }

        public override void Process(Level level)
        {
            level.Process(this);
        }


    }
}
