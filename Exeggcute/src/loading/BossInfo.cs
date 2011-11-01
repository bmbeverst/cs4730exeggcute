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
        public List<Spellcard> spellcards;
        public string name;
        public BodyInfo body;
        public BehaviorScript entryScript;
        public BehaviorScript defeatScript;
        public BehaviorScript deathScript;
        public Conversation intro;
        public Conversation outro;
        public RepeatedSound hurtSound;
        public float? scale;

        public BossInfo(string filename, List<string[]> tokens)
            : base(filename)
        {
            loadFromTokens(tokens, false);
        }
    }
}
