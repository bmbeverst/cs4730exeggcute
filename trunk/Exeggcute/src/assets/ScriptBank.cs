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

        private static CustomBank<ScriptBase> behaviorBank =
            new CustomBank<ScriptBase>("data/scripts/behaviors");

        private static CustomBank<ScriptBase> spawnBank =
            new CustomBank<ScriptBase>("data/scripts/spawns");

        private static CustomBank<ScriptBase> trajectoryBank =
            new CustomBank<ScriptBase>("data/scripts/trajectories");

        public static ScriptLoader loader = new ScriptLoader();

        public static BehaviorScript GetBehavior(string behavior)
        {
            return new BehaviorScript(behaviorBank[behavior]);
        }

        public static SpawnScript GetSpawn(string spawn)
        {
            return new SpawnScript(spawnBank[spawn]);
        }

        public static TrajectoryScript GetTrajectory(string trajectory)
        {
            return new TrajectoryScript(trajectoryBank[trajectory]);
        }

        public static List<string> GetLoadedBehaviors()
        {
            return behaviorBank.GetAllLoaded();
        }

        public static List<string> GetLoadedSpawns()
        {
            return spawnBank.GetAllLoaded();
        }

        public static List<string> GetLoadedTrajectories()
        {
            return trajectoryBank.GetAllLoaded();
        }

        public static void LoadAll()
        {
            foreach (string file in behaviorBank.AllFiles)
            {
                behaviorBank.Put(loader.Make(file), file);
            }

            foreach (string file in spawnBank.AllFiles)
            {
                spawnBank.Put(loader.Make(file), file);
            }
            
            foreach (string file in trajectoryBank.AllFiles)
            {
                trajectoryBank.Put(loader.Make(file), file);
            }
            
        }
    }
}
