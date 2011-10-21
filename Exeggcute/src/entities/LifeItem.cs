using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.entities
{
    class LifeItem : Collectable
    {
        public static Sprite HUDSprite;

        public LifeItem(Model model)
            : base(model, null)
        {
            if (HUDSprite == null)
            {
                HUDSprite = SpriteBank.Get("life");
            }
        }
    }
}
