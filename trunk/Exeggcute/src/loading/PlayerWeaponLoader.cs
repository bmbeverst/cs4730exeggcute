using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.scripting;
using Exeggcute.src.assets;
using System.Reflection;

namespace Exeggcute.src.loading
{
    /// <summary>
    /// Loads a player weapon from a list of lines.
    /// Format:
    /// </summary>
    /*class PlayerWeaponLoader : Loader
    {
        public void Load(List<string> lines, List<int> thresholds, List<Arsenal> arsenals)
        {
            for (int i = 0; i < lines.Count; i += 1)
            {
                string cleaned = Util.FlattenSpace(lines[i]);
                string[] pair = cleaned.Split(' ');
                int next = int.Parse(pair[0]);
                string arseName = pair[1];
                Arsenal nextArsenal = ArsenalBank.Get(arseName, World.PlayerShots);

                thresholds.Add(next);
                arsenals.Add(nextArsenal);

            }
            if (thresholds.Count == 0 || (thresholds.Count != arsenals.Count))
            {
                throw new ParseError("Player's weapon must have at least one arsenal, and arsenal and threshold must be the same size");
            }
        }
    }*/
}
