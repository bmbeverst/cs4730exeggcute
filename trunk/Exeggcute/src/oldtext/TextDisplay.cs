using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Exeggcute.src.oldtext
{
    //DEPRECATED
    class TextDisplay
    {
        public Texture2D texture { get; set; }
        public TextBoxList tbList;
        public string text;
        public SpriteFont font;
        //which box are we in?
        //public int currentBox = 0;
        //in the current box, what line are we on?
        //public int currentLine = 0;
        //in the current line, what spot are we at?
        //public int currentSpot = 0;
        public bool isDone = false;
        public bool beenRead = false;
        public TextDisplay(string txt, Vector2 origin, Texture2D[] t, SpriteFont f, SoundEffect[] s, int num)
        {
            font = f;
            text = txt;
            tbList = initialize();
            throw new Exception("DEPRECATED");
        }
        public TextBoxList initialize()
        {
            string[] stringArray = text.Split(' ');
            string possibleLine = "";
            string fullline = "";
            List<string> lines = new List<string>();
            for (int i = 0; i < stringArray.Length; i += 1)
            {
                possibleLine += " " + stringArray[i];
                if (font.MeasureString(possibleLine).X > 375)
                {
                    lines.Add(fullline);
                    fullline = "";
                    possibleLine = " " + stringArray[i];
                }
                fullline += " " + stringArray[i];
                if (i == stringArray.Length - 1)
                {
                    lines.Add(possibleLine);
                }
            }
            List<TextBox> result = new List<TextBox>();
            while (true)
            {
                TextLine[] box = new TextLine[6];
                bool doneflag = false;
                for (int i = 0; i < 6; i += 1)
                {
                    if (lines.Count > 0)
                    {
                        string nextline = lines[0];
                        lines.RemoveAt(0);
                        box[i] = new TextLine(nextline);
                    }
                    else
                    {
                        box[i] = null;
                        doneflag = true;
                    }
                }
                result.Add(new TextBox(box));
                if (doneflag) {
                    break;
                }
                
            }
            foreach (TextBox box in result)
            {
                for (int i = 0; i < 6; i += 1)
                {
                    if (box.lines[i] != null)
                    {
                        //Admin.debug(box.lines[i].ToString());
                    }
                }
                //Admin.debug("Done box\n\n\n");
            }
            return new TextBoxList(result, font);
        }
        public void execute(int aFlag)
        {
            tbList.increment(aFlag);
            if (tbList.isDone)
            {
                isDone = true;
                //Admin.debug("dundundun");
            }
            
        }
        public void write(SpriteBatch batch)
        {
            tbList.write(batch);
        }
        public void reset()
        {
            beenRead = true;
            tbList.reset();
            isDone = false;
        }
    }
}
