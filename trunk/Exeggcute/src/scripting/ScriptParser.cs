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
            return ParseLines(new Stack<string>(lines));
        }

        public List<TElement> ParseLines(Stack<string> lineStack)
        {
            List<List<TElement>> raw = GetRaw(lineStack);
            List<TElement> result = new List<TElement>();
            foreach(List<TElement> list in raw)
            {
                result.AddRange(list);
            }
            return result;
        }

        public List<List<TElement>> GetRaw(Stack<string> lineStack)
        {
            int size = lineStack.Count;
            List<List<TElement>> result = new List<List<TElement>>();
            for (int i = 0; i < size; i += 1)
            {
                string line = lineStack.Pop();
                string[] tokens = line.Split(DELIM);
                Stack<string> tokenStack = Util.Stackify<string>(tokens);
                List<TElement> parsed = parseElement(tokenStack);
                result.Add(parsed);
            }
            return result;
        }

        public virtual List<TElement> FromFile(string filepath)
        {
            Stack<string> lines = Util.StackifyFile(filepath);
            return ParseLines(lines);
        }

        public virtual List<List<TElement>> RawFromFile(string filepath)
        {
            Stack<string> lines = Util.StackifyFile(filepath);
            return GetRaw(lines);
        }

        protected abstract List<TElement> parseElement(Stack<string> tokens);
        protected virtual string getName(string filepath)
        {
            return Path.GetFileNameWithoutExtension(filepath);
        }
    }
}
