using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using Exeggcute.src.entities;
using System.Text.RegularExpressions;
using Exeggcute.src.text;

namespace Exeggcute.src.loading
{
    class BossLoader : Loader
    {
        public Boss Load(string name)
        {

            List<Spellcard> spellcards = new List<Spellcard>();
            string bossName = null;
            Model model = null;
            BehaviorScript entryScript = null;
            BehaviorScript defeatScript = null;
            BehaviorScript deathScript = null;
            Conversation intro = null;
            Conversation outro = null;

            string path = string.Format("data/bosses/{0}.boss", name);
            List<string> lines = Util.ReadAndStrip(path, true);
            for (int i = 0; i < lines.Count; i += 1)
            {
                string[] tokens = Util.CleanEntry(lines[i]);
                currentField = tokens[0];
                string value = tokens[1];
                if (matches("name"))
                {
                    bossName = value;
                }
                else if (matches("model"))
                {
                    model = ModelBank.Get(value);
                }
                else if (matches("entryScript"))
                {
                    entryScript = ScriptBank.GetBehavior(value);
                }
                else if (matches("defeatscript"))
                {
                    defeatScript = ScriptBank.GetBehavior(value);
                }
                else if (matches("deathscript"))
                {
                    deathScript = ScriptBank.GetBehavior(value);
                }
                else if (matches("intro"))
                {
                    intro = ConversationBank.Get(value);
                }
                else if (matches("outro"))
                {
                    outro = ConversationBank.Get(value);
                }
                else if (matches("spellcard"))
                {
                    int returnPoint;
                    SpellcardInfo scInfo = new SpellcardInfo(lines, i + 1, out returnPoint);
                    Spellcard card = new Spellcard(scInfo.Behavior,
                                                   scInfo.Attack,
                                                   scInfo.HeldItems,
                                                   scInfo.Duration.Value,
                                                   scInfo.Health.Value,
                                                   scInfo.Name);
                    spellcards.Add(card);
                    Console.WriteLine("return to {0}", returnPoint);
                    i = returnPoint;
                }
                else
                {
                    throw new ParseError("Unhandled type \"{0}\"", currentField);
                }
            }

            if (bossName == null ||
                model == null ||
                entryScript == null ||
                defeatScript == null ||
                deathScript == null ||
                intro == null ||
                outro == null ||
                spellcards.Count == 0)
            {
                throw new ParseError("All fields were not initialized!");
            }

            return new Boss(model, intro, outro, entryScript, defeatScript, deathScript, spellcards);

        }
 
    }
}
