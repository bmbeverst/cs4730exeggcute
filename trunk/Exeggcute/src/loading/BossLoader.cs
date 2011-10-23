using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using Exeggcute.src.entities;
using System.Text.RegularExpressions;

namespace Exeggcute.src.loading
{
    class BossLoader
    {
        protected static string currentField;
        public static Boss Load(string filename)
        {
            List<Spellcard> spellcards = new List<Spellcard>();
            string name = null;
            Model model = null;
            BehaviorScript onDeath = null;
            string path = string.Format("data/bosses/{0}.boss", name);
            List<string> lines = Util.StripComments(path, true);
            for (int i = 0; i < lines.Count; i += 1)
            {
                string[] tokens = Util.CleanEntry(lines[i]);
                currentField = tokens[0];
                string value = tokens[1];
                if (matches("name"))
                {
                    name = value;
                }
                else if (matches("model"))
                {
                    model = ModelBank.Get(value);
                }
                else if (matches("deathscript"))
                {
                    onDeath = ScriptBank.GetBehavior(value);
                }
                else if (matches("spellcard"))
                {
                    int returnPoint;
                    Spellcard card = SpellcardLoader.Load(lines, i, out returnPoint);
                    spellcards.Add(card);
                }
            }

            if (name == null ||
                model == null ||
                onDeath == null ||
                spellcards.Count == 0)
            {
                throw new ParseError("All fields were not initialized!");
            }

            return new Boss(model, onDeath, spellcards);

        }
        protected static bool matches(string regex)
        {
            return Regex.IsMatch(currentField, regex, RegexOptions.IgnoreCase);
        }
 
    }
}
