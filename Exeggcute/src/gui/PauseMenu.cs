using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.input;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.contexts;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;

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
            batch.Begin();
            base.Draw2D(batch);
            batch.End();
        }

        public override void Back()
        {
            cursor = 0;
            World.Unpause();
        }

    }
}
