using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.graphics;

namespace Exeggcute.src.assets
{
    class SpriteBank
    {
        private static Bank<SpriteName, Sprite> bank = new Bank<SpriteName, Sprite>();
        public static List<SpriteName> AllNames = bank.AllNames;
        public static Sprite Get(SpriteName name)
        {
            return bank[name];
        }

        public static void LoadAll(ContentManager content)
        {
            AllNames.ForEach(name => Load(content, name));
        }

        public static void Load(ContentManager content, SpriteName name)
        {
            bank.Put(SpriteLoader.Load(name), name);
        }
    }
}
