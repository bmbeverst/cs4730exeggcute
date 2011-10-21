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
        private static Bank<SpriteFont> bank =
            new Bank<SpriteFont>("ExeggcuteContent/fonts", "xnb");


        public static SpriteFont Get(string name)
        {
            return bank[name];
        }

        public static void LoadAll(ContentManager content)
        {
            bank.LoadAll(content);
        }

        public static void Load(ContentManager content, string name)
        {
            bank.Load(content, name);
        }
    }
}
