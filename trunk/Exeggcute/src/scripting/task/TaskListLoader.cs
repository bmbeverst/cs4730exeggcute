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
            if (type == TaskType.Msg)
            {
                int id = int.Parse(tokens.Pop());
                return new List<Task> {
                    new MessageTask(id)
                };
            }
            else if (type == TaskType.Spawn)
            {
                int id = int.Parse(tokens.Pop());
                Float3 pos = Util.ParseFloat3(tokens.Pop());
                FloatValue angle = Util.ParseFloatValue(tokens.Pop()).FromDegrees();
                EntityArgs args = new EntityArgs(pos, angle);
                return new List<Task> {
                    new SpawnTask(id, args)
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
                Float3 spawnPos = Util.ParseFloat3(tokens.Pop());
                return new List<Task> {
                    new BossTask(spawnPos)
                };
            }
            else
            {
                throw new ParseError("Unknown type \"{0}\"", type);
            }
        }
    }
}
