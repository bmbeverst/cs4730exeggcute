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

        public Data(string filename)
            : this(filename, '@')
        {

        }

        public Data(string filename, string text, char delim)
        {
            this.sections = new List<DataSection>();
            this.Filename = string.Format("{0}:subsection({1})", filename, delim);
            loadData(text, delim);
        }

        public Data(string filename, char delim)
        {
            this.sections = new List<DataSection>();
            this.Filename = filename;
            string text = File.ReadAllText(filename);
            loadData(text, delim); 
            
        }

        protected void loadData(string text, char delim)
        {
            RawText = text;
            List<string> split = RawText.Split(delim).ToList();
            for (int i = 0; i < split.Count; i += 1)
            {
                List<string> lines = Util.StripComments(split[i], true);

                if (lines.Count == 0)
                {
                    //Util.Warn("Empty section found {0}", text);
                    continue;
                }
                sections.Add(new DataSection(split[i], lines));
            }
        }
    }

    class DataSection
    {
        public string Tag { get; protected set; }
        public string TagValue { get; protected set; }
        public string RawText { get; protected set; }
        public List<string[]> Tokens { get; protected set; }
        public List<string> Lines { get; protected set; }

        public DataSection(string rawText, List<string> lines)
        {
            this.RawText = rawText;
            string[] tagTokens = lines[0].Split(':');
            this.Tag = tagTokens[0];
            if (tagTokens.Length >= 2)
            {
                this.TagValue = tagTokens[1];
            }
            else
            {
                this.TagValue = null;
            }
            lines.RemoveAt(0);
            this.Lines = lines;
            this.Tokens = MakeTokenList();
        }

        public List<string[]> MakeTokenList()
        {
            List<string[]> result = new List<string[]>();
            for (int i = 0; i < Lines.Count; i += 1)
            {
                result.Add(Util.CleanEntry(Lines[i]));
            }
            Tokens = result;
            return result;
        }
    }
}
