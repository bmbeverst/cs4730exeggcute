using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.loading;

namespace Exeggcute.src.scripting.arsenal
{
#pragma warning disable 0649

    class OptionInfo : Loadable
    {
        public BodyInfo Body;
        public int Damage;
        public BehaviorScript Behavior;
        public TrajectoryScript Trajectory;
        public SpawnScript Spawn;

        public OptionInfo(string name)
        {
            string filename = string.Format("data/options/{0}.option", name);
            loadFromFile(filename);
        }

        public OptionInfo(List<string[]> tokenList)
        {
            loadFromTokens(tokenList);
        }

        public static OptionInfo Parse(String name)
        {
            string filename = string.Format("data/options/{0}.option", name);
            return new OptionInfo(filename);
        }
    }
}
