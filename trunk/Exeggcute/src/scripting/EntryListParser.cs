using System.Collections.Generic;

namespace Exeggcute.src.scripting
{
    abstract class EntryListParser<TEntry>
    {
        public List<TEntry> Parse(string filepath)
        {
            List<string> lines = Util.ReadAndStrip(filepath, true);
            List<TEntry> result = new List<TEntry>();
            for (int i = 0; i < lines.Count; i += 1)
            {
                string flattened = Util.FlattenSpace(lines[i]);
                string[] tokens = flattened.Split(' ');
                Stack<string> stack = Util.Stackify<string>(tokens);
                TEntry parsed = parseEntry(stack);
                result.Add(parsed);
            }
            return result;
        }

        protected abstract TEntry parseEntry(Stack<string> element);

    }
}
