using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Exeggcute.src.assets;
using Exeggcute.src.entities.items;

namespace Exeggcute.src.loading
{
    class ItemBatchLoader
    {
        public ItemBatch Make(string filepath)
        {
            Float3 dispersion;
            List<Item> items = Load(filepath, out dispersion);
            return new ItemBatch(items, dispersion);
        }

        public List<Item> Load(string filepath, out Float3 dispersion)
        {
            List<Item> items = new List<Item>();

            List<string> lines = Util.ReadAndStrip(filepath, true);

            dispersion = Util.ParseFloat3(lines[0]);
            for (int i = 1; i < lines.Count; i += 1)
            {
                string cleaned = Util.FlattenSpace(lines[i]);
                string[] tokens = cleaned.Split(' ');
                string itemname = tokens[0];
                int amount = int.Parse(tokens[1]);
                //REALLY FIXME!!!!!!!!!!
                Item nextItem = ItemBank.Get(itemname);
                for (int k = 0; k < amount; k += 1)
                {
                    items.Add(nextItem);
                }
            }
            return items;
        }
    }
}
