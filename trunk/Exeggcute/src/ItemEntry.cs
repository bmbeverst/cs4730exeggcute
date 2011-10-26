using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting;
using Exeggcute.src.entities.items;
using Exeggcute.src.loading;

namespace Exeggcute.src
{
#pragma warning disable 0649
    class ItemEntry : Loadable
    {
        public BodyInfo Body;
        public BehaviorScript Behavior;
        public ItemType? Type;

        public ItemEntry(string filename)
        {
            loadFromFile(filename);
        }

        public Item MakeItem()
        {
            return new Item(Body.Model, 
                            Body.Texture, 
                            Body.Scale.Value, 
                            Behavior);
        }

    }
}
