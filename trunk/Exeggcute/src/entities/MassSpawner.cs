using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.scripting;

namespace Exeggcute.src.entities
{
    class SpawnerEntry
    {
        public SpawnScript Spawn { get; protected set; }
        public Arsenal Arsenal { get; protected set; }
        public Vector3 RelPosition { get; protected set; }
        public float Angle { get; protected set; }
        public SpawnerEntry(SpawnScript spawn, Arsenal arsenal, Vector3 pos, float angle)
        {
            this.Spawn = spawn;
            this.Arsenal = arsenal;
            this.RelPosition = pos;
            this.Angle = angle;
        }
    }

    class MassSpawner
    {
        /*public List<Spawner> spawners = new List<Spawner>();

        public bool IsDone
        {
            get { return doneTimer.IsDone; }
        }

        protected Timer doneTimer;



        public MassSpawner(List<SpawnerEntry> entries, int duration, HashList<Shot> shotList)
        {
            entries = new List<SpawnerEntry> {
                new SpawnerEntry(SpawnerName.player0, ArsenalName.test, new Vector3(0, 5, 0), 0),
                new SpawnerEntry(SpawnerName.player0, ArsenalName.test, new Vector3(-10, 5, 0), 0),
                new SpawnerEntry(SpawnerName.player0, ArsenalName.test, new Vector3(10, 5, 0), 0),

            };
            foreach (SpawnerEntry entry in entries)
            {
                //spawners.Add(new Spawner(entry.ScriptName, entry.ArsenalName, entry.RelPosition, entry.Angle, shotList));
            }

            spawners[0].SetPosition(new Vector3(0, 0, 0));
            doneTimer = new Timer(duration);
        }

        public void Update(CommandEntity parent)
        {
            if (parent != null)
            {
                foreach (Spawner spawner in spawners)
                {
                    //spawner.Follow(parent, true, false);
                }
            }
            foreach (Spawner spawner in spawners)
            {
                spawner.Update();
            }

            doneTimer.Increment();
        }

        public void Reset()
        {
            foreach (Spawner spawner in spawners)
            {
                spawner.Reset();
            }
            doneTimer.Reset();
        }*/
    }
}
