using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.entities;
using Exeggcute.src.loading;

namespace Exeggcute.src.scripting.roster
{
    class Roster
    {
        protected List<Enemy> enemies = new List<Enemy>();
        public Roster(string name)
        {
            string filename = string.Format("data/rosters/{0}.roster", name);
            List<string> lines = Util.ReadAndStrip(filename, true);
            foreach (string line in lines)
            {
                Enemy enemy = EnemyLoader.Load(line);
                enemies.Add(enemy);
            }
        }
        public Enemy Clone(int id, Float3 pos, FloatValue angle)
        {
            return enemies[id].Clone(pos, angle);
        }

        public static Roster Parse(string s)
        {
            return new Roster(s);
        }
    }
    /// <summary>
    /// A roster is a list of enemies/entities available to the level to spawn.
    /// </summary>
    /*class Roster
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
                Enemy enemy = new Enemy(entry, Enemy.GetDeathScript(), entry.HeldItems, World.EnemyShots, World.GibList, World.ItemList);
                cache[entry] = enemy;
                enemies.Add(enemy);
            }
        }


        public Enemy Clone(int id, Float3 pos, FloatValue angle)
        {
            return enemies[id].Clone(pos, angle);
        }
    }*/
}
