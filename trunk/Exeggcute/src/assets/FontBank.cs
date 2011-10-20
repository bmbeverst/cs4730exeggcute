using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Exeggcute.src.assets
{
    static class FontBank
    {
        private static Bank<FontName, SpriteFont> bank = 
            new Bank<FontName, SpriteFont>("fonts");
        public static List<FontName> AllNames = bank.AllNames;
        public static SpriteFont Get(FontName name)
        {
            return bank[name];
        }

        public static void LoadAll(ContentManager content)
        {
            bank.LoadAll(content);
        }

        public static void Load(ContentManager content, FontName name)
        {
            bank.Load(content, name);
        }
    }
}
