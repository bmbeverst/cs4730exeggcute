﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;
using Exeggcute.src.graphics;
using Exeggcute.src.input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.text
{
    /// <summary>
    /// TODO: draw background, draw shadows, make MessageMenu container class
    /// </summary>
    class TextBoxList
    {
        private List<TextBox> boxes;
        private int boxPtr;

        private RectSprite bg;
        private Sprite arrow;
        private FloatTimer timer;

        private float spacingY;

        private SpriteFont font;

        public Point Size { get; protected set; }

        public bool IsDone
        {
            get
            {
                if (boxPtr > boxes.Count) throw new InvalidOperationException();
                return boxPtr == boxes.Count && shouldExit;
            }
        }

        private bool shouldExit = false;

        public TextBoxList(SpriteFont font, string message, float rate)
        {
            
            //FIXME
            this.Size = new Point(500, 200);
            // =C
            this.arrow = Assets.Sprite["cursor"];
            this.font = font;
            this.boxes = new List<TextBox>();
            this.timer = new FloatTimer(rate);
            string[] words = message.Split(' ');
            //need to create
            Rectangle hackRect = Util.NearestMult(
                                     new Rectangle(0, 0, Size.X, Size.Y), 16);
            this.bg = new RectSprite(hackRect, new Color(0, 170, 0), true);
            
            List<TextLine> lines = parseLines(words, font);
            lines.Reverse();
            Stack<TextLine> lineStack = new Stack<TextLine>(lines);
            int count = lineStack.Count;

            this.spacingY = font.LineSpacing;

            int linesPerBox = (int)(Size.Y / spacingY);
            int remainder = count % linesPerBox;
            int numBoxes = remainder == 0 ? count / linesPerBox : count / linesPerBox + 1;
            for (int i = 0; i < numBoxes; i += 1)
            {
                List<TextLine> box = new List<TextLine>();
                for (int k = 0; k < linesPerBox; k += 1)
                {
                    if (lineStack.Count > 0)
                    {
                        box.Add(lineStack.Pop());
                    }
                }
                boxes.Add(new TextBox(box));
            }
        }

        public void Update(ControlManager controls)
        {
            timer.Increment();
            TextBox current = boxes[boxPtr];
            float delta = timer.GetDelta();
            for (int i = 0; i < delta && !current.IsDone; i += 1)
            {
                current.Increment();
            }
            if (controls[Ctrl.Action].IsPressed && !current.IsDone)
            {
                current.Increment();
            }

            if (current.IsDone && controls[Ctrl.Action].JustPressed)
            {
                current.Stop();
                boxPtr += 1;
                shouldExit = (boxPtr == boxes.Count);
            }
            
        }

        public void Draw(SpriteBatch batch, Vector2 pos, Color color)
        {
            int xOffset = -16;
            Rectangle buttonBounds = new Rectangle((int)pos.X, (int)pos.Y, bg.Size.X, bg.Size.Y);

            bg.Draw(batch, pos + new Vector2(xOffset, 0));
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

            //bg.Draw(batch, pos);
            TextBox current = boxes[boxPtr];
            boxes[boxPtr].Draw(batch, font, pos + new Vector2(0,10), color, spacingY);

            if (current.IsDone && boxPtr != boxes.Count - 1)
            {
                //arrow.Draw(batch, pos);
            }
        }

        public void Reset()
        {
            boxPtr = 0;
            foreach (TextBox box in boxes)
            {
                box.Reset();
            }
            shouldExit = false;
            timer.Reset();
        }

        private List<TextLine> parseLines(string[] words, SpriteFont font)
        {
            List<TextLine> lines = new List<TextLine>();
            string actual = "";
            for (int i = 0; i < words.Length; i += 1)
            {
                string space = (actual.Length == 0 && i != 0) ? "" : " ";
                float lookAhead = font.MeasureString(actual + space + words[i]).X;
                if (lookAhead > Size.X)
                {
                    lines.Add(new TextLine(actual));
                    actual = "";
                }

                actual += words[i] + space;
            }
            if (actual.Length > 0)
            {
                lines.Add(new TextLine(actual));
            }

            return lines;
        }
    }
}
