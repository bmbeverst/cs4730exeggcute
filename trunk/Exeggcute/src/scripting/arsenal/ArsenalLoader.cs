using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.loading;

namespace Exeggcute.src.scripting.arsenal
{
    class ArsenalLoader : Loadable
    {
        protected List<ArsenalEntry> entries;

    }
    /*class ArsenalLoader : EntryListParser<ArsenalEntry>
    {
        public Arsenal Make(string filepath)
        {
            return new Arsenal(Parse(filepath), null);
        }
        protected override ArsenalEntry parseEntry(Stack<string> tokens)
        {
            string modelName = tokens.Pop();
            string textureName = tokens.Pop();
            string spawnerBehaviorName = tokens.Pop();
            string spawnName = tokens.Pop();
            string shotTrajectoryName = tokens.Pop();
            Model model = ModelBank.Get(modelName);
            Texture2D texture = TextureBank.Get(textureName);
            BehaviorScript behavior = ScriptBank.GetBehavior(spawnerBehaviorName);
            SpawnScript spawn = ScriptBank.GetSpawn(spawnName);
            TrajectoryScript trajectory = ScriptBank.GetTrajectory(shotTrajectoryName);

            return new ArsenalEntry(model, texture, behavior, spawn, trajectory);
        }
    }*/
}
