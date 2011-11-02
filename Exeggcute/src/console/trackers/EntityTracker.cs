using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.entities;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;

namespace Exeggcute.src.console.trackers
{
    class EntityTracker : Tracker
    {
        protected SpriteFont font;
        public EntityTracker(Entity3D entity, string format, int[] indices, int frequency)
            : base(entity, format, indices, frequency)
        {
            this.font = Assets.Font["consolas8"];
        }


        public override void Draw2D(SpriteBatch batch, Vector2 pos)
        {
            batch.DrawString(font, output, pos, Color.White);
        }
    }
}
