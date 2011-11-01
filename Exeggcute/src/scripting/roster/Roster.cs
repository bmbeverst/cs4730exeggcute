using System.Collections.Generic;
using Exeggcute.src.assets;
using Exeggcute.src.entities;

namespace Exeggcute.src.scripting.roster
{
    class Roster
    {
        protected List<Enemy> enemies = new List<Enemy>();
        /*public Roster(string name)
        {
            Util.Warn("this usage of Roster is deprecated");
            string filename = string.Format("data/rosters/{0}.roster", name);
            List<string> lines = Util.ReadAndStrip(filename, true);
            foreach (string line in lines)
            {
                Enemy enemy = EnemyLoader.LoadByName(line);
                enemies.Add(enemy);
            }
        }*/

        public Roster(List<Enemy> enemies)
        {
            this.enemies = enemies;
        }

        public Enemy Clone(int id, Float3 pos, FloatValue angle)
        {
            return enemies[id].Clone(pos, angle);
        }

        public static Roster Parse(string s)
        {
            string[] entries = s.Split(',');
            List<Enemy> result = new List<Enemy>();
            foreach (string name in entries)
            {
                Enemy enemy = Assets.Enemy[name];
                result.Add(enemy);
            }
            return new Roster(result);
        }
    }
}
