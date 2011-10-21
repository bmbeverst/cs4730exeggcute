using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.graphics;
using System.IO;

namespace Exeggcute.src.assets
{
    class SpriteBank
    {
        private static Bank<Sprite> bank =
            new Bank<Sprite>("ExeggcuteContent/sprites", "sprite");

        public static Sprite Get(string name)
        {
            return bank[name];
        }

        public static void LoadAll(ContentManager content)
        {
            foreach (string name in bank.AllFiles)
            {
                Load(content, name);
            }
        }

        public static void Load(ContentManager content, string filepath)
        {
            string cutname = Path.GetFileNameWithoutExtension(filepath);
            bank.Put(SpriteLoader.Load(filepath), cutname);
        }
    }
}
