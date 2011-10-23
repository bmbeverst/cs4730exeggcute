using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.action;
using System.IO;

namespace Exeggcute.src.assets
{
    class ScriptBank
    {

        private static CustomBank<BehaviorScript> behaviorBank =
            new CustomBank<BehaviorScript>("data/scripts/behaviors");

        private static CustomBank<SpawnScript> spawnBank =
            new CustomBank<SpawnScript>("data/scripts/spawns");

        private static CustomBank<TrajectoryScript> trajectoryBank =
            new CustomBank<TrajectoryScript>("data/scripts/trajectories");

        public static ScriptLoader loader = new ScriptLoader();

        public static BehaviorScript GetBehavior(string behavior)
        {
            return behaviorBank[behavior];
        }

        public static SpawnScript GetSpawn(string spawn)
        {
            return spawnBank[spawn];
        }

        public static TrajectoryScript GetTrajectory(string trajectory)
        {
            return trajectoryBank[trajectory];
        }

        public static void LoadAll()
        {
            foreach (string file in behaviorBank.AllFiles)
            {
                behaviorBank.Put(loader.MakeBehavior(file), file);
            }

            foreach (string file in spawnBank.AllFiles)
            {
                spawnBank.Put(loader.MakeSpawn(file), file);
            }
            
            foreach (string file in trajectoryBank.AllFiles)
            {
                trajectoryBank.Put(loader.MakeTrajectory(file), file);
            }
            
        }
    }
}
