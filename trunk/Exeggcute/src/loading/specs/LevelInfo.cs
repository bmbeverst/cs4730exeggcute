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
    class LevelInfo : LoadedInfo
    {
        public Song BossTheme { get; protected set; }
        public Song LevelTheme { get; protected set; }
        public Boss MiniBoss { get; protected set; }
        public Boss MainBoss { get; protected set; }
        public Roster EnemyRoster { get; protected set; }

        protected LevelInfo()
        {

        }

        public LevelInfo(List<string[]> lines)
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
                    
                    MiniBoss = BossInfo.Make(value);
                }
                else if (matches("mainboss"))
                {
                    MainBoss = BossInfo.Make(value);
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
        }
    }
}
