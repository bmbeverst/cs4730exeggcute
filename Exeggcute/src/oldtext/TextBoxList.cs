using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Exeggcute.src.oldtext
{
    //DEPRECATED
    class TextBoxList //draw in here
    {
        public List<TextBox> boxList;
        public int currentBox { get; protected set; }
        public bool isDone { get; protected set; }
        //for this one must be a button press
        public bool incrementCondition { get; protected set; }
        private int cnt = 0;
        public int switchCounter 
        {
            get { return cnt; }
            protected set {
                cnt = value % 1; //scrolltimer
            }
        }
        public SpriteFont font;
        public TextBoxList(List<TextBox> blist, SpriteFont f)
        {
            boxList = blist;
            font = f;
            currentBox = 0;
            isDone = false;
            switchCounter = 1;
            incrementCondition = true;
            throw new Exception("DEPRECATED");
        }

        public void increment(int aFlag) //do we move to next text box?
        {
            if (boxList[currentBox].isDone)
            {
                if (currentBox < boxList.Count - 1)
                {
                    if (switchCounter == 0)
                    {
                        if (aFlag >= 1)
                        {
                            switchCounter = 1;
                            currentBox += 1;
                        }
                    }
                    else
                    {
                        switchCounter += 1;
                    }
                }
                else 
                {
                    if (switchCounter == 0)
                    {
                        if (aFlag >= 1)
                        {
                            switchCounter = 1;
                            isDone = true;
                        }
                    }
                    else
                    {
                        switchCounter += 1;
                    }
                }
            }
            else
            {
                if (incrementCondition)
                {
                    boxList[currentBox].increment();
                }
            }

        }

        public void write(SpriteBatch batch)
        {
            TextBox box = boxList[currentBox];
            
            for (int i = 0; i < box.currentLine+ 1; i += 1)
            {
                string writeline = box.lines[i].getSubstring();
                batch.DrawString(font, writeline, new Vector2(211, 396 + 22 * i), Color.White);
            
            }
        }
        public void reset()
        {
            foreach (TextBox box in boxList)
            {
                box.reset();
            }
            currentBox = 0;
            isDone = false;
        }
            

    }
}
