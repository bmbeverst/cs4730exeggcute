using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting
{
    abstract class ListParser<TEntry>
    {
        public char Delim { get; protected set; }

        public List<TEntry> Load(string name)
        {
            string filepath = getFilepath(name);
            List<string> lines = Util.StripComments(filepath, true);

            List<TEntry> result = new List<TEntry>();

            foreach (string line in lines)
            {
                string flat = Util.FlattenSpace(line);
                TEntry element;
                Stack<string> tokens = Util.Tokenize(flat, Delim);
                try
                {
                    element = parseEntry(tokens);
                }
                catch (Exception error)
                {
                    throw new ParseError(error, flat, filepath);
                }
                result.Add(element);
            }

            return result;

        }

        protected abstract TEntry parseEntry(Stack<string> tokens);
        protected abstract string getFilepath(string name);
    }
}
