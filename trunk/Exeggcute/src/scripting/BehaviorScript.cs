using Exeggcute.src.assets;

namespace Exeggcute.src.scripting
{
    class BehaviorScript : ScriptInstance
    {
        public BehaviorScript(ScriptBase script)
            : base(script)
        {

        }

        public static BehaviorScript Parse(string name)
        {
            return Assets.GetBehavior(name);
        }
    }
}
