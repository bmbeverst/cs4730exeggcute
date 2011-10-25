using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities.items;

namespace Exeggcute.src
{
    class ItemBatch
    {
        List<Item> myItems;
        Float3 dispersion;

        public ItemBatch(List<Item> items, Float3 dispersion)
        {
            this.myItems = items;
            this.dispersion = dispersion;
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
                copy.Add(item.Copy());
            }
            return new ItemBatch(copy, dispersion);
        }
    }
}
