using System.Collections.Generic;
using Exeggcute.src.entities;
using Exeggcute.src.scripting.roster;
using Microsoft.Xna.Framework.Media;

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

        public LevelInfo(string filename, List<string[]> tokenList)
            : base(filename)
        {
            loadFromTokens(tokenList, true);
        }
    }
}
