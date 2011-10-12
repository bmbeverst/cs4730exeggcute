using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Exeggcute.src.assets;

namespace Exeggcute.src.entities
{
    class LifeItem : Collectable
    {
        public static Sprite HUDSprite;

        public LifeItem(ModelName name)
            : base(name)
        {
            if (HUDSprite == null)
            {
                HUDSprite = SpriteBank.Get(SpriteName.life);
            }
        }
    }
}
