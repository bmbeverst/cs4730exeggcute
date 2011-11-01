using System.Collections.Generic;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework;
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

        public override void Draw2D(SpriteBatch batch)
        {
            base.Draw2D(batch);
            heading.Draw(batch, headingPos);
        }

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            base.Draw3D(graphics, camera);
        }

        public override void Back()
        {
            World.Back();
        }
    }
}
