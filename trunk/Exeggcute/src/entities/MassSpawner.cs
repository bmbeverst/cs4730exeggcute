using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    class MassSpawner
    {
        public List<Spawner> spawners = new List<Spawner>();

        public MassSpawner(List<KeyValuePair<ScriptName, ArsenalName>> scripts, HashList<Shot> shotList)
        {
            foreach (var pair in scripts)
            {
                Spawner spawner = new Spawner(pair.Key, pair.Value, shotList);
            }
        }

        public void Update()
        {
            foreach (Spawner spawner in spawners)
            {
                spawner.Update();
            }
        }

        public void Reset()
        {
            foreach (Spawner spawner in spawners)
            {
                spawner.Reset();
            }
        }
    }
}
