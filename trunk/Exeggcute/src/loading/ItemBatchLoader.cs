using System.Collections.Generic;
using Exeggcute.src.assets;
using Exeggcute.src.entities.items;

namespace Exeggcute.src.loading
{
    class ItemBatchLoader
    {
        public ItemBatch Load(string filepath)
        {
            List<Item> items = new List<Item>();
            List<string> lines = Util.ReadAndStrip(filepath, true);
            Float3 dispersion = Util.ParseFloat3(lines[0]);
            for (int i = 1; i < lines.Count; i += 1)
            {
                string cleaned = Util.FlattenSpace(lines[i]);
                string[] tokens = cleaned.Split(' ');
                string itemname = tokens[0];
                int amount = int.Parse(tokens[1]);
                Item nextItem = Assets.Item[itemname].MakeItem();
                for (int k = 0; k < amount; k += 1)
                {
                    items.Add(nextItem);
                }
            }
            return new ItemBatch(items, dispersion);
        }

    }
}
