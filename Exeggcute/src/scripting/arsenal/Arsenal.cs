using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src;
using Exeggcute.src.assets;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using Exeggcute.src.scripting;

namespace Exeggcute.src.scripting.arsenal
{
    /// <summary>
    /// An Arsenal is a list of shots which are available to a particular
    /// entity to spawn.
    /// </summary>
    class Arsenal
    {
        /// <summary>
        /// Used to hold cached shots since many will exist in more than one 
        /// arsenal.
        /// </summary>
        protected static Dictionary<ArsenalEntry, Shot> cache =
            new Dictionary<ArsenalEntry, Shot>();
        protected List<Shot> shots = new List<Shot>();

        public int Count
        {
            get { return shots.Count; }
        }

        public Arsenal(List<ArsenalEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (cache.ContainsKey(entry))
                {
                    shots.Add(cache[entry]);
                }
                ScriptName scriptName = entry.ScriptName;
                ModelName modelName = entry.ModelName;
                Shot shot = new Shot(modelName, scriptName);
                cache[entry] = shot;
                shots.Add(shot);
            }
        }

        public Shot Clone(int id, Vector3 pos, float angle)
        {
            return shots[id].Clone(pos, angle);
        }
    }
}
