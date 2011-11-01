using Exeggcute.src.assets;

namespace Exeggcute.src.scripting
{
    class TrajectoryScript : ScriptInstance
    {
        public TrajectoryScript(ScriptBase script)
            : base(script)
        {

        }

        public static TrajectoryScript Parse(string name)
        {
            return Assets.GetTrajectory(name);
        }
    }
}
