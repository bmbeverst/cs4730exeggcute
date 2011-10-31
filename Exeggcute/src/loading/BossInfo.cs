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
using Exeggcute.src.sound;

namespace Exeggcute.src.loading
{
#pragma warning disable 0649
    class BossInfo : Loadable
    {
        protected List<Spellcard> spellcards;
        protected string name;
        protected BodyInfo body;
        protected BehaviorScript entryScript;
        protected BehaviorScript defeatScript;
        protected BehaviorScript deathScript;
        protected Conversation intro;
        protected Conversation outro;
        protected RepeatedSound hurtSound;
        protected float? scale;

        public BossInfo(string name)
            : base(getFilename(name))
        {
            spellcards = new List<Spellcard>();
            string filename = getFilename(name);
            Data bossData = new Data(filename);
            DataSection info = bossData[0];
            if (!info.Tag.Equals("info", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ParseError("Boss info must come first");
            }
            loadFromTokens(bossData[0].Tokens, true);
            for (int i = 1; i < bossData.Count; i += 1)
            {
                List<string[]> tokenList = bossData[i].Tokens;

                SpellcardInfo scInfo = new SpellcardInfo(filename, bossData[i].Tokens);
                Spellcard next = scInfo.MakeSpellcard();
                spellcards.Add(next);
            }
            
        }

        public Boss MakeBoss()
        {
            return new Boss(name, 
                            body.Model, 
                            body.Texture,
                            body.Scale.Value, 
                            body.Radius.Value,
                            body.Rotation.Value,
                            intro, 
                            outro,
                            hurtSound,
                            entryScript, 
                            defeatScript, 
                            deathScript, 
                            spellcards);
        }

        private static string getFilename(string name)
        {
            return string.Format("data/bosses/{0}.boss", name);
        }
    }
}
