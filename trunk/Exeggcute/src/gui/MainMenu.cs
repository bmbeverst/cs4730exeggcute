using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.gui
{
    class MainMenu : Menu
    {
        Doodad buttonBoxOutline;
        public MainMenu(List<Button> buttons, WangMesh terrain, Rectangle bounds)
            : base(buttons, bounds, terrain, false)
        {
            this.buttonBoxOutline = new Doodad(buttonBounds, Color.Black, false);
        }

        public override void Update(ControlManager controls)
        {
            base.Update(controls);
        }

        public override void Draw2D(SpriteBatch batch)
        {
            buttonBoxOutline.Draw(batch);
            base.Draw2D(batch);
            DrawLogo(batch);
;        }

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            base.Draw3D(graphics, camera);
        }

        public override void Move(Direction dir)
        {
            if (dir == Direction.Up)
            {
                cursor -= 1;
            }
            else if (dir == Direction.Down)
            {
                cursor += 1;
            }
        }

        public override void Back()
        {
            cursor = buttons.Count - 1;
        }
    }
}
