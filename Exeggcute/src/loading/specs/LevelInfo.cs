using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;
using Exeggcute.src.entities;
using Exeggcute.src.scripting.roster;
using Exeggcute.src.assets;
using Exeggcute.src.scripting;

namespace Exeggcute.src.loading.specs
{
#pragma warning disable 0649
    class LevelInfo : Loadable
    {
        public Song BossTheme;
        public Song LevelTheme;
        public Boss MiniBoss;
        public Boss MainBoss;
        public Roster Roster;

        public LevelInfo(List<string[]> tokenList)
        {
            loadFromTokens(tokenList);
        }

        /*public LevelInfo(List<string[]> lines)
        {
            for (int i = 0; i < lines.Count; i += 1)
            {
                string[] tokens = lines[i];
                currentField = tokens[0];
                string value = tokens[1];
                if (matches("stagetheme"))
                {
                    LevelTheme = SongBank.Get(value);
                }
                else if (matches("bosstheme"))
                {
                    BossTheme = SongBank.Get(value);
                }
                else if (matches("miniboss"))
                {
                    BossInfo info = new BossInfo(value);
                    MiniBoss = info.MakeBoss();
                }
                else if (matches("mainboss"))
                {
                    BossInfo info = new BossInfo(value);
                    MainBoss = info.MakeBoss();
                }
                else if (matches("roster"))
                {
                    EnemyRoster = RosterBank.Get(value);
                }
                else
                {
                    throw new ParseError("Don't know what to do with field \"{0}\"", currentField);
                }

            }
            LoadedInfo.AssertInitialized(this);
        }*/
    }
}
