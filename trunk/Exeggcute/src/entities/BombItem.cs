using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    class BombItem : Collectable
    {
        public static Sprite HUDSprite;

        public BombItem(ModelName name)
            : base(name)
        {
            if (HUDSprite == null)
            {
                HUDSprite = SpriteBank.Get(SpriteName.bomb);
            }
        }
    }
}
