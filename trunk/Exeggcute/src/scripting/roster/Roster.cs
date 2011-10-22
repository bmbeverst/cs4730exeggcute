using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.roster
{

    /// <summary>
    /// A roster is a list of enemies/entities available to the level to spawn.
    /// </summary>
    class Roster
    {
        protected static Dictionary<RosterEntry, Enemy> cache =
            new Dictionary<RosterEntry, Enemy>();

        protected List<Enemy> enemies = new List<Enemy>();

        public int Count
        {
            get { return enemies.Count; }
        }

        public Roster(List<RosterEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (false && cache.ContainsKey(entry))
                {
                    enemies.Add(cache[entry]);
                }
                Enemy enemy = new Enemy(entry, Enemy.GetDeathScript(), World.EnemyShots, World.GibList);
                cache[entry] = enemy;
                enemies.Add(enemy);
            }
        }


        public Enemy Clone(int id, EntityArgs args)
        {
            return enemies[id].Clone(args);
        }
    }
}
