using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

namespace Exeggcute.src.assets
{
    class SoundBank
    {
        private static Bank<SoundName, SoundEffect> bank = 
            new Bank<SoundName, SoundEffect>("sfx");
        public static List<SoundName> AllNames = bank.AllNames;

        public static void Play(SoundName name)
        {
            bank[name].Play();
        }

        public static SoundEffect Get(SoundName name)
        {
            return bank[name];
        }

        public static void LoadAll(ContentManager content)
        {
            bank.LoadAll(content);
        }

        public static void Load(ContentManager content, SoundName name)
        {
            bank.Load(content, name);
        }
    }
}
