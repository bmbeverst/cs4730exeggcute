﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting;
using Exeggcute.src.scripting.arsenal;

namespace Exeggcute.src.loading
{
#pragma warning disable 0649
    class SpellcardInfo : Loadable
    {
        protected int? health;
        protected int? duration;
        protected BehaviorScript behavior;
        protected Arsenal attack;
        protected ItemBatch items;
        protected string name;

        public SpellcardInfo(string filename, List<string[]> tokens)
            : base(filename)
        {
            loadFromTokens(tokens, true);
        }

        public Spellcard MakeSpellcard()
        {
            return new Spellcard(behavior, 
                                 attack, 
                                 items, 
                                 duration.Value, 
                                 health.Value, 
                                 name);
        }
    }
}
