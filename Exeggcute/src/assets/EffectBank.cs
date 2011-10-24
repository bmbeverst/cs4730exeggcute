using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Exeggcute.src.assets
{
    static class EffectBank
    {
        private static Bank<Effect> bank = 
            new Bank<Effect>("ExeggcuteContent/shaders", "xnb");

        public static Effect Get(string name)
        {
            return bank[name];//.Clone();
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
