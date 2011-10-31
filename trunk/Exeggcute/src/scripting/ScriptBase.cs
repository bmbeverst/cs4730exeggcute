using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.action;
using Exeggcute.src.loading;

namespace Exeggcute.src
{
    class ScriptBase
    {
        public string Name { get; protected set; }

        protected List<ActionBase> actionList;
        protected int[] lineTable;
        public readonly int Count;

        protected static ScriptLoader loader = new ScriptLoader();
        public ScriptBase(string name, List<List<ActionBase>> lists)
        {
            this.Name = name;
            this.actionList = new List<ActionBase>();
            Count = lists.Aggregate(0, (sum, next) => sum + next.Count);
            this.lineTable = new int[lists.Count];
            int innerCtr = 0;
            int outerCtr = 0;

            foreach (List<ActionBase> list in lists)
            {
                lineTable[outerCtr] = innerCtr;
                foreach (ActionBase action in list)
                {
                    actionList.Add(action);
                    innerCtr += 1;
                }
                outerCtr += 1;
            }
        }

        public static ScriptBase LoadFromFile(string filename)
        {
            return new ScriptBase(filename, loader.RawFromFile(filename));
        }

        public ActionBase this[int i]
        {
            get { return actionList[i]; }
        }

        public int LineNumberToAction(int i)
        {
            return lineTable[i];
        }
    }
}
