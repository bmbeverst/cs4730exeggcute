using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.oldtext
{
    //DEPRECATED
    class TextLine
    {
        public string line {get; protected set;}
        public int currentSpot;
        public bool isDone { get; protected set;}
        public TextLine(string s)
        {
            line = s;
            currentSpot = 0;
            isDone = false;
            throw new Exception("DEPRECATED");
        }
        public void increment()
        {
            if (currentSpot < line.Length - 1)
            {
                currentSpot += 1;
            }
            else
            {
                isDone = true;
            }
        }
        public string getSubstring()
        {
            string res;
            
            if (!isDone)
            {
                res = line.Substring(0, currentSpot);
            }
            else
            {
                res = line;
            }
            return res ; //WARNING this might be wrong, might be 1 too short/too long
        }
        public void reset()
        {
            currentSpot = 0;
            isDone = false;
        }
        public override string ToString()
        {
            return getSubstring();
        }
    }
}
