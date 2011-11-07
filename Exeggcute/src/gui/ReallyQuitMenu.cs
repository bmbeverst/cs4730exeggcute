using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.contexts;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.gui
{
    enum QuitType
    {
        MainMenu, 
        ExitGame
    }

    class ReallyQuitMenu : Menu
    {
        protected SpriteText question;
        protected Vector2 questionPos;

        public ReallyQuitMenu(List<Button> buttons, WangMesh terrain, Rectangle bounds)
            : base(buttons, bounds, terrain, false)
        {
            this.question = new SpriteText(font, "Really exit?", Color.White);
            this.questionPos = new Vector2(buttonBounds.Left - 12, buttonBounds.Top - buttonHeight - 20);
        }

        public void Initialize(QuitType type)
        {
            ContextEvent boundEvent;
            if (type == QuitType.ExitGame)
            {
                boundEvent = new ExitGameEvent();
            }
            else
            {
                boundEvent = new ToMainMenuEvent();
            }
            buttons[0].AttachOnActivate(boundEvent);
        }

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {
            base.Draw3D(graphics, camera);
        }

        public override void Draw2D(SpriteBatch batch)
        {
            question.Draw(batch, questionPos);
 	        base.Draw2D(batch);
        }

        public override void Unload()
        {
            base.Unload();
            cursor = 0;
        }

        public override void Back()
        {
            cursor = 0;
            Worlds.World.Back();
        }
    }
}
