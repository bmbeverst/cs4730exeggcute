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
    class ArsenalEntry : Loadable
    {
        public BodyInfo Body;
        public int Damage;
        public BehaviorScript Behavior;
        public TrajectoryScript Trajectory;
        public SpawnScript Spawn;

        public ArsenalEntry(List<string[]> tokenList)
        {
            loadFromTokens(tokenList);
        }
    }
}
