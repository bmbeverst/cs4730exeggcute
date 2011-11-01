using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.sound;

namespace Exeggcute.src.loading.specs
{
#pragma warning disable 0649
    class PlayerInfo : Loadable
    {
        public BodyInfo body;
        public BehaviorScript deathScript;
        public GibBatch gibBatch;
        public RepeatedSound deathSound;
        public Arsenal special;
        public int? lives;
        public int? bombs;
        public float? moveSpeed;
        public float? focusSpeed;
        public float? hitRadius;
        public float? lightLevel;

        public PlayerInfo(string filename, List<string[]> tokenList)
            : base(filename)
        {
            loadFromTokens(tokenList, true);
        }
    }

}
