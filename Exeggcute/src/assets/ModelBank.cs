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
        static private Bank<ModelName, Model> bank = 
            new Bank<ModelName, Model>("models");
        public static List<ModelName> AllNames = bank.AllNames;
        public static Model Get(ModelName name)
        {
            return bank[name];
        }

        public static void LoadAll(ContentManager content)
        {
            bank.LoadAll(content);
        }

        public static void Load(ContentManager content, ModelName name)
        {
            bank.Load(content, name);
        }
    }
}
