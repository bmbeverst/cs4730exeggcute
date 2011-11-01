﻿using System.Collections.Generic;

namespace Exeggcute.src.scripting.task
{
    class TaskListLoader : ScriptParser<Task>
    {



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
                    new SongFadeTask(frames),
                    new BarrierTask(BarrierType.FadeOut)
                };
            }
            else
            {
                throw new ParseError("Unknown type \"{0}\"", type);
            }
        }
    }
}
