using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exeggcute.src.entities;

namespace Exeggcute.src.console.trackers
{
    class HudTracker : Tracker
    {
        protected SpriteFont font;
        public HudTracker(Entity3D entity, string format, int[] indices, int frequency)
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
