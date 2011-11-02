using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.entities.items;
using Exeggcute.src.loading;
using Microsoft.Xna.Framework;

namespace Exeggcute.src
{
    class ItemBatch
    {
        public List<Item> myItems;
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
