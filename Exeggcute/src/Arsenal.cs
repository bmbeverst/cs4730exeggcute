using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src;
using Exeggcute.src.assets;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;

namespace Exeggcute.src
{
    class Arsenal
    {
        /// <summary>
        /// Used to hold cached shots since many will exist in more than one 
        /// arsenal.
        /// </summary>
        public static Dictionary<Pair<ModelName, ScriptName>, Shot> AllShots = 
            new Dictionary<Pair<ModelName, ScriptName>, Shot>();
        protected  List<Shot> shots = new List<Shot>();
        public int Count
        {
            get { return shots.Count; }
        }
        public Arsenal(List<Pair<ModelName, ScriptName>> pairs)
        {
            foreach (var pair in pairs)
            {
                if (AllShots.ContainsKey(pair))
                {
                    shots.Add(AllShots[pair]);
                }
                ScriptName scriptName = pair.Second;
                ModelName modelName = pair.First;
                Shot shot = new Shot(modelName, scriptName);
                AllShots[pair] = shot;
                shots.Add(shot);
            }
        }

        public Shot Clone(int id, Vector3 pos, float angle)
        {
            return shots[id].Clone(pos, angle);
        }
    }
}
