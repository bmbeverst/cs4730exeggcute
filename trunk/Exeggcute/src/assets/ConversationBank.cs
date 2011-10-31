using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.text;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.assets
{
    class ConversationBank
    {
        protected static Dictionary<string, Conversation> cache =
            new Dictionary<string, Conversation>();

        public static Conversation Get(string name)
        {
            return cache[name];
        }
        const float scrollSpeed = 0.3f;
        public static void LoadAll()
        {
            SpriteFont font = Assets.Font["consolas"];
            List<string> allLines = Util.ReadLines("data/msg_boxes.txt");
            string total = "";
            for (int i = 0; i < allLines.Count; i += 1)
            {
                string line = allLines[i].TrimEnd(' ');
                line = line + ' ';
                total += line;
            }

            string[] messages = total.Split('@');
            for (int i = 1; i < messages.Length; i += 1)
            {
                TextBoxList box =  new TextBoxList(font, messages[i], scrollSpeed);
                Conversation convo = new Conversation(box);
                //FIXME make this nicer
                string name = string.Format("{0}", i - 1);
                cache[name] = convo;
            }
        }
    }
}
