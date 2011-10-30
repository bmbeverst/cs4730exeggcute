using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.roster;
using Microsoft.Xna.Framework.Content;
using System.IO;
using Exeggcute.src.entities;
using Exeggcute.src.entities.items;
using Exeggcute.src.loading;

namespace Exeggcute.src.assets
{
    class ItemBank
    {
        protected static CustomBank<Item> bank =
            new CustomBank<Item>("data/items");

        public static Item Get(string name)
        {
            return bank[name].Copy();
        }

        public static List<string> GetLoaded()
        {
            return bank.GetAllLoaded();
        }

        public static void LoadAll()
        {
            foreach (string file in bank.AllFiles)
            {
                ItemEntry entry = new ItemEntry(file);
                
                bank.Put(entry.MakeItem(), file);
            }
        }
    }
}
