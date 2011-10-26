using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.loading;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Exeggcute.src.entities
{
#pragma warning disable 0649
    class EnemyInfo : Loadable
    {
        public Model Model;
        public Texture2D Texture;
        public float? Scale;
        public SoundEffect ShootSFX;
        public SoundEffect DieSFX;
        public ItemBatch Items;
        public GibBatch Gibs;
        public int? Health;
        public int? Defense;

        public EnemyInfo(List<string[]> tokenList)
        {
            loadFromTokens(tokenList);
        }



    }
}
