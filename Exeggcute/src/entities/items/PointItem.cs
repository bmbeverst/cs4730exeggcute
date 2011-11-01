using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.entities.items
{
    class PointItem : Item
    {
        public PointItem(Model model, Texture2D texture, float scale, float radius, Vector3 rotation, BehaviorScript behavior)
            : base(model, texture, scale, radius, rotation, behavior)
        {

        }

        public override Item Clone()
        {
            return new PointItem(Surface, Texture, Scale, Radius, DegRotation, (BehaviorScript)script);
        }

        public override void Collect(Player player)
        {
            IsTrash = true;
            player.Collect(this);
        }
    }
}
