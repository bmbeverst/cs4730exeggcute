using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.entities.items
{
    class Item : ScriptedEntity
    {
        protected const float terminalSpeed = -0.3f;
        protected ItemEntry entry;
        public Item(ItemEntry entry)
            : base(entry.Surface, entry.Behavior)
        {
            this.entry = entry;
        }

        public Item Copy()
        {
            return new Item(entry);
        }
        public void Collect(Player player)
        {
            IsTrash = true;
            player.Collect(this);
        }

        public override void Update()
        {
            if (Speed < terminalSpeed)
            {
                Speed = terminalSpeed;
            }
            base.Update();
        }


    }
}
