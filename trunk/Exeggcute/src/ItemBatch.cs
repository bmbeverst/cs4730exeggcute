using System.Collections.Generic;
using Exeggcute.src.assets;
using Exeggcute.src.entities.items;
using Exeggcute.src.loading;
using Microsoft.Xna.Framework;

namespace Exeggcute.src
{
    class ItemBatch
    {
        private List<Item> myItems;
        private Float3 dispersion;

        public ItemBatch(List<Item> items, Float3 dispersion)
        {
            this.myItems = items;
            this.dispersion = dispersion;
        }

        public ItemBatch()
        {

        }

        public static ItemBatch LoadFromFile(string filename)
        {
            return Loaders.ItemBatch.Load(filename);
        }

        public void Release(HashList<Item> itemList, Vector3 deathPos)
        {
            dispersion = new Float3(new FloatRange(0, 5), new FloatRange(0, 5), new FloatValue(0));
            foreach (Item item in myItems)
            {
                Vector3 pos = dispersion.Vector3;
                item.SetPosition(deathPos + pos);
                itemList.Add(item);
            }
        }

        public ItemBatch Clone()
        {
            List<Item> copy = new List<Item>();
            foreach (Item item in myItems)
            {
                copy.Add(item.Clone());
            }
            return new ItemBatch(copy, dispersion);
        }

        public static ItemBatch Parse(string s)
        {
            return Assets.ItemBatch[s];
        }
    }
}
