using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Exeggcute.src;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting;
using Exeggcute.src.entities.items;
using Exeggcute.src.assets;

namespace Exeggcute.src.loading
{

    class ItemLoader : EntryListParser<ItemEntry>
    {
        public Item Make(string filepath)
        {
            List<ItemEntry> entries = Parse(filepath);
            if (entries.Count != 1)
            {
                throw new ExeggcuteError("items should only contain one entry, found {0}", entries.Count);
            }
            return new Item(entries[0]);
        }

        protected override ItemEntry parseEntry(Stack<string> tokens)
        {
            string modelname = tokens.Pop();
            string behaviorname = tokens.Pop();
            string itemtypename = tokens.Pop();
            Model model = ModelBank.Get(modelname);
            BehaviorScript behavior = ScriptBank.GetBehavior(behaviorname);
            ItemType type = Util.ParseEnum<ItemType>(itemtypename);
            return new ItemEntry(model, behavior, type);
        }
    }
}
