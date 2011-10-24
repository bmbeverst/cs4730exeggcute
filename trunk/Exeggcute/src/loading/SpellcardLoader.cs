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
    class SpellcardInfo : LoadedInfo
    {
        public int? Health { get; protected set; }
        public int? Duration { get; protected set; }
        public BehaviorScript Behavior { get; protected set; }
        public Arsenal Attack { get; protected set; }
        public ItemBatch HeldItems { get; protected set; }
        public string Name { get; protected set; }

        protected SpellcardInfo()
        {

        }

        public SpellcardInfo(List<string> lines, int start, out int end)
        {
            end = -1;
            for (int i = start; i < lines.Count; i += 1)
            {
                string[] tokens = Util.CleanEntry(lines[i]);
                currentField = tokens[0];
                string value = tokens[1];

                if (matches("name"))
                {
                    Name = value;
                }
                else if (matches("duration"))
                {
                    Duration = int.Parse(value);
                }
                else if (matches("health"))
                {
                    Health = int.Parse(value);
                }
                else if (matches("behavior"))
                {
                    Behavior = ScriptBank.GetBehavior(value);
                }
                else if (matches("attack"))
                {
                    Attack = ArsenalBank.Get(value, World.EnemyShots);
                }
                else if (matches("items"))
                {
                    HeldItems = ItemBatchBank.Get(value);
                }
                else if (matches("spellcard"))
                {
                    end = i - 1;
                    break;
                }
                else
                {
                    throw new ParseError("Unhandled spellcard field \"{0}\"", currentField);
                }
            }
            if (end == -1)
            {
                end = lines.Count - 1;
            }
            LoadedInfo.AssertInitialized(this);
        }
    }
}
