using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.task
{
    class TaskListLoader : ScriptParser<Task>
    {
        public override List<Task> FromFile(string data)
        {
            List<string> lines = Util.StripComments(data, true);
            return base.ParseLines(lines);
        }
        protected override List<Task> parseElement(Stack<string> tokens)
        {
            TaskType type = Util.ParseEnum<TaskType>(tokens.Pop());
            if (type == TaskType.Spawn)
            {
                //spawn id pos angle
                int id = int.Parse(tokens.Pop());
                Float3 pos = Util.ParseFloat3(tokens.Pop());
                FloatValue angle = Util.ParseFloatValue(tokens.Pop()).FromDegrees();
                return new List<Task> {
                    new SpawnTask(id, pos, angle)
                };
            }
            else if (type == TaskType.Wait)
            {
                int duration = int.Parse(tokens.Pop());
                return new List<Task> { 
                    new WaitTask(duration)
                };

            }
            else if (type == TaskType.KillAll)
            {
                return new List<Task> {
                    new KillAllTask()
                };
            }
            else if (type == TaskType.Boss)
            {
                return new List<Task> {
                    new BossTask()
                };
            }
            else if (type == TaskType.SongFade)
            {
                int frames = int.Parse(tokens.Pop());
                return new List<Task> {
                    new WaitTask(frames),
                    new SongFadeTask(frames)
                };
            }
            else
            {
                throw new ParseError("Unknown type \"{0}\"", type);
            }
        }
    }
}
