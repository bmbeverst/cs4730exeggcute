using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting
{
    class ArsenalLoader
    {
        private static readonly char DELIM = ' ';
        public static List<Pair<ModelName, ScriptName>> Load(ArsenalName name)
        {
            string filepath = string.Format("data/arsenals/{0}.{1}", name, "arsenal");
            List<string> lines = Util.StripComments('#', filepath, true);
            List<Pair<ModelName, ScriptName>> result = new List<Pair<ModelName, ScriptName>>();

            foreach (string line in lines)
            {
                Pair<ModelName, ScriptName> pair;
                try
                {
                    pair = parseLine(line);
                }
                catch (Exception error)
                {
                    throw new ParseError("{0}\nFailed to parse line {1}", error.Message, line);
                }
                result.Add(pair);
            }
            return result;
        }

        public static Pair<ModelName, ScriptName> parseLine(string line)
        {
            string[] tokens = line.Split(DELIM);
            ModelName model = Util.ParseEnum<ModelName>(tokens[0]);
            ScriptName script = Util.ParseEnum<ScriptName>(tokens[1]);
            return new Pair<ModelName, ScriptName>(model, script);
        }
    }
}
