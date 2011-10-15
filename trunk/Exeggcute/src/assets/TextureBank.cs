using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Exeggcute.src.assets
{

    static class TextureBank
    {
        static private Bank<TextureName, Texture2D> bank = new Bank<TextureName, Texture2D>();
        public static List<TextureName> AllNames = bank.AllNames;
        public static Texture2D Get(TextureName name)
        {
            return bank[name];
        }

        public static void LoadAll(ContentManager content)
        {
            bank.LoadAll(content);
        }

        public static void Load(ContentManager content, TextureName name)
        {
            bank.Load(content, name);
        }
    }
}
