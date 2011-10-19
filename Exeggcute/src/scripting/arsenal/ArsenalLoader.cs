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

            ModelName model = Util.ParseEnum<ModelName>(tokens.Pop());
            ScriptName spawnerMoveScript = Util.ParseEnum<ScriptName>(tokens.Pop());
            ScriptName spawnScript = Util.ParseEnum<ScriptName>(tokens.Pop());
            ScriptName shotMoveScript = Util.ParseEnum<ScriptName>(tokens.Pop());
            return new ArsenalEntry(model, spawnerMoveScript, spawnScript, shotMoveScript);
        }
    }
}
