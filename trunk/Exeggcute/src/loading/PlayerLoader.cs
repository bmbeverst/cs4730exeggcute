using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Exeggcute.src.entities;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.assets;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.scripting;

namespace Exeggcute.src.loading
{
    class PlayerLoader
    {
        const char fieldDelim = ':';
        protected static string currentField;
        public static Player Load(string name)
        {
            string fullpath = string.Format("data/players/{0}.player", name);
            List<string> names = Util.StripComments(fullpath, true);
            Model model = null;
            BehaviorScript deathScript = null;
            Arsenal weapon = null;
            Arsenal bomb = null;
            for (int i = 0; i < names.Count; i += 1)
            {
                string token = Util.RemoveSpace(names[i]);
                string[] pair = token.Split(fieldDelim);
                currentField = pair[0];
                string value = pair[1];
                if (matches("model"))
                {
                    model = ModelBank.Get(value);
                }
                else if (matches("deathscript"))
                {
                    deathScript = ScriptBank.GetBehavior(value);
                }
                else if (matches("weapon"))
                {
                    weapon = ArsenalBank.Get(value, World.PlayerShots);

                }
                else if (matches("bomb"))
                {
                    bomb = ArsenalBank.Get(value, World.PlayerShots);
                }
            }
            if (bomb == null || model == null || deathScript == null || weapon == null)
            {
                throw new ParseError("Must specify all fields");
            }
            return new Player(model, deathScript, weapon, bomb, World.PlayerShots, World.GibList);
        }
        protected static bool matches(string regex)
        {
            return Regex.IsMatch(currentField, regex, RegexOptions.IgnoreCase);
        }
    }
}
