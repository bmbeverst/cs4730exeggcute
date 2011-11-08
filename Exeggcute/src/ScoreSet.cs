using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Exeggcute.src
{
    class ScoreSet
    {
        public const int LENGTH = 10;
        protected List<ScoreEntry> localScores = new List<ScoreEntry>(); //ScoreEntry[] localScores = new ScoreEntry[LENGTH];
        protected List<ScoreEntry> networkScores = new List<ScoreEntry>(); //ScoreEntry[] networkScores = new ScoreEntry[LENGTH];
        private const string FILE = "score.bin";
        private const char DELIM = ',';

        //private bool networkUpdated = false;

        private bool networkAlreadyLoaded = false;
        public bool ViewingNetwork = false;
        public ScoreSet()
        {
            LoadLocal();
        }

        public void SortScores(List<ScoreEntry> scores)
        {
            scores.Sort();
        }

        public void LoadLocal()
        {
            List<string> lines = Util.ReadAndStrip(FILE, true);
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

                bool duplicate = false;
                foreach (ScoreEntry currentEntry in localScores)
                {
                    if (currentEntry.IntScore == entry.IntScore && currentEntry.Name == entry.Name && currentEntry.Date == entry.Date)
                    {
                        duplicate = true;
                    }
                }
                if (!duplicate)
                {
                   localScores.Add(entry);
                }
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
            if (!networkAlreadyLoaded)
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse("128.143.69.241"), 9030);

                Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    server.Connect(ip);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("{0}\nUnable to connect to server.", e.Message);
                    return;
                }

                server.Send(Encoding.ASCII.GetBytes("GetHighScores\r\n"));

                String name;
                int score;
                String date;
                byte[] data;
                int receivedDataLength;
                string scoreData;
                string stringData;
                string stringData2;

                data = new byte[1024];
                receivedDataLength = server.Receive(data);
                stringData = Encoding.ASCII.GetString(data, 0, receivedDataLength);

                receivedDataLength = server.Receive(data);
                stringData2 = Encoding.ASCII.GetString(data, 0, receivedDataLength);

                stringData = stringData + stringData2;

                while (networkScores.Count < 10) // just make sure always > 10 entries in table
                {
                    int index = stringData.IndexOf("\n");
                    scoreData = stringData.Substring(0, index);
                    stringData = stringData.Substring(index + 1);

                    score = Int32.Parse(scoreData);
                    Console.WriteLine(score);

                    index = stringData.IndexOf("\n");
                    name = stringData.Substring(0, index);
                    stringData = stringData.Substring(index + 1);
                    Console.WriteLine(name);

                    index = stringData.IndexOf("\n");
                    date = stringData.Substring(0, index);
                    stringData = stringData.Substring(index + 1);
                    Console.WriteLine(date);

                    bool duplicate = false;
                    foreach (ScoreEntry entry in networkScores)
                    {
                        if (entry.IntScore == score && entry.Name == name && entry.Date == date)
                        {
                            duplicate = true;
                        }
                    }
                    if (!duplicate)
                    {
                        networkScores.Add(new ScoreEntry(score, name, date));
                    }
                }
                server.Shutdown(SocketShutdown.Both);
                server.Close();
            }

            ViewingNetwork = true;
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
            HashSet<ScoreEntry> newEntries = new HashSet<ScoreEntry>();

            foreach (ScoreEntry entry in localScores)
            {
                newEntries.Add(entry);
            }

            foreach (ScoreEntry entry in networkScores)
            {
                newEntries.Add(entry);
            }

            List<ScoreEntry> listEntries = newEntries.ToList();
            listEntries.Sort();
            int size = listEntries.Count;
            listEntries.RemoveRange(10, size - 10);

            if (!networkAlreadyLoaded)
            {
                //byte[] data;
                //int receivedDataLength;
                //string stringData;

                int counter = 0;
                bool duplicate = false;
                foreach (ScoreEntry entry in networkScores)
                {
                    counter = 0;
                    if (entry.IntScore == listEntries.ElementAt(counter).IntScore && entry.Name == listEntries.ElementAt(counter).Name
                        && entry.Date == listEntries.ElementAt(counter).Date)
                    {
                        duplicate = true;
                    }
                    counter++;
                }
                if (!duplicate)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        IPEndPoint ip = new IPEndPoint(IPAddress.Parse("128.143.69.241"), 9030);

                        Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                        try
                        {
                            server.Connect(ip);
                        }
                        catch (SocketException e)
                        {
                            Console.WriteLine("{0}\nUnable to connect to server.", e.Message);
                            return;
                        }

                        String query = "InsertInto" + hackToString(listEntries.ElementAt(i).IntScore) + "&" +
                                   listEntries.ElementAt(i).Name + "&" + listEntries.ElementAt(i).Date + "\r\n";
                        server.Send(Encoding.ASCII.GetBytes(query));

                        server.Shutdown(SocketShutdown.Both);
                        server.Close();
                    }
                }
            }

            Clean();
            ViewingNetwork = true;
        }
        private string hackToString(int score)
        {
            return string.Format("{0:000,000,000}", score);
        }
        private void Clean()
        {
            IPEndPoint ip = new IPEndPoint(IPAddress.Parse("128.143.69.241"), 9030);

            Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                server.Connect(ip);
            }
            catch (SocketException e)
            {
                Console.WriteLine("{0}\nUnable to connect to server.", e.Message);
                return;
            }

            String query = "Clean\r\n";
            server.Send(Encoding.ASCII.GetBytes(query));

            server.Shutdown(SocketShutdown.Both);
            server.Close();
        }

        public void TryInsert(int score)
        {
            string name = Environment.UserName;
            DateTime now = DateTime.Now;
            int day = now.Day;
            int month = now.Month;
            int year = now.Year;
            if (year > 2000)
            {
                year -= 2000;
            }
            string date = string.Format("{0:00}.{1:00}.{2:00}", month, day, year);
            ScoreEntry newEntry = new ScoreEntry(score, name, date);

            localScores.Add(newEntry);
            localScores.Sort();
            localScores.RemoveAt(localScores.Count() - 1);
        }

        protected ScoreEntry parseElement(string[] tokens)
        {
            int score = int.Parse(tokens[0]);
            string name = tokens[1];
            string date = tokens[2];
            return new ScoreEntry(score, name, date);
        }

        public void Draw(SpriteBatch batch, SpriteFont font, Vector2 pos, Color color)
        {
            float height = font.MeasureString("A").Y;
            List<ScoreEntry> /*ScoreEntry[]*/ toDraw = ViewingNetwork ? networkScores : localScores;
            if (networkScores.Count == 0)
            {
                toDraw = localScores;
            }
            SortScores(toDraw);
            string headings = string.Format("   Score              {0,4}             {1}", "Name", "Date");
            batch.DrawString(font, headings, new Vector2(260, 120 - height), color);
            for (int i = 0; i < LENGTH; i += 1)
            {
                toDraw[i].Draw(batch, font, pos + new Vector2(260, 150+height * i), color);
            }
        }
    }
}
