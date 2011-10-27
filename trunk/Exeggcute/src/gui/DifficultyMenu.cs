using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.gui
{
    class DifficultyMenu : Menu
    {
        public SpriteText heading;
        public Vector2 headingPos;
        public DifficultyMenu(List<Button> buttons, Rectangle bounds)
            : base(buttons, bounds, false)
        {
            this.heading = new SpriteText(font, "Select difficulty", fontColor);
            this.headingPos = new Vector2(bounds.X - 50, bounds.Y - buttonHeight - 12);
            this.cursor = 1;
        }

        public override void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            batch.Begin();
            base.Draw(graphics, batch);
            heading.Draw(batch, headingPos);
            batch.End();
        }

        public override void Back()
        {
            World.Back();
        }
    }
}
