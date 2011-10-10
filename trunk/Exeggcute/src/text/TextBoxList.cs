using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Exeggcute.src.graphics;
using Exeggcute.src.input;

namespace Exeggcute.src.text
{
    /// <summary>
    /// TODO: draw background, draw shadows, make MessageMenu container class
    /// </summary>
    class TextBoxList
    {
        private List<TextBox> boxes;
        private int boxPtr;

        private IDrawable2D bg;
        private IDrawable2D arrow;
        private FloatTimer timer;

        private SpriteFont font;

        public Rectangle TextBounds { get; protected set; }
        public bool IsDone
        {
            get
            {
                if (boxPtr > boxes.Count) throw new InvalidOperationException();
                return boxPtr == boxes.Count && shouldExit;
            }
        }

        private bool shouldExit = false;

        public TextBoxList(SpriteFont font, string message, float rate, Rectangle bounds)
        {
            this.font = font;
            timer = new FloatTimer(rate);
            string[] words = message.Split(' ');
            List<TextLine> lines = parseLines(words, font);
        }

        public void Update(ControlManager controls)
        {
            timer.Increment();
            TextBox current = boxes[boxPtr];
            for (int i = 0; i < timer.GetDelta() && !current.IsDone; i += 1)
            {
                current.Increment();
            }

            if (current.IsDone && controls[Ctrl.Action].JustPressed)
            {
                boxPtr += 1;
                shouldExit = (boxPtr == boxes.Count);
            }
            
        }

        public void Draw(SpriteBatch batch, SpriteFont font, Vector2 pos, Color color, int spacingY)
        {

        }

        public void Reset()
        {
            boxPtr = 0;
            foreach (TextBox box in boxes)
            {
                box.Reset();
            }
        }

        private List<TextLine> parseLines(string[] words, SpriteFont font)
        {
            List<TextLine> lines = new List<TextLine>();
            string actual = "";
            for (int i = 0; i < words.Length; i += 1)
            {
                string space = (actual.Length == 0) ? "" : " ";
                float lookAhead = font.MeasureString(actual + space + words[i]).X;
                if (lookAhead > TextBounds.Width)
                {
                    Console.WriteLine("Adding line {0}", actual);
                    lines.Add(new TextLine(actual));
                    actual = "";
                }

                actual += space + words[i];
            }
            if (actual.Length > 0)
            {
                lines.Add(new TextLine(actual));
            }

            return lines;
        }
    }
}
