using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting;
using Exeggcute.src.entities.items;

namespace Exeggcute.src
{

    class ItemEntry
    {
        public Model Surface { get; protected set; }
        public BehaviorScript Behavior { get; protected set; }
        public ItemType Type { get; protected set; }

        public ItemEntry(Model model, BehaviorScript behavior, ItemType type)
        {
            this.Surface = model;
            this.Behavior = behavior;
            this.Type = type;
        }

    }
}
