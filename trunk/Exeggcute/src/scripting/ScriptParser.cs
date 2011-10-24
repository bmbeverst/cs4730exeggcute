using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Exeggcute.src.scripting
{
    abstract class ScriptParser<TElement>
    {
        protected const char DELIM = ' ';

        public List<TElement> ParseLines(List<string> lines)
        {
            lines.Reverse();
            Stack<string> lineStack = new Stack<string>(lines);
            int size = lineStack.Count;
            List<TElement> result = new List<TElement>();
            for (int i = 0; i < size; i += 1)
            {
                string line = lineStack.Pop();
                string[] tokens = line.Split(DELIM);
                Stack<string> tokenStack = Util.Stackify<string>(tokens);
                List<TElement> parsed = parseElement(tokenStack);
                result.AddRange(parsed);
            }
            return result;
        }

        public virtual List<TElement> FromFile(string filepath)
        {
            List<string> lines = Util.ReadAndStrip(filepath, true);
            return ParseLines(lines);
        }

        protected abstract List<TElement> parseElement(Stack<string> tokens);
        protected virtual string getName(string filepath)
        {
            return Path.GetFileNameWithoutExtension(filepath);
        }
    }
}
