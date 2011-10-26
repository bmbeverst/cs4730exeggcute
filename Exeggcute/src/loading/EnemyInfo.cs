using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework.Audio;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Exeggcute.src.loading
{
#pragma warning disable 0649
    class EnemyInfo : Loadable
    {
        public BodyInfo Body;
        public int? Health;
        public int? Defence;
        public BehaviorScript deathScript;
        public SoundEffect shootSFX;
        public SoundEffect dieSFX;
        public ItemBatch itembatch;
        public GibBatch gibbatch;

        public EnemyInfo(List<string[]> tokenList)
        {
            loadFromTokens(tokenList);
            
        }
    }
}
