using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.action;
using System.Collections;

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
