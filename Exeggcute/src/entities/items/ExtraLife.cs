using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.entities.items
{
    class ExtraLife : Item
    {
        public ExtraLife(Model model, Texture2D texture, float scale, BehaviorScript behavior)
            : base(model, texture, scale, behavior)
        {

        }
    }
}
