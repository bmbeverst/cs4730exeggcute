using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting
{
    abstract class ScriptParser<TElement>
    {
        protected const char DELIM = ' ';

        public List<TElement> ParseLines(string filename, List<string> lines)
        {
            lines.Reverse();
            return ParseLines(filename, new Stack<string>(lines));
        }

        public List<List<TElement>> RawFromLines(string filename, List<string> lines)
        {
            lines.Reverse();
            return GetRaw(filename, new Stack<string>(lines));
        }

        public List<TElement> ParseLines(string filename, Stack<string> lineStack)
        {
            List<List<TElement>> raw = GetRaw(filename, lineStack);
            List<TElement> result = new List<TElement>();
            foreach(List<TElement> list in raw)
            {
                result.AddRange(list);
            }
            return result;
        }

        public List<List<TElement>> GetRaw(string filename, Stack<string> lineStack)
        {
            List<string> failures = new List<string>();

            int size = lineStack.Count;
            List<List<TElement>> result = new List<List<TElement>>();
            for (int i = 0; i < size; i += 1)
            {
                string line = lineStack.Pop();
                string[] tokens = line.Split(DELIM);
                Stack<string> tokenStack = Util.Stackify<string>(tokens);
                try
                {
                    List<TElement> parsed = parseElement(tokenStack);
                    result.Add(parsed);
                }
                catch (FormatException format)
                {
                    failures.Add(string.Format("Improper data format for line \n    \"{0}\"", line));
                }
                catch (Exception e)
                {
                    failures.Add(e.Message);
                }
                
            }

            handleFailures(filename, failures);


            return result;
        }

        protected void handleFailures(string filename, List<string> failures)
        {
            if (failures.Count == 0) return;
            string message = Util.Join(failures, '\n');
            AssetManager.LogFailure("Failed to parse script from file {0}:\n{1}", filename, message);
        }

        public virtual List<TElement> FromFile(string filepath)
        {
            Stack<string> lines = Util.StackifyFile(filepath);
            return ParseLines(filepath, lines);
        }

        public virtual List<List<TElement>> RawFromFile(string filepath)
        {
            Stack<string> lines = Util.StackifyFile(filepath);
            return GetRaw(filepath, lines);
        }

        protected abstract List<TElement> parseElement(Stack<string> tokens);
        protected virtual string getName(string filepath)
        {
            return Path.GetFileNameWithoutExtension(filepath);
        }
    }
}
