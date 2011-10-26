using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.scripting;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Audio;

namespace Exeggcute.src.loading.specs
{
#pragma warning disable 0649
    class PlayerInfo : Loadable
    {
        public BodyInfo body;
        public BehaviorScript deathScript;
        public GibBatch gibBatch;
        public SoundEffect shootSFX;
        public SoundEffect dieSFX;
        public int? lives;
        public int? bombs;
        public float? moveSpeed;
        public float? focusSpeed;
        public float? hitRadius;
        public float? lightLevel;

        public PlayerInfo(List<string[]> tokenList)
        {
            loadFromTokens(tokenList);
        }
    }

}
