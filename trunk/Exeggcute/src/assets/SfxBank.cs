using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System.Text.RegularExpressions;
using Exeggcute.src.sound;

namespace Exeggcute.src.assets
{
    class SfxBank
    {
        private static Dictionary<string, float> durations =
            new Dictionary<string, float>();

        private static Dictionary<string, SoundEffect> soundBases = 
            new Dictionary<string, SoundEffect>();

        private static Bank<SoundEffectInstance> bank =
            new Bank<SoundEffectInstance>("ExeggcuteContent/sfx", "xnb");

        private static Dictionary<string, RepeatedSound> repeatedBank =
            new Dictionary<string, RepeatedSound>();
        public static void Play(string name)
        {
            bank[name].Play();
        }

        public static bool Contains(string name)
        {
            return bank.Contains(name);
        }

        public static SoundEffectInstance Get(string name)
        {
            return bank[name];
        }

        public static SoundEffect DeprecatedGetSound(string name)
        {
            return soundBases[name];
        }

        public static int GetDuration(string name)
        {
            return (int)(durations[name] + 0.5f);
        }

        public static void LoadAll(ContentManager content)
        {
            foreach (string filename in bank.AllFiles)
            {
                Load(content, filename);
            }
            
        }

        public static void Load(ContentManager content, string filename)
        {
            string name = bank.GetName(filename);
            // orz
            string relpath = Regex.Replace(filename, "ExeggcuteContent/", "");
            SoundEffect effect = content.Load<SoundEffect>(relpath);
            soundBases[name] = effect;
            durations[name] = (effect.Duration.Milliseconds / 1000f) * Engine.FPS;
            bank.PutWithFile(effect.CreateInstance(), filename);
        }

        public static RepeatedSound MakeRepeated(string name)
        {
            if (repeatedBank.ContainsKey(name))
            {
                return repeatedBank[name];
            }
            else
            {
                float volume = 1.0f;
                int duration = GetDuration(name);
                SoundEffect sound = DeprecatedGetSound(name);
                RepeatedSound repeated = new RepeatedSound(sound, duration, volume);
                repeatedBank[name] = repeated;
                return repeated;
            }
        }

        public static void UpdateAll()
        {
            foreach (var pair in repeatedBank)
            {
                pair.Value.Update();
            }
        }
    }
}

