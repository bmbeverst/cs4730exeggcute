using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exeggcute.src.assets;
using Exeggcute.src.input;
using Exeggcute.src.contexts;

namespace Exeggcute.src.gui
{
    class MainMenu : Menu
    {
        StaticSprite cursorSprite;
        Rectangle ButtonBox;
        List<Vector2> drawPositions = new List<Vector2>();
        Doodad buttonBoxOutline;
        int vertButtonSpacing; 
        public MainMenu()
            : base(getButtons(), false)
        {
            cursorSprite = new StaticSprite(TextureName.cursor, new Point(0, 0), 16, 16);
            int width = 100;
            int height = 100;
            int bottomOffset = Engine.YRes/4 + height;
            ButtonBox = new Rectangle((Engine.XRes - width) / 2, Engine.YRes - bottomOffset, width, height);
            buttonBoxOutline = new Doodad(ButtonBox, Color.Black, false);
            vertButtonSpacing = ButtonBox.Height / buttons.Count;
            for (int i = 0; i < buttons.Count; i += 1)
            {
                drawPositions.Add(new Vector2(ButtonBox.Left, ButtonBox.Top + vertButtonSpacing * i));
            }
        }

        public override void Update(ControlManager controls)
        {
            base.Update(controls);
        }

        public override void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            batch.Begin();
            for (int i = 0; i < buttons.Count; i += 1)
            {
                buttons[i].Draw(batch, drawPositions[i]);
            }
            cursorSprite.Draw(batch, new Vector2(ButtonBox.Left - cursorSprite.Width, ButtonBox.Top + vertButtonSpacing * cursor));
            buttonBoxOutline.Draw(batch);
            batch.End();
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
            resolveCursor();
        }

        private static List<Button> getButtons()
        {
            List<Button> buttons = new List<Button>();
            SpriteFont font = FontBank.Get(FontName.consolas);
            ListButton start =
                new ListButton(new LoadLevelEvent(),
                               new SpriteText(font, "Start", Color.Black));
            ListButton scores =
                new ListButton(new ToScoresEvent(),
                               new SpriteText(font, "High Scores", Color.Black));
            ListButton quit =
                new ListButton(new QuitEvent(),
                               new SpriteText(font, "End", Color.Black));
            buttons.Add(start);
            buttons.Add(scores);
            buttons.Add(quit);
            return buttons;

        }
    }
}
