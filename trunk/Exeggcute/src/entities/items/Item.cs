using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting;

namespace Exeggcute.src.entities.items
{
    class Item : ScriptedEntity
    {
        protected const float terminalSpeed = -0.3f;
        public Item(Model model, Texture2D texture, float scale, BehaviorScript behavior)
            : base(model, texture, scale, behavior)
        {
            
        }

        public Item Copy()
        {
            return new Item(Surface, Texture, Scale, (BehaviorScript)script);
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
