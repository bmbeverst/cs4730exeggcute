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

        private static Bank<BehaviorScript> behaviorBank =
            new Bank<BehaviorScript>("data/scripts/behaviors", "cl");

        private static Bank<SpawnScript> spawnBank =
            new Bank<SpawnScript>("data/scripts/spawns", "spawn");

        private static Bank<TrajectoryScript> trajectoryBank =
            new Bank<TrajectoryScript>("data/scripts/trajectories", "traj");

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
                string name = Path.GetFileNameWithoutExtension(file);
                List<ActionBase> list = loader.LoadBehavior(file);
                BehaviorScript script = new BehaviorScript(name, list);
                behaviorBank.Put(script, name);
            }

            foreach (string file in spawnBank.AllFiles)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                List<ActionBase> list = loader.LoadSpawn(file);
                SpawnScript script = new SpawnScript(name, list);
                spawnBank.Put(script, name);
            }
            
            foreach (string file in trajectoryBank.AllFiles)
            {
                string name = Path.GetFileNameWithoutExtension(file);
                List<ActionBase> list = loader.LoadShot(file);
                TrajectoryScript script = new TrajectoryScript(name, list);
                trajectoryBank.Put(script, name);
            }
            
        }
    }
}
