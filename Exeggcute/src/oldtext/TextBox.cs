using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Exeggcute.src.oldtext
{
    // DEPRECATED
    class TextBox
    {
        public TextLine[] lines;
        public int currentLine { get; protected set; }
        public bool isDone { get; protected set; }
        public Random r = new Random();
        public bool incrementCondition = true; // make it increment when a condition is met (to control speed of scrolling)
        public TextBox(TextLine[] l)
        {
            lines = l;
            isDone = false;
            currentLine = 0;
            throw new Exception("DEPRECATED");
        }

        public void increment()
        {
            if (lines[currentLine].isDone)
            {
                if (currentLine < 5 && lines[currentLine + 1] != null)
                {
                    currentLine += 1;
                }
                else
                {
                    isDone = true;
                }
            }
            else if (incrementCondition)
            {
                lines[currentLine].increment();
            }
            //else wait for increment condition (do nothing)
        }
        public void reset()
        {
            foreach (TextLine line in lines)
            {
                if (line != null)
                {
                    line.reset();
                }
            }
            currentLine = 0;
            isDone = false;
        }
    }
}
