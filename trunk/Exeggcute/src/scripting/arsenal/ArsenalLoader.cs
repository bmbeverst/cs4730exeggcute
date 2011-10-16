using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.assets;

namespace Exeggcute.src.scripting.arsenal
{
    class ArsenalLoader : ListParser<ArsenalName, ArsenalEntry>
    {
        public ArsenalLoader()
        {
            Delim = ' ';
        }

        protected override string getFilepath(ArsenalName name)
        {
            return string.Format("data/arsenals/{0}.arsenal", name);
        }

        protected override ArsenalEntry parseEntry(Stack<string> tokens)
        {
            string modelString = tokens.Pop();
            string scriptString = tokens.Pop();
            ModelName model = Util.ParseEnum<ModelName>(modelString);//tokens.Pop());
            ScriptName script = Util.ParseEnum<ScriptName>(scriptString);//tokens.Pop());
            return new ArsenalEntry(model, script);
        }
    }
}
