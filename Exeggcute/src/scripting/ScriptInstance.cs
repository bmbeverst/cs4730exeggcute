using Exeggcute.src.scripting.action;

namespace Exeggcute.src.scripting
{
    abstract class ScriptInstance
    {
        public string Name
        {
            get { return scriptBase.Name; }
        }
        public readonly int Count;

        protected ScriptBase scriptBase;
        protected int actionPtr = 0;

        public ScriptInstance(ScriptBase b)
        {
            this.scriptBase = b;
            this.Count = b.Count;
        }

        public void Next()
        {
            actionPtr += 1;
        }

        public ActionBase GetCurrent()
        {
            return scriptBase[actionPtr];
        }

        public void Jump(int i)
        {
            actionPtr = scriptBase.LineNumberToAction(i);
        }

        public ActionBase this[int i]
        {
            get { return scriptBase[i]; }
        }

    }
}
