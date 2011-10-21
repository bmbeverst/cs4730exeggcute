using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.assets
{
    static class ModelBank
    {
        static private Bank<Model> bank =
            new Bank<Model>("ExeggcuteContent/models", "xnb");

        public static Model Get(string name)
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
