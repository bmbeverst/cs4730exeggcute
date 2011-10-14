using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.entities;

namespace Exeggcute.src
{

    /// <summary>
    /// A roster is a list of enemies/entities available to the level to spawn.
    /// </summary>
    class Roster
    {
        protected static Dictionary<Pair<ModelName, ScriptName>, Enemy> cache =
            new Dictionary<Pair<ModelName, ScriptName>, Enemy>();
        protected List<Enemy> enemies;

        public int Count
        {
            get { return enemies.Count; }
        }

        public Roster(List<Pair<ModelName, ScriptName>> pairs)
        {

        }


        public Enemy Clone(EntityParams parms)
        {
            throw new NotImplementedException();
        }
    }
}
