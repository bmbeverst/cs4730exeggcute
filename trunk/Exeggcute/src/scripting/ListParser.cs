using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting
{
    abstract class ListParser<TName, TEntry>
    {
        public char Delim { get; protected set; }

        public List<TEntry> Load(TName name)
        {
            string filepath = getFilepath(name);
            List<string> lines = Util.StripComments(filepath, '#');

            List<TEntry> result = new List<TEntry>();

            foreach (string line in lines)
            {
                TEntry element;
                Stack<string> tokens = Util.Tokenize(line, Delim);
                try
                {
                    element = parseEntry(tokens);
                }
                catch (Exception error)
                {
                    throw new ParseError(error, line, filepath);
                }
                result.Add(element);
            }

            return result;

        }

        protected abstract TEntry parseEntry(Stack<string> tokens);
        protected abstract string getFilepath(TName name);
    }
}
