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

namespace Exeggcute.src.loading
{
    class BossInfo : LoadedInfo
    {
        public List<Spellcard> Spellcards { get; protected set; }
        public string BossName { get; protected set; }
        public Model Surface { get; protected set; }
        public BehaviorScript EntryScript { get; protected set; }
        public BehaviorScript DefeatScript { get; protected set; }
        public BehaviorScript DeathScript { get; protected set; }
        public Conversation Intro { get; protected set; }
        public Conversation Outro { get; protected set; }
        public float? ModelScale { get; protected set; }

        protected BossInfo()
        {

        }

        public Boss Load(string name)
        {
            BossInfo info = new BossInfo();

            string path = string.Format("data/bosses/{0}.boss", name);
            List<string> lines = Util.ReadAndStrip(path, true);
            for (int i = 0; i < lines.Count; i += 1)
            {
                string[] tokens = Util.CleanEntry(lines[i]);
                currentField = tokens[0];
                string value = tokens[1];
                if (matches("name"))
                {
                    info.BossName = value;
                }
                else if (matches("model"))
                {
                    info.Surface= ModelBank.Get(value);
                }
                else if (matches("scale"))
                {
                    info.ModelScale = int.Parse(value);
                }
                else if (matches("entryScript"))
                {
                    info.EntryScript = ScriptBank.GetBehavior(value);
                }
                else if (matches("defeatscript"))
                {
                    info.DefeatScript = ScriptBank.GetBehavior(value);
                }
                else if (matches("deathscript"))
                {
                    info.DeathScript = ScriptBank.GetBehavior(value);
                }
                else if (matches("intro"))
                {
                    info.Intro = ConversationBank.Get(value);
                }
                else if (matches("outro"))
                {
                    info.Outro = ConversationBank.Get(value);
                }
                else if (matches("spellcard"))
                {
                    int returnPoint;
                    //fixme combine these
                    SpellcardInfo scInfo = new SpellcardInfo(lines, i + 1, out returnPoint);
                    Spellcard card = new Spellcard(scInfo.Behavior,
                                                   scInfo.Attack,
                                                   scInfo.HeldItems,
                                                   scInfo.Duration.Value,
                                                   scInfo.Health.Value,
                                                   scInfo.Name);
                    info.Spellcards.Add(card);
                    i = returnPoint;
                }
                else
                {
                    throw new ParseError("Unhandled type \"{0}\"", currentField);
                }
            }

            AssertInitialized(info);
            if (Spellcards.Count == 0)
            {
                throw new ParseError("no spellcards found");
            }
            return new Boss(
                info.Surface,
                info.ModelScale.Value,
                info.Intro,
                info.Outro,
                info.EntryScript,
                info.DefeatScript,
                info.DeathScript,
                info.Spellcards);

        }
    }
    class BossLoader : Loader
    {
        public Boss Load(string name)
        {

            List<Spellcard> spellcards = new List<Spellcard>();
            string bossName = null;
            Model model = null;
            BehaviorScript entryScript = null;
            BehaviorScript defeatScript = null;
            BehaviorScript deathScript = null;
            Conversation intro = null;
            Conversation outro = null;
            float? modelScale = null;

            string path = string.Format("data/bosses/{0}.boss", name);
            List<string> lines = Util.ReadAndStrip(path, true);
            for (int i = 0; i < lines.Count; i += 1)
            {
                string[] tokens = Util.CleanEntry(lines[i]);
                currentField = tokens[0];
                string value = tokens[1];
                if (matches("name"))
                {
                    bossName = value;
                }
                else if (matches("model"))
                {
                    model = ModelBank.Get(value);
                }
                else if (matches("scale"))
                {
                    modelScale = int.Parse(value);
                }
                else if (matches("entryScript"))
                {
                    entryScript = ScriptBank.GetBehavior(value);
                }
                else if (matches("defeatscript"))
                {
                    defeatScript = ScriptBank.GetBehavior(value);
                }
                else if (matches("deathscript"))
                {
                    deathScript = ScriptBank.GetBehavior(value);
                }
                else if (matches("intro"))
                {
                    intro = ConversationBank.Get(value);
                }
                else if (matches("outro"))
                {
                    outro = ConversationBank.Get(value);
                }
                else if (matches("spellcard"))
                {
                    int returnPoint;
                    SpellcardInfo scInfo = new SpellcardInfo(lines, i + 1, out returnPoint);
                    Spellcard card = new Spellcard(scInfo.Behavior,
                                                   scInfo.Attack,
                                                   scInfo.HeldItems,
                                                   scInfo.Duration.Value,
                                                   scInfo.Health.Value,
                                                   scInfo.Name);
                    spellcards.Add(card);
                    i = returnPoint;
                }
                else
                {
                    throw new ParseError("Unhandled type \"{0}\"", currentField);
                }
            }

            if (bossName == null ||
                model == null ||
                entryScript == null ||
                defeatScript == null ||
                deathScript == null ||
                intro == null ||
                outro == null ||
                modelScale == null ||
                spellcards.Count == 0)
            {
                throw new ParseError("All fields were not initialized!");
            }

            return new Boss(model, modelScale.Value, intro, outro, entryScript, defeatScript, deathScript, spellcards);

        }
 
    }
}
