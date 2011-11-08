using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src
{
    class ScoreEntry : IComparable<ScoreEntry>
    {
        public string StringScore { get; protected set; }
        public int IntScore { get; protected set; }
        //public int Score { get; protected set; }
        public string Name { get; protected set; }
        public string Date { get; protected set; }

        public ScoreEntry(int score, string name, string date)
        {
            this.StringScore = IntToString(score);
            this.IntScore = score;
            this.Name = name;
            this.Date = date;
        }

        public ScoreEntry(int score, string name)
        {
            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;
            int year = now.Year;
            if (year > 2000)
            {
                year -= 2000;
            }
            string date = string.Format("{0:00}.{1:00}.{2:00}", month, day, year);

            this.StringScore = IntToString(score);
            this.IntScore = score;
            this.Name = name;
            this.Date = date;
        }

        public ScoreEntry()
        {
            this.IntScore = 0;
            this.StringScore = IntToString(IntScore);
            Name = "AAA";
            Date = "January 1, 2000";
        }

        public static string IntToString(int x)
        {
            return string.Format("{0:000,000,000}", x);
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", StringScore, Name, Date);
        }

        public int CompareTo(ScoreEntry other)
        {
            // Alphabetic sort by name if score is equal. [A to Z]
            if (this.IntScore == other.IntScore)
            {
                return this.Name.CompareTo(other.Name);
            }
            // Default to score sort. [High to low]
            return other.IntScore.CompareTo(this.IntScore);
        }

        public void Draw(SpriteBatch batch, SpriteFont font, Vector2 pos, Color color)
        {
            string scoreString = string.Format("{0:000,000,000}    {1,18}    {2}", IntScore, Name, Date);
            batch.DrawString(font, scoreString, pos, color);
        }

    }
}
