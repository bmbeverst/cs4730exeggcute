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
        private static Bank<EffectName, Effect> bank = new Bank<EffectName, Effect>();
        public static List<EffectName> AllNames = bank.AllNames;
        public static Effect Get(EffectName name)
        {
            return bank[name].Clone();
        }

        public static void LoadAll(ContentManager content)
        {
            bank.LoadAll(content);
        }

        public static void Load(ContentManager content, EffectName name)
        {
            bank.Load(content, name);
        }
    }
}
