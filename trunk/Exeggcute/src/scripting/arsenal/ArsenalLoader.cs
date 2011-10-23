using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.scripting.arsenal
{
    class ArsenalLoader : EntryListParser<ArsenalEntry>
    {
        public Arsenal Make(string filepath)
        {
            return new Arsenal(Parse(filepath), null);
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
