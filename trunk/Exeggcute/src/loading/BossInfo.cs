﻿using System;
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
#pragma warning disable 0649
    class BossInfo : Loadable
    {
        protected List<Spellcard> spellcards;
        protected string name;
        protected Model model;
        protected Texture2D texture;
        protected BehaviorScript entryScript;
        protected BehaviorScript defeatScript;
        protected BehaviorScript deathScript;
        protected Conversation intro;
        protected Conversation outro;
        protected float? scale;

        public BossInfo(string name)
        {
            spellcards = new List<Spellcard>();
            string filename = string.Format("data/bosses/{0}.boss", name);
            Data bossData = new Data(filename, true);
            DataSection info = bossData[0];
            if (!info.Tag.Equals("info", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ParseError("Boss info must come first");
            }
            loadFromTokens(bossData[0].Tokens);
            for (int i = 1; i < bossData.Count; i += 1)
            {
                List<string[]> tokenList = bossData[i].Tokens;

                SpellcardInfo scInfo = new SpellcardInfo(bossData[i].Tokens);
                Spellcard next = scInfo.MakeSpellcard();
                spellcards.Add(next);
            }
            
        }

        public Boss MakeBoss()
        {
            return new Boss(name, 
                            model, 
                            texture,
                            scale.Value, 
                            intro, 
                            outro, 
                            entryScript, 
                            defeatScript, 
                            deathScript, 
                            spellcards);
        }
    }
}
