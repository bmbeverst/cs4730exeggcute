﻿using System;
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
using Exeggcute.src.text;
using Exeggcute.src.console.commands;

namespace Exeggcute.src.gui
{
    abstract class Menu : ConsoleContext
    {
        protected int cursor;
        protected List<Button> buttons;
        protected List<Vector2> drawPositions;
        protected Rectangle buttonBounds;
        protected RectSprite outline;
        protected RectSprite outlineShadow;

        protected bool loops;
        protected float buttonHeight;
        protected Sprite cursorSprite;
        protected SpriteFont font;
        protected Color fontColor;
        protected SoundEffect selectSound;
        protected SoundEffect cancelSound;

        public Menu(List<Button> buttons, Rectangle bounds, bool loops)
        {
            Color bgColor = new Color(0, 170, 0);
            this.font = Assets.Font["consolas"];
            this.fontColor = Color.White;
            this.buttonHeight = buttons[0].Height;
            this.cursor = 0;
            this.buttons = buttons;
            this.loops = loops;
            this.buttonBounds = Util.NearestMult(bounds, 16);
            this.outline = new RectSprite(buttonBounds, bgColor, true);
            int shadowOffset = 6;
            Rectangle offsetOutline = new Rectangle(buttonBounds.Left + shadowOffset,
                                                    buttonBounds.Top + shadowOffset,
                                                    buttonBounds.Width + shadowOffset * 2,
                                                    buttonBounds.Height + shadowOffset * 2);
            this.outlineShadow = new RectSprite(offsetOutline, Color.Black, true);
            this.drawPositions = new List<Vector2>();
            Texture2D cursorTexture = Assets.Texture["cursor"];
            this.cursorSprite = new StaticSprite(cursorTexture, new Point(0, 0), 16, 16);
            int x = buttonBounds.X;
            int y = buttonBounds.Y;
            float spacing = buttonHeight;
            for (int i = 0; i < buttons.Count; i += 1)
            {
                drawPositions.Add(new Vector2(x, y + spacing * i));
                buttons[i].AttachParent(this);
            }

            this.cancelSound = Assets.Sfx["back"];
            this.selectSound = Assets.Sfx["select"];

        }

        public override void AcceptCommand(ConsoleCommand command)
        {
            throw new NotImplementedException();
        }

        public void AttachParent(IContext parent)
        {
            this.Parent = parent;
        }

        public virtual void Back()
        {
            cancelSound.Play();
        }

        public virtual void Select()
        {
            selectSound.Play();
        }

        public override void Update(ControlManager controls)
        {
            ResolveCursor();
            buttons[cursor].Update(controls);
            ResolveCursor();
        }

        public override void Draw2D(SpriteBatch batch)
        {
            int xOffset = -16;
            outlineShadow.Draw(batch, drawPositions[0] + new Vector2(xOffset + 16, 16));
            outline.Draw(batch, drawPositions[0] + new Vector2(xOffset, 0));
            TextBox.UpperLeftSprite.Draw(batch, new Vector2(buttonBounds.Left - 16 + xOffset, buttonBounds.Top - 16));
            TextBox.LowerLeftSprite.Draw(batch, new Vector2(buttonBounds.Left - 16 + xOffset, buttonBounds.Bottom));
            TextBox.UpperRightSprite.Draw(batch, new Vector2(buttonBounds.Right + xOffset, buttonBounds.Top - 16));
            TextBox.LowerRightSprite.Draw(batch, new Vector2(buttonBounds.Right + xOffset, buttonBounds.Bottom));
            for (int i = 0; i < buttonBounds.Width / 16; i += 1)
            {
                TextBox.TopSprite.Draw(batch, new Vector2(buttonBounds.Left + i * 16 + xOffset, buttonBounds.Top - 16));
                TextBox.LowerSprite.Draw(batch, new Vector2(buttonBounds.Left + i * 16 + xOffset, buttonBounds.Bottom));
            }
            for (int i = 0; i < buttonBounds.Height / 16; i += 1)
            {
                TextBox.LeftSprite.Draw(batch, new Vector2(buttonBounds.Left - 16 + xOffset, buttonBounds.Top + i * 16));
                TextBox.RightSprite.Draw(batch, new Vector2(buttonBounds.Right + xOffset, buttonBounds.Top + i * 16));
            }

            cursorSprite.Draw(batch, new Vector2(buttonBounds.Left - cursorSprite.Width - 3, buttonBounds.Top + buttonHeight * cursor + 5));
            for (int i = 0; i < buttons.Count; i += 1)
            {
                buttons[i].Draw(batch, drawPositions[i]);
            }
            
        }

        public override void Draw3D(GraphicsDevice graphics, Camera camera)
        {

        }


        public override void Unload()
        {

        }

        public override void Dispose()
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
                    return false;
                }
                else
                {
                    return true;
                }
            }
            //else cursor = Util.Clamp(cursor, 0, buttons.Count - 1);
        }


    }
}
