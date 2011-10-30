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
        static private Bank<Texture2D> bank =
            new Bank<Texture2D>("ExeggcuteContent/sprites", "xnb");

        public static Texture2D Get(string name)
        {
            return bank[name];
        }

        public static List<string> GetLoaded()
        {
            return bank.GetAllLoaded();
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
