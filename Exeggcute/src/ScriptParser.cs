using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src
{
    abstract class ScriptParser<TElement, TName>
    {

        public abstract string getFilepath(string name);
        public char Delim { get; protected set; }
        public List<TElement> Load(TName name)
        {
            string filepath = getFilepath(name.ToString());
            List<string> lines = Util.StripComments('#', filepath, true);
            lines.Reverse();
            Stack<string> lineStack = new Stack<string>(lines);
            int size = lineStack.Count;
            List<TElement> result = new List<TElement>();
            for (int i = 0; i < size; i += 1)
            {
                string line = lineStack.Pop();
                string[] tokens = line.Split(Delim);
                try
                {
                    List<TElement> parsed = parseElement(tokens);
                    foreach (TElement item in parsed)
                    {
                        result.Add(item);
                    }
                }
                catch (Exception error)
                {
                    throw new ParseError("{0}\nFailed to parse line {1}", error.StackTrace, line);
                }
            }
            return result;
        }

        protected abstract List<TElement> parseElement(string[] tokens);
    }
}
