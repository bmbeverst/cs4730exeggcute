using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting;

namespace Exeggcute.src.entities.items
{
    class PowerItem : Item
    {

        public PowerItem(Model model, Texture2D texture, float scale, BehaviorScript behavior)
            : base(model, texture, scale, behavior)
        {
            
        }
    }
}
