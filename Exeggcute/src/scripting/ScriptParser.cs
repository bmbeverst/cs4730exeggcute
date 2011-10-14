﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Exeggcute.src.scripting
{
    abstract class ScriptParser<TElement, TName>
    {
        
        public char Delim { get; protected set; }
        public List<TElement> Load(TName name)
        {
            string filepath = getFilepath(name.ToString());
            List<string> lines = Util.StripComments(filepath, '#', true);
            lines.Reverse();
            Stack<string> lineStack = new Stack<string>(lines);
            int size = lineStack.Count;
            List<TElement> result = new List<TElement>();
            for (int i = 0; i < size; i += 1)
            {
                string line = lineStack.Pop();
                string[] tokens = line.Split(Delim);
                List<TElement> parsed;
                try
                {
                    parsed = parseElement(tokens);
                }
                catch (Exception error)
                {
                    throw new ParseError(error, line, filepath);
                }

                result.AddRange(parsed);
            }
            return result;
        }

        protected abstract List<TElement> parseElement(string[] tokens);
        protected abstract string getFilepath(string name);
    }
}
