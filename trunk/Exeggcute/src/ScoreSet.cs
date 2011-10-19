using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting;

namespace Exeggcute.src
{
    class ScoreSet
    {
        public const int LENGTH = 10;
        protected ScoreEntry[] localScores = new ScoreEntry[LENGTH];
        protected ScoreEntry[] networkScores = new ScoreEntry[LENGTH];
        private const string FILE = "data/score.dat";
        private const char DELIM = ',';
        private bool networkUpdated = false;

        public ScoreSet()
        {
            LoadLocal();
        }

        public void LoadLocal()
        {
            List<string> lines = Util.StripComments(FILE, '#', true);
            if (lines.Count != 10)
            {
                restore(lines);
            }
            for (int i = 0; i < LENGTH; i += 1)
            {
                string line = lines[i];
                string[] tokens = line.Split(DELIM);
                ScoreEntry entry;
                try
                {
                    entry = parseElement(tokens);
                }
                catch (Exception error)
                {
                    throw new ParseError("{0}\nFailed to parse line {1}", error.Message, line);
                }
                localScores[i] = entry;
            }
        }

        protected void restore(List<string> lines)
        {
            Util.Warn("Score list not fully populated, adding dummy entries");
            for (int i = lines.Count; i < LENGTH; i += 1)
            {
                lines.Add(getDefaultEntry());
            }
        }

        public string getDefaultEntry()
        {
            return (new ScoreEntry()).ToString();
        }

        public void LoadNetwork()
        {
            // TODO load high scores from the network
        }

        public void WriteLocal()
        {
            string output = "";
            foreach (ScoreEntry entry in localScores)
            {
                output += entry.ToString() + "\n";
            }
            Util.WriteFile(FILE, output);
        }

        public void WriteNetwork()
        {

        }

        protected ScoreEntry parseElement(string[] tokens)
        {
            int score = int.Parse(tokens[0]);
            string name = tokens[1];
            DateTime time = DateTime.Parse(tokens[2]);
            return new ScoreEntry(score, name, time);
        }
    }
}
