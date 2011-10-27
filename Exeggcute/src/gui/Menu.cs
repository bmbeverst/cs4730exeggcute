using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Exeggcute.src.input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Exeggcute.src.graphics;
using Exeggcute.src.assets;
using Microsoft.Xna.Framework.Audio;
using Exeggcute.src.sound;

namespace Exeggcute.src.gui
{
    abstract class Menu : IContext
    {
        protected int cursor;
        protected List<Button> buttons;
        protected List<Vector2> drawPositions;
        protected Rectangle buttonBounds;
        protected RectSprite outline;
        protected bool loops;
        protected float buttonHeight;
        protected Sprite cursorSprite;
        protected SpriteFont font;
        protected Color fontColor;
        protected RepeatedSound selectSound;
        protected SoundEffect cancelSound;

        public Menu(List<Button> buttons, Rectangle bounds, bool loops)
        {
            this.font = FontBank.Get("consolas");
            this.fontColor = Color.White;
            this.buttonHeight = buttons[0].Height;
            this.cursor = 0;
            this.buttons = buttons;
            this.loops = loops;
            this.buttonBounds = bounds;
            this.outline = new RectSprite(buttonBounds, Color.Black, false);
            this.drawPositions = new List<Vector2>();
            Texture2D cursorTexture = TextureBank.Get("cursor");
            this.cursorSprite = new StaticSprite(cursorTexture, new Point(0, 0), 16, 16);
            int x = bounds.X;
            int y = bounds.Y;
            float spacing = buttonHeight;
            for (int i = 0; i < buttons.Count; i += 1)
            {
                drawPositions.Add(new Vector2(x, y + spacing * i));
                buttons[i].AttachParent(this);
            }

            this.cancelSound = SfxBank.DeprecatedGetSound("back");
            this.selectSound = SfxBank.MakeRepeated("select");

        }

        public virtual void Back()
        {
            cancelSound.Play();
        }

        public virtual void Select()
        {
            Console.WriteLine("Should have played");
            selectSound.Play();
        }

        public virtual void Update(ControlManager controls)
        {
            ResolveCursor();
            buttons[cursor].Update(controls);
            ResolveCursor();
        }

        public virtual void Draw(GraphicsDevice graphics, SpriteBatch batch)
        {
            cursorSprite.Draw(batch, new Vector2(buttonBounds.Left - cursorSprite.Width - 3, buttonBounds.Top + buttonHeight * cursor));
            for (int i = 0; i < buttons.Count; i += 1)
            {
                buttons[i].Draw(batch, drawPositions[i]);
            }
            outline.Draw(batch, drawPositions[0]);
        }


        public virtual void Load(ContentManager content)
        {

        }

        public virtual void Unload()
        {

        }

        public virtual void Dispose()
        {

        }

        public virtual void Move(Direction dir)
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

        public virtual bool ResolveCursor()
        {
            selectSound.Update();
            if (loops)
            {
                cursor = (cursor + buttons.Count) % buttons.Count;
                return true;
            }
            else
            {
                if (cursor < 0)
                {
                    cursor = 0;
                    return false;
                }
                else if (cursor > buttons.Count - 1)
                {
                    cursor = buttons.Count - 1;
                    Console.WriteLine("this universe");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            //else cursor = Util.Clamp(cursor, 0, buttons.Count - 1);
        }

        public virtual void Cleanup()
        {

        }


    }
}
