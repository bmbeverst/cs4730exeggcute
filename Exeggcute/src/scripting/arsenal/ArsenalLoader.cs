using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.scripting.arsenal
{
    class ArsenalLoader : ListParser<ArsenalEntry>
    {
        public ArsenalLoader()
        {
            Delim = ' ';
        }

        //fixme, doesnt get path, just appends the file extension
        protected override string getFilepath(string name)
        {
            return string.Format("{0}.arsenal", name);
        }

        protected override ArsenalEntry parseEntry(Stack<string> tokens)
        {
            string modelName = tokens.Pop();
            string spawnerBehaviorName = tokens.Pop();
            string spawnName = tokens.Pop();
            string shotTrajectoryName = tokens.Pop();
            Model model = ModelBank.Get(modelName);
            BehaviorScript behavior = ScriptBank.GetBehavior(spawnerBehaviorName);
            SpawnScript spawn = ScriptBank.GetSpawn(spawnName);
            TrajectoryScript trajectory = ScriptBank.GetTrajectory(shotTrajectoryName);

            return new ArsenalEntry(model, behavior, spawn, trajectory);
        }
    }
}
