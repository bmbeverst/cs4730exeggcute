using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.text
{
    class TextLine
    {
        private string line;
        private int linePtr;
        public float Width { get; protected set; }
        public float Height { get; protected set; }
        public bool IsDone
        {
            get { return linePtr == line.Length; }
        }

        public TextLine(string line)
        {
            this.line = line;
            this.linePtr = 0;
        }

        public static TextLine GetStatic(string line)
        {
            TextLine result = new TextLine(line);
            result.linePtr = line.Length;
            return result;
        }

        /// <summary>
        /// Increment the line by one character.
        /// <requires>!IsDone</requires>
        /// </summary>
        public void Increment()
        {
            if (IsDone) throw new InvalidOperationException("Do not increment if line is done");
            linePtr += 1;
        }

        public void Reset()
        {
            linePtr = 0;
        }

        public void Draw(SpriteBatch batch, SpriteFont font, Vector2 pos, Color color)
        {
            batch.DrawString(font, line.Substring(0, linePtr), pos, color);
        }

    }
}
