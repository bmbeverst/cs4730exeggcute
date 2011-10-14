using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.contexts.scripting
{
    class TaskListLoader : ScriptParser<Task, int>
    {
        public static readonly string EXT = "level";
        public static readonly string ROOT = "data/levels";
        public override string getFilepath(string name)
        {
            return string.Format("{0}/{1}.{2}", ROOT, name, EXT);
        }

        protected override List<Task> parseElement(string[] tokens)
        {
            TaskType type = Util.ParseEnum<TaskType>(tokens[0]);
            if (type == TaskType.Msg)
            {
                return new List<Task> {
                    new MessageTask()
                };
            }
            else
            {
                throw new Exception();
            }
        }
    }
}
