using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Net;
using System.Net.Sockets;

namespace Exeggcute.src
{
    class ScoreSet
    {
        public const int LENGTH = 10;
        protected List<ScoreEntry> localScores = new List<ScoreEntry>(); //ScoreEntry[] localScores = new ScoreEntry[LENGTH];
        protected List<ScoreEntry> networkScores = new List<ScoreEntry>(); //ScoreEntry[] networkScores = new ScoreEntry[LENGTH];
        private const string FILE = "data/score.dat";
        private const char DELIM = ',';

        private bool networkUpdated = false;

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
                //localScores[i] = entry;
                localScores.Add(entry);
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
                // TODO print the scores out on the screen

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

                for (int i = 0; i < 10; i++)
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

                    /*data = new byte[1024];
                    receivedDataLength = server.Receive(data);
                    stringData = Encoding.ASCII.GetString(data, 0, receivedDataLength);
                    time = new DateTime(Int32.Parse(stringData));
                    Console.WriteLine(stringData);*/
                    
                    //networkScores[i] = new ScoreEntry(score, name, new DateTime());
                    networkScores.Add(new ScoreEntry(score, name, date));
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
            // TODO use me to write data to the network
            // remember to only attempt to write to the 
            // network if we have actually beaten
            // a network high score!

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
                // TODO load high scores from the network

                byte[] data;
                int receivedDataLength;
                string stringData;

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

                    server.Send(Encoding.ASCII.GetBytes("InsertInto101&BenPowell&10.26.11\r\n"));

                    server.Send(Encoding.ASCII.GetBytes("InsertInto" + listEntries.ElementAt(i).Score + "&" +
                           listEntries.ElementAt(i).Name + "&" + listEntries.ElementAt(i).Date + "&"));

                    /*data = new byte[1024];
                    receivedDataLength = server.Receive(data);
                    stringData = Encoding.ASCII.GetString(data, 0, receivedDataLength);*/

                    server.Shutdown(SocketShutdown.Both);
                    server.Close();
                }
            }

            ViewingNetwork = true;
        }

        public void TryInsert(int score)
        {
            string name = "dummy";
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
            SortScores(toDraw);
            for (int i = 0; i < LENGTH; i += 1)
            {
                toDraw[i].Draw(batch, font, pos + new Vector2(0, height * i), color);
            }
        }
    }
}
