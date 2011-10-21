using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.entities
{
    class BombItem : Collectable
    {
        public static Sprite HUDSprite;

        public BombItem(Model model, BehaviorScript behavior)
            : base(model, behavior)
        {
            if (HUDSprite == null)
            {
                HUDSprite = SpriteBank.Get("bomb");
            }
        }
    }
}
