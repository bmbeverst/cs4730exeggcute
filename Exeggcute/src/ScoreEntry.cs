using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src
{
    class ScoreEntry
    {
        public int Score { get; protected set; }
        public string Name { get; protected set; }
        public DateTime Time { get; protected set; }
        public ScoreEntry(int score, string name, DateTime time)
        {
            Score = score;
            Name = name;
            Time = time;
        }

        public ScoreEntry()
        {
            Score = default(int);
            Name = "AAA";
            Time = default(DateTime);
        }

        public override string ToString()
        {
            return string.Format("{0},{1},{2}", Score, Name, Time.ToString());
        }

    }
}
