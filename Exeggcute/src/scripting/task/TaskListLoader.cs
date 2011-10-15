﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.task
{
    class TaskListLoader : ScriptParser<Task, int>
    {
        public static readonly string EXT = "level";
        public static readonly string ROOT = "data/levels";
        protected override string getFilepath(string name)
        {
            return string.Format("{0}/{1}.{2}", ROOT, name, EXT);
        }
        public TaskListLoader()
        {
            Delim = ' ';
        }

        protected override List<Task> parseElement(string[] tokens)
        {
            TaskType type = Util.ParseEnum<TaskType>(tokens[0]);
            if (type == TaskType.Msg)
            {
                int id = int.Parse(tokens[1]);
                return new List<Task> {
                    new MessageTask(id)
                };
            }
            else if (type == TaskType.Spawn)
            {
                int id = int.Parse(tokens[1]);
                Vector3 pos = Util.ParseVector3(tokens[2]);
                float angle = float.Parse(tokens[3]) * FastTrig.degreesToRadians;
                EntityArgs args = new EntityArgs(pos, angle);
                return new List<Task> {
                    new SpawnTask(id, args)
                };
            }
            else if (type == TaskType.Wait)
            {
                int duration = int.Parse(tokens[1]);
                return new List<Task> { 
                    new WaitTask(duration)
                };

            }
            else
            {
                throw new Exception();
            }
        }
    }
}