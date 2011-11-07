using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using Exeggcute.src.assets;
using Exeggcute.src.graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src.text
{
    class TextBox
    {
        private SpeechSynthesizer synth;
        private List<TextLine> lines;
        private int linePtr;
        private string total;
        private bool speechStarted;
        
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

        public static Sprite UpperLeftSprite;
        public static Sprite UpperRightSprite;
        public static Sprite LowerLeftSprite;
        public static Sprite LowerRightSprite;
        public static Sprite LeftSprite;
        public static Sprite RightSprite;
        public static Sprite TopSprite;
        public static Sprite LowerSprite;
        public static Sprite MiddleSprite;
        public TextBox(List<TextLine> lines)
        {
            this.lines = lines;
            this.total = "";
            foreach (var line in lines)
            {
                total += line.Line;
            }

            synth = new SpeechSynthesizer();
            synth.SelectVoiceByHints(VoiceGender.NotSet, VoiceAge.NotSet, 0);
            //synth.SelectVoice("LH Michael");
        }

        public static void LoadSprites()
        {
            UpperLeftSprite = Assets.Sprite["textboxUL"];
            UpperRightSprite = Assets.Sprite["textboxUR"];
            LowerLeftSprite = Assets.Sprite["textboxLL"];
            LowerRightSprite = Assets.Sprite["textboxLR"];
            LeftSprite = Assets.Sprite["textboxLeft"];
            RightSprite = Assets.Sprite["textboxRight"];
            TopSprite = Assets.Sprite["textboxTop"];
            LowerSprite = Assets.Sprite["textboxLower"];
            MiddleSprite = Assets.Sprite["textboxMiddle"];
        }

        public void Start()
        {
            synth.SpeakAsync(total);
        }

        public void Stop()
        {
            synth.SpeakAsyncCancelAll();
        }

        public void Draw(SpriteBatch batch, SpriteFont font, Vector2 pos, Color color, float spacingY)
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
            if (!speechStarted) Start();
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
            speechStarted = false;
            synth.SpeakAsyncCancelAll();
        }
    }
}
