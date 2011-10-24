using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.graphics;
using Exeggcute.src.assets;
using Exeggcute.src.contexts;

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
        protected SpriteFont font;

        public ReallyQuitMenu(List<Button> buttons, Rectangle bounds)
            : base(buttons, bounds, false)
        {
            this.font = FontBank.Get("consolas");
            this.question = new SpriteText(font, "Really exit?", Color.White);
            this.questionPos = new Vector2(buttonBounds.Left - 12, buttonBounds.Top - buttonHeight - 2);
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

        public override void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            batch.Begin();
            question.Draw(batch, questionPos);
 	        base.Draw(graphics, batch);
            batch.End();
        }

        public override void Unload()
        {
            base.Unload();
            cursor = 0;
        }
    }
}
