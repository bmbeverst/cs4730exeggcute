using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.text
{
    class TextBox
    {
        private List<TextLine> lines;
        private int linePtr;
        public TextLine CurrentLine
        {
            get { return lines[linePtr]; }
        }
        public bool IsDone 
        {
            get 
            {
                if (linePtr > lines.Count) throw new Exception("something went wrong");
                return linePtr == lines.Count; 
            }  
        }

        
        public TextBox(List<TextLine> lines)
        {
            this.lines = lines;
        }

        public void Draw(SpriteBatch batch, SpriteFont font, Vector2 pos, Color color, int spacingY)
        {
            //I don't remember why this works!
            int jMax = linePtr < lines.Count ? linePtr + 1 : linePtr;
            for (int j = 0; j < jMax; j += 1)
            {
                lines[j].Draw(batch, font, pos + new Vector2(0,j * spacingY), color);
            }
        }

        /// <summary>
        /// Increment the current line by one character.
        /// <requires>!IsDone</requires>
        /// </summary>
        public void Increment()
        {
            if (IsDone) throw new InvalidOperationException("Do not increment if IsDone");
            if (!CurrentLine.IsDone)
            {
                CurrentLine.Increment();
            }
            else
            {
                linePtr += 1;
            }
        }

        public void Reset()
        {
            linePtr = 0;
            foreach (TextLine line in lines)
            {
                line.Reset();
            }
        }
    }
}
