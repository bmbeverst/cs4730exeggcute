﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.entities
{
    class SpawnerEntry
    {
        public ScriptName ScriptName { get; protected set; }
        public ArsenalName ArsenalName { get; protected set; }
        public EntityArgs Args { get; protected set; }
        public SpawnerEntry(ScriptName scriptName, ArsenalName arsenalName, Vector3 pos, float angle)
        {
            ScriptName = scriptName;
            ArsenalName = arsenalName;
            Args = new EntityArgs(pos, angle);
        }
    }
    class MassSpawner
    {
        public List<Spawner> spawners = new List<Spawner>();

        public bool IsDone
        {
            get { return doneTimer.IsDone; }
        }

        protected Timer doneTimer;



        public MassSpawner(List<SpawnerEntry> entries, int duration, HashList<Shot> shotList)
        {
            entries = new List<SpawnerEntry> {
                new SpawnerEntry(ScriptName.playerspawner0, ArsenalName.test, new Vector3(0, 5, 0), 0),
                new SpawnerEntry(ScriptName.playerspawner0, ArsenalName.test, new Vector3(-10, 5, 0), 0),
                new SpawnerEntry(ScriptName.playerspawner0, ArsenalName.test, new Vector3(10, 5, 0), 0),

            };
            foreach (SpawnerEntry entry in entries)
            {
                spawners.Add(new Spawner(entry.ScriptName, entry.ArsenalName, entry.Args, shotList));
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
                    spawner.Follow(parent, false);
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
        }
    }
}
