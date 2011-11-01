using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.gui
{
    class PauseMenu : Menu
    {
        public PauseMenu(List<Button> buttons, Rectangle bounds)
            : base(buttons, bounds, false)
        {
            
        }

        public override void Update(ControlManager controls)
        {
            if (controls[Ctrl.Start].DoEatPress())
            {
                World.Unpause();
            }
            base.Update(controls);
        }

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            base.Draw3D(graphics, camera);
        }

        public override void Draw2D(SpriteBatch batch)
        {
            base.Draw2D(batch);
        }

        public override void Back()
        {
            cursor = 0;
            World.Unpause();
        }

    }
}
