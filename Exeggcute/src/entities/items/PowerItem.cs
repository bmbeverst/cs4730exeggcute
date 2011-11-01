using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.entities.items
{
    class PowerItem : Item
    {

        public PowerItem(Model model, Texture2D texture, float scale, float radius, Vector3 rotation, BehaviorScript behavior)
            : base(model, texture, scale, radius, rotation, behavior)
        {
            
        }
        public override Item Clone()
        {
            return new PowerItem(Surface, Texture, Scale, Radius, DegRotation, (BehaviorScript)script);
        }

        public override void Collect(Player player)
        {
            IsTrash = true;
            player.Collect(this);
        }
    }
}
