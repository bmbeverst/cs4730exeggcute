using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;
using System.Text.RegularExpressions;

namespace Exeggcute.src.loading
{
    class SpellcardLoader
    {
        public static Spellcard Load(List<string> lines, int start, out int end)
        {
            end = -1;
            int health = -1;
            int duration = -1;
            BehaviorScript behavior = null;
            Arsenal attack = null;
            ItemBatch items = null;
            string name = null;
            for (int i = start; i < lines.Count; i += 1)
            {
                string[] tokens = Util.CleanEntry(lines[i]);
                currentField = tokens[0];
                string value = tokens[1];

                if (matches("name"))
                {
                    name = value;
                }
                else if (matches("duration"))
                {
                    duration = int.Parse(value);
                }
                else if (matches("health"))
                {
                    health = int.Parse(value);
                }
                else if (matches("behavior"))
                {
                    behavior = ScriptBank.GetBehavior(value);
                }
                else if (matches("attack"))
                {
                    attack = ArsenalBank.Get(value, World.EnemyShots);
                }
                else if (matches("items"))
                {
                    items = ItemBatchBank.Get(value);
                }
                else if (matches("spellcard"))
                {
                    end = i;
                    break;
                }
            }
            if (end == -1)
            {
                end = lines.Count - 1;
            }
            if (behavior == null ||
                attack == null ||
                name == null ||
                items == null ||
                duration == -1 ||
                health == -1)
            {
                throw new ParseError("Not all fields were initialized");
            }
            return new Spellcard(behavior, attack, items, duration, health, name);


        }
        protected static string currentField;
        protected static bool matches(string regex)
        {
            return Regex.IsMatch(currentField, regex, RegexOptions.IgnoreCase);
        }
    }
}
