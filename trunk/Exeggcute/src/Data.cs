using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Exeggcute.src
{
    class Data
    {
        public string RawText { get; protected set; }
        public string Filename { get; protected set; }
        protected List<DataSection> sections;
        public DataSection this[int i]
        {
            get { return sections[i]; }
        }
        public int Count { get { return sections.Count; } }

        public Data(string filename, bool tokenize)
        {
            this.sections = new List<DataSection>();
            this.Filename = filename;
            this.RawText = File.ReadAllText(filename);

            List<string> split = RawText.Split('@').ToList();
            for (int i = 0; i < split.Count; i += 1)
            {
                int count;
                if (tokenize)
                {
                    List<string[]> tokens = Util.CleanData(split[i]);
                    count = tokens.Count;
                    if (count <= 1)
                    {
                        Util.Warn("Empty section found in {0}", filename);
                        continue;
                    }
                    sections.Add(new DataSection(tokens));
                }
                else
                {
                    List<string> lines = Util.StripComments(split[i], true);
                    count = lines.Count;
                    if (count <= 1)
                    {
                        Util.Warn("Empty section found in {0}", filename);
                        continue;
                    }
                    sections.Add(new DataSection(lines));
                }
                
                
                
            }
        }

    }

    class DataSection
    {
        public string Tag { get; protected set; }
        public List<string[]> Tokens { get; protected set; }
        public List<string> Lines { get; protected set; }

        public DataSection(List<string> lines)
        {
            this.Tag = lines[0].Split(':')[0];
            lines.RemoveAt(0);
            this.Lines = lines;
        }

        public DataSection(List<string[]> lines)
        {
            this.Tag = lines[0][0];
            lines.RemoveAt(0);
            this.Tokens = lines;
        }
    }
}
