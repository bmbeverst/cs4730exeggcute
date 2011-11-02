using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using Exeggcute.src.scripting;

namespace Exeggcute.src.loading
{
    class BossLoader : LoadedInfo
    {
        public Boss Load(string filename)
        {
            Data bossData = new Data(filename);
            DataSection infoSection = bossData[0];
            if (!infoSection.Tag.Equals("info", StringComparison.CurrentCultureIgnoreCase))
            {
                throw new ParseError("Boss info must come first");
            }

            BossInfo info = new BossInfo(filename, bossData[0].Tokens);

            info.spellcards = new List<Spellcard>();
            for (int i = 1; i < bossData.Count; i += 1)
            {
                List<string[]> tokenList = bossData[i].Tokens;

                SpellcardInfo scInfo = new SpellcardInfo(filename, bossData[i].Tokens);
                Spellcard next = scInfo.MakeSpellcard();
                info.spellcards.Add(next);
            }

            return new Boss(info.name,
                            info.body.Model,
                            info.body.Texture,
                            info.body.Scale.Value,
                            info.body.Radius.Value,
                            info.body.Rotation.Value,
                            info.intro,
                            info.outro,
                            info.hurtSound,
                            info.entryScript,
                            info.defeatScript,
                            info.deathScript,
                            info.alignment.Value,
                            info.spellcards);
        }
    }
}
