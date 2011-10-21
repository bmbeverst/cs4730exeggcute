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
        private static Bank<SoundEffect> bank =
            new Bank<SoundEffect>("ExeggcuteContent/sfx", "xnb");

        public static void Play(string name)
        {
            bank[name].Play();
        }

        public static SoundEffect Get(string name)
        {
            return bank[name];
        }

        public static void LoadAll(ContentManager content)
        {
            bank.LoadAll(content);
        }

        public static void Load(ContentManager content, string name)
        {
            bank.Load(content, name);
        }
    }
}
