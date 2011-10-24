using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Exeggcute.src.scripting.arsenal;
using Exeggcute.src.scripting;
using Exeggcute.src.assets;

namespace Exeggcute.src.loading.specs
{

    class PlayerInfo : LoadedInfo
    {
        public Model Surface { get; protected set; }
        public Arsenal Bomb { get; protected set; }
        public BehaviorScript DeathScript { get; protected set; }
        public int? NumLives { get; protected set; }
        public int? NumBombs { get; protected set; } 
        public float? MoveSpeed { get; protected set; }
        public float? FocusSpeed { get; protected set; }
        public float? ModelScale { get; protected set; }
        public float? HitRadius { get; protected set; }
        protected PlayerInfo()
        {

        }

        public PlayerInfo(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i += 1)
            {
                string[] pair = Tokenize(lines[i]);
                currentField = pair[0];
                string value = pair[1];
                if (matches("model"))
                {
                    Surface = ModelBank.Get(value);
                }
                else if (matches("deathscript"))
                {
                    DeathScript = ScriptBank.GetBehavior(value);
                }
                else if (matches("special"))
                {
                    Bomb = ArsenalBank.Get(value, World.PlayerShots);
                }
                else if (matches("lives"))
                {
                    NumLives = int.Parse(value);
                }
                else if (matches("bombs"))
                {
                    NumBombs = int.Parse(value);
                }
                else if (matches("scale"))
                {
                    ModelScale = float.Parse(value);
                }
                else if (matches("movespeed"))
                {
                    MoveSpeed = float.Parse(value);
                }
                else if (matches("focusspeed"))
                {
                    FocusSpeed = float.Parse(value);
                }
                else if (matches("hitradius"))
                {
                    HitRadius = float.Parse(value);
                }
                else
                {
                    throw new ParseError("Token type not handled {0}", currentField);
                }
            }

            LoadedInfo.AssertInitialized(this);
        }

    }
}
