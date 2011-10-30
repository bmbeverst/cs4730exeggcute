using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Exeggcute.src.entities.items;
using Exeggcute.src.loading;

namespace Exeggcute.src.assets
{
    class ItemBatchBank
    {
        protected static CustomBank<ItemBatch> bank =
            new CustomBank<ItemBatch>("data/itembatches");
        protected static ItemBatchLoader loader = new ItemBatchLoader();
        public static ItemBatch Get(string name)
        {
            return bank[name];
        }

        public static List<string> GetLoaded()
        {
            return bank.GetAllLoaded();
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
