using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Exeggcute.src
{
    class ScoreEntry
    {
        public string StringScore { get; protected set; }
        public int IntScore { get; protected set; }
        public string Name { get; protected set; }
        public DateTime Time { get; protected set; }
        public ScoreEntry(int score, string name, DateTime time)
        {

            this.StringScore = IntToString(score);
            this.IntScore = score;
            this.Name = name;
            this.Time = time;
        }

        public ScoreEntry()
        {
            this.IntScore = 0;
            this.StringScore = IntToString(IntScore);
            this.Name = "AAA";
            this.Time = default(DateTime);
        }

        public static string IntToString(int x)
        {
            return string.Format("{0:000,000,000}", x);
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", StringScore, Name, Time);
        }

        public void Draw(SpriteBatch batch, SpriteFont font, Vector2 pos, Color color)
        {
            batch.DrawString(font, ToString(), pos, color);
        }

    }
}
