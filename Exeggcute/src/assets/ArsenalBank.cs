using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.arsenal;
using System.IO;
using Exeggcute.src.entities;

namespace Exeggcute.src.assets
{
    class ArsenalBank
    {
        protected static CustomBank<Arsenal> bank =
            new CustomBank<Arsenal>("data/arsenals");

        protected static ArsenalLoader loader = new ArsenalLoader();
        public static Arsenal Get(string name, HashList<Shot> shotListHandle)
        {
            return bank[name].Copy(shotListHandle);
        }

        public static void LoadAll()
        {
            foreach (string file in bank.AllFiles)
            {
                bank.Put(loader.Make(file), file);
            }
        }
    }
}
